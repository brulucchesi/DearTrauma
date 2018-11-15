using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("References")]
    public AudioSource Audio;

    public void ReceiveAttack()
    {
        GetComponent<Animator>().SetTrigger("Break");
        Audio.Play();
    }
}
