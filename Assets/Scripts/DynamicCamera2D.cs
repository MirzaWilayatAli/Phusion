using UnityEngine;

public class DynamicCamera2D : MonoBehaviour
{
    [Header("Targets")]
    public Transform player1;
    public Transform player2;

    [Header("Follow Settings")]
    public float followSpeed = 5f;
    public Vector3 offset = new Vector3(0, 0, -10);

    [Header("Zoom Settings (Orthographic)")]
    public float minZoom = 5f;
    public float maxZoom = 12f;
    public float maxDistance = 15f;
    public float minDistanceBuffer = 2f;
    
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true;
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null) return;
        
        Vector3 midpoint = (player1.position + player2.position) / 2f;
        
        float distance = Vector2.Distance(player1.position, player2.position);
        distance = Mathf.Max(distance, minDistanceBuffer);
        
        float zoomFactor = Mathf.InverseLerp(0, maxDistance, distance);
        float targetZoom = Mathf.Lerp(minZoom, maxZoom, zoomFactor);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, followSpeed * Time.deltaTime);

        Vector3 desiredPosition = midpoint + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
    }
    
    public Vector2 GetCameraBounds()
    {
        float height = cam.orthographicSize;
        float width = height * cam.aspect;
        return new Vector2(width, height);
    }
}