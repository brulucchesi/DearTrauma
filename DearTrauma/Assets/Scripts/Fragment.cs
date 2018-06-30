using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fragment : MonoBehaviour
{

    [Header("References")]
    public Door LinkedDoor;
    public GameObject FragmentMemory;
    public GameObject FragmentVisual;

    [Header("Modifiers")]
    public string SceneName = "";

    private void Start()
    {
        FragmentMemory.SetActive(false);
        FragmentVisual.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (LinkedDoor)
            {
                LinkedDoor.Unlock();
            }
            if (FragmentMemory)
            {
                Time.timeScale = 0f;
                FragmentMemory.SetActive(true);
            }
            if (!SceneName.Equals(""))
            {
                SceneManager.LoadScene(SceneName);
            }
            FragmentVisual.SetActive(false);
        }
    }

    public void CloseMemory()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
