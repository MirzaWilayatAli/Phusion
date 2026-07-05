using UnityEngine;

public class LineAnimation : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    public Material lineMaterial;
    
    private void Awake()
    {
        if (lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer)
        {
            lineMaterial = lineRenderer.material;
        }
    }
}
