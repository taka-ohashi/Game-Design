using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class SelectionMoneyManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Image image;

    private float money;

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
        money = StaticMoney.GetMoneyCount();
        UpdateMoneyText();
        gem.StartListening("SetSelectionMoney", UpdateMoney);
        gem.StartListening("StartGame", HideText);
        gem.StartListening("LostGame", Destroy);
    }
    private void OnDestroy()
    {
        gem.StopListening("SetSelectionMoney", UpdateMoney);
        gem.StopListening("StartGame", HideText);
        gem.StopListening("LostGame", Destroy);
    }
    private void UpdateMoney(GameObject target, List<object> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find money");
        }
        if (parameters[0].GetType() != typeof(float))
        {
            throw new Exception("Illegal argument: parameter wrong type: " + parameters[0].GetType().ToString());
        }
        money = (float) parameters[0];
        UpdateMoneyText();
    }
    private void UpdateMoneyText()
    {
        text.text = string.Format("Money: ${0}", money);
    }
    private void HideText(GameObject target, List<object> parameters)
    {
        text.enabled = false;
    }
    private void Destroy(GameObject target, List<object> parameters)
    {
        Destroy(gameObject);
    }
}
