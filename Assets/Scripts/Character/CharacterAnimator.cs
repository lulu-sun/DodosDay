using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    [SerializeField] List<Sprite> walkDownSprites;
    [SerializeField] List<Sprite> walkUpSprites;
    [SerializeField] List<Sprite> walkRightSprites;
    [SerializeField] List<Sprite> walkLeftSprites;


    [SerializeField] List<Sprite> idleDownSprites;
    [SerializeField] List<Sprite> idleUpSprites;
    [SerializeField] List<Sprite> idleRightSprites;
    [SerializeField] List<Sprite> idleLeftSprites;

    // Parameters
    public Vector2 Direction { get; set; }
    public bool IsMoving { get; set; }
    
    // States
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator idleDownAnim;
    SpriteAnimator idleUpAnim;
    SpriteAnimator idleRightAnim;
    SpriteAnimator idleLeftAnim;

    SpriteAnimator currentWalkAnim;
    SpriteAnimator currentIdleAnim;
    bool wasPreviouslyMoving;

    // References
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        walkDownAnim = new SpriteAnimator(walkDownSprites, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites, spriteRenderer);

        idleDownAnim = new SpriteAnimator(idleDownSprites, spriteRenderer);
        idleUpAnim = new SpriteAnimator(idleUpSprites, spriteRenderer);
        idleRightAnim = new SpriteAnimator(idleRightSprites, spriteRenderer);
        idleLeftAnim = new SpriteAnimator(idleLeftSprites, spriteRenderer);

        currentWalkAnim = walkDownAnim;
        currentIdleAnim = idleDownAnim;
        Direction = Vector2.down;
    }

    public void HandleUpdate()
    {
        var prevAnim = currentWalkAnim;

        FaceDirection(Direction);

        if (currentWalkAnim != prevAnim || IsMoving != wasPreviouslyMoving)
        {
            if (IsMoving)
            {
                currentWalkAnim.Start();
            }
            else
            {
                currentIdleAnim.Start();
            }
        }

        if (IsMoving)
        {
            currentWalkAnim.HandleUpdate();
        }
        else
        {
            currentIdleAnim.HandleUpdate();
        }

        wasPreviouslyMoving = IsMoving;
    }

    private void FaceDirection(Vector2 direction)
    {
        // Direction cannot be diagonal.
        if (direction.x != 0 && direction.y != 0)
        {
            throw new ArgumentException($"facing direction {direction} cannot be diagonal.");
        }

        if (direction.x > 0)
        {
            currentWalkAnim = walkRightAnim;
            currentIdleAnim = idleRightAnim;
        }
        else if (direction.x < 0)
        {
            currentWalkAnim = walkLeftAnim;
            currentIdleAnim = idleLeftAnim;
        }
        else if (direction.y > 0)
        {
            currentWalkAnim = walkUpAnim;
            currentIdleAnim = idleUpAnim;
        }
        else if (direction.y < 0)
        {
            currentWalkAnim = walkDownAnim;
            currentIdleAnim = idleDownAnim;
        }
    }
}
