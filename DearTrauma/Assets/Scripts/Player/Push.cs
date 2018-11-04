﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour {

    [Header("Modifiers")]
    public LayerMask boxMask;
    public float Skin = 1f;

    private GameObject pushable;
    private Vector2 playerSize;
    private Vector2 boxSize;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        pushable = null;
        playerSize = GetComponent<CapsuleCollider2D>().size;
        boxSize = new Vector2(playerSize.x / 2f + (Skin * 2f), playerSize.y/2f);
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, (GetComponent<Movement>().Right)?Vector2.right: Vector2.left, distance, boxMask);

        //pode ter colLeft e colRight pra push e pull
        Collider2D col = Physics2D.OverlapBox(transform.position /*+
                                (GetComponent<Movement>().Right ? Vector3.right * boxSize.x / 2 : Vector3.left * boxSize.x / 2)*/,
                                boxSize, 0f, boxMask);

        if (col != null && col.gameObject.tag == "Pushable")
        {
            pushable = col.gameObject;

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                pushable.GetComponent<FixedJoint2D>().enabled = true;
                pushable.GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                //pushable.GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                //pushable.GetComponent<FixedJoint2D>().enabled = true;
                //pushable.GetComponent<Pushable>().beingPushed = true;

                if (anim)
                {
                    anim.SetBool("Pushing", true);
                }
            }
            else
            {
                pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                pushable.GetComponent<FixedJoint2D>().enabled = false;
                pushable.GetComponent<FixedJoint2D>().connectedBody = null;

                if (anim)
                {
                    anim.SetBool("Pushing", false);
                }
            }
        }
        else
        {
            if (pushable)
            {
                pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                pushable.GetComponent<FixedJoint2D>().enabled = false;
                pushable.GetComponent<FixedJoint2D>().connectedBody = null;
            }

            if (anim)
            {
                anim.SetBool("Pushing", false);
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * boxSize.x / 2);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * boxSize.x / 2);
    }
}
