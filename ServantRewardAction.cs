using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantRewardAction : MonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache15;
    private Animation mAnimation;
    [SerializeField]
    private AnimationClip mAnimNormal;
    [SerializeField]
    private AnimationClip mAnimSvtEquipNew;
    [SerializeField]
    private AnimationClip mAnimSvtNew;
    [SerializeField]
    private GameObject mCardParent;
    private System.Action mEndAct;
    private float mFadeInTime;
    private CStateManager<ServantRewardAction> mFSM;
    private bool mIsDoneLoad;
    private bool mIsFromTreasureBox;
    private bool mIsPlayReq;
    private int mLimitCount;
    private PLAY_FLAG mPlayFlag;
    private ScreenTouchInformationComponent mScreenTouchInfo;
    [SerializeField]
    private GameObject mScreenTouchInfoPrefab;
    private int mServantId;
    private SvtType.Type mSvtType;
    private UICharaGraphTexture mUICharaGraph;
    private long mUserSvtId;
    protected GameObject particleObj;
    [SerializeField]
    private GameObject particlePrefab;
    public const float SCREEN_TOUCH_INFO_Y = -265f;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<ServantRewardAction>(this, 6);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StatePlay());
            this.mFSM.add(2, new StateTouchWait());
            this.mFSM.add(3, new StateTalk());
            this.mFSM.add(4, new StateDetail());
            this.mFSM.add(5, new StateEnd());
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
        CTouch.init();
    }

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public void Play(System.Action end_act, float fade_in_time = 0)
    {
        this.Play(false, end_act, fade_in_time);
    }

    public void Play(bool is_from_treasure_box, System.Action end_act, float fade_in_time = 0)
    {
        this.mIsFromTreasureBox = is_from_treasure_box;
        this.mIsPlayReq = true;
        this.mFadeInTime = fade_in_time;
        this.mEndAct = end_act;
    }

    public void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    public void Setup(QuestRewardInfo qri, PLAY_FLAG play_flag = 0)
    {
        this.Setup(qri.objectId, qri.userSvtId, qri.limitCount, qri.num, qri.isNew, play_flag);
    }

    public void Setup(int servant_id, long user_svt_id, int limit_count, int svt_num, PLAY_FLAG play_flag = 0)
    {
        if ((this.GetState() == STATE.NONE) || (this.GetState() == STATE.END))
        {
            if (this.mUICharaGraph != null)
            {
                UnityEngine.Object.Destroy(this.mUICharaGraph.gameObject);
                this.mUICharaGraph = null;
            }
            this.mServantId = servant_id;
            this.mUserSvtId = user_svt_id;
            this.mLimitCount = limit_count;
            this.mPlayFlag = play_flag;
            this.mAnimation = base.transform.GetComponentInChildren<Animation>();
            base.transform.GetComponentInChildren<CommonEffectComponent>().SetEndlessEnable(true);
            base.gameObject.SetActive(false);
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.mServantId);
            this.mSvtType = (SvtType.Type) entity.type;
            this.mIsDoneLoad = false;
            if (this.mScreenTouchInfo == null)
            {
                GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mScreenTouchInfoPrefab);
                self.SafeSetParent(this);
                self.SetLocalPositionY(-265f);
                this.mScreenTouchInfo = self.GetComponent<ScreenTouchInformationComponent>();
            }
            this.mScreenTouchInfo.gameObject.SetActive(false);
            this.mUICharaGraph = CharaGraphManager.CreateTexturePrefab(this.mCardParent, this.mUserSvtId, true, 0, delegate {
                this.mIsDoneLoad = true;
                base.gameObject.SetActive(true);
                base.gameObject.SetLocalScale(Vector3.zero);
                this.mAnimation.Stop();
            });
        }
    }

    public void Setup(int servant_id, long user_svt_id, int limit_count, int svt_num, bool is_svt_new, PLAY_FLAG play_flag = 0)
    {
        if (is_svt_new)
        {
            play_flag |= PLAY_FLAG.SVT_NEW;
        }
        this.Setup(servant_id, user_svt_id, limit_count, svt_num, play_flag);
    }

    private void Update()
    {
        CTouch.process();
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
        if (this.mIsPlayReq && this.mIsDoneLoad)
        {
            this.mIsPlayReq = false;
            if ((this.mPlayFlag & PLAY_FLAG.FADE_IN) != PLAY_FLAG.NONE)
            {
                if (<>f__am$cache15 == null)
                {
                    <>f__am$cache15 = delegate {
                    };
                }
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein((this.mFadeInTime <= 0f) ? SceneManager.DEFAULT_FADE_TIME : this.mFadeInTime, <>f__am$cache15);
            }
            this.SetState(STATE.PLAY);
        }
    }

    public enum PLAY_FLAG
    {
        EVENT_SVT_GET = 8,
        FADE_IN = 2,
        FADE_OUT = 4,
        NONE = 0,
        SVT_NEW = 1
    }

    public enum STATE
    {
        NONE,
        PLAY,
        TOUCH_WAIT,
        TALK,
        DETAIL,
        END,
        SIZEOF
    }

    private class StateDetail : IState<ServantRewardAction>
    {
        public void begin(ServantRewardAction that)
        {
            <begin>c__AnonStoreyD3 yd = new <begin>c__AnonStoreyD3 {
                that = that
            };
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, new System.Action(yd.<>m__205));
        }

        public void end(ServantRewardAction that)
        {
        }

        public void update(ServantRewardAction that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyD3
        {
            private static System.Action <>f__am$cache1;
            internal ServantRewardAction that;

            internal void <>m__205()
            {
                if (<>f__am$cache1 == null)
                {
                    <>f__am$cache1 = delegate {
                    };
                }
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, <>f__am$cache1);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.FIRST_SINGLE_GET, this.that.mUserSvtId, (ServantStatusDialog.ClickDelegate) (isDecide => SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(null);
                    this.that.SetState(ServantRewardAction.STATE.END);
                })));
            }

            private static void <>m__206()
            {
            }

            internal void <>m__207(bool isDecide)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(null);
                    this.that.SetState(ServantRewardAction.STATE.END);
                });
            }

            internal void <>m__208()
            {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(null);
                this.that.SetState(ServantRewardAction.STATE.END);
            }
        }
    }

    private class StateEnd : IState<ServantRewardAction>
    {
        public void begin(ServantRewardAction that)
        {
            <begin>c__AnonStoreyD4 yd = new <begin>c__AnonStoreyD4 {
                that = that
            };
            if ((yd.that.mPlayFlag & ServantRewardAction.PLAY_FLAG.FADE_OUT) != ServantRewardAction.PLAY_FLAG.NONE)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, new System.Action(yd.<>m__209));
            }
            else
            {
                yd.that.mEndAct.Call();
            }
        }

        public void end(ServantRewardAction that)
        {
            SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
        }

        public void update(ServantRewardAction that)
        {
            that.SetState(ServantRewardAction.STATE.NONE);
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyD4
        {
            internal ServantRewardAction that;

            internal void <>m__209()
            {
                this.that.mEndAct.Call();
            }
        }
    }

    private class StateNone : IState<ServantRewardAction>
    {
        public void begin(ServantRewardAction that)
        {
        }

        public void end(ServantRewardAction that)
        {
        }

        public void update(ServantRewardAction that)
        {
        }
    }

    private class StatePlay : IState<ServantRewardAction>
    {
        private AnimationState mAnimState;

        public void begin(ServantRewardAction that)
        {
            SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
            that.gameObject.SetLocalScale(Vector3.one);
            AnimationClip mAnimNormal = that.mAnimNormal;
            if ((that.mPlayFlag & ServantRewardAction.PLAY_FLAG.SVT_NEW) != ServantRewardAction.PLAY_FLAG.NONE)
            {
                if (that.mSvtType == SvtType.Type.NORMAL)
                {
                    mAnimNormal = that.mAnimSvtNew;
                }
                else if (that.mSvtType == SvtType.Type.SERVANT_EQUIP)
                {
                    mAnimNormal = that.mAnimSvtEquipNew;
                }
            }
            that.mAnimation.clip = mAnimNormal;
            that.mAnimation.Play(that.mAnimation.clip.name);
            this.mAnimState = that.mAnimation[that.mAnimation.clip.name];
        }

        public void end(ServantRewardAction that)
        {
        }

        public void update(ServantRewardAction that)
        {
            if (CTouch.isTouchPush())
            {
                this.mAnimState.normalizedTime = 1f;
            }
            if (!that.mAnimation.IsPlaying(that.mAnimation.clip.name))
            {
                that.SetState(ServantRewardAction.STATE.TOUCH_WAIT);
            }
        }
    }

    private class StateTalk : IState<ServantRewardAction>
    {
        public void begin(ServantRewardAction that)
        {
            <begin>c__AnonStoreyD2 yd = new <begin>c__AnonStoreyD2 {
                that = that
            };
            int mServantId = yd.that.mServantId;
            bool flag = (yd.that.mPlayFlag & ServantRewardAction.PLAY_FLAG.EVENT_SVT_GET) != ServantRewardAction.PLAY_FLAG.NONE;
            SingletonTemplate<clsQuestCheck>.Instance.mfInit();
            SingletonTemplate<clsQuestCheck>.Instance.PlayGacha(yd.that.mUserSvtId, mServantId, yd.that.mLimitCount, flag, new System.Action(yd.<>m__204));
        }

        public void end(ServantRewardAction that)
        {
        }

        public void update(ServantRewardAction that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyD2
        {
            internal ServantRewardAction that;

            internal void <>m__204()
            {
                this.that.SetState(ServantRewardAction.STATE.DETAIL);
            }
        }
    }

    private class StateTouchWait : IState<ServantRewardAction>
    {
        public void begin(ServantRewardAction that)
        {
            that.mScreenTouchInfo.gameObject.SetActive(true);
        }

        public void end(ServantRewardAction that)
        {
        }

        public void update(ServantRewardAction that)
        {
            if (CTouch.isTouchPush())
            {
                that.mScreenTouchInfo.gameObject.SetActive(false);
                if (that.mSvtType == SvtType.Type.NORMAL)
                {
                    if ((that.mPlayFlag & ServantRewardAction.PLAY_FLAG.SVT_NEW) != ServantRewardAction.PLAY_FLAG.NONE)
                    {
                        that.SetState(ServantRewardAction.STATE.TALK);
                    }
                    else
                    {
                        that.SetState(ServantRewardAction.STATE.DETAIL);
                    }
                }
                else if (that.mSvtType == SvtType.Type.SERVANT_EQUIP)
                {
                    that.SetState(ServantRewardAction.STATE.DETAIL);
                }
                else
                {
                    that.SetState(ServantRewardAction.STATE.END);
                }
            }
        }
    }
}

