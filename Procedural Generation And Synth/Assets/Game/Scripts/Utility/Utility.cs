using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    private static float _keyDifference = 0f;

    public static float KeyDifferece { get { if (_keyDifference == 0f) { _keyDifference = Mathf.Pow(2f, 1f / 12f); }  return _keyDifference; } }

    public static float NthRoot(float A, float N)
    {
        float epsilon = Mathf.Epsilon;
        float n = N;
        float x = A / n;

        while (Mathf.Abs(A - Mathf.Pow(x, n)) > epsilon)
        {
            x = (1f / n) * ((n - 1) * x + (A / Mathf.Pow(x, n - 1)));
        }

        return x;
    }
}
