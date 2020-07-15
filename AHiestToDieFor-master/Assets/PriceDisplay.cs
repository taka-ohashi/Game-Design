using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class PriceDisplay : MonoBehaviour
{
    public string robberType;

    private TextMeshProUGUI priceText;

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
        priceText = GetComponent<TextMeshProUGUI>();
        gem.StartListening(string.Format("Update{0}RobberCost", robberType), UpdateCost);
    }
    private void UpdateCost(GameObject target, List<object> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find money");
        }
        if (parameters[0].GetType() != typeof(float))
        {
            throw new Exception("Illegal argument: parameter wrong type: " + parameters[0].GetType().ToString());
        }
        UpdateCost((float)parameters[0]);
    }

    private void UpdateCost(float cost)
    {
        priceText.text = string.Format("${0}", cost);
    }
}
