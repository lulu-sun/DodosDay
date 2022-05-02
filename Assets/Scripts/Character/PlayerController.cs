using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    BoxCollider2D boxCollider;

    Vector2 movement;

    Character character;

    public Character Character { get => character; }

    void Awake()
    {
        character = GetComponent<Character>();
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            Interact();
        }

        character.MoveOneFrame(movement);
        character.HandleUpdate();
        character.Animate(movement);
    }

    void Interact()
    {
        // Get the position of what I'm interacting with based on the direction I'm facing.
        // Need to scale the distance based on boxCollider size. (Longer in x direction, shorter for y)
        var interactPos = transform.position + new Vector3(
            character.Direction.x * boxCollider.size.x,
            character.Direction.y * boxCollider.size.y);

        Debug.DrawLine(transform.position, interactPos, Color.red, 0.5f);

        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, GameLayers.Instance.InteractableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(character.Direction);
        }
    }
}