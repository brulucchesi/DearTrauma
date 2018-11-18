using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithCameraX : MonoBehaviour
{
    public float xPosition = 10.75f;
    public float yPosition = -8.1f;
    private GameObject cam;

    public float smoothTime = 0.05f;
    private Vector3 velocity = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        cam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(cam.transform.position.x + xPosition, yPosition, transform.position.z), ref velocity, smoothTime);
    }
}
