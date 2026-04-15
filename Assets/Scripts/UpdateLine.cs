using System;
using UnityEngine;

public class UpdateLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    public int index = 0;
    private void FixedUpdate()
    {
        lineRenderer.SetPosition(index, transform.position);
    }
}
