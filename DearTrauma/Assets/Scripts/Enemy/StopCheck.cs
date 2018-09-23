using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCheck : MonoBehaviour {

    [Header("References")]
    public Transform EnemyBottomRightLimit;

    [Header("Modifiers")]
    public Vector2 BoxSize;
    public LayerMask Mask;

    private Vector2 boxCenter;

    private void Update()
    {
        boxCenter = EnemyBottomRightLimit.position + (GetComponentInParent<EnemyMove>().Right ?
                                                     Vector3.right * BoxSize.x/2:
                                                     Vector3.left * BoxSize.x / 2);
        if(Physics2D.OverlapBox(boxCenter, BoxSize, 0f, Mask) == null)
        {
            if (GetComponentInParent<EnemyMove>().LostWaypoint.Value == false)
            {
                GetComponentInParent<EnemyMove>().LostWaypoint.Value = true;
            }
            if (GetComponentInParent<EnemyMove>().ReachedLimit.Value == false)
            {
                GetComponentInParent<EnemyMove>().ReachedLimit.Value = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != 8 && collision.gameObject.layer != 13)
        {
            if(GetComponentInParent<EnemyMove>().LostWaypoint.Value == false)
            {
                GetComponentInParent<EnemyMove>().LostWaypoint.Value = true;
            }
            if (GetComponentInParent<EnemyMove>().ReachedLimit.Value == false)
            {
                GetComponentInParent<EnemyMove>().ReachedLimit.Value = true;
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
