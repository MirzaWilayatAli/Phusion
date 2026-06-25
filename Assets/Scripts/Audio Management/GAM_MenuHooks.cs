using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GAM_MenuHooks : MonoBehaviour
{
    public Slider master;
    public Slider music;
    public Slider sfx;
    public GlobalAudioManager audioManager;
    IEnumerator WaitForGam()
    {
        while (!GlobalAudioManager.Instance)
        {
            yield return null;
        }
        audioManager = GlobalAudioManager.Instance;
        master.value = audioManager.MasterVolume;
        music.value = audioManager.MusicVolume;
        sfx.value = audioManager.SfxVolume;
        unlock = true;
    }
    [SerializeField] private bool unlock = false;
    private void Awake()
    {
        StartCoroutine(WaitForGam());
    }

    private void Update()
    {
        if (unlock)
        {
            audioManager.MasterVolume = master.value;
            audioManager.MusicVolume = music.value;
            audioManager.SfxVolume = sfx.value;
        }
    }
}
