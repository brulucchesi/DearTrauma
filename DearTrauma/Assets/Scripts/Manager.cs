using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Manager : MonoBehaviour
{
    [Header("References")]
    public GameObject Player;
    public GameObject FinalFragment;
    public GameObject Boss;

    [Header("Modifiers")]
    public float BossSeconds = 0f;

    public IObservable<long> EscInput;
    public IObservable<long> AnyInput;

    private GameObject analytics;

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
        EscInput = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape));
        AnyInput = Observable.EveryUpdate().Where(_ => Input.anyKeyDown);
        EscInput.Where(_ => (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Game))
                           .Subscribe(_ => ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Pause));
        AnyInput.Where(_ => (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Credits))
                           .Subscribe(_ => ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.DemoEnd));
    }

    public void AnimationMiddle()
    {
        FinalFragment.SetActive(false);
        Boss.SetActive(true);
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
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Credits);
    }
}
