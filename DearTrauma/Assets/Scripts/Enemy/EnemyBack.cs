using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBack : MonoBehaviour
{
    [Header("References")]
    public EnemyMove enemyMove;

    private GameObject analytics;

    private void Start()
    {
        analytics = GameObject.Find("Analytics");
    }

	public void ReceiveAttack()
    {
        print("receive");
        if (Manager.GetInstance().Player.GetComponent<Movement>().GetCanMove() &&
           (Manager.GetInstance().Player.GetComponent<Movement>().Right == GetComponentInParent<EnemyMove>().Right))
        {
            int enemyNumber = enemyMove.SentEnemyNumber();

            analytics.GetComponent<UnityAnalyticsEvents>().PlayerKilledEnemy(enemyNumber);

            GetComponentInParent<Animator>().SetBool("Died", true);
            GetComponentInParent<EnemyMove>().Dead = true;
            GetComponentInParent<EnemyMove>().DeathAudio.Play();
            GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
