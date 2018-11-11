using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    private GameObject analytics;

    [Header("References")]
    public Transform[] PartsToFlip;
    public AudioSource WalkAudio;
    public AudioSource DeathAudio;
    public AudioSource DamageAudio;
    public AudioSource[] HideAudio;

    [Header("Modifiers")]
    public float InitialSpeed;
    public float SpeedHideMod = 2f;

    [HideInInspector]
    public bool Right;

    [HideInInspector]
    public bool Safe;

    [HideInInspector]
    public ReactiveProperty<bool> Dead = new ReactiveProperty<bool>(false);

    private Rigidbody2D rb;
    private Vector2 lastCheckPoint;
    private bool canMove;
    private bool isJumping;
    private float speed;
    private bool canFlip = true;

    void Start()
    {
        analytics = GameObject.Find("Analytics");

        Right = true;
        canMove = true;
        Safe = false;
        rb = GetComponent<Rigidbody2D>();
        lastCheckPoint = transform.position;
        speed = InitialSpeed;
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            float hor = Input.GetAxis("Horizontal");
            if (hor > 0)
            {
                GetComponent<Animator>().SetBool("Mirror", false);
                if (!Right)
                {
                    Flip();
                }
            }
            if (hor < 0)
            {
                GetComponent<Animator>().SetBool("Mirror", true);
                if (Right)
                {
                    Flip();
                }
            }

            Vector2 vel = new Vector2(((Mathf.Abs(hor) - 1) < 0.1f) ? hor * speed * 1.5f : hor * speed, rb.velocity.y);

            rb.velocity = vel;

            if (Mathf.Abs(vel.x) > 0.01f)
            {
                Walk();
            }
            else
            {
                if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                {
                    Stop();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyFront") && !Safe)
        {

            int enemyNumber = collision.gameObject.transform.parent.parent.GetComponent<EnemyMove>().SentEnemyNumber();

            analytics.GetComponent<UnityAnalyticsEvents>().EnemyKilledPlayer(enemyNumber);

            SetCanMove(false);
            collision.GetComponentInParent<EnemyMove>().AttackPlayer();

            DamageAudio.Play();
        }
        else if (collision.CompareTag("Trap"))
        {
            SetCanMove(false);
            collision.GetComponent<Trap>().AttackPlayer();

            DamageAudio.Play();
        }
        else if (collision.CompareTag("Hole"))
        {
            canMove = false;
            collision.GetComponent<Hole>().AttackPlayer();

            DamageAudio.Play();
        }
        else if (collision.CompareTag("Checkpoint"))
        {
            Vector3? pos = collision.GetComponent<Checkpoint>().GetCheckpoint();
            if (pos != null)
            {
                SetCheckpoint(pos.Value);
            }
        }
    }

    public void SetCanMove(bool move)
    {
        rb.velocity = Vector2.zero;
        canMove = move;
        if (!canMove)
        {
            StopWalkAudio(false);
        }
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public void PlayerDied()
    {
        GetComponent<Animator>().SetBool("Dead", true);
        Physics2D.IgnoreLayerCollision(8, 10, true);
        DeathAudio.Play();

        Dead.Value = true;
    }

    public void SetCheckpoint(Vector2 pos)
    {
        lastCheckPoint = pos;
    }

    public void FallDead()
    {
        Dead.Value = true;
        Observable.Timer(System.TimeSpan.FromMilliseconds(100)).Subscribe(_ => ReturnToCheckpoint());
    }

    public void ReturnToCheckpoint()
    {
        GetComponent<Animator>().SetBool("Dead", false);
        Physics2D.IgnoreLayerCollision(8, 10, false);
        transform.position = lastCheckPoint;
        SetCanMove(true);

        Dead.Value = false;
    }

    public void Hide(bool hide)
    {
        Safe = hide;
        speed = (hide) ? InitialSpeed / SpeedHideMod : InitialSpeed;
        GetComponent<Animator>().SetBool("Hidden", hide);

        if (hide)
        {
            HideAudio[Random.Range(0, HideAudio.Length)].Play();
        }
    }

    void Flip()
    {
        if (canFlip)
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

    void Walk()
    {
        GetComponent<Animator>().SetBool("Walking", true);
        if (!WalkAudio.isPlaying && !isJumping)
        {
            WalkAudio.Play();
        }
    }

    void Stop()
    {
        GetComponent<Animator>().SetBool("Walking", false);
        StopWalkAudio(false);
    }

    public void StopWalkAudio(bool isJumpingBool)
    {
        isJumping = isJumpingBool;
        WalkAudio.Stop();
    }

    public void SetCanFlip(bool can)
    {
        canFlip = can;
    }
}
