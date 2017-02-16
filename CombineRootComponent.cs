using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CombineRootComponent : SceneRootComponent
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache24;
    public EventBannerControl bannerCtr;
    protected int baseSvtCollectionLimtCnt;
    protected int baseSvtCollectionLv;
    private UserServantEntity baseUsrSvtData;
    [SerializeField]
    protected CheckCombineEnalbleControl checkCombineCtr;
    public CombineInitData combineData;
    [SerializeField]
    protected Transform combineInstance;
    [SerializeField]
    protected Transform combineResultInstance;
    public CombineServantListViewManager combineSvtManager;
    protected SetLevelUpData dataInfo;
    protected static int FIGURE_ID = 0x7a4a4;
    public LimitCntUpControl limitCntCtr;
    protected List<LimitCntUpItemComponent> limitUpItemList;
    [SerializeField]
    protected GameObject maskPanelObj;
    private List<UserServantEntity> materialUsrSvtList;
    public MenuListControl menuCtr;
    public NpCombineControl npCombineCtr;
    protected string requestVoiceData;
    private string resultIdx;
    protected long[] selectServantIdList;
    [SerializeField]
    protected GameObject servantBase;
    public SkillCombineControl skillCombineCtr;
    protected UIStandFigureR standFigure;
    [SerializeField]
    protected StandFigureBack standFigureBack;
    protected State state;
    public ServantCombineControl svtCombineCtr;
    public SvtEquipCombineControl svtEqCombineCtr;
    public SvtEqCombineListViewManager svtEqListManager;
    public TitleInfoControl titleInfo;
    protected SceneJumpInfo transitionData;
    protected string voiceData;
    protected string[][] voiceListReturn;
    protected string[][] voiceListWelcome;
    protected SePlayer voicePlayer;
    protected int voicePlayingcnt;
    protected string[] voicePlayingList;

    public void BackCombineTop()
    {
        if (this.state == State.INIT_SVTEQ_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.DispMain(true);
                this.svtEqCombineCtr.gameObject.SetActive(false);
                this.svtEqCombineCtr.initSvtEqCombine();
                this.svtEqListManager.DestroyList();
                this.svtEqListManager.ResetInit();
                this.state = State.INIT_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(this.PlayRandomDavinchVoice));
            });
        }
    }

    public void BackLimitCntUp()
    {
        if (this.state == State.INIT_LIMITUP_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.DispMain(true);
                this.limitCntCtr.gameObject.SetActive(false);
                this.limitCntCtr.initLimitUp();
                this.combineSvtManager.DestroyList();
                this.combineSvtManager.ResetInit();
                this.state = State.INIT_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(this.PlayRandomDavinchVoice));
            });
        }
    }

    public void BackLimitCntUpTop()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.limitCntCtr.gameObject.SetActive(true);
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_LIMITUP_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void BackMaterialSvtList()
    {
        if (this.state == State.INIT_MATERIAL_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.svtCombineCtr.gameObject.SetActive(true);
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void BackNpCombineTop()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.npCombineCtr.gameObject.SetActive(true);
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_NP_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void BackNpMaterialSvtList()
    {
        if (this.state == State.INIT_MATERIAL_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.npCombineCtr.gameObject.SetActive(true);
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_NP_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void BackServantList()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.svtCombineCtr.setSelectMaterialEnable();
                this.svtCombineCtr.gameObject.SetActive(true);
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void BackSkillCombineTop()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.skillCombineCtr.gameObject.SetActive(true);
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_SKILL_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void BackSvtCombine()
    {
        if (this.state == State.INIT_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.DispMain(true);
                this.svtCombineCtr.gameObject.SetActive(false);
                this.svtCombineCtr.initSvtCombine();
                this.combineSvtManager.DestroyList();
                this.combineSvtManager.ResetInit();
                this.state = State.INIT_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(this.PlayRandomDavinchVoice));
            });
        }
    }

    public void BackSvtEqCombineTop()
    {
        if (this.state == State.INIT_SVTEQ_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtEqListManager.gameObject.SetActive(false);
                this.svtEqCombineCtr.setSelectMaterialEnable();
                this.svtEqCombineCtr.gameObject.SetActive(true);
                this.svtEqListManager.DestroyList();
                this.state = State.INIT_SVTEQ_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void BackSvtEqMaterialList()
    {
        if (this.state == State.INIT_SVTEQ_MATERIAL_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtEqListManager.gameObject.SetActive(false);
                this.svtEqCombineCtr.gameObject.SetActive(true);
                this.svtEqListManager.DestroyList();
                this.state = State.INIT_SVTEQ_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void BackSvtNpCombine()
    {
        if (this.state == State.INIT_NP_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.DispMain(true);
                this.npCombineCtr.gameObject.SetActive(false);
                this.combineSvtManager.DestroyList();
                this.combineSvtManager.ResetInit();
                this.state = State.INIT_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(this.PlayRandomDavinchVoice));
            });
        }
    }

    public void BackSvtSkillCombine()
    {
        if (this.state == State.INIT_SKILL_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.DispMain(true);
                this.skillCombineCtr.gameObject.SetActive(false);
                this.combineSvtManager.DestroyList();
                this.combineSvtManager.ResetInit();
                this.state = State.INIT_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(this.PlayRandomDavinchVoice));
            });
        }
    }

    public override void beginFinish()
    {
        this.quit();
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        base.setMainMenuBar(MainMenuBar.Kind.COMBINE, 0x2e);
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
        Debug.Log("Call beginInitialize");
    }

    public override void beginResume()
    {
        Debug.Log("Call beginResume");
        base.beginResume();
    }

    public override void beginStartUp()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.standFigureBack.Set(FIGURE_ID, 0, Face.Type.NORMAL, null);
        MainMenuBar.setMenuActive(true, null);
        base.beginStartUp();
        Debug.Log("Call beginStartUp");
    }

    public override void beginStartUp(object data)
    {
        if (data == null)
        {
            this.transitionData = null;
        }
        else
        {
            this.transitionData = data as SceneJumpInfo;
        }
        this.LoadVoice();
        this.beginStartUp();
    }

    private void CallbackLimitUpRequest(string result)
    {
        base.myFSM.SendEvent("REQUEST_OK");
    }

    private void CallbackSkillCombineRequest(string result)
    {
        base.myFSM.SendEvent("REQUEST_OK");
    }

    private void CallbackSvtCombineRequest(string result)
    {
        if (result.Equals("ng"))
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
        else
        {
            this.resultIdx = result;
            base.myFSM.SendEvent("REQUEST_OK");
        }
    }

    private void CallbackSvtEqCombineRequest(string result)
    {
        if (result.Equals("ng"))
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
        else
        {
            this.resultIdx = result;
            base.myFSM.SendEvent("REQUEST_OK");
        }
    }

    private void CallbackTdCombineRequest(string result)
    {
        base.myFSM.SendEvent("REQUEST_OK");
    }

    private void checkCombineEnable()
    {
        this.checkCombineCtr.checkCombineEnableNum();
        CombineEnableData enableData = this.checkCombineCtr.getCombineEnableNumInfo();
        this.menuCtr.setCombineEnableNum(enableData);
    }

    public void checkEventSvt(bool isConfirm)
    {
        Debug.Log("!! ** !! checkEventSvt: " + this.baseUsrSvtData);
        UserServantEntity selectBaseSvtData = this.combineSvtManager.GetSelectBaseSvtData();
        if (selectBaseSvtData == null)
        {
            base.myFSM.SendEvent("CANCLE_NOTICE");
        }
        if ((selectBaseSvtData != null) && selectBaseSvtData.IsEventJoin())
        {
            string message = this.setEventJoinMsgDetail(selectBaseSvtData);
            if (isConfirm)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDecideDlg(LocalizationManager.Get("EVENT_JOIN_COMBINE_MSG_TITLE"), message, "确定", "取消", new CommonConfirmDialog.ClickDelegate(this.closeEventSvtConfirmDlg));
            }
            else
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(LocalizationManager.Get("EVENT_JOIN_COMBINE_MSG_TITLE"), message, new NotificationDialog.ClickDelegate(this.closeEventSvtNoticeDlg), -1);
            }
        }
        else
        {
            base.myFSM.SendEvent("END_NOTICE");
        }
    }

    private void checkExeBtnState(bool stateFlg)
    {
        if (stateFlg)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.myFSM.SendEvent("EXE_SVTCOMBINE");
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        }
    }

    public void CheckIsFromFollower()
    {
        GameObject obj2 = GameObject.Find("ItemDetailInfo");
        if ((obj2 != null) && obj2.activeInHierarchy)
        {
            base.myFSM.SendEvent("OPEN_ITEMPLACE");
        }
        else
        {
            base.myFSM.SendEvent("OPEN_INIT");
        }
    }

    public void checkTransitionInfo()
    {
        if (this.transitionData != null)
        {
            if (this.transitionData.Name == "ServantCombine")
            {
                base.myFSM.SendEvent("COMBINE_SVT_DIRECT");
                this.transitionData = null;
            }
            else if (this.transitionData.Name == "ServantEQCombine")
            {
                base.myFSM.SendEvent("COMBINE_SVTEQ_DIRECT");
                this.transitionData = null;
            }
            else
            {
                this.transitionData = null;
            }
        }
    }

    private void closeEventSvtConfirmDlg(bool isDecide)
    {
        if (isDecide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(() => base.myFSM.SendEvent("END_NOTICE"));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(() => base.myFSM.SendEvent("CANCLE_NOTICE"));
        }
    }

    public void closeEventSvtNoticeDlg(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog(() => base.myFSM.SendEvent("END_NOTICE"));
    }

    public void CloseLimitUpResultInfo()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.InitLimitUpCombine();
            this.titleInfo.setBackBtnColliderEnable(true);
            UserServantEntity usrSvtData = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
            if (!usrSvtData.isLimitCountMax())
            {
                this.limitCntCtr.checkIsSelectBaseSvt(usrSvtData.id);
                this.limitCntCtr.setBaseSvtCardImg(usrSvtData);
                this.limitCntCtr.showItemListInfo();
                this.combineSvtManager.SetSelectBaseSvtData(usrSvtData);
                this.limitCntCtr.setStateInfoMsg(StateType.EXE_COMBINE);
            }
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        });
    }

    public void CloseSvtCombineResultInfo()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.InitSvtCombine();
            this.titleInfo.setBackBtnColliderEnable(true);
            UserServantEntity usrSvtData = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
            if ((!usrSvtData.isLevelMax() || !usrSvtData.isAdjustHpMax()) || !usrSvtData.isAdjustAtkMax())
            {
                this.svtCombineCtr.checkIsSelectBaseSvt(usrSvtData.id);
                this.svtCombineCtr.setBaseSvtCardImg(usrSvtData);
                this.svtCombineCtr.setSelectMaterialEnable();
                this.combineSvtManager.SetSelectBaseSvtData(usrSvtData);
                this.svtCombineCtr.setStateInfoMsg(StateType.SELECT_MATERIAL);
            }
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        });
    }

    public void CloseSvtEqCombineResultInfo()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.InitSvtEqCombine();
            this.titleInfo.setBackBtnColliderEnable(true);
            UserServantEntity usrSvtData = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
            if (!usrSvtData.isLevelMax() || !usrSvtData.isLimitCountMax())
            {
                this.svtEqCombineCtr.checkIsSelectBaseSvtEq(usrSvtData.id);
                this.svtEqCombineCtr.setBaseSvtEqCardImg(usrSvtData);
                this.svtEqCombineCtr.setSelectMaterialEnable();
                this.svtEqListManager.SetSelectBaseSvtData(usrSvtData);
                this.svtEqCombineCtr.setStateInfoMsg(StateType.SELECT_MATERIAL);
            }
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        });
    }

    private void createSvtStandFigure()
    {
        if (this.standFigure == null)
        {
            this.standFigure = StandFigureManager.CreateRenderPrefab(this.servantBase, FIGURE_ID, 0, Face.Type.NORMAL, 1, null);
        }
    }

    private void destroySvtStandFigure()
    {
        if (this.standFigure != null)
        {
            UnityEngine.Object.Destroy(this.standFigure.gameObject);
            this.standFigure = null;
        }
    }

    public void DispLimitUpResult()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        SingletonMonoBehaviour<CommonUI>.Instance.OpenLimitUpCombineResult(CombineResultEffectComponent.Kind.LIMITUP, this.baseUsrSvtData, this.baseSvtCollectionLimtCnt, new CombineResultEffectComponent.ClickDelegate(this.EndDispResult));
        this.maskPanelObj.SetActive(true);
    }

    private void DispMain(bool isShow)
    {
        MainMenuBar.setMenuActive(isShow, null);
        this.servantBase.SetActive(isShow);
        this.bannerCtr.gameObject.SetActive(isShow);
        this.menuCtr.gameObject.SetActive(isShow);
        if (isShow)
        {
            this.standFigureBack.Fadein(null);
        }
        else
        {
            this.standFigureBack.Fadeout(null);
        }
        this.checkCombineEnable();
    }

    public void DispSkillCombineResult()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        SingletonMonoBehaviour<CommonUI>.Instance.OpenSkillCombineResult(CombineResultEffectComponent.Kind.SKILL_LEVELUP, this.baseUsrSvtData, this.dataInfo.currentId, this.dataInfo.nextLv, new CombineResultEffectComponent.ClickDelegate(this.EndDispResult), 0, 0);
        this.maskPanelObj.SetActive(true);
    }

    public void DispSvtEqCombineResult()
    {
    }

    public void DispTreasureDvcCombineResult()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        SingletonMonoBehaviour<CommonUI>.Instance.OpenNobleCombineResult(CombineResultEffectComponent.Kind.TREASUREDVC_LEVELUP, this.baseUsrSvtData, this.dataInfo.currentId, this.dataInfo.nextLv, new CombineResultEffectComponent.ClickDelegate(this.EndDispResult), 0, this.dataInfo.currentLv);
        this.maskPanelObj.SetActive(true);
    }

    protected void EndCloseShowServant()
    {
        base.myFSM.SendEvent("CLOSE_SVT_STATUS");
    }

    public void EndDispResult(bool isDecide)
    {
        if (isDecide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseCombineResult();
            base.myFSM.SendEvent("CLICK_BACK");
            SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
        }
    }

    protected void EndLoadVoice()
    {
        if (this.voiceData != null)
        {
            SoundManager.releaseAudioAssetStorage(this.voiceData);
            this.voiceData = null;
        }
        if (this.requestVoiceData != null)
        {
            this.voiceData = this.requestVoiceData;
            this.requestVoiceData = null;
            int index = UnityEngine.Random.Range(0, this.voiceListWelcome.Length);
            this.PlayVoice(this.voiceListWelcome[index]);
        }
    }

    protected void EndShowServant(bool isDecide)
    {
        UserServantEntity selectBaseSvtData = this.combineSvtManager.GetSelectBaseSvtData();
        switch (this.state)
        {
            case State.INIT_BASE_SELECT:
            case State.INIT_MATERIAL_SELECT:
            case State.INIT_SKILL_BASE_SELECT:
            case State.INIT_NP_BASE_SELECT:
            case State.INIT_NP_MATERIAL_SELECT:
                this.combineSvtManager.ModifyItem();
                this.combineSvtManager.SetMode(CombineServantListViewManager.InitMode.INPUT);
                break;

            case State.INIT_SKILL_COMBINE_TOP:
                this.skillCombineCtr.setBaseSvtCardImg(selectBaseSvtData);
                break;

            case State.INIT_SVTEQ_COMBINE_TOP:
                this.svtEqListManager.ModifyItem();
                break;

            case State.INIT_SVTEQ_BASE_SELECT:
            case State.INIT_SVTEQ_MATERIAL_SELECT:
                this.svtEqListManager.ModifyItem();
                this.svtEqListManager.SetMode(SvtEqCombineListViewManager.InitMode.INPUT);
                break;

            default:
                this.combineSvtManager.ModifyItem();
                break;
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseShowServant));
    }

    public void FadeOutDisp()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, () => base.myFSM.SendEvent("END_FADE"));
    }

    public void FadeOutLimitUpDisp()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, () => base.myFSM.SendEvent("END_FADE"));
    }

    public void FadeOutSvtEqDisp()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, () => base.myFSM.SendEvent("END_FADE"));
    }

    public void Init()
    {
        if (this.state == State.INIT)
        {
            if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_COMBINE))
            {
                if (<>f__am$cache24 == null)
                {
                    <>f__am$cache24 = delegate {
                    };
                }
                SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.COMBINE_TOP, TutorialFlag.Id.TUTORIAL_LABEL_COMBINE, <>f__am$cache24);
            }
            this.combineData.getEventData();
            this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.COMBINE);
            this.titleInfo.setBackBtnSprite(true);
            this.titleInfo.setBackBtnDepth(0x2d);
            this.bannerCtr.setBannerList();
            this.menuCtr.setMenuEventNotice();
            this.checkCombineEnable();
            this.maskPanelObj.SetActive(false);
            this.state = State.INIT_TOP;
        }
    }

    public void InitLimitUpCombine()
    {
        if (this.state == State.INIT_LIMITUP_TOP)
        {
            this.maskPanelObj.SetActive(false);
            this.combineSvtManager.DestroyList();
            this.combineSvtManager.ResetInit();
            this.limitCntCtr.initLimitUp();
        }
    }

    public void InitSkillNpCombine()
    {
        this.maskPanelObj.SetActive(false);
        this.combineSvtManager.DestroyList();
        this.combineSvtManager.ResetInit();
        this.titleInfo.setBackBtnColliderEnable(true);
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        if (this.state == State.INIT_SKILL_COMBINE_TOP)
        {
            this.skillCombineCtr.initSvtSkillCombine(SkillCombineControl.TargetType.SKILL_COMBINE);
            base.myFSM.SendEvent("INIT_SKILLCOMBINE");
        }
        if (this.state == State.INIT_NP_COMBINE_TOP)
        {
            this.npCombineCtr.initSvtNpCombine();
            base.myFSM.SendEvent("INIT_NPCOMBINE");
        }
    }

    public void InitSvtCombine()
    {
        if (this.state == State.INIT_COMBINE_TOP)
        {
            this.maskPanelObj.SetActive(false);
            this.combineSvtManager.DestroyList();
            this.combineSvtManager.ResetInit();
            this.svtCombineCtr.initSvtCombine();
        }
    }

    public void InitSvtEqCombine()
    {
        if (this.state == State.INIT_SVTEQ_COMBINE_TOP)
        {
            this.maskPanelObj.SetActive(false);
            this.svtEqListManager.DestroyList();
            this.svtEqListManager.ResetInit();
            this.svtEqCombineCtr.initSvtEqCombine();
        }
    }

    protected void LoadVoice()
    {
        if ((this.requestVoiceData == null) && (this.voiceData == null))
        {
            ConstantStrMaster master = (ConstantStrMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.CONSTANT_STR);
            this.voiceListWelcome = master.GetCombineWelcomeVoiceList();
            this.voiceListReturn = master.GetCombineReturnVoiceList();
            if ((this.voiceListWelcome == null) || (this.voiceListReturn == null))
            {
                this.requestVoiceData = null;
            }
            else
            {
                this.requestVoiceData = "ChrVoice_" + FIGURE_ID;
                SoundManager.loadAudioAssetStorage(this.requestVoiceData, new System.Action(this.EndLoadVoice), SoundManager.CueType.ALL);
            }
        }
    }

    public void OnClickBack()
    {
        State state = this.state;
        switch (state)
        {
            case State.INIT_TOP:
                this.titleInfo.sendEvent("GOTO_TERMINAL");
                return;

            case State.INIT_COMBINE_TOP:
            case State.INIT_BASE_SELECT:
            case State.INIT_MATERIAL_SELECT:
            case State.INIT_LIMITUP_TOP:
            case State.INIT_SKILL_COMBINE_TOP:
            case State.INIT_NP_COMBINE_TOP:
                break;

            default:
                switch (state)
                {
                    case State.INIT_SVTEQ_COMBINE_TOP:
                    case State.INIT_SVTEQ_BASE_SELECT:
                    case State.INIT_SVTEQ_MATERIAL_SELECT:
                        break;

                    default:
                        return;
                }
                break;
        }
        this.titleInfo.sendEvent("CLICK_BACK");
    }

    public void onMaterialSelectList()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        base.myFSM.SendEvent("SELECT_MATERIAL");
    }

    public void onNpMaterialSelectList()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        base.myFSM.SendEvent("SELECT_MATERIAL");
    }

    protected void OnSelectMaterialSvt(SetCombineData combineData)
    {
        this.svtCombineCtr.setCombineData(combineData);
    }

    public void onSvtEqMaterialSelectList()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        base.myFSM.SendEvent("SELECT_MATERIAL");
    }

    protected void PlayRandomDavinchVoice()
    {
        if (this.voiceData != null)
        {
            int index = UnityEngine.Random.Range(0, this.voiceListReturn.Length);
            this.PlayVoice(this.voiceListReturn[index]);
        }
    }

    public void PlayVoice(string[] playlist)
    {
        if (this.voiceData != null)
        {
            if (this.voicePlayer != null)
            {
                Debug.Log("voiceplyer Not NULL");
                this.StopVoice();
            }
            this.voicePlayingList = playlist;
            this.voicePlayingcnt = -1;
            this.PlayVoiceList();
        }
    }

    protected void PlayVoiceList()
    {
        this.voicePlayingcnt++;
        if ((this.voicePlayingList == null) || (this.voicePlayingcnt >= this.voicePlayingList.Length))
        {
            this.voicePlayingList = null;
            this.voicePlayer = null;
        }
        else
        {
            this.voicePlayer = SoundManager.playVoice(this.voiceData, "0_" + this.voicePlayingList[this.voicePlayingcnt], SoundManager.DEFAULT_VOLUME, new System.Action(this.PlayVoiceList));
            Debug.Log("called :" + this.voicePlayingList[this.voicePlayingcnt] + " " + this.voicePlayer.AssetName);
        }
    }

    public void quit()
    {
        this.standFigureBack.Init();
        this.svtCombineCtr.gameObject.SetActive(false);
        this.skillCombineCtr.gameObject.SetActive(false);
        this.combineSvtManager.gameObject.SetActive(false);
        this.combineSvtManager.DestroyList();
        this.combineSvtManager.ResetInit();
        this.destroySvtStandFigure();
        this.ReleaseVoiceAsset();
        this.menuCtr.resetScrollView();
        this.state = State.INIT;
    }

    protected void ReleaseVoiceAsset()
    {
        this.requestVoiceData = null;
        if (this.voicePlayer != null)
        {
            this.StopVoice();
            this.voicePlayer = null;
        }
        if (this.voiceData != null)
        {
            SoundManager.releaseAudioAssetStorage(this.voiceData);
            this.voiceData = null;
        }
    }

    public void RequestLimitUp()
    {
        this.baseUsrSvtData = this.combineSvtManager.GetSelectBaseSvtData();
        this.baseSvtCollectionLimtCnt = this.combineSvtManager.GetBaseCollectionLimitCnt();
        this.limitUpItemList = this.limitCntCtr.getItemInfoList();
        if (this.baseUsrSvtData.id > 0L)
        {
            SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
            NetworkManager.getRequest<ServantLimitCombineRequest>(new NetworkManager.ResultCallbackFunc(this.CallbackLimitUpRequest)).beginRequest(this.baseUsrSvtData.id);
            this.maskPanelObj.SetActive(true);
        }
    }

    public void RequestSkillCombine()
    {
        this.baseUsrSvtData = this.combineSvtManager.GetSelectBaseSvtData();
        if (this.baseUsrSvtData.id > 0L)
        {
            this.dataInfo = this.skillCombineCtr.getTargetData();
            SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
            NetworkManager.getRequest<ServantSkillCombineRequest>(new NetworkManager.ResultCallbackFunc(this.CallbackSkillCombineRequest)).beginRequest(this.baseUsrSvtData.id, this.dataInfo.currentIndex, this.dataInfo.currentId);
            this.maskPanelObj.SetActive(true);
        }
    }

    public void RequestSvtCombine()
    {
        this.baseUsrSvtData = this.combineSvtManager.GetSelectBaseSvtData();
        this.baseSvtCollectionLv = this.combineSvtManager.GetBaseCollectionLv();
        long id = this.baseUsrSvtData.id;
        long[] materialUsrSvtIdList = this.combineSvtManager.getSelectCombineData().materialUsrSvtIdList;
        if ((id > 0L) && (materialUsrSvtIdList != null))
        {
            this.materialUsrSvtList = new List<UserServantEntity>();
            for (int i = 0; i < materialUsrSvtIdList.Length; i++)
            {
                long num3 = materialUsrSvtIdList[i];
                UserServantEntity item = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(num3);
                this.materialUsrSvtList.Add(item);
            }
            string materialSvtIds = JsonManager.toJson(materialUsrSvtIdList);
            int useQp = this.svtCombineCtr.getSpendQpVal();
            int getExp = this.svtCombineCtr.getCombineExpVal();
            SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
            NetworkManager.getRequest<ServantCombineRequest>(new NetworkManager.ResultCallbackFunc(this.CallbackSvtCombineRequest)).beginRequest(id, materialSvtIds, useQp, getExp);
            this.maskPanelObj.SetActive(true);
        }
    }

    public void RequestSvtEqCombine()
    {
        this.baseUsrSvtData = this.svtEqListManager.GetSelectBaseSvtData();
        this.baseSvtCollectionLv = this.svtEqListManager.GetBaseCollectionLv();
        long id = this.baseUsrSvtData.id;
        long[] materialUsrSvtIdList = this.svtEqListManager.getSelectCombineData().materialUsrSvtIdList;
        if ((id > 0L) && (materialUsrSvtIdList != null))
        {
            this.materialUsrSvtList = new List<UserServantEntity>();
            for (int i = 0; i < materialUsrSvtIdList.Length; i++)
            {
                long num3 = materialUsrSvtIdList[i];
                UserServantEntity item = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(num3);
                this.materialUsrSvtList.Add(item);
            }
            string materialSvtIds = JsonManager.toJson(materialUsrSvtIdList);
            SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
            NetworkManager.getRequest<ServantEquipCombineRequest>(new NetworkManager.ResultCallbackFunc(this.CallbackSvtEqCombineRequest)).beginRequest(id, materialSvtIds);
            this.maskPanelObj.SetActive(true);
        }
    }

    public void RequestTdCombine()
    {
        this.baseUsrSvtData = this.combineSvtManager.GetSelectBaseSvtData();
        long[] materialUsrSvtIdList = this.combineSvtManager.getSelectCombineData().materialUsrSvtIdList;
        if ((this.baseUsrSvtData.id > 0L) && (materialUsrSvtIdList != null))
        {
            this.dataInfo = this.npCombineCtr.getTargetData();
            this.materialUsrSvtList = new List<UserServantEntity>();
            for (int i = 0; i < materialUsrSvtIdList.Length; i++)
            {
                long id = materialUsrSvtIdList[i];
                UserServantEntity item = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(id);
                this.materialUsrSvtList.Add(item);
            }
            string materialSvtIds = JsonManager.toJson(materialUsrSvtIdList);
            SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
            NetworkManager.getRequest<ServantTreasureDvcCombineRequest>(new NetworkManager.ResultCallbackFunc(this.CallbackTdCombineRequest)).beginRequest(this.baseUsrSvtData.id, this.dataInfo.currentIndex, this.dataInfo.currentId, materialSvtIds);
            this.maskPanelObj.SetActive(true);
        }
    }

    public void ResetBaseSvtSkillCombine()
    {
        UserServantEntity resData = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
        if (!this.skillCombineCtr.checkIsMaxLvSkills(resData))
        {
            this.skillCombineCtr.setBaseSvtSkillInfo(resData, this.dataInfo.realIndex);
            this.skillCombineCtr.setBaseSvtCardImg(resData);
            this.combineSvtManager.SetSelectBaseSvtData(resData);
        }
    }

    public void ResetBaseSvtTdCombine()
    {
        UserServantEntity resData = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.baseUsrSvtData.id);
        if (!this.npCombineCtr.checkTdLvMax(resData))
        {
            this.npCombineCtr.setBaseSvtNpInfo(resData);
            this.npCombineCtr.setBaseSvtCardImg(resData);
            this.npCombineCtr.setSelectMaterialEnable();
            this.combineSvtManager.SetSelectBaseSvtData(resData);
            this.npCombineCtr.setStateInfoMsg(StateType.SELECT_MATERIAL);
        }
    }

    public void SelectMaterialSvt()
    {
        if (this.state == State.INIT_MATERIAL_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.svtCombineCtr.gameObject.SetActive(true);
                this.OnSelectMaterialSvt(this.combineSvtManager.getSelectCombineData());
                this.combineSvtManager.DestroyList();
                this.svtCombineCtr.setStateInfoMsg(StateType.EXE_COMBINE);
                this.state = State.INIT_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void SelectNpMaterialSvt()
    {
        if (this.state == State.INIT_MATERIAL_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.npCombineCtr.gameObject.SetActive(true);
                SetCombineData data = this.combineSvtManager.getSelectCombineData();
                this.npCombineCtr.setNpCombineData(data);
                this.npCombineCtr.setStateInfoMsg(StateType.EXE_COMBINE);
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_NP_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void SelectShowServant()
    {
        UserServantEntity userSvtEntity = null;
        ServantStatusDialog.Kind cOMBINE = ServantStatusDialog.Kind.COMBINE;
        switch (this.state)
        {
            case State.INIT_COMBINE_TOP:
                switch (this.svtCombineCtr.getTargetType())
                {
                    case ServantCombineControl.TargetType.BASE_STATUS:
                        userSvtEntity = this.combineSvtManager.GetSelectBaseSvtData();
                        break;

                    case ServantCombineControl.TargetType.MATERIAL_STATUS:
                        cOMBINE = ServantStatusDialog.Kind.COMBINE_MATERIAL_SVT;
                        userSvtEntity = this.svtCombineCtr.getMaterialUsrSvtData();
                        break;
                }
                break;

            case State.INIT_BASE_SELECT:
                userSvtEntity = this.combineSvtManager.getSelectUsrSvtEntity();
                break;

            case State.INIT_MATERIAL_SELECT:
                cOMBINE = !this.combineSvtManager.checkIsSelectMaterial() ? ServantStatusDialog.Kind.COMBINE : ServantStatusDialog.Kind.COMBINE_MATERIAL_SVT;
                userSvtEntity = this.combineSvtManager.getSelectUsrSvtEntity();
                break;

            case State.INIT_LIMITUP_TOP:
            case State.INIT_SKILL_COMBINE_TOP:
                userSvtEntity = this.combineSvtManager.GetSelectBaseSvtData();
                break;

            case State.INIT_NP_COMBINE_TOP:
                switch (this.npCombineCtr.getTargetType())
                {
                    case NpCombineControl.TargetType.BASE_STATUS:
                        userSvtEntity = this.npCombineCtr.getBaseUsrSvtData();
                        break;

                    case NpCombineControl.TargetType.MATERIAL_STATUS:
                        cOMBINE = ServantStatusDialog.Kind.COMBINE_MATERIAL_SVT;
                        userSvtEntity = this.npCombineCtr.getMaterialUsrSvtData();
                        break;
                }
                break;

            case State.INIT_SVTEQ_COMBINE_TOP:
            {
                SvtEquipCombineControl.TargetType type3 = this.svtEqCombineCtr.getTargetType();
                if (type3 != SvtEquipCombineControl.TargetType.BASE_STATUS)
                {
                    if (type3 == SvtEquipCombineControl.TargetType.MATERIAL_STATUS)
                    {
                        cOMBINE = ServantStatusDialog.Kind.COMBINE_MATERIAL_EQUIP;
                        userSvtEntity = this.svtEqCombineCtr.getMaterialUsrSvtData();
                    }
                    break;
                }
                userSvtEntity = this.svtEqListManager.GetSelectBaseSvtData();
                break;
            }
            case State.INIT_SVTEQ_BASE_SELECT:
                userSvtEntity = this.svtEqListManager.getSelectUsrSvtEntity();
                break;

            case State.INIT_SVTEQ_MATERIAL_SELECT:
                cOMBINE = !this.svtEqListManager.checkIsSelectMaterial() ? ServantStatusDialog.Kind.COMBINE : ServantStatusDialog.Kind.COMBINE_MATERIAL_EQUIP;
                userSvtEntity = this.svtEqListManager.getSelectUsrSvtEntity();
                break;
        }
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(cOMBINE, userSvtEntity, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
    }

    protected string setEventJoinMsgDetail(UserServantEntity targetEnt)
    {
        DateTime time = NetworkManager.getLocalDateTime();
        string str2 = string.Format(LocalizationManager.Get("DISP_TIMESTR_FORMAT"), new object[] { time.Year, time.Month, time.Day, time.Hour, time.Minute });
        if (targetEnt.IsEventJoin())
        {
            EventServantEntity entity = targetEnt.getEventServant();
            if (entity != null)
            {
                str2 = entity.getEndTimeStr();
            }
        }
        string str3 = LocalizationManager.Get("CONFIRM_TITLE_SVT_COMBINE");
        string str4 = LocalizationManager.Get("COMBINE_MATERIAL_SVT");
        switch (this.state)
        {
            case State.INIT_BASE_SELECT:
                if (this.combineSvtManager.getItemType() == CombineServantListViewItem.Type.LIMITUP_BASE)
                {
                    str3 = LocalizationManager.Get("CONFIRM_TITLE_LIMITUP");
                    str4 = LocalizationManager.Get("COMBINE_MATERIAL_ITEM");
                }
                break;

            case State.INIT_LIMITUP_TOP:
                str3 = LocalizationManager.Get("CONFIRM_TITLE_LIMITUP");
                str4 = LocalizationManager.Get("COMBINE_MATERIAL_ITEM");
                break;

            case State.INIT_SKILL_COMBINE_TOP:
            case State.INIT_SKILL_BASE_SELECT:
                str3 = LocalizationManager.Get("CONFIRM_TITLE_SKILL_COMBINE");
                str4 = LocalizationManager.Get("COMBINE_MATERIAL_ITEM");
                break;

            case State.INIT_NP_COMBINE_TOP:
            case State.INIT_NP_BASE_SELECT:
                str3 = LocalizationManager.Get("CONFIRM_TITLE_TD_COMBINE");
                str4 = LocalizationManager.Get("COMBINE_MATERIAL_SVT");
                break;
        }
        return string.Format(LocalizationManager.Get("EVENT_JOIN_COMBINE_MSG"), new object[] { str2, str3, str4, str3 });
    }

    public void setLimitUpResultInfo()
    {
    }

    public void SetNeedItemList()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.limitCntCtr.showItemListInfo();
                this.limitCntCtr.gameObject.SetActive(true);
                this.combineSvtManager.DestroyList();
                if (this.combineSvtManager.GetSelectBaseSvtData() != null)
                {
                    this.limitCntCtr.setStateInfoMsg(StateType.EXE_COMBINE);
                }
                else
                {
                    this.limitCntCtr.setStateInfoMsg(StateType.SELECT_BASE);
                }
                this.state = State.INIT_LIMITUP_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void setResultInfo()
    {
    }

    public void SetSelectMaterialSvt()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.svtCombineCtr.setSelectMaterialEnable();
                this.svtCombineCtr.gameObject.SetActive(true);
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void SetSelectMaterialSvtList()
    {
        if (this.state == State.INIT_MATERIAL_SELECT)
        {
            this.combineSvtManager.setSelectedSvtList();
        }
    }

    public void SetSelectNpMaterialSvtList()
    {
        if (this.state == State.INIT_MATERIAL_SELECT)
        {
            this.combineSvtManager.setSelectedSvtList();
        }
    }

    public void SetSelectSvtEqMaterialList()
    {
        if (this.state == State.INIT_SVTEQ_MATERIAL_SELECT)
        {
            this.svtEqListManager.setSelectedSvtList();
        }
    }

    public void SetSelectSvtNp()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.npCombineCtr.gameObject.SetActive(true);
                UserServantEntity usrSvtData = this.combineSvtManager.GetSelectBaseSvtData();
                this.npCombineCtr.initSvtNpCombine();
                if (usrSvtData != null)
                {
                    this.npCombineCtr.setBaseSvtCardImg(usrSvtData);
                    this.npCombineCtr.setBaseSvtNpInfo(usrSvtData);
                    this.npCombineCtr.setSelectMaterialEnable();
                    this.npCombineCtr.setStateInfoMsg(StateType.SELECT_MATERIAL);
                }
                else
                {
                    this.npCombineCtr.setStateInfoMsg(StateType.SELECT_BASE);
                }
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_NP_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void SetSelectSvtSkill()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.combineSvtManager.gameObject.SetActive(false);
                this.skillCombineCtr.gameObject.SetActive(true);
                UserServantEntity usrSvtData = this.combineSvtManager.GetSelectBaseSvtData();
                this.skillCombineCtr.initSvtSkillCombine(SkillCombineControl.TargetType.SKILL_COMBINE);
                if (usrSvtData != null)
                {
                    this.skillCombineCtr.setBaseSvtCardImg(usrSvtData);
                    this.skillCombineCtr.setBaseSvtSkillInfo(usrSvtData, 0);
                }
                else
                {
                    this.skillCombineCtr.setStateInfoMsg(StateType.SELECT_BASE);
                }
                this.combineSvtManager.DestroyList();
                this.state = State.INIT_SKILL_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void setSvtEqCombineResultInfo()
    {
    }

    private void setTitleInfo()
    {
        switch (this.state)
        {
            case State.INIT_TOP:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.COMBINE);
                this.titleInfo.SetHelpBtn(true);
                break;

            case State.INIT_COMBINE_TOP:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SERVANT_COMBINE);
                break;

            case State.INIT_BASE_SELECT:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SERVANT_COMBINE);
                break;

            case State.EXIT_BASE_SELECT:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SERVANT_COMBINE);
                break;

            case State.INIT_MATERIAL_SELECT:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SERVANT_COMBINE);
                break;

            case State.INIT_LIMITUP_TOP:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.LIMIT_COMBINE);
                break;

            case State.INIT_SKILL_COMBINE_TOP:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SKILL_COMBINE);
                break;

            case State.INIT_NP_COMBINE_TOP:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.NP_COMBINE);
                break;

            case State.INIT_SVTEQ_COMBINE_TOP:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SVT_EQUIP_COMBINE);
                break;

            case State.INIT_SVTEQ_BASE_SELECT:
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SVT_EQUIP_COMBINE);
                break;
        }
    }

    public void ShowBaseListLimitCntUp()
    {
        if (this.state == State.INIT_LIMITUP_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.limitCntCtr.gameObject.SetActive(false);
                this.combineSvtManager.gameObject.SetActive(true);
                this.combineSvtManager.CreateList(CombineServantListViewItem.Type.LIMITUP_BASE);
                this.state = State.INIT_BASE_SELECT;
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.LIMIT_COMBINE);
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowBaseListNpCombine()
    {
        if (this.state == State.INIT_NP_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.npCombineCtr.gameObject.SetActive(false);
                this.combineSvtManager.gameObject.SetActive(true);
                this.combineSvtManager.CreateList(CombineServantListViewItem.Type.NP_BASE);
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.NP_COMBINE);
                this.state = State.INIT_BASE_SELECT;
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowBaseListSkillCombine()
    {
        if ((this.state == State.INIT_SKILL_COMBINE_TOP) || (this.state == State.INIT_NP_COMBINE_TOP))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.skillCombineCtr.gameObject.SetActive(false);
                this.combineSvtManager.gameObject.SetActive(true);
                this.combineSvtManager.CreateList(CombineServantListViewItem.Type.SKILL_BASE);
                if (this.state == State.INIT_SKILL_COMBINE_TOP)
                {
                    this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SKILL_COMBINE);
                }
                else if (this.state == State.INIT_NP_COMBINE_TOP)
                {
                    this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.NP_COMBINE);
                }
                this.state = State.INIT_BASE_SELECT;
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowBaseSvtEq()
    {
        if (this.state == State.INIT_SVTEQ_BASE_SELECT)
        {
            UserServantEntity selectBaseSvtData = this.svtEqListManager.GetSelectBaseSvtData();
            if (selectBaseSvtData != null)
            {
                this.svtEqCombineCtr.checkIsSelectBaseSvtEq(selectBaseSvtData.id);
                this.svtEqCombineCtr.setBaseSvtEqCardImg(selectBaseSvtData);
                this.svtEqListManager.clearSelectedSvtList();
                this.svtEqCombineCtr.setStateInfoMsg(StateType.SELECT_MATERIAL);
            }
            else
            {
                this.svtEqCombineCtr.checkIsSelectBaseSvtEq(0L);
                this.svtEqCombineCtr.setStateInfoMsg(StateType.SELECT_BASE);
            }
        }
    }

    public void ShowBaseSvtEqList()
    {
        if (this.state == State.INIT_SVTEQ_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtEqCombineCtr.gameObject.SetActive(false);
                this.svtEqListManager.gameObject.SetActive(true);
                this.svtEqListManager.CreateList(SvtEqCombineListViewItem.Type.SVTEQ_BASE);
                this.state = State.INIT_SVTEQ_BASE_SELECT;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowCombineEffect()
    {
        this.maskPanelObj.SetActive(false);
        this.combineInstance.GetComponent<CombineEffectComponent>().SetServantCombineInfo(this.baseUsrSvtData, this.materialUsrSvtList);
        this.titleInfo.setBackBtnColliderEnable(false);
        SoundManager.stopBgm();
    }

    public void ShowExeNoticeDlg()
    {
        switch (this.state)
        {
            case State.INIT_LIMITUP_TOP:
                this.checkExeBtnState(this.limitCntCtr.getExeBtnState());
                return;

            case State.INIT_SKILL_COMBINE_TOP:
                this.checkExeBtnState(this.skillCombineCtr.getExeBtnState());
                return;

            case State.INIT_NP_COMBINE_TOP:
                this.checkExeBtnState(this.npCombineCtr.getExeBtnState());
                return;

            case State.INIT_COMBINE_TOP:
                this.checkExeBtnState(this.svtCombineCtr.getExeBtnState());
                break;

            case State.INIT_SVTEQ_COMBINE_TOP:
                this.checkExeBtnState(this.svtEqCombineCtr.getExeBtnState());
                return;
        }
    }

    public void ShowLimitCntUp()
    {
        if (this.state == State.INIT_TOP)
        {
            this.DispMain(false);
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.limitCntCtr.initLimitUp();
                this.limitCntCtr.gameObject.SetActive(true);
                this.limitCntCtr.setStateInfoMsg(StateType.SELECT_BASE);
                this.state = State.INIT_LIMITUP_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowLimitUpEffect()
    {
        CombineEffectComponent component = this.combineInstance.GetComponent<CombineEffectComponent>();
        List<int> list = new List<int>();
        for (int i = 0; i < this.limitUpItemList.Count; i++)
        {
            int item = this.limitUpItemList[i].getItemImgId();
            list.Add(item);
        }
        component.SetSkillCombineInfo(this.baseUsrSvtData, list);
        this.titleInfo.setBackBtnColliderEnable(false);
        SoundManager.stopBgm();
    }

    public void ShowMaterialSvtList()
    {
        if (this.state == State.INIT_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtCombineCtr.gameObject.SetActive(false);
                this.combineSvtManager.gameObject.SetActive(true);
                this.combineSvtManager.CreateList(CombineServantListViewItem.Type.MATERIAL);
                this.combineSvtManager.SetMode(CombineServantListViewManager.InitMode.INPUT);
                this.state = State.INIT_MATERIAL_SELECT;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowNpMaterialSvtList()
    {
        if (this.state == State.INIT_NP_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.npCombineCtr.gameObject.SetActive(false);
                this.combineSvtManager.gameObject.SetActive(true);
                this.combineSvtManager.CreateList(CombineServantListViewItem.Type.NP_MATERIAL);
                this.combineSvtManager.SetMode(CombineServantListViewManager.InitMode.INPUT);
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.NP_COMBINE);
                this.state = State.INIT_MATERIAL_SELECT;
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowOpenResult()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        int infoIdx = int.Parse(this.resultIdx);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantCombineResult(CombineResultEffectComponent.Kind.SVT_COMBINE, infoIdx, this.baseUsrSvtData, this.baseSvtCollectionLv, new CombineResultEffectComponent.ClickDelegate(this.EndDispResult));
        this.maskPanelObj.SetActive(true);
    }

    public void ShowSelectBaseSvt()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            string usrSvtId = base.myFSM.Fsm.Variables.GetFsmString("TestUsrSvtId").Value;
            this.svtCombineCtr.setTestLb(usrSvtId);
            UserServantEntity selectBaseSvtData = this.combineSvtManager.GetSelectBaseSvtData();
            if (selectBaseSvtData != null)
            {
                this.svtCombineCtr.checkIsSelectBaseSvt(selectBaseSvtData.id);
                this.svtCombineCtr.setBaseSvtCardImg(selectBaseSvtData);
                this.combineSvtManager.clearSelectedSvtList();
                this.svtCombineCtr.setStateInfoMsg(StateType.SELECT_MATERIAL);
            }
            else
            {
                this.svtCombineCtr.checkIsSelectBaseSvt(0L);
                this.svtCombineCtr.setStateInfoMsg(StateType.SELECT_BASE);
            }
        }
    }

    public void ShowSelectLimitUpBaseSvt()
    {
        if (this.state == State.INIT_BASE_SELECT)
        {
            UserServantEntity selectBaseSvtData = this.combineSvtManager.GetSelectBaseSvtData();
            if (selectBaseSvtData != null)
            {
                this.limitCntCtr.checkIsSelectBaseSvt(selectBaseSvtData.id);
                this.limitCntCtr.setBaseSvtCardImg(selectBaseSvtData);
                this.combineSvtManager.clearSelectedSvtList();
            }
            else
            {
                this.limitCntCtr.checkIsSelectBaseSvt(0L);
            }
        }
    }

    public void ShowSelectSvtEqMaterial()
    {
        if (this.state == State.INIT_SVTEQ_MATERIAL_SELECT)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtEqListManager.gameObject.SetActive(false);
                this.svtEqCombineCtr.gameObject.SetActive(true);
                this.svtEqCombineCtr.setSvtEqCombineData(this.svtEqListManager.getSelectCombineData());
                this.svtEqListManager.DestroyList();
                this.svtEqCombineCtr.setStateInfoMsg(StateType.EXE_COMBINE);
                this.state = State.INIT_SVTEQ_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowServantList()
    {
        if (this.state == State.INIT_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtCombineCtr.gameObject.SetActive(false);
                this.combineSvtManager.gameObject.SetActive(true);
                this.combineSvtManager.CreateList(CombineServantListViewItem.Type.BASE);
                this.state = State.INIT_BASE_SELECT;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowSkillCombineEffect()
    {
        CombineEffectComponent component = this.combineInstance.GetComponent<CombineEffectComponent>();
        int[] combineItemIds = this.dataInfo.combineItemIds;
        List<int> list = new List<int>();
        for (int i = 0; i < combineItemIds.Length; i++)
        {
            ItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(combineItemIds[i]);
            list.Add(entity.imageId);
        }
        component.SetSkillCombineInfo(this.baseUsrSvtData, list);
        this.titleInfo.setBackBtnColliderEnable(false);
        SoundManager.stopBgm();
    }

    public void ShowSvtCombine()
    {
        if (this.state == State.INIT_TOP)
        {
            this.DispMain(false);
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtCombineCtr.gameObject.SetActive(true);
                this.svtCombineCtr.initSvtCombine();
                this.svtCombineCtr.setStateInfoMsg(StateType.SELECT_BASE);
                this.state = State.INIT_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowSvtEqCombineEffect()
    {
        this.combineInstance.GetComponent<CombineEffectComponent>().SetServantCombineInfo(this.baseUsrSvtData, this.materialUsrSvtList);
        this.titleInfo.setBackBtnColliderEnable(false);
        SoundManager.stopBgm();
    }

    public void ShowSvtEqLevelUpStatus()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        int infoIdx = int.Parse(this.resultIdx);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantCombineResult(CombineResultEffectComponent.Kind.SVTEQ_COMBINE, infoIdx, this.baseUsrSvtData, this.baseSvtCollectionLv, new CombineResultEffectComponent.ClickDelegate(this.EndDispResult));
        this.maskPanelObj.SetActive(true);
    }

    public void ShowSvtEqMaterialList()
    {
        if (this.state == State.INIT_SVTEQ_COMBINE_TOP)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtEqCombineCtr.gameObject.SetActive(false);
                this.svtEqListManager.gameObject.SetActive(true);
                this.svtEqListManager.CreateList(SvtEqCombineListViewItem.Type.SVTEQ_MATERIAL);
                this.svtEqListManager.SetMode(SvtEqCombineListViewManager.InitMode.INPUT);
                this.state = State.INIT_SVTEQ_MATERIAL_SELECT;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowSvtEquipCombineTop()
    {
        if (this.state == State.INIT_TOP)
        {
            this.DispMain(false);
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.svtEqCombineCtr.gameObject.SetActive(true);
                this.svtEqCombineCtr.initSvtEqCombine();
                this.svtEqCombineCtr.setStateInfoMsg(StateType.SELECT_BASE);
                this.state = State.INIT_SVTEQ_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowSvtNpCombine()
    {
        if (this.state == State.INIT_TOP)
        {
            this.DispMain(false);
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.npCombineCtr.gameObject.SetActive(true);
                this.npCombineCtr.initSvtNpCombine();
                this.npCombineCtr.setStateInfoMsg(StateType.SELECT_BASE);
                this.state = State.INIT_NP_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowSvtSkillCombine()
    {
        if (this.state == State.INIT_TOP)
        {
            this.DispMain(false);
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                this.skillCombineCtr.gameObject.SetActive(true);
                this.skillCombineCtr.initSvtSkillCombine(SkillCombineControl.TargetType.SKILL_COMBINE);
                this.skillCombineCtr.setStateInfoMsg(StateType.SELECT_BASE);
                this.state = State.INIT_SKILL_COMBINE_TOP;
                this.setTitleInfo();
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
            });
        }
    }

    public void ShowTdCombineEffect()
    {
        this.combineInstance.GetComponent<CombineEffectComponent>().SetServantCombineInfo(this.baseUsrSvtData, this.materialUsrSvtList);
        this.titleInfo.setBackBtnColliderEnable(false);
        SoundManager.stopBgm();
    }

    public void Start()
    {
        base.Start();
    }

    public void StopVoice()
    {
        if (this.voicePlayer != null)
        {
            this.voicePlayer.Destroy();
            this.voicePlayer = null;
            this.voicePlayingList = null;
        }
    }

    public void svtCombineFadeIn()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
    }

    protected enum State
    {
        INIT,
        INIT_TOP,
        INPUT_TOP,
        INIT_COMBINE_TOP,
        EXIT_COMBINE_TOP,
        INIT_BASE_SELECT,
        EXIT_BASE_SELECT,
        INIT_MATERIAL_SELECT,
        EXIT_MATERIAL_SELECT,
        INIT_LIMITUP_TOP,
        INIT_SKILL_COMBINE_TOP,
        EXIT_SKILL_COMBINE_TOP,
        INIT_SKILL_BASE_SELECT,
        EXIT_SKILL_BASE_SELECT,
        INIT_NP_COMBINE_TOP,
        EXIT_NP_COMBINE_TOP,
        INIT_NP_BASE_SELECT,
        EXIT_NP_BASE_SELECT,
        INIT_NP_MATERIAL_SELECT,
        INIT_SVTEQ_COMBINE_TOP,
        INIT_SVTEQ_BASE_SELECT,
        INIT_SVTEQ_MATERIAL_SELECT
    }

    public enum StateType
    {
        SELECT_BASE,
        SELECT_MATERIAL,
        EXE_COMBINE
    }
}

