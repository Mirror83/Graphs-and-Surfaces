using System;
using UnityEngine;
using static FunctionLibrary;

public class GPUGraph : MonoBehaviour
{

    [SerializeField, Range(10, 200)]
    int resolution = 10;

    [SerializeField]
    FunctionName functionName = FunctionName.Wave;

    public enum TransitionMode { Cycle, Random }

    [SerializeField]
    TransitionMode transitionMode;

    //[SerializeField, Min(0f)]
    //float functionDuration = 1f, transitionDuration = 1f;

    //bool transitioning;
    //FunctionName transitionFunctionName;

    //float duration = 0f;

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
        stepId = Shader.PropertyToID("_Step");

    void UpdateFunctionOnGPU()
    {
        //int kernelIndex = computeShader.FindKernel("FunctionKernel");
        float step = 2f / resolution;
        computeShader.SetInt(resolutionId, resolution);
        computeShader.SetFloat(timeId, Time.time);
        computeShader.SetFloat(stepId, step);
        computeShader.SetBuffer(0, positionsId, positionsBuffer);

        int groups = Mathf.CeilToInt(resolution / 8f);
        computeShader.Dispatch(0, groups, groups, 1);

        material.SetBuffer(positionsId, positionsBuffer);
        material.SetFloat(stepId, step);

        var rparams = new RenderParams { 
            material = material,
            worldBounds = new Bounds(Vector3.zero, Vector3.one * (2f + step))
        };

        Graphics.RenderMeshPrimitives(rparams, mesh, 0, positionsBuffer.count);
    }


    void Start()
    {
        positionsBuffer = new ComputeBuffer(resolution * resolution, 3 * 4);
    }

    void OnDisable()
    {
        positionsBuffer.Release();
        positionsBuffer = null;
    }

    void Update()
    {
        UpdateFunctionOnGPU();
    }

    Vector3 F(float u, float v, float time)
    {
        var function = GetFunction(functionName);
        return function(u, v, time);
    }

    void PickNextFunction()
    {
        functionName = transitionMode == TransitionMode.Cycle ?
            GetNextFunctionName(functionName) :
            GetRandomFunctionNameOtherThan(functionName);
    }
}
