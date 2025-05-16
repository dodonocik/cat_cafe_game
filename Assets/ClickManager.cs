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
    public GameObject[] teabagFrames;
    public GameObject orderButtons;
    int activeRoom = 0;
    int activeShelf = 0;
    bool switchedToExtras = false;
    float delay = 0.5f;
    public void RightButtonClick()
    {
        Debug.Log("test");
        StartCoroutine(ChangeRoom(1, delay));
    }
    public void LeftButtonClick()
    {
        Debug.Log("back");
        StartCoroutine(ChangeRoom(0, delay));
    }

    public void NextPage()
    {
        if(GameManager.selectedItem = null)
            SelectItem(GameManager.selectedItem);
        int desiredShelf;
        if (switchedToExtras)
        {
            if (extrasShelves.Length - 1 == activeShelf)
            {
                desiredShelf = 0;
            }
            else
                desiredShelf = activeShelf + 1;
        }
        else
        {
            if(teaShelves.Length - 1 == activeShelf)
            {
                desiredShelf = 0;
            }
            else
                desiredShelf = activeShelf + 1;
        }
        StartCoroutine(SwitchPages(desiredShelf));
    }

    public void PreviousPage()
    {
        if (GameManager.selectedItem = null)
            SelectItem(GameManager.selectedItem);
        int desiredShelf;
        if (switchedToExtras)
        {
            if (activeShelf == 0)
            {
                desiredShelf = extrasShelves.Length - 1;
            }
            else
                desiredShelf = activeShelf - 1;
        }
        else
        {
            if (activeShelf == 0)
            {
                desiredShelf = teaShelves.Length - 1; ;
            }
            else
                desiredShelf = activeShelf - 1;
        }
        StartCoroutine(SwitchPages(desiredShelf));
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
                StartCoroutine(FadeTeabagFrame(0));
                ShowOrderButtons();
            }
            if(GameManager.orderIngridients.Count == 2)
            {
                teabagFrames[2].SetActive(true);
                StartCoroutine (FadeTeabagFrame(1));
            }
            
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
        if (switchedToExtras)
        {
            StartCoroutine(FadeOutShelf(extrasShelves[activeShelf], delay));
            StartCoroutine(FadeTeabagFrame(2));
        }
        else
        {
            StartCoroutine(FadeOutShelf(teaShelves[activeShelf], delay));
            StartCoroutine(FadeTeabagFrame(1));
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
        if(switchedToExtras)
        {
            StartCoroutine(SwitchBackToTeaShelf());
        }
        StartCoroutine(FadeInTeabag());

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

    private IEnumerator SwitchPages(int desiredPage)
    {
        if(switchedToExtras)
        {
            yield return StartCoroutine(FadeOutShelf(extrasShelves[activeShelf],delay));
            activeShelf = desiredPage;
            yield return StartCoroutine(FadeInShelf(extrasShelves[activeShelf], delay));
        }
        else
        {
            yield return StartCoroutine(FadeOutShelf(teaShelves[activeShelf], delay));
            activeShelf = desiredPage;
            yield return StartCoroutine(FadeInShelf(teaShelves[activeShelf], delay));
        }
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

        while (color.a < 1)
        {
            color.a -= Time.deltaTime / delay;
            foreach (SpriteRenderer sprite in sprites)
            {               
                sprite.color = color;               
            }
            yield return null;
        }

        if(switchedToExtras)
        {
            extrasShelves[activeShelf].SetActive(false);
        }
        else
            teaShelves[activeShelf].SetActive(false);

        yield return null;
    }

    private IEnumerator FadeInShelf(GameObject group, float delay)
    {
        CanvasGroup cg = group.GetComponent<CanvasGroup>();
        if(switchedToExtras)
            extrasShelves[activeShelf].SetActive(true);
        else
            teaShelves[activeShelf].SetActive(true);
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

    private IEnumerator FadeTeabagFrame(int frame)
    {
        Color color = new Color(1, 1, 1, 1);
       SpriteRenderer sr = teabagFrames[frame].GetComponent<SpriteRenderer>();

        while (color.a > 0)
        {
            color.a -= Time.deltaTime / delay;

            sr.color = color;
            
            yield return null;
        }
        teabagFrames[frame].SetActive(false);

        yield return null;
    }

    private IEnumerator FadeInTeabag()
    {
        Color color = new Color(1, 1, 1, 0);
        teabagFrames[0].SetActive(true);

        SpriteRenderer sr = teabagFrames[0].GetComponent<SpriteRenderer>();
        while (color.a < 1)
            {
                color.a += Time.deltaTime / delay;

                sr.color = color;

                yield return null;
            }
        foreach (var frame in teabagFrames)
        {
            SpriteRenderer spr = frame.GetComponent<SpriteRenderer>();
            spr.color = new Color(1, 1, 1, 1);

        }


        yield return null;
    }
}
