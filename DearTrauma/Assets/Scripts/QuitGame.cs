using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class QuitGame : MonoBehaviour {

    private void Start()
    {
        GetComponent<Button>().OnClickAsObservable().Subscribe(_ => ButtonQuit());
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }
}
