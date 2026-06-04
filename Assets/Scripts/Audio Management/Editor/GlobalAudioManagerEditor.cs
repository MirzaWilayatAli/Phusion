using System;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GlobalAudioManager))]
public class GlobalAudioManagerEditor : Editor
{
    GlobalAudioManager _self;
    private void OnEnable()
    {
        _self = (GlobalAudioManager)target;
    }

    public override void OnInspectorGUI() {
        if (!Application.isPlaying)
        {
            EditorGUILayout.LabelField("[[ THIS DOES NOT WORK OUTSIDE OF PLAY MODE ]]");
        }
        else
        {
            EditorGUILayout.LabelField("Game Volume");
            _self.MasterVolume = EditorGUILayout.Slider("Master", _self.MasterVolume, 0f, 1f);
            _self.MusicVolume = EditorGUILayout.Slider("Music", _self.MusicVolume, 0f, 1f);
            _self.SfxVolume = EditorGUILayout.Slider("SFX", _self.SfxVolume, 0f, 1f);
        }
    }
}
