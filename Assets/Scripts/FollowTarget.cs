using System;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Transform origin;
    private Vector3 _velocity;
    public float smoothTime = 0.25f;
    public float maxSpeed = 100;
    private void Update()
    {
        if (target)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.transform.position, ref _velocity, smoothTime, maxSpeed, Time.deltaTime);
        }
        else
        {
            if (Vector3.Distance(transform.position, origin.position) <= 0.1f)
            {
                transform.position = origin.position;
            }
            else
            {
                transform.position = Vector3.SmoothDamp(transform.position, origin.position, ref _velocity, smoothTime, maxSpeed, Time.deltaTime);
            }
        }
    }
}
