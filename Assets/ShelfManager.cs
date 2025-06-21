using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ShelfManager : MonoBehaviour
{
    public GameObject[] shelfItems;
    public GameObject nextButton;
    public GameObject prevButton;
    private  List<ItemObject> items = new List<ItemObject>();
    private static int startIndex = 0;
    void Start()
    {
        
    }

    public  void setItemsList(List<ItemObject> newItems)
    {
        items = newItems;
    }

    public void displayItems()
    {
        int index = startIndex;
        foreach(var shelfItem in shelfItems)
        {
            shelfItem.SetActive(true);
            Item item = shelfItem.GetComponent<Item>();
            item.sprite.SetActive(true);
            if (index<items.Count)
            {    
                item.setItemObject(items[index]);
            }
            else
            {
                item.sprite.SetActive(false);
                shelfItem.SetActive(false);
            }
            index++;
        }
    }

    public void nextPage()
    {

        startIndex += shelfItems.Length;

        if (startIndex >= items.Count)
        {
            startIndex = 0;
        }
        Debug.Log("next " + startIndex);

        displayItems();
    }

    public void previousPage()
    {
        startIndex -= shelfItems.Length;

        if (startIndex < 0)
        {
            int fullPages = items.Count / shelfItems.Length;
            int remainder = items.Count % shelfItems.Length;

            if (remainder == 0)
            {
                startIndex = (fullPages - 1) * shelfItems.Length;
            }
            else
            {
                startIndex = fullPages * shelfItems.Length;
            }
        }
        Debug.Log("prev " + startIndex);
        displayItems();

    }

    public void resetIndex()
    {
        startIndex = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
