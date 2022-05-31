using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CatchingGameSystem : MonoBehaviour
{
    // Needs to be the catching game player, not the main game player
    [SerializeField] PlayerController player;

    [SerializeField] GameObject fallingObjectPrefab;

    [SerializeField] GameObject canvas;

    [SerializeField] Text scoreText;

    [SerializeField] Camera catchingGameCamera;

    [SerializeField] Image[] hearts;

    [SerializeField] Sprite filledHeart;

    [SerializeField] Sprite emptyHeart;

    private int score;

    public const int startingLivesCount = 3;

    private int lives;

    public int winningScore = 20;

    // Distance from center of canvas to one end (with some included buffer space)
    private float xLength = 250f;

    public event Action OnStartGame;
    
    public event Action OnEndGame;

    public bool isRunning { get; private set; }

    public static CatchingGameSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnStartGame += () => 
        {
            score = 0;
            lives = startingLivesCount;
            SetScoreText();
            isRunning = true;
            foreach (Image heart in hearts)
            {
                heart.sprite = filledHeart;
            }
            catchingGameCamera.gameObject.SetActive(true);
            StartCoroutine(SpawnFallingObjects());
        };

        OnEndGame += () =>
        {
            isRunning = false;
            catchingGameCamera.gameObject.SetActive(false);
        };
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    private void SpawnFallingObject(float xLoc, bool isGood, float gravityScale = 1f)
    {
        GameObject obj = Instantiate(fallingObjectPrefab, transform.position + new Vector3(xLoc, 400, 0), Quaternion.identity);
        obj.GetComponent<FallingObject>().isGood = isGood;
        obj.transform.SetParent(canvas.transform, false);
        obj.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }

    public void IncrementScore()
    {
        score++;
        SetScoreText();
    }

    private void SetScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void DecreaseLife()
    {
        lives--;
        hearts[lives].sprite = emptyHeart;
    }

    private IEnumerator SpawnFallingObjects()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 0.5f));
            SpawnFallingObject(Random.Range(-xLength, xLength), Random.Range(0f, 3f) >= 1, GetRandomGravityScale());
        }
    }

    public void HandleUpdate()
    {
        player.HandleUpdate();

        if (lives <= 0)
        {
            AudioManager.Instance.PlayEndGameSfx();
            EndGame();
        }
        else if (score >= winningScore)
        {
            GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.CatchingGame, CheckpointState.Complete);
            AudioManager.Instance.PlaySuccessSfx();
            EndGame();
        }
    }

    private float GetRandomGravityScale()
    {
        return Random.Range(1f, Mathf.Pow(1.1f, score)) - 0.5f;
    }
}
