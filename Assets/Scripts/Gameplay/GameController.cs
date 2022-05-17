using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Play, Battle, Dialogue, Cutscene, Paused, CatchingGame, ChasingGame, TitleScreen }

public class GameController : MonoBehaviour
{
    public PlayerController playerController;

    [SerializeField] Camera worldCamera;

    Stack<GameState> gameStateStack;

    GameState currentGameState { get => gameStateStack.Peek(); }

    // dummy var for visibility of gamestate in unity inspector
    [SerializeField] GameState currentGameStateForUnityInspector;

    // Change to trigger different starting game state for debugging purposes
    // [SerializeField] GameState startingGameState = GameState.Play;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        Instance = this;
        gameStateStack = new Stack<GameState>();
    }

    public void Pause()
    {
        SwitchState(GameState.Paused);
    }

    public void Unpause()
    {
        UnswitchState();
    }

    private void ToggleMainCamera(bool on)
    {
        worldCamera.gameObject.SetActive(on);
    }
    
    private void Start()
    {
        


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

        BattleSystem.Instance.OnStartBattle += () =>
        {
            if (currentGameState != GameState.Battle)
            {
                SwitchState(GameState.Battle);
                ToggleMainCamera(false);
            }
        };


        BattleSystem.Instance.OnEndBattle += () =>
        {
            if (currentGameState == GameState.Battle)
            {
                UnswitchState();
                ToggleMainCamera(true);
            }
        };

        CatchingGameSystem.Instance.OnStartGame += () =>
        {
            if (currentGameState != GameState.CatchingGame)
            {
                SwitchState(GameState.CatchingGame);
                ToggleMainCamera(false);
            }
        };

        CatchingGameSystem.Instance.OnEndGame += () =>
        {
            if (currentGameState == GameState.CatchingGame)
            {
                UnswitchState();
                ToggleMainCamera(true);
            }
        };

        ChasingGameSystem.Instance.OnStartGame += () =>
        {
            if (currentGameState != GameState.ChasingGame)
            {
                SwitchState(GameState.ChasingGame);
                ToggleMainCamera(false);
            }
        };

        ChasingGameSystem.Instance.OnEndGame += () =>
        {
            if (currentGameState == GameState.ChasingGame)
            {
                UnswitchState();
                ToggleMainCamera(true);
            }
        };

        TitleScreen.Instance.OnShowTitle += () =>
        {
            if (currentGameState != GameState.TitleScreen)
            {
                SwitchState(GameState.TitleScreen);
                ToggleMainCamera(false);
            }
        };

        TitleScreen.Instance.OnLeaveTitle += () =>
        {
            if (currentGameState == GameState.TitleScreen)
            {
                UnswitchState();
                ToggleMainCamera(true);
            }
        };

        gameStateStack.Push(GameState.Play);

        if (CutsceneManager.Instance.currentScene.name == "Intro")
        {
            gameStateStack.Push(GameState.Cutscene);
            TitleScreen.Instance.ShowTitle();
        }

    }

    private void SwitchState(GameState newState)
    {
        gameStateStack.Push(newState);
    }

    private void UnswitchState()
    {
        gameStateStack.Pop();
    }

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
        else if (currentGameState == GameState.Battle)
        {
            BattleSystem.Instance.HandleUpdate();
        }
        else if (currentGameState == GameState.CatchingGame)
        {
            CatchingGameSystem.Instance.HandleUpdate();
        }
        else if (currentGameState == GameState.ChasingGame)
        {
            ChasingGameSystem.Instance.HandleUpdate();
        }
    }
}
