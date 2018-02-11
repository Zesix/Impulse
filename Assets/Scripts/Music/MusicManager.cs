using UnityEngine;
using System.Collections;
using Zenject;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : Singleton<MusicManager>
{
    [SerializeField]
    private float _volume;

    public float Volume
    {
        get
        {
            return _source.volume;
        }
        set
        {
            _volume = value;
            _source.volume = value;
        }
    }

    // General playlist variables
    [Header("Playlist Parameters")]
    [Tooltip("List of playlist to execute")]
    public List<MusicPlaylist> Playlists;

    // General playlist execution mode
    public bool ShufflePlaylists;
    public bool RepeatPlaylists;
    
    [Header("General Parameters")]
    public float FadeDuration;
    public bool PlayOnAwake;

    // Counter of current playlist in execution
    private int _playlistCounter;

    // Counter of current song in execution inside a playlist
    private int _songCounter;

    [Inject]
    private AudioSource _source;

    private void Start()
    {
        // Safety check
        if (Playlists.Count <= 0)
            return;

        // Tweak audio source parameters
        _source.playOnAwake = true;

        if (FadeDuration > 0)
            _source.volume = 0f;
        else
            Volume = _volume;

        // Shuffle Playlists
        if (ShufflePlaylists)
        {
            Playlists = Shuffle(Playlists);
        }

        // Start playlist play
        if (PlayOnAwake)
            PlayAllTracks();
    }

    /// <summary>
    /// Use this method to play all possible tracks
    /// </summary>
    public void PlayAllTracks()
    {
        StopAllCoroutines();

        // Execute playlist with first element
        _playlistCounter = 0;
        StartCoroutine(PlayPlaylist(Playlists[_playlistCounter]));
    }


    private IEnumerator PlayPlaylist(MusicPlaylist targetPlaylist)
    {
        // Shuffle target playlist if it is required
        if(targetPlaylist.Shuffle)
        {
            targetPlaylist.MusicList = Shuffle(targetPlaylist.MusicList);
        }

        // Execute target playlist until it finishes
        _songCounter = 0;
        while (_songCounter < targetPlaylist.MusicList.Count)
        {
            // Execute current song
            yield return StartCoroutine(PlaySongE(targetPlaylist.MusicList[_songCounter]));

            // Move to next song
            _songCounter++;
        }

        // Check if there is another playlist to execute, if so do so
        _playlistCounter++;
        if(Playlists.Count < _playlistCounter)
        {
            StartCoroutine(PlayPlaylist(Playlists[_playlistCounter]));
        }
        // Otherwise, and if this system is set to loop, restart the play cycle
        else
        {
            _playlistCounter = 0;
            StartCoroutine(PlayPlaylist(Playlists[_playlistCounter]));
        }
    }


    public void Stop(bool fade)
    {
        StopAllCoroutines();
        if (fade)
            StartCoroutine(StopWithFade());
        else
            _source.Stop();
    }

    public void Next()
    {
        _source.Stop();
    }

    private IEnumerator StopWithFade()
    {
        if (FadeDuration > 0)
        {
            float lerpValue = 0f;
            while (lerpValue < 1f)
            {
                lerpValue += Time.deltaTime / FadeDuration;
                _source.volume = Mathf.Lerp(_volume, 0f, lerpValue);
                yield return null;
            }
        }
        _source.Stop();
    }

    public void PlaySong(AudioClip song)
    {
        StartCoroutine(PlaySongE(song));
    }

    private IEnumerator PlaySongE(AudioClip clip)
    {
        _source.Stop();
        _source.clip = clip;
        _source.Play();
        StartCoroutine(FadeIn());
        while (_source.isPlaying)
        {
            if (_source.clip.length - _source.time <= FadeDuration)
            {
                yield return StartCoroutine(FadeOut());
            }
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        if (FadeDuration > 0f)
        {
            float startTime = _source.clip.length - FadeDuration;
            float lerpValue = 0f;
            while (lerpValue < 1f && _source.isPlaying)
            {
                lerpValue = Mathf.InverseLerp(startTime, _source.clip.length, _source.time);
                _source.volume = Mathf.Lerp(_volume, 0f, lerpValue);
                yield return null;
            }
            _source.volume = 0f;
        }
    }

    private IEnumerator FadeIn()
    {
        if (FadeDuration > 0f)
        {
            var lerpValue = 0f;
            while (lerpValue < 1f && _source.isPlaying)
            {
                lerpValue = Mathf.InverseLerp(0f, FadeDuration, _source.time);
                _source.volume = Mathf.Lerp(0f, _volume, lerpValue);
                yield return null;
            }
            _source.volume = _volume;
        }
    }

    /// <summary>
    /// Use this method to randomize any list
    /// </summary>
    public static List<T> Shuffle<T>(List<T> list)
    {
        return list.OrderBy(x => Random.value).ToList();
    }

}
