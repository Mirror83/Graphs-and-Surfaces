using System;
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
        float time = Time.time;
        float step = 2f / resolution;

        float v = 0.5f * step - 1f;

        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution){
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }

            float u = (x + 0.5f) * step - 1f;
            
            points[i].localPosition = F(u, v, time);
        }
    }
}
