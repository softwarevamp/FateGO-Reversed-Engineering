using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuestRewardBoxAction : MonoBehaviour
{
    private Animation mAnimation;
    [SerializeField]
    private UISprite mBoxBaseSp;
    [SerializeField]
    private UISprite mBoxLockSp;
    [SerializeField]
    private UISprite mBoxOpenSp;
    private System.Action mEndAct;
    private CStateManager<QuestRewardBoxAction> mFSM;
    [SerializeField]
    private UIAtlas mGoldAtlas;
    [SerializeField]
    private UIAtlas mNormalAtlas;
    [SerializeField]
    private UIAtlas mSilverAtlas;
    protected GameObject particleObj;
    [SerializeField]
    private GameObject particlePrefab;
    public const float WHITE_IN_TIME = 1f;
    public const float WHITE_OUT_TIME = 1f;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<QuestRewardBoxAction>(this, 2);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StatePlay());
            this.SetState(STATE.NONE);
        }
        if ((this.particleObj == null) && (this.particlePrefab != null))
        {
            GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.particlePrefab);
            if (self != null)
            {
                self.SafeSetParent(this);
                self.SetLocalPosition(Vector3.zero);
                self.GetComponentInChildren<UIUnityRenderer>().depth = 0;
                this.particleObj = self;
            }
        }
    }

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public void Play(System.Action end_act)
    {
        this.mEndAct = end_act;
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, () => this.SetState(STATE.PLAY));
    }

    public void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    public void Setup(BOX_TYPE box_type)
    {
        this.mAnimation = base.transform.GetComponentInChildren<Animation>();
        base.transform.GetComponentInChildren<CommonEffectComponent>().SetEndlessEnable(true);
        base.gameObject.SetActive(false);
        switch (box_type)
        {
            case BOX_TYPE.NORMAL:
                this.mBoxBaseSp.atlas = this.mNormalAtlas;
                this.mBoxBaseSp.spriteName = "box_n_base";
                this.mBoxLockSp.atlas = this.mNormalAtlas;
                this.mBoxLockSp.spriteName = "box_n_lock";
                this.mBoxOpenSp.atlas = this.mNormalAtlas;
                this.mBoxOpenSp.spriteName = "box_n_open";
                break;

            case BOX_TYPE.SILVER:
                this.mBoxBaseSp.atlas = this.mSilverAtlas;
                this.mBoxBaseSp.spriteName = "box_s_base";
                this.mBoxLockSp.atlas = this.mSilverAtlas;
                this.mBoxLockSp.spriteName = "box_s_lock";
                this.mBoxOpenSp.atlas = this.mSilverAtlas;
                this.mBoxOpenSp.spriteName = "box_s_open";
                break;

            case BOX_TYPE.GOLD:
                this.mBoxBaseSp.atlas = this.mGoldAtlas;
                this.mBoxBaseSp.spriteName = "box_g_base";
                this.mBoxLockSp.atlas = this.mGoldAtlas;
                this.mBoxLockSp.spriteName = "box_g_lock";
                this.mBoxOpenSp.atlas = this.mGoldAtlas;
                this.mBoxOpenSp.spriteName = "box_g_open";
                break;
        }
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    public enum BOX_TYPE
    {
        NORMAL,
        SILVER,
        GOLD,
        SIZEOF
    }

    public enum STATE
    {
        NONE,
        PLAY,
        SIZEOF
    }

    private class StateNone : IState<QuestRewardBoxAction>
    {
        public void begin(QuestRewardBoxAction that)
        {
        }

        public void end(QuestRewardBoxAction that)
        {
        }

        public void update(QuestRewardBoxAction that)
        {
        }
    }

    private class StatePlay : IState<QuestRewardBoxAction>
    {
        public void begin(QuestRewardBoxAction that)
        {
            that.gameObject.SetActive(true);
            that.mAnimation.Play(that.mAnimation.clip.name);
        }

        public void end(QuestRewardBoxAction that)
        {
        }

        public void update(QuestRewardBoxAction that)
        {
            <update>c__AnonStoreyC0 yc = new <update>c__AnonStoreyC0 {
                that = that
            };
            if (!yc.that.mAnimation.IsPlaying(yc.that.mAnimation.clip.name))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.WHITE, 1f, new System.Action(yc.<>m__1C3));
                yc.that.SetState(QuestRewardBoxAction.STATE.NONE);
            }
        }

        [CompilerGenerated]
        private sealed class <update>c__AnonStoreyC0
        {
            internal QuestRewardBoxAction that;

            internal void <>m__1C3()
            {
                this.that.mEndAct.Call();
            }
        }
    }
}

