using System;
using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;
    public Action<Vector3> VolumeChanged;

    private const string MasterVolumeKey = "MasterVolume";
    private const string MusicVolumeKey = "MusicVolume";
    private const string SfxVolumeKey = "SfxVolume";

    [SerializeField] private bool valueUpdated = false;

    public float MasterVolume
    {
        get => volume.x;
        set
        {
            volume.x = Mathf.Clamp(value, 0f, 1f);
            valueUpdated = true;
            SaveVolumeSettings();
        }
    }

    public float MusicVolume
    {
        get => volume.y;
        set
        {
            volume.y = Mathf.Clamp(value, 0f, 1f);
            valueUpdated = true;
            SaveVolumeSettings();
        }
    }

    public float SfxVolume
    {
        get => volume.z;
        set
        {
            volume.z = Mathf.Clamp(value, 0f, 1f);
            valueUpdated = true;
            SaveVolumeSettings();
        }
    }

    public Vector3 Volume => volume;

    /*
     * X = master volume
     * Y = music volume
     * Z = sound effect volume
     *
     * y * x or z * x to handle volume
     */

    [SerializeField] private Vector3 volume = new Vector3(1f, 1f, 1f);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);

            LoadVolumeSettings();
        }
        else
        {
            if (DebugHandler.IsDebugEnabled)
                Debug.LogWarning(
                    "There was more than one instance of GlobalAudioManager, destroying the most recent one."
                );

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (valueUpdated)
        {
            VolumeChanged?.Invoke(volume);
            valueUpdated = false;
        }
    }

    private void SaveVolumeSettings()
    {
        PlayerPrefs.SetFloat(MasterVolumeKey, volume.x);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume.y);
        PlayerPrefs.SetFloat(SfxVolumeKey, volume.z);

        PlayerPrefs.Save();
    }

    private void LoadVolumeSettings()
    {
        volume.x = PlayerPrefs.GetFloat(MasterVolumeKey, volume.x);
        volume.y = PlayerPrefs.GetFloat(MusicVolumeKey, volume.y);
        volume.z = PlayerPrefs.GetFloat(SfxVolumeKey, volume.z);

        volume.x = Mathf.Clamp01(volume.x);
        volume.y = Mathf.Clamp01(volume.y);
        volume.z = Mathf.Clamp01(volume.z);

        valueUpdated = true;
    }
}
