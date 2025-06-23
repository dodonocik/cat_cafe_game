using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private static List<OrderIngredient> currentOrder = new List<OrderIngredient>();
    public static int money = 0;
    public List<ItemObject> allItems;
    public static List<ItemObject> teaItems= new List<ItemObject>();
    public static List<ItemObject> toppingItems = new List<ItemObject>();
    public static List<ItemObject> extraItems = new List<ItemObject>();
    public static List<OrderIngredient> orderIngridients = new List<OrderIngredient>();
    public static Item selectedItem;
    public static bool minigameActive = false;
    private static List<int> brewMinigameScores = new List<int>();
    public static List<int> pourMinigameScores = new List<int>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        teaItems.Clear();
        toppingItems.Clear();
        extraItems.Clear();
        brewMinigameScores.Clear();
        pourMinigameScores.Clear();
        orderIngridients.Clear();
        money = 0;
        MoneyManager.daysAmount = 0;
        foreach (var item in allItems)
        {
            if(item.type == "tea")
                teaItems.Add(item);
            if(item.type == "topping")
                toppingItems.Add(item);
            if(item.type == "extra")
                extraItems.Add(item);
        }
        foreach (var item in teaItems)
            Debug.Log(item.name);
        OrderManager.PrepareLists();
        MoneyManager.daysAmount = 0;
    }

    public static void StartMinigame()
    {
        Debug.Log("starting minigame");

        if (BrewingMinigameManager.Instance == null)
        {
            Debug.LogError("BrewingMinigameManager.Instance is NULL!");
            return;
        }

        BrewingMinigameManager.Instance.StartMinigame();
        minigameActive=true;
    }

    public static void StopMinigame()
    {
        
        BrewingMinigameManager.Instance.StopMinigame();
        brewMinigameScores.Add(((int)BrewingMinigameManager.getScore()));
        minigameActive = false;
        OrderIngredient orderItem = new OrderIngredient(selectedItem.itemObject);
        orderIngridients.Add(orderItem);
        DisplayUi.Instance.UpdateIngridients();
        Debug.Log(orderIngridients);
    }

    public static void StartPourMinigame()
    {
        PourMinigameManager.Instance.StartMinigame(selectedItem);
        OrderIngredient orderItem = new OrderIngredient(selectedItem.itemObject);
        orderIngridients.Add(orderItem);
        DisplayUi.Instance.UpdateIngridients();
    }

  

    // Update is called once per frame
    void Update()
    {
    }

    public static void updateMoney()
    {
        DisplayUi.Instance.UpdateMoney();
    }
    
    public static void ClearOrder()
    {
        orderIngridients = new List<OrderIngredient>();
        brewMinigameScores.Clear();
        pourMinigameScores.Clear();
        DisplayUi.Instance.UpdateIngridients();
    }

    public static void FillOrder()
    {
        currentOrder = OrderManager.GenerateOrder();
    }

    public static List<OrderIngredient> GetCurrentOrder()
    {
        return currentOrder;
    }
    
    public static int EvaluateScore()
    {
        float avgScore = 0f;
        float ingridientScore= OrderManager.EvaluateOrder(currentOrder, orderIngridients);
        foreach (var minigameScore in brewMinigameScores)
        {
            avgScore += minigameScore;
        }
        Debug.Log(avgScore);
        avgScore = avgScore / brewMinigameScores.Count;
        float otherScore = 0f;
        if (pourMinigameScores.Count > 0)
        {
            foreach (var minigameScore in pourMinigameScores)
            {
                otherScore += (minigameScore * 10);
            }
            otherScore = otherScore / pourMinigameScores.Count;
        }
        else
            otherScore = avgScore;//wtedy srednia nic nie zrobi
        float finalScore = (avgScore + otherScore) / 2f;
        finalScore *= ingridientScore;
        return (int)finalScore;
    }

    public static void AddMoney(int amount)
    {
        money += amount;
        updateMoney();
    }

    public static void EndDay()
    {
        MoneyManager.daysAmount = money;
        MoneyManager.fullAmount += money;
        SceneManager.LoadSceneAsync(2);
    }
}


