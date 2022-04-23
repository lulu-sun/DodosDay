using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour
{
    [SerializeField] float fadeTimeInSeconds = 0.5f;
    [SerializeField] int sceneToLoad = -1;
    [SerializeField] PortalId portalId = PortalId.A;
    Fader fader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Player entered the portal with sceneToLoad={sceneToLoad}");
        StartCoroutine(SwitchScene());
    }

    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator SwitchScene()
    {
        DontDestroyOnLoad(gameObject);

        GameController.Instance.Pause();
        yield return fader.FadeIn(fadeTimeInSeconds);

        yield return SceneManager.LoadSceneAsync(sceneToLoad);

        var destinationPortal = GameObject.FindObjectsOfType<SpawnPoint>().Single(sp => sp.portalId == portalId);
        GameController.Instance.playerController.transform.position = destinationPortal.transform.position;

        yield return fader.FadeOut(fadeTimeInSeconds);
        GameController.Instance.Unpause();

        Destroy(gameObject);
    }
}

public enum PortalId { A, B, C, D, E};