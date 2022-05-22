using System;
using System.Collections.Generic;
using UnityEngine;

public class GameCheckpoints : MonoBehaviour
{
    public static GameCheckpoints Instance { get; private set; }

    private Dictionary<Checkpoint, CheckpointState> gameCheckpointsState;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameCheckpointsState = new Dictionary<Checkpoint, CheckpointState>();
        foreach (Checkpoint checkpoint in Enum.GetValues(typeof(Checkpoint)))
        {
            gameCheckpointsState[checkpoint] = CheckpointState.NotStarted;
        }
    }

    public CheckpointState GetCheckpointState(Checkpoint checkpoint)
    {
        return gameCheckpointsState[checkpoint];
    }

    public void UpdateCheckpointState(Checkpoint checktpoint, CheckpointState checkpointState)
    {
        this.gameCheckpointsState[checktpoint] = checkpointState;
    }

    public void LogCurrentStates()
    {
        string statesString = "GameCheckpoints:\n";
        foreach (var pair in gameCheckpointsState)
        {
            Checkpoint checkpoint = pair.Key;
            CheckpointState checkpointState = pair.Value;

            statesString += $"{checkpoint}: {checkpointState}\n";
        }

        Debug.Log(statesString);
    }
}

// to enable tracking a new game checkpoint, just add it to this enum:
public enum Checkpoint
{
    // Cutscenes
    IntroCutscene,
    NaomiCutscene,

    // Minigames
    ChasingGame,
    CatchingGame,
    PokemonBattle,
    IceRinkGame,
    CheeseGame
}

public enum CheckpointState
{
    NotStarted,
    NotCompleted,
    Complete
}