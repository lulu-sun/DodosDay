using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChasingGameSystem : MonoBehaviour
{
    [SerializeField] PlayerController player;

    [SerializeField] NPCController chaser;

    [SerializeField] Camera chasingGameCamera;

    public float winningTime = 20f;

    public event Action OnStartGame;
    
    public event Action OnEndGame;

    public bool IsRunning { get; private set; }

    public static ChasingGameSystem Instance { get; private set; }

    private Vector2 collisionSize;

    private Vector3 playerStartingPosition;

    private Vector3 chaserStartingPosition;

    private float playerStartingMoveSpeed;

    private float chaserStartingMoveSpeed;

    private Character playerCharacter;

    private Character chaserCharacter;

    private float timerInSeconds = 0f;

    public float FinishTime { get; private set; }

    public float getFasterTimeInSeconds = 5f;

    private float nextTimeCheckPointInSeconds;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerStartingPosition = player.transform.position;
        chaserStartingPosition = chaser.transform.position;
        playerCharacter = player.GetComponent<Character>();
        chaserCharacter = chaser.GetComponent<Character>();
        playerStartingMoveSpeed = playerCharacter.moveSpeed;
        chaserStartingMoveSpeed = chaserCharacter.moveSpeed;

        OnStartGame += () =>
        {
            player.transform.position = playerStartingPosition;
            chaser.transform.position = chaserStartingPosition;
            playerCharacter.moveSpeed = playerStartingMoveSpeed;
            chaserCharacter.moveSpeed = chaserStartingMoveSpeed;
            timerInSeconds = 0f;
            nextTimeCheckPointInSeconds = getFasterTimeInSeconds;
            chasingGameCamera.gameObject.SetActive(true);
            IsRunning = true;
            StartCoroutine(ChasePlayer());
        };

        OnEndGame += () =>
        {
            chasingGameCamera.gameObject.SetActive(false);
            FinishTime = timerInSeconds;

            var state = CheckpointState.StartedButNotComplete;
            if (FinishTime >= winningTime)
            {
                state = CheckpointState.Complete;
            }

            GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.ChasingGame, state);
            IsRunning = false;
        };

        collisionSize = (player.GetComponent<BoxCollider2D>().size + chaser.GetComponent<BoxCollider2D>().size) / 2;
        collisionSize += new Vector2(0.05f, 0.05f);
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    public void HandleUpdate()
    {
        player.HandleUpdate();
        
        timerInSeconds += Time.deltaTime;
    }

    private void UpdateMoveSpeeds(float playerMultiplier, float chaserMultiplier)
    {
        playerCharacter.moveSpeed *= playerMultiplier;
        chaserCharacter.moveSpeed *= chaserMultiplier;
    }

    private IEnumerator ChasePlayer()
    {
        while (IsRunning)
        {
            Vector2 playerLoc = player.transform.position;
            Vector2 chaserLoc = chaser.transform.position;

            Vector2 differenceVec = playerLoc - chaserLoc;

            if (Mathf.Abs(differenceVec.x) <= collisionSize.x && Mathf.Abs(differenceVec.y) <= collisionSize.y)
            {
                Debug.Log("Caught you!");
                EndGame();
                yield break;
            }

            if (Mathf.Abs(differenceVec.y) <= 0.5f)
            {
                differenceVec.y = 0;
            }
            else
            {
                differenceVec.x = 0;
            }

            differenceVec.Normalize();

            yield return chaser.WalkEnumerator(differenceVec);

            if (timerInSeconds >= nextTimeCheckPointInSeconds)
            {
                nextTimeCheckPointInSeconds += getFasterTimeInSeconds;
                UpdateMoveSpeeds(1.2f, 1.3f);
            }
        }
    }
}
