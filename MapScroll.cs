using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class MapScroll
{
    private float mAutoMvChg;
    private Vector2 mAutoMvEdPos;
    private System.Action mAutoMvEndAct;
    private int mAutoMvFrm;
    private float mAutoMvNow;
    private Vector2 mAutoMvStPos;
    private Camera mCamera;
    private Vector2 mInertiaSpd;
    private bool mIsTouchEnable;
    private bool mIsTouchNow;
    private MapCamera mMapCamera;
    private System.Action mPlaySEAction_Flick;
    private Vector2 mPosNow;
    private Vector2 mPosOld;
    private Vector2 mSpd;
    private Vector2 mTchDif;
    private Vector2 mTchDifOld;
    private Vector2 mTgt;

    private bool BrakeMv(float spd_rate = 0f) => 
        this.BrakeMv(this.mMapCamera.CameraRect, spd_rate);

    private bool BrakeMv(Rect cam_rect, float spd_rate = 0f)
    {
        Vector2 mTgt = this.mTgt;
        Rect mvBrakeRect = this.mMapCamera.MvBrakeRect;
        if (cam_rect.x < mvBrakeRect.x)
        {
            if (spd_rate <= 0f)
            {
                return true;
            }
            mTgt.x += (mvBrakeRect.x - cam_rect.x) * spd_rate;
        }
        else if (cam_rect.width > mvBrakeRect.width)
        {
            if (spd_rate <= 0f)
            {
                return true;
            }
            mTgt.x += (mvBrakeRect.width - cam_rect.width) * spd_rate;
        }
        if (cam_rect.y > mvBrakeRect.y)
        {
            if (spd_rate <= 0f)
            {
                return true;
            }
            mTgt.y += (mvBrakeRect.y - cam_rect.y) * spd_rate;
        }
        else if (cam_rect.height < mvBrakeRect.height)
        {
            if (spd_rate <= 0f)
            {
                return true;
            }
            mTgt.y += (mvBrakeRect.height - cam_rect.height) * spd_rate;
        }
        this.mTgt = mTgt;
        return false;
    }

    public Vector2 GetScrlPos() => 
        this.GetScrlPosVec3();

    public Vector3 GetScrlPosVec3() => 
        this.mCamera.gameObject.GetLocalPosition();

    public Vector2 GetScrlTgtPos() => 
        this.mTgt;

    public void Init(MapCamera mc)
    {
        if (this.mMapCamera != null)
        {
            this.UnInit();
        }
        this.mMapCamera = mc;
        this.mCamera = mc.GetCamera();
    }

    public bool IsStop() => 
        (this.mSpd == Vector2.zero);

    private void LimitMv()
    {
        float num4 = 1f - this.mMapCamera.Zoom.GetZoomRate();
        float num5 = (0.124f * num4) + 0.001f;
        num5 = -num5;
        Vector2 pos = this.LimitMv(this.GetScrlPos(), this.mMapCamera.CameraRect, num5);
        this.SetScrlPos(pos);
    }

    private Vector2 LimitMv(Vector2 pos, Rect cam_rect, float rebound_rate = 0)
    {
        Vector2 vector = pos;
        Rect mvLimitRect = this.mMapCamera.MvLimitRect;
        if (cam_rect.x < mvLimitRect.x)
        {
            vector.x += mvLimitRect.x - cam_rect.x;
            this.mTgt.x = vector.x;
            this.mInertiaSpd.x *= rebound_rate;
        }
        else if (cam_rect.width > mvLimitRect.width)
        {
            vector.x += mvLimitRect.width - cam_rect.width;
            this.mTgt.x = vector.x;
            this.mInertiaSpd.x *= rebound_rate;
        }
        if (cam_rect.y > mvLimitRect.y)
        {
            vector.y += mvLimitRect.y - cam_rect.y;
            this.mTgt.y = vector.y;
            this.mInertiaSpd.y *= rebound_rate;
        }
        else if (cam_rect.height < mvLimitRect.height)
        {
            vector.y += mvLimitRect.height - cam_rect.height;
            this.mTgt.y = vector.y;
            this.mInertiaSpd.y *= rebound_rate;
        }
        if (this.mMapCamera.Zoom.IsZoomMaxFitPosFix)
        {
            float mapBgRateW = this.mMapCamera.MapBgRateW;
            float mapBgRateH = this.mMapCamera.MapBgRateH;
            if ((mapBgRateW < mapBgRateH) || (mapBgRateW == mapBgRateH))
            {
                vector.x = 0f;
                this.mTgt.x = vector.x;
            }
            if ((mapBgRateW <= mapBgRateH) && (mapBgRateW != mapBgRateH))
            {
                return vector;
            }
            vector.y = 0f;
            this.mTgt.y = vector.y;
        }
        return vector;
    }

    public void Process(bool is_tch_enable)
    {
        Vector2 scrlPos = this.GetScrlPos();
        if (this.mAutoMvFrm > 0)
        {
            this.mAutoMvFrm--;
            if (this.mAutoMvFrm > 0)
            {
                this.mAutoMvNow += this.mAutoMvChg;
                float num = (this.mAutoMvNow * this.mAutoMvNow) * this.mAutoMvNow;
                Vector2 vector2 = (Vector2) ((this.mAutoMvStPos - this.mAutoMvEdPos) * num);
                scrlPos = this.mAutoMvEdPos + vector2;
            }
            else
            {
                scrlPos = this.mAutoMvEdPos;
                this.mAutoMvEndAct.Call();
            }
            this.SetScrlPos(scrlPos);
        }
        else
        {
            this.mPosOld = this.mPosNow;
            this.mPosNow = CTouch.getScreenPosition();
            bool flag = false;
            if (!this.mIsTouchNow)
            {
                this.mTgt += this.mInertiaSpd;
                this.mInertiaSpd = (Vector2) (this.mInertiaSpd * 0.88f);
                if ((Math.Abs(this.mInertiaSpd.x) < 0.01f) && (Math.Abs(this.mInertiaSpd.y) < 0.01f))
                {
                    this.Stop(false);
                }
                this.BrakeMv(0.45f);
                if ((CTouch.isTouchPush() && this.mIsTouchEnable) && is_tch_enable)
                {
                    this.mTchDifOld = Vector2.zero;
                    this.mTchDif = Vector2.zero;
                    this.mIsTouchNow = true;
                }
            }
            else
            {
                if ((!CTouch.isTouchKeep() || !this.mIsTouchEnable) || !is_tch_enable)
                {
                    this.mIsTouchNow = false;
                    Vector2 mTchDif = this.mTchDif;
                    if (mTchDif.sqrMagnitude < this.mTchDifOld.sqrMagnitude)
                    {
                        mTchDif = this.mTchDifOld;
                    }
                    this.mInertiaSpd += mTchDif;
                    if (this.mPlaySEAction_Flick != null)
                    {
                        this.mPlaySEAction_Flick();
                    }
                }
                else
                {
                    this.Stop(false);
                }
                if ((CTouch.isDrag() && this.mIsTouchEnable) && is_tch_enable)
                {
                    this.mTchDifOld = this.mTchDif;
                    this.mTchDif = this.mPosOld - this.mPosNow;
                    this.mTchDif = (Vector2) (this.mTchDif * this.mMapCamera.Zoom.GetZoomSize());
                    if (this.BrakeMv(0f))
                    {
                        this.mTchDif = (Vector2) (this.mTchDif / 2f);
                    }
                    this.mTgt += this.mTchDif;
                    flag = true;
                }
            }
            this.mSpd = this.mTgt - scrlPos;
            if (!flag)
            {
                this.mSpd = (Vector2) (this.mSpd * 0.75f);
            }
            scrlPos += this.mSpd;
            this.SetScrlPos(scrlPos);
            if ((!this.IsStop() && (Math.Abs(this.mSpd.x) < 0.01f)) && (Math.Abs(this.mSpd.y) < 0.01f))
            {
                this.Stop(true);
            }
            this.LimitMv();
        }
    }

    public void SetScrlPos(Vector2 pos)
    {
        this.mCamera.gameObject.SetLocalPosition(pos);
    }

    public void SetScrlTgtPos(Vector2 pos)
    {
        this.mTgt = pos;
    }

    public void StartAutoMove(Vector3 screenPos, float sec, System.Action endAct = null)
    {
        Vector2 vector = new Vector2(screenPos.x, screenPos.y);
        this.mTgt = new Vector2(vector.x, vector.y);
        this.Stop(false);
        this.BrakeMv(this.mMapCamera.GetWorldRect((Vector3) this.mTgt), 1f);
        int num = (int) (Application.targetFrameRate * sec);
        if (num <= 0)
        {
            num = 1;
        }
        this.mAutoMvFrm = num;
        this.mAutoMvStPos = this.GetScrlPos();
        this.mAutoMvEdPos = this.mTgt;
        this.mAutoMvNow = 1f;
        this.mAutoMvChg = (0f - this.mAutoMvNow) / ((float) this.mAutoMvFrm);
        this.mAutoMvEndAct = endAct;
    }

    public void Stop(bool is_force = false)
    {
        if (is_force)
        {
            this.SetScrlPos(this.mTgt);
            this.mSpd = Vector2.zero;
        }
        this.mInertiaSpd = Vector2.zero;
    }

    public void UnInit()
    {
        if (this.mMapCamera != null)
        {
            this.mMapCamera = null;
        }
    }

    public bool IsTouchEnable
    {
        get => 
            this.mIsTouchEnable;
        set
        {
            this.mIsTouchEnable = value;
        }
    }
}

