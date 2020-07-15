using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class RobberSelectionManager : MonoBehaviour
{
    public GameObject[] robberPrefabs;
    public Sprite[] robberList;
    public Button[] buttonList;

    private GameObject[] selectedRobbers;
    private float[] robberBaseCosts;
    private Button button;
    private int slotNum = -1;
    private int robberNum = -1;

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
        selectedRobbers = new GameObject[4];
        robberBaseCosts = new float[] {
            Constants.BASE_FAST_ROBBER_COST, 
            Constants.BASE_STRONG_ROBBER_COST, 
            Constants.BASE_BIG_ROBBER_COST, 
            Constants.BASE_GREEDY_ROBBER_COST
        };
    }
    private void Start()
    {
        gem.StartListening("StartGame", StartGame);
    }
    private void OnDestroy()
    {
        gem.StopListening("StartGame", StartGame);
    }
    // Update is called once per frame
    void Update()
    {
        //displays image over button
        // Only go into this if statement
        // if the player has enough money to buy anything to begin with
        if (robberNum > -1 && slotNum > -1)
        {
            if (CheckIfEnoughMoney(robberNum, slotNum) == false)
            {
                robberNum = -1;
                slotNum = -1;
                button.transform.GetChild(1).gameObject.SetActive(false);
                button.transform.GetChild(0).gameObject.SetActive(false);
                return;
            }
            if (selectedRobbers[slotNum] != null)
            {
                StaticMoney.AddMoney(GetRobberCost(selectedRobbers[slotNum]));
            }
            StaticMoney.RemoveMoney(GetNewRobberCost(robberPrefabs[robberNum]));
            gem.TriggerEvent("SetSelectionMoney", gameObject, new List<object> { StaticMoney.GetMoneyCount() });
            button = buttonList[slotNum];
            button.image.sprite = robberList[robberNum];
            selectedRobbers[slotNum] = robberPrefabs[robberNum];
            button.transform.GetChild(1).gameObject.SetActive(false);
            button.transform.GetChild(0).gameObject.SetActive(false);

            //shows next button
            //TODO: dont show button if player can't afford
            if (slotNum < 3)
            {
                button = buttonList[slotNum + 1];
                button.transform.gameObject.SetActive(true);
            }

            //reset
            robberNum = -1;
            slotNum = -1;
        }
    }
    private bool CheckIfEnoughMoney(int robberNum, int slotNum)
    {
        float newAmount = StaticMoney.GetMoneyCount();
        if (selectedRobbers[slotNum] != null)
        {
            newAmount += GetRobberCost(selectedRobbers[slotNum]);
        }
        newAmount -= GetNewRobberCost(robberPrefabs[robberNum]);
        return newAmount > 0;
    }
    private void StartGame(GameObject target, List<object> parameters)
    {
        gameObject.SetActive(false);
    }
    public void AttemptStartGame()
    {
        if (selectedRobbers.All(robber => robber == null))
        {
            return;
        }
        List<GameObject> filteredSelection = selectedRobbers
            .Where(robber => robber != null)
            .ToList();
        gem.TriggerEvent("AttemptStartGame", gameObject, new List<object> { filteredSelection });
    }

    //Set what sprite num
    public void pickFast()
    {
        robberNum = 0;

    }
    public void pickStrong()
    {
        robberNum = 1;
    }
    public void pickBig()
    {
        robberNum = 2;
    }
    public void pickGreedy()
    {
        robberNum = 3;
    }

    //Set what button slot player chooses
    public void slot0()
    {
        slotNum = 0;
        UpdateCosts();
    }
    public void slot1()
    {
        slotNum = 1;
        UpdateCosts();
    }
    public void slot2()
    {
        slotNum = 2;
        UpdateCosts();
    }
    public void slot3()
    {
        slotNum = 3;
        UpdateCosts();
    }
    private void UpdateCosts()
    {
        gem.TriggerEvent("UpdateFastRobberCost", gameObject, new List<object> { GetNewRobberCost(robberPrefabs[0]) });
        gem.TriggerEvent("UpdateStrongRobberCost", gameObject, new List<object> { GetNewRobberCost(robberPrefabs[1]) });
        gem.TriggerEvent("UpdateBigRobberCost", gameObject, new List<object> { GetNewRobberCost(robberPrefabs[2]) });
        gem.TriggerEvent("UpdateGreedyRobberCost", gameObject, new List<object> { GetNewRobberCost(robberPrefabs[3]) });
    }
    private float GetNewRobberCost(GameObject prefab)
    {
        int index = Array.FindIndex(robberPrefabs, robberPrefab => robberPrefab == prefab);
        int numberOfRobbers = selectedRobbers
            .Where(robber => robber == prefab)
            .ToList().Count;
        float baseCost = robberBaseCosts[index];
        return baseCost + baseCost * 0.25f * numberOfRobbers;
    }
    private float GetRobberCost(GameObject prefab)
    {
        int index = Array.FindIndex(robberPrefabs, robberPrefab => robberPrefab == prefab);
        int numberOfRobbers = selectedRobbers
            .Where(robber => robber == prefab)
            .ToList().Count;
        if (numberOfRobbers > 0)
        {
            numberOfRobbers--;
        }
        float baseCost = robberBaseCosts[index];
        return baseCost + baseCost * 0.25f * numberOfRobbers;
    }
    private class Purchase {
        public GameObject robberPrefab;
        public float cost;

        public Purchase(GameObject robberPrefab, float cost)
        {
            this.robberPrefab = robberPrefab;
            this.cost = cost;
        }
   }
}
