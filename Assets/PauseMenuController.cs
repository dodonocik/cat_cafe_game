using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    void Start()
    {
        menuCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuCanvas.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void Resume()
    {
        menuCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}
