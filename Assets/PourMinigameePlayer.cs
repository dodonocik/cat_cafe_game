using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PourMinigameePlayer : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 10f;
    static int score;
    public TextMeshProUGUI scoreDisplay;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2 (horizontalInput*moveSpeed, rb.linearVelocity.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Falling Object"))
            score++;
        UpdateNametag();
    }
    public static int GetScore()
        { return score; }
    public void UpdateNametag()
    {
       scoreDisplay.text = "Score: " + score.ToString();
    }
}
