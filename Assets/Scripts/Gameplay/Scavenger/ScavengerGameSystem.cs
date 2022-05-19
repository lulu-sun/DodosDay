using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ScavengerGameSystem : MonoBehaviour
{
    [SerializeField] Sprite filledCheese;
    [SerializeField] Sprite unfilledCheese;
    [SerializeField] Image[] cheeses;

    public static ScavengerGameSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (Image cheese in cheeses)
        {
            cheese.sprite = unfilledCheese;
        }
    }

    public void ShowCheeseFound(int cheeseId)
    {
        cheeses[cheeseId].sprite = filledCheese;
    }
}
