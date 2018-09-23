using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Evolution : MonoBehaviour
{

    [Header("References")]
    public RuntimeAnimatorController BigAnimator;
    public GameObject Fragment;
    public SpriteRenderer SpriteDefault;
    public Sprite SpriteBig;
    public GameObject Background;

    [Header("Modifiers")]
    public float ScaleMultiplier = 2f;
    public float ScaleMultiplierBackground = 2.5f;
    public BoolReactiveProperty EvolutionOn = new BoolReactiveProperty();

    private void Start()
    {
        EvolutionOn.Subscribe(_ =>
        {
            if (_)
            {
                EvolutionTransition();
            }
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fragment"))
        {
            if (Fragment && collision.name == Fragment.name)
            {
                EvolutionTransition();
            }

            collision.GetComponent<Fragment>().Collect();
        }
    }

    private void EvolutionTransition()
    {
        SpriteDefault.sprite = SpriteBig;
        GetComponent<Animator>().runtimeAnimatorController = BigAnimator;
        transform.localScale *= ScaleMultiplier;
        Background.transform.localScale *= ScaleMultiplierBackground;
        Camera.main.GetComponent<CamFollow>().Big();
        transform.position = transform.position + Vector3.up * 3;
        GetComponent<Jump>().GroundedSkinY *= ScaleMultiplier;
    }
}
