using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ChangeCamera : MonoBehaviour
{

    public GameObject ThisCamera;

    public GameObject Main;

    public PlayableDirector ThisTimeline;

    private bool timelineHasPlayed = false;

    private bool timelineIsReseted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Selected");
            ThisCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = true;
            ThisCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().MoveToTopOfPrioritySubqueue();

            if (ThisTimeline != null)
            {
                if (!timelineHasPlayed)
                {
                    Debug.Log("alo to tocanu");
                    timelineHasPlayed = true;
                    Debug.Log("play animation from camera");
                    ThisTimeline.Play();
                }
                else
                {
                    ThisCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = false;
                    Main.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = true;
                    Main.GetComponent<Cinemachine.CinemachineVirtualCamera>().MoveToTopOfPrioritySubqueue();
                    Debug.Log("animation da camera já deu play");
                }

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Leave");
            ThisCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = false;
            Main.GetComponent<Cinemachine.CinemachineVirtualCamera>().MoveToTopOfPrioritySubqueue();
        }
    }

    private void Update()
    {
        if(ThisTimeline != null)
        {
            if (ThisCamera.GetComponent<PlayableDirector>().state != PlayState.Playing && timelineHasPlayed && !timelineIsReseted)
            {
                timelineIsReseted = true;
                ThisCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = false;
                Main.GetComponent<Cinemachine.CinemachineVirtualCamera>().MoveToTopOfPrioritySubqueue();
            }
        }
    }
}
