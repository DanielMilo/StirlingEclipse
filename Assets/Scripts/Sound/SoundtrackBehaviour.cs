using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundtrackBehaviour : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] AudioClip[] clips;

    SoundtrackExceptionContainer sceneClip;
    AudioClip chosenClip;

    AudioSource source;

    float isNotPlayingTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        // pick a random clip
        SelectNewTrack();
    }

    // Update is called once per frame
    void Update()
    {
        if(!source.isPlaying)
        {
            isNotPlayingTimer += Time.deltaTime;
        }

        if(source.time >= source.clip.length || isNotPlayingTimer >= 5)
        {
            SelectNewTrack();
        }
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
