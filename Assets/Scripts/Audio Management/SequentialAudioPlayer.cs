using UnityEngine;
using System.Collections;
using TMPro;

public class SequentialAudioPlayer : MonoBehaviour
{
    [Header("Playlist")]
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private TextMeshProUGUI nowPlayingText;

    private AudioSource audioSource;
    private Coroutine playlistRoutine;

    private int playlistIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        // This will force show the first track immediately
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

            // this should reset the index count so that this remains valid
            if (playlistIndex >= audioClips.Length)
                playlistIndex = 0;

            // Start the next track if nothing is playing
            if (!audioSource.isPlaying)
            {
                PlayClip(audioClips[playlistIndex]);
            }

            // Wait until the current track finishes
            if (audioSource.clip != null && audioSource.time >= audioSource.clip.length)
            {
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
        
        if (nowPlayingText != null)
        {
            nowPlayingText.text = clip.name;
        }

        audioSource.Play();
    }
}