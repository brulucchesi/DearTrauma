using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump : MonoBehaviour {

    [Header("Modifiers")]
    [Range(1,10)]
    public float JumpVelocity;
    public float FallMultiplier = 2.5f;
    public float LowJumpMultiplier = 2f;

    public float GroundedSkin = 0.05f;
    public LayerMask Mask;

    private Rigidbody2D rb;
    private bool jumpPress;
    private bool grounded;
    private bool doubleJump;

    private Vector2 playerSize;
    private Vector2 boxSize;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<BoxCollider2D>().size;
        boxSize = new Vector2(playerSize.x - (GroundedSkin * 2), GroundedSkin);
        doubleJump = true;
    }
    
    void Update ()
    {
        if(Input.GetButtonDown("Jump") && (grounded || doubleJump) && GetComponent<Movement>().GetCanMove())
        {
            jumpPress = true;
            if(!grounded)
            {
                doubleJump = false;
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.gravityScale = FallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = LowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1f;
        }
	}

    private void FixedUpdate()
    {
        if(jumpPress)
        {
            rb.AddForce(Vector2.up * JumpVelocity, ForceMode2D.Impulse);
            jumpPress = false;
            grounded = false;
        }
        else
        {
            Vector2 boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
            grounded = (Physics2D.OverlapBox(boxCenter, boxSize, 0f, Mask) != null);
            if(grounded)
            {
                doubleJump = true;
            }
        }
    }
}
