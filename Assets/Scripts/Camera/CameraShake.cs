using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float defaultMagnitude = 0.05f;
    
    private Coroutine shakeCoroutine;

    private void OnEnable()
    {
        shakeCoroutine = StartCoroutine(ShakeCoroutine());
    }

    private void OnDisable()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
        }
    }
    

    private IEnumerator ShakeCoroutine()
    {
        while (true) // This will run forever until disabled
        {
            Vector3 basePosition = transform.localPosition;

            float offsetX = Random.Range(-1f, 1f) * defaultMagnitude;
            float offsetY = Random.Range(-1f, 1f) * defaultMagnitude;

            transform.localPosition = basePosition + new Vector3(offsetX, offsetY, 0);

            yield return null;
        }
    }
    
    public void SetMagnitude(float magnitude)
    {
        defaultMagnitude = magnitude;
    }
}