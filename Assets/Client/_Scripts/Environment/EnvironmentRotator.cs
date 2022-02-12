using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentRotator : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    

    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
    }
}
