using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class ActivateScreenElement : MonoBehaviour {
    
    [Header("References")]
    public List<GameObject> ScreenElements;
    public Button FirstButton;

    [Header("Modifiers")]
    public List<ScreenManager.ScreenType> ActiveScreens;

    void Start()
    {
        ScreenManager.GetInstance().CurrentScreen.Subscribe(screen =>
        {
            var active = ActiveScreens.Contains(screen);

            foreach (GameObject go in ScreenElements)
            {
                if (go != null)
                {
                    go.SetActive(active);
                }
            }

            if(active)
            {
                EventSystem.current.SetSelectedGameObject(null);
                StartCoroutine(SelectButton());
            }
        });
    }

    IEnumerator SelectButton()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        if (FirstButton)
        {
            EventSystem.current.SetSelectedGameObject(FirstButton.gameObject);
        }
    }
}
