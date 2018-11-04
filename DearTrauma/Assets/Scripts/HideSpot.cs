using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour {

    [Header("Modifiers")]
    public LayerMask boxMask;

    private Vector2 boxSize;
    private GameObject Player;

    private void Start()
    {
        boxSize = GetComponent<BoxCollider2D>().size;
    }

    private void Update()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(transform.position, boxSize, 0, boxMask);

        if(cols.Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                GetComponent<Animator>().SetBool("Hide", true);
                cols[0].GetComponent<Movement>().Hide(true);
                Physics2D.IgnoreLayerCollision(8, 10, true);
                Player = cols[0].gameObject;
            }
            else if ((Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)) && Player)
            {
                GetComponent<Animator>().SetBool("Hide", false);
                Player.GetComponent<Movement>().Hide(false);
                Physics2D.IgnoreLayerCollision(8, 10, false);
                Player = null;
            }
        }
        else if(Player)
        {
            GetComponent<Animator>().SetBool("Hide", false);
            Player.GetComponent<Movement>().Hide(false);
            Physics2D.IgnoreLayerCollision(8, 10, false);
            Player = null;
        }
    }
}
