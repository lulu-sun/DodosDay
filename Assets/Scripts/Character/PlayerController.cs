using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    BoxCollider2D boxCollider;
    Rigidbody2D rb;
    Vector2 movement;

    CharacterAnimator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<CharacterAnimator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement by prioritizing
        // horizontal movement.
        if (movement.x != 0) {
            movement.y = 0;
        }

        if (movement != Vector2.zero)
        {
            animator.Direction = new Vector2(Mathf.Clamp(movement.x, -1f, 1f), Mathf.Clamp(movement.y, -1f, 1f));
        }
        animator.IsMoving = movement != Vector2.zero;

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            Interact();
        }

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        animator.HandleUpdate();
    }

    void Interact()
    {
        // Get the position of what I'm interacting with based on the direction I'm facing.
        // Need to scale the distance based on boxCollider size. (Longer in x direction, shorter for y)
        var interactPos = transform.position + new Vector3(
            animator.Direction.x * boxCollider.size.x,
            animator.Direction.y * boxCollider.size.y);

        Debug.DrawLine(transform.position, interactPos, Color.red, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.Instance.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(animator.Direction);
        }
    }
}


// // Start is called before the first frame update
    // public float moveSpeed;

    // // public event Action OnEncountered;
    // private Vector2 input;

    // private Character character;

    // private void Awake()
    // {
    //     character = GetComponent<Character>();
    // }

    // // Update is called once per frame
    // public void HandleUpdate()
    // {
    //     if (!character.IsMoving)
    //     {
    //         input.x = Input.GetAxisRaw("Horizontal")/2;
    //         input.y = Input.GetAxisRaw("Vertical")/2;
            
    //         // remove diagonal movement
    //         if (input.x != 0) input.y = 0;

    //         if (input != Vector2.zero)
    //         {
    //             StartCoroutine(character.Move(input, CheckForEncounters));
    //         }
    //     }

    //     character.HandleUpdate();

    //     if (Input.GetKeyDown(KeyCode.Z))
    //         Interact();
        
    // }

    // void Interact()
    // {
    //     var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
    //     var interactPos = transform.position + facingDir;
    //     // Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);

    //     var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.i.InteractableLayer);
    //     if (collider != null)
    //     {
    //         collider.GetComponent<Interactable>()?.Interact();
    //     }
    // }

    // private void CheckForEncounters()
    // {
    //     if (Physics2D.OverlapCircle(transform.position, 0.2f, GameLayers.i.GrassLayer) != null)
    //     {
    //         if (UnityEngine.Random.Range(1,101) <= 10)
    //         {
    //             character.Animator.IsMoving = false;
    //             // OnEncountered();

    //             // Debug.Log("Encountered a wild pokemon");
    //         }
    //     }
    // }



    // public IEnumerator Move(Vector3 targetPos)
    // {
    //     isMoving = true;
        
    //     while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    //         yield return null;
    //     }

    //     transform.position = targetPos;
    //     isMoving = false;

    //     CheckForEncounters();
    // }
    
    // private bool IsWalkable(Vector3 targetPos)  
    // {
    //     if (Physics2D.OverlapCircle(targetPos, 0.1f, solidObjectsLayer | interactableLayer) != null)
    //     {
    //         return false;
    //     } 
    //     return true;
    // }