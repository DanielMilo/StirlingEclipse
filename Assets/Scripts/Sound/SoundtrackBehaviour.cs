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

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        // pick a random clip
        if(clips.Length > 0)
        {
            chosenClip = clips[Random.Range(0, clips.Length)];
            source.clip = chosenClip;
            source.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
            source.mute = !source.mute;
        }
    }
}
