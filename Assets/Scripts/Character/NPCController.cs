using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCController : MonoBehaviour, Interactable
{
    public string Name;
    [SerializeField] Dialogue dialogue;
    // [SerializeField] List<Vector2> movementPattern;
    // [SerializeField] float timeBetweenPattern;
    NPCState state;
    float idleTimer = 0f;
    // int currentPattern = 0;

    Character character;

    public void Interact(Vector2 facingDirection)
    {
        // Talk(facingDirection);
        Walk();
    }

    private void Talk(Vector2 facingDirection)
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
            }));
        }
    }

    private void Walk()
    {
        // state = NPCState.Walking;
        // StartCoroutine(character.Move(new Vector2(0, -4), () => {
        //     StartCoroutine(character.Move(new Vector2(4, 0), () => {
        //         StartCoroutine(character.Move(new Vector2(0, 4), () => {
        //             StartCoroutine(character.Move(new Vector2(-4, 0), () => {
        //                 character.FaceDirection(Vector2.down);
        //                 state = NPCState.Idle;
        //             }));    
        //         }));
        //     }));
        // }));
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