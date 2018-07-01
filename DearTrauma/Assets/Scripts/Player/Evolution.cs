using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour {

    [Header("References")]
    public RuntimeAnimatorController BigAnimator;

    [Header("Modifiers")]
    public float ScaleMultiplier = 2f;

    private int fragmentCounter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Fragment"))
        {
            fragmentCounter++;
            if(fragmentCounter == 3)
            {
                GetComponent<Animator>().runtimeAnimatorController = BigAnimator;
                transform.localScale *= ScaleMultiplier;
                Camera.main.GetComponent<CamFollow>().Big();
                //transform.Translate(transform.position + (Vector3.up * 2));
            }
        }
    }
}
