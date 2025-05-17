using System;
using TMPro;
using UnityEngine;

public class MinigameObjectFallControler : MonoBehaviour
{
    float wait = 0.5f;
    public GameObject fallingObject;
    static Color spriteCol = Color.white;
    static Sprite sprite = null;
   static int spawnCount = 0;
    static bool reset = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Update()
    {
        if(reset)
        {
            InvokeRepeating("Fall", wait, wait);
            reset = false;
        }
    }

    // Update is called once per frame
    void Fall()
    {
        if (spawnCount >= 10)
        {
            CancelInvoke("Fall");
            return;
        }

        SpriteRenderer sr = fallingObject.GetComponent<SpriteRenderer>();

        if (sr != null&&sprite!=null) 
        {
            sr.sprite = sprite;
        }
            sr.color = spriteCol;

        Instantiate(fallingObject, new Vector3(UnityEngine.Random.Range(-5,5),5,10), Quaternion.identity);
        spawnCount++;

    }
    public static void SetSprite(Item item)
    {
        sprite = item.pourMinigameSprite;
        spriteCol = item.pourSpriteColor;
    }

    public static void ResetGame()
    {
        spawnCount = 0;
        PourMinigamePlayer.ResetGame();
        reset = true;
    }

   
}
