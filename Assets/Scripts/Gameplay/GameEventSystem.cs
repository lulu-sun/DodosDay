using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance { get; private set; }

    private Dictionary<NPCType, GameEventTrigger> npcGameTriggers;

    private Dictionary<string, GameEventTrigger> enterSceneGameTriggers;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        npcGameTriggers = new Dictionary<NPCType, GameEventTrigger>();
        enterSceneGameTriggers = new Dictionary<string, GameEventTrigger>();

        InitializeGameEvents();
    }

    public bool TryTriggerNPCGameEvent(NPCController npc, Vector2 facingDirection)
    {
        if (npcGameTriggers.ContainsKey(npc.npcType))
        {
            npcGameTriggers[npc.npcType].TriggerGameEvent(npc, facingDirection);
            return true;
        }

        return false;
    }

    public bool TryTriggerEnterSceneGameEvent(string sceneName)
    {
        if (enterSceneGameTriggers.ContainsKey(sceneName))
        {
            enterSceneGameTriggers[sceneName].TriggerGameEvent(null, Vector2.zero);
            return true;
        }

        return false;
    }

    private void InitializeGameEvents()
    {
        //// NPC Game events

        // Naomi
        AddNPCGameTrigger(NPCType.Naomi, new GameEvent[]
        {
            new GameEvent(
                () => GameCheckpoints.Instance.NotComplete(Checkpoint.ChasingGame),
                (n, f) => CutsceneManager.Instance.NaomiTryAgainDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.GameCompleted),
                (n, f) => CutsceneManager.Instance.NaomiChaseAgainDialogue(n, f)),


            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.ChasingGame),
                (n, f) => CutsceneManager.Instance.NaomiCompletedDialogue(n, f))
        });

        // Jane
        AddNPCGameTrigger(NPCType.Jane, new GameEvent[]
        {
            new GameEvent(
                () => GameCheckpoints.Instance.NeverStarted(Checkpoint.PokemonBattle),
                (n, f) => CutsceneManager.Instance.JaneFirstDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.StartedButNotComplete(Checkpoint.PokemonBattle),
                (n, f) => CutsceneManager.Instance.JaneBattleAgainDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.GameCompleted),
                (n, f) => CutsceneManager.Instance.JaneGameEndDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.PokemonBattle),
                (n, f) => CutsceneManager.Instance.JaneAfterBattleDialogue(n, f))
        });

        // JuanJuan

        AddNPCGameTrigger(NPCType.JuanJuan, new GameEvent[]
        {
            new GameEvent(
                () => GameCheckpoints.Instance.NeverStarted(Checkpoint.CatchingGame),
                (n, f) => CutsceneManager.Instance.JuanJuanFirstDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.StartedButNotComplete(Checkpoint.CatchingGame),
                (n, f) => CutsceneManager.Instance.JuanJuanTryAgainDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.GameCompleted),
                (n, f) => CutsceneManager.Instance.JuanJuanGameEndDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.CatchingGame),
                (n, f) => CutsceneManager.Instance.JuanJuanCompletedDialogue(n, f))
        });
        // Arcade
        AddNPCGameTrigger(NPCType.ArcadeMachine, new GameEvent[]
        {       
            new GameEvent(
                () => true,
                (n, f) => CutsceneManager.Instance.StartArcadeGame(n, f)),
        });

        // Rachel

        AddNPCGameTrigger(NPCType.Rachel, new GameEvent[]
        {
            new GameEvent(
                () => GameCheckpoints.Instance.NeverStarted(Checkpoint.CheeseGame),
                (n, f) => CutsceneManager.Instance.RachelFirstDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.StartedButNotComplete(Checkpoint.CheeseGame),
                (n, f) => CutsceneManager.Instance.RachelWaitingDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.GameCompleted),
                (n, f) => CutsceneManager.Instance.RachelEndGameDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.CheeseGame),
                (n, f) => CutsceneManager.Instance.RachelHasCheeseDialogue(n, f))
        });


        // Noelle

        AddNPCGameTrigger(NPCType.Noelle, new GameEvent[]
        {
            new GameEvent(
                () => GameCheckpoints.Instance.NeverStarted(Checkpoint.IceRinkGame),
                (n, f) => CutsceneManager.Instance.NoelleFirstDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.StartedButNotComplete(Checkpoint.IceRinkGame),
                (n, f) => CutsceneManager.Instance.NoelleWaitingDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.GameCompleted),
                (n, f) => CutsceneManager.Instance.NoelleEndGameDialogue(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.IceRinkGame),
                (n, f) => CutsceneManager.Instance.NoelleCompletedDialogue(n, f))
        });


        // Radio

        AddNPCGameTrigger(NPCType.Radio, new GameEvent[]
        {
            new GameEvent(
                () => GameCheckpoints.Instance.NotComplete(Checkpoint.RadioPlayingMusic),
                (n, f) => CutsceneManager.Instance.RadioStartMusic(n, f)),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.RadioPlayingMusic),
                (n, f) => CutsceneManager.Instance.RadioAlreadyPlayingMusic(n, f)),


        });

        //// Enter Scene Game events

        // Island_n
        AddEnterSceneGameTrigger("Island_n", new GameEvent[]
        {

            new GameEvent(
                () => GameCheckpoints.Instance.NeverStarted(Checkpoint.NaomiCutscene),
                (n, f) => CutsceneManager.Instance.RunNaomiCutscene()),

            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.NaomiCutscene),
                (n, f) => CutsceneManager.Instance.SpawnNaomi())
        });

        AddEnterSceneGameTrigger("Island_R", new GameEvent[]
        {
            new GameEvent(
                () => GameCheckpoints.Instance.Complete(Checkpoint.RadioPlayingMusic),
                (n, f) =>
                {
                    AudioManager.Instance.FadeMusic(0.3f, 0f);
                    AudioManager.Instance.PlayMainMusic();
                    GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.RadioPlayingMusic, CheckpointState.NeverStarted);
                })
        });

        AddEnterSceneGameTrigger("Last_Island", new GameEvent[]
        {
            new GameEvent(
                () => true,
                (n, f) => CutsceneManager.Instance.FinalIslandCutscene(n, f))
        });
    }

    private void AddEnterSceneGameTrigger(string sceneName, IEnumerable<GameEvent> gameEvents)
    {
        // Verify sceneName exists.
        if (!SceneMapper.Instance.SceneNameExists(sceneName))
        {
            throw new ArgumentException($"Scene {sceneName} cannot be found. Make sure there are no typos (case sensitive!) and the scene has been added in build settings.");
        }

        enterSceneGameTriggers[sceneName] = new GameEventTrigger(gameEvents);
    }

    private void AddNPCGameTrigger(NPCType npcType, IEnumerable<GameEvent> gameEvents)
    {
        npcGameTriggers[npcType] = new GameEventTrigger(gameEvents);
    }
}

