using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnManager : MonoBehaviour
{
    public List<GameObject> spawnPoints;

    private GlobalEventManager gem;
    private Queue<GameObject> robbersSpawnQueue;

    private int currentSpawnPoints = 0;
    private int totalRobberCount;
    private int deaths;
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
        robbersSpawnQueue = new Queue<GameObject>();
    }
    private void Start()
    {
        gem.StartListening("RobbersSelected", RobbersSelected);
        gem.StartListening("Death", SpawnNextRobber);
    }
    private void OnDestroy()
    {
        gem.StopListening("RobbersSelected", RobbersSelected);
        gem.StopListening("Death", SpawnNextRobber);
    }
    private void RobbersSelected(GameObject target, List<object> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find list of robbers");
        }
        if (parameters[0].GetType() != typeof(Queue<GameObject>))
        {
            throw new Exception("Illegal argument: parameter wrong type: " + parameters[0].GetType().ToString());
        }
        if ((parameters[0] as Queue<GameObject>).Count > 4)
        {
            throw new Exception("Illegal argument: too many robbers in queue: " + (parameters[0] as Queue<GameObject>).Count);
        }
        robbersSpawnQueue = (Queue<GameObject>) parameters[0];
        totalRobberCount = ((Queue<GameObject>) parameters[0]).Count;
        InstantiateRobbers(Constants.MAX_ROBBERS_OUT_SIMULTANEOUSLY);
    }
    private void InstantiateRobbers(int amount)
    {
        int n = amount;
        if (n > robbersSpawnQueue.Count)
        {
            n = robbersSpawnQueue.Count;
        }
        for (int i = 0; i < n; i++)
        {
            Instantiate(robbersSpawnQueue.Dequeue(), spawnPoints[currentSpawnPoints % spawnPoints.Count].transform.position, spawnPoints[currentSpawnPoints % spawnPoints.Count].transform.rotation);
            currentSpawnPoints++;
        }
    }
    private void SpawnNextRobber(GameObject target, List<object> parameters)
    {
        deaths++;
        if (deaths == totalRobberCount)
        {
            StartCoroutine(LoseGame());
        }
        InstantiateRobbers(1);
    }
    private IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(1);
        gem.TriggerEvent("LostGame", gameObject);
    }
}
