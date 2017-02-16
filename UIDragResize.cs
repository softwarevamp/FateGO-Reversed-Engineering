using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag-Resize Widget")]
public class UIDragResize : MonoBehaviour
{
    public int maxHeight = 0x186a0;
    public int maxWidth = 0x186a0;
    private bool mDragging;
    private int mHeight;
    public int minHeight = 100;
    public int minWidth = 100;
    private Vector3 mLocalPos;
    private Plane mPlane;
    private Vector3 mRayPos;
    private int mWidth;
    public UIWidget.Pivot pivot = UIWidget.Pivot.BottomRight;
    public UIWidget target;

    private void OnDrag(Vector2 delta)
    {
        if (this.mDragging && (this.target != null))
        {
            float num;
            Ray currentRay = UICamera.currentRay;
            if (this.mPlane.Raycast(currentRay, out num))
            {
                Transform cachedTransform = this.target.cachedTransform;
                cachedTransform.localPosition = this.mLocalPos;
                this.target.width = this.mWidth;
                this.target.height = this.mHeight;
                Vector3 vector = currentRay.GetPoint(num) - this.mRayPos;
                cachedTransform.position += vector;
                Vector3 vector2 = (Vector3) (Quaternion.Inverse(cachedTransform.localRotation) * (cachedTransform.localPosition - this.mLocalPos));
                cachedTransform.localPosition = this.mLocalPos;
                NGUIMath.ResizeWidget(this.target, this.pivot, vector2.x, vector2.y, this.minWidth, this.minHeight, this.maxWidth, this.maxHeight);
            }
        }
    }

    private void OnDragEnd()
    {
        this.mDragging = false;
    }

    private void OnDragStart()
    {
        if (this.target != null)
        {
            float num;
            Vector3[] worldCorners = this.target.worldCorners;
            this.mPlane = new Plane(worldCorners[0], worldCorners[1], worldCorners[3]);
            Ray currentRay = UICamera.currentRay;
            if (this.mPlane.Raycast(currentRay, out num))
            {
                this.mRayPos = currentRay.GetPoint(num);
                this.mLocalPos = this.target.cachedTransform.localPosition;
                this.mWidth = this.target.width;
                this.mHeight = this.target.height;
                this.mDragging = true;
            }
        }
    }
}

