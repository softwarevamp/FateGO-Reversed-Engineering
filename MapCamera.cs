using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    private static readonly Vector2 left_top = new Vector2(-SCREEN_W_HALF, SCREEN_H_HALF);
    public static readonly int MAP_BASE_H = 0x4d8;
    public static readonly int MAP_BASE_W = 0x780;
    [SerializeField]
    private Camera mCamera;
    private Rect mCameraRect;
    private bool mIsInitDone;
    private bool mIsTouchEnable;
    [SerializeField]
    private UITexture mMapBg;
    private Rect mMvBrakeRect;
    private Rect mMvLimitRect;
    private MapScroll mScrl;
    private MapZoom mZoom;
    private static readonly Vector2 right_bottom = new Vector2(SCREEN_W_HALF, -SCREEN_H_HALF);
    private static readonly float SCREEN_H_HALF = (ManagerConfig.HEIGHT / 2);
    private static readonly float SCREEN_W_HALF = (ManagerConfig.WIDTH / 2);

    private void CalcWorldRect()
    {
        Vector3 localPosition = this.mCamera.gameObject.GetLocalPosition();
        this.mCameraRect = this.GetWorldRect(localPosition);
    }

    public Camera GetCamera() => 
        this.mCamera;

    public Rect GetWorldRect(Vector3 cam_pos)
    {
        Vector2 vector = new Vector2(left_top.x, left_top.y);
        Vector2 vector2 = new Vector2(right_bottom.x, right_bottom.y);
        float zoomSize = this.mZoom.GetZoomSize();
        vector = (Vector2) (vector * zoomSize);
        vector2 = (Vector2) (vector2 * zoomSize);
        vector.x += cam_pos.x;
        vector.y += cam_pos.y;
        vector2.x += cam_pos.x;
        vector2.y += cam_pos.y;
        return new Rect(vector.x, vector.y, vector2.x, vector2.y);
    }

    public void Init()
    {
        if (!this.mIsInitDone)
        {
            this.mScrl = new MapScroll();
            this.mScrl.Init(this);
            this.mZoom = new MapZoom();
            this.mZoom.Init(this);
            this.IsTouchEnable = true;
            this.mIsInitDone = true;
            this.SetEnable(false);
        }
    }

    public bool IsEnable() => 
        this.mCamera.enabled;

    public void Process(bool is_tch_enable = true)
    {
        if (this.mIsInitDone && this.IsEnable())
        {
            this.mZoom.Process(is_tch_enable);
            this.mScrl.Process(is_tch_enable && !this.mZoom.IsTouchNow);
        }
    }

    public void SetEnable(bool is_enable)
    {
        this.mCamera.enabled = is_enable;
    }

    public void SetMapTexture(Texture2D tex, int w, int h)
    {
        this.mMapBg.mainTexture = tex;
        this.mMapBg.width = w;
        this.mMapBg.height = h;
        this.MapBgRateW = ((float) this.MapBg.width) / ((float) ManagerConfig.WIDTH);
        this.MapBgRateH = ((float) this.MapBg.height) / ((float) ManagerConfig.HEIGHT);
        this.SetMoveLimit();
        int num = w;
        float rate = ((float) w) / ((float) MAP_BASE_W);
        if (this.MapBgRateH < this.MapBgRateW)
        {
            rate = ((float) h) / ((float) MAP_BASE_H);
        }
        this.mZoom.SetZoomRate(rate);
    }

    private void SetMoveLimit()
    {
        float num2 = this.mMapBg.width - 4f;
        float num3 = this.mMapBg.height - 4f;
        float x = -num2 / 2f;
        float y = num3 / 2f;
        float width = num2 / 2f;
        float height = -num3 / 2f;
        this.mMvLimitRect = new Rect(x, y, width, height);
        float num9 = this.mMvLimitRect.x + 60f;
        float num10 = this.mMvLimitRect.y - 60f;
        float num11 = this.mMvLimitRect.width - 60f;
        float num12 = this.mMvLimitRect.height + 60f;
        this.mMvBrakeRect = new Rect(num9, num10, num11, num12);
    }

    public void UnInit()
    {
        if (this.mIsInitDone)
        {
            if (this.mScrl != null)
            {
                this.mScrl.UnInit();
                this.mScrl = null;
            }
            if (this.mZoom != null)
            {
                this.mZoom.UnInit();
                this.mZoom = null;
            }
            this.mIsInitDone = false;
        }
    }

    public Rect CameraRect
    {
        get
        {
            this.CalcWorldRect();
            return this.mCameraRect;
        }
    }

    public bool IsTouchEnable
    {
        get => 
            this.mIsTouchEnable;
        set
        {
            this.mIsTouchEnable = value;
            this.mScrl.IsTouchEnable = value;
            this.mZoom.IsTouchEnable = value;
        }
    }

    public UITexture MapBg =>
        this.mMapBg;

    public float MapBgRateH { get; private set; }

    public float MapBgRateW { get; private set; }

    public Rect MvBrakeRect =>
        this.mMvBrakeRect;

    public Rect MvLimitRect =>
        this.mMvLimitRect;

    public MapScroll Scrl =>
        this.mScrl;

    public MapZoom Zoom =>
        this.mZoom;
}

