using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] AudioClip backgroundMusic;
    [SerializeField] Camera titleScreenCamera;
    

    public event Action OnShowTitle;
    public event Action OnLeaveTitle;

    public static TitleScreen Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTitle()
    {
        OnShowTitle?.Invoke();
        titleScreenCamera.gameObject.SetActive(true);

    }

    public void LeaveTitle()
    {
        OnLeaveTitle?.Invoke();
        titleScreenCamera.gameObject.SetActive(false);
    }


}
