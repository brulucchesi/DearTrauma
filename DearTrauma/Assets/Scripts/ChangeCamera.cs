﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamera : MonoBehaviour
{

    public GameObject ThisCamera;

    public GameObject Main;

    public Animator ThisAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Selected");
            ThisCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().enabled = true;
            ThisCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().MoveToTopOfPrioritySubqueue();

            if (ThisAnimator != null)
            {
                Debug.Log("play animation from camera");
                ThisAnimator.SetTrigger("play");
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
}
