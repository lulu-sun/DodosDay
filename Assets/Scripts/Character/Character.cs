using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    // public float moveSpeed;
    // public bool IsMoving { get; private set; }
    // CharacterAnimator animator;
    // public BoxCollider2D collider;

    // private void Awake()
    // {
    //     animator = GetComponent<CharacterAnimator>();
    // }

    // public IEnumerator Move(Vector2 moveVec, Action OnMoveOver=null)
    // {
    //     animator.MoveX = Mathf.Clamp(moveVec.x, -1f, 1f);
    //     animator.MoveY = Mathf.Clamp(moveVec.y, -1f, 1f);

    //     var targetPos = transform.position;
    //     targetPos.x += moveVec.x;
    //     targetPos.y += moveVec.y;

    //     if (!IsWalkable(targetPos))
    //     {
    //         yield break;
    //     }

    //     IsMoving = true;

    //     while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
    //     {
    //         transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    //         yield return null;
    //     }
    //     transform.position = targetPos;

    //     IsMoving = false;

    //     OnMoveOver?.Invoke();
    // }

    // public void HandleUpdate()
    // {
    //     animator.IsMoving = IsMoving;
    // }

    // private bool IsWalkable(Vector3 targetPos)
    // {
    //     if (Physics2D.OverlapCircle(targetPos, 0.1f, GameLayers.i.SolidLayer | GameLayers.i.InteractableLayer) != null)
    //     {
    //         return false;
    //     } 
    //     return true;
    // }

    // public CharacterAnimator Animator {
    //     get => animator;
    // }
}
