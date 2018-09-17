using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vigiaCrystalsPosition : MonoBehaviour {

    public GameObject track;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        transform.position = track.transform.position;

	}
}
