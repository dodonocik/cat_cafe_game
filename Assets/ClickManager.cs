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
        StartCoroutine(ChangeRoom(1, 0));
    }
    public void LeftButtonClick()
    {
        Debug.Log("back");
        StartCoroutine(ChangeRoom(0, 0));
    }

    public IEnumerator ChangeRoom(int roomNumber, float delay)
    {
        Color c = blackoutImage.color;
        blackoutImage.enabled = true;

        while (blackoutImage.color.a < 1) 
        {
            c.a += Time.deltaTime;
            blackoutImage.color = c;
        }

        rooms[activeRoom].SetActive(false);
        rooms[roomNumber].SetActive(true);
        activeRoom = roomNumber;

        while (blackoutImage.color.a > 1)
        {
            c.a -= Time.deltaTime;
            blackoutImage.color = c;
        }

        blackoutImage.enabled=false;

        yield return null;
    }
}
