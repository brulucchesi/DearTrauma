using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBack : MonoBehaviour {

	public void ReceiveAttack()
    {
        Debug.Log("me atacaram :(");
        Destroy(transform.parent.gameObject);
    }
}
