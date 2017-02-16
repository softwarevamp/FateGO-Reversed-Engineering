using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScrPlayerStatus : MonoBehaviour
{
    public static readonly string AP_OVER_TXT_COLOR_STR = "[ffb505]";
    public CommandSpellIconComponent commandSpellComp;
    private int CommandSpellRest;
    public MstExpInfoComponent expInfoWindow;
    public static readonly float FRAME_OUT_POS_X = -ManagerConfig.WIDTH;
    private System.Action mApRecoverAct;
    [SerializeField]
    private GameObject mApRecoverBtn;
    private BoxCollider mApRecoverBtnColl;
    private UISprite mApRecoverBtnSp;
    public UISlider mcApSldP;
    public UISlider mcApSldP2;
    public UILabel mcApStrP;
    public UILabel mcApTimeP;
    public UISlider mcExpSldP;
    public GameObject mcFaceObjP;
    public GameObject mcGiftBaseP;
    public UIButton mcGiftButtonP;
    public UILabel mcLevelStrP;
    private System.Action mCloseGiftAct;
    private UIMasterFaceTexture mcMasterCurTexP;
    private Gender.Type meMasterGender = Gender.Type.FEMALE;
    private float mfApPar;
    private float mfApParOld = -1f;
    private float mfExpPar;
    private float mFrameInPosX;
    private int miApMax;
    private int miApNow;
    private int miGiftCount;
    private int miLevel;
    private int miMasterEquipID = 1;
    private bool mIsEnableApRecoverBtn;
    private int miSpellMax;
    private int miSpellNow;
    private NoticeNumberComponent mNoticeNumber;
    [SerializeField]
    private GameObject mNoticeNumberPrefab;
    private PlayMakerFSM mTargetFsm;
    private bool mtIsUpdate;
    public static readonly float MV_SPD_TIME = 0.25f;
    private UserGameEntity userGameEntity;

    private void Awake()
    {
        this.mFrameInPosX = base.gameObject.GetLocalPositionX();
        this.FrameOut(true);
        this.mApRecoverBtnSp = this.mApRecoverBtn.gameObject.GetComponent<UISprite>();
        this.mApRecoverBtnColl = this.mApRecoverBtn.gameObject.GetComponent<BoxCollider>();
        this.SetApRecoverBtnEnable(true);
    }

    protected void cbfGift_Exit(bool isDecide)
    {
        this.mfCallFsmEvent("EV_GIFT_PROC_FIN");
    }

    protected void cbfGift_ExitClose()
    {
        this.mCloseGiftAct.Call();
        SingletonMonoBehaviour<CommonUI>.Instance.CloseUsrPresentList();
    }

    public void cbfGift_Init(GameObject oFsmObject)
    {
        this.mfCallFsmEvent("EV_GIFT_INIT_FIN");
    }

    public void cbfGift_Proc(GameObject oFsmObject)
    {
        Debug.Log(string.Concat(new object[] { "GiftBtn:Input.touchCount=", Input.touchCount, " : frm=", Time.frameCount }));
        SingletonMonoBehaviour<CommonUI>.Instance.OpenUsrPresentList(true, new UserPresentBoxWindow.ClickDelegate(this.cbfGift_Exit), delegate {
            if (this.userGameEntity != null)
            {
                this.mfSetGiftCount(this.getPresentList(this.userGameEntity).Length);
                this.mfSetIsUpdate(true);
            }
        });
    }

    public void cbfGift_Push()
    {
        TerminalPramsManager.PlaySE_Decide();
        this.mfCallFsmEvent("BTN_GIFT");
    }

    public void cbfSlide_In()
    {
        this.mfCallFsmEvent("SLIDE_IN");
    }

    public void cbfSlide_Out()
    {
        this.mfCallFsmEvent("SLIDE_OUT");
    }

    public void cbfStatus_Init(GameObject oFsmObject)
    {
        this.mfInitUserData();
        this.mfCallFsmEvent("EV_PRAM_INIT_FIN");
    }

    public void FrameIn()
    {
        this.FrameIn(false);
    }

    public void FrameIn(bool is_force)
    {
        this.FrameInOut(true, is_force);
    }

    private void FrameInOut(bool is_framein, bool is_force = false)
    {
        GameObject gameObject = base.gameObject;
        float mFrameInPosX = this.mFrameInPosX;
        float num2 = FRAME_OUT_POS_X;
        float x = !is_framein ? num2 : mFrameInPosX;
        if (is_force || (x == gameObject.GetLocalPositionX()))
        {
            gameObject.SetLocalPositionX(x);
            this.FrameInOutEnd();
        }
        else
        {
            float duration = TerminalPramsManager.GetIntpTime_AutoResume(MV_SPD_TIME);
            Vector3 localPosition = gameObject.GetLocalPosition();
            localPosition.x = mFrameInPosX;
            Vector3 vector2 = gameObject.GetLocalPosition();
            vector2.x = num2;
            TweenPosition position = UITweener.Begin<TweenPosition>(gameObject, duration);
            position.from = !is_framein ? localPosition : vector2;
            position.to = !is_framein ? vector2 : localPosition;
            position.method = UITweener.Method.EaseOut;
            position.eventReceiver = base.gameObject;
            position.callWhenFinished = "FrameInOutEnd";
        }
    }

    private void FrameInOutEnd()
    {
        this.UpdatePanel();
        this.mfCallFsmEvent("GO_NEXT");
    }

    public void FrameOut()
    {
        this.FrameOut(false);
    }

    public void FrameOut(bool is_force)
    {
        this.FrameInOut(false, is_force);
    }

    public UserPresentBoxEntity[] getPresentList(UserGameEntity userGame) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserPresentBoxMaster>(DataNameKind.Kind.USER_PRESENT_BOX).getVaildList(userGame.userId);

    private void mfCallFsmEvent(string sEventStr)
    {
        if (null == this.mTargetFsm)
        {
            this.mTargetFsm = base.GetComponent<PlayMakerFSM>();
        }
        if (null != this.mTargetFsm)
        {
            this.mTargetFsm.Fsm.Event(sEventStr);
        }
    }

    public void mfFaceMngCallback()
    {
        foreach (UITexture texture in this.mcFaceObjP.GetComponentsInChildren<UITexture>())
        {
            if (texture.name == "Body")
            {
                texture.SetDimensions(350, 350);
                texture.pivot = UIWidget.Pivot.Center;
                texture.gameObject.transform.localPosition = Vector3.zero;
            }
        }
    }

    private string mfGetApRecoverTimeStr()
    {
        string str = string.Empty;
        if (this.userGameEntity.IsNeedRecoverAct())
        {
            long num = this.userGameEntity.getActNextRecoverTime();
            int num2 = (int) (num / 60L);
            int num3 = (int) (num % 60L);
            str = $"{num2:D}:{num3:D2}";
        }
        return str;
    }

    private string mfGetCommandSpellRecoverTimeStr()
    {
        string str = string.Empty;
        this.miSpellNow = this.userGameEntity.getCommandSpell();
        this.miSpellMax = BalanceConfig.CommandSpellMax;
        if (this.miSpellNow != this.miSpellMax)
        {
            int num;
            long num2;
            this.userGameEntity.getCmdSpellInfo(out num, out num2);
            long num3 = num2 / 60L;
            long num4 = num2 % 60L;
            str = $"{num3:D}:{num4:D2}";
        }
        return str;
    }

    private int mfGetFsmValueInt(string sValueStr)
    {
        int num = 0;
        if (null == this.mTargetFsm)
        {
            this.mTargetFsm = base.GetComponent<PlayMakerFSM>();
        }
        if (null != this.mTargetFsm)
        {
            num = this.mTargetFsm.Fsm.Variables.GetFsmInt(sValueStr).Value;
        }
        return num;
    }

    public void mfInitUserData()
    {
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.SetAllParam(this.userGameEntity);
        this.mfSetIsUpdate(true);
    }

    public void mfSetAp(int iApNow, int iApMax)
    {
        this.miApNow = iApNow;
        this.miApMax = iApMax;
        if (0 < this.miApNow)
        {
            this.mfApPar = ((float) this.miApNow) / ((float) this.miApMax);
        }
        else
        {
            this.mfApPar = 0f;
        }
    }

    public void mfSetFaceID(int iEquipID, Gender.Type eGender)
    {
        this.miMasterEquipID = iEquipID;
        this.meMasterGender = eGender;
    }

    private void mfSetFsmValueInt(string sValueStr, int iValueInt)
    {
        if (null == this.mTargetFsm)
        {
            this.mTargetFsm = base.GetComponent<PlayMakerFSM>();
        }
        if (null != this.mTargetFsm)
        {
            this.mTargetFsm.Fsm.Variables.GetFsmInt(sValueStr).Value = iValueInt;
        }
    }

    public void mfSetGiftCount(int iGiftCount)
    {
        this.miGiftCount = iGiftCount;
    }

    public void mfSetIsUpdate(bool tIsUpdate)
    {
        this.mtIsUpdate = tIsUpdate;
    }

    public void mfSetLevel(int iLevel)
    {
        this.miLevel = iLevel;
    }

    public void mfSetSpell(int iSpellNow, int iSpellMax)
    {
        this.miSpellNow = iSpellNow;
        this.miSpellMax = iSpellMax;
    }

    public void mfUpdatePrams()
    {
        int num2;
        long num3;
        if (this.mcMasterCurTexP != null)
        {
            UnityEngine.Object.Destroy(this.mcMasterCurTexP.gameObject);
            this.mcMasterCurTexP = null;
        }
        int equipId = 0;
        if (this.miMasterEquipID > 0)
        {
            equipId = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(this.miMasterEquipID).equipId;
        }
        this.mcMasterCurTexP = MasterFaceManager.CreatePrefab(this.mcFaceObjP, UIMasterFaceRender.DispType.STATUS, (int) this.meMasterGender, equipId, 1, new System.Action(this.mfFaceMngCallback));
        this.mcLevelStrP.text = string.Empty + this.miLevel;
        this.mcExpSldP.value = this.mfExpPar;
        if (0 <= this.miApMax)
        {
            this.UpdateApStatus();
            this.mcApTimeP.text = this.mfGetApRecoverTimeStr();
        }
        this.commandSpellComp.SetData(this.userGameEntity);
        this.userGameEntity.getCmdSpellInfo(out num2, out num3);
        if (0 < this.miGiftCount)
        {
            this.mcGiftButtonP.normalSprite = "status_icongift_close";
            Animation component = this.mcGiftButtonP.gameObject.GetComponent<Animation>();
            if (component != null)
            {
                component.Play("PlayerStatusGiftAnimClip");
            }
            this.mcGiftBaseP.SetActive(true);
            if (null == this.mNoticeNumber)
            {
                GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.mNoticeNumberPrefab);
                self.SafeSetParent(this.mcGiftBaseP);
                this.mNoticeNumber = self.GetComponent<NoticeNumberComponent>();
            }
            this.mNoticeNumber.SetNumber(this.miGiftCount);
        }
        else
        {
            this.mcGiftButtonP.normalSprite = "status_icongift_open";
            Animation animation2 = this.mcGiftButtonP.gameObject.GetComponent<Animation>();
            if (animation2 != null)
            {
                animation2.Play("PlayerStatusGiftStopClip");
            }
            this.mcGiftBaseP.SetActive(false);
        }
        this.mtIsUpdate = false;
    }

    public void OnClickApRecoverBtn()
    {
        if (this.mIsEnableApRecoverBtn)
        {
            TerminalPramsManager.PlaySE_Decide();
            SingletonMonoBehaviour<CommonUI>.Instance.OpenApRecoverItemListDialog(0, delegate (ApRecoverDlgComponent.Result result) {
                if (result == ApRecoverDlgComponent.Result.RECOVER)
                {
                    this.mApRecoverAct.Call();
                }
                SingletonMonoBehaviour<CommonUI>.Instance.CloseApRecoverItemListDialog();
            });
        }
    }

    public void OpenExpInfo()
    {
        int num;
        int num2;
        float num3;
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        if (!this.userGameEntity.getExpInfo(out num, out num2, out num3))
        {
            Debug.LogError("userGameEntity not found");
        }
        this.expInfoWindow.openExpInfo(num2, true);
    }

    public void OpenMasterFormation()
    {
        SingletonMonoBehaviour<SceneManager>.Instance.changeScene(SceneList.Type.MasterFormation, SceneManager.FadeType.BLACK, null);
    }

    public void SetAllParam(UserGameEntity entity)
    {
        int num;
        int num2;
        this.mfSetLevel(entity.getLv());
        this.mfSetFaceID((int) entity.userEquipId, (Gender.Type) ((int) Enum.ToObject(typeof(Gender.Type), entity.genderType)));
        entity.getExpInfo(out num, out num2, out this.mfExpPar);
        this.mfSetAp(entity.getAct(), entity.getActMax());
        this.mfSetSpell(entity.getCommandSpell(), ConstantMaster.getValue("MAX_COMMAND_SPELL"));
        this.mfSetGiftCount(this.getPresentList(entity).Length);
    }

    public void SetApRecoverAct(System.Action act)
    {
        this.mApRecoverAct = act;
    }

    public void SetApRecoverBtnEnable(bool is_enable)
    {
        this.mIsEnableApRecoverBtn = is_enable;
        this.mApRecoverBtnSp.color = !is_enable ? Color.gray : Color.white;
    }

    public void SetCloseGiftAct(System.Action act)
    {
        this.mCloseGiftAct = act;
    }

    public void SetGiftBtnColliderEnable(bool is_enable)
    {
        this.mcGiftButtonP.gameObject.GetComponent<BoxCollider>().enabled = is_enable;
    }

    private void Start()
    {
        if (null == this.mTargetFsm)
        {
            this.mTargetFsm = base.GetComponent<PlayMakerFSM>();
        }
    }

    private void Update()
    {
        if (this.mtIsUpdate)
        {
            this.mfUpdatePrams();
        }
        if (this.userGameEntity != null)
        {
            this.mfSetAp(this.userGameEntity.getAct(), this.userGameEntity.getActMax());
            if (this.mfApPar != this.mfApParOld)
            {
                this.mfApParOld = this.mfApPar;
                this.UpdateApStatus();
            }
            if (this.mcApTimeP != null)
            {
                this.mcApTimeP.text = this.mfGetApRecoverTimeStr();
            }
            int num = this.userGameEntity.getCommandSpell();
            if (this.CommandSpellRest != num)
            {
                this.commandSpellComp.SetData(this.userGameEntity);
                this.CommandSpellRest = num;
            }
        }
    }

    private void UpdateApStatus()
    {
        if (this.mcApSldP != null)
        {
            this.mcApSldP.value = (this.mfApPar <= 1f) ? this.mfApPar : 1f;
        }
        if (this.mcApSldP2 != null)
        {
            this.mcApSldP2.value = (this.mfApPar <= 1f) ? 0f : (this.mfApPar - 1f);
            if (this.mcApSldP2.value > 0f)
            {
                this.mcApStrP.text = AP_OVER_TXT_COLOR_STR + this.miApNow.ToString() + "[-]/" + this.miApMax.ToString();
            }
            else
            {
                this.mcApStrP.text = this.miApNow.ToString() + "/" + this.miApMax.ToString();
            }
        }
    }

    private void UpdatePanel()
    {
        this.expInfoWindow.Close();
        UIPanel component = base.GetComponent<UIPanel>();
        if (component != null)
        {
            component.Invalidate(true);
        }
    }

    public enum enSpell
    {
        Zero,
        One,
        Two,
        Tree,
        enMAX
    }
}

