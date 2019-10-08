using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] AudioMixerGroup mainMixer;
    [SerializeField] AudioClip pickupClip;

    [Header("Warning")]
    [SerializeField] AudioClip warningClip;
    [SerializeField] float minWarningVolume;
    [SerializeField] float maxWarningVolume;

    [Header("Engine")]
    [SerializeField] AudioClip engineClip;
    [SerializeField] float engineMinPitch;
    [SerializeField] float engineMaxPitch;
    [SerializeField] float enginePitchChangeSpeed;

    //[SerializeField] AudioClip chargingClip;

    Craft player;
    GameController controller;
    AudioSource playerAudioSource;
    AudioSource engineSource;
    AudioSource warningSource;

    float defaultPitch;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Craft>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        playerAudioSource = GetComponent<AudioSource>();
        Debug.Log(defaultPitch);

        engineSource = player.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        if(engineClip != null)
        {
            engineSource.clip = engineClip;
            engineSource.loop = true;
            engineSource.outputAudioMixerGroup = mainMixer;
            engineSource.Play();
        }

        warningSource = player.gameObject.AddComponent(typeof(AudioSource)) as AudioSource;
        if(warningClip != null)
        {
            warningSource.clip = warningClip;
            warningSource.loop = true;
            warningSource.outputAudioMixerGroup = mainMixer;
            warningSource.volume = 0;
            warningSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        EngineSound();
        WarningSound();
    }

    public void OnPickup()
    {
        if(pickupClip != null)
        {
            playerAudioSource.clip = pickupClip;
            playerAudioSource.loop = false;
            playerAudioSource.pitch = 1f;
            playerAudioSource.Play();
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
            warningSource.volume = 0;
        }
    }

    void EngineSound()
    {
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
