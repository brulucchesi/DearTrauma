using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    [Header("References")]
    public GameObject WaypointsParent;
    public Transform Front;
    public Transform[] PartsToFlip;

    [Header("Modifiers")]
    public float FollowSpeed = 20f;
    public float WaypointSpeed = 2f;
    public float TimeToWait = 2f;

    [HideInInspector]
    public bool Right;

    private List<Vector3> waypoints;
    private int currentWaypoint;
    private Coroutine waypointCoroutine;
    private Coroutine followCoroutine;
    private Vector3 lastPos;
    private bool haveAttacked = false;

    // Use this for initialization
    void Start()
    {
        lastPos = transform.position;

        Right = true;
        CalculateWaypoints();

        Flip(waypoints[currentWaypoint]);
        waypointCoroutine = StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint]));
    }

    private void CalculateWaypoints()
    {
        currentWaypoint = 0;
        waypoints = new List<Vector3>();
        foreach (Transform t in WaypointsParent.transform)
        {
            waypoints.Add(t.position);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            foreach (Vector3 v in waypoints)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(v, 0.2f);
            }
        }
    }

    private IEnumerator MoveToWaypoint(Vector3 pointOrig)
    {
        //Vector3 init = transform.localPosition;

        //float startTime = Time.time;
        //float journeyLength = Vector3.Distance(init, point);
        //float distCovered;
        //float fracJourney = 0;

        //while (fracJourney < 1)
        //{
        //    distCovered = (Time.time - startTime) * WaypointSpeed;
        //    fracJourney = distCovered / journeyLength;
        //    transform.localPosition = Vector3.Lerp(init, point, fracJourney);
        //    yield return new WaitForSeconds(1 / 30);
        //}
        Vector3 point = new Vector3(pointOrig.x, transform.position.y);

        while (Vector2.Distance(point, transform.position) > 1f)
        {
            point = new Vector3(pointOrig.x, transform.position.y);
            if (point.x > transform.position.x)
            {
                Vector2 vel = GetComponent<Rigidbody2D>().velocity;
                vel.x = WaypointSpeed;
                GetComponent<Rigidbody2D>().velocity = vel;
            }
            else
            {
                Vector2 vel = GetComponent<Rigidbody2D>().velocity;
                vel.x = -WaypointSpeed;
                GetComponent<Rigidbody2D>().velocity = vel;
            }

            //if (Vector2.Distance(lastPos, transform.position) < 0.1f)
            //{
            //    if (waypointCoroutine != null)
            //    {
            //        lastPos = transform.position;
            //        yield return new WaitForSeconds(1 / 30);
            //        StopCoroutine(waypointCoroutine);
            //        currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
            //        Flip(waypoints[currentWaypoint]);
            //        waypointCoroutine = StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint]));
            //    }
            //}

            lastPos = transform.position;
            yield return new WaitForSeconds(1 / 30);
        }

        //  Debug.Log("waypoint");
        GetComponent<Animator>().SetBool("Idle", true);
        yield return new WaitForSeconds(TimeToWait);
        GetComponent<Animator>().SetBool("Idle", false);
        currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        Flip(waypoints[currentWaypoint]);
        waypointCoroutine = StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint]));
    }

    private IEnumerator Follow(Transform t, bool isPlayer)
    {

        //Vector3 point = Manager.GetInstance().Player.transform.position;
        while (true/*Vector2.Distance(point, transform.position) > 0.01f*/)
        {
            if (isPlayer && Manager.GetInstance().Player.GetComponent<Movement>().Safe)
            {
                Debug.Log("Stop Follow player");

                StartFollow(false, null, false);
            }
            if (t == null)
            {
                Debug.Log("Stop Follow at all");
                StartFollow(false, null, false);
            }
            else
            {
                Vector3 point = t.position;
                Flip(point);
                //point.y = transform.position.y;
                //transform.position = Vector2.MoveTowards(transform.position, point, FollowSpeed * Time.deltaTime);

                if (point.x > transform.position.x)
                {
                    Vector2 vel = GetComponent<Rigidbody2D>().velocity;
                    vel.x = FollowSpeed;
                    GetComponent<Rigidbody2D>().velocity = vel;
                }
                else
                {
                    Vector2 vel = GetComponent<Rigidbody2D>().velocity;
                    vel.x = -FollowSpeed;
                    GetComponent<Rigidbody2D>().velocity = vel;
                }
            }

            yield return new WaitForSeconds(1 / 30);
        }
    }

    private void Flip(Vector3 point)
    {
        //   Debug.Log("flip");
        //Going right
        //  Debug.Log("antes: " + Right);
        if (point.x > transform.position.x)
        {
            //if (!Right)
            //{
            //    //  Debug.Log("flipright");
            //    Back.transform.position = LeftPos.transform.position;
            //    Front.transform.position = RightPos.transform.position;
            //    GetComponent<Animator>().SetTrigger("Right");
            //}
            //Right = true;
            if (!Right)
            {
                Flip();
            }
        }
        else //Going left
        {
            //if (Right)
            //{
            //       Debug.Log("flipleft");
            //    Back.transform.position = RightPos.transform.position;
            //    Front.transform.position = LeftPos.transform.position;
            //    GetComponent<Animator>().SetTrigger("Left");
            //}
            //Right = false;
            if (Right)
            {
                Flip();
            }
        }
        // Debug.Log("dps: " + Right);
    }

    public void StartFollow(bool follow, Transform point, bool isPlayer)
    {
        if (follow)
        {
            if (waypointCoroutine != null)
            {
                StopCoroutine(waypointCoroutine);
            }
            followCoroutine = StartCoroutine(Follow(point, isPlayer));
        }
        else if (GetComponent<Range>().CurrentTargetEnemy() && !haveAttacked)
        {
            if (followCoroutine != null)
            {
                StopCoroutine(followCoroutine);
            }

            if (waypointCoroutine != null)
            {
                StopCoroutine(waypointCoroutine);
            }

            if (GetComponent<Range>().CurrentColliderTransform() != null)
            {
                followCoroutine = StartCoroutine(Follow(GetComponent<Range>().CurrentColliderTransform().transform, isPlayer));
            }
        }
        else
        {
            if (followCoroutine != null)
            {
                Debug.Log("Stop Coroutine");

                StopCoroutine(followCoroutine);
            }
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

    public void AttackPlayer()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }

    public void PlayerDied()
    {
        Manager.GetInstance().Player.GetComponent<Movement>().PlayerDied();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.layer == 10)
        //{
        //    haveAttacked = true;
        //    if (followCoroutine != null)
        //    {
        //        Destroy(collision.gameObject);
        //    }
        //}
    }

    void Flip()
    {
        Right = !Right;
        foreach (Transform t in PartsToFlip)
        {
            Vector3 currentScale = t.localScale;
            currentScale.x *= -1;
            t.localScale = currentScale;
        }
    }
}
