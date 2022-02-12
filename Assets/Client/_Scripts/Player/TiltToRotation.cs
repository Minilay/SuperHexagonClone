using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltToRotation : MonoBehaviour
{
    [SerializeField] private float _rotation;

    private Vector3 defaultAngles;
    
    private void Awake()
    {
        defaultAngles = transform.localEulerAngles;
    }

    private void FixedUpdate()
    {
        var angle = _rotation * Input.GetAxis("Horizontal");
        transform.localEulerAngles = new Vector3(
            defaultAngles.x,
            defaultAngles.y + angle,
            defaultAngles.z);
    }
}
