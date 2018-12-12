using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource BackgroundMusic;
    public AudioClip Title;
    public AudioClip GamePlay;
    public AudioClip Credits;
    public AudioClip EmptySound;

    void Awake()
    {
        BackgroundMusic.clip = Title;
        BackgroundMusic.Play();
    }

    public void ChangeGamePlay()
    {
        BackgroundMusic.clip = GamePlay;
        BackgroundMusic.Play();
    }

    public void ChangeBoss()
    {
        BackgroundMusic.clip = EmptySound;
        BackgroundMusic.gameObject.SetActive(false);
        BackgroundMusic.gameObject.SetActive(true);
    }

    public void ChangeCredits()
    {
        BackgroundMusic.clip = Credits;
        BackgroundMusic.Play();
    }
}
