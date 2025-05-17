using System;
using UnityEngine;

public class FallingObject : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Falling Object"))//niech falling objecty nie usuwaja samych siebie
        {
            Destroy(this.gameObject);
           PourMinigameManager.CountDestroyedObject();
        }
    }
}
