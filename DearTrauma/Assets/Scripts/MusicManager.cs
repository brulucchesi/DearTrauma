using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource BackgroundMusic;
    public AudioClip Title;
    public AudioClip GamePlay;
    public AudioClip Boss;
    public AudioClip Credits;

	void Awake ()
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
        BackgroundMusic.clip = Boss;
        BackgroundMusic.Play();

    }
    public void ChangeCredits()
    {
        BackgroundMusic.clip = Credits;
        BackgroundMusic.Play();

    }

}
