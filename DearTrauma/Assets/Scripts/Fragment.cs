using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fragment : MonoBehaviour {

    [Header("References")]
    public Door LinkedDoor;
    public GameObject FragmentMemoryVisual;

    [Header("Modifiers")]
    public string SceneName = "";

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
            if (!SceneName.Equals(""))
            {
                SceneManager.LoadScene(SceneName);
            }
            gameObject.SetActive(false);
        }
    }
}
