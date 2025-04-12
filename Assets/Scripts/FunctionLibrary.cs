using UnityEngine;
using static UnityEngine.Mathf;

public static class FunctionLibrary {
    public delegate Vector3 Function(float u, float v, float t);

    public enum FunctionName { 
        Wave,
        MutliWave,
        Ripple,
        Sphere,
        RotatingStarTorus,
        //Torus,
    }

    static readonly Function[] functions = {
        Wave,
        MutliWave,
        Ripple,
        Sphere,
        RotatingStarTorus,
        //Torus,
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

    public static Vector3 Morph(
        float u, float v, float t, Function from, Function to, float progress  
    )
    {
        return Vector3.LerpUnclamped(from(u, v, t), to(u, v, t), SmoothStep(0f, 1f, progress));
    }

    public static Vector3 Wave(float u, float v, float t) {
        Vector3 position;
        position.x = u;
        position.z = v;
        position.y = Sin(PI * (u + v + t));
        return position;
    }

    public static Vector3 MutliWave(float u, float v, float t) {
        Vector3 position;
        position.x = u;
        position.z = v;

        float y = Sin(PI * (u + 0.5f * t));
        y += Sin(2f * PI * (v + t)) * 0.5f;
        y += Sin(PI * (u + v + 0.25f + t));
        y *= 2f / 3f;
        position.y = y;

        return position;
    }

    public static Vector3 Ripple(float u, float v, float t) {
        Vector3 position;
        position.x = u;
        position.z = v;

        float d = Sqrt(u * u + v * v);
        float y = Sin(PI * (4f * d - t));
        y /= 1f + 10f * d;
        position.y = y;

        return position;
    }

    public static Vector3 Sphere(float u, float v, float t) {
        Vector3 p;
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    public static Vector3 RotatingStarTorus(float u, float v, float t) {
        Vector3 p;
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t)); // Major radius
		float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t)); // Minor radius

        float s = r1 + r2 * Cos(PI * v);
        p.x = s * Sin(PI * u);
        p.y = r2 * Sin(PI * v);
        p.z = s * Cos(PI * u);

        return p;
    }

    //public static Vector3 Torus(float u, float v, float t) {
    //    Vector3 p;
    //    float r1 = 0.75f; // Major radius
    //    float r2 = 0.25f; // Minor radius

    //    float s = r1 + r2 * Cos(PI * v);
    //    p.x = s * Sin(PI * u);
    //    p.y = r2 * Sin(PI * v);
    //    p.z = s * Cos(PI * u);

    //    return p;
    //}

}