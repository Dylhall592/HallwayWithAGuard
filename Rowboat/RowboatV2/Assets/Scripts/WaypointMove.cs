using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMove : MonoBehaviour
{
    [SerializeField]
    private GameObject monsterSpawner;
    MonsterFind monsterFind;
    [SerializeField]
    public GameObject lookBackRangeBubble;

    public List<GameObject> waypoints;
    public Transform cam;
    public float speed = 4;
    int index = 0;

    void Start()
    {
        monsterFind = monsterSpawner.GetComponent<MonsterFind>();
    }

    void Update()
    {
        if(monsterFind.searching == false){
            if (Input.GetKey(KeyCode.W))
            {
                //Turns off the monsters lookback bubble incase it turns causing it to slap into the player
                lookBackRangeBubble.SetActive(false);
                //moves the player from waypoint to waypoint down the path
                Vector3 destination = waypoints[index].transform.position;
                Vector3 newPos = Vector3.MoveTowards(transform.position, waypoints[index].transform.position, speed * Time.deltaTime);
                transform.position = newPos;

                //matches the players rotation to the next waypoints rotation, this means we will have to rotate every waypoint in the direction we want to go
                Quaternion currentRotation = transform.rotation;
                Quaternion nextRotation = waypoints[index].transform.rotation;
                transform.rotation = Quaternion.Slerp(currentRotation, nextRotation, 2 * Time.deltaTime);
        
                Quaternion currentcamRotation = cam.transform.rotation;
                Quaternion nextcamRotation = waypoints[index].transform.rotation;
                cam.transform.rotation = Quaternion.Slerp(currentcamRotation, nextcamRotation, 2 * Time.deltaTime);

                float distance = Vector3.Distance(transform.position, destination);
                //makes the index counter update so we aren't stuck on the first waypoint
                if(distance <= 0.03)
                {
                    if(index < waypoints.Count-1)
                    {
                        index++;
                    }
                }
            }else{
                //turns back on the lookback bubble when the player is not moving
                lookBackRangeBubble.SetActive(true);
            }
        }
    }
}
