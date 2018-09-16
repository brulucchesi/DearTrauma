using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour
{

    [Header("References")]
    public RuntimeAnimatorController BigAnimator;
    public GameObject Fragment;
    public SpriteRenderer SpriteDefault;
    public Sprite SpriteBig;
    public GameObject Background;
    public bool EvolutionOn = false;

    [Header("Modifiers")]
    public float ScaleMultiplier = 2f;
    public float ScaleMultiplierBackground = 2.5f;

    private void Start()
    {
        if (EvolutionOn)
        {
            SpriteDefault.sprite = SpriteBig;
            GetComponent<Animator>().runtimeAnimatorController = BigAnimator;
            transform.localScale *= ScaleMultiplier;
            Background.transform.localScale *= ScaleMultiplierBackground;
            Camera.main.GetComponent<CamFollow>().Big();
            transform.position = transform.position + Vector3.up * 3;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fragment"))
        {
            if (Fragment && collision.name == Fragment.name)
            {
                SpriteDefault.sprite = SpriteBig;
                GetComponent<Animator>().runtimeAnimatorController = BigAnimator;
                transform.localScale *= ScaleMultiplier;
                Background.transform.localScale *= ScaleMultiplierBackground;
                Camera.main.GetComponent<CamFollow>().Big();
                transform.position = transform.position + Vector3.up * 3;
            }

            collision.GetComponent<Fragment>().Collect();
        }

    }
}
