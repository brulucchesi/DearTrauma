using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PauseManager : MonoBehaviour {

    [Header("References")]
    public Button[] PauseButtons;
    public Button UnpauseButton;

    [HideInInspector]
    public bool Paused;

    private void Awake()
    {
        Paused = false;
        Time.timeScale = 1;
    }

    void Start ()
    {
        UnpauseButton.OnClickAsObservable().Subscribe(_ => UnpauseGame());

        var EscInput = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape));
        EscInput.Subscribe(_ =>
        {
            if (!Paused)
            {
                PauseGame();
            }
            else if (Paused)
            {
                UnpauseGame();
            }
        });
    }

    private void PauseGame()
    {
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Pause);
        Paused = true;
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game);
        Paused = false;
        Time.timeScale = 1;
    }
}
