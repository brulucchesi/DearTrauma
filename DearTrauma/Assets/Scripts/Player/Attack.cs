using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    [Header("References")]
    public GameObject AttackPoint;
    public AudioSource AttackAudio;
    public ParticleSystem AttackLeft;
    public ParticleSystem AttackRight;
    public ParticleSystem AttackLeftBig;
    public ParticleSystem AttackRightBig;

    [Header("Modifiers")]
    public LayerMask Mask;

    private Animator anim;
    public Vector2 attackSize;

    private void Start()
    {
        anim = GetComponent<Animator>();
        Vector2 playerSize = GetComponent<CapsuleCollider2D>().size;
        //attackSize = new Vector2(playerSize.x/2, playerSize.y);
    }
    
    void Update ()
    {
	    if(Input.GetButtonDown("Attack") && GetComponent<Movement>().GetCanMove() && !GetComponent<Push>().Pushing && !Manager.GetInstance().Paused)
        {
            StartAttack();
        }

        if(!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            AttackParticleEnd();
        }
	}

    private void StartAttack()
    {
        AttackAudio.Play();
        if (anim)
        {
            anim.SetTrigger("Attack");
        }
        else
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(AttackPoint.transform.position + 
                                (GetComponent<Movement>().Right ? Vector3.right * attackSize.x/2: Vector3.left * attackSize.x / 2),
                                attackSize, 0f, Mask);
        foreach (Collider2D col in colliders)
        {
            col.gameObject.SendMessage("ReceiveAttack",SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(AttackPoint.transform.position +
                           (GetComponent<Movement>().Right ? Vector3.right * attackSize.x / 2 : Vector3.left * attackSize.x / 2),
                           attackSize);
        }
    }

    public void AttackParticleStart()
    {
        if(GetComponent<Movement>().Right)
        {
            AttackRight.Stop();
            AttackRight.Play();
            AttackLeft.Stop();

            AttackRightBig.Stop();
            AttackRightBig.Play();
            AttackLeftBig.Stop();
        }
        else
        {
            AttackLeft.Stop();
            AttackLeft.Play();
            AttackRight.Stop();

            AttackLeftBig.Stop();
            AttackLeftBig.Play();
            AttackRightBig.Stop();
        }
    }

    public void AttackParticleEnd()
    {
        AttackRight.Stop();
        AttackLeft.Stop();

        AttackRightBig.Stop();
        AttackLeftBig.Stop();
    }
}
