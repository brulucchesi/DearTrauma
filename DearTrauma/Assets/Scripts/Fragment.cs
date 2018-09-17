using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fragment : MonoBehaviour
{

    private GameObject analytics;

    [Header("References")]
    [Range(1, 4)]
    public int memoryNumber = 0;
    public Door LinkedDoor;
    public GameObject FragmentMemory;
    public GameObject FragmentVisual;

    [Header("Modifiers")]
    public string SceneName = "";
    public bool Boss = false;

    private void Start()
    {
        analytics = GameObject.Find("Analytics");

        FragmentMemory.SetActive(false);
        FragmentVisual.SetActive(true);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (LinkedDoor)
    //        {
    //            LinkedDoor.Unlock();
    //        }
    //        if (FragmentMemory)
    //        {
    //            Time.timeScale = 0f;
    //            FragmentMemory.SetActive(true);
    //        }
    //        if (!SceneName.Equals(""))
    //        {
    //            SceneManager.LoadScene(SceneName);
    //        }
    //        FragmentVisual.SetActive(false);
    //    }
    //}

    public void Collect()
    {
        analytics.GetComponent<UnityAnalyticsEvents>().EndLevel(memoryNumber);

        if (LinkedDoor)
        {
            LinkedDoor.Unlock();
        }
        if (FragmentMemory)
        {
           //Time.timeScale = 0f;
            FragmentMemory.SetActive(true);
        }
        if (!SceneName.Equals(""))
        {
            SceneManager.LoadScene(SceneName);
        }
        FragmentVisual.SetActive(false);
    }

    public void CloseMemory()
    {
        if (memoryNumber < 4)
        {
            analytics.GetComponent<UnityAnalyticsEvents>().StartLevel(memoryNumber + 1);
        }

        if (Boss)
        {
            Manager.GetInstance().GetComponent<Animator>().SetTrigger("end");
        }
        else
        {
            gameObject.SetActive(false);
        }
        //Time.timeScale = 1f;
    }
}
