using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class SequentialAudioPlayer : MonoBehaviour
{
    [Header("Playlist")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private TextMeshProUGUI nowPlayingText;

    [Header("Fade Settings")]
    [SerializeField] private float fadeInTime = 1f;
    [SerializeField] private float fadeOutTime = 1f;
    [SerializeField] private float maxVolume = 1f;

    private AudioSource audioSource;
    private Coroutine playlistRoutine;
    private Coroutine fadeRoutine;

    private int playlistIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // Show the first track immediately.
        if (audioClips != null && audioClips.Length > 0 && nowPlayingText != null)
        {
            nowPlayingText.text = audioClips[0].name;
        }

        playlistRoutine = StartCoroutine(PlaylistRoutine());
    }

    private IEnumerator PlaylistRoutine()
    {
        while (true)
        {
            if (audioClips == null || audioClips.Length == 0)
            {
                yield return null;
                continue;
            }

            // Make sure the index is valid.
            if (playlistIndex >= audioClips.Length)
                playlistIndex = 0;

            // Start the next track if nothing is playing.
            if (!audioSource.isPlaying)
            {
                PlayClip(audioClips[playlistIndex]);
            }

            // Fade out near the end of the track.
            float remaining = audioSource.clip.length - audioSource.time;

            if (remaining <= fadeOutTime)
            {
                yield return Fade(0f);

                audioSource.Stop();
                playlistIndex++;
            }

            yield return null;
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip == null)
            return;

        audioSource.clip = clip;
        audioSource.time = 0f;
        audioSource.volume = 0f;

        // Update the text
        if (nowPlayingText != null)
        {
            nowPlayingText.text = clip.name;
        }

        audioSource.Play();

        // Fade in the new track
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(Fade(maxVolume));
    }

    private IEnumerator Fade(float targetVolume)
    {
        float start = audioSource.volume;
        float duration = targetVolume > start ? fadeInTime : fadeOutTime;

        if (duration <= 0f)
        {
            audioSource.volume = targetVolume;
            fadeRoutine = null;
            yield break;
        }

        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;

            audioSource.volume = Mathf.Lerp(start, targetVolume, t / duration);

            yield return null;
        }

        audioSource.volume = targetVolume;
        fadeRoutine = null;
    }
}