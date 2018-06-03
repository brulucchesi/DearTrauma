using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    
    [Header("References")]
    public GameObject WaypointsParent;
    public GameObject Back;
    public GameObject Front;
    public GameObject LeftPos;
    public GameObject RightPos;

    [Header("Modifiers")]
    public float FollowSpeed = 20f;
    public float WaypointSpeed = 2f;

    private List<Transform> waypoints;
    private bool followPlayer;
    private int currentWaypoint;

    // Use this for initialization
    void Start ()
    {
        currentWaypoint = 0;
        waypoints = new List<Transform>();
        foreach (Transform t in WaypointsParent.transform)
        {
            waypoints.Add(t);
        }

        Flip();
        StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint].position));
	}
	
	private IEnumerator MoveToWaypoint(Vector3 point)
    {
        Vector3 init = transform.localPosition;

        float startTime = Time.time;
        float journeyLength = Vector3.Distance(init, point);
        float distCovered;
        float fracJourney = 0;

        while (fracJourney < 1)
        {
            distCovered = (Time.time - startTime) * WaypointSpeed;
            fracJourney = distCovered / journeyLength;
            transform.localPosition = Vector3.Lerp(init, point, fracJourney);
            yield return new WaitForSeconds(1 / 30);
        }

        currentWaypoint = (currentWaypoint + 1)%waypoints.Count;
        Flip();
        StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint].position));
    }

    private void Flip()
    {
        //Going right
        if(waypoints[currentWaypoint].position.x > transform.position.x)
        {
            Back.transform.position = LeftPos.transform.position;
            Front.transform.position = RightPos.transform.position;
        }
        else //Going left
        {
            Back.transform.position = RightPos.transform.position;
            Front.transform.position = LeftPos.transform.position;
        }
    }
}
