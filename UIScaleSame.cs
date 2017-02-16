using System;
using UnityEngine;

public class UIScaleSame : MonoBehaviour
{
    private Camera mCamera;

    public void SetCamera(Camera cam)
    {
        this.mCamera = cam;
    }

    private void Update()
    {
        if (this.mCamera != null)
        {
            float orthographicSize = this.mCamera.orthographicSize;
            base.gameObject.SetLocalScale(orthographicSize, orthographicSize);
        }
    }
}

