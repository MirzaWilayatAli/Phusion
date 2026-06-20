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
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.SetActive(false);
            if (sceneLoader != null) sceneLoader.ReloadScene();
        }
    }
}