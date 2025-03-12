using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Transform point = Instantiate(pointPrefab);
        point.localPosition = Vector3.right;
    }
}
