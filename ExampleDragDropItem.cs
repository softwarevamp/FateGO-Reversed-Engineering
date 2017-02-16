﻿using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Drag and Drop Item (Example)")]
public class ExampleDragDropItem : UIDragDropItem
{
    public GameObject prefab;

    protected override void OnDragDropRelease(GameObject surface)
    {
        if (surface != null)
        {
            ExampleDragDropSurface component = surface.GetComponent<ExampleDragDropSurface>();
            if (component != null)
            {
                GameObject obj2 = NGUITools.AddChild(component.gameObject, this.prefab);
                obj2.transform.localScale = component.transform.localScale;
                Transform transform = obj2.transform;
                transform.position = UICamera.lastWorldPosition;
                if (component.rotatePlacedObject)
                {
                    transform.rotation = Quaternion.LookRotation(UICamera.lastHit.normal) * Quaternion.Euler(90f, 0f, 0f);
                }
                NGUITools.Destroy(base.gameObject);
                return;
            }
        }
        base.OnDragDropRelease(surface);
    }
}

