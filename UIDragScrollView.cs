using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Drag Scroll View")]
public class UIDragScrollView : MonoBehaviour
{
    [HideInInspector, SerializeField]
    private UIScrollView draggablePanel;
    private bool mAutoFind;
    private UIScrollView mScroll;
    private bool mStarted;
    private Transform mTrans;
    public UIScrollView scrollView;

    private void FindScrollView()
    {
        UIScrollView view = NGUITools.FindInParents<UIScrollView>(this.mTrans);
        if ((this.scrollView == null) || (this.mAutoFind && (view != this.scrollView)))
        {
            this.scrollView = view;
            this.mAutoFind = true;
        }
        else if (this.scrollView == view)
        {
            this.mAutoFind = true;
        }
        this.mScroll = this.scrollView;
    }

    private void OnDrag(Vector2 delta)
    {
        if ((this.scrollView != null) && NGUITools.GetActive(this))
        {
            this.scrollView.Drag();
        }
    }

    private void OnEnable()
    {
        this.mTrans = base.transform;
        if ((this.scrollView == null) && (this.draggablePanel != null))
        {
            this.scrollView = this.draggablePanel;
            this.draggablePanel = null;
        }
        if (this.mStarted && (this.mAutoFind || (this.mScroll == null)))
        {
            this.FindScrollView();
        }
    }

    private void OnPress(bool pressed)
    {
        if (this.mAutoFind && (this.mScroll != this.scrollView))
        {
            this.mScroll = this.scrollView;
            this.mAutoFind = false;
        }
        if (((this.scrollView != null) && base.enabled) && NGUITools.GetActive(base.gameObject))
        {
            this.scrollView.Press(pressed);
            if (!pressed && this.mAutoFind)
            {
                this.scrollView = NGUITools.FindInParents<UIScrollView>(this.mTrans);
                this.mScroll = this.scrollView;
            }
        }
    }

    private void OnScroll(float delta)
    {
        if ((this.scrollView != null) && NGUITools.GetActive(this))
        {
            this.scrollView.Scroll(delta);
        }
    }

    private void Start()
    {
        this.mStarted = true;
        this.FindScrollView();
    }
}

