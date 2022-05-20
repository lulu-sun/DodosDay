using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoriesSystem : MonoBehaviour
{

    [SerializeField] Sprite unfilledMemory;
    [SerializeField] Sprite filledMemory;
    [SerializeField] Canvas overlay;
    [SerializeField] Image[] memories;

    public static MemoriesSystem Instance { get; private set; }

    public int memoriesFound;

    public void FindMemory()
    {
        ShowMemoryFound();
        ++memoriesFound;
    }

    public void ShowMemoryFound()
    {
        memories[memoriesFound - 1].sprite = filledMemory;
    }

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


    // Start is called before the first frame update
    void Start()
    {
        foreach (Image memory in memories)
        {
            memory.sprite = unfilledMemory;
        }
    }



}
