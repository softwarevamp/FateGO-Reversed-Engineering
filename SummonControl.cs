using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SummonControl : BaseMonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache49;
    [CompilerGenerated]
    private static System.Action <>f__am$cache4A;
    [CompilerGenerated]
    private static System.Action <>f__am$cache4B;
    [CompilerGenerated]
    private static System.Action <>f__am$cache4C;
    private int afterTicketNum;
    private string assetPath;
    private AssetData bannerAssetData;
    private List<GachaBannerComponent> bannerInfoList;
    public UIWrapContent bannerLoopCtr;
    private List<int> befSvtList = new List<int>();
    public GameObject bgRoot;
    private Vector3 center;
    public UICenterOnChild centerChild;
    protected static readonly float COLOR_VAL = 0.375f;
    public SummonConfirmDlgComponent confirmDlgInfo;
    private GachaBannerComponent currentBannerComp;
    private VaildGachaInfo currentGachaInfo;
    private int currentIdx;
    private int currentMoveIdx;
    public UILabel currentPointNumLb;
    public GameObject currentResourceInfo;
    public UILabel currentStoneNumLb;
    public GameObject detailInfo;
    private int friendPoint;
    public PlayMakerFSM fsm;
    private GachaMaster gachaMst;
    private GachaRqParamData gachaParamData;
    private GachaInfos[] gachaResInfoList;
    private int getSvtIdx;
    private int haveChargeStone;
    private int haveFreeStone;
    private int haveStone;
    private int haveTicketNum;
    public UIPanel indexPanel;
    private Vector3 initBannerPos;
    private bool isDailyGacha;
    private bool isDoneSecTutorial;
    private bool isDoneSvtEqTutorial;
    private bool isDoneTutorial;
    public UIButton leftArrowBtn;
    public GameObject maskBgObject;
    public GameObject maskObject;
    private int maxSvtEqNum;
    private int maxSvtNum;
    private int needPoint;
    private int needStone;
    public GameObject noneGachaInfo;
    private GachaEntity[] payGachaList;
    private GachaEntity pointGachaData;
    public UISprite pointNumInfo;
    private int resType;
    public UIButton rightArrowBtn;
    public GameObject slideIndexPrefab;
    public UIGrid sliderGrid;
    public UISprite stoneNumInfo;
    public GameObject summonBannerInfo;
    public GameObject summonBannerPrefab;
    public UIScrollView summonBannerScrollView;
    private SummonEffectComponent summonComp;
    public SummonInfoControl summonInfoCtr;
    public Transform summonInstance;
    public SummonResultComponent summonResultInfo;
    private int summonType;
    protected int TalkSvtId;
    protected int TalkSvtLimitCount;
    public TitleInfoControl titleInfo;
    public readonly Vector2 TUTORIAL_FORMATION_ARROW_POS = new Vector2(-365f, -160f);
    public readonly Rect TUTORIAL_FORMATION_ARROW_RECT = new Rect(-445f, -310f, 150f, 200f);
    public readonly Vector2 TUTORIAL_MENU_ARROW_POS = new Vector2(430f, -250f);
    public readonly Rect TUTORIAL_MENU_ARROW_RECT = new Rect(340f, -320f, 200f, 100f);
    public readonly Vector2 TUTORIAL_SUMMON_ARROW_POS = new Vector2(-90f, -170f);
    public readonly Rect TUTORIAL_SUMMON_ARROW_RECT = new Rect(-120f, -220f, 240f, 100f);
    private TUTORIAL_KIND tutorialKind;
    private GachaEntity[] useGachaData;
    private UserGameEntity userGameEntity;
    private List<VaildGachaInfo> useSummonList;
    private List<VaildGachaInfo> vaildGachaList = new List<VaildGachaInfo>();

    private void callbackGachaDraw(string result)
    {
        if (result.Equals("ng"))
        {
            this.fsm.SendEvent("REQUEST_NG");
        }
        else if (!result.Equals("ng"))
        {
            resData[] dataArray = JsonManager.DeserializeArray<resData>("[" + result + "]");
            this.gachaResInfoList = dataArray[0].gachaInfos;
            this.fsm.SendEvent("REQUEST_OK");
        }
        else
        {
            this.fsm.SendEvent("REQUEST_NG");
        }
    }

    private void callbackTutorialSet(string result)
    {
        this.fsm.SendEvent("REQUEST_OK");
    }

    public void checkGachaResource()
    {
        if (this.gachaParamData.gachaType == 3)
        {
            if (this.summonInfoCtr.getIsFree())
            {
                this.fsm.SendEvent("SHOW_CONFIRM_DLG");
            }
            else
            {
                this.fsm.SendEvent("CHECK_POINT");
            }
        }
        else if ((this.gachaParamData.gachaType == 1) || (this.gachaParamData.gachaType == 7))
        {
            this.fsm.SendEvent("CHECK_STONE");
        }
        else if (this.gachaParamData.gachaType == 5)
        {
            this.fsm.SendEvent("SHOW_CONFIRM_DLG");
        }
    }

    public void checkGetSvtNum()
    {
        if (this.gachaResInfoList.Length > 1)
        {
            this.maskBgObject.SetActive(false);
            this.fsm.SendEvent("GET_MULTI");
        }
        else
        {
            this.fsm.SendEvent("GET_SINGLE");
        }
    }

    public void checkIsNewSvt()
    {
        if (this.gachaResInfoList[0].isNew)
        {
            this.fsm.SendEvent("NEW_SVT");
        }
        else
        {
            this.fsm.SendEvent("GO_BACK");
        }
    }

    public void checkIsNewSvtMulti()
    {
        this.resType = 1;
        for (int i = 0; i < this.gachaResInfoList.Length; i++)
        {
            GachaInfos infos = this.gachaResInfoList[i];
            if (infos.isNew)
            {
                this.resType = 2;
                break;
            }
        }
        this.showSummonResultInfo();
    }

    public void checkIsNewSvtNum()
    {
        Debug.Log("!! *** checkIsNewSvtNum Index: " + this.getSvtIdx);
        if (this.getSvtIdx <= (this.gachaResInfoList.Length - 1))
        {
            if (this.gachaResInfoList[this.getSvtIdx].isNew)
            {
                int objectId = this.gachaResInfoList[this.getSvtIdx].objectId;
                if (!this.checkOverlapSvt(objectId))
                {
                    this.fsm.SendEvent("NEW_SVT");
                    this.befSvtList.Add(objectId);
                }
                else
                {
                    this.fsm.SendEvent("OLD_SVT");
                }
            }
            else
            {
                this.fsm.SendEvent("OLD_SVT");
            }
        }
        else
        {
            this.fsm.SendEvent("FINAL_SVT");
        }
    }

    private bool checkMaxDrawNum()
    {
        bool flag = false;
        if (this.gachaParamData != null)
        {
            UserGachaEntity entity = this.getUserGachaData(this.gachaParamData.gachaId);
            GachaEntity entity2 = this.getCurrentGachaData(this.gachaParamData.gachaId);
            if (((entity != null) && (entity2.maxDrawNum > 0)) && (entity.num >= entity2.maxDrawNum))
            {
                flag = true;
            }
        }
        return flag;
    }

    private bool checkOverlapSvt(int svtId)
    {
        int count = this.befSvtList.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                int num3 = this.befSvtList[i];
                if (num3 == svtId)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void checkSummonTutorial()
    {
        if (TutorialFlag.IsProgressDone(TutorialFlag.Progress._1) && !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_STONE_GACHA))
        {
            this.isDoneTutorial = false;
            this.tutorialKind = TUTORIAL_KIND.TUTORIAL1_MSG;
            this.progTutorial();
        }
        else if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._2) && TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_STONE_GACHA))
        {
            this.isDoneTutorial = false;
            this.fsm.SendEvent("RETURN_TUTORIAL");
        }
        else if ((TutorialFlag.IsProgressDone(TutorialFlag.Progress._2) && TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_STONE_GACHA)) && !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_GACHA_SCENE))
        {
            this.isDoneSecTutorial = false;
            this.tutorialKind = TUTORIAL_KIND.TUTORIAL3_MSG;
            this.progTutorial();
        }
        else
        {
            this.setTutorialCtrEnable(true);
            this.summonInfoCtr.setTutorialExeBtnEnable(true);
            this.titleInfo.setBackBtnColliderEnable(true);
            MainMenuBar.SetMenuBtnColliderEnable(true);
            this.fsm.SendEvent("CLEAR_TUTORIAL");
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
    }

    public void checkUserServantNum()
    {
        int num;
        int num2;
        this.maxSvtNum = this.userGameEntity.svtKeep;
        this.maxSvtEqNum = this.userGameEntity.svtEquipKeep;
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getCount(out num, out num2);
        if ((num < this.maxSvtNum) && (num2 < this.maxSvtEqNum))
        {
            this.fsm.SendEvent("CHECK_RESOURCE");
        }
        else if (num >= this.maxSvtNum)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenSvtFrameShortDlg(num, this.maxSvtNum, false, false, new ServantFrameShortDlgComponent.CallbackFunc(this.closeShotSvtFrameDlg));
        }
        else if (num2 >= this.maxSvtEqNum)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenSvtFrameShortDlg(num2, this.maxSvtEqNum, true, false, new ServantFrameShortDlgComponent.CallbackFunc(this.closeShotSvtEqFrameDlg));
        }
    }

    public void checkUsrFriendPoint()
    {
        TblUserEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<TblUserMaster>(DataNameKind.Kind.TBL_USER_GAME).getUserData(this.userGameEntity.userId);
        this.friendPoint = entity.friendPoint;
        this.needPoint = this.gachaParamData.gachaResourceNum;
        if (this.friendPoint >= this.needPoint)
        {
            this.fsm.SendEvent("SHOW_CONFIRM_DLG");
        }
        else
        {
            this.confirmDlgInfo.OpenShortPoint(this.friendPoint, new SummonConfirmDlgComponent.CallbackFunc(this.closeShotPointDlg));
        }
    }

    public void checkUsrStoneNum()
    {
        bool flag = this.gachaParamData.gachaType == 7;
        this.haveStone = this.summonInfoCtr.getUsrStoneNum();
        this.haveFreeStone = this.summonInfoCtr.HaveFreeStoneNum;
        this.haveChargeStone = this.summonInfoCtr.HaveChargeStoneNum;
        int num = !flag ? this.haveStone : this.haveChargeStone;
        this.needStone = this.gachaParamData.gachaResourceNum;
        if (num < this.needStone)
        {
            if (flag)
            {
                this.confirmDlgInfo.OpenShortChargeStone(this.needStone, this.haveChargeStone, this.haveFreeStone, new SummonConfirmDlgComponent.CallbackFunc(this.closeShotStoneDlg));
            }
            else
            {
                this.confirmDlgInfo.OpenShortStone(this.haveStone, new SummonConfirmDlgComponent.CallbackFunc(this.closeShotStoneDlg));
            }
        }
        else
        {
            this.fsm.SendEvent("SHOW_CONFIRM_DLG");
        }
    }

    private void checkValidGachaList()
    {
        long userId = this.userGameEntity.userId;
        UserQuestEntity entity = null;
        this.vaildGachaList = new List<VaildGachaInfo>();
        this.useSummonList = new List<VaildGachaInfo>();
        for (int i = 0; i < this.useGachaData.Length; i++)
        {
            GachaEntity data = this.useGachaData[i];
            long[] args = new long[] { userId, (long) data.condQuestId };
            entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_QUEST).getEntityFromId<UserQuestEntity>(args);
            if ((entity != null) && ((entity.getClearNum() > 0) || (entity.getQuestPhase() >= data.condQuestPhase)))
            {
                if (data.beforeGachaId > 0)
                {
                    UserGachaEntity entity3 = this.getUserGachaData(data.beforeGachaId);
                    GachaEntity entity4 = this.getCurrentGachaData(data.beforeGachaId);
                    if (((entity3 != null) && (entity3.num >= entity4.maxDrawNum)) && TutorialFlag.IsProgressDone(TutorialFlag.Progress._2))
                    {
                        UserGachaEntity entity5 = this.getUserGachaData(data.id);
                        GachaEntity entity6 = this.getCurrentGachaData(data.id);
                        if (entity5 != null)
                        {
                            if ((entity6.maxDrawNum <= 0) || (entity5.num < entity6.maxDrawNum))
                            {
                                this.setVaildInfoList(data);
                            }
                        }
                        else
                        {
                            this.setVaildInfoList(data);
                        }
                    }
                }
                else
                {
                    this.setVaildInfoList(data);
                }
            }
        }
        for (int j = 0; j < this.vaildGachaList.Count; j++)
        {
            this.setUseData(this.vaildGachaList[j]);
        }
        this.vaildGachaList = this.useSummonList;
        this.vaildGachaList.Sort(new Comparison<VaildGachaInfo>(this.SlotCompare));
    }

    public void clearBannerList()
    {
        int childCount = this.bannerLoopCtr.transform.childCount;
        int num2 = this.sliderGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(this.bannerLoopCtr.transform.GetChild(i).gameObject);
            }
        }
        if (num2 > 0)
        {
            for (int j = num2 - 1; j >= 0; j--)
            {
                UnityEngine.Object.Destroy(this.sliderGrid.transform.GetChild(j).gameObject);
            }
        }
    }

    public void clearResultList()
    {
        this.summonResultInfo.clearResultList();
        this.summonResultInfo.gameObject.SetActive(false);
    }

    private void close()
    {
        base.StartCoroutine(this.WaitBattleChrLoad());
    }

    public void closeResultList()
    {
        this.getSvtIdx = 0;
        this.summonResultInfo.gameObject.SetActive(false);
        this.summonResultInfo.clearResultList();
    }

    private void closeShotPointDlg(bool res)
    {
        this.confirmDlgInfo.Close();
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.fsm.SendEvent("CLICK_CANCEL");
    }

    private void closeShotStoneDlg(bool res)
    {
        this.confirmDlgInfo.Close();
        if (res)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.fsm.SendEvent("GO_BUY_STONE");
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.fsm.SendEvent("CLICK_CANCEL");
        }
    }

    private void closeShotSvtEqFrameDlg(ServantFrameShortDlgComponent.resultClicked res)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseSvtFrameShortDlg(null);
        switch (res)
        {
            case ServantFrameShortDlgComponent.resultClicked.CONFIRM:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.fsm.SendEvent("SVTEQ_FRAME_PURCHASE");
                break;

            case ServantFrameShortDlgComponent.resultClicked.POWERUP:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.fsm.SendEvent("CLICK_CANCEL");
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Combine, SceneManager.FadeType.BLACK, new SceneJumpInfo("ServantEQCombine"));
                break;

            case ServantFrameShortDlgComponent.resultClicked.SELL:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.fsm.SendEvent("CLICK_CANCEL");
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Shop, SceneManager.FadeType.BLACK, new SceneJumpInfo("SellServant", 1));
                break;

            default:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.fsm.SendEvent("CLICK_CANCEL");
                break;
        }
    }

    private void closeShotSvtFrameDlg(ServantFrameShortDlgComponent.resultClicked res)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseSvtFrameShortDlg(null);
        switch (res)
        {
            case ServantFrameShortDlgComponent.resultClicked.CONFIRM:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.fsm.SendEvent("SVT_FRAME_PURCHASE");
                break;

            case ServantFrameShortDlgComponent.resultClicked.POWERUP:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.fsm.SendEvent("CLICK_CANCEL");
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Combine, SceneManager.FadeType.BLACK, new SceneJumpInfo("ServantCombine"));
                break;

            case ServantFrameShortDlgComponent.resultClicked.SELL:
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.fsm.SendEvent("CLICK_CANCEL");
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Shop, SceneManager.FadeType.BLACK, new SceneJumpInfo("SellServant", 0));
                break;

            default:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.fsm.SendEvent("CLICK_CANCEL");
                break;
        }
    }

    private void confirmResult(bool res)
    {
        this.confirmDlgInfo.Close();
        if (res)
        {
            MainMenuBar.setMenuActive(false, null);
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.fsm.SendEvent("START_GACHA");
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.fsm.SendEvent("CLICK_CANCEL");
        }
    }

    private void createSummonInfo()
    {
        this.bannerInfoList = new List<GachaBannerComponent>();
        if (this.vaildGachaList != null)
        {
            if (this.bannerLoopCtr == null)
            {
                this.bannerLoopCtr = this.summonBannerScrollView.gameObject.AddComponent<UIWrapContent>();
            }
            if (this.centerChild == null)
            {
                this.centerChild = this.bannerLoopCtr.gameObject.AddComponent<UICenterOnChild>();
            }
            this.centerChild.onFinished = (SpringPanel.OnFinished) Delegate.Combine(this.centerChild.onFinished, new SpringPanel.OnFinished(this.OnCenterOnChildFinished));
            this.summonBannerScrollView.onDragStarted = (UIScrollView.OnDragNotification) Delegate.Combine(this.summonBannerScrollView.onDragStarted, new UIScrollView.OnDragNotification(this.OnDragStarted));
            int num = this.vaildGachaList.Count * 2;
            int count = this.vaildGachaList.Count;
            int idx = 0;
            this.bannerLoopCtr.itemSize = 0x404;
            for (int i = 0; i < num; i++)
            {
                GameObject obj2 = base.createObject(this.summonBannerPrefab, this.bannerLoopCtr.transform, null);
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localPosition = this.bannerLoopCtr.transform.localPosition;
                int num5 = i + 1;
                obj2.name = "0" + num5;
                if (num5 > 9)
                {
                    obj2.name = "1" + num5;
                }
                GachaBannerComponent item = obj2.GetComponent<GachaBannerComponent>();
                this.bannerInfoList.Add(item);
                idx = ((num / 2) - count) + i;
                if (idx > (count - 1))
                {
                    idx -= count;
                }
                Debug.Log(string.Concat(new object[] { "*** !!! vaildGachaList Idx: ", idx, " _WarId: ", this.vaildGachaList[idx].warId, " _Gacha Id", this.vaildGachaList[idx].id, " _Gacha Name: ", this.vaildGachaList[idx].name }));
                if ((NetworkManager.PlatformManagement[0] == 0) && (idx == 0))
                {
                    idx = 1;
                }
                string searchTarget = "img_summon_" + this.vaildGachaList[idx].imgId.ToString();
                GameObject bannerAtlas = this.searchBannerImg(searchTarget);
                if (bannerAtlas != null)
                {
                    item.setBannerGachaInfo(this.vaildGachaList[idx], idx, i, bannerAtlas, searchTarget);
                }
                else
                {
                    item.setBannerGachaInfo(this.vaildGachaList[idx], idx, i, null, string.Empty);
                }
            }
            float num7 = this.sliderGrid.cellWidth * 0.5f;
            for (int j = 0; j < count; j++)
            {
                base.createObject(this.slideIndexPrefab, this.sliderGrid.transform, null).transform.localScale = Vector3.one;
            }
            this.bannerLoopCtr.SortAlphabetically();
            this.bannerLoopCtr.resetScroll();
            this.bannerLoopCtr.WrapContent();
            this.sliderGrid.transform.localPosition = new Vector3(-(num7 * (count - 1)), this.center.y, this.center.z);
            this.sliderGrid.repositionNow = true;
            if (NetworkManager.PlatformManagement[0] == 0)
            {
                this.currentIdx = 1;
            }
            this.summonInfoCtr.setSummonInfo(this.vaildGachaList[this.currentIdx], new SummonInfoControl.ClickDelegate(this.exeSummon));
            this.setSliderIcon(this.currentIdx);
            this.setResourceInfo();
        }
        else
        {
            this.noneGachaInfo.SetActive(true);
        }
    }

    private void deleteBannerList()
    {
        int childCount = this.bannerLoopCtr.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyImmediate(this.bannerLoopCtr.transform.GetChild(i).gameObject);
            }
        }
        int num3 = this.sliderGrid.transform.childCount;
        if (num3 > 0)
        {
            for (int j = num3 - 1; j >= 0; j--)
            {
                UnityEngine.Object.DestroyImmediate(this.sliderGrid.transform.GetChild(j).gameObject);
            }
        }
        this.currentIdx = 0;
        this.currentMoveIdx = 0;
    }

    public void DialogCallBack(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseDialogCallBack));
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        });
    }

    public void EndCloseDialogCallBack()
    {
        this.fsm.SendEvent("CLOSE");
    }

    private void endPurchaseStone(StonePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseStonePurchaseMenu();
        switch (result)
        {
            case StonePurchaseMenu.Result.CANCEL:
            case StonePurchaseMenu.Result.WAIT:
                this.fsm.SendEvent("PURCHASE_CANCEL");
                break;

            case StonePurchaseMenu.Result.ERROR:
                this.fsm.SendEvent("PURCHASE_ERROR");
                break;

            case StonePurchaseMenu.Result.PURCHASE:
                this.fsm.SendEvent("PURCHASE_OK");
                break;
        }
    }

    private void endPurchaseSvtEqFrame(ServantEquipFramePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipFramePurchaseMenu();
        switch (result)
        {
            case ServantEquipFramePurchaseMenu.Result.CANCEL:
                this.fsm.SendEvent("PURCHASE_CANCEL");
                break;

            case ServantEquipFramePurchaseMenu.Result.ERROR:
                this.fsm.SendEvent("PURCHASE_ERROR");
                break;

            case ServantEquipFramePurchaseMenu.Result.PURCHASE:
                this.fsm.SendEvent("PURCHASE_OK");
                break;
        }
    }

    private void endPurchaseSvtFrame(ServantFramePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantFramePurchaseMenu();
        switch (result)
        {
            case ServantFramePurchaseMenu.Result.CANCEL:
                this.fsm.SendEvent("PURCHASE_CANCEL");
                break;

            case ServantFramePurchaseMenu.Result.ERROR:
                this.fsm.SendEvent("PURCHASE_ERROR");
                break;

            case ServantFramePurchaseMenu.Result.PURCHASE:
                this.fsm.SendEvent("PURCHASE_OK");
                break;
        }
    }

    private void exeFormation()
    {
        if (<>f__am$cache4A == null)
        {
            <>f__am$cache4A = () => MainMenuBar.SetDispBtnColliderEnable(true, MainMenuBarButton.Kind.FORMATION);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialArrowMark(this.TUTORIAL_FORMATION_ARROW_POS, (float) 0f, this.TUTORIAL_FORMATION_ARROW_RECT, <>f__am$cache4A);
        MainMenuBar.SetDispBtnAct(MainMenuBarButton.Kind.FORMATION, delegate {
            MainMenuBar.SetDispBtnColliderEnable(false, MainMenuBarButton.Kind.SIZEOF);
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(delegate {
                TutorialFlag.SetProgress(TutorialFlag.Progress._2);
                this.tutorialKind = TUTORIAL_KIND.NONE;
                this.isDoneTutorial = true;
            });
        });
    }

    private void exeSummon(GachaRqParamData paramData)
    {
        this.gachaParamData = paramData;
        if (this.tutorialKind == TUTORIAL_KIND.TUTORIAL1_MSG)
        {
            this.confirmDlgInfo.setTutorial(false);
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(null);
            this.tutorialKind = TUTORIAL_KIND.TUTORIAL2_MSG;
        }
        this.fsm.SendEvent("CHECK_SERVANT_FRAME");
    }

    public void fadeOut()
    {
    }

    private GachaEntity getCurrentGachaData(int gachaId) => 
        this.gachaMst.getEntityFromId<GachaEntity>(gachaId);

    private UserGachaEntity getUserGachaData(int gachaId)
    {
        long[] args = new long[] { NetworkManager.UserId, (long) gachaId };
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GACHA).getEntityFromId<UserGachaEntity>(args);
    }

    public void incereIdx()
    {
        this.getSvtIdx++;
    }

    private bool isSvtEqSummonResult()
    {
        int length = this.gachaResInfoList.Length;
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        for (int i = 0; i < length; i++)
        {
            GachaInfos infos = this.gachaResInfoList[i];
            if (master.getEntityFromId<ServantEntity>(infos.objectId).IsServantEquip && infos.isNew)
            {
                return true;
            }
        }
        return false;
    }

    private void LoadBannerEnd(AssetData data)
    {
        Debug.Log("!! ** !!LoadMatEnd " + data.Name);
        if (data == null)
        {
            this.fsm.SendEvent("FAIL_LOAD");
        }
        this.bannerAssetData = data;
        this.createSummonInfo();
        this.setPositionByWarId();
        this.fsm.SendEvent("END_LOAD");
    }

    private void OnCenterOnChildFinished()
    {
        if (this.isDoneTutorial)
        {
            this.leftArrowBtn.enabled = true;
            this.rightArrowBtn.enabled = true;
            this.titleInfo.setBackBtnColliderEnable(true);
        }
        this.currentBannerComp = this.centerChild.centeredObject.GetComponent<GachaBannerComponent>();
        this.currentGachaInfo = this.currentBannerComp.getBannerGachaInfo();
        this.currentIdx = this.currentBannerComp.getBannerIdx();
        this.currentMoveIdx = this.currentBannerComp.getMoveBannerIdx();
        this.setSliderIcon(this.currentIdx);
        this.summonInfoCtr.setEnableSummonBnt(true);
        this.fsm.SendEvent("CHANGE_BANNER");
    }

    public void OnClickBack()
    {
        this.titleInfo.sendEvent("CLICK_BACK");
    }

    public void onClickChangeBanner()
    {
        this.leftArrowBtn.enabled = false;
        this.rightArrowBtn.enabled = false;
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        int childCount = this.bannerLoopCtr.transform.childCount;
        int index = this.currentMoveIdx + 1;
        if (index >= childCount)
        {
            index = 0;
        }
        this.summonInfoCtr.setEnableSummonBnt(false);
        this.titleInfo.setBackBtnColliderEnable(false);
        this.centerChild.CenterOn(this.bannerLoopCtr.transform.GetChild(index));
    }

    public void onClickLeftChangeBanner()
    {
        this.leftArrowBtn.enabled = false;
        this.rightArrowBtn.enabled = false;
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        int childCount = this.bannerLoopCtr.transform.childCount;
        int index = this.currentMoveIdx - 1;
        if (index < 0)
        {
            index = childCount - 1;
        }
        this.summonInfoCtr.setEnableSummonBnt(false);
        this.titleInfo.setBackBtnColliderEnable(false);
        this.centerChild.CenterOn(this.bannerLoopCtr.transform.GetChild(index));
    }

    public void OnClickSummonDetail()
    {
        this.summonInfoCtr.OnClickDetail(new System.Action(this.reDispSummonBannerList));
    }

    private void OnDragStarted()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.WINDOW_SLIDE);
        this.summonInfoCtr.setEnableSummonBnt(false);
        this.titleInfo.setBackBtnColliderEnable(false);
    }

    public void OnEndSummonEffect()
    {
        this.SetDispBgParts(true);
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, () => this.fsm.SendEvent("END_FADE"));
    }

    private void openAfterSummonInfo()
    {
        if (<>f__am$cache49 == null)
        {
            <>f__am$cache49 = delegate {
                MainMenuBar.SetMenuBtnColliderEnable(true);
                MainMenuBar.SetDispBtnColliderEnable(false, MainMenuBarButton.Kind.SIZEOF);
            };
        }
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(LocalizationManager.Get("TUTORIAL_MESSAGE_SUMMON2"), this.TUTORIAL_MENU_ARROW_POS, this.TUTORIAL_MENU_ARROW_RECT, (float) 0f, new Vector2(0f, 0f), -1, <>f__am$cache49);
        MainMenuBar.SetMenuBtnAct(delegate {
            MainMenuBar.SetMenuBtnColliderEnable(false);
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialNotificationDialogArrow(delegate {
                this.tutorialKind = TUTORIAL_KIND.EXE_FORMATION;
                this.progTutorial();
            });
        });
    }

    private void openSummonExeArrow()
    {
        int num = ((ConstantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.CONSTANT)).GetValue("USER_FREE_STONE");
        string message = string.Format(LocalizationManager.Get("TUTORIAL_MESSAGE_SUMMON1"), (this.summonInfoCtr.getUsrStoneNum() >= num) ? num : this.summonInfoCtr.tutoSummonSinglePrice());
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialNotificationDialogArrow(message, this.TUTORIAL_SUMMON_ARROW_POS, this.TUTORIAL_SUMMON_ARROW_RECT, (float) 90f, new Vector2(0f, 45f), -1, delegate {
            this.fsm.SendEvent("CLEAR_TUTORIAL");
            this.summonInfoCtr.setTutorialExeBtnEnable(true);
        });
    }

    private void openSvtEqInfo()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.SUMMON_TOP, TutorialFlag.Id.TUTORIAL_LABEL_GACHA_SCENE, () => this.fsm.SendEvent("CLEAR_TUTORIAL"));
    }

    public void procTutorialFlg()
    {
        if (!this.isDoneTutorial && (this.tutorialKind == TUTORIAL_KIND.TUTORIAL2_MSG))
        {
            NetworkManager.getRequest<TutorialSetRequest>(new NetworkManager.ResultCallbackFunc(this.callbackTutorialSet)).beginRequest(TutorialFlag.Id.TUTORIAL_LABEL_STONE_GACHA);
        }
        else
        {
            this.fsm.SendEvent("REQUEST_OK");
        }
    }

    private void progTutorial()
    {
        switch (this.tutorialKind)
        {
            case TUTORIAL_KIND.TUTORIAL1_MSG:
                this.setTutorialCtrEnable(false);
                this.titleInfo.setBackBtnColliderEnable(false);
                MainMenuBar.SetMenuBtnColliderEnable(false);
                this.summonInfoCtr.setTutorialExeBtnEnable(false);
                this.openSummonExeArrow();
                break;

            case TUTORIAL_KIND.TUTORIAL2_MSG:
                this.setTutorialCtrEnable(false);
                this.titleInfo.setBackBtnColliderEnable(false);
                this.summonInfoCtr.setTutorialExeBtnEnable(false);
                MainMenuBar.SetMenuBtnColliderEnable(false);
                this.openAfterSummonInfo();
                break;

            case TUTORIAL_KIND.EXE_FORMATION:
                this.exeFormation();
                break;

            case TUTORIAL_KIND.TUTORIAL3_MSG:
                this.openSvtEqInfo();
                break;
        }
    }

    public void quit()
    {
        this.summonBannerInfo.transform.localPosition = new Vector3(this.initBannerPos.x, this.initBannerPos.y, this.initBannerPos.z);
        this.clearBannerList();
        this.clearResultList();
        this.summonResultInfo.gameObject.SetActive(false);
        if (this.vaildGachaList != null)
        {
            this.vaildGachaList.Clear();
        }
        this.summonType = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GachaMaster>(DataNameKind.Kind.GACHA).getVaildPayType();
        TerminalPramsManager.SummonType = this.summonType;
        this.releaseBannerData();
    }

    private void reDispSummonBannerList()
    {
        if (!this.resetSummonVaildData() || this.checkMaxDrawNum())
        {
            this.deleteBannerList();
            this.checkValidGachaList();
            this.createSummonInfo();
        }
    }

    private void releaseBannerData()
    {
        if (this.bannerAssetData != null)
        {
            AssetManager.releaseAssetStorage(this.assetPath);
            this.bannerAssetData = null;
        }
    }

    public void requestGachaDraw()
    {
        this.maskObject.SetActive(true);
        GachaDrawRequest request = NetworkManager.getRequest<GachaDrawRequest>(new NetworkManager.ResultCallbackFunc(this.callbackGachaDraw));
        int gachaId = this.gachaParamData.gachaId;
        int gachaTime = this.gachaParamData.gachaTime;
        int warId = this.gachaParamData.warId;
        int ticketItemId = this.gachaParamData.ticketItemId;
        int shopIdIdx = this.gachaParamData.shopIdIdx;
        Debug.Log(string.Concat(new object[] { "--**-- GachaDrawRequest Param ID: ", gachaId, " _Time: ", gachaTime, " _WarId: ", warId, " _TicketId: ", ticketItemId, " _ShopIdIdx: ", shopIdIdx }));
        SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
        request.beginRequest(gachaId, gachaTime, warId, ticketItemId, shopIdIdx);
    }

    public void resetMainDisp()
    {
        SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.maskBgObject.SetActive(false);
        this.maskObject.SetActive(false);
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        this.titleInfo.setTitleInfo(this.fsm, true, null, TitleInfoControl.TitleKind.SUMMON);
        MainMenuBar.setMenuActive(true, null);
        this.titleInfo.setBackBtnSprite(true);
        this.setDispSummonInfo(true);
        this.setUserResourceDisp();
        this.getSvtIdx = 0;
        this.summonInfoCtr.setSummonDispInfo();
        MainMenuBar.UpdateNoticeNumber();
        if (this.tutorialKind == TUTORIAL_KIND.TUTORIAL2_MSG)
        {
            this.titleInfo.setBackBtnColliderEnable(false);
            this.progTutorial();
        }
        else
        {
            this.titleInfo.setBackBtnColliderEnable(true);
            this.reDispSummonBannerList();
        }
    }

    public void resetMaxSvtInfo()
    {
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.resetStoneInfo();
    }

    public void resetResultList()
    {
        if (!TutorialFlag.IsProgressDone(TutorialFlag.Progress._2))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTutorialArrowMark(null);
            this.summonResultInfo.ClearTouchBlocker();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.summonResultInfo.clearResultList();
            this.summonResultInfo.gameObject.SetActive(false);
            this.fsm.SendEvent("END_SUMMON");
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        });
    }

    public void resetStoneInfo()
    {
        this.setUserResourceDisp();
        this.summonInfoCtr.setSummonDispInfo();
    }

    private bool resetSummonVaildData()
    {
        bool flag = true;
        GachaEntity[] entityArray = this.gachaMst.getListValidData();
        if (this.useGachaData.Length != entityArray.Length)
        {
            flag = false;
        }
        else
        {
            for (int i = 0; i < this.useGachaData.Length; i++)
            {
                if (!this.useGachaData[i].Equals(entityArray[i]))
                {
                    flag = false;
                    break;
                }
            }
        }
        if (!flag)
        {
            this.useGachaData = entityArray;
        }
        return flag;
    }

    public void returnTutorial()
    {
        this.tutorialKind = TUTORIAL_KIND.TUTORIAL2_MSG;
        this.progTutorial();
    }

    private GameObject searchBannerImg(string searchTarget)
    {
        foreach (GameObject obj2 in this.bannerAssetData.GetObjectList<GameObject>())
        {
            UIAtlas component = obj2.GetComponent<UIAtlas>();
            if ((component != null) && (component.GetSprite(searchTarget) != null))
            {
                return obj2;
            }
        }
        return null;
    }

    private void setCenter()
    {
        Vector3[] worldCorners = this.indexPanel.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = this.indexPanel.transform.InverseTransformPoint(position);
        }
        this.center = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
        Debug.Log("**!! Center: " + this.center);
    }

    public void setChangeSummonInfo()
    {
        this.currentGachaInfo = this.vaildGachaList[this.currentIdx];
        this.setResourceInfo();
        this.summonInfoCtr.setSummonInfo(this.currentGachaInfo, new SummonInfoControl.ClickDelegate(this.exeSummon));
        this.summonInfoCtr.setAlphaSummonBtn();
    }

    public void SetDispBgParts(bool isDisp)
    {
        string[] strArray = new string[] { "ring_gard1_1", "ring_gard1_2", "ring_gard1_3", "ring_gard2_1", "ring_gard2_2", "ring_gard2_3", "center_glow" };
        foreach (string str in strArray)
        {
            this.bgRoot.transform.getNodeFromName(str, true).gameObject.SetActive(isDisp);
        }
    }

    public void setDispRePosition(int currentIdx)
    {
        int count = this.vaildGachaList.Count;
        int num2 = currentIdx;
        if ((count % 2) == 1)
        {
            if ((num2 != (count - 1)) && (num2 != 0))
            {
                num2++;
            }
            else if (num2 == (count - 1))
            {
                num2--;
            }
        }
        this.bannerLoopCtr.setScrollPos(currentIdx);
        this.setSliderIcon(currentIdx);
        this.currentIdx = currentIdx;
        this.currentMoveIdx = currentIdx;
        this.setChangeSummonInfo();
    }

    public void setDispSummonForm()
    {
    }

    private void setDispSummonInfo(bool isDisp)
    {
        this.titleInfo.gameObject.SetActive(isDisp);
        this.currentResourceInfo.SetActive(isDisp);
        this.detailInfo.SetActive(isDisp);
        this.summonBannerInfo.SetActive(isDisp);
    }

    public void setPositionByWarId()
    {
        if ((this.summonType > 0) && (this.bannerInfoList != null))
        {
            int count = this.bannerInfoList.Count;
            for (int i = 0; i < count; i++)
            {
                GachaBannerComponent component = this.bannerInfoList[i];
                VaildGachaInfo info = component.getBannerGachaInfo();
                if (info == null)
                {
                    this.setDispRePosition(1);
                    return;
                }
                if (info.type == this.summonType)
                {
                    int currentIdx = component.getBannerIdx();
                    this.setDispRePosition(currentIdx);
                    break;
                }
            }
        }
    }

    private void setResourceInfo()
    {
        this.currentGachaInfo = this.vaildGachaList[this.currentIdx];
        bool isPointSummon = this.currentGachaInfo.isPointSummon;
        UIWidget component = this.stoneNumInfo.GetComponent<UIWidget>();
        UIWidget widget2 = this.currentStoneNumLb.GetComponent<UIWidget>();
        UIWidget widget3 = this.pointNumInfo.GetComponent<UIWidget>();
        UIWidget widget4 = this.currentPointNumLb.GetComponent<UIWidget>();
        Color color = new Color(COLOR_VAL, COLOR_VAL, COLOR_VAL);
        if (isPointSummon)
        {
            component.color = color;
            widget2.color = color;
            widget3.color = Color.white;
            widget4.color = Color.white;
        }
        else
        {
            widget3.color = color;
            widget4.color = color;
            component.color = Color.white;
            widget2.color = Color.white;
        }
    }

    public void setResultListType()
    {
        this.resType = 1;
        this.summonResultInfo.setListByType(this.resType);
        this.showSummonResultInfo();
    }

    private void setSliderIcon(int idx)
    {
        int childCount = this.sliderGrid.transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            SelectBannerSliderIcon componentInChildren = this.sliderGrid.transform.GetChild(i).GetComponentInChildren<SelectBannerSliderIcon>();
            componentInChildren.setEnableOnImg(false);
            if (i == idx)
            {
                componentInChildren.setEnableOnImg(true);
            }
        }
    }

    public void setSummonData()
    {
        this.initBannerPos = this.summonBannerInfo.transform.localPosition;
        this.maskBgObject.SetActive(false);
        this.maskObject.SetActive(false);
        this.setDispSummonInfo(true);
        this.titleInfo.setTitleInfo(this.fsm, true, null, TitleInfoControl.TitleKind.SUMMON);
        this.titleInfo.setBackBtnSprite(true);
        this.titleInfo.setBackBtnDepth(0x16);
        this.currentIdx = 0;
        this.currentMoveIdx = 0;
        this.getSvtIdx = 0;
        this.isDoneTutorial = true;
        this.isDoneSecTutorial = true;
        this.assetPath = "SummonBanners/DownloadSummonBanner";
        this.summonType = TerminalPramsManager.SummonType;
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.setUserResourceDisp();
        this.gachaMst = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<GachaMaster>(DataNameKind.Kind.GACHA);
        this.useGachaData = this.gachaMst.getListValidData();
        this.checkValidGachaList();
        if (this.vaildGachaList.Count <= 0)
        {
            this.vaildGachaList = null;
        }
        this.summonBannerScrollView.ResetPosition();
        if (AssetManager.isExistAssetStorage(this.assetPath))
        {
            AssetManager.loadAssetStorage(this.assetPath, new AssetLoader.LoadEndDataHandler(this.LoadBannerEnd));
        }
        else
        {
            this.fsm.SendEvent("FAIL_LOAD");
        }
    }

    public void setSummonResultList()
    {
        if ((TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_GACHA_SCENE) && !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_GACHA_SVT_EQUIP)) && this.isSvtEqSummonResult())
        {
            if (<>f__am$cache4C == null)
            {
                <>f__am$cache4C = delegate {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.FIRST_EQUIP, TutorialFlag.Id.TUTORIAL_LABEL_GACHA_SVT_EQUIP, <>f__am$cache4C);
        }
        this.summonResultInfo.initGachaResultList(this.gachaResInfoList, this.resType);
    }

    private void setTutorialCtrEnable(bool isTutorial)
    {
        for (int i = 0; i < this.bannerInfoList.Count; i++)
        {
            this.bannerInfoList[i].setEnabledCollider(isTutorial);
        }
        this.leftArrowBtn.enabled = isTutorial;
        this.rightArrowBtn.enabled = isTutorial;
        this.summonInfoCtr.setTutorialBtnEnable(isTutorial);
    }

    private void setUseData(VaildGachaInfo info)
    {
        int priority = info.priority;
        VaildGachaInfo item = info;
        for (int i = 0; i < this.vaildGachaList.Count; i++)
        {
            VaildGachaInfo info3 = this.vaildGachaList[i];
            if (((info.id != info3.id) && (info.slotId == info3.slotId)) && (priority < info3.priority))
            {
                priority = info3.priority;
                item = info3;
            }
        }
        if (!this.useSummonList.Contains(item))
        {
            this.useSummonList.Add(item);
        }
    }

    private void setUserResourceDisp()
    {
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.currentStoneNumLb.text = $"{this.userGameEntity.stone:N0}";
        TblUserEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<TblUserMaster>(DataNameKind.Kind.TBL_USER_GAME).getUserData(this.userGameEntity.userId);
        this.currentPointNumLb.text = $"{entity.friendPoint:N0}";
    }

    private void setVaildInfoList(GachaEntity data)
    {
        VaildGachaInfo item = new VaildGachaInfo {
            id = data.id,
            name = data.name,
            beforeId = data.beforeGachaId,
            priority = data.priority,
            imgId = data.imageId,
            warId = data.warId,
            slotId = data.gachaSlot,
            type = data.type,
            ticketItemId = data.ticketItemId,
            isOpen = true,
            isPointSummon = (data.freeDrawFlag <= 0) ? false : true,
            detailUrl = data.detailUrl
        };
        this.vaildGachaList.Add(item);
    }

    public void showConfirmDlg()
    {
        PayType.Type gachaType = (PayType.Type) this.gachaParamData.gachaType;
        switch (gachaType)
        {
            case PayType.Type.STONE:
            {
                int afterStoneNum = this.haveStone - this.needStone;
                int haveFreeStone = this.haveFreeStone;
                int haveChargeStone = this.haveChargeStone;
                haveFreeStone -= this.needStone;
                if (haveFreeStone < 0)
                {
                    haveChargeStone += haveFreeStone;
                    haveFreeStone = 0;
                }
                this.confirmDlgInfo.OpenConfirmStone(gachaType, this.summonInfoCtr.getSummonPrice(), this.haveStone, this.haveFreeStone, this.haveChargeStone, afterStoneNum, haveFreeStone, haveChargeStone, new SummonConfirmDlgComponent.CallbackFunc(this.confirmResult));
                break;
            }
            case PayType.Type.FRIEND_POINT:
                this.isDailyGacha = this.summonInfoCtr.getIsFree();
                if (!this.isDailyGacha)
                {
                    int afterPointNum = this.friendPoint - this.gachaParamData.gachaResourceNum;
                    this.confirmDlgInfo.OpenConfirmPoint(this.friendPoint, this.needPoint, afterPointNum, new SummonConfirmDlgComponent.CallbackFunc(this.confirmResult));
                    break;
                }
                this.confirmDlgInfo.OpenConfirmFree(new SummonConfirmDlgComponent.CallbackFunc(this.confirmResult));
                break;

            case PayType.Type.TICKET:
                this.haveTicketNum = this.summonInfoCtr.getUsrTicketNum();
                this.afterTicketNum = this.haveTicketNum - this.gachaParamData.gachaTime;
                this.confirmDlgInfo.OpenConfirmTicket(this.haveTicketNum, this.afterTicketNum, new SummonConfirmDlgComponent.CallbackFunc(this.confirmResult));
                break;

            case PayType.Type.CHARGE_STONE:
            {
                int num4 = this.haveStone - this.needStone;
                int afterChargeStoneNum = this.haveChargeStone - this.needStone;
                if (afterChargeStoneNum <= 0)
                {
                    afterChargeStoneNum = 0;
                }
                this.confirmDlgInfo.OpenConfirmStone(gachaType, this.summonInfoCtr.getSummonPrice(), this.haveStone, this.haveFreeStone, this.haveChargeStone, num4, this.haveFreeStone, afterChargeStoneNum, new SummonConfirmDlgComponent.CallbackFunc(this.confirmResult));
                break;
            }
        }
    }

    public void showServantDialog()
    {
        if ((TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_GACHA_SCENE) && !TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_GACHA_SVT_EQUIP)) && this.isSvtEqSummonResult())
        {
            if (<>f__am$cache4B == null)
            {
                <>f__am$cache4B = delegate {
                };
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.FIRST_EQUIP, TutorialFlag.Id.TUTORIAL_LABEL_GACHA_SVT_EQUIP, <>f__am$cache4B);
        }
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.FIRST_SINGLE_GET, this.gachaResInfoList[0].userSvtId, new ServantStatusDialog.ClickDelegate(this.DialogCallBack));
    }

    public void showStonePurchase()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenStonePurchaseMenu(new StonePurchaseMenu.CallbackFunc(this.endPurchaseStone), null);
    }

    public void showSummonEffect()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
            this.maskObject.SetActive(false);
            this.SetDispBgParts(false);
            this.setDispSummonInfo(false);
            this.titleInfo.setBackBtnColliderEnable(false);
        });
        SoundManager.playBgm("BGM_SUMMON_1");
        this.summonComp = this.summonInstance.GetComponent<SummonEffectComponent>();
        this.summonComp.Initialize();
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        ServantLimitMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT);
        float num = UnityEngine.Random.value;
        if (this.gachaResInfoList != null)
        {
            int length = this.gachaResInfoList.Length;
            for (int i = 0; i < length; i++)
            {
                SummonEffectComponent.CardType oTHER;
                bool isRankup = false;
                GachaInfos infos = this.gachaResInfoList[i];
                ServantEntity entity = master.getEntityFromId<ServantEntity>(infos.objectId);
                int rarity = 1;
                if (entity.IsServant)
                {
                    rarity = master2.getEntityFromId<ServantLimitEntity>(infos.objectId, infos.limitCount).rarity;
                    if (rarity == 4)
                    {
                        float num5 = 0.4f;
                        isRankup = num < num5;
                    }
                    else if (rarity == 5)
                    {
                        float num6 = 0.2f;
                        isRankup = num < num6;
                    }
                }
                else
                {
                    rarity = master2.getEntityFromId<ServantLimitEntity>(infos.objectId, infos.limitCount).rarity;
                }
                bool isNewCard = false;
                SummonEffectComponent.NoticeEffect nONE = SummonEffectComponent.NoticeEffect.NONE;
                switch (entity.type)
                {
                    case 3:
                    case 7:
                        oTHER = SummonEffectComponent.CardType.OTHER;
                        break;

                    case 6:
                        isNewCard = infos.isNew;
                        oTHER = SummonEffectComponent.CardType.CANCER;
                        break;

                    case 8:
                        oTHER = SummonEffectComponent.CardType.CANCER;
                        break;

                    default:
                    {
                        oTHER = SummonEffectComponent.CardType.SERVANT;
                        isNewCard = infos.isNew;
                        WeightRate<int> rate = new WeightRate<int>();
                        switch (rarity)
                        {
                            case 4:
                                rate.setWeight(60, 0);
                                rate.setWeight(40, 1);
                                nONE = rate.getData(UnityEngine.Random.Range(0, rate.getTotalWeight()));
                                if (nONE != SummonEffectComponent.NoticeEffect.NONE)
                                {
                                    isRankup = false;
                                }
                                break;

                            case 5:
                                rate.setWeight(60, 0);
                                rate.setWeight(20, 1);
                                rate.setWeight(20, 2);
                                nONE = rate.getData(UnityEngine.Random.Range(0, rate.getTotalWeight()));
                                if (nONE != SummonEffectComponent.NoticeEffect.NONE)
                                {
                                    isRankup = false;
                                }
                                break;
                        }
                        break;
                    }
                }
                this.summonComp.AddSummonInfo(infos.objectId, infos.limitCount, isRankup, isNewCard, nONE, (Rarity.TYPE) rarity, oTHER);
            }
        }
    }

    public void showSummonResultInfo()
    {
        if (this.resType == 2)
        {
            this.titleInfo.setTitleInfo(this.fsm, false, null, TitleInfoControl.TitleKind.SUMMON);
        }
        else
        {
            this.maskBgObject.SetActive(false);
            this.titleInfo.setBackBtnColliderEnable(true);
            this.titleInfo.setTitleInfo(this.fsm, true, null, TitleInfoControl.TitleKind.SUMMON);
            this.titleInfo.setBackBtnSprite(true);
        }
        this.setUserResourceDisp();
        this.titleInfo.gameObject.SetActive(true);
        if (!this.summonResultInfo.gameObject.activeSelf)
        {
            this.summonResultInfo.gameObject.SetActive(true);
        }
    }

    public void showSvtEqFramePurchase()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipFramePurchaseMenu(new ServantEquipFramePurchaseMenu.CallbackFunc(this.endPurchaseSvtEqFrame), new System.Action(this.resetStoneInfo));
    }

    public void showSvtFramePurchase()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantFramePurchaseMenu(new ServantFramePurchaseMenu.CallbackFunc(this.endPurchaseSvtFrame), new System.Action(this.resetStoneInfo));
    }

    public void showSvtTalk()
    {
        Debug.Log("**  !! showSvtTalk: " + this.getSvtIdx);
        this.setDispSummonInfo(false);
        GachaInfos infos = this.gachaResInfoList[this.getSvtIdx];
        long userSvtId = infos.userSvtId;
        int objectId = infos.objectId;
        int limitCount = infos.limitCount;
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(objectId).IsServant)
        {
            if (this.summonResultInfo.gameObject.activeSelf)
            {
                this.summonResultInfo.gameObject.SetActive(false);
            }
            this.TalkSvtId = objectId;
            this.TalkSvtLimitCount = limitCount;
            ServantAssetLoadManager.preloadServant(objectId, limitCount);
            SingletonTemplate<clsQuestCheck>.Instance.mfInit();
            SingletonTemplate<clsQuestCheck>.Instance.PlayGacha(userSvtId, objectId, limitCount, new System.Action(this.close));
        }
        else
        {
            this.fsm.SendEvent("CLICK_OK");
        }
    }

    private int SlotCompare(VaildGachaInfo a, VaildGachaInfo b)
    {
        if (b.slotId < a.slotId)
        {
            return -1;
        }
        if (b.slotId > a.slotId)
        {
            return 1;
        }
        if (b.id < a.id)
        {
            return -1;
        }
        if (b.id > a.id)
        {
            return 1;
        }
        return 0;
    }

    [DebuggerHidden]
    protected IEnumerator WaitBattleChrLoad() => 
        new <WaitBattleChrLoad>c__Iterator37 { <>f__this = this };

    [CompilerGenerated]
    private sealed class <WaitBattleChrLoad>c__Iterator37 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal SummonControl <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                case 1:
                    if (ServantAssetLoadManager.checkLoad())
                    {
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 1;
                        return true;
                    }
                    ServantAssetLoadManager.unloadServant(this.<>f__this.TalkSvtId, this.<>f__this.TalkSvtLimitCount);
                    this.<>f__this.fsm.SendEvent("CLICK_OK");
                    this.$PC = -1;
                    break;
            }
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    public enum GACHATYPE
    {
        CHARGE_STONE = 7,
        FREEGACHA = 3,
        PAYGACHA = 1,
        TICKETGACHA = 5
    }

    protected enum QUESTTYPE
    {
        FRIENDSHIP = 3,
        HEROBALLAD = 6
    }

    public class resData
    {
        public GachaInfos[] gachaInfos;
    }

    public enum TUTORIAL_KIND
    {
        NONE,
        TUTORIAL1_MSG,
        TUTORIAL2_MSG,
        EXE_FORMATION,
        TUTORIAL3_MSG,
        SVTEQ_TUTORIAL_MSG
    }
}

