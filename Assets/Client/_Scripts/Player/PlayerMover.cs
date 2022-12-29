using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        transform.Rotate(
            new Vector3(0,
                (Input.GetAxisRaw("Horizontal")) * _rotationSpeed * Time.deltaTime,
                0
                ));
    }
}
