using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {
    
    [Header("References")]
    public Transform Target;

    [Header("Modifiers")]
    public float SmoothTime = 0.3f;
    public float CamY = 1.64f;
    public float CamX = 2.05f;
    public float CamYBig = 0.4f;
    public float CamXBig = 2.05f;
    public float SizeBig = 7.5f;

    private Vector3 velocity = Vector3.zero;
    private float camY;
    private float camX;

    private void Start()
    {
        camY = CamY;
        camX = CamX;
    }

    void Update()
    {
        Vector3 targetPosition = Target.TransformPoint(new Vector3(CamX, CamY, -10));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
    }

    public void Big()
    {
        camY = CamYBig;
        camX = CamXBig;
        Camera.main.orthographicSize = SizeBig;
    }
}
