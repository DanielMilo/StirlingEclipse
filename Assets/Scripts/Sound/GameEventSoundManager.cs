using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventSoundManager : MonoBehaviour
{

    [SerializeField] AudioClip victoryClip;
    [SerializeField] AudioClip deathClip;

    GameController controller;
    AudioSource gameEventSource;
    AudioSource musicSource;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<GameController>();
        gameEventSource = GetComponent<AudioSource>();
        musicSource = controller.data.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayClips();
        AdjustMusicVolume();
    }

    void PlayClips()
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

    void AdjustMusicVolume()
    {
        if(gameEventSource.isPlaying)
        {
            musicSource.volume = Mathf.Lerp(musicSource.volume, 0.3f, 2f*Time.deltaTime);
        }
        else
        {
            musicSource.volume = Mathf.Lerp(musicSource.volume, 1f, 1f * Time.deltaTime);
        }
    }
}