// A single game event and its requirements for it to be triggered.
public class GameEvent
{
    // checks whether game event should be triggered
    private Func<bool> shouldEventTrigger;

    // runs the gameEvent
    private Action<NPCController, Vector2> gameEvent;

    public GameEvent(Func<bool> shouldEventTrigger, Action<NPCController, Vector2> gameEvent)
    {
        this.shouldEventTrigger = shouldEventTrigger;
        this.gameEvent = gameEvent;
    }

    public bool TryRunGameEvent(NPCController npc, Vector2 facingDirection)
    {
        if (this.shouldEventTrigger())
        {
            this.gameEvent(npc, facingDirection);
            return true;
        }

        return false;
    }
}

// Multiple possible game events triggered by the same action.
// Only one of the game events will occur depending on game state.
public class GameEventTrigger
{
    private List<GameEvent> gameEvents;

    public GameEventTrigger(IEnumerable<GameEvent> gameEvents)
    {
        this.gameEvents = new List<GameEvent>(gameEvents);
    }

    public void TriggerGameEvent(NPCController npc, Vector2 facingDirection)
    {
        foreach (GameEvent gameEvent in gameEvents)
        {
            if (gameEvent.TryRunGameEvent(npc, facingDirection))
            {
                break;
            }
        }
    }
}