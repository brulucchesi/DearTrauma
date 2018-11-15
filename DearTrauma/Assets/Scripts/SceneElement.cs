using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneElement : MonoBehaviour
{
    [Header("References")]
    public AudioSource Audio;

    public void PlaySound()
    {
        Audio.Play();
    }
}
