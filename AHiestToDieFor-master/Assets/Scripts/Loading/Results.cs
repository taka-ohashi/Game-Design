using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Results : MonoBehaviour
{

    public GameObject button;
    public float money = 100;
    public int robbersAlive = 4;

    private float countingMoney = 0;
    private int countingRobbers = 0;
    private float addingAmount = 10;

    private float total = 0;

    private bool doneCountingMoney = false;
    private bool doneCountingRobbers = false;
    private bool doneCountingTotal = true;
    private bool robberWaiting = false;

    private string moneyText;

    private bool resetTotal = false;

    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();

        money = StaticMoney.GetMoneyCount();

        robbersAlive = StaticMoney.GetRobbersAlive();

        total = StaticMoney.GetTotalMoneyCount();
    }

    // Update is called once per frame
    void Update()
    {
        if(!doneCountingMoney)
        {
            CountMoney();
        }
        else if(!doneCountingRobbers)
        {
            CountRobbers();
        }
        else if(!doneCountingTotal)
        {
            if(!resetTotal) {addingAmount = 0; resetTotal = true;}
            CountTotal();
        }
    }

    private void CountMoney()
    {
        //this method counts up the money
        countingMoney += 10 + addingAmount;

        if(countingMoney > money)
        {
            doneCountingMoney = true;

            countingMoney = (int)money;

            //stores text values for money collected
            moneyText = text.text + "\n\n";
        }

        text.text = "" + countingMoney;

        addingAmount += 1;
    }

    private void CountRobbers()
    {
        //this method couts up the robbers
        if(countingRobbers < robbersAlive)
        {
            if(!robberWaiting)
            {
                StartCoroutine("LoadRobbers");
            }
        }
        else
        {
            doneCountingRobbers = true;

            //resets for counting total
            countingMoney = 0;

            //stores text values for robbers alive
            moneyText = text.text + "\n\n";

            //stores total money the player earned
            total = money + (robbersAlive * 1000);

            StartCoroutine("WaitTotal");
        }
    }

    private void CountTotal()
    {
        countingMoney += addingAmount;

        if(countingMoney > total)
        {
            doneCountingMoney = true;

            countingMoney = (int)total;

            doneCountingTotal = true;

            button.SetActive(true);
        }

        text.text = moneyText + countingMoney;

        addingAmount += 1;
    }



    IEnumerator LoadRobbers()
    {
        robberWaiting = true;
        yield return new WaitForSeconds(.5f);
        countingRobbers += 1;
        text.text = moneyText + countingRobbers;
        robberWaiting = false;
    }

    IEnumerator WaitTotal()
    {
        yield return new WaitForSeconds(.5f);
        doneCountingTotal = false;
    }


}
