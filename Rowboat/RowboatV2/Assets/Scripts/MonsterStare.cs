using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStare : MonoBehaviour
{
    void Update()
    {
        transform.LookAt(GameObject.Find("Player").transform);
    }
}
