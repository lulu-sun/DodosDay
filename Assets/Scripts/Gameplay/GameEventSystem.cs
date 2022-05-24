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

    public bool TryTriggerNPCGameEvent(NPCType npcType, Vector2 facingDirection)
    {
        if (npcGameTriggers.ContainsKey(npcType))
        {
            npcGameTriggers[npcType].TriggerGameEvent(facingDirection);
            return true;
        }

        return false;
    }

    public bool TryTriggerEnterSceneGameEvent(string sceneName)
    {
        if (enterSceneGameTriggers.ContainsKey(sceneName))
        {
            enterSceneGameTriggers[sceneName].TriggerGameEvent(Vector2.zero);
            return true;
        }

        return false;
    }

    private void InitializeGameEvents()
    {
        //// NPC Game events

        // Naomi
        // Jane
        // JuanJuan
        // Rachel
        // Noelle
        // Arcade
        // Radio

        //// Enter Scene Game events

        // Island_n
        AddEnterSceneGameTrigger("Island_n", new GameEvent[]
        {
            new GameEvent(
                () => GameCheckpoints.Instance.NeverStarted(Checkpoint.NaomiCutscene),
                (_) => CutsceneManager.Instance.RunNaomiCutscene())
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
    private Action<Vector2> gameEvent;

    public GameEvent(Func<bool> shouldEventTrigger, Action<Vector2> gameEvent)
    {
        this.shouldEventTrigger = shouldEventTrigger;
        this.gameEvent = gameEvent;
    }

    public bool TryRunGameEvent(Vector2 facingDirection)
    {
        if (this.shouldEventTrigger())
        {
            this.gameEvent(facingDirection);
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

    public void TriggerGameEvent(Vector2 facingDirection)
    {
        foreach (GameEvent gameEvent in gameEvents)
        {
            if (gameEvent.TryRunGameEvent(facingDirection))
            {
                break;
            }
        }
    }
}