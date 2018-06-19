using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour {

    [Header("Modifiers")]
    public LayerMask Mask;
    public Vector2 BoxSize;

    private EnemyMove enemyMove;
    private Vector2 boxCenter;
    private bool lastScan;

    private void Start()
    {
        lastScan = false;
        enemyMove = GetComponent<EnemyMove>();
    }

    private void Update()
    {
        boxCenter = (enemyMove.Right)?
                    (Vector2)enemyMove.RightPos.transform.position + Vector2.right * (BoxSize.x/2):
                    (Vector2)enemyMove.LeftPos.transform.position + Vector2.left * (BoxSize.x / 2);
        Collider2D player = Physics2D.OverlapBox(boxCenter, BoxSize, 0f, Mask);
        bool playerInRange = (player != null);
        if (lastScan != playerInRange)
        {
            if(player && !player.GetComponent<Movement>().Safe)
            {
                enemyMove.StartFollowPlayer(playerInRange);
            }
            else
            {
                enemyMove.StartFollowPlayer(playerInRange);
            }
            lastScan = playerInRange;
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
