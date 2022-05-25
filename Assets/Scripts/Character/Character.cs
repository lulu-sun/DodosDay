using System.Collections;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    CharacterAnimator animator;

    public float moveSpeed = 5f;

    Rigidbody2D rb;

    bool isPlayer;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<CharacterAnimator>();
        rb = GetComponent<Rigidbody2D>();
        isPlayer = GetComponent<PlayerController>() != null;
    }

    public void Animate(Vector2 movement)
    {
        if (movement.x != 0 && movement.y != 0)
        {
            throw new ArgumentException($"moveVec={movement} must be in a straight line.");
        }

        if (movement != Vector2.zero)
        {
            animator.Direction = new Vector2(Mathf.Clamp(movement.x, -1f, 1f), Mathf.Clamp(movement.y, -1f, 1f));
        }
        animator.IsMoving = movement != Vector2.zero;
    }

    public Vector2 Direction { get => animator.Direction; private set => animator.Direction = value; }

    public void FaceDirection(Vector2 direction)
    {
        Direction = direction;
        animator.HandleUpdate();
    }

    public IEnumerator Move(Vector2 moveVec, Action onMoveOver=null)
    {
        if (moveVec.x != 0 && moveVec.y != 0)
        {
            throw new ArgumentException($"moveVec={moveVec} must be in a straight line.");
        }

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        var targetPosition = new Vector2(transform.position.x + moveVec.x, transform.position.y + moveVec.y);
        var moveDir = new Vector2(Mathf.Clamp(moveVec.x, -1f, 1f), Mathf.Clamp(moveVec.y, -1f, 1f));

        float closest = Mathf.Infinity;
        float dist;

        while ((dist = Vector2.Distance(targetPosition, new Vector2(transform.position.x, transform.position.y))) > Mathf.Epsilon)
        {
            // If we ever start getting further away then just stop.
            if (dist <= closest + Mathf.Epsilon)
            {
                closest = dist;
            }
            else
            {
                break;
            }

            // Debug.Log($"{transform.position}, {targetPosition}, {Vector2.Distance(targetPosition, new Vector2(transform.position.x, transform.position.y))}");

            MoveOneFrame(moveDir);
            Animate(moveDir);
            yield return null;
        }

        if (!isPlayer)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }    
        animator.IsMoving = false;

        onMoveOver?.Invoke();
    }

    public void HandleUpdate()
    {
        animator.HandleUpdate();
    }

    public void MoveOneFrame(Vector2 direction)
    {
        rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
    }
}
