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

    private int score = 0;

    public int winningScore = 20;

    // Distance from center of canvas to one end (with some included buffer space)
    private float xLength = 250f;

    public event Action OnStartGame;
    
    public event Action OnEndGame;

    private bool isRunning;

    public static CatchingGameSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OnStartGame += () => 
        {
            isRunning = true;
            catchingGameCamera.gameObject.SetActive(true);
            StartCoroutine(SpawnFallingObjects());
        };

        OnEndGame += () =>
        {
            isRunning = false;
            catchingGameCamera.gameObject.SetActive(false);
        };

        // OnStartGame?.Invoke();
    }

    public void StartGame()
    {
        OnStartGame?.Invoke();
    }

    public void EndGame()
    {
        OnEndGame?.Invoke();
    }

    private void SpawnFallingObject(float xLoc, float gravityScale = 1f)
    {
        var obj = Instantiate(fallingObjectPrefab, transform.position + new Vector3(xLoc, 400, 0), Quaternion.identity);
        obj.transform.SetParent(canvas.transform, false);
        obj.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }

    public void IncrementScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    private IEnumerator SpawnFallingObjects()
    {
        while (isRunning)
        {
            yield return new WaitForSeconds(1f);
            SpawnFallingObject(Random.Range(-xLength, xLength), Random.Range(1f, Mathf.Pow(1.1f, score)) - 0.5f);
        }
    }

    public void HandleUpdate()
    {
        player.HandleUpdate();
    }
}
