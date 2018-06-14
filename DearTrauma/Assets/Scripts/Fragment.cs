using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour {

    [Header("References")]
    public Door LinkedDoor;
    public GameObject FragmentMemoryVisual;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if(LinkedDoor)
            {
                LinkedDoor.Unlock();
            }
            if (FragmentMemoryVisual)
            {
                FragmentMemoryVisual.SetActive(true);
            }
            if (LinkedDoor)
            {
                LinkedDoor.Unlock();
            }
            gameObject.SetActive(false);
        }
    }
}
