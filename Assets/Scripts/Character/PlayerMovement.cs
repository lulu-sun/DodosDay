using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    public Rigidbody2D rb;

    Vector2 movement;

    CharacterAnimator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<CharacterAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Prevent diagonal movement by prioritizing
        // horizontal movement.
        if (movement.x != 0) {
            movement.y = 0;
        }

        animator.MoveX = Mathf.Clamp(movement.x, -1f, 1f);
        animator.MoveY = Mathf.Clamp(movement.y, -1f, 1f);
        animator.IsMoving = movement != Vector2.zero;
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
