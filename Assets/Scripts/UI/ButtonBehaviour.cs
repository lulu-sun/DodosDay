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
        //SceneManager.LoadSceneAsync(SceneMapper.Instance.GetBuildIndexBySceneName("Intro"));
        TitleScreen.Instance.LeaveTitle();
    }

    public void OnMemoriesButtonPress()
    {
        Debug.Log("Memories Button pressed.");

    }

    public void OnButtonHover()
    {
        AudioManager.Instance.PlayPopSfx();
    }
}

