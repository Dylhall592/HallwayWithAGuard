using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFollow : MonoBehaviour
{
    //~~~~~~~~~~~~~~~~~~~~~~~~Objects~~~~~~~~~~~~~~~~~~~~~~~~
    [SerializeField]
    private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    public new GameObject camera;
    [SerializeField]
    private GameObject monsterSpawner;
    MonsterFind monsterFind;
    private GameObject lookBackBubble;
    //~~~~~~~~~~~~~~~~~~~~~~~~Things you can edit~~~~~~~~~~~~~~~~~~~~~~~~
    //How high the monster rises from the water
    private float riseHeight = 2f;
    //How fast the monster rises from the water
    private float riseSpeed = 1.2f;
    //How large the monsters sight bubble should be, used for when turning around to check if the monster is there
    private float lookBackSize = 7f;
    //~~~~~~~~~~~~~~~~~~~~~~~~Things you can't~~~~~~~~~~~~~~~~~~~~~~~~
    //Scale based on distance from player
    private float distanceScale;

    void Start()
    {
        lookBackBubble = this.transform.GetChild(0).gameObject;
        monsterFind = monsterSpawner.GetComponent<MonsterFind>();
        //Stops the monster from walking around
        agent.enabled = false;
        //Starts in a lowered state
        this.transform.Translate(new Vector3(0, -riseHeight, 0));
    }

    void Update()
    {
        //Changes the size of the sight disc the player can look at when looking behind them
        distanceScale = Vector3.Distance(camera.transform.position, this.transform.position) - monsterFind.minSpawnDistance;
        distanceScale = distanceScale / monsterFind.minSpawnDistance;
        this.transform.Find("LookBackRange").localScale = new Vector3(1 + (lookBackSize * distanceScale), 1 + (lookBackSize * distanceScale), 1);
        //Makes the monster walk towards the player, pretty simple stuff (Needs NavMesh Surface)
        if(agent.enabled == true)
        {
            agent.SetDestination(player.transform.position);
        }
    }

    public IEnumerator Rise()
    {
        //Enables sight bubble because they actually use the same tag
        lookBackBubble.SetActive(true);
        //Makes the monster rise from the water
        for(int i = 100; i > 0; i--)
        {
            this.transform.Translate(new Vector3(0, ((riseHeight * i) / 5000), 0));
            yield return new WaitForSeconds(riseSpeed/100);
        }
        //Lets the monster walk around
        agent.enabled = true;
    }

    public IEnumerator Sink()
    {
        //Stops the monster from walking around
        agent.enabled = false;
        //Makes the monster sink into the water
        for(int i = 0; i < 100; i++)
        {
            this.transform.Translate(new Vector3(0, -((riseHeight * i) / 5000), 0));
            yield return new WaitForSeconds(riseSpeed/100);
        }
        //Disables sight bubble because they actually use the same tag
        lookBackBubble.SetActive(false);
        //Starts the rise of one of the looking monsters
        StartCoroutine(monsterFind.Rise()); 
    }
}
