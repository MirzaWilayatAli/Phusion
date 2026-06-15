using System;
using UnityEngine;

public class SmoothTransform : MonoBehaviour
{
    public float smoothTime = 0.5f;
    public Vector3 velocity = Vector3.zero;
    public Transform target;
    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
    }
}
