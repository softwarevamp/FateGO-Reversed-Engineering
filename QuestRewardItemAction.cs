using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class QuestRewardItemAction : MonoBehaviour
{
    private Animation mAnimation;
    [SerializeField]
    private GameObject mAppearEffObj;
    private System.Action mEndAct;
    private CStateManager<QuestRewardItemAction> mFSM;
    private bool mIsFromTreasureBox;
    [SerializeField]
    private UILabel mItemLabel;
    [SerializeField]
    private UISprite mItemSp;
    private ScreenTouchInformationComponent mScreenTouchInfo;
    [SerializeField]
    private GameObject mScreenTouchInfoPrefab;
    protected GameObject particleObj;
    [SerializeField]
    private GameObject particlePrefab;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<QuestRewardItemAction>(this, 4);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StatePlay());
            this.mFSM.add(2, new StateItemLabel());
            this.mFSM.add(3, new StateTouchWait());
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

    public void Play(bool is_from_treasure_box, System.Action end_act, float fade_in_time = 0)
    {
        this.mIsFromTreasureBox = is_from_treasure_box;
        this.mEndAct = end_act;
        if (this.mIsFromTreasureBox)
        {
            this.SetState(STATE.PLAY);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein((fade_in_time <= 0f) ? SceneManager.DEFAULT_FADE_TIME : fade_in_time, delegate {
            if (!this.mIsFromTreasureBox)
            {
                this.SetState(STATE.PLAY);
            }
        });
    }

    public void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    public void Setup(QuestRewardInfo qri)
    {
        this.mAnimation = base.transform.GetComponentInChildren<Animation>();
        base.transform.GetComponentInChildren<CommonEffectComponent>().SetEndlessEnable(true);
        this.mItemSp.gameObject.SafeGetComponent<ItemIconComponent>().SetGift((Gift.Type) qri.type, qri.objectId, qri.num);
        string name = string.Empty;
        if (qri.type == 2)
        {
            name = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(qri.objectId).name;
        }
        else
        {
            name = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(qri.objectId).name;
        }
        string str2 = string.Format(LocalizationManager.Get("QUEST_CLEAR_REWARD_GET"), name, qri.num);
        this.mItemLabel.text = str2;
        base.gameObject.SetActive(false);
        this.mItemLabel.gameObject.SetActive(false);
        if (this.mScreenTouchInfo == null)
        {
            GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mScreenTouchInfoPrefab);
            self.SafeSetParent(this);
            this.mScreenTouchInfo = self.GetComponent<ScreenTouchInformationComponent>();
        }
        this.mScreenTouchInfo.gameObject.SetActive(false);
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
        PLAY,
        ITEM_LABEL,
        TOUCH_WAIT,
        SIZEOF
    }

    private class StateItemLabel : IState<QuestRewardItemAction>
    {
        private const float MV_TIME = 0.25f;

        public void begin(QuestRewardItemAction that)
        {
            <begin>c__AnonStoreyC1 yc = new <begin>c__AnonStoreyC1 {
                that = that
            };
            yc.tgt_obj = yc.that.mItemLabel.gameObject;
            yc.tgt_obj.SetActive(true);
            yc.mo = yc.tgt_obj.SafeGetComponent<MoveObject>();
            Vector3 localPosition = yc.tgt_obj.GetLocalPosition();
            localPosition.x = ManagerConfig.WIDTH;
            Vector3 to = yc.tgt_obj.GetLocalPosition();
            yc.mo.Play(localPosition, to, 0.25f, new System.Action(yc.<>m__1C5), new System.Action(yc.<>m__1C6), 0f, Easing.TYPE.EXPONENTIAL_OUT);
            SoundManager.playSystemSe(SeManager.SystemSeKind.GET_ITEM);
        }

        public void end(QuestRewardItemAction that)
        {
        }

        public void update(QuestRewardItemAction that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyC1
        {
            internal MoveObject mo;
            internal GameObject tgt_obj;
            internal QuestRewardItemAction that;

            internal void <>m__1C5()
            {
                this.tgt_obj.SetLocalPosition(this.mo.Now());
            }

            internal void <>m__1C6()
            {
                this.that.SetState(QuestRewardItemAction.STATE.TOUCH_WAIT);
            }
        }
    }

    private class StateNone : IState<QuestRewardItemAction>
    {
        public void begin(QuestRewardItemAction that)
        {
        }

        public void end(QuestRewardItemAction that)
        {
        }

        public void update(QuestRewardItemAction that)
        {
        }
    }

    private class StatePlay : IState<QuestRewardItemAction>
    {
        private AnimationState mAnimState;

        public void begin(QuestRewardItemAction that)
        {
            that.gameObject.SetActive(true);
            that.mAnimation.Play(that.mAnimation.clip.name);
            this.mAnimState = that.mAnimation[that.mAnimation.clip.name];
            this.mAnimState.normalizedTime = !that.mIsFromTreasureBox ? ((float) 0) : ((float) 1);
            that.mAppearEffObj.SetActive(!that.mIsFromTreasureBox);
        }

        public void end(QuestRewardItemAction that)
        {
        }

        public void update(QuestRewardItemAction that)
        {
            if (!that.mAnimation.IsPlaying(that.mAnimation.clip.name) && !SingletonMonoBehaviour<CommonUI>.Instance.maskFadeIsBusy())
            {
                that.SetState(QuestRewardItemAction.STATE.ITEM_LABEL);
            }
        }
    }

    private class StateTouchWait : IState<QuestRewardItemAction>
    {
        public void begin(QuestRewardItemAction that)
        {
            that.mScreenTouchInfo.gameObject.SetActive(true);
        }

        public void end(QuestRewardItemAction that)
        {
        }

        public void update(QuestRewardItemAction that)
        {
            <update>c__AnonStoreyC2 yc = new <update>c__AnonStoreyC2 {
                that = that
            };
            if (CTouch.isTouchPush())
            {
                yc.that.mScreenTouchInfo.gameObject.SetActive(false);
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, new System.Action(yc.<>m__1C7));
                yc.that.SetState(QuestRewardItemAction.STATE.NONE);
            }
        }

        [CompilerGenerated]
        private sealed class <update>c__AnonStoreyC2
        {
            internal QuestRewardItemAction that;

            internal void <>m__1C7()
            {
                this.that.mEndAct.Call();
            }
        }
    }
}

