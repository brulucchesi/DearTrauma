using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{

    private GameObject analytics;
    [SerializeField] private int holeNumber = -1;

    private void Start()
    {
        analytics = GameObject.Find("Analytics");
    }

    public void AttackPlayer()
    {
        GetComponent<Animator>().SetTrigger("Hole");
    }

    public void PlayerDied()
    {

        Debug.Log("Buraco");
        analytics.GetComponent<UnityAnalyticsEvents>().FellIntoHole(holeNumber);

        Manager.GetInstance().Player.GetComponent<Movement>().FallDead();
    }
}
