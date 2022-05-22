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
        for ()
    }

    public CheckpointState GetCheckpointState(Checkpoint checkpoint)
    {
        return gameCheckpointsState[checkpoint];
    }

    public void UpdateCheckpointState(Checkpoint checktpoint, CheckpointState checkpointState)
    {
        this.gameCheckpointsState[checktpoint] = checkpointState;
    }
}

// to enable tracking a new game checkpoint, add it to this enum:
public enum Checkpoint
{
    IntroCutscene,
    NaomiCutscene,
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