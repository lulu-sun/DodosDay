using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour
{

    [SerializeField] int sceneToLoad = -1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Player entered the portal with sceneToLoad={sceneToLoad}");
        StartCoroutine(SwitchScene());
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var destinationPortal = GameObject.FindGameObjectsWithTag("SpawnPoint").First();
        GameController.Instance.playerController.transform.position = destinationPortal.transform.position;

        Destroy(gameObject);
    }
}
