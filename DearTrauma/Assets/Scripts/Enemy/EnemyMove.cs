using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

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

    [HideInInspector]
    public bool Dead;

    private List<Vector3> waypoints;
    private int currentWaypoint;
    private Coroutine waypointCoroutine;
    private Coroutine followCoroutine;
    private Vector3 lastPos;
    private bool haveAttacked = false;

    private bool canFollow;
    private bool canWaypoint;
    private IDisposable waypointDisposable;
    private IDisposable followDisposable;

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
        canWaypoint = true;
        //if (waypointCoroutine != null)
        //{
        //    StopCoroutine(waypointCoroutine);
        //}

        Vector3 point = new Vector3(pointOrig.x, transform.position.y);

        while (Vector2.Distance(point, transform.position) > 1f)
        {
            if (canWaypoint)
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

                lastPos = transform.position;
                yield return new WaitForSeconds(1 / 30);
            }
            else
            {
                yield return new WaitForSeconds(1 / 30);
            }
        }

        if (canWaypoint)
        {
            GetComponent<Animator>().SetBool("Idle", true);
            yield return new WaitForSeconds(TimeToWait);
            GetComponent<Animator>().SetBool("Idle", false);
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
            Flip(waypoints[currentWaypoint]);
            //waypointCoroutine = StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint]));
            waypointDisposable = Observable.FromCoroutine(() => MoveToWaypoint(waypoints[currentWaypoint])).Subscribe();
        }
    }

    private IEnumerator Follow(Transform t, bool isPlayer)
    {
        canFollow = true;
        //if (followCoroutine != null)
        //{
        //    StopCoroutine(followCoroutine);
        //}

        while (true)
        {
            if (canFollow)
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
            else
            {
                yield return new WaitForSeconds(1 / 30);
            }
        }
    }

    private void Flip(Vector3 point)
    {
        if (point.x > transform.position.x)
        {
            if (!Right)
            {
                Flip();
            }
        }
        else //Going left
        {
            if (Right)
            {
                Flip();
            }
        }
    }

    public void StartFollow(bool follow, Transform point, bool isPlayer)
    {
        if (follow)
        {
            if (waypointDisposable != null)
            {
                //StopCoroutine(waypointCoroutine);
                waypointDisposable.Dispose();
                waypointDisposable = null;
                canWaypoint = false;
            }
            if (followDisposable != null)
            {
                //StopCoroutine(followCoroutine);
                followDisposable.Dispose();
                followDisposable = null;
                canFollow = false;
            }

            //followCoroutine = StartCoroutine(Follow(point, isPlayer));
            followDisposable = Observable.FromCoroutine(() => Follow(point, isPlayer)).Subscribe();
        }
        //else if (GetComponent<Range>().CurrentTargetEnemy() && !haveAttacked)
        //{
        //    if (followCoroutine != null)
        //    {
        //        StopCoroutine(followCoroutine);
        //    }

        //    if (waypointCoroutine != null)
        //    {
        //        StopCoroutine(waypointCoroutine);
        //    }

        //    if (GetComponent<Range>().CurrentColliderTransform() != null)
        //    {
        //        followCoroutine = StartCoroutine(Follow(GetComponent<Range>().CurrentColliderTransform().transform, isPlayer));
        //    }
        //}
        else
        {
            if (followDisposable != null)
            {
                //StopCoroutine(followCoroutine);
                followDisposable.Dispose();
                followDisposable = null;
                canFollow = false;
            }
            if (waypointDisposable != null)
            {
                //StopCoroutine(waypointCoroutine);
                waypointDisposable.Dispose();
                waypointDisposable = null;
                canWaypoint = false;
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

            GetComponent<Animator>().SetBool("Idle", true);
            Observable.Timer(System.TimeSpan.FromSeconds(3f)).Subscribe(_ =>
            {
                GetComponent<Animator>().SetBool("Idle", false);
                currentWaypoint = minIndex;
                Flip(waypoints[currentWaypoint]);
                //waypointCoroutine = StartCoroutine(MoveToWaypoint(waypoints[currentWaypoint]));
                waypointDisposable = Observable.FromCoroutine(() => MoveToWaypoint(waypoints[currentWaypoint])).Subscribe();
            }
            );
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
