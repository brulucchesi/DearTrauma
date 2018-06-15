﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour {

    [Header("Modifiers")]
    public float distance = 1f;
    public LayerMask boxMask;

    private GameObject pushable;

    // Use this for initialization
    void Start()
    {
        pushable = null;
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, (GetComponent<Movement>().Right)?Vector2.right: Vector2.left, distance, boxMask);

        if (hit.collider != null && hit.collider.gameObject.tag == "Pushable")
        {
            pushable = hit.collider.gameObject;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                //pushable.GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();
                //pushable.GetComponent<FixedJoint2D>().enabled = true;
                //pushable.GetComponent<Pushable>().beingPushed = true;
            }
            //else if (Input.GetKeyUp(KeyCode.LeftShift))
            //{
            //    pushable.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation);
            //    //pushable.GetComponent<FixedJoint2D>().enabled = false;
            //    //pushable.GetComponent<Pushable>().beingPushed = false;
            //}
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if(pushable)
            {
                pushable.GetComponent<Rigidbody2D>().constraints = (RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation);
                //pushable.GetComponent<FixedJoint2D>().enabled = false;
                //pushable.GetComponent<Pushable>().beingPushed = false;
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + Vector2.right * transform.localScale.x * distance);
    }
}
