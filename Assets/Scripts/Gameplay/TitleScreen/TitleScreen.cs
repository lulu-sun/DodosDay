using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Camera titleScreenCamera;
    [SerializeField] GameObject Canvas;

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

        Canvas.SetActive(true);
        OnShowTitle?.Invoke();
        titleScreenCamera.gameObject.SetActive(true);
        AudioManager.Instance.PlayTitleMusic();
        Debug.Log("gotem");
    }


    private IEnumerator Delay(float delay, Action onFinished = null)
    {
        yield return new WaitForSeconds(delay);
        OnLeaveTitle?.Invoke();
        Canvas.SetActive(false);
        titleScreenCamera.gameObject.SetActive(false);
        onFinished?.Invoke();
    }

    public void LeaveTitle(string nextScene = null)
    {
        startButton.interactable = false;
        memoriesButton.interactable = false;

        AudioManager.Instance.FadeMusic(1.5f, 0f);
        StartCoroutine(Delay(1.5f, () =>
        {
            if (nextScene != null)
            {
                SceneManager.LoadSceneAsync(SceneMapper.Instance.GetBuildIndexBySceneName(nextScene));
            }
        }));
    }


}
