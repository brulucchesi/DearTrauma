﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UniRx;

public class ButtonSceneLoader : MonoBehaviour {
    
    [Header("Modifiers")]
    public string Scene;

    void Start()
    {
        GetComponent<Button>().OnClickAsObservable().Subscribe(_ =>
        {
            if (Scene.Equals("restart"))
            {
                Manager.GetInstance().Restart();
            }
            else
            {
                SceneManager.LoadScene(Scene);
            }
        });
    }
}
