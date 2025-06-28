using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class RangeCircle : MonoBehaviour
{
    public float radius = 2f;
    public int segments = 60;

    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.loop = true;
        line.widthMultiplier = 0.1f;
        DrawCircle();
    }

    void DrawCircle()
    {
        Vector3[] points = new Vector3[segments];
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            points[i] = new Vector3(x, 0.01f, z); // cao hơn mặt đất 1 chút
        }

        line.positionCount = segments;
        line.SetPositions(points);
    }
}
