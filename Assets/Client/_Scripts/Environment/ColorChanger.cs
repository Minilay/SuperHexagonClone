using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ColorChanger : MonoBehaviour
{
    [SerializeField] private float _colorChangeSpeed;

    [SerializeField] private List<Material> _materials;
    [SerializeField] private List<float> _brightness;
    
    private float _colorHue;

    private void Awake()
    {
        _colorHue = Random.Range(0, 360);
    }

    private void FixedUpdate()
    {
        _colorHue = (_colorHue + _colorChangeSpeed * Time.deltaTime) % 360;

        for (var i = 0; i < _materials.Count; i++)
            _materials[i].color = ColorExtension.CreateColorByHSV((int)_colorHue, 100, (int)_brightness[i]);

    }
    
}
