using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

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

            return transform.position;
        }

        return null;
    }
}
