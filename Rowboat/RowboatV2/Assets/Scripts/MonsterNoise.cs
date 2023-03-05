using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterNoise : MonoBehaviour
{
    //~~~~~~~~~~~~~~~~~~~~~~~~Objects~~~~~~~~~~~~~~~~~~~~~~~~
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    private AudioClip soundClip;
    [SerializeField]
    private GameObject monsterSpawner;
    MonsterFind monsterFind;
    [SerializeField]
    private GameObject deathBubble;
    Death death;
    //~~~~~~~~~~~~~~~~~~~~~~~~Things you can edit~~~~~~~~~~~~~~~~~~~~~~~~
    //Max time the player can go without hearing a sound
    private float minTime = 10f;
    //Max time the player can go without hearing a sound
    private float maxTime = 20f;
    //How long the player has to react to the sound cue
    private float soundReaction = 3.5f;
    //~~~~~~~~~~~~~~~~~~~~~~~~Things you can't~~~~~~~~~~~~~~~~~~~~~~~~
    //Upon leaving the spawn turns on, allows the minigame thing to actually start
    public bool activated = false;
    //If the player is in a state where they need to hide
    public bool needToHide = false;
    //The current timer, goes down, when it hits zero the sound goes off
    [SerializeField]
    private float currTimer;
    //The amount of time the player has left to react to the sound cue
    [SerializeField]
    private float currReactionTime;
    //PLACEHOLDER FOR ACTUAL HIDING, NEEDS TO BE ADDED
    [SerializeField]
    private bool hiding = false;

    void Start()
    {
        monsterFind = monsterSpawner.GetComponent<MonsterFind>();
        death = deathBubble.GetComponent<Death>();
        StartSoundTimer();
    }

    void Update()
    {
        //PLACEHOLDING FOR HIDING
        if (Input.GetKey(KeyCode.H))
        {
            hiding = true;
        }else{
            hiding = false;
        }

        //Countdown of the timer to start the sound cue, does not go down if the player is searching for the monster
        if(activated == true && monsterFind.searching == false && needToHide == false)
        {
            currTimer = currTimer - Time.deltaTime;
            if(currTimer < 0) 
            {
                SoundCueGame();
            }
        }

        if(needToHide == true)
        {
            currReactionTime = currReactionTime - Time.deltaTime;
            if(hiding == true) 
            {
                //timer goes 2x faster when the player is hiding so they don't just sit there like an idiot
                currReactionTime = currReactionTime - Time.deltaTime;
            }
            //When the timer runs out
            if(currReactionTime < 0) 
            {
                if(hiding == true) 
                {
                    //Probably play a small sound cue here to tell the player they are good
                    needToHide = false;
                    StartSoundTimer();
                }else{
                    //DIE
                    Debug.Log("Killed by noise");
                    death.KillPlayer();
                }
            }
        }
    }

    //Plays the sound and sets variables that makes it so the player needs to hide
    void SoundCueGame()
    {
        audioSource.PlayOneShot(soundClip);
        needToHide = true;
        currReactionTime = soundReaction;
    }

    //Starts the timer for the next sound cue
    void StartSoundTimer()
    {
        currTimer = Random.Range(minTime, maxTime);
    }
}
