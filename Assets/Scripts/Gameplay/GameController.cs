using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Play, Battle, Dialogue, Cutscene, Paused }

public class GameController : MonoBehaviour
{
    public PlayerController playerController;

    Stack<GameState> gameStateStack;

    GameState currentGameState { get => gameStateStack.Peek(); }

    // dummy var for visibility of gamestate in unity inspector
    [SerializeField] GameState currentGameStateForUnityInspector;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        gameStateStack = new Stack<GameState>();
        gameStateStack.Push(GameState.Play);
    }

    public void Pause()
    {
        SwitchState(GameState.Paused);
    }

    public void Unpause()
    {
        UnswitchState();
    }

    private void Start()
    {
        if (CutsceneManager.Instance.currentScene.name == "Intro")
        {
            gameStateStack.Push(GameState.Cutscene);
        }

        // playerController.OnEncountered += StartBattle;
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            if (currentGameState != GameState.Dialogue)
            {
                SwitchState(GameState.Dialogue);
            }
        };

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (currentGameState == GameState.Dialogue)
            {
                UnswitchState();
            }
        };

        CutsceneManager.Instance.OnStartCutscene += () => 
        {
            if (currentGameState != GameState.Cutscene)
            {
                SwitchState(GameState.Cutscene);
            }
        };

        CutsceneManager.Instance.OnEndCutscene += () =>
        {
            if (currentGameState == GameState.Cutscene)
            {
                UnswitchState();
            }
        };
    }

    private void SwitchState(GameState newState)
    {
        gameStateStack.Push(newState);
    }

    private void UnswitchState()
    {
        gameStateStack.Pop();
    }

    // void StartBattle()
    // {
    //     state = GameState.Battle;
    // }

    private void Update()
    {
        currentGameStateForUnityInspector = gameStateStack.Peek();

        if (currentGameState == GameState.Play)
        {
            playerController.HandleUpdate();
        }
        else if (currentGameState == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
        else if (currentGameState == GameState.Cutscene)
        {
            CutsceneManager.Instance.HandleUpdate();
        }
        // else if (state == GameState.Battle)
        // {
        //     battleSystem.HandleUpdate();
        // }
    }
}
