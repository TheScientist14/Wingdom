using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages sounds that are meant to be played once
public class UISoundManager : MonoBehaviour
{
    [SerializeField] AudioSource speaker;

    public static UISoundManager Instance;

    void Awake()
    {
        if (Instance)
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
            Debug.LogWarning("No audio source set to "+name);
        }
    }

    /**
     * Plays an audio clip
     * if a sound is already playing, stops it
     */
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
            speaker.outputAudioMixerGroup.audioMixer.SetFloat("Volume", volume);
        }
    }
}
