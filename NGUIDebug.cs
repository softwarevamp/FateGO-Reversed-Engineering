using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Debug")]
public class NGUIDebug : MonoBehaviour
{
    private static NGUIDebug mInstance = null;
    private static List<string> mLines = new List<string>();
    private static bool mRayDebug = false;

    public static void Clear()
    {
        mLines.Clear();
    }

    public static void CreateInstance()
    {
        if (mInstance == null)
        {
            GameObject target = new GameObject("_NGUI Debug");
            mInstance = target.AddComponent<NGUIDebug>();
            UnityEngine.Object.DontDestroyOnLoad(target);
        }
    }

    public static void DrawBounds(Bounds b)
    {
        Vector3 center = b.center;
        Vector3 vector2 = b.center - b.extents;
        Vector3 vector3 = b.center + b.extents;
        Debug.DrawLine(new Vector3(vector2.x, vector2.y, center.z), new Vector3(vector3.x, vector2.y, center.z), Color.red);
        Debug.DrawLine(new Vector3(vector2.x, vector2.y, center.z), new Vector3(vector2.x, vector3.y, center.z), Color.red);
        Debug.DrawLine(new Vector3(vector3.x, vector2.y, center.z), new Vector3(vector3.x, vector3.y, center.z), Color.red);
        Debug.DrawLine(new Vector3(vector2.x, vector3.y, center.z), new Vector3(vector3.x, vector3.y, center.z), Color.red);
    }

    public static void Log(params object[] objs)
    {
        string text = string.Empty;
        for (int i = 0; i < objs.Length; i++)
        {
            if (i == 0)
            {
                text = text + objs[i].ToString();
            }
            else
            {
                text = text + ", " + objs[i].ToString();
            }
        }
        LogString(text);
    }

    private static void LogString(string text)
    {
        if (Application.isPlaying)
        {
            if (mLines.Count > 20)
            {
                mLines.RemoveAt(0);
            }
            mLines.Add(text);
            CreateInstance();
        }
        else
        {
            Debug.Log(text);
        }
    }

    private void OnGUI()
    {
        if (mLines.Count == 0)
        {
            if ((mRayDebug && (UICamera.hoveredObject != null)) && Application.isPlaying)
            {
                GUILayout.Label("Last Hit: " + NGUITools.GetHierarchy(UICamera.hoveredObject).Replace("\"", string.Empty), new GUILayoutOption[0]);
            }
        }
        else
        {
            int num = 0;
            int count = mLines.Count;
            while (num < count)
            {
                GUILayout.Label(mLines[num], new GUILayoutOption[0]);
                num++;
            }
        }
    }

    public static bool debugRaycast
    {
        get => 
            mRayDebug;
        set
        {
            if (Application.isPlaying)
            {
                mRayDebug = value;
                if (value)
                {
                    CreateInstance();
                }
            }
        }
    }
}

