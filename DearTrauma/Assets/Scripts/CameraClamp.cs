﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp : MonoBehaviour {
    
    [Header("Modifiers")]
    public bool ClearOnExit = true;
    public bool TopLimit;
    public bool BottomLimit;
    public bool LeftLimit;
    public bool RightLimit;
    public float CamY = 1.64f;
    public float CamX = 2.05f;
    public float CamYBig = 0.4f;
    public float CamXBig = 2.05f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Camera.main.GetComponent<CamFollow>().SetClamp(BottomLimit? transform.position.y:float.MinValue,
            //                                               TopLimit ? transform.position.y : float.MaxValue,
            //                                               LeftLimit ? transform.position.x : float.MinValue,
            //                                               RightLimit ? transform.position.x : float.MaxValue);
            Camera.main.GetComponent<CamFollow>().SetOffset(CamY, CamX, CamYBig, CamXBig);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(ClearOnExit)
            {
                //Camera.main.GetComponent<CamFollow>().SetClamp(float.MinValue, float.MaxValue, float.MinValue, float.MaxValue);
                Camera.main.GetComponent<CamFollow>().ResetOffset();
            }
        }
    }
}
