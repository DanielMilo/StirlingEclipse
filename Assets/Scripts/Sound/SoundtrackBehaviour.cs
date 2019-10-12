using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundtrackBehaviour : MonoBehaviour
{
    [SerializeField] AudioMixerGroup mainMixer;
    [SerializeField] AudioClip[] clips;
    AudioClip chosenClip;
    SoundtrackExceptionContainer sceneClip;

    AudioSource source;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.outputAudioMixerGroup = mainMixer.audioMixer.FindMatchingGroups("Music")[0];

        // pick a random clip
        SelectNewTrack();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        if (timer > source.clip.length)
        {
            SelectNewTrack();
            timer = 0;       
        }


        //  if (source.time >= source.clip.length || !source.isPlaying)
       // {
       //     SelectNewTrack();
       // }
    }

    public void OnSceneLoad()
    {
        sceneClip = GameObject.FindObjectOfType<SoundtrackExceptionContainer>();
        if(sceneClip != null && source.clip != sceneClip.clip)
        {
            source.clip = sceneClip.clip;
            source.Play();
        }
    }

    void SelectNewTrack()
    {
        if(sceneClip != null && sceneClip.clip != null)
        {
            source.clip = sceneClip.clip;
            source.Play();
        }
        else if(clips.Length > 0)
        {
            chosenClip = clips[Random.Range(0, clips.Length)];
            source.clip = chosenClip;
            source.Play();
        }
    }
}
