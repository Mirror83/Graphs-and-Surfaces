using static UnityEngine.Mathf;

public static class FunctionLibrary {
    public delegate float Function(float x);
    
    public enum FunctionName { Parabola, Wave, CubicCurve }

    static Function[] functions = {
        Parabola,
        Wave,
        CubicCurve
    };

    public static Function GetFunction(FunctionName name) {
        return functions[(int)name];
    }

    public static float Parabola(float x) {
        return x * x;
    }

    public static float CubicCurve(float x) {
        return x * x * x;
    }

    public static float Wave(float x) {
        return Sin(PI * x);
    }
}