using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

    [Header("Modifiers")]
    public float Speed;

    [HideInInspector]
    public bool Right;

    private Rigidbody2D rb;
    private Vector2 lastCheckPoint;
    private bool canMove;

    void Start()
    {
        canMove = true;
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
                //hor = Mathf.Clamp(hor, 1f, 1f);
            }
            if (hor < 0)
            {
                Right = false;
                //hor = Mathf.Clamp(hor, -1f, -1f);
            }

            Vector2 vel = new Vector2((hor == 1 || hor == -1) ? hor * Speed * 1.5f : hor * Speed, rb.velocity.y);

            rb.velocity = vel;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyFront"))
        {
            collision.transform.parent.GetComponent<EnemyMove>().AttackPlayer();
        }
    }

    public void SetCanMove(bool move)
    {
        rb.velocity = Vector2.zero;
        canMove = move;
    }

    public void PlayerDied()
    {
        SetCanMove(false);
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
}
