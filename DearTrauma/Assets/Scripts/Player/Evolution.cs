using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour
{

    [Header("References")]
    public RuntimeAnimatorController BigAnimator;

    public GameObject Fragment;

    [Header("Modifiers")]
    public float ScaleMultiplier = 2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == Fragment.name)
        {
            GetComponent<Animator>().runtimeAnimatorController = BigAnimator;
            transform.localScale *= ScaleMultiplier;
            Camera.main.GetComponent<CamFollow>().Big();
            //GetComponent<Rigidbody2D>().AddForce(Vector2.up * 7, ForceMode2D.Impulse);
            transform.position = transform.position + Vector3.up * 3;
            collision.GetComponent<Fragment>().Collect();
        }
    }
}
