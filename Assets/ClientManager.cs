using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using System.Collections;
using UnityEngine.UI;

public class ClientManager : MonoBehaviour
{
    public List<Client> allClients;
    private int currentClientId;
    public GameObject clientSprite;
    public GameObject clientClickbox;
    public GameObject clientTextbox;
    public GameObject clientNametag;
    private static List<RuntimeClient> clientsData = new List<RuntimeClient>();
    private int paid;
    private int served_clients_count = 0;
    private enum OrderStage
    {
        Pending,
        Placed,
        Finished,
        Evaluated
    }
    private OrderStage currentStage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PickClient();
        currentStage = OrderStage.Pending;
        served_clients_count = 0;
        if(clientsData.Count == 0 )
        foreach (var client in allClients)
        {
            clientsData.Add(new RuntimeClient
            {
                client = client,
                tip_score = client.tip_score
            });
        }
    }

    public void PickClient()
    {
        int previosClientId = currentClientId;
        do {
            currentClientId = Random.Range(0, allClients.Count);
        }while(currentClientId == previosClientId);
 
        Sprite newSprite = allClients[currentClientId].sprite;
        StartCoroutine(SwapFrames(newSprite, 0.5f));
        DisplayNametag();
    }

    public void CreateOrder()
    {
        GameManager.FillOrder();
    }

    private string EvaluateOrder()
    {
        string text = " ";
        float tip = 1f;
        float newTip = clientsData[currentClientId].tip_score;
        int finalScore = GameManager.EvaluateScore();
        if (finalScore > 0)
        {
            newTip -= 0.1f;
            text = "Oh! I don't like this...";
            if (finalScore > 25)
                text = "It's fine.";

                if (finalScore > 50)
                {
                    tip = clientsData[currentClientId].tip_score;
                    text = "This is a good tea.";
                }
                if (finalScore > 75)
                    text = "This is a really good tea!";
                if (finalScore > 90)
                {
                    text = "I love this! :3";
                    newTip += 0.2f;
                }
                if (finalScore == 100)
                    text = "This is the best tea I've ever had!";
        }
        else
            text = "This is not what i wanted!";
        paid = (int)(finalScore*tip);
        clientsData[currentClientId].tip_score = newTip;
        return text;
    }

    public void DisplayNametag()
    {
        string name = allClients[currentClientId].name;
        clientNametag.GetComponentInChildren<TextMeshProUGUI>().text = name;
    }

    public void UpdateTextbox()
    {
        TextMeshProUGUI text = clientTextbox.GetComponentInChildren<TextMeshProUGUI>();
        switch (currentStage)
        {
            case OrderStage.Pending:
                text.text = "I'd like to place an order! :3";
                break;
            case OrderStage.Placed:
                text.text = "Please make me a good tea with: "+GetOrderDescription(GameManager.GetCurrentOrder());
                break;
            case OrderStage.Finished:
                text.text = EvaluateOrder();
                break;
            case OrderStage.Evaluated:
                text.text = Money();
                break;
        }
    }

    public void ManageClick()
    {
            
        switch (currentStage)
        {
            case OrderStage.Pending:
                currentStage = OrderStage.Placed;
                CreateOrder();
                UpdateTextbox();
                ClickManager.ResetOrderSpace();
                break;
            case OrderStage.Placed:
                if (OrderManager.ready)
                {
                    currentStage = OrderStage.Finished;
                    UpdateTextbox();
                }
                break;
            case OrderStage.Finished:
                currentStage = OrderStage.Evaluated;
                UpdateTextbox();
                GameManager.AddMoney(paid);               
                break;
            case OrderStage.Evaluated:
                if(served_clients_count == 4)
                {
                    GameManager.EndDay();
                    return;
                }
                PickClient();
                currentStage = OrderStage.Pending;
                UpdateTextbox();
                GameManager.ClearOrder();
                served_clients_count++;
                break;
        }
    }

    public static string GetOrderDescription(List<OrderIngredient> order)
    { 

        List<string> names = new List<string>();
        foreach (OrderIngredient ingredient in order)
        {
            names.Add(ingredient.name);
        }

        if (order.Count == 1)
            return names[0]+".";

        string description = "";
        for(int i = 0; i < names.Count; i++)
        {
            description += names[i];
            if (i == names.Count - 1)
            {
                description += ".";
                continue;
            }
            if(i==names.Count - 2)
                description += " and ";
            else description += ", ";

        }
        return description;
    }

    private string Money()
    {
        string text;

        if (paid == 0)
        {
            text = "I'm not paying you for this!";
        }
        else
            text = "Here's your payment\nClient paid: "+paid.ToString()+".";

        return text;
    }

    private IEnumerator SwapFrames(Sprite newFrame, float delay)
    {
        yield return StartCoroutine(AnimationManager.FadeOutCat(clientSprite.GetComponent<SpriteRenderer>(), delay));
        clientSprite.GetComponent<SpriteRenderer>().sprite = newFrame;
        yield return StartCoroutine(AnimationManager.FadeInCat(clientSprite.GetComponent<SpriteRenderer>(), delay));

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
