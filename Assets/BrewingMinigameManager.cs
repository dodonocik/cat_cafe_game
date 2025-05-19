using UnityEngine;

public class BrewingMinigameManager : MonoBehaviour
{
    public static BrewingMinigameManager Instance { get; private set; }

    public GameObject minigame;
    public GameObject minigamePointer;
    private static float minigamePointerPosY;
    static UnityEngine.Vector3 minigamePointerPos;
    static bool goingUp;
    static float score;
   
    void Awake()
    {
        Debug.Log("BrewingMinigameManager Awake");
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject); 
    }
    void Start()
    {
        score = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (minigame.activeSelf)
        {
            UpdateMinigame();
        }
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

    public void StartMinigame()
    {
        score = 0;
        minigame.SetActive(true);
        goingUp = true;
        minigamePointerPos = minigamePointer.transform.localPosition;
        minigamePointerPosY = minigamePointerPos.y;
        Debug.Log(minigamePointerPos);
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

    static public float getScore()
    {
        return score;
    }
}
