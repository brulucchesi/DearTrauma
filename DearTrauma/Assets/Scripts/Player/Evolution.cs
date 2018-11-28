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
    public GameObject Visual;
    public GameObject VisualBig;
    public AudioSource TransformAudio;

    [Header("Modifiers")]
    public float ScaleMultiplier = 2f;
    public float ScaleMultiplierBackground = 2.5f;
    public BoolReactiveProperty EvolutionOn = new BoolReactiveProperty();

    private bool evolution;

    private void Start()
    {
        Visual.SetActive(true);
        VisualBig.SetActive(false);

        EvolutionOn.Subscribe(_ =>
        {
            if (_)
            {
                StartCoroutine("Wait");
            }
        });
    }

    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        InitiateTransform();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fragment"))
        {
            if (Fragment && collision.name == Fragment.name)
            {
                if (!evolution)
                {
                    evolution = true;
                }
            }

            collision.GetComponent<Fragment>().Collect(this.gameObject);
        }
    }

    public void InitiateTransform()
    {
        GetComponent<Movement>().SetCanMove(false);
        GetComponent<Animator>().SetBool("Walking", false);
        Visual.SetActive(false);
        VisualBig.SetActive(true);
        GetComponent<Animator>().runtimeAnimatorController = BigAnimator;
        //transform.localScale *= ScaleMultiplier;
        transform.position = transform.position + Vector3.up * 3;
        GetComponent<Jump>().GroundedSkinY *= ScaleMultiplier;
        GetComponent<Jump>().JumpVelocity *= ScaleMultiplier;
        GetComponent<Jump>().DoubleJumpVelocity *= ScaleMultiplier;
        GetComponent<CapsuleCollider2D>().size = new Vector2(1.3f, 3.6f);
        GetComponent<CapsuleCollider2D>().offset = new Vector2(0.0f, 0.1f);

        GetComponent<Animator>().SetBool("Transforming", true);
        TransformAudio.Play();
    }

    private void EndTransformation()
    {
        GetComponent<Animator>().SetBool("Transforming", false);
        GetComponent<Movement>().SetCanMove(true);
    }
}
