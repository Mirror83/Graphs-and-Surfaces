using static UnityEngine.Mathf;

public static class FunctionLibrary {
    public delegate float Function(float x);
    
    public enum FunctionName { Parabola, Wave }

    public static float Parabola(float x) {
        return x * x;
    }

    public static float Wave(float x) {
        return Sin(PI * x);
    }
}