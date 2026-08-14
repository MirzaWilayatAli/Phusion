using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadBindsFromPrefs : MonoBehaviour
{
    public InputActionAsset map;
    public string playerPrefKey = "Default";
    private void Awake()
    {
        string savedBindings = PlayerPrefs.GetString($"BindingOverrides_{playerPrefKey}", string.Empty);
        if (!string.IsNullOrEmpty(savedBindings))
            map.LoadBindingOverridesFromJson(savedBindings);
    }
}
