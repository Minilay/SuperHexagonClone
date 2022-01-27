using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class ListExtension
{
    public static bool isEmpty<T>(this List<T> lst) => 
        !lst?.Any() ?? true;
    
    public static List<int> Shuffle(this List<int> lst) =>
    new List<int>(lst.OrderBy(_ => Random.Range(0, lst.Count)));


}
