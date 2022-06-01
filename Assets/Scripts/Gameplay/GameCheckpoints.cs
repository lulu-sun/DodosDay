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

        ///// TESTING FOR Final Cutscene!!! DONT FORGET TO REMOVE!!!!
        GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.AllMemoriesFound, CheckpointState.Complete);
        AudioManager.Instance.PlayMainMusic();
        /////
    }

    public CheckpointState GetState(Checkpoint checkpoint)
    {
        return gameCheckpointsState[checkpoint];
    }

    public void UpdateCheckpointState(Checkpoint checkpoint, CheckpointState checkpointState)
    {
        Debug.Log($"Update GameCheckpoint {checkpoint}: {gameCheckpointsState[checkpoint]} -> {checkpointState}");
        this.gameCheckpointsState[checkpoint] = checkpointState;
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
    IceRinkGameAndChickenWings,
    CheeseGame,
    
    // Memories
    CheeseGameMemoryRecorded,
    ChickenWingsMemoryRecorded,
    NaomiChasingMemoryRecorded,
    CatchingGameMemoryRecorded,
    PokemonBattleMemoryRecorded,

    // New Place
    EnteredAllison,
    EnteredIceRink,

    //Interactions
    RadioPlayingMusic,
    MeetingOllie,

    // Game State
    AllMemoriesFound,
}

public enum CheckpointState
{
    NeverStarted,
    StartedButNotComplete,
    Complete
}