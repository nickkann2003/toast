using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField]
    private List<AudioClip> daytimeMusic;

    [SerializeField]
    private AudioSource musicSource;

    [SerializeField]
    private float volume = 1f;

    [SerializeField]
    private float songInterval = 2f;
    float rInterval = 0f;

    [SerializeField]
    private Vector2 songDurationRange;
    private float rSongDuration = 0f;

    private float lerpProgress = 1f;
    private bool fadeOut = false;

    int currentSong = 0;
    int nextSong = 0;

    private void Awake()
    {
        instance = this;   
    }

    private void Start()
    {
        rInterval = songInterval;
        ForceMusicClip(0);
        musicSource.loop = true;
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        float lp = Time.deltaTime / 6f;
        rSongDuration -= dt;
        if(rSongDuration < 0)
        {
            int n = currentSong;
            while ((n = Random.Range(0, daytimeMusic.Count)) == currentSong);
            SetMusicClip(n);
        }
        if (fadeOut)
        {
            lerpProgress -= lp;
            if(lerpProgress <= 0f)
            {
                rInterval -= lp*2;
                if(rInterval <= 0f)
                {
                    lerpProgress = 0f;
                    fadeOut = false;
                    currentSong = nextSong;
                    musicSource.Stop();
                    musicSource.clip = daytimeMusic[currentSong];
                    musicSource.Play();
                    rInterval = songInterval;
                }
            }
        }
        else
        {
            if(lerpProgress < 1f)
            {
                lerpProgress += lp;
                if(lerpProgress > 1f)
                {
                    lerpProgress = 1f;
                    rInterval = songInterval;
                }
            }
        }

        musicSource.volume = Mathf.Lerp(0.0f, volume, lerpProgress);
    }

    public void SetMusicClip(int i)
    {
        nextSong = i;
        fadeOut = true;
        rInterval = songInterval;
        rSongDuration = Random.Range(songDurationRange.x, songDurationRange.y);
    }

    public void ForceMusicClip(int i)
    {
        currentSong = i;
        musicSource.clip = daytimeMusic[i];
        musicSource.Play();
        rSongDuration = Random.Range(songDurationRange.x, songDurationRange.y);
    }
}
