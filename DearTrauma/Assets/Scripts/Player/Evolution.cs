using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Anima2D;

public class Evolution : MonoBehaviour
{
    [Header("References")]
    public RuntimeAnimatorController BigAnimator;
    public GameObject Fragment;
    public GameObject Background;
    public GameObject Visual;
    public GameObject VisualBig;
    public AudioSource TransformAudio;

    [Header("Modifiers")]
    public float ScaleMultiplier = 2f;
    public float ScaleMultiplierBackground = 2.5f;
    public BoolReactiveProperty EvolutionOn = new BoolReactiveProperty();

    private void Start()
    {
        Visual.SetActive(true);
        VisualBig.SetActive(false);

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

            collision.GetComponent<Fragment>().Collect(this.gameObject);
        }
    }

    private void EvolutionTransition()
    {
        Visual.SetActive(false);
        VisualBig.SetActive(true);
        GetComponent<Animator>().runtimeAnimatorController = BigAnimator;
        //transform.localScale *= ScaleMultiplier;
        Background.transform.localScale *= ScaleMultiplierBackground;
        Camera.main.GetComponent<CamFollow>().Big();
        transform.position = transform.position + Vector3.up * 3;
        GetComponent<Jump>().GroundedSkinY *= ScaleMultiplier;
        GetComponent<Jump>().JumpVelocity *= ScaleMultiplier;
        GetComponent<Jump>().DoubleJumpVelocity *= ScaleMultiplier;

        TransformAudio.Play();
    }
}
