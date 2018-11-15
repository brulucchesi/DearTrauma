using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePhysicsMat : MonoBehaviour {

    public PhysicsMaterial2D ZeroFriction;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == 9 || collision.gameObject.layer == 12) && GetComponent<Jump>().jumpCount.Value < 2)
        {
            collision.gameObject.GetComponent<Collider2D>().sharedMaterial = ZeroFriction;
        }
        else
        {
            collision.gameObject.GetComponent<Collider2D>().sharedMaterial = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9 || collision.gameObject.layer == 12)
        {
            collision.gameObject.GetComponent<Collider2D>().sharedMaterial = null;
        }
    }
}
