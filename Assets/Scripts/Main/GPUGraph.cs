﻿using System;
using UnityEngine;
using static FunctionLibrary;

public class GPUGraph : MonoBehaviour
{
    const int maxResolution = 1000;

    [SerializeField, Range(10, maxResolution)]
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

    ComputeBuffer positionsBuffer;

    [SerializeField]
    ComputeShader computeShader;

    [SerializeField]
    Material material;

    [SerializeField]
    Mesh mesh;

    static readonly int
        positionsId = Shader.PropertyToID("_Positions"),
        resolutionId = Shader.PropertyToID("_Resolution"),
        timeId = Shader.PropertyToID("_Time"),
        stepId = Shader.PropertyToID("_Step"),
        transitionDurationId = Shader.PropertyToID("_TransitionProgress");

    void UpdateFunctionOnGPU()
    {
        int kernelIndex = 
            (int)functionName + (int)(transitioning ? transitionFunctionName : functionName) * FunctionCount;
        float step = 2f / resolution;

        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(timeId, Time.time);
        computeShader.SetFloat(stepId, step);
        computeShader.SetBuffer(kernelIndex, positionsId, positionsBuffer);

        if (transitioning)
        {
            computeShader.SetFloat(
                transitionDurationId, 
                Mathf.SmoothStep(0f, 1f, duration / transitionDuration));
        }

        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(kernelIndex, groups, groups, 1);

        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);

        var rparams = new RenderParams { 
            material = material,
            worldBounds = new Bounds(Vector3.zero, Vector3.one * (2f + step))
        };

        Graphics.RenderMeshPrimitives(rparams, mesh, 0, resolution * resolution);
    }


    void Start()
    {
        positionsBuffer = new ComputeBuffer(maxResolution * maxResolution, 3 * 4);
    }

    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
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
        UpdateFunctionOnGPU();
    }

    void PickNextFunction()
    {
        functionName = transitionMode == TransitionMode.Cycle ?
            GetNextFunctionName(functionName) :
            GetRandomFunctionNameOtherThan(functionName);
    }
}
