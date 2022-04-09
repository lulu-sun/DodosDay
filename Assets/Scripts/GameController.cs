using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialogue }

public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    // [SerializeField] BattleSystem battleSystem;

    GameState state;

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
                state = GameState.FreeRoam;
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
