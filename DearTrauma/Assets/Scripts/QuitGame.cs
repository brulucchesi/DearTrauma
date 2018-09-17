using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class QuitGame : MonoBehaviour
{

    private GameObject analytics;

    private void Start()
    {
        analytics = GameObject.Find("Analytics");

        GetComponent<Button>().OnClickAsObservable().Subscribe(_ => ButtonQuit());
    }

    public void ButtonQuit()
    {
        analytics.GetComponent<UnityAnalyticsEvents>().QuitGame();
        Application.Quit();
    }
}
