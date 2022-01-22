using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 PolarToCartesian(float angle, float radius = 1) =>
        new Vector3(
            Mathf.Cos(angle * Mathf.Deg2Rad) * radius,
            Mathf.Sin(angle * Mathf.Deg2Rad) * radius
        );

}