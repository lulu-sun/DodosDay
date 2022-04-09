using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{
    public GameObject player;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var plantLocation = transform.position.y;
        var playerLocation = player.transform.position.y;

        if (playerLocation < plantLocation)
        {
            this.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Plant");
        }
        else
        {
            this.GetComponent<Renderer>().sortingLayerID = SortingLayer.NameToID("Player");
        }

        Debug.Log(player.transform.position.y);
    }
}
