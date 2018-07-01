using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour {

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

    private void Start()
    {
        lastTarget = RangeTarget.None;
        enemyMove = GetComponent<EnemyMove>();
    }

    private void Update()
    {
        boxCenter = (enemyMove.Right)?
                    (Vector2)enemyMove.RightPos.transform.position + Vector2.right * (BoxSize.x/2):
                    (Vector2)enemyMove.LeftPos.transform.position + Vector2.left * (BoxSize.x / 2);
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCenter, BoxSize, 0f, Mask);
        RangeTarget currentTarget = RangeTarget.None;
        Collider2D currentCollider = null;
        foreach(Collider2D col in colliders)
        {
            if(col.gameObject.layer == 8)
            {
                currentTarget = RangeTarget.Player;
                currentCollider = col;
                break;
            }
            else if (col.gameObject.layer == 10 && !col.isTrigger && col.gameObject != gameObject)
            {
                currentTarget = RangeTarget.Enemy;
                currentCollider = col;
            }
        }
        if (currentTarget != lastTarget)
        {
            if(currentTarget != RangeTarget.None)
            {
                if (currentTarget == RangeTarget.Player)
                {
                    if(!currentCollider.GetComponent<Movement>().Safe)
                    {
                        enemyMove.StartFollow(true, currentCollider.transform, true);
                    }
                }
                else
                {
                    enemyMove.StartFollow(true, currentCollider.transform, false);
                }
            }
            else
            {
                enemyMove.StartFollow(false, null, false);
            }
            lastTarget = currentTarget;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(Application.isPlaying)
        {
            Gizmos.DrawCube(boxCenter, BoxSize);
        }
    }
}
