using System;
using UnityEngine;
using static FunctionLibrary;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10, 200)]
    int resolution = 10;

    [SerializeField]
    FunctionName functionName = FunctionName.Wave;

    public enum TransitionMode { Cycle, Random }

    [SerializeField]
    TransitionMode transitionMode;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    bool transitioning;
    FunctionName transitionFunctionName;

    float duration = 0f;

    Transform[] points;

    Vector3 F(float u, float v, float time) {
        var function = GetFunction(functionName);
        return function(u, v, time);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var step = 2f / resolution;
        var scale = Vector3.one * step;

        points = new Transform[resolution * resolution];

        for (int i = 0; i < points.Length; i++) {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    void Update()
    {
        duration += Time.deltaTime;
        if (transitioning)
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else if (duration >= functionDuration)
        {
            duration -= functionDuration;

            transitioning = true;
            transitionFunctionName = functionName;
            PickNextFunction();
        }

        if (transitioning)
        {
            UpdateFunctionTransition();
        }
        else
        {
            UpdateCurrentFunction();
        }
    }

    void PickNextFunction()
    {
        functionName = transitionMode == TransitionMode.Cycle ?
            GetNextFunctionName(functionName) :
            GetRandomFunctionNameOtherThan(functionName);
    }

    void UpdateCurrentFunction()
    {
        float time = Time.time;
        float step = 2f / resolution;

        float v = 0.5f * step - 1f;

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }

            float u = (x + 0.5f) * step - 1f;

            points[i].localPosition = F(u, v, time);
        }
    }

    void UpdateFunctionTransition()
    {
        Function from = GetFunction(transitionFunctionName), to = GetFunction(functionName);

        float progress = duration / transitionDuration;

        float time = Time.time;
        float step = 2f / resolution;

        float v = 0.5f * step - 1f;

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }

            float u = (x + 0.5f) * step - 1f;

            points[i].localPosition = Morph(u, v, time, from, to, progress);
        }
    }
}
