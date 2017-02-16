using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class srcLineSprite : MonoBehaviour
{
    public const float GRAY = 0.5f;
    public UIAtlas mcAtlasP;
    public Vector2 mcFrom;
    public UISprite mcLineSprite;
    public Transform mcMyTrans;
    public Vector2 mcTo;
    public TweenAlpha mcTweenAlphaP;
    public TweenScale mcTweenScaleP;
    private CStateManager<srcLineSprite> mFSM;
    public float mfWidth = 20f;
    public int miLineH;
    public int miLineW;
    private clsMapCtrl_SpotRoadInfo mMapCtrl_SpotRoadInfo;
    public string msSpriteName = string.Empty;
    private System.Action mStateEndAct;
    private bool mtIsUpdate;
    public const float WHITE = 1f;

    private void Awake()
    {
        if (((null == this.mcMyTrans) || (null == this.mcLineSprite)) || ((null == this.mcTweenAlphaP) || (null == this.mcTweenScaleP)))
        {
            Debug.LogError("Isn't Attach!!");
        }
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<srcLineSprite>(this, 5);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StateMapMain());
            this.mFSM.add(2, new StateQaaHide());
            this.mFSM.add(3, new StateQaaGray());
            this.mFSM.add(4, new StateQaaDisp());
            this.SetState(STATE.MAP_MAIN, null);
        }
    }

    public float GetAim(Vector2 p1, Vector2 p2)
    {
        float x = p2.x - p1.x;
        float y = p2.y - p1.y;
        return (Mathf.Atan2(y, x) * 57.29578f);
    }

    public static string GetGobjName(int id) => 
        ("Spot_Line_" + id.ToString("00"));

    public clsMapCtrl_SpotRoadInfo GetMapCtrl_SpotRoadInfo() => 
        this.mMapCtrl_SpotRoadInfo;

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public void mfSetAtlas(UIAtlas cAtlasP, string sSpriteName)
    {
        this.mcAtlasP = cAtlasP;
        this.msSpriteName = sSpriteName;
        this.mtIsUpdate = true;
    }

    public void mfSetITweenSize(float fSrcW, float fDstW, float fTime)
    {
        this.mcTweenScaleP.from = new Vector3(1f, fSrcW, 1f);
        this.mcTweenScaleP.to = new Vector3(1f, fDstW, 1f);
        this.mcTweenScaleP.duration = fTime;
        this.mcTweenAlphaP.from = 0f;
        this.mcTweenAlphaP.to = 1f;
        this.mcTweenAlphaP.duration = fTime;
    }

    public void mfSetPos2(Vector2 cFrom, Vector2 cTo)
    {
        this.mcFrom = cFrom;
        this.mcTo = cTo;
        Vector2 to = this.mcTo - this.mcFrom;
        Vector2.Angle(Vector2.zero, to);
        Vector2 vector = this.mcFrom + ((Vector2) (to / 2f));
        float aim = this.GetAim(this.mcFrom, this.mcTo);
        this.mcMyTrans.localPosition = new Vector3(vector.x, vector.y, this.mcMyTrans.localPosition.z);
        this.mcMyTrans.localRotation = Quaternion.AngleAxis(aim, new Vector3(0f, 0f, 1f));
        this.miLineH = (int) this.mfWidth;
        this.miLineW = (int) (to.magnitude + 0.5f);
        this.mtIsUpdate = true;
    }

    public void SetContrast(float val)
    {
        Color color = new Color(val, val, val, 1f);
        this.mcLineSprite.color = color;
    }

    public void SetMapCtrl_SpotRoadInfo(clsMapCtrl_SpotRoadInfo _MapCtrl_SpotRoadInfo)
    {
        this.mMapCtrl_SpotRoadInfo = _MapCtrl_SpotRoadInfo;
    }

    private void SetQaaColorAnim(bool is_disp)
    {
        <SetQaaColorAnim>c__AnonStoreyE7 ye = new <SetQaaColorAnim>c__AnonStoreyE7 {
            <>f__this = this
        };
        float sec = 0.4f;
        ye.eo = base.gameObject.SafeGetComponent<EasingObject>();
        ye.from = !is_disp ? 1f : 0.5f;
        ye.to = !is_disp ? 0.5f : 1f;
        ye.eo.Play(sec, new System.Action(ye.<>m__249), new System.Action(ye.<>m__24A), 0f, Easing.TYPE.EXPONENTIAL_OUT);
    }

    private void SetQaaScaleAnim(bool is_disp)
    {
        float duration = 0.4f;
        TweenScale scale = UITweener.Begin<TweenScale>(base.gameObject, duration);
        scale.from = !is_disp ? Vector3.one : Vector3.zero;
        scale.to = !is_disp ? Vector3.zero : Vector3.one;
        scale.method = UITweener.Method.EaseOut;
        scale.eventReceiver = base.gameObject;
        scale.callWhenFinished = "StateQaaEnd";
        TweenPosition position = UITweener.Begin<TweenPosition>(base.gameObject, duration);
        Vector3 vector = new Vector3(this.mcFrom.x, this.mcFrom.y, this.mcMyTrans.localPosition.z);
        position.from = !is_disp ? this.mcMyTrans.localPosition : vector;
        position.to = !is_disp ? vector : this.mcMyTrans.localPosition;
        position.method = UITweener.Method.EaseOut;
    }

    public void SetState(STATE state, System.Action end_act = null)
    {
        this.mFSM.setState((int) state);
        this.mStateEndAct = end_act;
    }

    private void Start()
    {
    }

    private void StateQaaEnd()
    {
        this.mStateEndAct.Call();
        this.SetState(STATE.MAP_MAIN, null);
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    [CompilerGenerated]
    private sealed class <SetQaaColorAnim>c__AnonStoreyE7
    {
        internal srcLineSprite <>f__this;
        internal EasingObject eo;
        internal float from;
        internal float to;

        internal void <>m__249()
        {
            float val = (this.to - this.from) * this.eo.Now();
            this.<>f__this.SetContrast(val);
        }

        internal void <>m__24A()
        {
            this.<>f__this.SetContrast(this.to);
            this.<>f__this.StateQaaEnd();
        }
    }

    public enum STATE
    {
        NONE,
        MAP_MAIN,
        QAA_HIDE,
        QAA_GRAY,
        QAA_DISP,
        SIZEOF
    }

    private class StateMapMain : IState<srcLineSprite>
    {
        public void begin(srcLineSprite that)
        {
        }

        public void end(srcLineSprite that)
        {
        }

        public void update(srcLineSprite that)
        {
            if (that.mtIsUpdate)
            {
                if ((null != that.mcLineSprite) && (that.mcLineSprite.atlas != that.mcAtlasP))
                {
                    that.mcLineSprite.atlas = that.mcAtlasP;
                    that.mcLineSprite.spriteName = that.msSpriteName;
                    that.mcLineSprite.SetDimensions(that.miLineW, that.miLineH);
                }
                that.mtIsUpdate = false;
            }
        }
    }

    private class StateNone : IState<srcLineSprite>
    {
        public void begin(srcLineSprite that)
        {
        }

        public void end(srcLineSprite that)
        {
        }

        public void update(srcLineSprite that)
        {
        }
    }

    private class StateQaaDisp : IState<srcLineSprite>
    {
        public void begin(srcLineSprite that)
        {
            switch (that.mMapCtrl_SpotRoadInfo.mfGetDispType())
            {
                case clsMapCtrl_SpotRoadInfo.enDispType.None:
                    that.SetContrast(1f);
                    that.SetQaaScaleAnim(true);
                    break;

                case clsMapCtrl_SpotRoadInfo.enDispType.Normal:
                    that.StateQaaEnd();
                    break;

                case clsMapCtrl_SpotRoadInfo.enDispType.Glay:
                    that.SetQaaColorAnim(true);
                    break;
            }
        }

        public void end(srcLineSprite that)
        {
        }

        public void update(srcLineSprite that)
        {
        }
    }

    private class StateQaaGray : IState<srcLineSprite>
    {
        public void begin(srcLineSprite that)
        {
            switch (that.mMapCtrl_SpotRoadInfo.mfGetDispType())
            {
                case clsMapCtrl_SpotRoadInfo.enDispType.None:
                    that.SetContrast(0.5f);
                    that.SetQaaScaleAnim(true);
                    break;

                case clsMapCtrl_SpotRoadInfo.enDispType.Normal:
                    that.SetQaaColorAnim(false);
                    break;

                case clsMapCtrl_SpotRoadInfo.enDispType.Glay:
                    that.StateQaaEnd();
                    break;
            }
        }

        public void end(srcLineSprite that)
        {
        }

        public void update(srcLineSprite that)
        {
        }
    }

    private class StateQaaHide : IState<srcLineSprite>
    {
        public void begin(srcLineSprite that)
        {
            switch (that.mMapCtrl_SpotRoadInfo.mfGetDispType())
            {
                case clsMapCtrl_SpotRoadInfo.enDispType.None:
                    that.StateQaaEnd();
                    break;

                case clsMapCtrl_SpotRoadInfo.enDispType.Normal:
                case clsMapCtrl_SpotRoadInfo.enDispType.Glay:
                    that.SetQaaScaleAnim(false);
                    break;
            }
        }

        public void end(srcLineSprite that)
        {
        }

        public void update(srcLineSprite that)
        {
        }
    }
}

