﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Manager : MonoBehaviour {

    private GameObject analytics;

    [Header("References")]
    public GameObject Player;
    public GameObject FinalFragment;
    public GameObject Boss;

    static private Manager _instance;

    static public Manager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {

        analytics = GameObject.Find("Analytics");

        Observable.EveryUpdate().Where(_ => Input.anyKey &&
                                            (ScreenManager.GetInstance().CurrentScreen.Value ==
                                             ScreenManager.ScreenType.Start))
            .Subscribe(_ =>
            {
              ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game);
                if (analytics != null)
                {
                    analytics.GetComponent<UnityAnalyticsEvents>().StartLevel(1);

                }
            }
            );
        var escInput = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape));
        var anyInput = Observable.EveryUpdate().Where(_ => Input.anyKeyDown);
        escInput.Where(_=> (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Game))
                           .Subscribe(_ => ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Pause));
        anyInput.Where(_ => (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Credits))
                           .Subscribe(_ => ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.DemoEnd));
    }

    public void AnimationMiddle()
    {
        FinalFragment.SetActive(false);
        Boss.SetActive(true);
    }

    public void EndAnimation()
    {
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Credits);
    }
}
