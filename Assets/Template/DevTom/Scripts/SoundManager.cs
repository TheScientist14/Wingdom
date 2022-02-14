using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages sounds that are meant to be played once
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource speaker;

    public static SoundManager instance;

    void Awake()
    {
        if(instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        if (!speaker)
        {
            Debug.LogWarning("No audio source set to SoundManager");
        }
    }

    public void PlayClip(AudioClip audioClip)
    {
        if (speaker)
        {
            speaker.Stop();
            speaker.clip = audioClip;
            speaker.Play();
        }
    }

    public void SetVolume(float volume)
    {
        if (speaker)
        {
            speaker.volume = volume;
        }
    }
}
