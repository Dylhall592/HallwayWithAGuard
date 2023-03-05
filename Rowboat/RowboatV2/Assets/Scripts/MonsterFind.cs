using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFind : MonoBehaviour
{
    //~~~~~~~~~~~~~~~~~~~~~~~~Objects~~~~~~~~~~~~~~~~~~~~~~~~
    [SerializeField]
    public GameObject monsterWatcher;
    [SerializeField]
    public GameObject monsterFollower;
    MonsterFollow monsterFollow;
    [SerializeField]
    public GameObject player;
    [SerializeField]
    public GameObject water;
    [SerializeField]
    public new GameObject camera;
    WhatAmILookingAt lookingAt;
    MonsterNoise monsterNoise;
    [SerializeField]
    private GameObject deathBubble;
    Death death;
    //~~~~~~~~~~~~~~~~~~~~~~~~Things you can edit~~~~~~~~~~~~~~~~~~~~~~~~
    //Max time the player can go without hearing a sound
    private float minTime = 25f;
    //Max time the player can go without hearing a sound
    private float maxTime = 40f;
    //The amount of time the player has to see the monster to start a find without killing them, is a part of the timer
    private float lookBackTime = 10f;
    //How high the monster rises from the water
    private float riseHeight = 4f;
    //How fast the monster rises from the water
    private float riseSpeed = 1.2f;
    //How long the player has to find the creature once a search is started
    private float searchTime = 6f;
    //How long the player has to look at the creature for a search to end
    private float lookTime = 2.15f;
    //The max distance the monster can spawn away
    public float maxSpawnDistance = 30f;
    //The min distance the monster can spawn away
    public float minSpawnDistance = 5f;
    //Scale size, increase to make the monster easier to look at
    //If minSpawnDistance changes this will effect how it scales and will probably need adjusted, renderer for VisionRange can be turned on
    public float scaleSize = 1.2f;
    //~~~~~~~~~~~~~~~~~~~~~~~~Things you can't~~~~~~~~~~~~~~~~~~~~~~~~
    //The current timer, goes down, when it hits zero the sound goes off
    [SerializeField]
    private float currTimer;
    //The current instance of spawned monster
    private GameObject currMonster;
    //If the player needs to look for the creature
    public bool searching = false;
    //How long the player has left to find the monster
    [SerializeField]
    private float searchTimeLeft;
    //How long the player has looked at the monster
    private float stareTime = 0f;
    //Pauses the timer because the player is looking at the creature
    private bool starePause = false;
    //The spawn location of the creature
    private Vector3 spawnLocation = Vector3.zero;
    //The angle it tries to spawn the monster at
    private float spawnAngle;
    //Checks that the raycast don't hit things that are bad
    private bool goodAngle;
    //Scale based on distance from player
    private float distanceScale;
    //Helps stop crashes, in theory could destroy the monster if the player hits very small odds
    private float preventCrash;

    void Start()
    {
        lookingAt = camera.GetComponent<WhatAmILookingAt>();
        monsterFollow = monsterFollower.GetComponent<MonsterFollow>();
        monsterNoise = camera.GetComponent<MonsterNoise>();
        currTimer = Random.Range(minTime, maxTime);
        death = deathBubble.GetComponent<Death>();
    }

    void Update()
    {
        //~~~~~~~~~~~~~~~~~~~~~~~~The Search Timer~~~~~~~~~~~~~~~~~~~~~~~~
        //Countdown of the timer to start the sound cue, does not go down if the player is searching for the monster
        if(monsterNoise.activated == true && searching == false && monsterNoise.needToHide == false)
        {
            currTimer = currTimer - Time.deltaTime;
            if(currTimer < 0) 
            {
                Debug.Log("Killed by lack of find");
                death.KillPlayer();
            }
            if(currTimer < lookBackTime)
            {
                if(lookingAt.LookingAtMonster() == true)
                {
                    StartCoroutine(monsterFollow.Sink());
                    //Gives em a lil extra time because this runs until the monster fully rises again
                    currTimer = Random.Range(minTime + 3f, maxTime + 3f);
                }
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~The Search~~~~~~~~~~~~~~~~~~~~~~~~
        if(searching == true)
        {
            if(lookingAt.LookingAtMonster() == true)
            {
                //Increase the amount of time you have looked at the creature
                stareTime = stareTime + Time.deltaTime;
                //Pauses timers
                starePause = true;
                //Make the monster leave if you have looked at them long enough
                if (stareTime >= lookTime)
                {
                    Debug.Log("Sinking monster");
                    searching = false;
                    StartCoroutine(Sink(currMonster));
                }
            }else{
                starePause = false;
            }
        }
        //~~~~~~~~~~~~~~~~~~~~~~~~The Timer~~~~~~~~~~~~~~~~~~~~~~~~
        if(searching == true && starePause == false)
        {
            searchTimeLeft = searchTimeLeft - Time.deltaTime;
            if(searchTimeLeft < 0) 
            {
                Debug.Log("Killed by find");
                death.KillPlayer();
            }
        }
    }

    public IEnumerator Rise()
    {
        //Resets stare number
        stareTime = 0;
        preventCrash = 0;
        //~~~~~~~~~~~~~~~~~~~~~~~~Spawning Monster~~~~~~~~~~~~~~~~~~~~~~~~
        GameObject tempObj;
        tempObj = Instantiate(monsterWatcher) as GameObject;
        tempObj.transform.position = (CreateSpawnLocation());

        tempObj.transform.Translate(new Vector3(0, -(riseHeight/2), 0));
        //Changes the size of the vision range depending on how far the monster is so the bubble is small when it is up close and large when far away
        distanceScale = Vector3.Distance(camera.transform.position, spawnLocation) - minSpawnDistance;
        distanceScale = distanceScale / minSpawnDistance;
        tempObj.transform.Find("VisionRange").localScale = new Vector3(1 + (scaleSize * distanceScale), 1 + (scaleSize * distanceScale), 1 + (scaleSize * distanceScale));

        searchTimeLeft = searchTime;
        currMonster = tempObj;
        searching = true;

        //Makes the monster rise from the water
        for(int i = 100; i > 0; i--)
        {
            tempObj.transform.Translate(new Vector3(0, ((riseHeight * i) / 5000), 0));
            yield return new WaitForSeconds(riseSpeed/100);
        }
    }
    
    public IEnumerator Sink(GameObject tempObj)
    {
        //Makes the monster sink into the water
        for(int i = 0; i < 100; i++)
        {
            tempObj.transform.Translate(new Vector3(0, -((riseHeight * i) / 5000), 0));
            yield return new WaitForSeconds(riseSpeed/100);
        }
        Destroy(tempObj);
        StartCoroutine(monsterFollow.Rise());
    }
    
    private Vector3 CreateSpawnLocation()
    {
        //Looks for a spawn position until it finds one, this is a lil dangerous btw cause if this breaks it will repeat forever
        //While(true) is an atrocity btw, any coder would probably kill me for that
        while(true){
            goodAngle = true;
            //~~~~~~~~~~~~~~~~~~~~~~~~Random Location~~~~~~~~~~~~~~~~~~~~~~~~
            //Creates a random area on a 1 radius circle
            spawnAngle = Random.Range(0, 360);
            spawnLocation =  (new Vector3(Mathf.Sin(Mathf.PI * 2 * spawnAngle / 360), camera.transform.position.y, Mathf.Cos(Mathf.PI * 2 * spawnAngle / 360)));
            //Makes the location within a random area of the spawning donut
            spawnLocation.x = spawnLocation.x * Random.Range(minSpawnDistance, maxSpawnDistance);
            spawnLocation.z = spawnLocation.z * Random.Range(minSpawnDistance, maxSpawnDistance);
            //Moves to the location of the player/camera
            spawnLocation =  (new Vector3(spawnLocation.x + camera.transform.position.x, spawnLocation.y, spawnLocation.z + camera.transform.position.z));
            //~~~~~~~~~~~~~~~~~~~~~~~~Checking the Player's Sightline~~~~~~~~~~~~~~~~~~~~~~~~
            //Raycast that checks if the player can see the monster
            float distance = Vector3.Distance(camera.transform.position, spawnLocation);
            RaycastHit[] hits;
            //The +1 stops monster from spawning inside of a wall lookin all weird
            hits = Physics.RaycastAll(camera.transform.position, (spawnLocation - camera.transform.position), distance + 1);
            Debug.DrawLine(camera.transform.position, spawnLocation, Color.yellow, 10);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                //Any bad tags that should block the player's line of sight
                if(hit.collider.tag == "VisionObstruction")
                {
                    goodAngle = false;
                }
            }
            if(goodAngle == true)
            {
                Debug.DrawLine(camera.transform.position, spawnLocation, Color.red, 10);
                spawnLocation.y = spawnLocation.y - 2;
                return spawnLocation;
                break;
            }
            //~~~~~~~~~~~~~~~~~~~~~~~~Crash preventative so ya'll don't accidentally destroy the game and forget to save~~~~~~~~~~~~~~~~~~~~~~~~
            //My repentance for while(true)
            preventCrash = preventCrash + 1;
            if(preventCrash > 150)
            {
                Debug.Log("You broke the game wtf!!!!!!!!!!!!");
                Debug.Log("The monster could not spawn right for some reason, tell Griffin");
                preventCrash = 0;
                return spawnLocation;
                break;
            }
        }
    }
}
