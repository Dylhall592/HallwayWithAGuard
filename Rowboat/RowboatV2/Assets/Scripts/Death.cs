using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    void OnCollisionStay(Collision collision)
    {
        //If the monster runs into the death bubble on the player
        if (collision.collider.tag == "Monster")
        {
            KillPlayer();
        }
    }

    public void KillPlayer()
    {
        Debug.Log("The player has died");
        //Switch menus or something
    }
}
