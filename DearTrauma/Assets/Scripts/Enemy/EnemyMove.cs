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

    [HideInInspector]
    public ReactiveProperty<bool> Following = new ReactiveProperty<bool>();

    [HideInInspector]
    public ReactiveProperty<float> DistToWaypoint = new ReactiveProperty<float>();

    private List<Vector3> waypoints;
    private int currentWaypoint;
    private Coroutine waypointCoroutine;
    private Coroutine followCoroutine;
    private bool haveAttacked = false;

    private bool canFollow;
    private bool canWaypoint;
    private IDisposable waypointDisposable;
    private IDisposable followDisposable;

    // Use this for initialization
    void Start()
    {
        Right = true;
        canWaypoint = true;
        CalculateWaypoints();

        currentWaypoint = 0;
        Flip(waypoints[currentWaypoint]);

        Vector3 point = new Vector3(waypoints[currentWaypoint].x, transform.position.y);
        DistToWaypoint.Value = Vector2.Distance(point, transform.position);
        
        Following.Value = false;

        var fixUpdate = Observable.EveryUpdate().Where(_=>!Dead);
        var toWaypoint = fixUpdate.Where(_ => Following.Value == false && canWaypoint);
        toWaypoint.Subscribe(_ =>
        {
            CalculateWaypointVel();
        });

        DistToWaypoint.Where(_ => Following.Value == false && canWaypoint).Subscribe(dist =>
        {
            if(dist < 1f)
            {
                canWaypoint = false;

                GetComponent<Animator>().SetBool("Idle", true);
                Observable.Timer(TimeSpan.FromSeconds(TimeToWait)).Subscribe(_ =>
                {
                    if (Following.Value == false && !Dead)
                    {
                        currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
                        Flip(waypoints[currentWaypoint]);
                        canWaypoint = true;
                        GetComponent<Animator>().SetBool("Idle", false);
                    }
                });
            }
        });

        var toFollow = fixUpdate.Where(_ => Following.Value == true/* && canFollow*/);
        toFollow.Subscribe(_ =>
        {
            CalculateFollowVel();
        });
    }

    private void CalculateWaypointVel()
    {
        Vector3 point = new Vector3(waypoints[currentWaypoint].x, transform.position.y);
        DistToWaypoint.Value = Vector2.Distance(point, transform.position);
        
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
    }

    private void CalculateFollowVel()
    {
        if (Manager.GetInstance().Player.GetComponent<Movement>().Safe)
        {
            Following.Value = false;
            Observable.Timer(TimeSpan.FromSeconds(TimeToWait)).Subscribe(_ =>
            {
                if (Following.Value == false && !Dead)
                {
                    canWaypoint = true;
                }
            });
        }
        else
        {
            Vector3 point = Manager.GetInstance().Player.transform.position;
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

    public void AttackPlayer()
    {
        GetComponent<Animator>().SetTrigger("Attack");
    }

    public void PlayerDied()
    {
        Manager.GetInstance().Player.GetComponent<Movement>().PlayerDied();
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
}
