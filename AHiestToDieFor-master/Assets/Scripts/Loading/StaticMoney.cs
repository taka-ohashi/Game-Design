using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticMoney : MonoBehaviour
{

    private static float TotalMoney = 5000;

    private static float moneyCount = 5000;

    private static int robbersAlive = 0;

    private static int lastScene = 0;

    public static float GetTotalMoneyCount() {return moneyCount;}
    public static float GetMoneyCount() {return moneyCount;}
    public static int GetRobbersAlive() {return robbersAlive;}
    public static void AddMoney(float money) {
        moneyCount += money;
    }
    public static void RemoveMoney(float money) {
        moneyCount -= money; 
    }
    public static void SetMoney(float money)
    {
        moneyCount = money;
    }
    public static void AddTotalMoney(float money) {TotalMoney += money;}
    public static void ResetMoney() {moneyCount = TotalMoney;}
    public static void SetRobbersAlive(int robbers) {robbersAlive = robbers;}
    public static int GetLastScene() {return lastScene;}
    public static void SetLastScene(int scene) {lastScene = scene;}
}
