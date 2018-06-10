using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {
    
    [Header("References")]
    public Transform Target;

    [Header("Modifiers")]
    public float smoothTime = 0.3f;
    public float camY = 2f;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        Vector3 targetPosition = Target.TransformPoint(new Vector3(0, camY, -10));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
