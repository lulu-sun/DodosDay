using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public void OnStartButtonPress()
    {
        Debug.Log("Start Button pressed.");        
        TitleScreen.Instance.LeaveTitle(onFinished: () => CutsceneManager.Instance.RunIntroCutscene());
    }

    public void OnMemoriesButtonPress()
    {
        Debug.Log("Memories Button pressed.");
        TitleScreen.Instance.LeaveTitleForAlbum();
    }

    public void PlayPopSfx()
    {
        AudioManager.Instance.PlayPopSfx();
    }

    public void OnMailboxButtonPress()
    {
        Debug.Log("Mailbox Button pressed.");
        TitleScreen.Instance.LeaveTitle(onFinished: () => MailboxSystem.Instance.OpenMailbox());
    }

    public void OnOpenLuluMessageButtonPress()
    {
        MailboxSystem.Instance.OpenLuluMessage();
    }

    public void OnOpenJuanJuanMessageButtonPress()
    {
        MailboxSystem.Instance.OpenJuanJuanMessage();
    }

    public void OnCloseMessageButtonPress()
    {
        MailboxSystem.Instance.GoBackToMailbox();
    }

    public void OnCloseMailboxButtonPress()
    {
        MailboxSystem.Instance.CloseMailbox();
        TitleScreen.Instance.ShowTitle();
    }
}

