using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Pushable : MonoBehaviour {
    
    private Vector3 initialPos;

    void Start()
    {
        initialPos = transform.position;

        Manager.GetInstance().Player.GetComponent<Movement>().Dead.Where(dead => dead == true).Subscribe(dead =>
        {
            StartCoroutine(WaitFadeFall());
        });
    }

    IEnumerator WaitFadeFall()
    {
        yield return new WaitUntil(() => Manager.GetInstance().FadeMiddle);

        ResetPos();
    }

    private void ResetPos()
    {
        transform.position = initialPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 10)
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}
