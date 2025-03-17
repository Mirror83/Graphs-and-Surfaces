using static UnityEngine.Mathf;

public static class FunctionLibrary {
    public delegate float Function(float x, float t);
    
    public enum FunctionName { Wave }

    static Function[] functions = {
        Wave,
    };

    public static Function GetFunction(FunctionName name) {
        return functions[(int)name];
    }

    public static float Wave(float x, float t) {
        return Sin(PI * (x + t));
    }
}