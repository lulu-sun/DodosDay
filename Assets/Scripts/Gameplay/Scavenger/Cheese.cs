using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    [SerializeField] int cheeseId = -1;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name.Contains("Player"))
        {
            ScavengerGameSystem.Instance.ShowCheeseFound(cheeseId);
            gameObject.SetActive(false);
        }
    }
}
