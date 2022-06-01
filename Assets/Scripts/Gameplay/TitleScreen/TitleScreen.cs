using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Camera titleScreenCamera;
    [SerializeField] GameObject canvas;

    [SerializeField] Button startButton;
    [SerializeField] Button memoriesButton;

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
        startButton.interactable = true;
        memoriesButton.interactable = true;

        canvas.SetActive(true);
        OnShowTitle?.Invoke();
        titleScreenCamera.gameObject.SetActive(true);
        AudioManager.Instance.PlayTitleMusic();
    }


    private IEnumerator DelayedLeaveTitle(float delay, Action onFinished = null)
    {
        yield return new WaitForSeconds(delay);
        OnLeaveTitle?.Invoke();
        canvas.SetActive(false);
        onFinished?.Invoke();
    }

    public void LeaveTitleForAlbum()
    {
        LeaveTitle("PhotoAlbum", () =>
        {
            titleScreenCamera.gameObject.SetActive(false);
        });
    }

    public void LeaveTitle(string nextScene = null, Action onFinished = null)
    {
        startButton.interactable = false;
        memoriesButton.interactable = false;

        AudioManager.Instance.FadeMusic(1.5f, 0f);
        StartCoroutine(DelayedLeaveTitle(1.5f, () =>
        {
            if (nextScene != null)
            {
                SceneManager.LoadSceneAsync(SceneMapper.Instance.GetBuildIndexBySceneName(nextScene));
            }

            onFinished?.Invoke();
        }));
    }


}
