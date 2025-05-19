using UnityEngine;
using TMPro;

public class DisplayUi : MonoBehaviour
{
    
    public GameObject ingridients;
    public GameObject money;

    public static DisplayUi Instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        Debug.Log("BrewingMinigameManager Awake");
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void UpdateMoney()
    {
        money.GetComponentInChildren<TextMeshProUGUI>().text = GameManager.money.ToString();
    }

    public void UpdateIngridients()
    {
        ingridients.GetComponentInChildren<TextMeshProUGUI>().text = "Ingridients:";
        foreach (var item in GameManager.orderIngridients)
        {
            ingridients.GetComponentInChildren<TextMeshProUGUI>().text += "\n" + item.itemName;
        }
    }
}
