using System;
using UnityEngine;
using static FunctionLibrary2D;

public class Graph2D : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10, 200)]
    int resolution = 10;

    [SerializeField]
    FunctionName functionName = FunctionName.Wave;

    public enum TransitionMode { Cycle, Random, NoTransition }

    [SerializeField]
    TransitionMode transitionMode = TransitionMode.NoTransition;

    [SerializeField, Min(0f)]
    float functionDuration = 1f, transitionDuration = 1f;

    bool transitioning;
    FunctionName transitionFunctionName;

    float duration = 0f;

    Transform[] points;

    Vector3 F(float u, float time) {
        var function = GetFunction(functionName);
        return function(u, time);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       

        points = new Transform[resolution];

        var step = 2f / resolution;
        var scale = Vector3.one * step;

        Vector3 position = Vector3.zero;

        for (int i = 0; i < points.Length; i++) {
            Transform point = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            point.localPosition = position;

            point.localScale = scale;

            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    void Update()
    {
        if (transitionMode != TransitionMode.NoTransition)
        {
            UpdateWithTransition();
        } else
        {
            UpdateCurrentFunction();
        }
    }

    void UpdateWithTransition()
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
        if (transitionMode != TransitionMode.NoTransition)
        {
            functionName = transitionMode == TransitionMode.Cycle ?
                GetNextFunctionName(functionName) :
                GetRandomFunctionNameOtherThan(functionName);
        }
    }

    void UpdateCurrentFunction()
    {
        float time = Time.time;
        var step = 2f / resolution;

        for (int i = 0; i < points.Length; i++)
        {
            var u = (i + 0.5f) * step - 1f;
            points[i].localPosition = F(u, time);
        }
    }

    void UpdateFunctionTransition()
    {
        Function from = GetFunction(transitionFunctionName), to = GetFunction(functionName);

        float progress = duration / transitionDuration;

        float time = Time.time;
        float step = 2f / resolution;

        float v = 0.5f * step - 1f;

        for (int i = 0; i < points.Length; i++)
        {
            float u = (i + 0.5f) * step - 1f;
            points[i].localPosition = Morph(u, time, from, to, progress);
        }
    }
}
