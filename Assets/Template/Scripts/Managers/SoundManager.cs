using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages sounds that are meant to be played once
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource speaker;

    public static SoundManager Instance;

    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
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
