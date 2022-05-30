using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenWingsSystem : MonoBehaviour
{
    [SerializeField] Canvas chickenWingsCanvas;

    public static ChickenWingsSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetActive(false);
    }

    public void SetActive(bool active)
    {
        chickenWingsCanvas.gameObject.SetActive(active);
    }
}
