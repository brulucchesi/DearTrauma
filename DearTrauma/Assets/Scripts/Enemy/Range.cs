using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{

    enum RangeTarget
    {
        Player,
        Enemy,
        None
    }

    [Header("Modifiers")]
    public LayerMask Mask;
    public Vector2 BoxSize;

    private EnemyMove enemyMove;
    private Vector2 boxCenter;
    private RangeTarget lastTarget;
    private Collider2D storeCollider;
    private RangeTarget storeTarget;

    private void Start()
    {
        lastTarget = RangeTarget.None;
        enemyMove = GetComponent<EnemyMove>();
    }

    private void Update()
    {
        boxCenter = (enemyMove.Right) ?
                    (Vector2)enemyMove.Front.transform.position + Vector2.right * (BoxSize.x / 2) :
                    (Vector2)enemyMove.Front.transform.position + Vector2.left * (BoxSize.x / 2);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, BoxSize, 0f, Mask);
        RangeTarget currentTarget = RangeTarget.None;
        Collider2D currentCollider = null;
        foreach (Collider2D col in colliders)
        {
            if (col.gameObject.layer == 8)
            {
                currentTarget = RangeTarget.Player;
                currentCollider = col;
                break;
            }
            else if (col.gameObject.layer == 10 && !col.isTrigger && col.gameObject != gameObject)
            {
                // Debug.Log("See Enemy");
                currentTarget = RangeTarget.Enemy;
                storeTarget = RangeTarget.Enemy;
                currentCollider = col;
                storeCollider = currentCollider;
            }
        }

        //Debug.Log("current Target " + currentTarget);
       // Debug.Log("Last Target " + lastTarget);

        if (currentTarget != lastTarget)
        {
            if (currentTarget != RangeTarget.None)
            {
                if (currentTarget == RangeTarget.Player)
                {
                    if (!currentCollider.GetComponent<Movement>().Safe)
                    {
                        enemyMove.StartFollow(true, currentCollider.transform, true);
                        lastTarget = currentTarget;
                    }
                    else
                    {
                      //  Debug.Log("Safe");
                    }
                }
                else
                {
                   // Debug.Log("Follow Enemy");
                    enemyMove.StartFollow(true, currentCollider.transform, false);
                    lastTarget = currentTarget;
                }
            }
            else
            {
                // Debug.Log("Stop Follow");
                enemyMove.StartFollow(false, null, false);
                lastTarget = currentTarget;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawCube(boxCenter, BoxSize);
        }
    }

    public bool CurrentTargetEnemy()
    {
        if (storeTarget == RangeTarget.Enemy)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Collider2D CurrentColliderTransform()
    {
        return storeCollider;
    }
}
