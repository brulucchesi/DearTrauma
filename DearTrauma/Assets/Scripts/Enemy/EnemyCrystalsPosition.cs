using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrystalsPosition : MonoBehaviour {

    public GameObject track;

	void Update ()
	{
	    transform.position = Vector3.MoveTowards(transform.position, track.transform.position, Time.deltaTime);
	}
}
