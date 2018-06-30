﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

    [Header("Modifiers")]
    public float Speed;
    public float SpeedHideMod = 2f;

    [HideInInspector]
    public bool Right;

    [HideInInspector]
    public bool Safe;

    private Rigidbody2D rb;
    private Vector2 lastCheckPoint;
    private bool canMove;

    void Start()
    {
        canMove = true;
        Safe = false;
        rb = GetComponent<Rigidbody2D>();
        lastCheckPoint = transform.position;
    }

    void Update()
    {
        if(canMove)
        {
            float hor = Input.GetAxis("Horizontal");
            if (hor > 0)
            {
                Right = true;
                GetComponent<Animator>().SetTrigger("Right");
                //hor = Mathf.Clamp(hor, 1f, 1f);
            }
            if (hor < 0)
            {
                Right = false;
                GetComponent<Animator>().SetTrigger("Left");
                //hor = Mathf.Clamp(hor, -1f, -1f);
            }

            Vector2 vel = new Vector2((hor == 1 || hor == -1) ? hor * Speed * 1.5f : hor * Speed, rb.velocity.y);

            rb.velocity = vel;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyFront") && !Safe)
        {
            SetCanMove(false);
            collision.transform.parent.GetComponent<EnemyMove>().AttackPlayer();
        }
        else if (collision.CompareTag("Trap"))
        {
            SetCanMove(false);
            collision.GetComponent<Trap>().AttackPlayer();
        }
        else if (collision.CompareTag("Hole"))
        {
            SetCanMove(false);
            collision.GetComponent<Hole>().AttackPlayer();
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            Vector3? pos = collision.GetComponent<Checkpoint>().GetCheckpoint();
            if(pos != null)
            {
                SetCheckpoint(pos.Value);
            }
        }
    }

    public void SetCanMove(bool move)
    {
        rb.velocity = Vector2.zero;
        canMove = move;
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public void PlayerDied()
    {
        GetComponent<Animator>().SetTrigger("Died");
    }

    public void SetCheckpoint(Vector2 pos)
    {
        lastCheckPoint = pos;
    }

    public void ReturnToCheckpoint()
    {
        transform.position = lastCheckPoint;
        SetCanMove(true);
    }

    public void Hide(bool hide)
    {
        Safe = hide;
        Speed = (hide) ? Speed / SpeedHideMod : Speed * SpeedHideMod;
    }
}
