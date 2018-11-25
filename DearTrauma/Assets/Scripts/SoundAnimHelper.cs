using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimHelper : MonoBehaviour {

    [Header("References")]
    public AudioSource[] WalkAudios;

    public void PlayWalkOnce()
    {
        int rand = Random.Range(0, WalkAudios.Length);

        for (int i = 0; i < WalkAudios.Length; i++)
        {
            if (i == rand)
            {
                WalkAudios[i].Play();
            }
            else
            {
                WalkAudios[i].Stop();
            }
        }
    }
}
