using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public static IEnumerator FadeOutShelf(GameObject group, float delay)
    {
        CanvasGroup cg = group.GetComponent<CanvasGroup>();
        SpriteRenderer[] sprites = group.GetComponentsInChildren<SpriteRenderer>();
        Color color = new Color(1, 1, 1, 1);
        cg.interactable = false;
        cg.blocksRaycasts = false;

        while (color.a > 0)
        {
            color.a -= Time.deltaTime / delay;
            foreach (SpriteRenderer sprite in sprites)
            {
                sprite.color = color;
            }
            yield return null;
        }
        group.SetActive(false);
        yield return null;
    }

    public static IEnumerator FadeInShelf(GameObject group, float delay)
    {
        group.SetActive(true);
        CanvasGroup cg = group.GetComponent<CanvasGroup>();

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

    public static IEnumerator FadeOutFrame(GameObject frame, float delay)
    {
        Color color = new Color(1, 1, 1, 1);
        SpriteRenderer sr = frame.GetComponent<SpriteRenderer>();

        while (color.a > 0)
        {
            color.a -= Time.deltaTime / delay;

            sr.color = color;

            yield return null;
        }
        frame.SetActive(false);

        yield return null;
    }
    public static IEnumerator FadeInFrame(GameObject frame, float delay)
    {
        Color color = new Color(1, 1, 1, 0);
        frame.SetActive(true);
        SpriteRenderer sr = frame.GetComponent<SpriteRenderer>();

        while (color.a < 1)
        {
            color.a += Time.deltaTime / delay;

            sr.color = color;

            yield return null;
        }

        yield return null;
    }

}
