using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, IPlayerTriggerable
{
    public void OnPlayerTriggered(PlayerController player)
    {
        // throw new System.NotImplementedException();
        Debug.Log("Player entered the portal");
    }
}
