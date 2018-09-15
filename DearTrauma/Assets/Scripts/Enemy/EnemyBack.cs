using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBack : MonoBehaviour {

	public void ReceiveAttack()
    {
        Destroy(transform.parent.parent.gameObject);
    }
}
