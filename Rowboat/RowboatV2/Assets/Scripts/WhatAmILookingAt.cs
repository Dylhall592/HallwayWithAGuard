using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhatAmILookingAt : MonoBehaviour
{
    [SerializeField]
    public float sightDistance = 500f;
    [SerializeField]
    private bool looking = false;

    public Vector3 collision = Vector3.zero;
    public LayerMask layer;

    void Update()
    {
        //Visual for when the game is paused to see where the player is looking and how far it goes
        Vector3 forward = transform.TransformDirection(Vector3.forward) * sightDistance;
        Debug.DrawRay(transform.position, forward, Color.green);

        looking = false;
        //Raycast that shoots out from the players eyeballs
        var ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(GetComponent<Camera>().transform.position, forward, sightDistance);
        for (int i = 0; i < hits.Length; i++)
         {
            RaycastHit hit = hits[i];
            //Any bad tags that should block the player's line of sight
            if(hit.collider.tag == "MonsterSightRange")
            {
                looking = true;
            }
        }
    }

    public bool LookingAtMonster()
    {
        return looking;
    }

}
