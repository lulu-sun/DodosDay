using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEventSystem : MonoBehaviour
{
    public static GameEventSystem Instance { get; private set; }

    private Dictionary<NPCType, GameEventTrigger> npcGameTriggers;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        npcGameTriggers = new Dictionary<NPCType, GameEventTrigger>();
    }

    public void TriggerNPCGameEvent(NPCType npcType, Vector2 facingDirection)
    {
        npcGameTriggers[npcType].TriggerGameEvent(facingDirection);
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