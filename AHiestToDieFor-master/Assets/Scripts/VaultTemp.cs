using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VaultTemp : MonoBehaviour
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

    void OnCollisionStay(Collision other)

    {
        if(other.gameObject.CompareTag("Player"))
        {
            gem.TriggerEvent("StoleVault", other.gameObject);
            //Destroy(gameObject);
        }
    }
}
