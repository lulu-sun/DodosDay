using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MailboxSystem : MonoBehaviour
{
    [SerializeField] Canvas inboxCanvas;
    [SerializeField] Canvas messageCanvas;

    [SerializeField] Text luluMessage;
    [SerializeField] Text juanjuanMessage;

    [SerializeField] Button inboxCloseButton;

    public static MailboxSystem Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CloseMailbox();
        CloseMessageView();
    }

    public void OpenMailbox()
    {
        inboxCanvas.gameObject.SetActive(true);
        inboxCloseButton.interactable = true;
    }

    public void CloseMailbox()
    {
        inboxCanvas.gameObject.SetActive(false);
    }

    private void OpenMessageView()
    {
        messageCanvas.gameObject.SetActive(true);
    }

    private void CloseMessageView()
    {
        luluMessage.gameObject.SetActive(false);
        juanjuanMessage.gameObject.SetActive(false);
        messageCanvas.gameObject.SetActive(false);
    }

    public void GoBackToMailbox()
    {
        CloseMessageView();
        OpenMailbox();
    }

    public void OpenLuluMessage()
    {
        CloseMailbox();
        OpenMessageView();
        luluMessage.gameObject.SetActive(true);
    }

    public void OpenJuanJuanMessage()
    {
        CloseMailbox();
        OpenMessageView();
        juanjuanMessage.gameObject.SetActive(true);
    }

    public void GoBackToTitleScreen(Action onFinished = null)
    {
        inboxCloseButton.interactable = false;
        StartCoroutine(DelayedLeaveMailbox(1.5f, onFinished));
    }

    private IEnumerator DelayedLeaveMailbox(float delay, Action onFinished = null)
    {
        AudioManager.Instance.FadeMusic(delay, 0);
        yield return new WaitForSeconds(delay);
        CloseMailbox();
        onFinished?.Invoke();
    }
}
