using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Manager : MonoBehaviour {

    [Header("References")]
    public GameObject Player;

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
        Observable.EveryUpdate().Where(_ => Input.anyKey &&
                                            (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Start))
                                            .Subscribe(_ => ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game));
        var escInput = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape));
        escInput.Where(_=> (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Game))
                           .Subscribe(_ => ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Pause));
        escInput.Where(_ => (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Credits))
                           .Subscribe(_ => ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.DemoEnd));
    }
}
