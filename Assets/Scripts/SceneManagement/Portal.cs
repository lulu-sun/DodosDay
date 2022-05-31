using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class Portal : MonoBehaviour
{
    [SerializeField] float fadeTimeInSeconds = 0.5f;
    [SerializeField] string sceneToLoadName = "";
    [SerializeField] PortalId portalId = PortalId.A;
    Fader fader;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            Debug.Log($"Player entered the portal with sceneToLoadName={sceneToLoadName}");
            StartCoroutine(SwitchScene());
        }
    }

    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator SwitchScene()
    {
        if (sceneToLoadName == "Last_Island" && GameCheckpoints.Instance.NotComplete(Checkpoint.AllMemoriesFound))
        {
            CutsceneManager.Instance.BlockFinalIsland();
            yield return null;
        }
        else
        {
            DontDestroyOnLoad(gameObject);

            GameController.Instance.Pause();
            yield return fader.FadeIn(fadeTimeInSeconds);

            yield return SceneManager.LoadSceneAsync(SceneMapper.Instance.GetBuildIndexBySceneName(sceneToLoadName));

            var destinationSpawnPoint = FindObjectsOfType<SpawnPoint>().Single(sp => sp.portalId == portalId);
            GameController.Instance.playerController.transform.position = destinationSpawnPoint.transform.position;

            yield return fader.FadeOut(fadeTimeInSeconds);

            GameController.Instance.Unpause();
            GameEventSystem.Instance.TryTriggerEnterSceneGameEvent(sceneToLoadName);

            Destroy(gameObject);
        }
    }
}

public enum PortalId { A, B, C, D, E };