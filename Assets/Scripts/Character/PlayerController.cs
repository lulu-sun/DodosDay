using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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