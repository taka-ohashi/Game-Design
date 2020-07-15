using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RobberSpawnNotifyer : MonoBehaviour
{
    private GlobalEventManager gem;
    private void Awake()
    {
        List<MonoBehaviour> deps = new List<MonoBehaviour>
        {
            (gem = FindObjectOfType(typeof(GlobalEventManager)) as GlobalEventManager),
        };
        if (deps.Contains(null))
        {
            throw new Exception("Could not find dependency");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            gem.TriggerEvent("RobberEnteredSpawnArea", other.gameObject);
            // triggers event in SelectedManager
        }
    }
}
