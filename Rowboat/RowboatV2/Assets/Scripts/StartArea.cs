using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartArea : MonoBehaviour
{
    [SerializeField]
    public new GameObject camera;
    MonsterNoise monsterNoise;
    [SerializeField]
    public GameObject monsterFollower;
    MonsterFollow monsterFollow;

    void Start()
    {
        monsterNoise = camera.GetComponent<MonsterNoise>();
        monsterFollow = monsterFollower.GetComponent<MonsterFollow>();
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Left starting area");
            //I have this check here incase it ever happens a second time
            if(monsterNoise.activated == false)
            {
                monsterNoise.activated = true;
                StartCoroutine(monsterFollow.Rise());
                //Destroy(this.gameObject);
                GetComponent<Collider>().enabled = false;
            }
        }
    }
}
