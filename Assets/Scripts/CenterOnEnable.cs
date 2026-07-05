using UnityEngine;

public class CenterOnEnable : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    private void OnEnable()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        if (targetCamera == null)
        {
            Debug.LogWarning("CenterOnEnable: No camera found.");
            return;
        }

        Vector3 center = targetCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Mathf.Abs(transform.position.z - targetCamera.transform.position.z)));

        transform.position = new Vector3(
            center.x,
            center.y,
            transform.position.z
        );
    }
}