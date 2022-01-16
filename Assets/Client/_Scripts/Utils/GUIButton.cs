using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public static class GUIButton
{
    public static void Button(Action action, string name = "Button")
    {
        if (GUILayout.Button(name))
        {
            action?.Invoke();
        }
    }
}
