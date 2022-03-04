using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages sounds that are meant to be played once
public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource speaker;

    private int _clipPriority = 0;

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

    /**
     * Plays an audio clip
     * if a sound is already playing, compares priority and stops the current sound if the priority is higher or equal
     * if priority is lower than 0, override the current sound anyway
     */
    public void PlayClip(AudioClip audioClip, int priority = -1)
    {
        if (speaker)
        {
            if (speaker.isPlaying && priority >= 0)
            {
                if(_clipPriority <= priority && _clipPriority >= 0)
                {
                    speaker.Stop();
                    speaker.clip = audioClip;
                    speaker.Play();
                    _clipPriority = priority;
                }
            }
            else
            {
                speaker.Stop();
                speaker.clip = audioClip;
                speaker.Play();
                _clipPriority = priority;
            }
        }
    }

    public void StopCurrentClip()
    {
        if (speaker)
        {
            speaker.Stop();
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
