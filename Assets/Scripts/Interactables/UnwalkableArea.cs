using UnityEngine;
using UnityEngine.Events;

public class UnwalkableArea : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

    public UnityEvent playDeathSFX;
    public UnityEvent playPsionFormDestructionSFX;
    public UnityEvent playInteractiveObjectDestructionSFX;
    
    private void Awake()
    {
        sceneLoader = FindFirstObjectByType<SceneLoader>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PsionForm"))
        {
            if (other.gameObject.TryGetComponent(out PsionFormManager manager))
            {
                playPsionFormDestructionSFX?.Invoke();
                manager.player1.gameObject.SetActive(false);
                manager.player2.gameObject.SetActive(false);
            }
            else
            {
                if (other.gameObject.TryGetComponent<PlayerController>(out var playerController))
                {
                    playDeathSFX?.Invoke();
                }
            }

            other.gameObject.SetActive(false);

            if (sceneLoader != null)
            {
                sceneLoader.ReloadSceneWithDelay(2);
            }
        }
        else
        {
            playInteractiveObjectDestructionSFX?.Invoke();
            Destroy(other.gameObject);
        }
    }
}