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

    public int enemyNumber = -1;

    public int SentEnemyNumber()
    {
        return enemyNumber;
    }

    private EnemyMove enemyMove;

    private void Start()
    {
        enemyMove = GetComponentInParent<EnemyMove>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            enemyMove.PlayerInRange.Value = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            enemyMove.PlayerInRange.Value = false;
        }
    }
}
