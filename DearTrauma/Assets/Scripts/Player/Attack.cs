using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    [Header("References")]
    public GameObject AttackPoint;

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
	    if(Input.GetButtonDown("Attack"))
        {
            StartAttack();
        }
	}

    private void StartAttack()
    {
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
            Gizmos.DrawCube(AttackPoint.transform.position +
                           (GetComponent<Movement>().Right ? Vector3.right * attackSize.x / 2 : Vector3.left * attackSize.x / 2),
                           attackSize);
        }
    }
}
