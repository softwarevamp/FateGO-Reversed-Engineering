using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center Scroll View on Child")]
public class UICenterOnChild : MonoBehaviour
{
    private GameObject mCenteredObject;
    private UIScrollView mScrollView;
    public float nextPageThreshold;
    public OnCenterCallback onCenter;
    public SpringPanel.OnFinished onFinished;
    public float springStrength = 8f;

    public void CenterOn(Transform target)
    {
        if ((this.mScrollView != null) && (this.mScrollView.panel != null))
        {
            Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
            Vector3 panelCenter = (Vector3) ((worldCorners[2] + worldCorners[0]) * 0.5f);
            this.CenterOn(target, panelCenter);
        }
    }

    private void CenterOn(Transform target, Vector3 panelCenter)
    {
        if (((target != null) && (this.mScrollView != null)) && (this.mScrollView.panel != null))
        {
            Transform cachedTransform = this.mScrollView.panel.cachedTransform;
            this.mCenteredObject = target.gameObject;
            Vector3 vector = cachedTransform.InverseTransformPoint(target.position);
            Vector3 vector2 = cachedTransform.InverseTransformPoint(panelCenter);
            Vector3 vector3 = vector - vector2;
            if (!this.mScrollView.canMoveHorizontally)
            {
                vector3.x = 0f;
            }
            if (!this.mScrollView.canMoveVertically)
            {
                vector3.y = 0f;
            }
            vector3.z = 0f;
            SpringPanel.Begin(this.mScrollView.panel.cachedGameObject, cachedTransform.localPosition - vector3, this.springStrength).onFinished = this.onFinished;
        }
        else
        {
            this.mCenteredObject = null;
        }
        if (this.onCenter != null)
        {
            this.onCenter(this.mCenteredObject);
        }
    }

    private void OnDisable()
    {
        if (this.mScrollView != null)
        {
            this.mScrollView.centerOnChild = null;
        }
    }

    private void OnDragFinished()
    {
        if (base.enabled)
        {
            this.Recenter();
        }
    }

    private void OnEnable()
    {
        if (this.mScrollView != null)
        {
            this.mScrollView.centerOnChild = this;
            this.Recenter();
        }
    }

    private void OnValidate()
    {
        this.nextPageThreshold = Mathf.Abs(this.nextPageThreshold);
    }

    [ContextMenu("Execute")]
    public void Recenter()
    {
        if (this.mScrollView == null)
        {
            this.mScrollView = NGUITools.FindInParents<UIScrollView>(base.gameObject);
            if (this.mScrollView == null)
            {
                Debug.LogWarning(string.Concat(new object[] { base.GetType(), " requires ", typeof(UIScrollView), " on a parent object in order to work" }), this);
                base.enabled = false;
                return;
            }
            if (this.mScrollView != null)
            {
                this.mScrollView.centerOnChild = this;
                this.mScrollView.onDragFinished = (UIScrollView.OnDragNotification) Delegate.Combine(this.mScrollView.onDragFinished, new UIScrollView.OnDragNotification(this.OnDragFinished));
            }
            if (this.mScrollView.horizontalScrollBar != null)
            {
                this.mScrollView.horizontalScrollBar.onDragFinished = (UIProgressBar.OnDragFinished) Delegate.Combine(this.mScrollView.horizontalScrollBar.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
            }
            if (this.mScrollView.verticalScrollBar != null)
            {
                this.mScrollView.verticalScrollBar.onDragFinished = (UIProgressBar.OnDragFinished) Delegate.Combine(this.mScrollView.verticalScrollBar.onDragFinished, new UIProgressBar.OnDragFinished(this.OnDragFinished));
            }
        }
        if (this.mScrollView.panel != null)
        {
            Transform transform = base.transform;
            if (transform.childCount != 0)
            {
                Vector3[] worldCorners = this.mScrollView.panel.worldCorners;
                Vector3 panelCenter = (Vector3) ((worldCorners[2] + worldCorners[0]) * 0.5f);
                Vector3 velocity = (Vector3) (this.mScrollView.currentMomentum * this.mScrollView.momentumAmount);
                Vector3 vector3 = NGUIMath.SpringDampen(ref velocity, 9f, 2f);
                Vector3 vector4 = panelCenter - ((Vector3) (vector3 * 0.01f));
                float maxValue = float.MaxValue;
                Transform target = null;
                int index = 0;
                int num3 = 0;
                int num4 = 0;
                int childCount = transform.childCount;
                int num6 = 0;
                while (num4 < childCount)
                {
                    Transform child = transform.GetChild(num4);
                    if (child.gameObject.activeInHierarchy)
                    {
                        float num7 = Vector3.SqrMagnitude(child.position - vector4);
                        if (num7 < maxValue)
                        {
                            maxValue = num7;
                            target = child;
                            index = num4;
                            num3 = num6;
                        }
                        num6++;
                    }
                    num4++;
                }
                if (((this.nextPageThreshold > 0f) && (UICamera.currentTouch != null)) && ((this.mCenteredObject != null) && (this.mCenteredObject.transform == transform.GetChild(index))))
                {
                    Vector3 totalDelta = (Vector3) UICamera.currentTouch.totalDelta;
                    totalDelta = (Vector3) (base.transform.rotation * totalDelta);
                    float f = 0f;
                    UIScrollView.Movement movement = this.mScrollView.movement;
                    if (movement == UIScrollView.Movement.Horizontal)
                    {
                        f = totalDelta.x;
                    }
                    else if (movement == UIScrollView.Movement.Vertical)
                    {
                        f = totalDelta.y;
                    }
                    else
                    {
                        f = totalDelta.magnitude;
                    }
                    if (Mathf.Abs(f) > this.nextPageThreshold)
                    {
                        UIGrid component = base.GetComponent<UIGrid>();
                        if ((component != null) && (component.sorting != UIGrid.Sorting.None))
                        {
                            List<Transform> childList = component.GetChildList();
                            if (f > this.nextPageThreshold)
                            {
                                if (num3 > 0)
                                {
                                    target = childList[num3 - 1];
                                }
                                else
                                {
                                    target = (base.GetComponent<UIWrapContent>() != null) ? childList[childList.Count - 1] : childList[0];
                                }
                            }
                            else if (f < -this.nextPageThreshold)
                            {
                                if (num3 < (childList.Count - 1))
                                {
                                    target = childList[num3 + 1];
                                }
                                else
                                {
                                    target = (base.GetComponent<UIWrapContent>() != null) ? childList[0] : childList[childList.Count - 1];
                                }
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Next Page Threshold requires a sorted UIGrid in order to work properly", this);
                        }
                    }
                }
                this.CenterOn(target, panelCenter);
            }
        }
    }

    private void Start()
    {
        this.Recenter();
    }

    public GameObject centeredObject =>
        this.mCenteredObject;

    public delegate void OnCenterCallback(GameObject centeredObject);
}

