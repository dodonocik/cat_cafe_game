using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    public Image blackoutImage;
    public GameObject[] rooms;
    int activeRoom = 0;
    public void RightButtonClick()
    {
        Debug.Log("test");
        StartCoroutine(ChangeRoom(1, 0.2f));
    }
    public void LeftButtonClick()
    {
        Debug.Log("back");
        StartCoroutine(ChangeRoom(0, 0.2f));
    }

    public void TeabagClick()
    {
        Debug.Log("click");
        if (GameManager.minigameActive)
        {
            GameManager.StopMinigame();
            GameManager.orderIngridients.Add(GameManager.selectedItem);
            return;
        }
        if (!GameManager.minigameActive)
        {
            Debug.Log("inactive");
            if (GameManager.selectedItem != null)
                GameManager.StartMinigame();
        }
    }

    public void SelectItem(Item item)
    {
        Debug.Log("old selected item: " + item.name);
        if (GameManager.selectedItem != null)
        {
            SpriteRenderer oldSr = GameManager.selectedItem.sprite.GetComponent<SpriteRenderer>();
            oldSr.color = Color.white;

            if (GameManager.selectedItem == item)
            {
                GameManager.selectedItem = null;//deselect current item
                Debug.Log("selected item: ");
                Debug.Log(GameManager.selectedItem);
                return;
            }
        }
        SpriteRenderer sr = item.sprite.GetComponent<SpriteRenderer>();
        GameManager.selectedItem = item;
       
        sr.color = new Color(0.769f, 0.600f, 0.718f, 1f);
        Debug.Log("selected item: " + item.name);
    }

    public IEnumerator ChangeRoom(int roomNumber, float delay)
    {
        Color c = blackoutImage.color;
        blackoutImage.gameObject.SetActive(true);

        while (blackoutImage.color.a < 1) 
        {
            c.a += Time.deltaTime/delay;
            blackoutImage.color = c;
            yield return null;
        }

        rooms[activeRoom].SetActive(false);
        rooms[roomNumber].SetActive(true);
        activeRoom = roomNumber;

        while (blackoutImage.color.a > 0)
        {
            c.a -= Time.deltaTime/delay;
            blackoutImage.color = c;
            yield return null;
        }

        blackoutImage.gameObject.SetActive(false);

        yield return null;
    }
}
