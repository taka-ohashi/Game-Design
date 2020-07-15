using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HideStartButtonScript : MonoBehaviour
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
    private void Start()
    {
        gem.StartListening("StartGame", StartGame);
    }
    private void OnDestroy()
    {
        gem.StopListening("StartGame", StartGame);
    }
    private void StartGame(GameObject target, List<object> parameters)
    {
        gameObject.SetActive(false);
    }
}
