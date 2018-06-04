using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour {

    [Header("References")]
    public Door LinkedDoor;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            LinkedDoor.Unlock();
            gameObject.SetActive(false);
        }
    }
}
