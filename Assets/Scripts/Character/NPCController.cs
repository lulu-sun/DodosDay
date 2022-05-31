using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum NPCType
{ 
    Default,
    Lulu,
    Jane,
    Naomi,
    JuanJuan,
    Rachel,
    Noelle,
    Ollie,
    Dumpling,
    ArcadeMachine,
    Radio,
    SchoolSign,
    WingsSign,
    ChickenWingsShop
}

public class NPCController : MonoBehaviour, Interactable
{
    public string Name { get => npcType.ToString(); }

    public NPCType npcType;

    public Vector2 startingFacingDirection = Vector2.down;
    
    NPCState state;

    float idleTimer = 0f;

    public Character character { get; private set; }

    public void Interact(Vector2 facingDirection)
    {
        Debug.Log($"Interaction with {npcType}");
        GameEventSystem.Instance.TryTriggerNPCGameEvent(this, facingDirection);
    }

    public void Talk(Dialogue dialogue, Vector2 facingDirection, Action onFinished = null)
    {
        if (state == NPCState.Idle)
        {
            var originalDirection = character.Direction;

            // Turn to face the player. 
            character.FaceDirection(-facingDirection);

            state = NPCState.Dialogue;
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, () => {
                idleTimer = 0f;
                state = NPCState.Idle;

                // Turn back to original orientation after talking.
                character.FaceDirection(originalDirection);

                onFinished?.Invoke();
            }));
        }
    }

    public void Walk(Vector2 movement, Action onFinished = null)
    {
        StartCoroutine(character.Move(movement, onFinished));
    }

    public IEnumerator WalkEnumerator(Vector2 movement, Action onFinished = null)
    {
        yield return character.Move(movement, onFinished);
    }


    // Repeatedly walk in a well defined pattern. 
    // Not sure if we will use this. 
    private IEnumerator Walk(List<Vector2> movementPattern, float timeBetweenPattern = 0f)
    {
        if (movementPattern.Count == 0)
        {
            throw new ArgumentException($"movementPattern={movementPattern} cannot be empty.");
        }

        int currentPatternIndex = 0;

        if (state == NPCState.Idle)
        {
            while (currentPatternIndex < movementPattern.Count)
            {
                idleTimer += Time.deltaTime;
                if (idleTimer > timeBetweenPattern)
                {
                    idleTimer = 0f;
                    state = NPCState.Walking;
                    yield return character.Move(movementPattern[currentPatternIndex], () => {
                        state = NPCState.Idle;
                    });
                    currentPatternIndex++;
                }

                yield return null;
            }
        }
    }

    private void Update()
    {
        character.HandleUpdate();
    }

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void Start()
    {
        character.FaceDirection(startingFacingDirection);
    }
}

public enum NPCState { Idle, Walking, Dialogue }