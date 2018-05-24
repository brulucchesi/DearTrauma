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
        Vector2 playerSize = GetComponent<BoxCollider2D>().size;
        attackSize = new Vector2(playerSize.x/2, playerSize.y);
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
            anim.SetTrigger(GetComponent<Movement>().Right?"AttackRight": "AttackLeft");
        }
        else
        {
            PerformAttack();
        }
    }

    public void PerformAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(AttackPoint.transform.position, attackSize, 0f, Mask);
        foreach (Collider2D col in colliders)
        {
            col.gameObject.SendMessage("ReceiveAttack");
        }
    }
}
