using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroScript : MonoBehaviour
{
    public float time;

    private void Awake()
    {
        StartCoroutine(WaitIntro());
    }

    IEnumerator WaitIntro()
    {
        yield return new WaitForSeconds(time);
        transform.parent.GetComponent<PlayableDirector>().Play();
        Manager.GetInstance().EndIntro();
    }
}