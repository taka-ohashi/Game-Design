using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class WinScreenTrigger : MonoBehaviour
{
    private GlobalEventManager gem;

    private List<GameObject> nearbyRobbers;

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
        nearbyRobbers = new List<GameObject>();
    }
    private void Start()
    {
        gem.StartListening("Death", RemoveRobber);
    }
    private void OnDestroy()
    {
        gem.StopListening("Death", RemoveRobber);
    }
    private void RemoveRobber(GameObject target, List<object> parameters)
    {
        if (nearbyRobbers.Contains(target))
        {
            nearbyRobbers.Remove(target);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (!nearbyRobbers.Contains(other.gameObject))
            {
                nearbyRobbers.Add(other.gameObject);
            }
            //Debug.Log("EscapeWithMoney");
            
            //goes to EscapeWithMoney() in game mananger SCript (bottom)
            gem.TriggerEvent("EscapeWithMoney", other.gameObject, nearbyRobbers.Select(robber => (object)robber).ToList());
            // triggers event in GameManager
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (nearbyRobbers.Contains(other.gameObject))
            {
                nearbyRobbers.Remove(other.gameObject);
            }
        }
    }
}
