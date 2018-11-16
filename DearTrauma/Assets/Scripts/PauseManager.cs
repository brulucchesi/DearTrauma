using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PauseManager : MonoBehaviour {

    [Header("References")]
    public Button[] PauseButtons;
    public Button UnpauseButton;

    private void Awake()
    {
        Manager.GetInstance().Paused = false;
        Time.timeScale = 1;
    }

    void Start ()
    {
        UnpauseButton.OnClickAsObservable().Subscribe(_ => UnpauseGame());

        var EscInput = Observable.EveryUpdate().Where(_ => Input.GetKeyDown(KeyCode.Escape));
        EscInput.Subscribe(_ =>
        {
            if (!Manager.GetInstance().Paused)
            {
                PauseGame();
            }
            else if (Manager.GetInstance().Paused)
            {
                UnpauseGame();
            }
        });
    }

    private void PauseGame()
    {
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Pause);
        Manager.GetInstance().Paused = true;
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game);
        Manager.GetInstance().Paused = false;
        Time.timeScale = 1;
    }
}
