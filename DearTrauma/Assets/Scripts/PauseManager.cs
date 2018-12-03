using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class PauseManager : MonoBehaviour
{

    [Header("References")]
    public Button[] PauseButtons;
    public Button UnpauseButton;

    void Start()
    {
        Manager.GetInstance().Paused = false;
        Time.timeScale = 1;

        UnpauseButton.OnClickAsObservable().Subscribe(_ => UnpauseGame());

        var EscInput = Observable.EveryUpdate().Where(_ => this && Input.GetKeyDown(KeyCode.Escape));
        var disp = EscInput.Subscribe(_ =>
        {
            Debug.Log("esc");
            if (!Manager.GetInstance().Paused)
            {
                PauseGame();
            }
            else if (Manager.GetInstance().Paused)
            {
                UnpauseGame();
            }
        });

        //Manager.GetInstance().Restarted.Where(r => r).Subscribe(screen =>
        //{
        //    disp.Dispose();
        //});
    }

    private void PauseGame()
    {
        if (ScreenManager.GetInstance().CurrentScreen.Value == ScreenManager.ScreenType.Game)
        {
            print("pause");
            ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Pause);
            Manager.GetInstance().Paused = true;
            Time.timeScale = 0;
        }
    }

    private void UnpauseGame()
    {
        print("unpause");
        ScreenManager.GetInstance().SetCurrentScreen(ScreenManager.ScreenType.Game);
        Manager.GetInstance().Paused = false;
        Time.timeScale = 1;
    }
}
