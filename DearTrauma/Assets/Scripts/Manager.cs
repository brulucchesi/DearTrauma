using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.Playables;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public IObservable<long> EscInput;
    public IObservable<long> AnyInput;

    private GameObject analytics;

    private GameObject musicManager;


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
        musicManager = GameObject.Find("MusicManager");

        analytics = GameObject.Find("Analytics");

        Observable.EveryUpdate().Where(_ => Input.anyKey &&
                                            (ScreenManager.GetInstance().CurrentScreen.Value ==
                                             ScreenManager.ScreenType.Start))
            .Subscribe(_ =>
                {
                    musicManager.GetComponent<MusicManager>().ChangeGamePlay();
                    ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game);
                    if (analytics != null)
                    {
                        analytics.GetComponent<UnityAnalyticsEvents>().StartLevel(1);
                    }
                }
            );

        EscInput = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape));
        AnyInput = Observable.EveryUpdate().Where(_ => Input.anyKeyDown);
        EscInput.Subscribe(_ =>
        {
            if (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Game)
            {
                ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Pause);
            }
            else if (GetComponent<PauseManager>().Paused)
            {
                ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game);
            }
        });
        AnyInput.Where(_ => (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Credits))
                           .Subscribe(_ => ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.DemoEnd));
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Button>() != null)
            {
                ButtonClick.Play();
            }
            else
            {
                NormalClick.Play();
            }
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
}
