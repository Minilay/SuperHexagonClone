using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public static class ListExtension
{
    public static bool isEmpty<T>(this List<T> lst) => !lst?.Any() ?? true;
    
    public static List<int> Shuffle(this List<int> lst)
    {
        Random random = new Random();
        return new List<int>(lst.OrderBy(_ => random.Next()));
    }


}
