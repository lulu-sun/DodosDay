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
            gameCheckpointsState[checkpoint] = CheckpointState.NeverStarted;
        }
    }

    public CheckpointState GetState(Checkpoint checkpoint)
    {
        return gameCheckpointsState[checkpoint];
    }

    public void UpdateCheckpointState(Checkpoint checktpoint, CheckpointState checkpointState)
    {
        this.gameCheckpointsState[checktpoint] = checkpointState;
    }

    public bool NeverStarted(Checkpoint checkpoint)
    {
        return GetState(checkpoint) == CheckpointState.NeverStarted;
    }

    public bool StartedButNotComplete(Checkpoint checkpoint)
    {
        return GetState(checkpoint) == CheckpointState.StartedButNotComplete;
    }


    public bool NotComplete(Checkpoint checkpoint)
    {
        return !Complete(checkpoint);
    }

    public bool Complete(Checkpoint checkpoint)
    {
        return GetState(checkpoint) == CheckpointState.Complete;
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
    CheeseGame,
    
    // Hack to record cheese game properly
    CheeseGameMemoryRecorded,

    // Dialogues
    EnteredAllison,
    //Interactions
    RadioPlayingMusic,
    MeetingOllie,


    // Game State
    GameCompleted,
}

public enum CheckpointState
{
    NeverStarted,
    StartedButNotComplete,
    Complete
}