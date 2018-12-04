using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ScreenManager : MonoBehaviour
{

    private GameObject analytics;
    private int helpCount = 0;

    public enum ScreenType
    {
        Start,
        Game,
        Pause,
        Settings,
        Credits,
        Controls,
        DemoEnd,
        Quit,
        None,
        Fragment,
        Intro
    }

    public ReactiveProperty<ScreenType> CurrentScreen = new ReactiveProperty<ScreenType>();
    public ReactiveProperty<ScreenType> PreviousScreen = new ReactiveProperty<ScreenType>();

    static private ScreenManager _instance;

    static public ScreenManager GetInstance()
    {
        return _instance;
    }

    private void Awake()
    {
        _instance = this;
        CurrentScreen.Value = ScreenType.Start;
        PreviousScreen.Value = ScreenType.Start;
    }

    private void Start()
    {
        analytics = GameObject.Find("Analytics");
    }

    public void SetCurrentScreen(ScreenType screen)
    {
        if (screen != ScreenType.Fragment && screen != ScreenType.None && ScreenManager.GetInstance().CurrentScreen.Value != ScreenType.Fragment)
        {
            GetComponent<Animator>().SetTrigger("fade");
            StartCoroutine(WaitFade(screen));
        }
        else
        {
            PreviousScreen.Value = CurrentScreen.Value;
            CurrentScreen.Value = screen;
        }
    }

    IEnumerator WaitFade(ScreenType screen)
    {
        yield return new WaitUntil(() => Manager.GetInstance().FadeMiddle);

        if (screen == ScreenType.Controls)
        {
            helpCount++;
            analytics.GetComponent<UnityAnalyticsEvents>().HelpNeeded(helpCount);
        }

        PreviousScreen.Value = CurrentScreen.Value;
        CurrentScreen.Value = screen;
    }
}
