using System;
using UnityEngine;

public class FOVSync : MonoBehaviour
{
    public Camera observeCamera;

    private void LateUpdate()
    {
        Camera component = base.GetComponent<Camera>();
        if (((this.observeCamera != null) && (component != null)) && (this.observeCamera.fieldOfView != component.fieldOfView))
        {
            component.fieldOfView = this.observeCamera.fieldOfView;
        }
    }

    private void Start()
    {
    }
}

