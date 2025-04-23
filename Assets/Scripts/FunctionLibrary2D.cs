using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary2D {
    public delegate float Function(float x, float t);

    public enum FunctionName { 
        HorizontalLine,
        Line,
        Parabola,
        Cubic,
        Wave,
        MutliWave,
        Ripple,
    }

    static readonly Function[] functions = {
        HorizontalLine,
        Line,
        Parabola,
        Cubic,
        Wave,
        MutliWave,
        Ripple,
    };

    public static int FunctionCount => functions.Length;

    public static Function GetFunction(FunctionName name) {
        return functions[(int)name];
    }

    public static FunctionName GetNextFunctionName(FunctionName name)
    {
        return (FunctionName)(((int)name + 1) % functions.Length);
    }

    public static FunctionName GetRandomFunctionNameOtherThan(FunctionName name)
    {
        var choice = (FunctionName)Random.Range(1, functions.Length);
        return choice == name ? 0 : choice;
    }

    public static float HorizontalLine(float x, float t)
    {
        return 0;
    }
    public static float Line(float x, float t)
    {
        return x;
    }

    public static float Parabola(float x, float t)
    {
        return x * x;
    }

    public static float Cubic(float x, float t)
    {
        return x * x * x;
    }

    public static float Wave(float x, float t) {
         return Sin(PI * (x + t));
    }

    public static float MutliWave(float x, float t) { 
        float y = Sin(PI * (x + 0.5f * t));
        y += Sin(2f * PI * (y + t)) * 0.5f;
        y *= 2f / 3f;

        return y;
    }

    public static float Ripple(float x, float t) { 
        float d = Abs(x);
        float y = Sin(PI * (4f * d - t));
        y /= 1f + 10f * d;

        return y;
    }

}