using System;
using Unity.VisualScripting;
using UnityEngine;
using static FunctionLibrary;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(10, 100)]
    int resolution = 10;

    [SerializeField]
   FunctionName functionName;

    Transform[] points;

    float Y(float x) {
        var function = GetFunction(functionName);
        return function(x);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var position = Vector3.zero;
        var step = 2f / resolution;
        var scale = Vector3.one * step;

        points = new Transform[resolution];

        for (int i = 0; i < points.Length; i++) {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            position.x = step * (i + 0.5f) - 1f;
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
            position.y = Y(position.x);
            point.localPosition = position;
        }
    }
}
