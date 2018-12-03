using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class Fragment : MonoBehaviour
{
    [Header("References")]
    [Range(1, 4)]
    public int levelNumber = 0;
    public Door LinkedDoor;
    public GameObject FragmentMemory;
    public GameObject FragmentVisual;
    public GameObject CanCloseVisual;
    public Text TextCanClose;
    public AudioSource Audio;

    [Header("Modifiers")]
    public string SceneName = "";
    public bool Boss = false;
    public float TimeToClose = 2f;

    private GameObject analytics;
    private bool canClose;

    private GameObject musicManager;
    private GameObject playerGameObject;

    private void Start()
    {
        musicManager = GameObject.Find("MusicManager");
        canClose = false;
        analytics = GameObject.Find("Analytics");

        FragmentMemory.SetActive(false);
        FragmentVisual.SetActive(true);

        Observable.EveryUpdate().Where(_ => this && Input.anyKeyDown).Subscribe(_ =>
        {
            if (canClose)
            {
                CloseMemory();
                canClose = false;
            }
        });
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

    public void Collect(GameObject player)
    {
        analytics.GetComponent<UnityAnalyticsEvents>().EndLevel(levelNumber);

        if (levelNumber == 3)
        {
            gameObject.SetActive(false);
        }
        FragmentVisual.SetActive(false);

        if (FragmentMemory && levelNumber != 3)
        {
            Audio.Play();

            Manager.GetInstance().GetComponent<Animator>().SetTrigger("fade");

            player.GetComponent<Movement>().SetCanMove(false);

            StartCoroutine(WaitFadeFragmentIn(player));
        }
    }

    IEnumerator WaitFadeFragmentIn(GameObject player)
    {
        yield return new WaitUntil(() => Manager.GetInstance().FadeMiddle);

        if (LinkedDoor)
        {
            LinkedDoor.Unlock();
        }

        if (FragmentMemory && levelNumber != 3)
        {
            //Time.timeScale = 0f;
            FragmentMemory.SetActive(true);
            playerGameObject = player;
            player.GetComponent<Movement>().SetCanMove(false);
            ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Fragment);

            Observable.Timer(System.TimeSpan.FromSeconds(TimeToClose)).Subscribe(_ =>
            {
                canClose = true;
                CanCloseVisual.SetActive(true);
            }
            );
        }
        if (!SceneName.Equals(""))
        {
            SceneManager.LoadScene(SceneName);
        }
    }

    private void Update()
    {
        if (canClose)
        {
            TextCanClose.color = new Color(TextCanClose.color.r, TextCanClose.color.g, TextCanClose.color.b, Mathf.PingPong(Time.time, 2));
        }
    }

    public void CloseMemory()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(0.2f)).Subscribe(_ =>
        {
            ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game);
        }
        );

        if (levelNumber < 4)
        {
            analytics.GetComponent<UnityAnalyticsEvents>().StartLevel(levelNumber + 1);
        }

        if (Boss)
        {
            musicManager.GetComponent<MusicManager>().ChangeBoss();
            Manager.GetInstance().GetComponent<Animator>().SetTrigger("end");
            playerGameObject.GetComponent<Animator>().SetBool("Walking", false);
        }
        else
        {
            if (levelNumber != 3)
            {
                Manager.GetInstance().GetComponent<Animator>().SetTrigger("fade");

                StartCoroutine(WaitFadeFragmentOut());
            }
            else
            {
                playerGameObject.GetComponent<Movement>().SetCanMove(true);
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator WaitFadeFragmentOut()
    {
        yield return new WaitUntil(() => Manager.GetInstance().FadeMiddle);

        playerGameObject.GetComponent<Movement>().SetCanMove(true);
        gameObject.SetActive(false);
        if (levelNumber == 2)
        {
            playerGameObject.GetComponent<Evolution>().InitiateTransform();
        }
    }
}
