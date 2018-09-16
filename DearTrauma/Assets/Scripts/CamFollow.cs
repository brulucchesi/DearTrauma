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

    private float minX, maxX, minY, maxY;

    private void Start()
    {
        SetClamp(float.MinValue, float.MaxValue, float.MinValue, float.MaxValue);
        camY = CamY;
        camX = CamX;
    }

    void Update()
    {
        Vector3 targetPosition = Target.TransformPoint(new Vector3(CamX, CamY, -10));
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);

        Vector3 clampedPos = transform.position;
        clampedPos.y = Mathf.Clamp(transform.position.y, minY, maxY);
        clampedPos.x = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = clampedPos;
    }

    public void Big()
    {
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
}
