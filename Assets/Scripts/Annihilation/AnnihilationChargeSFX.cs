using System;
using UnityEngine;
// Handle SFX manually, instead of via GameObject on/off so AudioSourceManager can run
public class AnnihilationChargeSfx : MonoBehaviour
{
    [SerializeField] private GameObject annihilationCanvas;
    [SerializeField] private AudioSource annihilationSfx;

    private void Awake()
    {
        if(!annihilationSfx) annihilationSfx = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(annihilationCanvas.activeSelf && !annihilationSfx.isPlaying) annihilationSfx.Play(); else if(!annihilationCanvas.activeSelf) annihilationSfx.Stop();
    }
}
