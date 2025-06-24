using UnityEngine;
using System.Collections;

public class BrewingMinigameManager : MonoBehaviour
{
    public static BrewingMinigameManager Instance { get; private set; }

    public ParticleSystem successParticles;

    public GameObject minigame;
    public GameObject minigamePointer;
    private static float minigamePointerPosY;
    static UnityEngine.Vector3 minigamePointerPos;
    static bool goingUp;
    static float score;
    static bool stopped = false;
   
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
        if (minigame.activeSelf && !stopped)
        {
            UpdateMinigame();
        }
    }

    public void StopMinigame()
    {
        stopped = true;
        float actualDistance = Mathf.Abs(minigamePointerPosY - 1.27f);
        float normalizedScore = Mathf.Clamp01(1f - (actualDistance / 5.2f));
        score = Mathf.RoundToInt(normalizedScore * 100);
        Debug.Log(minigamePointerPos);
        Debug.Log(score);

        if (score >= 90 && successParticles != null)
        {
            successParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            successParticles.Play();
            Debug.Log("playing particles");
            StartCoroutine(Delayed());
            return;
        }

        

        minigamePointerPos.y = 1.27f;
        minigamePointer.transform.localPosition = minigamePointerPos;
        minigame.SetActive(false);
    }

    public void StartMinigame()
    {
        stopped = false;
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
            minigamePointerPosY -= 10f*Time.deltaTime;
           
        }
        if (goingUp)
        {
            minigamePointerPosY += 10f*Time.deltaTime;
        }
        minigamePointerPos.y = minigamePointerPosY;
        minigamePointer.transform.localPosition = minigamePointerPos;
    }

    private IEnumerator Delayed()
    {
        yield return new WaitForSeconds(0.2f); // 300 ms delay
        minigamePointerPos.y = 1.27f;
        minigamePointer.transform.localPosition = minigamePointerPos;
        minigame.SetActive(false);
    }
    static public float getScore()
    {
        return score;
    }
}
