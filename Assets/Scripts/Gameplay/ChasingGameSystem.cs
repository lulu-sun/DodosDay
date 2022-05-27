using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChasingGameSystem : MonoBehaviour
{
    [SerializeField] PlayerController player;

    [SerializeField] NPCController chaser;

    [SerializeField] Camera chasingGameCamera;

    public float winningTime = 30f;

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
                MemoriesSystem.Instance.MarkMemoryFound();
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
        Vector2 prevDiffVec = Vector2.zero;

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

            bool yDiff = Mathf.Abs(differenceVec.y) > 0.1f;
            bool xDiff = Mathf.Abs(differenceVec.x) > 0.1f;

            if (!yDiff)
            {
                differenceVec.y = 0;
            }
            else if (!xDiff)
            {
                differenceVec.x = 0;
            }
            else
            {
                bool xSameAsPrev = Mathf.Clamp(differenceVec.x, -1f, 1f) == prevDiffVec.x;
                bool ySameAsPrev = Mathf.Clamp(differenceVec.y, -1f, 1f) == prevDiffVec.y;

                // Can only change path if at least x or y is different.
                bool changePath = (!xSameAsPrev || !ySameAsPrev) && UnityEngine.Random.Range(0f, 100f) < 0.1f;

                // Keep the same path. Only possible if either x or y is the same.
                if (!changePath)
                {
                    // At least one of the diff directions have remained the same.
                    if (xSameAsPrev || ySameAsPrev)
                    {
                        differenceVec = prevDiffVec;
                    }
                    // Both diff directions have changed. Pick one randomly.
                    else
                    {
                        if (UnityEngine.Random.Range(0f, 100f) < 50f)
                        {
                            differenceVec.x = 0;
                        }
                        else
                        {
                            differenceVec.y = 0;
                        }
                    }
                }
                // Change path. 
                else
                {
                    // If at least one diff direction remained the same, pick the other one.
                    if (xSameAsPrev || ySameAsPrev)
                    {
                        if (xSameAsPrev)
                        {
                            differenceVec.x = 0;
                        }
                        else
                        {
                            differenceVec.y = 0;
                        }
                    }
                    // No directions are the same, so just pick one randomly.
                    else
                    {
                        if (UnityEngine.Random.Range(0f, 100f) < 50f)
                        {
                            differenceVec.x = 0;
                        }
                        else
                        {
                            differenceVec.y = 0;
                        }
                    }
                }
            }

            differenceVec.Normalize();

            chaser.character.MoveOneFrame(differenceVec);
            yield return null;
            prevDiffVec = differenceVec;

            if (timerInSeconds >= nextTimeCheckPointInSeconds)
            {
                nextTimeCheckPointInSeconds += getFasterTimeInSeconds;
                UpdateMoveSpeeds(1.2f, 1.3f);
            }
        }
    }
}
