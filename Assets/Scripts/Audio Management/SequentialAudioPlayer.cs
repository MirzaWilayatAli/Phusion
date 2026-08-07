using UnityEngine;
using System.Collections;
using TMPro;

[RequireComponent(typeof(AudioSource))]
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

    private int playlistIndex;

    private AudioClip overrideClip;
    private bool playOverride;
    private bool stopOverride;

    // Resume data
    private AudioClip savedClip;
    private float savedTime;
    
    private Coroutine fadeRoutine;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playlistRoutine = StartCoroutine(PlaylistRoutine());
    }

    private IEnumerator PlaylistRoutine()
    {
        while (true)
        {
            // ---------- Override music ----------
            if (playOverride)
            {
                playOverride = false;

                // Save current playlist position
                if (audioSource.isPlaying)
                {
                    savedClip = audioSource.clip;
                    savedTime = audioSource.time;

                    if (fadeRoutine != null)
                        StopCoroutine(fadeRoutine);

                    fadeRoutine = StartCoroutine(Fade(0f));
                    yield return fadeRoutine;

                    audioSource.Stop();
                }

                PlayClip(overrideClip);

                // Wait until it finishes or StopCurrentTrack() is called
                while (audioSource.isPlaying && !stopOverride)
                    yield return null;

                stopOverride = false;

                if (fadeRoutine != null)
                    StopCoroutine(fadeRoutine);

                fadeRoutine = StartCoroutine(Fade(0f));
                yield return fadeRoutine;

                audioSource.Stop();

                // Resume playlist
                if (savedClip != null)
                {
                    audioSource.clip = savedClip;
                    audioSource.time = savedTime;

                    if (nowPlayingText != null)
                        nowPlayingText.text = savedClip.name;

                    audioSource.volume = 0f;
                    audioSource.Play();

                    yield return Fade(maxVolume);
                }

                continue;
            }

            // ---------- Playlist ----------
            if (!audioSource.isPlaying)
            {
                if (playlistIndex >= audioClips.Length)
                    playlistIndex = 0;

                PlayClip(audioClips[playlistIndex]);
            }

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
        audioSource.clip = clip;
        audioSource.time = 0f;
        audioSource.volume = 0f;
        audioSource.Play();

        if (nowPlayingText != null)
            nowPlayingText.text = clip.name;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(Fade(maxVolume));
    }

    private IEnumerator Fade(float targetVolume)
    {
        float start = audioSource.volume;
        float duration = targetVolume > start ? fadeInTime : fadeOutTime;

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
    
    // Interrupt the playlist and play this clip.
    // Playlist resumes where it left off afterwards.
    public void PlayTrack(AudioClip clip)
    {
        if (clip == null)
            return;

        overrideClip = clip;
        playOverride = true;
    }
    
    // Stop the currently playing override track.
    public void StopCurrentTrack()
    {
        stopOverride = true;
    }
}