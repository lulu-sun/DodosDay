using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    private float minY = -100f;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= minY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("Player"))
        {
            CatchingGameSystem.Instance.IncrementScore();
            Destroy(gameObject);
        }
    }
}