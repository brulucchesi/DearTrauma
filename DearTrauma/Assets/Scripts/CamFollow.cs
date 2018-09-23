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
    private Vector3 camOffset;
    private float camY;
    private float camX;
    private float camYBig;
    private float camXBig;

    private float minX, maxX, minY, maxY;

    private bool isBig = false;

    private void Start()
    {
        SetClamp(float.MinValue, float.MaxValue, float.MinValue, float.MaxValue);
        camY = CamY;
        camX = CamX;
    }

    void Update()
    {
        if (!isBig)
        {
            var newOffset = new Vector3(camX, camY, -10);
            camOffset = Vector3.SmoothDamp(camOffset, newOffset, ref velocity, SmoothTime);
        }
        else
        {
            var newOffset = new Vector3(camXBig, camYBig, -10);
            camOffset = Vector3.SmoothDamp(camOffset, newOffset, ref velocity, SmoothTime);
        }

        Vector3 targetPosition = Target.TransformPoint(camOffset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);

        //Vector3 clampedPos = transform.position;
        //clampedPos.y = Mathf.Clamp(transform.position.y, minY, maxY);
        //clampedPos.x = Mathf.Clamp(transform.position.x, minX, maxX);
        //transform.position = clampedPos;
    }

    public void Big()
    {
        isBig = true;
        camY = CamYBig;
        camX = CamXBig;
        Camera.main.orthographicSize = SizeBig;
    }

    public void SetClamp(float miny, float maxy, float minx, float maxx)
    {
        minX = minx;
        maxX = maxx;
        minY = miny;
        maxY = maxy;
    }

    public void SetOffset(float x, float y, float yBig, float xBig)
    {
        camY = y;
        camX = x;
        camYBig = yBig;
        camXBig = xBig;
    }

    public void ResetOffset()
    {
        camY = CamY;
        camX = CamX;
        camYBig = CamYBig;
        camXBig = CamXBig;
    }
}
