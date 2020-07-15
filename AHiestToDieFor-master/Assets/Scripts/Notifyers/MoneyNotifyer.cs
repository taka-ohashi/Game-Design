using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoneyNotifyer : MonoBehaviour
{
    private GlobalEventManager gem;

    public float amount;

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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            gem.TriggerEvent("AddMoneyToRobber", collision.gameObject, new List<object> { amount });
            // triggers event in MoneyBag
            Destroy(gameObject);
        }
    }
}
