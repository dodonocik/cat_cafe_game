using UnityEngine;

public class PourMinigameManager : MonoBehaviour
{
    public static PourMinigameManager Instance { get; private set; }
    public GameObject minigameObject;
    private static int score;
    static int destroyedObjects = 0;
    bool isActive = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive&&destroyedObjects==10)
            StopMinigame();
    }

    public void StartMinigame(Item item)
    {
        minigameObject.SetActive(true);
        destroyedObjects = 0;
        isActive = true;
        MinigameObjectFallControler.ResetGame();
        MinigameObjectFallControler.SetSprite(item);
    }


    public void StopMinigame()
    {
        score = PourMinigamePlayer.GetScore();
        minigameObject.SetActive(false);
        isActive = false;
        GameManager.pourMinigameScores.Add(score);
    }

    public static void CountDestroyedObject()
    {
        destroyedObjects++;
    }
}
