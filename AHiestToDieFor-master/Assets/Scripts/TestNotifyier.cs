using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TestNotifyier : MonoBehaviour
{
    private GlobalEventManager gem;
    public List<GameObject> TEST_SPAWN_ROBBERS;
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
        StartCoroutine(SpawnRobbers());
    }

    private IEnumerator SpawnRobbers()
    {
        yield return new WaitForSeconds(1);
        gem.TriggerEvent("RobbersSelected", gameObject, new List<object> { new Queue<GameObject>(TEST_SPAWN_ROBBERS) });
    }
}
