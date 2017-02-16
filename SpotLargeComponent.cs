using System;
using UnityEngine;

public class SpotLargeComponent : MonoBehaviour
{
    private const float LARGE_IN_TIME = 0.25f;
    private const float LARGE_OUT_TIME = 0.25f;
    private const float LARGE_POS_X = -230f;
    private const float LARGE_POS_Y = -85f;
    private const float LARGE_SCL = 2f;
    private const UITweener.Method LARGE_TWEEN_METHOD = UITweener.Method.EaseOut;
    private SrcSpotBasePrefab mBaseSpot;
    private CStateManager<SpotLargeComponent> mFSM;
    private System.Action mHideEndAct;
    private MapCamera mMapCamera;
    private System.Action mShowEndAct;
    [SerializeField]
    private UILabel mSpotNameLabel;
    [SerializeField]
    private UISprite mSpotNameSp;
    [SerializeField]
    private UISprite mSpotSp;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<SpotLargeComponent>(this, 4);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StateLargeIn());
            this.mFSM.add(2, new StateLargeMain());
            this.mFSM.add(3, new StateLargeOut());
            this.SetState(STATE.NONE);
        }
    }

    private Vector3 GetBasePosition()
    {
        if (this.mBaseSpot == null)
        {
            return Vector3.zero;
        }
        if (this.mMapCamera == null)
        {
            return Vector3.zero;
        }
        Vector3 vector = this.mBaseSpot.gameObject.GetLocalPosition() - this.mMapCamera.Scrl.GetScrlPos();
        return (Vector3) (vector * this.GetBaseScale());
    }

    private float GetBaseScale()
    {
        if (this.mMapCamera == null)
        {
            return 0f;
        }
        return (1f / this.mMapCamera.Zoom.GetZoomSize());
    }

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public void LargeIn(SrcSpotBasePrefab spot, MapCamera map_cam, System.Action end_act)
    {
        this.mBaseSpot = spot;
        this.mMapCamera = map_cam;
        SrcSpotBasePrefab.SetSpotUI(this.mBaseSpot.mcAtlasP, this.mSpotSp, this.mBaseSpot.mfGetSpotID());
        SrcSpotBasePrefab.SetSpotNameUI(this.mBaseSpot.mcAtlasP, this.mSpotNameSp, this.mSpotNameLabel, this.mBaseSpot.mfGetSpotName());
        float baseScale = this.GetBaseScale();
        base.gameObject.SetLocalScale(baseScale);
        Vector3 basePosition = this.GetBasePosition();
        base.gameObject.SetLocalPosition(basePosition);
        this.mShowEndAct = end_act;
        this.SetState(STATE.LARGE_IN);
    }

    public void LargeOut(System.Action end_act)
    {
        this.mHideEndAct = end_act;
        this.SetState(STATE.LARGE_OUT);
    }

    private void SetLargeSpotNameScale_NormalScale()
    {
        Vector3 localScale = base.gameObject.GetLocalScale();
        this.mSpotNameSp.gameObject.SetLocalScale(1f / localScale.x, 1f / localScale.y);
    }

    private void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    private void StateLargeIn_End()
    {
        this.mShowEndAct.Call();
        this.mShowEndAct = null;
        this.SetState(STATE.LARGE_MAIN);
    }

    private void StateLargeOut_End()
    {
        this.mHideEndAct.Call();
        this.mHideEndAct = null;
        this.SetState(STATE.NONE);
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    public enum STATE
    {
        NONE,
        LARGE_IN,
        LARGE_MAIN,
        LARGE_OUT,
        SIZEOF
    }

    private class StateLargeIn : IState<SpotLargeComponent>
    {
        private GameObject mNameBgObj;

        public void begin(SpotLargeComponent that)
        {
            float duration = 0.25f;
            TweenPosition position = UITweener.Begin<TweenPosition>(that.gameObject, duration);
            position.from = that.gameObject.GetLocalPosition();
            position.to = new Vector3(-230f, -85f, 0f);
            position.method = UITweener.Method.EaseOut;
            TweenScale scale = UITweener.Begin<TweenScale>(that.gameObject, duration);
            scale.from = that.gameObject.GetLocalScale();
            scale.to = new Vector3(2f, 2f, 1f);
            scale.method = UITweener.Method.EaseOut;
            TweenAlpha alpha = UITweener.Begin<TweenAlpha>(that.gameObject, duration);
            alpha.from = 0f;
            alpha.to = 1f;
            alpha.method = UITweener.Method.EaseOut;
            alpha.eventReceiver = that.gameObject;
            alpha.callWhenFinished = "StateLargeIn_End";
        }

        public void end(SpotLargeComponent that)
        {
        }

        public void update(SpotLargeComponent that)
        {
            that.SetLargeSpotNameScale_NormalScale();
        }
    }

    private class StateLargeMain : IState<SpotLargeComponent>
    {
        public void begin(SpotLargeComponent that)
        {
            that.SetLargeSpotNameScale_NormalScale();
            that.gameObject.GetParent().GetComponent<UIPanel>().Invalidate(true);
            that.gameObject.SetActive(false);
            that.gameObject.SetActive(true);
            UIAtlas atlas = TerminalSceneComponent.Instance.TerminalMap.GetAtlas();
            that.mSpotSp.atlas = atlas;
            that.mSpotNameSp.atlas = atlas;
        }

        public void end(SpotLargeComponent that)
        {
        }

        public void update(SpotLargeComponent that)
        {
        }
    }

    private class StateLargeOut : IState<SpotLargeComponent>
    {
        private GameObject mNameBgObj;

        public void begin(SpotLargeComponent that)
        {
            float duration = 0.25f;
            TweenPosition position = UITweener.Begin<TweenPosition>(that.gameObject, duration);
            position.from = that.gameObject.GetLocalPosition();
            position.to = that.GetBasePosition();
            position.method = UITweener.Method.EaseOut;
            TweenScale scale = UITweener.Begin<TweenScale>(that.gameObject, duration);
            scale.from = that.gameObject.GetLocalScale();
            float baseScale = that.GetBaseScale();
            scale.to = new Vector3(baseScale, baseScale, 1f);
            scale.method = UITweener.Method.EaseOut;
            TweenAlpha alpha = UITweener.Begin<TweenAlpha>(that.gameObject, duration);
            alpha.from = 1f;
            alpha.to = 0f;
            alpha.method = UITweener.Method.EaseOut;
            alpha.eventReceiver = that.gameObject;
            alpha.callWhenFinished = "StateLargeOut_End";
        }

        public void end(SpotLargeComponent that)
        {
        }

        public void update(SpotLargeComponent that)
        {
            that.SetLargeSpotNameScale_NormalScale();
        }
    }

    private class StateNone : IState<SpotLargeComponent>
    {
        public void begin(SpotLargeComponent that)
        {
        }

        public void end(SpotLargeComponent that)
        {
        }

        public void update(SpotLargeComponent that)
        {
        }
    }
}

