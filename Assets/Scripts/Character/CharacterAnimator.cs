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

    //Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }
    
    //States
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;


    SpriteAnimator idleDownAnim;
    SpriteAnimator idleUpAnim;
    SpriteAnimator idleRightAnim;
    SpriteAnimator idleLeftAnim;

    SpriteAnimator currentAnim;
    bool wasPreviouslyMoving;

    //References
    SpriteRenderer spriteRenderer;

    private void Start()
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


        currentAnim = idleDownAnim;
    }

    private void Update()
    {
        Debug.Log(MoveX);
        Debug.Log(MoveY);

        var prevAnim = currentAnim;

        if (IsMoving)
        {
            currentAnim.HandleUpdate();
        }
        else
        {
            if (MoveX > 0)
            {
                currentAnim = idleRightAnim;
            }
            
            else if (MoveX < 0)
            {
                currentAnim = idleLeftAnim;
            }

            else if (MoveY > 0)
            {
                currentAnim = idleUpAnim;
            }

            else if (MoveY < 0)
            {
                currentAnim = idleDownAnim;
            }

            currentAnim.HandleUpdate();

        }

        if (MoveX > 0)
            currentAnim = walkRightAnim;
        else if (MoveX < 0)
            currentAnim = walkLeftAnim;
        else if (MoveY > 0)
            currentAnim = walkUpAnim;
        else if (MoveY < 0)
            currentAnim = walkDownAnim;

        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();

            // spriteRenderer.sprite = currentAnim.Frames[0];

        wasPreviouslyMoving = IsMoving;
    }



}
