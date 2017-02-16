using System;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public bool rotationFlip;
    public Camera targetCamera;

    private void LateUpdate()
    {
        this.UpdateBillboard();
    }

    public void setCamera(Camera wkcamera)
    {
        this.targetCamera = wkcamera;
    }

    private void Start()
    {
        if (this.targetCamera == null)
        {
            this.targetCamera = Camera.main;
        }
    }

    public void UpdateBillboard()
    {
        if (this.targetCamera != null)
        {
            Vector3 position = this.targetCamera.transform.position;
            position.y = base.transform.position.y;
            if (this.rotationFlip)
            {
                Vector3 vector2 = position - base.transform.position;
                vector2.x *= -1f;
                vector2.y *= -1f;
                vector2.z *= -1f;
                position = vector2 + base.transform.position;
            }
            base.transform.LookAt(position);
        }
    }
}

