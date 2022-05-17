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
        //StartCoroutine(FadeAudioSource.StartFade(BackgroundMusic, 0.1f, 0f));
        //SceneManager.LoadSceneAsync(SceneMapper.Instance.GetBuildIndexBySceneName("Intro"));
        Debug.Log(TitleScreen.Instance);
        TitleScreen.Instance.LeaveTitle();
    }

    public void OnMemoriesButtonPress()
    {
        Debug.Log("Memories Button pressed.");
    }
}

