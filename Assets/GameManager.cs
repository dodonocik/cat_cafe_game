using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<string> currentOrder = new List<string>();
    
    public List<Item> allItems = new List<Item>();
    public static List<Item> orderIngridients = new List<Item>();
    public static Item selectedItem;
    public static bool minigameActive = false;
    public static List<int> pourMinigameScores = new List<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        minigameActive = false;
        Debug.Log(orderIngridients);
    }

    public static void StartPourMinigame()
    {
        PourMinigameManager.Instance.StartMinigame(selectedItem);
    }

  

    // Update is called once per frame
    void Update()
    {
    }

    
}


