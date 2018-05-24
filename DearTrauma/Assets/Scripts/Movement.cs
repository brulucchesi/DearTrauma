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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Right = true;
            rb.velocity = new Vector2(Speed, rb.velocity.y);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Right = false;
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        //Vector2 vel = new Vector2((hor == 1 || hor == -1)? hor * Speed * 1.5f: hor * Speed, rb.velocity.y);

        //rb.velocity = vel;
	}
}
