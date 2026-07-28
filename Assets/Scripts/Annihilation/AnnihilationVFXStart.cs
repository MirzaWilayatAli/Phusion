using UnityEngine;
// Handle VFX/SFX manually, instead of via GameObject on/off so AudioSourceManager can run
public class AnnihilationVFXStart : MonoBehaviour
{
    [SerializeField] private GameObject annihilationVfx;
    [SerializeField] private AudioSource annihilationSfx;

    public void StartFX()
    {
        if(!annihilationSfx.isPlaying) annihilationSfx.Play();
        annihilationVfx.SetActive(true);
    }
}
