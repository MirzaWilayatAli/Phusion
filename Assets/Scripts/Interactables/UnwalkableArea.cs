using System;
using UnityEngine;

public class UnwalkableArea : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;

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
                manager.player1.mainController.playDeathSFX?.Invoke();
                manager.player1.gameObject.SetActive(false);
                manager.player2.mainController.playDeathSFX?.Invoke();
                manager.player2.gameObject.SetActive(false);
            }
            else
            {
                if (other.gameObject.TryGetComponent<PlayerController>(out var playerController))
                {
                    playerController.playDeathSFX?.Invoke();
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
            Destroy(other.gameObject);
        }
    }
}