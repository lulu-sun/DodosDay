using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesMapper : MonoBehaviour
{
    public static ScenesMapper Instance { get; private set; }

    private Scene[] scenesByIndex;

    private Dictionary<string, int> sceneIndicesByName;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        scenesByIndex = new Scene[SceneManager.sceneCountInBuildSettings];
        sceneIndicesByName = new Dictionary<string, int>();

        for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCountInBuildSettings; sceneIndex++)
        {
            scenesByIndex[sceneIndex] = SceneManager.GetSceneByBuildIndex(sceneIndex);
            if (scenesByIndex[sceneIndex] != null)
            {
                sceneIndicesByName[scenesByIndex[sceneIndex].name] = sceneIndex;
            }
        }
    }

    public int GetBuildIndexBySceneName(string sceneName)
    {
        return sceneIndicesByName[sceneName];
    }

    public string GetSceneNameByIndex(int sceneIndex)
    {
        return GetSceneByIndex(sceneIndex).name;
    }

    public Scene GetSceneByIndex(int sceneIndex)
    {
        return scenesByIndex[sceneIndex];
    }
}