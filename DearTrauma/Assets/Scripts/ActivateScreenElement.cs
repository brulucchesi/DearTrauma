using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ActivateScreenElement : MonoBehaviour {
    
    [Header("References")]
    public List<GameObject> ScreenElements;

    [Header("Modifiers")]
    public List<ScreenManager.ScreenType> ActiveScreens;

    void Start()
    {
        ScreenManager.GetInstance().CurrentScreen.Subscribe(screen =>
        {
            foreach (GameObject go in ScreenElements)
            {
                if (go != null)
                {
                    go.SetActive(ActiveScreens.Contains(screen));
                }
            }
        });
    }
}
