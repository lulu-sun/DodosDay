using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EssentialObjectsLoader : MonoBehaviour
{
    [SerializeField] GameObject essentialObjectsPrefab;

    private void Awake()
    {
        var existingObjects = FindObjectsOfType<EssentialObjects>();
        if (existingObjects.Length == 0)
        {
            Instantiate(essentialObjectsPrefab, Vector3.zero, Quaternion.identity);

            // find spawn point if there is one
            // var spawnPoint = GameObject.FindObjectsOfType<SpawnPoint>().First();
            // GameController.Instance.playerController.transform.position = spawnPoint.transform.position;
        }
    }
}
