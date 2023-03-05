using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollider : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.collider);
        Debug.Log(collision.collider.tag);
    }
}
