using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMapper : MonoBehaviour
{
    public static SceneMapper Instance { get; private set; }

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
            string pathToScene = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(pathToScene);
            //if (scenesByIndex[sceneIndex] != null && scenesByIndex[sceneIndex].name != null)
            //{
            //    Debug.Log($"{sceneIndex}: {scenesByIndex[sceneIndex].name}");
            //    sceneIndicesByName[scenesByIndex[sceneIndex].name] = sceneIndex;
            //}
            sceneIndicesByName[sceneName] = sceneIndex;

            Debug.Log($"{sceneIndex}: {sceneName}");
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

    public bool SceneNameExists(string sceneName)
    {
        return sceneIndicesByName.ContainsKey(sceneName);
    }
}