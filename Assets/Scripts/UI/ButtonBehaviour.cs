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

    public void OnButtonHover()
    {
        AudioManager.Instance.PlayPopSfx();
    }

    public void OnMailboxButtonPress()
    {

    }
}

