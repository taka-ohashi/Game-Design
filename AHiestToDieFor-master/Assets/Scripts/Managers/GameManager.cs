using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    private GlobalEventManager gem;

    private List<GameObject> robbers;

    public TextMeshProUGUI moneyText;

    public float winAmount;
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
        robbers = new List<GameObject>();
    }

    private void Start()
    {
        gem.StartListening("AttemptStartGame", AttemptStartGame);
        gem.StartListening("UpdateMoney", UpdateMoney);
        gem.StartListening("EscapeWithMoney", Escape);
        gem.StartListening("RobberEnteredSpawnArea", TrackRobber);
        gem.StartListening("Death", RemoveRobber);
    }
    private void OnDestroy()
    {
        gem.StopListening("AttemptStartGame", AttemptStartGame);
        gem.StopListening("UpdateMoney", UpdateMoney);
        gem.StopListening("EscapeWithMoney", Escape);
        gem.StopListening("RobberEnteredSpawnArea", TrackRobber);
        gem.StopListening("Death", RemoveRobber);
    }

    private void AttemptStartGame(GameObject target, List<object> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find list of robbers");
        }
        if (parameters[0].GetType() != typeof(List<GameObject>))
        {
            throw new Exception("Illegal argument: parameter wrong type: " + parameters[0].GetType().ToString());
        }

        Queue<GameObject> robbers = new Queue<GameObject>((List<GameObject>) parameters[0]);

        gem.TriggerEvent("StartGame", gameObject);
        gem.TriggerEvent("RobbersSelected", gameObject, new List<object> { robbers });
    }
    private void TrackRobber(GameObject target, List<object> parameters)
    {
        if (robbers.Contains(target))
        {
            return;
        }
        robbers.Add(target);
    }
    private void RemoveRobber(GameObject target, List<object> parameters)
    {
        if (!robbers.Contains(target))
        {
            throw new Exception("Missing robber: Tried to remove robber that didn't exist");
        }
        robbers.Remove(target);
    }
    private float GetAccumulatedStolenMoney()
    {
        robbers = robbers.Where(robber => robber != null).ToList();
        return robbers.Select(robber => robber.GetComponent<MoneyBag>().money).Sum();
    }
    private void UpdateMoney(GameObject target, List<object> parameters)
    {
        moneyText.text = string.Format("Stolen money: ${0}", GetAccumulatedStolenMoney());
    }
    private void Escape(GameObject target, List<object> parameters)
    {
        if (parameters.Count == 0)
        {
            throw new Exception("Missing parameter: Could not find list of robbers parameter");
        }
        foreach(object robber in parameters)
        {
            if (robber.GetType() != typeof(GameObject))
            {
                throw new Exception("Illegal argument: parameter wrong type");
            }
        }
        List<GameObject> robbersCloseToEscapeVan = parameters.Select(robber => (GameObject)robber).ToList();
        if (robbers.All(robbersCloseToEscapeVan.Contains))
        {
            if (GetAccumulatedStolenMoney() >= winAmount)
            {
                //Store money, robbers, and next level
                StaticMoney.SetMoney(GetAccumulatedStolenMoney());
                //StaticMoney.set
                StaticMoney.SetRobbersAlive(robbers.Count);
                StaticMoney.SetLastScene(SceneManager.GetActiveScene().buildIndex + 1);

                SceneManager.LoadScene(1);
                //print(LoadNewScene.scene);

                //load next scene unless no more levels, then load title screen
                //if(LoadNewScene.scene <= 3) {SceneManager.LoadScene(LoadNewScene.scene);}
                ///else {SceneManager.LoadScene(0);}
            }
        }
    }

}
