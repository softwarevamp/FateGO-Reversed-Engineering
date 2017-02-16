using System;
using UnityEngine;

public static class Debug
{
    public static void Break()
    {
        if (IsEnable())
        {
            UnityEngine.Debug.Break();
        }
    }

    public static void DrawLine(Vector3 vec1, Vector3 vec2, Color col)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.DrawLine(vec1, vec2, col);
        }
    }

    public static void DrawRay(Vector3 vec1, Vector3 vec2, Color col)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.DrawRay(vec1, vec2, col);
        }
    }

    private static bool IsEnable() => 
        false;

    public static void Log(object message)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.Log(message);
        }
    }

    public static void Log(object message, UnityEngine.Object context)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.Log(message, context);
        }
    }

    public static void LogError(object message)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.LogError(message);
        }
    }

    public static void LogError(object message, UnityEngine.Object context)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.LogError(message, context);
        }
    }

    public static void LogWarning(object message)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.LogWarning(message);
        }
    }

    public static void LogWarning(object message, UnityEngine.Object context)
    {
        if (IsEnable())
        {
            UnityEngine.Debug.LogWarning(message, context);
        }
    }
}

