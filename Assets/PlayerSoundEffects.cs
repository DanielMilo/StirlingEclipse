using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] AudioClip pickupClip;
    [SerializeField] AudioClip criticalClip;
    [SerializeField] AudioClip chargingClip;

    Craft player;
    GameController controller;
    AudioSource playerAudioSource;

    float defaultPitch;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Craft>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerAudioSource = GetComponent<AudioSource>();
        defaultPitch = playerAudioSource.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPickup()
    {
        if(pickupClip != null)
        {
            playerAudioSource.clip = pickupClip;
            playerAudioSource.loop = false;
            playerAudioSource.pitch = defaultPitch;
            playerAudioSource.Play();
        }
    }

    public void OnCritical()
    {
        if(criticalClip != null)
        {
            playerAudioSource.clip = criticalClip;
            playerAudioSource.loop = true;
            playerAudioSource.pitch = defaultPitch;
            playerAudioSource.Play();
        }
    }

    public void OnCharging(float chargingPercentage)
    {
        if(chargingClip != null)
        {
            playerAudioSource.clip = chargingClip;
            playerAudioSource.loop = true;
            playerAudioSource.pitch = defaultPitch; // modify pitch here according to charging rate
            playerAudioSource.Play();
        }
    }
}
