using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCheck : MonoBehaviour {

    [Header("References")]
    public Transform EnemyBottomRightLimit;

    [Header("Modifiers")]
    public Vector2 BoxSize;
    public LayerMask GroundMask;
    public LayerMask StopMask;

    private Vector2 boxCenter;

    private void Update()
    {
        boxCenter = EnemyBottomRightLimit.position + (GetComponentInParent<EnemyMove>().Right ?
                                                     Vector3.right * BoxSize.x/2:
                                                     Vector3.left * BoxSize.x / 2);
        if(Physics2D.OverlapBox(boxCenter, BoxSize, 0f, GroundMask) == null)
        {
            if (GetComponentInParent<EnemyMove>().ReachedGroundLimit.Value == false)
            {
                GetComponentInParent<EnemyMove>().ReachedGroundLimit.Value = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (StopMask == (StopMask | (1 << collision.gameObject.layer)))
        {
            if (GetComponentInParent<EnemyMove>().ReachedWallLimit.Value == false)
            {
                GetComponentInParent<EnemyMove>().ReachedWallLimit.Value = true;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (StopMask == (StopMask | (1 << collision.gameObject.layer)))
        {
            if (GetComponentInParent<EnemyMove>().ReachedWallLimit.Value == false)
            {
                GetComponentInParent<EnemyMove>().ReachedWallLimit.Value = true;
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
}
