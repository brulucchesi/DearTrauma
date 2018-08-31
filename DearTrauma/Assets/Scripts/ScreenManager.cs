using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ScreenManager : MonoBehaviour
{

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
    }

    public void SetCurrentScreen(ScreenType screen)
    {
        PreviousScreen.Value = CurrentScreen.Value;
        CurrentScreen.Value = screen;
    }
}
