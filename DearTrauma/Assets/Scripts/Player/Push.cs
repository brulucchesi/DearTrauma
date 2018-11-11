using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour {

    [Header("Modifiers")]
    public LayerMask boxMask;
    public float Skin = 1f;

    private GameObject pushable;
    private Vector2 playerSize;
    private Vector2 boxSize;
    private Vector2 boxSizeDown;

    private Animator anim;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        pushable = null;
        playerSize = GetComponent<CapsuleCollider2D>().size;
        boxSize = new Vector2(playerSize.x + (Skin * 2f), playerSize.y/2f);
        boxSizeDown = boxSize * 0.9f;
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

        Collider2D colDown = Physics2D.OverlapBox(transform.position + Vector3.down * playerSize.y/2f,
                                boxSizeDown, 0f, boxMask);

        if (col != null && col.gameObject.tag == "Pushable")
        {
            pushable = col.gameObject;

            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                pushable.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                pushable.GetComponent<FixedJoint2D>().enabled = true;
                pushable.GetComponent<FixedJoint2D>().connectedBody = GetComponent<Rigidbody2D>();

                if (anim)
                {
                    GetComponent<Movement>().SetCanFlip(false);
                    anim.SetBool("Pushing", true);

                    if(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) < 0.01f)
                    {
                        anim.SetBool("pushing_back", false);
                        anim.SetBool("pushing_stop", true);
                    }

                    if(GetComponent<Rigidbody2D>().velocity.x >= 0.01f)
                    {
                        anim.SetBool("pushing_stop", false);
                        anim.SetBool("pushing_back", false);
                    }

                    if(GetComponent<Rigidbody2D>().velocity.x <= -0.01f)
                    {
                        anim.SetBool("pushing_stop", false);
                        anim.SetBool("pushing_back", true);
                    }
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
                    anim.SetBool("pushing_back", false);
                    anim.SetBool("pushing_stop", false);
                    GetComponent<Movement>().SetCanFlip(true);
                }
                if(GetComponent<Jump>().jumpCount.Value < 2)
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
                pushable.GetComponent<FixedJoint2D>().enabled = false;
                pushable.GetComponent<FixedJoint2D>().connectedBody = null;
            }

            if (anim)
            {
                anim.SetBool("Pushing", false);
            }

            if(colDown)
            {
                colDown.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,1,0,0.5f);
        Gizmos.DrawCube(transform.position, boxSize);
        Gizmos.DrawCube(transform.position + Vector3.down * playerSize.y / 2f, boxSizeDown);
    }
}
