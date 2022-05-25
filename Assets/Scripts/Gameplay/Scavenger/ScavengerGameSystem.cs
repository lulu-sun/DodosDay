using UnityEngine;
using UnityEngine.UI;
using System;

public class CheeseGameSystem : MonoBehaviour
{
    [SerializeField] Sprite filledCheese;
    [SerializeField] Sprite unfilledCheese;
    [SerializeField] Image[] cheeses;
    [SerializeField] Canvas overlay;

    public bool IsActive { get => overlay.gameObject.activeSelf; }

    public static CheeseGameSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        overlay.gameObject.SetActive(true);
    }

    public void EndGame()
    {
        overlay.gameObject.SetActive(false);
    }

    private void Start()
    {
        foreach (Image cheese in cheeses)
        {
            cheese.sprite = unfilledCheese;
        }

        // For Testing
        //StartGame();
    }

    public void ShowCheeseFound(int cheeseId)
    {
        cheeses[cheeseId].sprite = filledCheese;
        
    }
}
