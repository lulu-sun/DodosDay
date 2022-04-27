using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCController : MonoBehaviour, Interactable
{
    public string Name;
    [SerializeField] Dialogue dialogue;
    
    NPCState state;
    // int currentPattern = 0;

    float idleTimer = 0f;

    public Character character { get; private set; }

    public void Interact(Vector2 facingDirection)
    {
        Talk(facingDirection, () => Walk(new Vector2(3, 0)));
        // StartCoroutine(Walk(new List<Vector2>() 
        // { 
        //     new Vector2(4, 0),
        //     new Vector2(0, -4),
        //     new Vector2(-4, 0),
        //     new Vector2(0, 4)
        // }));
    }

    private void Talk(Vector2 facingDirection, Action onFinished = null)
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

    private void Walk(Vector2 movement, Action onFinished = null)
    {
        StartCoroutine(character.Move(movement, onFinished));
    }

    // Repeatedly walk in a well defined pattern. 
    // Not sure if we will use this. 
    private IEnumerator Walk(List<Vector2> movementPattern, float timeBetweenPattern = 0f)
    {
        Debug.Log("walk");

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
        if (DialogueManager.Instance.IsShowing)
        {
            return;
        }

        character.HandleUpdate();
    }
    
    private void Awake()
    {
        character = GetComponent<Character>();
        dialogue.Name = Name;
    }
}

public enum NPCState { Idle, Walking, Dialogue }