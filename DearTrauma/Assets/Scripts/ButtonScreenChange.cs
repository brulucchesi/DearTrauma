using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ButtonScreenChange : MonoBehaviour
{

    [Header("Modifiers")]
    public ScreenManager.ScreenType NextScreen;
    public bool Back = false;

    private ScreenManager.ScreenType previousScreen = ScreenManager.ScreenType.None;

    void Start()
    {
        GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
        {
            if (Back)
            {
                ScreenManager.GetInstance().SetCurrentScreen(previousScreen);
                previousScreen = ScreenManager.ScreenType.None;
            }
            else
            {
                ScreenManager.GetInstance().SetCurrentScreen(NextScreen);
            }
        });
    }

    private void OnEnable()
    {
        if (previousScreen == ScreenManager.ScreenType.None)
        {
            if (ScreenManager.GetInstance())
            {
                previousScreen = ScreenManager.GetInstance().PreviousScreen.Value;
            }
        }
    }
}
