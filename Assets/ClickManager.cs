using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    public Image blackoutImage;
    public GameObject[] rooms;
    public GameObject[] teabagFrames;
    public GameObject orderButtons;
    public GameObject teacup;
    public GameObject teacupBack;
    static ShelfManager shelfManager;
    int activeRoom = 0;
    bool switchedToExtras = false;
    bool stageTwoActive = false;
    float delay = 0.5f;
    public void RightButtonClick()
    {
        Debug.Log("test");
        StartCoroutine(ChangeRoom(activeRoom + 1, delay));
    }
    public void LeftButtonClick()
    {
        Debug.Log("back");
        int room = (activeRoom == 0) ? 0 : (activeRoom - 1);
        StartCoroutine(ChangeRoom(room, delay));
    }

    public void TeabagClick()
    {
        Debug.Log("click");
        if (stageTwoActive)
        {
            return;
        }
        if (GameManager.minigameActive)
        {
            GameManager.StopMinigame();
            SelectItem(GameManager.selectedItem);//unselect added item
            if (GameManager.orderIngridients.Count == 1)
            {
                shelfManager.setItemsList(GameManager.extraItems);
                shelfManager.resetIndex();
                shelfManager.displayItems();
                switchedToExtras = true;
                teabagFrames[1].SetActive(true);
                StartCoroutine(AnimationManager.FadeOutFrame(teabagFrames[0], delay));
                ShowOrderButtons();
            }
            if(GameManager.orderIngridients.Count == 2)
            {
                teabagFrames[2].SetActive(true);
                StartCoroutine (AnimationManager.FadeOutFrame(teabagFrames[1], delay));
            }
            
            return;
        }
        if (!GameManager.minigameActive)
        {
            if (GameManager.selectedItem != null)//start the game only if an item is selected
                GameManager.StartMinigame();
        }
    }

    public void TeacupClick()
    {
        if (!stageTwoActive || GameManager.selectedItem == null)
            return;
        GameManager.StartPourMinigame();
        ShowOrderButtons();
        SelectItem (GameManager.selectedItem);  
    }

    public void SelectItem(Item item)
    {
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

    public void ConfirmOrder()
    {
        HideOrderButtons();
        if(activeRoom == 1)
        {
            stageTwoActive = true;
        }
        if (activeRoom == 2)
        {
            LoopOrder();
        }
        
    }

    public void CancelOrder()
    {
        GameManager.orderIngridients.Clear();
        if (GameManager.selectedItem != null)
        {
            SelectItem(GameManager.selectedItem);//odklikowuje item
        }
        HideOrderButtons();
        switchedToExtras = false;
        stageTwoActive = false;

        switch (activeRoom)
        {
            case 1:
                shelfManager.resetIndex();
                shelfManager.setItemsList(GameManager.teaItems);
                shelfManager.displayItems();
                foreach (var frame in teabagFrames)
                {
                    frame.SetActive(true);
                    Color color = new Color(1, 1, 1, 1);
                    SpriteRenderer sr = frame.GetComponent<SpriteRenderer>();
                    sr.color = color;
                }
                break;
            case 2:
                teacup.SetActive(false);
                teacupBack.SetActive(false);
                break;
        }

        GameManager.ClearOrder();
        

    }

    public IEnumerator ChangeRoom(int roomNumber, float delay)
    {
        Color c = blackoutImage.color;
        blackoutImage.gameObject.SetActive(true);

        while (blackoutImage.color.a < 1)
        {
            c.a += Time.deltaTime / delay;
            blackoutImage.color = c;
            yield return null;
        }
        if(GameManager.selectedItem != null) 
            SelectItem(GameManager.selectedItem);
        SetUpRoom(roomNumber);

        while (blackoutImage.color.a > 0)
        {
            c.a -= Time.deltaTime / delay;
            blackoutImage.color = c;
            yield return null;
        }

        blackoutImage.gameObject.SetActive(false);

        yield return null;
    }
    private IEnumerator SwitchToExtras()
    {
        yield return null;
    }

    private IEnumerator SwitchPages(GameObject currentPage, GameObject desiredPage)
    {
            yield return StartCoroutine(AnimationManager.FadeOutShelf(currentPage, delay));
            yield return StartCoroutine(AnimationManager.FadeInShelf(desiredPage, delay));
        
    }

    private void ShowOrderButtons()
    {
        Transform buttons = rooms[activeRoom].transform.Find("Order Buttons");

        buttons.gameObject.SetActive(true);

    }
        
    private void HideOrderButtons()
    {
        Transform buttons = rooms[activeRoom].transform.Find("Order Buttons");
        buttons.gameObject.SetActive(false);
    }

    private IEnumerator SwitchBackToTeaShelf()
    {
        yield return null;
    }


    public void LoopOrder()
    {
        GameManager.updateMoney();
        CancelOrder();
    }

    private void SetUpRoom(int roomNumber)
    {
        rooms[activeRoom].SetActive(false);
        rooms[roomNumber].SetActive(true);
        activeRoom = roomNumber;

        switch (roomNumber)
        {
            case 0:
                break;
            case 1:

                shelfManager = rooms[activeRoom].GetComponent<ShelfManager>();
                shelfManager.setItemsList(GameManager.teaItems);
                if (switchedToExtras)
                {
                    shelfManager.setItemsList(GameManager.extraItems);
                }
                shelfManager.displayItems();
                break;
            case 2:
                shelfManager = rooms[activeRoom].GetComponent<ShelfManager>();
                shelfManager.setItemsList(GameManager.toppingItems);
                if (stageTwoActive)
                {
                    teacup.SetActive(true);
                    teacupBack.SetActive(true);
                    ShowOrderButtons();
                }
                break;
        }
    }

}
