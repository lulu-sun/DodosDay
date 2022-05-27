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
        ResetCheeseImages();
        CheeseSetActive(true);
    }

    public void EndGame()
    {
        overlay.gameObject.SetActive(false);
        CheeseSetActive(false);
    }

    private void CheeseSetActive(bool active)
    {
        foreach (Cheese cheese in Resources.FindObjectsOfTypeAll<Cheese>())
        {
            cheese.gameObject.SetActive(active);
        }
    }

    private void ResetCheeseImages()
    {
        foreach (Image cheese in cheeses)
        {
            cheese.sprite = unfilledCheese;
        }
    }

    private void Start()
    {
        ResetCheeseImages();

        // For Testing
        //StartGame();
    }

    public void ShowCheeseFound(int cheeseId)
    {
        cheeses[cheeseId].sprite = filledCheese;

        if (AllCheesesFound())
        {
            GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.CheeseGame, CheckpointState.Complete);
        }
    }

    public bool AllCheesesFound()
    {
        int count = 0;
        foreach (Image cheeseImage in cheeses)
        {
            if (cheeseImage.sprite == filledCheese)
            {
                count++;
            }
        }

        return count == cheeses.Length;
    }
}
