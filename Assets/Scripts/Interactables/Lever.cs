using System;
using UnityEngine;

public class Lever : TriggerBase
{
    public float onRotation = -50f;
    public float offRotation = 50f;
    public float delta = 2f;
    public bool rot = false;
    [Tooltip("This is so you can set the rotation properly. Do not modify.")]
    public float current;
    public bool Approximation(float x, float y, float d) => Mathf.Abs(x - y) <= d;
    private void Update()
    {
        current = transform.eulerAngles.z;
        
        if (Approximation(transform.rotation.eulerAngles.z, offRotation, delta) && rot == true)
        {
            Debug.Log("off");
            base.onTriggerOff.Invoke();
            rot = false;
        } else if (Approximation(transform.rotation.eulerAngles.z, onRotation, delta) && rot == false)
        {
            Debug.Log("on");
            base.onTriggerOn.Invoke();
            rot = true;
        }
    }
}
