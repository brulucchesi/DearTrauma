using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour {

    [Header("Modifiers")]
    public float Speed;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");

        Vector2 vel = new Vector2((hor == 1 || hor == -1)? hor * Speed * 1.5f: hor * Speed, rb.velocity.y);

        rb.velocity = vel;
	}
}
