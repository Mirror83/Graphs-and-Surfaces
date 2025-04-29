using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary2D {
    public delegate Vector3 Function(float u, float t);

    public enum FunctionName { 
        HorizontalLine,
        Line,
        Parabola,
        Cubic,
        Circle,
        Wave,
        MutliWave,
        Ripple,
    }

    static readonly Function[] functions = {
        HorizontalLine,
        Line,
        Parabola,
        Cubic,
        Circle,
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

    public static Vector3 HorizontalLine(float u, float t)
    {
        return new Vector3
        {
            x = u,
            y = 0,
            z = 0
        };
    }
    public static Vector3 Line(float u, float t)
    {
        float x, y;
        x = y = u;
        return new Vector3(x, y);
    }

    public static Vector3 Parabola(float u, float t)
    {
        var x = u;
        var y = x * x;
        return new Vector3(x, y);
    }

    public static Vector3 Cubic(float u, float t)
    {
        var x = u;
        var y = x * x * x;
        return new Vector3(x, y);
    }

    public static Vector3 Circle(float u, float t)
    {
        var x = Cos(PI * (u));
        var y = Sin(PI * (u));
        return new Vector3(x, y);
    }

    public static Vector3 Wave(float u, float t) {
        var x = u;
        var y = Sin(PI * (x + t));
       
        return new Vector3(x, y); 
    }

    public static Vector3 MutliWave(float u, float t) {
        float y = Sin(PI * (u + 0.5f * t));
        y += Sin(2f * PI * (y + t)) * 0.5f;
        y *= 2f / 3f;

        return new Vector3
        {
            x = u,
            y = y,
            z = 0
        };
    }

    public static Vector3 Ripple(float u, float t) { 
        float d = Abs(u);
        float y = Sin(PI * (4f * d - t));
        y /= 1f + 10f * d;

        return new Vector3
        {
            x = u,
            y = y,
            z = 0
        };
    }

}