/**
 * @author SerapH
 */

using System;
using UnityEngine;

public struct MathUtility
{
    public static readonly float sqrt2 = Mathf.Sqrt(2);
    public static readonly Vector3[] orientations = new Vector3[360];

    public static void Initialize()
    {
        for (int a = 0; a < 360; a++)
            orientations[a] = Quaternion.Euler(0, 0, a) * Vector3.right;
    }

    public static int ManhattanDistance(int xA, int yA, int xB, int yB)
    {
        return Math.Abs(xA - xB) + Math.Abs(yA - yB);
    }

    public static float ManhattanDistance(float xA, float yA, float xB, float yB)
    {
        return Mathf.Abs(xA - xB) + Mathf.Abs(yA - yB);
    }

    public static float EuclideanDistance(int xA, int yA, int xB, int yB)
    {
        int dx = xA - xB;
        int dy = yA - yB;

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static float EuclideanDistance(float xA, float yA, float xB, float yB)
    {
        float dx = xA - xB;
        float dy = yA - yB;

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    public static int ChebyshevDistance(int xA, int yA, int xB, int yB)
    {
        return Math.Max(Math.Abs(xA - xB), Math.Abs(yA - yB));
    }

    public static int ManhattanDistance(int xA, int yA, int zA, int xB, int yB, int zB)
    {
        return Math.Abs(xA - xB) + Math.Abs(yA - yB) + Math.Abs(zA - zB);
    }

    public static float EuclideanDistance(int xA, int yA, int zA, int xB, int yB, int zB)
    {
        int dx = xA - xB;
        int dy = yA - yB;
        int dz = zA - zB;

        return Mathf.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public static int ChebyshevDistance(int xA, int yA, int zA, int xB, int yB, int zB)
    {
        return Math.Max(Math.Abs(xA - xB), Math.Max(Math.Abs(yA - yB), Math.Abs(zA - zB)));
    }

    public static float CubicCurve1(float x)
    {
        return (Mathf.Pow(3 * x - 1, 3) + 1) / 9;
    }

    public static float CubicCurve2(float x)
    {
        return (Mathf.Pow(4 * x - 2, 3) + 8) / 16;
    }

    public static Vector3 GetOrientation(int angle)
    {
        return orientations[angle % 360];
    }
}
