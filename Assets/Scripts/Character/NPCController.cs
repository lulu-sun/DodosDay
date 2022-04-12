using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    public string Name;
    [SerializeField] Dialogue dialogue;
    // [SerializeField] List<Vector2> movementPattern;
    // [SerializeField] float timeBetweenPattern;
    NPCState state;
    float idleTimer = 0f;
    // int currentPattern = 0;
    CharacterAnimator animator;

    public void Interact(Vector2 facingDirection)
    {
        if (state == NPCState.Idle)
        {
            var originalDirection = animator.Direction;

            Debug.Log($"{originalDirection}, {facingDirection}, {-facingDirection}");

            // Turn to face the player. 
            animator.FaceDirection(-facingDirection);

            state = NPCState.Dialogue;
            StartCoroutine(DialogueManager.Instance.ShowDialogue(dialogue, () => {
                idleTimer = 0f;
                state = NPCState.Idle;
                
                // Turn back to original orientation after talking.
                animator.FaceDirection(originalDirection);
            }));
        }
    }
    
    private void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        dialogue.Name = Name;
    }

    // private void Update()
    // {
    //     if (state == NPCState.Idle)
    //     {
    //         idleTimer += Time.deltaTime;

    //         if (idleTimer > timeBetweenPattern)
    //         {
    //             idleTimer = 0f;
    //             if (movementPattern.Count > 0)
    //             {
    //                 StartCoroutine(Walk());
    //             }
    //         }
    //     }
    //     character.HandleUpdate();
    // }

    // IEnumerator Walk()
    // {
    //     state = NPCState.Walking;
        
    //     yield return character.Move(movementPattern[currentPattern]);
    //     currentPattern = (currentPattern + 1) % movementPattern.Count;

    //     state = NPCState.Idle;
    // }  
}

public enum NPCState { Idle, Walking, Dialogue }