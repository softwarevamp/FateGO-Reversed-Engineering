using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class SrcSpotBasePrefab : MonoBehaviour
{
    public const float GRAY = 0.5f;
    public UIAtlas mcAtlasP;
    private GameObject mcRootGobjP;
    private UISprite mcSpotNameSp;
    public UISprite mcSpotNextSp;
    public UISprite mcSpotSprite;
    private CStateManager<SrcSpotBasePrefab> mFSM;
    public int miQuestCount;
    public int miSpotID;
    private clsMapCtrl_SpotInfo mMapCtrl_SpotInfo;
    private NoticeNumberComponent mNoticeNumber;
    [SerializeField]
    private GameObject mNoticeNumberPrefab;
    private GameObject mParticleSystemObj;
    [SerializeField]
    private GameObject mQaaDispEffObj;
    public UILabel mSpotNameLabel;
    private int mSpotNameOfsX;
    private int mSpotNameOfsY;
    private string mSpotNameStr;
    private string msSmfNameForBtnClick = "smfSpotBtn_Click";
    private System.Action mStateEndAct;
    private bool mtIsUpdate;
    public const float OFS_Y = -32f;
    private const int SPOT_NAME_BG_W_MIN = 40;
    private const int SPOT_NAME_BG_W_OFS = 50;
    public const string SPOT_PLAYER_NAME = "Spot_Player";
    public const float WHITE = 1f;

    private void Awake()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<SrcSpotBasePrefab>(this, 5);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StateMapMain());
            this.mFSM.add(2, new StateQaaHide());
            this.mFSM.add(3, new StateQaaGray());
            this.mFSM.add(4, new StateQaaDisp());
            this.SetState(STATE.MAP_MAIN, null);
        }
    }

    public void cbfBtn_Click()
    {
        Debug.Log(string.Concat(new object[] { "Spot:Input.touchCount=", Input.touchCount, " : frm=", Time.frameCount }));
        if (this.mcRootGobjP != null)
        {
            this.mcRootGobjP.SendMessage(this.msSmfNameForBtnClick, base.gameObject);
            TerminalPramsManager.PlaySE_Decide();
            TerminalPramsManager.SpotId = this.miSpotID;
        }
    }

    private void DestroyEffect()
    {
        DestroyEffect(this);
    }

    private static void DestroyEffect(SrcSpotBasePrefab spot)
    {
        if (spot.mParticleSystemObj != null)
        {
            UnityEngine.Object.Destroy(spot.mParticleSystemObj);
            spot.mParticleSystemObj = null;
        }
    }

    public float GetContrast() => 
        this.mcSpotSprite.color.r;

    public static string GetGobjName(int id) => 
        ("Spot_" + id.ToString("00"));

    public clsMapCtrl_SpotInfo GetMapCtrl_SpotInfo() => 
        this.mMapCtrl_SpotInfo;

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public int mfGetSpotID() => 
        this.miSpotID;

    public string mfGetSpotName() => 
        this.mSpotNameStr;

    public void mfSetAtlas(UIAtlas cAtlasP)
    {
        this.mcAtlasP = cAtlasP;
        this.mtIsUpdate = true;
    }

    public void mfSetCommopn(GameObject cRootGobjP)
    {
        this.mcRootGobjP = cRootGobjP;
    }

    public void mfSetQuestCount(int iQuestCount)
    {
        this.miQuestCount = iQuestCount;
        this.mtIsUpdate = true;
    }

    public void mfSetSpotID(int iSpotID)
    {
        this.miSpotID = iSpotID;
        this.mtIsUpdate = true;
    }

    public void mfSetSpotName(string name, int ofs_x, int ofs_y)
    {
        this.mSpotNameStr = name;
        this.mSpotNameOfsX = ofs_x;
        this.mSpotNameOfsY = ofs_y;
        this.mtIsUpdate = true;
    }

    private void OnDisable()
    {
        this.DestroyEffect();
    }

    public void SetBtnColliderEnable(bool is_enable)
    {
        base.gameObject.GetComponent<BoxCollider>().enabled = is_enable;
    }

    public void SetContrast(float val)
    {
        Color color = new Color(val, val, val, 1f);
        this.mcSpotSprite.color = color;
        this.SpotNameSp.color = color;
        this.mSpotNameLabel.color = color;
    }

    public void SetMapCtrl_SpotInfo(clsMapCtrl_SpotInfo _MapCtrl_SpotInfo)
    {
        this.mMapCtrl_SpotInfo = _MapCtrl_SpotInfo;
    }

    private void SetQaaColorAnim(bool is_disp)
    {
        <SetQaaColorAnim>c__AnonStoreyD5 yd = new <SetQaaColorAnim>c__AnonStoreyD5 {
            <>f__this = this
        };
        float sec = 0.4f;
        yd.eo = base.gameObject.SafeGetComponent<EasingObject>();
        yd.from = this.GetContrast();
        yd.to = !is_disp ? 0.5f : 1f;
        yd.eo.Play(sec, new System.Action(yd.<>m__20A), new System.Action(yd.<>m__20B), 0f, Easing.TYPE.EXPONENTIAL_OUT);
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
    }

    public static void SetSpotNameUI(UIAtlas atlas, UISprite sp, UILabel lb, string spot_name)
    {
        lb.text = spot_name;
        sp.atlas = atlas;
        sp.spriteName = "img_spotname_bg";
        int num = lb.width + 50;
        if (num < 40)
        {
            num = 40;
        }
        sp.width = num;
    }

    public static void SetSpotUI(UIAtlas atlas, UISprite sp, int spot_id)
    {
        SpotEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SpotMaster>(DataNameKind.Kind.SPOT).getEntityFromId<SpotEntity>(spot_id);
        sp.atlas = atlas;
        sp.spriteName = "spot_" + entity.imageId.ToString("000000");
        sp.enabled = true;
        GameObject gameObject = sp.gameObject;
        gameObject.SetLocalPositionX((float) entity.imageOfsX);
        gameObject.SetLocalPositionY((float) -entity.imageOfsY);
    }

    public void SetState(STATE state, System.Action end_act = null)
    {
        this.mStateEndAct = end_act;
        this.mFSM.setState((int) state);
    }

    public void SetTouchType()
    {
        base.gameObject.GetComponent<UIButton>().enabled = this.mMapCtrl_SpotInfo.mfGetTouchType() == clsMapCtrl_SpotInfo.enTouchType.Enable;
    }

    public void SetTouchType(clsMapCtrl_SpotInfo.enTouchType type)
    {
        this.mMapCtrl_SpotInfo.mfSetTouchType(type);
        this.SetTouchType();
    }

    public void SetUISacleSameCamera(Camera cam)
    {
        this.mSpotNameLabel.transform.parent.gameObject.SafeGetComponent<UIScaleSame>().SetCamera(cam);
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

    private void UpdateDisp(bool is_force = false)
    {
        if (this.mtIsUpdate || is_force)
        {
            if (null != this.mcSpotSprite)
            {
                SetSpotUI(this.mcAtlasP, this.mcSpotSprite, this.miSpotID);
            }
            if (null != this.mSpotNameLabel)
            {
                GameObject gameObject = this.mSpotNameLabel.transform.parent.parent.gameObject;
                gameObject.SetActive(this.mMapCtrl_SpotInfo.mfGetDispType() == clsMapCtrl_SpotInfo.enDispType.Normal);
                Vector3 localPosition = gameObject.transform.localPosition;
                localPosition.x = this.mSpotNameOfsX;
                localPosition.y = this.mSpotNameOfsY;
                gameObject.transform.localPosition = localPosition;
                SetSpotNameUI(this.mcAtlasP, this.SpotNameSp, this.mSpotNameLabel, this.mSpotNameStr);
            }
            if (this.GetState() == STATE.MAP_MAIN)
            {
                if (this.mNoticeNumber == null)
                {
                    GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.mNoticeNumberPrefab);
                    obj3.SafeSetParent(this);
                    this.mNoticeNumber = obj3.GetComponent<NoticeNumberComponent>();
                }
                this.mNoticeNumber.SetNumber(this.miQuestCount);
                GameObject self = this.mNoticeNumber.gameObject;
                if (this.mMapCtrl_SpotInfo.mfGetDispType() != clsMapCtrl_SpotInfo.enDispType.Normal)
                {
                    self.SetActive(false);
                }
                SpotEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SpotMaster>(DataNameKind.Kind.SPOT).getEntityFromId<SpotEntity>(this.miSpotID);
                self.SetLocalPositionX((float) entity.questOfsX);
                self.SetLocalPositionY((float) -entity.questOfsY);
            }
            bool flag = this.mMapCtrl_SpotInfo.IsNextDisp();
            this.mcSpotNextSp.transform.parent.gameObject.SetActive(flag);
            this.mtIsUpdate = false;
        }
    }

    private UISprite SpotNameSp
    {
        get
        {
            if (this.mcSpotNameSp == null)
            {
                this.mcSpotNameSp = this.mSpotNameLabel.transform.parent.gameObject.GetComponent<UISprite>();
            }
            return this.mcSpotNameSp;
        }
    }

    [CompilerGenerated]
    private sealed class <SetQaaColorAnim>c__AnonStoreyD5
    {
        internal SrcSpotBasePrefab <>f__this;
        internal EasingObject eo;
        internal float from;
        internal float to;

        internal void <>m__20A()
        {
            float num = (this.to - this.from) * this.eo.Now();
            float val = this.from + num;
            this.<>f__this.SetContrast(val);
        }

        internal void <>m__20B()
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

    private class StateMapMain : IState<SrcSpotBasePrefab>
    {
        public void begin(SrcSpotBasePrefab that)
        {
            that.mtIsUpdate = true;
        }

        public void end(SrcSpotBasePrefab that)
        {
        }

        public void update(SrcSpotBasePrefab that)
        {
            that.UpdateDisp(false);
        }
    }

    private class StateNone : IState<SrcSpotBasePrefab>
    {
        public void begin(SrcSpotBasePrefab that)
        {
        }

        public void end(SrcSpotBasePrefab that)
        {
        }

        public void update(SrcSpotBasePrefab that)
        {
        }
    }

    private class StateQaaDisp : IState<SrcSpotBasePrefab>
    {
        public void begin(SrcSpotBasePrefab that)
        {
            SoundManager.playSe("ar2");
            switch (that.mMapCtrl_SpotInfo.mfGetDispType())
            {
                case clsMapCtrl_SpotInfo.enDispType.None:
                    that.SetContrast(1f);
                    that.SetQaaScaleAnim(true);
                    break;

                case clsMapCtrl_SpotInfo.enDispType.Normal:
                    that.StateQaaEnd();
                    break;

                case clsMapCtrl_SpotInfo.enDispType.Glay:
                    that.SetQaaColorAnim(true);
                    break;
            }
            that.DestroyEffect();
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(that.mQaaDispEffObj);
            obj2.transform.parent = that.gameObject.transform;
            obj2.transform.localPosition = new Vector3(0f, Mathf.Abs((float) -32f), 0f);
            obj2.transform.localRotation = Quaternion.identity;
            obj2.transform.localScale = Vector3.one;
            that.mParticleSystemObj = obj2;
        }

        public void end(SrcSpotBasePrefab that)
        {
        }

        public void update(SrcSpotBasePrefab that)
        {
        }
    }

    private class StateQaaGray : IState<SrcSpotBasePrefab>
    {
        public void begin(SrcSpotBasePrefab that)
        {
            switch (that.mMapCtrl_SpotInfo.mfGetDispType())
            {
                case clsMapCtrl_SpotInfo.enDispType.None:
                    that.SetContrast(0.5f);
                    that.SetQaaScaleAnim(true);
                    break;

                case clsMapCtrl_SpotInfo.enDispType.Normal:
                    that.SetQaaColorAnim(false);
                    break;

                case clsMapCtrl_SpotInfo.enDispType.Glay:
                    that.StateQaaEnd();
                    break;
            }
        }

        public void end(SrcSpotBasePrefab that)
        {
        }

        public void update(SrcSpotBasePrefab that)
        {
        }
    }

    private class StateQaaHide : IState<SrcSpotBasePrefab>
    {
        public void begin(SrcSpotBasePrefab that)
        {
            switch (that.mMapCtrl_SpotInfo.mfGetDispType())
            {
                case clsMapCtrl_SpotInfo.enDispType.None:
                    that.StateQaaEnd();
                    break;

                case clsMapCtrl_SpotInfo.enDispType.Normal:
                case clsMapCtrl_SpotInfo.enDispType.Glay:
                    that.SetQaaScaleAnim(false);
                    break;
            }
        }

        public void end(SrcSpotBasePrefab that)
        {
        }

        public void update(SrcSpotBasePrefab that)
        {
        }
    }
}

