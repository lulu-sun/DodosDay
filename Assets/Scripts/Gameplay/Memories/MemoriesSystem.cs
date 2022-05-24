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

    public int memoriesFound = 0;

    public void MarkMemoryFound()
    {
        memories[memoriesFound].sprite = filledMemory;
        ++memoriesFound;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SetActive(bool on)
    {
        overlay.gameObject.SetActive(on);
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
