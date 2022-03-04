using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource speaker;
    [SerializeField] AudioClip[] musicClips;

    private int _prevMusic;

    public static MusicManager Instance;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        if (speaker)
        {
            if (musicClips.Length >= 2)
            {
                _prevMusic = Random.Range(0, musicClips.Length);
                NextMusic();
                Debug.Log("Starting music");
            }
            else if(musicClips.Length == 1)
            {
                speaker.loop = true;
                speaker.clip = musicClips[0];
                speaker.Play();
            }
            else
            {
                Debug.LogWarning("No music set to MusicManager");
            }
        }
        else
        {
            Debug.LogWarning("No audio source set to MusicManager");
        }
    }

    void NextMusic()
    {
        int iMusic = Random.Range(0, musicClips.Length - 1);
        if(iMusic >= _prevMusic)
        {
            iMusic++;
        }
        Debug.Log(iMusic);
        speaker.clip = musicClips[iMusic]; ;
        Invoke(nameof(NextMusic), speaker.clip.length);
        _prevMusic = iMusic;
    }

    public void SetVolume(int volume)
    {
        speaker.outputAudioMixerGroup.audioMixer.SetFloat("Volume", volume);
    }

    public void SetPause(bool pause)
    {
        if (pause)
        {
            speaker.Pause();
            CancelInvoke(nameof(NextMusic));
        }
        else
        {
            speaker.Play();
            Invoke(nameof(NextMusic), speaker.clip.length - speaker.time);
        }
    }
}
