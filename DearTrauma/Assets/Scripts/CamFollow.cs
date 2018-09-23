using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {
    
    [Header("References")]
    public Transform Target;

    [Header("Modifiers")]
    public float SmoothTime = 0.3f;
    public float CamYDefault = 1.64f;
    public float CamXDefault = 2.05f;
    public float CamYBigDefault = 0.4f;
    public float CamXBigDefault = 2.05f;
    public float SizeBig = 7.5f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 camOffset;
    public float camY;
    public float camX;
    public float camYBig;
    public float camXBig;

    private float minX, maxX, minY, maxY;

    private bool isBig = false;

    private void Start()
    {
        SetClamp(float.MinValue, float.MaxValue, float.MinValue, float.MaxValue);
        camY = CamYDefault;
        camX = CamXDefault;
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
        camY = CamYBigDefault;
        camX = CamXBigDefault;
        Camera.main.orthographicSize = SizeBig;
    }

    public void SetClamp(float miny, float maxy, float minx, float maxx)
    {
        minX = minx;
        maxX = maxx;
        minY = miny;
        maxY = maxy;
    }

    public void SetOffset(float y, float x, float yBig, float xBig)
    {
        camY = y;
        camX = x;
        camYBig = yBig;
        camXBig = xBig;
    }

    public void ResetOffset()
    {
        camY = CamYDefault;
        camX = CamXDefault;
        camYBig = CamYBigDefault;
        camXBig = CamXBigDefault;
    }
}
