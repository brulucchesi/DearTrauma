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

    [HideInInspector]
    public bool Right;

    private List<Vector3> waypoints;
    private int currentWaypoint;
    private Coroutine waypointCoroutine;
    private Coroutine followCoroutine;

    // Use this for initialization
    void Start ()
    {
        currentWaypoint = 0;
        waypoints = new List<Vector3>();
        foreach (Transform t in WaypointsParent.transform)
        {
            waypoints.Add(t.position);
        }

        Flip(waypoints[currentWaypoint]);
        waypointCoroutine = StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint]));
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
        Flip(waypoints[currentWaypoint]);
        waypointCoroutine = StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint]));
    }

    private IEnumerator FollowPlayer()
    {
        while(true)
        {
            Vector3 point = Manager.GetInstance().Player.transform.position;
            Flip(point);
            point.y = transform.position.y;
            transform.position = Vector2.MoveTowards(transform.position, point, FollowSpeed * Time.deltaTime);
            yield return new WaitForSeconds(1 / 30);
        }
    }

    private void Flip(Vector3 point)
    {
        //Going right
        if(point.x > transform.position.x)
        {
            Right = true;
            Back.transform.position = LeftPos.transform.position;
            Front.transform.position = RightPos.transform.position;
        }
        else //Going left
        {
            Right = false;
            Back.transform.position = RightPos.transform.position;
            Front.transform.position = LeftPos.transform.position;
        }
    }

    public void StartFollowPlayer(bool follow)
    {
        if(follow)
        {
            StopCoroutine(waypointCoroutine);
            followCoroutine = StartCoroutine(FollowPlayer());
        }
        else
        {
            StopCoroutine(followCoroutine);
            float minDist = float.MaxValue;
            int minIndex = -1;
            for (int i = 0; i < waypoints.Count; i++)
            {
                float dist = Vector2.Distance(waypoints[i], transform.position);
                if (dist < minDist)
                {
                    minIndex = i;
                    minDist = dist;
                }
            }

            currentWaypoint = minIndex;
            Flip(waypoints[currentWaypoint]);
            waypointCoroutine = StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint]));
        }
    }
}
