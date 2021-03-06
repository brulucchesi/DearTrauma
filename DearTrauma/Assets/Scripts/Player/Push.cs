﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    [Header("References")]
    public AudioSource Audio;

    [Header("Modifiers")]
    public LayerMask boxMask;
    public float Skin = 1f;

    [HideInInspector]
    public bool Pushing;

    private GameObject pushable;
    private Vector2 playerSize;
    private Vector2 boxSize;
    private Vector2 boxSizeDown;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        Pushing = false;
        anim = GetComponent<Animator>();
        pushable = null;
        playerSize = GetComponent<CapsuleCollider2D>().size;
        boxSize = new Vector2(playerSize.x/2 + (Skin * 2f), playerSize.y - (Skin * 2f));
        boxSizeDown = boxSize * 0.9f;
    }

    // Update is called once per frame
    void Update()
    {
        Physics2D.queriesStartInColliders = false;
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, (GetComponent<Movement>().Right)?Vector2.right: Vector2.left, distance, boxMask);

        //pode ter colLeft e colRight pra push e pull
        Collider2D col = Physics2D.OverlapBox(transform.position +
                                (GetComponent<Movement>().Right ? Vector3.right * boxSize.x / 2 : Vector3.left * boxSize.x / 2),
                                boxSize, 0f, boxMask);

        Collider2D colDown = Physics2D.OverlapBox(transform.position + Vector3.down * playerSize.y / 2f,
                                boxSizeDown, 0f, boxMask);

        Pushing = false;
        if (col != null && col.gameObject.tag == "Pushable" && !Manager.GetInstance().Paused)
        {
            pushable = col.gameObject;

            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) 
                && GetComponent<Jump>().jumpCount.Value == 2 && !GetComponent<Animator>().GetBool("Descendo")
                && !GetComponent<Animator>().GetBool("Subindo"))
                /*&& GetComponent<Animator>().GetBool("Grounded")*/)
            {
                Pushing = true;

                pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                pushable.GetComponent<Joint2D>().enabled = true;
                pushable.GetComponent<Joint2D>().connectedBody = GetComponent<Rigidbody2D>();

                if (anim)
                {
                    GetComponent<Movement>().SetCanFlip(false);
                    anim.SetBool("Pushing", true);
                    anim.SetBool("Attack", false);

                    float vel = 0.05f;

                    if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < vel)
                    {
                        anim.SetBool("pushing_back", false);
                        anim.SetBool("pushing_stop", true);
                        Audio.Stop();
                    }

                    if (GetComponent<Movement>().IsRight())
                    {
                        if (GetComponent<Rigidbody2D>().velocity.x >= vel)
                        {
                            anim.SetBool("pushing_stop", false);
                            anim.SetBool("pushing_back", false);
                            if (!Audio.isPlaying)
                            {
                                Audio.Play();
                            }
                        }

                        if (GetComponent<Rigidbody2D>().velocity.x <= -vel)
                        {
                            anim.SetBool("pushing_stop", false);
                            anim.SetBool("pushing_back", true);
                            if (!Audio.isPlaying)
                            {
                                Audio.Play();
                            }
                        }
                    }
                    else
                    {
                        if (GetComponent<Rigidbody2D>().velocity.x <= -vel)
                        {
                            anim.SetBool("pushing_stop", false);
                            anim.SetBool("pushing_back", false);
                            if (!Audio.isPlaying)
                            {
                                Audio.Play();
                            }
                        }

                        if (GetComponent<Rigidbody2D>().velocity.x >= vel)
                        {
                            anim.SetBool("pushing_stop", false);
                            anim.SetBool("pushing_back", true);
                            if (!Audio.isPlaying)
                            {
                                Audio.Play();
                            }
                        }
                    }
                }
            }
            else
            {
                pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                pushable.GetComponent<Joint2D>().enabled = false;
                pushable.GetComponent<Joint2D>().connectedBody = null;
                Audio.Stop();

                if (anim)
                {
                    anim.SetBool("Pushing", false);
                    anim.SetBool("pushing_back", false);
                    anim.SetBool("pushing_stop", false);
                    GetComponent<Movement>().SetCanFlip(true);
                }
                if (GetComponent<Jump>().jumpCount.Value < 2)
                {
                    pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
        else
        {
            if (pushable)
            {
                pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                pushable.GetComponent<Joint2D>().enabled = false;
                pushable.GetComponent<Joint2D>().connectedBody = null;
                Audio.Stop();

                pushable = null;
            }

            if (anim)
            {
                anim.SetBool("Pushing", false);
                GetComponent<Movement>().SetCanFlip(true);
            }

            if (colDown)
            {
                colDown.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(transform.position + (GetComponent<Movement>().Right ? 
                        Vector3.right * boxSize.x / 2 : Vector3.left * boxSize.x / 2), boxSize);
        Gizmos.DrawCube(transform.position + Vector3.down * playerSize.y / 2f, boxSizeDown);
    }
}
