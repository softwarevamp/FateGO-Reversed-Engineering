using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class CTouch
{
    public static float DRAG_LEN = 15f;
    public static float FLICK_LEN = 5f;
    private static int mDragFrameCnt = -1;
    private static float mDragLen;
    private static bool mIsInitDone;
    private static bool mIsReq_MultiTouchEnabled;
    private static bool mIsTchMouseNow = false;
    private static bool mIsTchNow = false;
    private static Vector2 mPosNow;
    private static Vector2 mPosPush;
    private static int mProcessOldFrameCount;
    private static Camera mScreenCam;
    private static Vector2 mScrPosDelta;
    private static float mScrPosDeltaLen;
    private static float mScrPosDeltaLenOld;
    private static Vector2 mScrPosDeltaOld;
    private static Vector2 mScrPosNow;
    private static Vector2 mScrPosOld;
    private static Vector2 mScrPosPush;
    private static TCH_STATE mState = TCH_STATE.NONE;
    private static UnityEngine.Touch[] mTouch = new UnityEngine.Touch[1];
    public const int TOUCH_MAX = 1;

    private static  event TouchEventHandler mOnTouchPressEvent;

    private static  event TouchEventHandler mOnTouchReleaseEvent;

    public static  event TouchEventHandler OnTouchPressEvent;

    public static  event TouchEventHandler OnTouchReleaseEvent;

    public static int getDragFrameCnt() => 
        mDragFrameCnt;

    public static int getFlickDirX()
    {
        Vector2 vector;
        Vector2 zero;
        if (isFlick())
        {
            vector = getScrPosDelta();
            zero = Vector2.zero;
            if (vector.x < 0f)
            {
                zero.x = -1f;
                goto Label_005E;
            }
            if (vector.x > 0f)
            {
                zero.x = 1f;
                goto Label_005E;
            }
        }
        return 0;
    Label_005E:
        if (Vector2.Dot(vector.normalized, zero.normalized) < 0.75f)
        {
            return 0;
        }
        return (int) zero.x;
    }

    public static int getFlickDirY()
    {
        Vector2 vector;
        Vector2 zero;
        if (isFlick())
        {
            vector = getScrPosDelta();
            zero = Vector2.zero;
            if (vector.y > 0f)
            {
                zero.y = -1f;
                goto Label_005E;
            }
            if (vector.y < 0f)
            {
                zero.y = 1f;
                goto Label_005E;
            }
        }
        return 0;
    Label_005E:
        if (Vector2.Dot(vector.normalized, zero.normalized) < 0.75f)
        {
            return 0;
        }
        return (int) zero.y;
    }

    public static Vector2 getPosNow() => 
        mPosNow;

    public static Vector2 getScreenPosition() => 
        mScrPosNow;

    public static Vector2 getScreenPosition(Camera cam) => 
        getScreenPosition(GetTouchPos(), mScreenCam);

    public static Vector2 getScreenPosition(Vector2 tch_pos) => 
        getScreenPosition(tch_pos, mScreenCam);

    public static Vector2 getScreenPosition(Vector2 tch_pos, Camera cam)
    {
        if (cam == null)
        {
            return Vector2.zero;
        }
        Vector3 origin = mScreenCam.ScreenPointToRay((Vector3) tch_pos).origin;
        origin.x /= mScreenCam.transform.lossyScale.x;
        origin.y /= mScreenCam.transform.lossyScale.y;
        return origin;
    }

    public static Vector2 getScrPosDelta() => 
        mScrPosDelta;

    public static float getScrPosDeltaLen() => 
        mScrPosDeltaLen;

    public static float getScrPosDeltaLenOld() => 
        mScrPosDeltaLenOld;

    public static Vector2 getScrPosDeltaOld() => 
        mScrPosDeltaOld;

    public static Vector2 GetTouchPos()
    {
        Vector2 mousePosition = Input.mousePosition;
        if (Input.touchCount == 1)
        {
            mousePosition = Input.GetTouch(0).position;
        }
        return mousePosition;
    }

    public static TCH_STATE getTouchState() => 
        mState;

    public static void init()
    {
        if (!mIsInitDone)
        {
            mIsInitDone = true;
            mIsReq_MultiTouchEnabled = false;
        }
    }

    public static bool isDrag() => 
        (isDragMode() && isTouchKeep());

    public static bool isDragMode() => 
        (mDragFrameCnt >= 0);

    public static bool isFlick() => 
        isFlick(FLICK_LEN);

    public static bool isFlick(float flickLen) => 
        (isTouchRelease() && (getScrPosDeltaLen() >= flickLen));

    public static bool isTouchKeep() => 
        (mState == TCH_STATE.KEEP);

    public static bool isTouchNone() => 
        (mState == TCH_STATE.NONE);

    public static bool isTouchPush() => 
        (mState == TCH_STATE.PUSH);

    public static bool isTouchRelease() => 
        (mState == TCH_STATE.RELEASE);

    public static void process()
    {
        if (mIsInitDone && (mProcessOldFrameCount != Time.frameCount))
        {
            mProcessOldFrameCount = Time.frameCount;
            if (Input.multiTouchEnabled && !mIsReq_MultiTouchEnabled)
            {
                if (Input.touchCount <= 1)
                {
                    Input.multiTouchEnabled = false;
                }
            }
            else if (!Input.multiTouchEnabled && mIsReq_MultiTouchEnabled)
            {
                Input.multiTouchEnabled = true;
            }
            processSingleTouch();
        }
    }

    private static void processSingleTouch()
    {
        mState = TCH_STATE.NONE;
        mPosNow = GetTouchPos();
        if (Input.touchCount == 1)
        {
            mTouch[0] = Input.GetTouch(0);
        }
        mScrPosOld = mScrPosNow;
        mScrPosNow = getScreenPosition(mPosNow);
        if (!mIsTchNow && (((Input.touchCount == 1) && (mTouch[0].phase == TouchPhase.Began)) || Input.GetMouseButtonDown(0)))
        {
            if (Input.touchCount == 0)
            {
                mIsTchMouseNow = true;
            }
            mIsTchNow = true;
            mState = TCH_STATE.PUSH;
            mPosPush = mPosNow;
            mScrPosPush = getScreenPosition(mPosPush);
            mScrPosOld = mScrPosNow;
            mScrPosDeltaOld = Vector2.zero;
            mScrPosDelta = Vector2.zero;
            mDragFrameCnt = -1;
            mDragLen = 0f;
            if (mOnTouchPressEvent != null)
            {
                mOnTouchPressEvent();
            }
        }
        else if (mIsTchNow && (((Input.touchCount == 1) && ((mTouch[0].phase == TouchPhase.Moved) || (mTouch[0].phase == TouchPhase.Stationary))) || mIsTchMouseNow))
        {
            mState = TCH_STATE.KEEP;
        }
        if (mIsTchNow && (((Input.touchCount == 1) && ((mTouch[0].phase == TouchPhase.Ended) || (mTouch[0].phase == TouchPhase.Canceled))) || Input.GetMouseButtonUp(0)))
        {
            mIsTchMouseNow = false;
            mIsTchNow = false;
            mState = TCH_STATE.RELEASE;
            if (mOnTouchReleaseEvent != null)
            {
                mOnTouchReleaseEvent();
            }
        }
        if (isTouchKeep() && (mDragFrameCnt >= 0))
        {
            mDragFrameCnt++;
        }
        if (isTouchKeep() && (mDragFrameCnt < 0))
        {
            Vector2 vector = mScrPosNow - mScrPosPush;
            mDragLen = vector.magnitude;
            if (mDragLen >= DRAG_LEN)
            {
                mDragFrameCnt = 0;
            }
        }
        mScrPosDeltaOld = mScrPosDelta;
        mScrPosDelta = mScrPosNow - mScrPosOld;
        mScrPosDeltaLenOld = mScrPosDeltaLen;
        mScrPosDeltaLen = 0f;
        if (mScrPosDelta != Vector2.zero)
        {
            mScrPosDeltaLen = mScrPosDelta.magnitude;
        }
    }

    public static void SetMultiTouchEnabled(bool is_enabled)
    {
        mIsReq_MultiTouchEnabled = is_enabled;
    }

    public static void setParam(float flick_len, float drag_len, Camera screen_cam)
    {
        if (mIsInitDone)
        {
            FLICK_LEN = flick_len;
            DRAG_LEN = drag_len;
            setScreenCamera(screen_cam);
        }
    }

    public static void setScreenCamera(Camera cam)
    {
        mScreenCam = cam;
    }

    public enum TCH_STATE
    {
        NONE,
        PUSH,
        KEEP,
        RELEASE
    }

    public delegate void TouchEventHandler();
}

