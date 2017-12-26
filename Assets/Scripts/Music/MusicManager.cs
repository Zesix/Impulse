using UnityEngine;
using System.Collections;
using Zenject;

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

    public MusicPlaylist Playlist;
    public bool Shuffle;
    public RepeatMode Repeat;
    public float FadeDuration;
    public bool PlayOnAwake;

    [Inject]
    private AudioSource _source;

    private void Start()
    {
        // grab audio source
        _source.playOnAwake = false;
        if (FadeDuration > 0)
            _source.volume = 0f;
        else
            Volume = _volume;
        if (Playlist == null)
            return;
        if (Playlist.MusicList.Length > 0)
            _source.clip = Playlist.MusicList[0];
        else
            Debug.LogError("There are no music in the list");

        if (PlayOnAwake)
            Play();
    }

    public void Play()
    {
        if (Playlist)
        {
            StartCoroutine(PlayMusicList());
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

    public void ChangePlaylist(MusicPlaylist list)
    {
        Playlist = list;
        _counter = 0;
        StopAllCoroutines();
        StartCoroutine(ChangePlaylistE());
    }

    private IEnumerator ChangePlaylistE()
    {
        if (_source.isPlaying)
            yield return StartCoroutine(StopWithFade());
        StartCoroutine(PlayMusicList());
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

    private int _counter;

    private IEnumerator PlayMusicList()
    {
        while (true)
        {
            yield return StartCoroutine(PlaySongE(Playlist.MusicList[_counter]));
            if (Repeat == RepeatMode.Track)
            {

            }
            else if (Shuffle)
            {
                int newTrack = GetNewTrack();
                while (newTrack == _counter && Playlist.MusicList.Length != 1)
                {
                    newTrack = GetNewTrack();
                }
                _counter = newTrack;

            }
            else
            {
                _counter++;
                if (_counter >= Playlist.MusicList.Length - 1)
                {
                    if (Repeat == RepeatMode.Playlist)
                    {
                        _counter = 0;
                    }
                    else
                        yield break;
                }
            }
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

    private int GetNewTrack()
    {
        return Random.Range(0, Playlist.MusicList.Length);
    }

}

public enum RepeatMode
{
    Playlist,
    Track,
    None
}
