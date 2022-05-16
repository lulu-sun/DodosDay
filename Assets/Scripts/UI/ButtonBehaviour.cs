using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    public void OnStartButtonPress()
    {
        Debug.Log("Start Button pressed.");
        SceneManager.LoadSceneAsync(SceneMapper.Instance.GetBuildIndexBySceneName("Intro"));
    }

    public void OnMemoriesButtonPress()
    {
        Debug.Log("Memories Button pressed.");
    }
}
