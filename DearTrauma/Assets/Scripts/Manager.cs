﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Playables;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [Header("References")]
    public GameObject Player;
    public AudioSource ButtonClick;
    public AudioSource NormalClick;

    [Header("Boss References")]
    public GameObject FinalFragment;
    public GameObject Boss;
    public PlayableDirector BossPlayableDirector;
    public AudioSource BossTransition;
    public AudioSource BossRoar;
    public AudioClip EmptySound;

    [Header("Modifiers")]
    public float BossSeconds = 0f;

    [HideInInspector]
    public bool Paused;

    [HideInInspector]
    public bool BossActive;

    [HideInInspector]
    public BoolReactiveProperty Restarted;

    [HideInInspector]
    public bool FadeMiddle = false;

    public IObservable<long> AnyInput;

    private GameObject analytics;

    private GameObject musicManager;

    private GameObject lastselect;

    static private Manager _instance;

    static public Manager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _instance = this;
    }

    public void VoltaPlayer()
    {
        //Debug.Log("voltaaaa");
        Player.GetComponent<Movement>().SetCanMove(true);
    }

    public void TerminaCreditos()
    {
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.DemoEnd);
    }

    private void Start()
    {
        Restarted.Value = false;
        BossActive = false;
        bool started = false;

        lastselect = new GameObject();

        musicManager = GameObject.Find("MusicManager");

        analytics = GameObject.Find("Analytics");

        Observable.EveryUpdate().Where(_ => this && Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !started &&
                                            (ScreenManager.GetInstance().CurrentScreen.Value ==
                                             ScreenManager.ScreenType.Start))
            .Subscribe(_ =>
                {
                    started = true;
                    ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Intro);
                }
            );

        AnyInput = Observable.EveryUpdate().Where(_ => Input.anyKeyDown);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastselect);
        }
        else
        {
            lastselect = EventSystem.current.currentSelectedGameObject;
        }

        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Button>() != null)
        //    {
        //        ButtonClick.Play();
        //    }
        //    else
        //    {
        //        NormalClick.Play();
        //    }
        //}
    }

    public void EndIntro()
    {
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game);
        // Player.GetComponent<Movement>().SetCanMove(false);

        if (musicManager)
        {
            musicManager.GetComponent<MusicManager>().ChangeGamePlay();
        }

        if (analytics != null)
        {
            analytics.GetComponent<UnityAnalyticsEvents>().StartLevel(1);
        }
    }

    public void AnimationMiddle()
    {
        Player.GetComponent<Movement>().SetCanMove(false);

        FinalFragment.SetActive(false);
        Boss.SetActive(true);

        BossTransition.clip = EmptySound;
        BossTransition.Play();

        BossRoar.clip = EmptySound;
        BossRoar.Play();

        BossPlayableDirector.Play();
    }

    public void EndAnimation()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(BossSeconds)).Subscribe(_ =>
        {
            GetComponent<Animator>().SetTrigger("credits");
        }
        );
    }

    public void EndBoss()
    {
        musicManager.GetComponent<MusicManager>().ChangeCredits();
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Credits);
    }

    public void MiddleFade()
    {
        FadeMiddle = true;
    }

    public void EndFade()
    {
        FadeMiddle = false;
    }

    public void Restart()
    {
        Restarted.Value = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayButtonClick()
    {
        ButtonClick.Play();
    }

    public void PlayNormalClick()
    {
        NormalClick.Play();
    }
}
