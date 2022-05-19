using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicPlayer;
    [SerializeField] AudioSource sfxPlayer;

    [SerializeField] AudioClip titleMusic;
    [SerializeField] AudioClip mainMusic;
    [SerializeField] AudioClip popSfx;
    [SerializeField] AudioClip typingSfx;

    public static AudioManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayTitleMusic()
    {
        PlayMusic(titleMusic, 1f, 0f, 1f);
    }

    public void PlayMainMusic()
    {
        PlayMusic(mainMusic, 2f, 0f, 1f);
    }

    public void PlayPopSfx()
    {
        PlaySfx(popSfx, 0.5f);
    }

    public void PlayTypingSfx()
    {
        PlaySfx(typingSfx, 0.3f, true);
    }

    public void StopTypingSfx()
    {
        StopSfx(sfxPlayer);
    }

    public void PlaySfx(AudioClip clip, float volume, bool loop = false)
    {
        if (clip == null)
        {
            return;
        }

        sfxPlayer.clip = clip;
        sfxPlayer.loop = loop;
        sfxPlayer.volume = volume;
        sfxPlayer.Play();
    }

    public void StopSfx(AudioSource source)
    {
        if (source == null)
        {
            return;
        }

        source.Stop();
    }

    public void PlayMusic(AudioClip clip, float duration, float startVolume, float targetVolume, bool loop = true)
    {
        if (clip == null)
        {
            return;
        }

        //musicPlayer.pitch = 1.102f; somehow this isnt an issue anymore???
         
        musicPlayer.clip = clip;
        musicPlayer.loop = loop;

        StartCoroutine(StartFade(musicPlayer, duration, startVolume, targetVolume));
        musicPlayer.Play();
    }

    public void FadeMusic(float duration, float targetVolume)
    {
        StartCoroutine(StartFade(musicPlayer, duration, musicPlayer.volume, targetVolume));
    }

    private static IEnumerator StartFade(AudioSource audioSource, float duration, float startVolume, float targetVolume)
    {
        float currentTime = 0;
        float start = startVolume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

}
