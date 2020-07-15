using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class ShowMoneyTextScript : MonoBehaviour
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
        gem.StartListening("StartGame", ShowText);
        gem.StartListening("LostGame", HideText);
    }
    private void ShowText(GameObject target, List<object> parameters)
    {
        GetComponent<TextMeshProUGUI>().enabled = true;
    }
    private void HideText(GameObject target, List<object> parameters)
    {
        GetComponent<TextMeshProUGUI>().enabled = false;
    }
}
