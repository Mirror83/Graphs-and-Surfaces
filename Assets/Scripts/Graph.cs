using System;
using Unity.Collections;
using UnityEngine;
using static FunctionLibrary;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    [SerializeField]
    FunctionName functionName = FunctionName.Wave;

    Transform[] points;

    float Y(float x, float z) {
        float time = Time.time;
        var function = GetFunction(functionName);
        return function(x, z, time);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var position = Vector3.zero;
        var step = 2f / resolution;
        var scale = Vector3.one * step;

        points = new Transform[resolution * resolution];

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
            }
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            position.x = step * (x + 0.5f) - 1f;
            position.z = step * (z + 0.5f) - 1f;
            point.localPosition = position;
            // It is not necessary to keep the point at its original world
            // position, rotation and scale
            point.SetParent(transform, false);
            points[i] = point;
        }
    }

    void Update()
    {
        Vector3 position;
        foreach (Transform point in points)
        {
            position = point.localPosition;
            position.y = Y(position.x, position.z);
            point.localPosition = position;
        }
    }
}
