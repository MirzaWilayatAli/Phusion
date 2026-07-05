using System;
using UnityEngine;
// unused
public class MiddlePoint : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    public float val = 2f;
    public float smoothing = 0.25f;
    public Vector3 velocity = Vector3.zero;
    public void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, (object1.position + object2.position) / val, ref velocity, smoothing);
    }
}
