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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastCheckPoint = transform.position;
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        if(hor>0)
        {
            Right = true;
            //hor = Mathf.Clamp(hor, 1f, 1f);
        }
        if (hor < 0)
        {
            Right = false;
            //hor = Mathf.Clamp(hor, -1f, -1f);
        }

        Vector2 vel = new Vector2((hor == 1 || hor == -1)? hor * Speed * 1.5f: hor * Speed, rb.velocity.y);

        rb.velocity = vel;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("EnemyFront"))
        {
            ReturnToCheckpoint();
        }
    }

    public void SetCheckpoint(Vector2 pos)
    {
        lastCheckPoint = pos;
    }

    public void ReturnToCheckpoint()
    {
        rb.velocity = Vector2.zero;
        transform.position = lastCheckPoint;
    }
}
