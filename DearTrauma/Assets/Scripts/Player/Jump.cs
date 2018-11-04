using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody2D))]
public class Jump : MonoBehaviour
{

    [Header("References")]
    public AudioSource JumpAudio;
    public AudioSource LandAudio;

    [Header("Modifiers")]
    public float JumpVelocity;
    public float DoubleJumpVelocity;
    public float FallGravity = 2.5f;
    public float JumpGravity = 2f;

    public float GroundedSkinX = 0.05f;
    public float GroundedSkinY = 0.05f;
    public LayerMask Mask;

    private Rigidbody2D rb;
    private ReactiveProperty<bool> jumpPress = new ReactiveProperty<bool>(false);
    private bool grounded;
    private bool touching;

    private Vector2 playerSize;
    private Vector2 boxSize;
    private Vector2 boxCenter;

    private Animator anim;

    private ReactiveProperty<int> jumpCount = new ReactiveProperty<int>(2);
    private bool canResetJump;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerSize = GetComponent<CapsuleCollider2D>().size;
        boxSize = new Vector2(playerSize.x - (GroundedSkinX * 2), GroundedSkinY);

        canResetJump = true;
        anim.SetBool("grounded", true);

        jumpPress.Subscribe(jumpPress =>
        {
            if (jumpPress)
            {
                CalculateJump();
            }
        });
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(0, 0, 1, 0.5f);
            Gizmos.DrawCube(boxCenter, boxSize);
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && (jumpCount.Value > 0) && GetComponent<Movement>().GetCanMove())
        {
            jumpPress.Value = true;
        }
        boxCenter = (Vector2)transform.position + Vector2.down * (playerSize.y + boxSize.y) * 0.5f;
        boxSize = new Vector2(playerSize.x - (GroundedSkinX * 2), GroundedSkinY);
        grounded = (Physics2D.OverlapBox(boxCenter, boxSize, 0f, Mask) != null) && touching;

        if (rb.velocity.y < -0.1f && !grounded)
        {
            rb.gravityScale = FallGravity;
            anim.SetBool("Descendo", true);
        }
        else if (rb.velocity.y > 2f/* && !Input.GetButton("Jump")*/)
        {
            rb.gravityScale = JumpGravity;
            anim.SetBool("Subindo", true);
        }
        else
        {
            rb.gravityScale = 1f;
            anim.SetBool("Subindo", false);
            anim.SetBool("Descendo", false);
        }

        if (jumpCount.Value < 2 || rb.velocity.y < -2f)
        {
            anim.SetBool("grounded", false);
        }

        if (grounded)
        {
            if (canResetJump)
            {
                jumpCount.Value = 2;
                canResetJump = false;

                GetComponent<Movement>().StopWalkAudio(false);
            }
            anim.SetBool("grounded", true);
        }
    }

    private void CalculateJump()
    {
        GetComponent<Movement>().StopWalkAudio(true);
        JumpAudio.Play();
        if (jumpCount.Value == 1)
        {
            Vector3 vel = rb.velocity;
            vel.y = 0;
            rb.velocity = vel;

            rb.AddForce(Vector2.up * DoubleJumpVelocity, ForceMode2D.Impulse);
        }
        else if (jumpCount.Value == 2)
        {
            rb.AddForce(Vector2.up * JumpVelocity, ForceMode2D.Impulse);
        }

        if (anim)
        {
            anim.SetBool("grounded", false);
            anim.SetBool("Subindo", true);
            anim.SetBool("Descendo", false);
        }

        jumpCount.Value--;
        jumpPress.Value = false;

        Observable.Timer(System.TimeSpan.FromMilliseconds(500)).Subscribe(_ =>
        {
            canResetJump = true;
        });
    }

    public void EndLand()
    {
        anim.SetBool("Land", false);
    }

    private void Land()
    {
        anim.SetBool("Land", true);
        LandAudio.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if(collision.gameObject.layer == 9)
        //{
        touching = true;
        //}
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //if (collision.gameObject.layer == 9)
        //{
        touching = false;
        //}
    }
}
