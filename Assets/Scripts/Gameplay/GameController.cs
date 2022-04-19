using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialogue, Paused }

public class GameController : MonoBehaviour
{
    public PlayerController playerController;

    GameState state;
    private GameState previousState;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Pause()
    {
        previousState = state;
        state = GameState.Paused;
    }

    public void Unpause()
    {
        state = previousState;
    }

    private void Start()
    {
        // playerController.OnEncountered += StartBattle;
        DialogueManager.Instance.OnShowDialogue += () =>
        {
            state = GameState.Dialogue;
        };

        DialogueManager.Instance.OnCloseDialogue += () =>
        {
            if (state == GameState.Dialogue)
            {
                state = GameState.FreeRoam;
            }
        };
    }

    // void StartBattle()
    // {
    //     state = GameState.Battle;
    // }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }

        else if (state == GameState.Dialogue)
        {
            DialogueManager.Instance.HandleUpdate();
        }
        // else if (state == GameState.Battle)
        // {
        //     battleSystem.HandleUpdate();
        // }
    }
}
