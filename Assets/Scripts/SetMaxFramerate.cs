using System;
using UnityEngine;

public class SetMaxFramerate : MonoBehaviour
{
    // without this, our game will have no max framerate, and therefore just waste CPU/GPU trying to render as much as it possibly can.
    public static SetMaxFramerate Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        } else Destroy(this);
        Application.targetFrameRate = 120; // 60 * 2 for smoothness
    }
}
