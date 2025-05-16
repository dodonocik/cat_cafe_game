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
    int activeRoom = 0;
    int activeShelf = 0;
    bool switchedToExtras = false;
    bool stageOneComplete = false;
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
                StartCoroutine(FadeOutFrame(teabagFrames[0]));
                ShowOrderButtons();
            }
            if(GameManager.orderIngridients.Count == 2)
            {
                teabagFrames[2].SetActive(true);
                StartCoroutine (FadeOutFrame(teabagFrames[1]));
            }
            
            return;
        }
        if (!GameManager.minigameActive)
        {
            if (GameManager.selectedItem != null)//start the game only if an item is selected
                GameManager.StartMinigame();
        }
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
                StartCoroutine(FadeInFrame(teabagFrames[0]));
                StartCoroutine(FadeOutShelf(
                    GameManager.orderIngridients.Count > 1
                    ? extrasShelves[activeShelf]: teaShelves[activeShelf],delay));
                stageOneComplete = true;
                break;
            case 2:
                //wylacza teacup
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
                StartCoroutine(FadeInFrame(teabagFrames[0]));
                break;
            case 2:
                stageOneComplete = false;
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
        yield return StartCoroutine(FadeOutShelf(teaShelves[activeShelf],delay));
        activeShelf = 0;
        switchedToExtras = true;
        yield return StartCoroutine(FadeInShelf(extrasShelves[activeShelf], delay));
        
    }

    private IEnumerator SwitchPages(GameObject currentPage, GameObject desiredPage)
    {
            yield return StartCoroutine(FadeOutShelf(currentPage, delay));
            yield return StartCoroutine(FadeInShelf(desiredPage, delay));
        
    }

    private void ShowOrderButtons()
    {
        orderButtons.SetActive(true);
    }
    private void HideOrderButtons()
    {
        orderButtons.SetActive(false );
    }

    private IEnumerator FadeOutShelf(GameObject group, float delay)
    {
        CanvasGroup cg = group.GetComponent<CanvasGroup>();
        SpriteRenderer[] sprites = group.GetComponentsInChildren<SpriteRenderer>();
        Color color = new Color(1, 1, 1, 1);
        cg.interactable = false;
        cg.blocksRaycasts = false;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime / delay;
            foreach (SpriteRenderer sprite in sprites)
            {               
                sprite.color = color;               
            }
            yield return null;
        }
        group.SetActive(false);
        yield return null;
    }

    private IEnumerator FadeInShelf(GameObject group, float delay)
    {
        group.SetActive(true);
        CanvasGroup cg = group.GetComponent<CanvasGroup>();
       
        SpriteRenderer[] sprites = group.GetComponentsInChildren<SpriteRenderer>();
        Color color = new Color(1, 1, 1, 0);
        cg.interactable = false;
        cg.blocksRaycasts = false;

        while (color.a < 1)
        {
            color.a += Time.deltaTime / delay;
            foreach (SpriteRenderer sprite in sprites)
            {               
                sprite.color = color;        
            }
            yield return null;
        }

        cg.interactable = true;
        cg.blocksRaycasts = true;

        yield return null;
    }

    private IEnumerator SwitchBackToTeaShelf()
    {
        yield return StartCoroutine(FadeOutShelf(extrasShelves[activeShelf], delay));

        activeShelf = 0;
        switchedToExtras = false;

        yield return StartCoroutine(FadeInShelf(teaShelves[activeShelf], delay));
    }

    private IEnumerator FadeOutFrame(GameObject frame)
    {
        Color color = new Color(1, 1, 1, 1);
       SpriteRenderer sr = frame.GetComponent<SpriteRenderer>();

        while (color.a > 0)
        {
            color.a -= Time.deltaTime / delay;

            sr.color = color;
            
            yield return null;
        }
        frame.SetActive(false);

        yield return null;
    }
    private IEnumerator FadeInFrame(GameObject frame)
    {
        Color color = new Color(1, 1, 1, 0);
        frame.SetActive(true);
        SpriteRenderer sr = frame.GetComponent<SpriteRenderer>();

        while (color.a < 1)
        {
            color.a += Time.deltaTime / delay;

            sr.color = color;

            yield return null;
        }

        yield return null;
    }

}
