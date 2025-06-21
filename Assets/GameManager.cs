using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    private List<string> currentOrder = new List<string>();
    public static int money = 0;
    public List<ItemObject> allItems;
    public static List<ItemObject> teaItems= new List<ItemObject>();
    public static List<ItemObject> toppingItems = new List<ItemObject>();
    public static List<ItemObject> extraItems = new List<ItemObject>();
    public static List<OrderIngredient> orderIngridients = new List<OrderIngredient>();
    public static Item selectedItem;
    public static bool minigameActive = false;
    public static List<int> pourMinigameScores = new List<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(var item in allItems)
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
        pourMinigameScores.Add(((int)BrewingMinigameManager.getScore()));
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
        foreach(var score in pourMinigameScores)
        {
            money += score;
        }

        DisplayUi.Instance.UpdateMoney();
    }
    
    public static void ClearOrder()
    {
        orderIngridients = new List<OrderIngredient>();
        pourMinigameScores.Clear();
        DisplayUi.Instance.UpdateIngridients();
    }
}


