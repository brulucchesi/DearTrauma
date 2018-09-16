using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TutorialElement : MonoBehaviour {

    [Header("References")]
    public Image TutorialImage;

    [Header("Modifiers")]
    public bool ByDistance = true;
    [Header("Distance")]
    public float MinDist = 0f;
    public float MaxDist = 30f;
    [Header("Not Distance")]
    [Space]
    [Range(0,1)]
    public float MaxAlpha = 0.9f;
    public ReactiveProperty<bool> Active = new ReactiveProperty<bool>(true);

    private Vector2 initPos;
    private Color initColor;

    private Vector2 boxSize;
    private Vector2 boxCenter;

    private void Start()
    {
        Color color = TutorialImage.color;
        color.a = 0;
        TutorialImage.color = color;

        Active.Subscribe(a =>
        {
            if(!a)
            {
                Color c = TutorialImage.color;
                c.a = 0;
                TutorialImage.color = c;
            }
        });

        boxSize = new Vector2(MaxDist * 2, 2);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(Active.Value && collision.CompareTag("Player"))
    //    {
    //        initPos = collision.transform.position;
    //        initColor = TutorialImage.color;
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ByDistance)
        {
            return;
        }

        if (Active.Value && collision.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (ByDistance)
        {
            return;
        }

        if (Active.Value && collision.CompareTag("Player"))
        {

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!ByDistance)
        {
            return;
        }

        if (Active.Value && collision.CompareTag("Player"))
        {
            float frac;
            float distPlayer;
            float distTotal;

            if(collision.transform.position.x > transform.position.x + MaxDist || collision.transform.position.x < transform.position.x - MaxDist)
            {
                return;
            }

            if (collision.transform.position.x > transform.position.x)
            { 
                distPlayer = Vector3.Distance(transform.position + Vector3.right * MinDist, collision.transform.position);
                distTotal = Vector3.Distance(transform.position + Vector3.right * MinDist, transform.position + Vector3.right * MaxDist);
            }
            else
            {
                distPlayer = Vector3.Distance(transform.position + Vector3.left * MinDist, collision.transform.position);
                distTotal = Vector3.Distance(transform.position + Vector3.left * MinDist, transform.position + Vector3.left * MaxDist);
            }

            frac = Mathf.Clamp(Mathf.Abs(distTotal - distPlayer),0,distTotal) / distTotal;
            print(frac);
            Color color = TutorialImage.color;
            color.a = Mathf.Lerp(0, MaxAlpha, frac);
            TutorialImage.color = color;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
        {
            Gizmos.DrawCube(transform.position, boxSize);
        }
    }
}
