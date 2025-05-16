using TMPro;
using UnityEngine;

public class MinigameObjectFallControler : MonoBehaviour
{
    float wait = 1f;
    public GameObject fallingObject;
    static Color spriteCol = Color.white;
    static Sprite sprite = null;
   static int spawnCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("Fall", wait, wait);
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

        Instantiate(fallingObject, new Vector3(Random.Range(-5,5),5,10), Quaternion.identity);
        spawnCount++;

    }
    public static void ResetSpawnCount()
    {
        spawnCount = 0;
    }
    public static void SetSprite(Item item)
    {
        sprite = item.pourMinigameSprite;
        spriteCol = item.pourSpriteColor;
    }
  
}
