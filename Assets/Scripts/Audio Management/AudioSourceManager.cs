using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioSourceManager : MonoBehaviour
{
    public GlobalAudioManager globalAudioManager;
    public AudioSource audioSource;
    public AudioSourceType audioSourceType;
    public bool playOnInitialise;
    IEnumerator WaitForGam()
    {
        while (!GlobalAudioManager.Instance)
        {
            yield return null;
        }
        globalAudioManager = GlobalAudioManager.Instance;
        globalAudioManager.VolumeChanged += VolumeChanged;
        VolumeChanged(globalAudioManager.Volume);
        if(playOnInitialise) audioSource.Play();
        if(DebugHandler.IsDebugEnabled) Debug.LogWarning("GlobalAudioManager has been found, Actions hooked up.");
    }

    private void VolumeChanged(Vector3 vol)
    {
        if (!audioSource)
        {
            if(DebugHandler.IsDebugEnabled) Debug.LogWarning("Audio Source is not set: this is not a critical problem, but could be indicative of missing objects.");
            return;
        }
        switch (audioSourceType)
        {
            case AudioSourceType.Music:
                audioSource.volume = vol.y * vol.x;
                break;
            case AudioSourceType.Sfx:
                audioSource.volume = vol.z * vol.x;
                break;
            default:
                if(DebugHandler.IsDebugEnabled) Debug.LogWarning("Missing Audio Source Type?");
                break;
        }
    }

    private void Awake()
    {
        StartCoroutine(WaitForGam());
        TryGetComponent<AudioSource>(out audioSource);
    }

    private void OnDisable()
    {
        globalAudioManager.VolumeChanged -= VolumeChanged;
    }

    private void OnDestroy()
    {
        globalAudioManager.VolumeChanged -= VolumeChanged;
    }
}