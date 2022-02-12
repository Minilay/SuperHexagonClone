using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ColorExtension
{
    private static void Validation(ref int h,ref int s,ref int v)
    {
        while (h < 0)
            h += 360;
        h %= 360;

        while (s < 0)
            s += 100;
        s %= 100;

        while (v < 0)
            v += 100;

        v %= 100;
    }
    public static Color CreateColorByHSV(int h, int s, int v)
    {
        // Validation(ref h, ref s, ref v);
        
        var rgb = new int[3];

        var baseColor = (h + 60) % 360 / 120;
        var shift = (h + 60) % 360 - (120 * baseColor + 60 );
        var secondaryColor = (baseColor + (shift >= 0 ? 1 : -1) + 3) % 3;
        
        //Setting Hue
        rgb[baseColor] = 255;
        rgb[secondaryColor] = (int) ((Mathf.Abs(shift) / 60.0f) * 255.0f);
        
        //Setting Saturation
        for (var i = 0; i < 3; i++)
            rgb[i] += (int) ((255 - rgb[i]) * ((100 - s) / 100.0f));
        
        //Setting Value
        for (var i = 0; i < 3; i++)
            rgb[i] -= (int) (rgb[i] * (100-v) / 100.0f);

        var color = new Color {r = rgb[0] / 255.0f, g = rgb[1] / 255.0f, b = rgb[2] / 255.0f};
        return color;
    }
}
