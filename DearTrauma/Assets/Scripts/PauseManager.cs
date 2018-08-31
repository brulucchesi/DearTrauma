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

    void Start ()
    {
        Paused = false;

        //foreach (Button bt in PauseButtons)
        //{
        //    bt.OnClickAsObservable().Subscribe(_ => PauseGame());
        //}
        //UnpauseButton.OnClickAsObservable().Subscribe(_ => UnpauseGame());

        ScreenManager.GetInstance().CurrentScreen.Subscribe(screen =>
        {
            if(screen == ScreenManager.ScreenType.Pause)
            {
                PauseGame();
            }
            else if (screen == ScreenManager.ScreenType.Game)
            {
                UnpauseGame();
            }
        });
    }

    private void PauseGame()
    {
        Paused = true;
        Time.timeScale = 0;
    }

    private void UnpauseGame()
    {
        Time.timeScale = 1;
        Paused = false;
    }
}
