using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource speaker;
    [SerializeField] AudioClip[] musicClips;

    private int prevMusic;

    public static MusicManager instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
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
                prevMusic = Random.Range(0, musicClips.Length);
                NextMusic();
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
        if(iMusic >= prevMusic)
        {
            iMusic++;
        }
        speaker.clip = musicClips[iMusic]; ;
        Invoke(nameof(NextMusic), speaker.clip.length);
        prevMusic = iMusic;
    }

    public void SetVolume(int volume)
    {
        speaker.volume = volume;
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
