using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GlobalEventManager : MonoBehaviour
{

    private Dictionary<string, Action<GameObject, List<object>>> eventDictionary = new Dictionary<string, Action<GameObject, List<object>>>();

    public void StartListening(string eventName, Action<GameObject, List<object>> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] += listener;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public void StopListening(string eventName, Action<GameObject, List<object>> listener)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= listener;
        }
    }

    public void TriggerEvent(string eventName, GameObject target, List<object> parameters = null)
    {
        parameters = parameters ?? new List<object>();
        Action<GameObject, List<object>> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(target, parameters);
        }
    }
}

