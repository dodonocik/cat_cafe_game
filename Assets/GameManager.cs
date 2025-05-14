using NUnit.Framework.Constraints;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<string> currentOrder = new List<string>();
    public GameObject minigame;
    public GameObject minigamePointer;
    private float minigamePointerPosY;
    UnityEngine.Vector3 minigamePointerPos;
    bool goingUp;
    float score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        score = 1;
    }

    public void StartMinigame()
    {
        minigame.SetActive(true);
        goingUp = true;
        minigamePointerPos = minigamePointer.transform.localPosition;
        minigamePointerPosY = minigamePointerPos.y;
        Debug.Log(minigamePointerPos);
    }

    public void StopMinigame()
    {
        float actualDistance = Mathf.Abs(minigamePointerPosY - 1.27f);
        float normalizedScore = Mathf.Clamp01(1f - (actualDistance / 5.2f));
        score = Mathf.RoundToInt(normalizedScore * 100);
        Debug.Log(minigamePointerPos);
        Debug.Log(score);

        minigamePointerPos.y = 1.27f;
        minigamePointer.transform.localPosition = minigamePointerPos;
        minigame.SetActive(false);
    }

    private void UpdateMinigame()
    {
        if (minigamePointerPosY >= 6.50f)
        {
            goingUp = false;
        }
        if (minigamePointerPosY <= -3.9f)
        {
            goingUp = true;
        }
        if (!goingUp)
        {
            minigamePointerPosY -= 0.04f;
            minigamePointerPos.y = minigamePointerPosY;
            minigamePointer.transform.localPosition = minigamePointerPos;
        }
        if (goingUp)
        {
            minigamePointerPosY += 0.04f;
            minigamePointerPos.y = minigamePointerPosY;
            minigamePointer.transform.localPosition = minigamePointerPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (minigame.activeSelf)
        {
           UpdateMinigame();
        }
    }

    public void TeabagClick()
    {
        Debug.Log("click");
        if (minigame.activeSelf)
        {
            StopMinigame();
            return;
        }
        if(!minigame.activeSelf)
        {
            StartMinigame();
        }
    }
}


