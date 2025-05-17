using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour
{
    public Image blackoutImage;
    public GameObject[] rooms;
    public GameObject[] teaShelves;
    public GameObject[] extrasShelves;
    public GameObject[] addInShelves;
    public GameObject[] teabagFrames;
    public GameObject orderButtons;
    public GameObject teacup;
    public GameObject teacupBack;
    int activeRoom = 0;
    int activeShelf = 0;
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

    public void NextPage()
    {
        if (GameManager.selectedItem != null)
            SelectItem(GameManager.selectedItem);
        int desiredShelf=0;
        switch (activeRoom)
        {
            case 1:
                var shelves = switchedToExtras ? extrasShelves : teaShelves;
                desiredShelf = (shelves.Length - 1 == activeShelf) ? 0 : (activeShelf + 1);
                StartCoroutine(SwitchPages(shelves[activeShelf], shelves[desiredShelf]));
                break;

            case 2:
                desiredShelf = (addInShelves.Length - 1 == activeShelf) ? 0 : (activeShelf + 1);
                StartCoroutine(SwitchPages(addInShelves[activeShelf], addInShelves[desiredShelf]));
                break;

        }
        activeShelf = desiredShelf;
        
    }

    public void PreviousPage()
    {
        if (GameManager.selectedItem != null)
            SelectItem(GameManager.selectedItem);
        int desiredShelf = 0;
        switch (activeRoom) {
            case 1:
                var shelves = switchedToExtras ? extrasShelves : teaShelves;
                desiredShelf = (activeShelf == 0) ? (shelves.Length - 1) : (activeShelf - 1);
                StartCoroutine(SwitchPages(shelves[activeShelf], shelves[desiredShelf]));
                break;
            case 2:
                desiredShelf = (activeShelf==0) ? (addInShelves.Length-1) : (activeShelf - 1);
                StartCoroutine(SwitchPages(addInShelves[activeShelf], addInShelves[desiredShelf]));
                break;

        }
        activeShelf = desiredShelf;
    }

    public void TeabagClick()
    {
        Debug.Log("click");
        if (GameManager.minigameActive)
        {
            GameManager.StopMinigame();
            GameManager.orderIngridients.Add(GameManager.selectedItem);
            SelectItem(GameManager.selectedItem);//unselect added item
            if (GameManager.orderIngridients.Count == 1)
            {
                StartCoroutine(SwitchToExtras());
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
        GameManager.orderIngridients.Add(GameManager.selectedItem);//to se gm powinien robic sam, do zmiany zaraz po poprawieniu shelfow
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
        if (GameManager.selectedItem != null)
        {
            SelectItem(GameManager.selectedItem);//odklikowuje item
        }
        switch (activeRoom)
        {
            case 1:
                StartCoroutine(AnimationManager.FadeInFrame(teabagFrames[0], delay));
                StartCoroutine(AnimationManager.FadeOutShelf(extrasShelves[activeShelf],delay));//jesli liczba skladnikow == 1 to automatycznie ustawia sie extras shelf, nie trzeba tego sprawdzac tu
                stageTwoActive = true;
                break;
            case 2:
                StartCoroutine(AnimationManager.FadeOutFrame(teacup, delay));
                StartCoroutine(AnimationManager.FadeOutShelf(addInShelves[activeShelf], delay));
                stageTwoActive=false;
                teacupBack.SetActive(false);
                break;

        }
        
        activeShelf = 0;
        
    }

    public void CancelOrder()
    {
        GameManager.orderIngridients.Clear();
        if (GameManager.selectedItem != null)
        {
            SelectItem(GameManager.selectedItem);//odklikowuje item
        }
        HideOrderButtons();
        
        switch(activeRoom)
        {
            case 1:
                if (switchedToExtras)
                {
                    StartCoroutine(SwitchBackToTeaShelf());
                }
                StartCoroutine(AnimationManager.FadeInFrame(teabagFrames[0],delay));
                break;
            case 2:
                StartCoroutine(AnimationManager.FadeOutFrame(teacup,delay));
                teacupBack.SetActive(false);
                stageTwoActive = false;
                break;
        }

        

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

        rooms[activeRoom].SetActive(false);
        rooms[roomNumber].SetActive(true);
        activeRoom = roomNumber;
        SceneOnLoad();

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
        yield return StartCoroutine(AnimationManager.FadeOutShelf(teaShelves[activeShelf],delay));
        activeShelf = 0;
        switchedToExtras = true;
        yield return StartCoroutine(AnimationManager.FadeInShelf(extrasShelves[activeShelf], delay));
        
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
        buttons.gameObject.SetActive(false );
    }

    private IEnumerator SwitchBackToTeaShelf()
    {
        yield return StartCoroutine(AnimationManager.FadeOutShelf(extrasShelves[activeShelf], delay));

        activeShelf = 0;
        switchedToExtras = false;

        yield return StartCoroutine(AnimationManager.FadeInShelf(teaShelves[activeShelf], delay));
    }

    private void SceneOnLoad()
    {
        if (GameManager.selectedItem != null)
            SelectItem(GameManager.selectedItem);
        switch (activeRoom)
        {
            case 0:
                break;
            case 1:
                if (GameManager.orderIngridients.Count == 0)
                {
                    activeShelf = 0;
                    stageTwoActive = false;
                    switchedToExtras = false;
                    StartCoroutine(SwitchBackToTeaShelf());
                }
                break;
            case 2:
                if (stageTwoActive)
                {
                    StartCoroutine(AnimationManager.FadeInFrame(teacup, delay));
                    teacupBack.SetActive(true);
                }
                

                break;
        }

    }

}
