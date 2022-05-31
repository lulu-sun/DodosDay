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
        if (memoriesFound < memories.Length)
        {
            memories[memoriesFound].sprite = filledMemory;
            ++memoriesFound;
        }

        if (memoriesFound >= memories.Length)
        {
            GameCheckpoints.Instance.UpdateCheckpointState(Checkpoint.AllMemoriesFound, CheckpointState.Complete);
        }
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
