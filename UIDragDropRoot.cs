using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag and Drop Root")]
public class UIDragDropRoot : MonoBehaviour
{
    public static Transform root;

    private void OnDisable()
    {
        if (root == base.transform)
        {
            root = null;
        }
    }

    private void OnEnable()
    {
        root = base.transform;
    }
}

