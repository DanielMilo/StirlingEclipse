using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundEffects : MonoBehaviour
{
    [SerializeField] AudioClip pickupClip;
    [SerializeField] AudioClip criticalClip;
    [SerializeField] AudioClip chargingClip;
    [SerializeField] AudioClip engineClip;
    [SerializeField] float engineMinPitch;
    [SerializeField] float engineMaxPitch;
    [SerializeField] float enginePitchChangeSpeed;

    Craft player;
    GameController controller;
    AudioSource playerAudioSource;
    AudioSource engineSource;

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
            engineSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float targetPitch = engineMinPitch + (engineMaxPitch - engineMinPitch) * player.engine.enginePowerPercentage;
        float enginePitch = Mathf.Lerp(engineSource.pitch, targetPitch, enginePitchChangeSpeed*Time.deltaTime);
        engineSource.pitch = enginePitch;
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

    public void OnCritical()
    {
        if(criticalClip != null)
        {
            playerAudioSource.clip = criticalClip;
            playerAudioSource.loop = true;
            playerAudioSource.pitch = 1f;
            playerAudioSource.Play();
        }
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
