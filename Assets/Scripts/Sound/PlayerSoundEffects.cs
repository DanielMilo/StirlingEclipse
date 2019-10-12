using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] AudioMixerGroup mainMixer;

    [Header("Pickup")]
    [SerializeField] float pickupVolume = 1;
    [SerializeField] AudioClip[] pickupHeatClips;
    [SerializeField] AudioClip[] pickupColdClips;

    [Header("Warning")]
    [SerializeField] float minWarningVolume;
    [SerializeField] float maxWarningVolume;
    [SerializeField] AudioClip warningClip;

    [Header("Engine")]
    [SerializeField] float engineVolume = 1;
    [SerializeField] AudioClip engineClip;
    [SerializeField] float engineMinPitch;
    [SerializeField] float engineMaxPitch;
    [SerializeField] float enginePitchChangeSpeed;

    //[SerializeField] AudioClip chargingClip;

    Craft player;
    GameController controller;
    AudioSource pickupAudioSource;
    AudioSource engineSource;
    AudioSource warningSource;

    bool isDeathWarningEffect = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Craft>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        pickupAudioSource = player.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        pickupAudioSource.outputAudioMixerGroup = mainMixer.audioMixer.FindMatchingGroups("Sound Effects")[0];

        engineSource = player.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        if(engineClip != null)
        {
            engineSource.clip = engineClip;
            engineSource.loop = true;
            engineSource.outputAudioMixerGroup = mainMixer.audioMixer.FindMatchingGroups("Sound Effects")[0];
            engineSource.Play();
        }

        warningSource = player.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        if(warningClip != null)
        {
            warningSource.clip = warningClip;
            warningSource.loop = true;
            warningSource.outputAudioMixerGroup = mainMixer.audioMixer.FindMatchingGroups("Sound Effects")[0];
            warningSource.volume = 0;
            warningSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        pickupAudioSource.volume = pickupVolume;

        EngineSound();
        WarningSound();
    }

    public void OnPickup(Resource resource)
    {
        if(pickupHeatClips.Length > 0)
        {
            switch(resource)
            {
                case Resource.heat:
                    pickupAudioSource.clip = pickupHeatClips[Random.Range(0, pickupHeatClips.Length)];
                    break;
                case Resource.cold:
                    pickupAudioSource.clip = pickupColdClips[Random.Range(0, pickupColdClips.Length)];
                    break;
            }
            
            pickupAudioSource.loop = false;
            pickupAudioSource.pitch = 1f;
            pickupAudioSource.Play();
        }
    }

    void WarningSound()
    {
        if(player.heatdeathTimerPercentage > 0f && controller.gameState != GameState.death && controller.gameState != GameState.victory)
        {
            warningSource.loop = true;
            warningSource.pitch = 1f;
            float newVolume = minWarningVolume + (maxWarningVolume - minWarningVolume) * player.heatdeathTimerPercentage;
            warningSource.volume = newVolume;
        }
        else
        {
            if(controller.gameState == GameState.death)
            {
                warningSource.pitch = Mathf.Lerp(warningSource.pitch, 0f, 0.5f * Time.deltaTime);
                warningSource.volume = Mathf.Lerp(warningSource.volume, 0f, 1f * Time.deltaTime);
                
            }
            else
            {
                warningSource.volume = 0;
            }
            
        }
    }

    void EngineSound()
    {
        engineSource.volume = engineVolume;

        float targetPitch = engineMinPitch + (engineMaxPitch - engineMinPitch) * player.engine.enginePowerPercentage;
        float enginePitch = Mathf.Lerp(engineSource.pitch, targetPitch, enginePitchChangeSpeed * Time.deltaTime);
        engineSource.pitch = enginePitch;
    }  

    public void OnCharging(float chargingPercentage)
    {
        /*
        Debug.Log("on charging");
        if(chargingClip != null)
        {
            Debug.Log("charging not null");
            if(playerAudioSource.clip == chargingClip)
            {
                Debug.Log("changing audio clip");
                playerAudioSource.clip = chargingClip;
                playerAudioSource.loop = true;
                playerAudioSource.Play();
            }
            playerAudioSource.pitch = defaultPitch * (0.1f * chargingPercentage); // modify pitch here according to charging rate
        }
        */
    }
}
