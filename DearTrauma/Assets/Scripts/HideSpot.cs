using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GetComponent<Animator>().SetBool("Hide", true);
            collision.GetComponent<Movement>().Hide(true);
            Physics2D.IgnoreLayerCollision(8, 10, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Animator>().SetBool("Hide", false);
            collision.GetComponent<Movement>().Hide(false);
            Physics2D.IgnoreLayerCollision(8, 10, false);
        }
    }
}
