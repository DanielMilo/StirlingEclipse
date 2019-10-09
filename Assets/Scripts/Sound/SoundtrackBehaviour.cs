using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundtrackBehaviour : MonoBehaviour
{
    [SerializeField] AudioMixer mainMixer;
    [SerializeField] AudioClip[] clips;
    AudioClip chosenClip;

    AudioSource source;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

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

    void SelectNewTrack()
    {
        if(clips.Length > 0)
        {
            chosenClip = clips[Random.Range(0, clips.Length)];
            source.clip = chosenClip;
            source.Play();
        }
    }
}
