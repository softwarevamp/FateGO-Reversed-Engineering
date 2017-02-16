using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MapZoom
{
    private float mAutoZmChg;
    private float mAutoZmEdVal;
    private System.Action mAutoZmEndAct;
    private int mAutoZmFrm;
    private float mAutoZmNow;
    private float mAutoZmStVal;
    private Camera mCamera;
    private bool mIsAutoZoom;
    private bool mIsTouchEnable;
    private bool mIsTouchNow;
    private float mLenOld;
    private MapCamera mMapCamera;
    private bool mOldMultiTouchEnabled;
    private float mSpd;
    private UnityEngine.Touch[] mTchInf = new UnityEngine.Touch[2];
    private float mTgt;
    private float mZoomMax;
    private float mZoomRange;
    private const int TOUCH_NUM = 2;
    public static readonly float ZOOM_BASE_MAX = 1.675f;
    public static readonly float ZOOM_MARGIN = 0.2f;
    public static readonly float ZOOM_MIN = 0.45f;

    private float CalcSpeed()
    {
        int touchCount = this.GetTouchCount();
        if (touchCount > 2)
        {
            return 0f;
        }
        bool flag = false;
        Vector2 zero = Vector2.zero;
        for (int i = 0; i < touchCount; i++)
        {
            this.mTchInf[i] = Input.GetTouch(i);
        }
        zero = this.mTchInf[1].position;
        for (int j = 0; j < touchCount; j++)
        {
            if (this.mTchInf[j].phase == TouchPhase.Moved)
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            return 0f;
        }
        float num4 = 0f;
        Vector2 vector2 = CTouch.getScreenPosition();
        Vector2 vector4 = CTouch.getScreenPosition(zero) - vector2;
        float magnitude = vector4.magnitude;
        if (this.mLenOld != 0f)
        {
            float num7 = this.mLenOld - magnitude;
            num4 = num7 * 0.01f;
        }
        this.mLenOld = magnitude;
        float num9 = this.mTgt + num4;
        float num10 = 0f;
        if (num9 < ZOOM_MIN)
        {
            num10 = ZOOM_MIN - num9;
        }
        else if (num9 > this.mZoomMax)
        {
            num10 = this.mZoomMax - num9;
        }
        float num11 = 1f - Mathf.Clamp((float) (Math.Abs(num10) * 1f), (float) 0f, (float) 1f);
        num4 *= num11;
        if (num4 < 0f)
        {
            this.IsZoomMaxFit = false;
        }
        return num4;
    }

    private int GetTouchCount() => 
        Input.touchCount;

    public float GetZoomRate()
    {
        float num = Mathf.Clamp(this.GetZoomSize(), ZOOM_MIN, this.mZoomMax) - ZOOM_MIN;
        return (num / this.mZoomRange);
    }

    public float GetZoomSize() => 
        this.mCamera.orthographicSize;

    public void Init(MapCamera mc)
    {
        if (this.mCamera != null)
        {
            this.UnInit();
        }
        this.mMapCamera = mc;
        this.mCamera = mc.GetCamera();
        this.SetZoomSize(ZOOM_MIN, true);
        this.mOldMultiTouchEnabled = Input.multiTouchEnabled;
    }

    public bool IsStop() => 
        (this.mSpd == 0f);

    private void Limit(float spd_rate)
    {
        if (!this.IsZoomMaxFit)
        {
            float mTgt = this.mTgt;
            float num2 = 0f;
            if (mTgt < ZOOM_MIN)
            {
                num2 = ZOOM_MIN - mTgt;
            }
            else if (mTgt > this.mZoomMax)
            {
                num2 = this.mZoomMax - mTgt;
            }
            mTgt += num2 * spd_rate;
            this.mTgt = mTgt;
        }
    }

    [Conditional("UNITY_EDITOR")]
    private void MouseScrollWheel()
    {
        Vector3 mousePosition = Input.mousePosition;
        if (((0f <= mousePosition.x) && (mousePosition.x <= Screen.width)) && ((0f <= mousePosition.y) && (mousePosition.y <= Screen.height)))
        {
            float axis = Input.GetAxis("Mouse ScrollWheel");
            float num2 = 0.25f;
            if (0f < axis)
            {
                axis = -num2;
            }
            else if (axis < 0f)
            {
                axis = num2;
            }
            if (axis < 0f)
            {
                this.IsZoomMaxFit = false;
            }
            this.mTgt += axis;
        }
    }

    public void Process(bool is_tch_enable)
    {
        float zoomSize = this.GetZoomSize();
        CTouch.SetMultiTouchEnabled(is_tch_enable);
        this.mIsAutoZoom = false;
        if (this.mAutoZmFrm > 0)
        {
            this.mAutoZmFrm--;
            if (this.mAutoZmFrm > 0)
            {
                this.mAutoZmNow += this.mAutoZmChg;
                float num2 = (this.mAutoZmNow * this.mAutoZmNow) * this.mAutoZmNow;
                float num3 = (this.mAutoZmStVal - this.mAutoZmEdVal) * num2;
                zoomSize = this.mAutoZmEdVal + num3;
            }
            else
            {
                zoomSize = this.mAutoZmEdVal;
                this.mAutoZmEndAct.Call();
            }
            this.SetZoomSize(zoomSize, false);
            this.mIsAutoZoom = true;
        }
        else
        {
            if (!this.mIsTouchNow)
            {
                this.Limit(0.45f);
                if (((this.GetTouchCount() == 2) && this.mIsTouchEnable) && is_tch_enable)
                {
                    this.mLenOld = 0f;
                    this.mIsTouchNow = true;
                }
                if (this.IsZoomMaxFitPosFix && !this.IsZoomMaxFit)
                {
                    this.IsZoomMaxFitPosFix = false;
                }
            }
            else if (((this.GetTouchCount() != 2) || !this.mIsTouchEnable) || !is_tch_enable)
            {
                this.mIsTouchNow = false;
            }
            else
            {
                float num5 = this.CalcSpeed();
                this.mTgt += num5;
            }
            this.mSpd = (this.mTgt - zoomSize) * 0.75f;
            zoomSize += this.mSpd;
            this.SetZoomSize(zoomSize, false);
            if (!this.IsStop() && (Math.Abs(this.mSpd) < 0.01f))
            {
                this.Stop(true);
            }
            if (this.IsTouchEnable && is_tch_enable)
            {
            }
        }
    }

    public void SetZoomRate(float rate)
    {
        this.mZoomMax = ZOOM_BASE_MAX * rate;
        this.mZoomRange = this.mZoomMax - ZOOM_MIN;
        this.IsZoomMaxFit = false;
    }

    public void SetZoomSize(float size, bool is_tgt_update = false)
    {
        float num = ZOOM_MIN - ZOOM_MARGIN;
        float num2 = this.mZoomMax + ZOOM_MARGIN;
        if (size < num)
        {
            size = num;
        }
        else if (size >= num2)
        {
            size = num2;
            this.mTgt = size;
            this.IsZoomMaxFit = true;
            this.IsZoomMaxFitPosFix = true;
        }
        this.mCamera.orthographicSize = size;
        if (is_tgt_update)
        {
            this.mTgt = this.mCamera.orthographicSize;
        }
    }

    public void StartAutoZoom(float zoom, float sec, System.Action endAct = null)
    {
        this.mTgt = zoom;
        this.Stop(false);
        int num = (int) (Application.targetFrameRate * sec);
        if (num <= 0)
        {
            num = 1;
        }
        this.mAutoZmFrm = num;
        this.mAutoZmStVal = this.GetZoomSize();
        this.mAutoZmEdVal = zoom;
        this.mAutoZmNow = 1f;
        this.mAutoZmChg = (0f - this.mAutoZmNow) / ((float) this.mAutoZmFrm);
        this.mAutoZmEndAct = endAct;
        this.mIsAutoZoom = true;
    }

    public void Stop(bool is_force = false)
    {
        this.mSpd = 0f;
        if (is_force)
        {
            this.SetZoomSize(this.mTgt, false);
        }
    }

    public void UnInit()
    {
        if (this.mMapCamera != null)
        {
            this.mMapCamera = null;
            CTouch.SetMultiTouchEnabled(this.mOldMultiTouchEnabled);
        }
    }

    public bool IsAutoZoom =>
        this.mIsAutoZoom;

    public bool IsTouchEnable
    {
        get => 
            this.mIsTouchEnable;
        set
        {
            this.mIsTouchEnable = value;
        }
    }

    public bool IsTouchNow =>
        this.mIsTouchNow;

    public bool IsZoomMaxFit { get; private set; }

    public bool IsZoomMaxFitPosFix { get; private set; }
}

