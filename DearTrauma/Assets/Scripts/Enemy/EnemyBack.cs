using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBack : MonoBehaviour
{

    private GameObject analytics;

    private void Start()
    {
        analytics = GameObject.Find("Analytics");
    }

	public void ReceiveAttack()
    {

        int enemyNumber = gameObject.transform.parent.parent.GetComponent<Range>().SentEnemyNumber();

        analytics.GetComponent<UnityAnalyticsEvents>().PlayerKilledEnemy(enemyNumber);

        Destroy(transform.parent.parent.gameObject);
    }
}
