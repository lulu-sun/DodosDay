using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallingObject : MonoBehaviour
{
    private float minY = -100f;

    [SerializeField] Sprite goodChimkenSprite;
    [SerializeField] Sprite badChimkenSprite;

    [SerializeField] Image spriteImage;

    public bool isGood { get; set; }

    private void Start()
    {
        if (isGood)
        {
            spriteImage.sprite = goodChimkenSprite;
        }
        else
        {
            spriteImage.sprite = badChimkenSprite;
        }
    }

    private void Update()
    {
        if (!CatchingGameSystem.Instance.isRunning)
        {
            Destroy(gameObject);
        }

        if (transform.position.y <= minY)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("Player"))
        {
            if (isGood)
            {
                AudioManager.Instance.PlayPopSfx();
                CatchingGameSystem.Instance.IncrementScore();
            }
            else
            {
                AudioManager.Instance.PlayLosePointSfx();
                CatchingGameSystem.Instance.DecreaseLife();
            }
            Destroy(gameObject);
        }
    }
}
