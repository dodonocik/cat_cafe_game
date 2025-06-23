using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class OrderManager
{
    private static List<OrderIngredient> teas = new List<OrderIngredient>();
    private static List<OrderIngredient> extras = new List<OrderIngredient>();
    private static List<OrderIngredient> toppings = new List<OrderIngredient>();
    public static bool ready = false;
    public static void PrepareLists()
    {
        
        foreach (var tea in GameManager.teaItems) {
            OrderIngredient item = new OrderIngredient(tea);
            teas.Add(item);
        }
        foreach (var extra in GameManager.extraItems) { 
            OrderIngredient orderIngredient = new OrderIngredient(extra);
            extras.Add(orderIngredient);
        }
        foreach (var topping in GameManager.toppingItems)
        {
            OrderIngredient item = new OrderIngredient(topping);
            toppings.Add(item);
        }
    }

    public static List<OrderIngredient> GenerateOrder()
    {
        ready = false;
        List<OrderIngredient> orderIngredients = new List<OrderIngredient>();
        int index  = Random.Range(0, teas.Count);
        orderIngredients.Add(teas[index]);
        int amount = Random.Range(0, 2);
        for(int i = 0; i < amount; i++)
        {
            OrderIngredient extra;
            do
            {
                index = Random.Range(0, extras.Count);
                extra = extras[index];
            } while (orderIngredients.Contains(extra));
            orderIngredients.Add(extras[index]);
        }
        amount = Random.Range(0, 3);
        for(int i = 0; i < amount; i++)
        {   
            OrderIngredient topping;
            do
            {
                index = Random.Range(0, toppings.Count);
                topping = toppings[index];
            } while (orderIngredients.Contains(topping));
            orderIngredients.Add(toppings[index]);
        }
        return orderIngredients;
    }

    public static float EvaluateOrder(List<OrderIngredient> expectedOrder, List<OrderIngredient> actualOrder)
    {
        int correctIngridients = 0;
        foreach (var expectedIngridient in expectedOrder)
        {
            if (actualOrder.Any(actual => actual.id == expectedIngridient.id))
                correctIngridients++;
        }
        return (float)correctIngridients / expectedOrder.Count; ;
    }
}
