using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class EnemyMove : MonoBehaviour
{
    public enum MoveState
    {
        Waypoint,
        Follow,
        LostWaypoint,
        Waiting
    }

    [Header("References")]
    public int EnemyNumber = -1;
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
    public ReactiveProperty<MoveState> CurrentMoveState = new ReactiveProperty<MoveState>(MoveState.Waypoint);

    [HideInInspector]
    public ReactiveProperty<bool> PlayerInRange = new ReactiveProperty<bool>(false);

    [HideInInspector]
    public ReactiveProperty<bool> ReachedLimit = new ReactiveProperty<bool>(false);

    private List<Vector3> waypoints;
    private int currentWaypoint;
    private bool useWaypoint;

    private List<IDisposable> timersDisposables;

    public int SentEnemyNumber()
    {
        return EnemyNumber;
    }

    void Start()
    {
        timersDisposables = new List<IDisposable>();
        Right = true;
        useWaypoint = true;
        CalculateWaypoints();
        currentWaypoint = 0;

        Flip(waypoints[currentWaypoint]);
        GetComponent<Animator>().SetBool("Idle", false);

        var fixUpdate = Observable.EveryUpdate().Where(_ => this && !Dead);
        fixUpdate.Subscribe(_ =>
        {
            switch(CurrentMoveState.Value)
            {
                case MoveState.Waypoint:
                    Waypoint();
                    break;
                case MoveState.Follow:
                    Follow();
                    break;
                case MoveState.LostWaypoint:
                    LostWaypoint();
                    break;
                case MoveState.Waiting:
                    Waiting();
                    break;
                default:
                    break;
            }
        });

        ReachedLimit.Subscribe(limit =>
        {
            if (limit)
            {
                useWaypoint = false;
                CurrentMoveState.Value = MoveState.Waiting;

                GetComponent<Animator>().SetBool("Idle", true);
                Observable.Timer(TimeSpan.FromSeconds(TimeToWait)).Subscribe(_ =>
                {
                    if (this && !Dead)
                    {
                        CurrentMoveState.Value = MoveState.LostWaypoint;
                        GetComponent<Animator>().SetBool("Idle", false);
                        Flip();
                        ReachedLimit.Value = false;
                    }
                });
            }
        });

        CurrentMoveState.Subscribe(_=>DisposeAllTimers());

        PlayerInRange.Subscribe(inRange =>
        {
            if (inRange)
            {
                GetComponent<Animator>().SetBool("Idle", false);
                GetComponent<Animator>().speed *= FollowSpeed / WaypointSpeed;
                CurrentMoveState.Value = MoveState.Follow;
            }
            else
            {
                Vector2 stopvel = GetComponent<Rigidbody2D>().velocity;
                stopvel.x = 0.0f;
                GetComponent<Rigidbody2D>().velocity = stopvel;
                GetComponent<Animator>().speed = 1;

                PlayerOnExitRange();
            }
        });
    }

    private void Waypoint()
    {
        Vector3 point = new Vector3(waypoints[currentWaypoint].x, transform.position.y);
        float dist = Vector2.Distance(point, transform.position);

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

        if (dist < 1f)
        {
            GetComponent<Animator>().SetBool("Idle", true);
            CurrentMoveState.Value = MoveState.Waiting;
            IDisposable timer = null;
            timer = Observable.Timer(TimeSpan.FromSeconds(TimeToWait)).Subscribe(_ =>
            {
                if (this && !Dead)
                {
                    currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
                    Flip(waypoints[currentWaypoint]);
                    GetComponent<Animator>().SetBool("Idle", false);
                    CurrentMoveState.Value = MoveState.Waypoint;
                }
                timersDisposables.Remove(timer);
            });
            timersDisposables.Add(timer);
        }
    }

    private void Follow()
    {
        if (Manager.GetInstance().Player.GetComponent<Movement>().Safe)
        {
            PlayerOnExitRange();
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

    private void LostWaypoint()
    {
        if (Right)
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

    private void Waiting()
    {

    }

    private void DisposeAllTimers()
    {
        foreach (var timer in timersDisposables)
        {
            timer.Dispose();
        }
    }
    
    private void PlayerOnExitRange()
    {
        CurrentMoveState.Value = MoveState.Waiting;

        GetComponent<Animator>().SetBool("Idle", true);
        IDisposable timer = null;
        timer = Observable.Timer(TimeSpan.FromSeconds(TimeToWait)).Subscribe(_ =>
        {
            if (this && !Dead)
            {
                if (useWaypoint)
                {
                    CurrentMoveState.Value = MoveState.Waypoint;
                    Flip(waypoints[currentWaypoint]);
                }
                else
                {
                    CurrentMoveState.Value = MoveState.LostWaypoint;
                }
                GetComponent<Animator>().SetBool("Idle", false);
            }
            timersDisposables.Remove(timer);
        });
        timersDisposables.Add(timer);
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
