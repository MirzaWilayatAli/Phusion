using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;
    public Action<Vector3> VolumeChanged;

    // tbh there's no reason to do it the getter and setter way but I think it feels a little neater in my head. idk tbh lol
    [SerializeField] private bool valueUpdated = false;
    
    public float MasterVolume
    {
        get => volume.x;
        set
        {
            volume.x = Mathf.Clamp(value, 0f, 1f);
            valueUpdated = true;
        }
    }
    
    public float MusicVolume
    {
        get => volume.y;
        set
        {
            volume.y = Mathf.Clamp(value, 0f, 1f);
            valueUpdated = true;
        }
    }
    
    public float SfxVolume
    {
        get => volume.z;
        set
        {
            volume.z = Mathf.Clamp(value, 0f, 1f);
            valueUpdated = true;
        }
    }

    public Vector3 Volume => volume;
     /*
     * X = master volume
     * Y = music volume
     * Z = sound effect volume
     *
     * y * x or z * x to handle vol
     */
    [SerializeField] private Vector3 volume = new Vector3(1f, 1f, 1f); 
    private void Awake()
    {
        if(Instance == null) Instance = this;
        else
        {
            if(DebugHandler.IsDebugEnabled) Debug.LogWarning("There was more than one instance of GlobalAudioManager, destroying the most recent one.");
            Destroy(this);
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
}
