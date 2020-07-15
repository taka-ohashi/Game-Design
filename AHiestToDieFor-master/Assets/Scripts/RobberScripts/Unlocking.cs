using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unlocking : MonoBehaviour
{
    private GlobalEventManager gem;

    [SerializeField]
    private float unlockingSpeed;

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

    private void Update()
    {
        
    }

    // void OnCollisionStay(Collision other)
    // {
    //     if(other.gameObject.tag == "Vault")
    //     {
    //         BeginUnlocking(gameObject, new List<object>());
    //     }
    // }

    // void Start()
    // {
    //     gem.StartListening("Unlock", BeginUnlocking);
    //     gem.StartListening("Move", StopUnlocking);
    //     gem.StartListening("Shot", StopUnlocking);
    //     gem.StartListening("Death", StopUnlocking);
    // }

    // public void OnDestroy()
    // {
    //     gem.StopListening("Unlock", BeginUnlocking);
    //     gem.StopListening("StopUnlocking", BeginUnlocking);
    //     gem.StopListening("Move", StopUnlocking);
    //     gem.StopListening("Shot", StopUnlocking);
    //     gem.StopListening("Death", StopUnlocking);
        
    // }

    // public void BeginUnlocking(GameObject target, List<object> parameters)
    // {
    //     if (target != gameObject)
    //     {
    //         return;
    //     }
    //     gem.TriggerEvent("BeginUnlocking", gameObject, new List<object> { unlockingSpeed });
    //     // triggers event in CrackVault
    // }
    // public void StopUnlocking(GameObject target, List<object> parameters)
    // {
    //     if (target != gameObject)
    //     {
    //         return;
    //     }

    //     gem.TriggerEvent("StopUnlocking", gameObject);
    // }

    public float GetUnlockingSpeed() {return unlockingSpeed;}
}
