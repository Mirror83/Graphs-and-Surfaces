using static UnityEngine.Mathf;

public static class FunctionLibrary {
    public delegate float Function(float x, float z, float t);
    
    public enum FunctionName { Wave, MutliWave, Ripple }

    static Function[] functions = {
        Wave,
        MutliWave,
        Ripple
    };

    public static Function GetFunction(FunctionName name) {
        return functions[(int)name];
    }

    public static float Wave(float x, float z, float t) {
        return Sin(PI * (x + t));
    }

    public static float MutliWave(float x, float z, float t) {
        float y = Sin(PI * (x + 0.5f * t));
        y += Sin(2f * PI * (x + t)) * 0.5f;
        y *= 2f / 3f;
        return y;
    }

    public static float Ripple(float x, float z, float t) {
        float d= Abs(x);
        float y = Sin(PI * (4f * d - t));
        return y / (1f + 10f * d);
    }
}