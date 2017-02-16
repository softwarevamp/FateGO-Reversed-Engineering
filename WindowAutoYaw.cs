using System;
using UnityEngine;

[AddComponentMenu("NGUI/Examples/Window Auto-Yaw")]
public class WindowAutoYaw : MonoBehaviour
{
    private Transform mTrans;
    public Camera uiCamera;
    public int updateOrder;
    public float yawAmount = 20f;

    private void OnDisable()
    {
        this.mTrans.localRotation = Quaternion.identity;
    }

    private void OnEnable()
    {
        if (this.uiCamera == null)
        {
            this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
        this.mTrans = base.transform;
    }

    private void Update()
    {
        if (this.uiCamera != null)
        {
            Vector3 vector = this.uiCamera.WorldToViewportPoint(this.mTrans.position);
            this.mTrans.localRotation = Quaternion.Euler(0f, ((vector.x * 2f) - 1f) * this.yawAmount, 0f);
        }
    }
}

