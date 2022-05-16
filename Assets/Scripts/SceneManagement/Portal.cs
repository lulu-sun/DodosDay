using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour
{
    [SerializeField] float fadeTimeInSeconds = 0.5f;
    [SerializeField] string sceneToLoad = "";
    [SerializeField] PortalId portalId = PortalId.A;
    Fader fader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            Debug.Log($"Player entered the portal with sceneToLoad={sceneToLoad}");
            StartCoroutine(SwitchScene());
        }
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

        yield return SceneManager.LoadSceneAsync(SceneMapper.Instance.GetBuildIndexBySceneName(sceneToLoad));

        var destinationSpawnPoint = GameObject.FindObjectsOfType<SpawnPoint>().Single(sp => sp.portalId == portalId);
        GameController.Instance.playerController.transform.position = destinationSpawnPoint.transform.position;

        yield return fader.FadeOut(fadeTimeInSeconds);
        GameController.Instance.Unpause();

        Destroy(gameObject);
    }
}

public enum PortalId { A, B, C, D, E };