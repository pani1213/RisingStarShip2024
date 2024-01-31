using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BezierTest : MonoBehaviour
{
    public List<Vector3> controlPoints; // 베지어 곡선을 정의하는 제어점 리스트
    public int curveResolution = 20; // 곡선의 해상도
    [Range(0, 1)]
    public float fill;
    public GameObject obj;

    void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.white;
        for (int i = 0; i < curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            Vector3 point = CalculateBezierPoint(t, controlPoints);
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
    private void Update()
    {
        obj.transform.position =  CalculateBezierPoint(fill, controlPoints);
    }

    Vector3 CalculateBezierPoint(float _value, List<Vector3> points)
    {
        if (points.Count == 1)
            return points[0];

        List<Vector3> reducedPoints = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            reducedPoints.Add(Vector3.Lerp(points[i], points[i + 1], _value));
        }
        return CalculateBezierPoint(_value, reducedPoints);
    }
}
