using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    [Header("References")]
    public AudioSource Audio;

    private bool active;

    private void Start()
    {
        active = true;
    }

    public Vector3? GetCheckpoint()
    {
        if(active)
        {
            GetComponent<Animator>().SetTrigger("Checkpoint");
            active = false;
            Audio.Play();

            return transform.position;
        }

        return null;
    }
}
