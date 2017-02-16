using System;
using UnityEngine;

[AddComponentMenu("NGUI/Internal/Snapshot Point"), ExecuteInEditMode]
public class UISnapshotPoint : MonoBehaviour
{
    public float farClip = 100f;
    [Range(10f, 80f)]
    public int fieldOfView = 0x23;
    public bool isOrthographic = true;
    public float nearClip = -100f;
    public float orthoSize = 30f;
    public Texture2D thumbnail;

    private void Start()
    {
        if (base.tag != "EditorOnly")
        {
            base.tag = "EditorOnly";
        }
    }
}

