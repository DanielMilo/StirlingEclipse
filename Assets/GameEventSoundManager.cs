using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSoundManager : MonoBehaviour
{

    [SerializeField] AudioClip victoryClip;
    [SerializeField] AudioClip deathClip;

    GameController controller;
    AudioSource gameEventSource;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
        gameEventSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.gameState == GameState.victory)
        {
            if(victoryClip != null && gameEventSource.clip != victoryClip)
            {
                gameEventSource.clip = victoryClip;
                gameEventSource.Play();
            }

        }
        else if(controller.gameState == GameState.death)
        {
            if(deathClip != null && gameEventSource.clip != deathClip)
            {
                gameEventSource.clip = deathClip;
                gameEventSource.Play();
            }
        }
    }
}
