using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CommonUI : SingletonMonoBehaviour<CommonUI>
{
    [SerializeField]
    protected AllocMem allocMem;
    [SerializeField]
    protected GameObject apRecoverItemListDlgPrefab;
    protected ApRecoverDlgComponent apRecvDlgComp;
    [SerializeField]
    protected Transform baseMount;
    [SerializeField]
    protected UISprite bottomImg;
    protected ClassCompatibilityMenu classCompatibilityMenu;
    [SerializeField]
    protected GameObject classCompatibilityMenuPrefab;
    protected System.Action closeClassCompatibilityMenuCallback;
    protected System.Action closeEquipGraphListMenuCallback;
    protected System.Action closeNotificationCallback;
    protected System.Action closeServantEquipStatusDialogCallback;
    protected System.Action closeServantFilterSelectMenuCallback;
    protected System.Action closeServantSortSelectMenuCallback;
    protected System.Action closeServantStatusDialogCallback;
    protected System.Action closeSupportServantEquipListMenuCallback;
    protected CombineResultEffectComponent combineResEffectComp;
    [SerializeField]
    protected GameObject combineResEffectPrefab;
    [SerializeField]
    protected CommonConfirmDialog commonConfirmDialog;
    [SerializeField]
    protected ConnectMark connectMark;
    [SerializeField]
    protected DetailInfoDialog detailInfoDialog;
    protected EquipGraphListMenu equipGraphListMenu;
    [SerializeField]
    protected GameObject equipGraphListMenuPrefab;
    [SerializeField]
    protected ErrorDialog errorDialog;
    protected ItemDetailInfoComponent itemDetailDlgComp;
    [SerializeField]
    protected GameObject itemDetailDlgPrefab;
    [SerializeField]
    protected UISprite leftImg;
    protected LogoMain logoMain;
    protected System.Action logoMainCallback;
    [SerializeField]
    protected GameObject logoPrefab;
    [SerializeField]
    protected MaskFade maskFade;
    [SerializeField]
    protected GameObject missionNotifyPrefab;
    [SerializeField]
    protected GameObject missionNotifyRoot;
    private LoginResultData mLoginResult;
    [SerializeField]
    protected NotificationDialog notifiDialog;
    [SerializeField]
    protected PopupMessageDialog popupMessageDialog;
    protected PresentBoxNotificationMenu presentBoxNotificationMenu;
    [SerializeField]
    protected GameObject presentBoxNotificationMenuPrefab;
    [SerializeField]
    protected GameObject questClearRewardWindowPrefab;
    [SerializeField]
    protected ErrorDialog retryBootDialog;
    [SerializeField]
    protected ErrorDialog retryDialog;
    [SerializeField]
    protected UISprite rightImg;
    protected ServantEquipFramePurchaseMenu servantEquipFramePurchaseMenu;
    [SerializeField]
    protected GameObject servantEquipFramePurchaseMenuPrefab;
    protected ServantStatusDialog servantEquipStatusDialog;
    [SerializeField]
    protected GameObject servantEquipStatusDialogPrefab;
    protected ServantFilterSelectMenu servantFilterSelectMenu;
    [SerializeField]
    protected GameObject servantFilterSelectMenuPrefab;
    protected ServantFramePurchaseMenu servantFramePurchaseMenu;
    [SerializeField]
    protected GameObject servantFramePurchaseMenuPrefab;
    [SerializeField]
    protected GameObject servantFrameShortDlgPrefab;
    protected ServantSortSelectMenu servantSortSelectMenu;
    [SerializeField]
    protected GameObject servantSortSelectMenuPrefab;
    protected ServantStatusDialog servantStatusDialog;
    [SerializeField]
    protected GameObject servantStatusDialogPrefab;
    protected StonePurchaseMenu stonePurchaseMenu;
    [SerializeField]
    protected GameObject stonePurchaseMenuPrefab;
    [SerializeField]
    protected StonePurchaseNotificationMenu stonePurchaseNotificationMenu;
    [SerializeField]
    protected GameObject supportSelectEquipListMenuPrefab;
    protected SupportServantEquipListMenu supportServantEquipListMenu;
    protected ServantFrameShortDlgComponent svtFrameShortDialog;
    [SerializeField]
    protected UISprite topImg;
    protected TripleButtonDlgComponent tripleButtonDialog;
    [SerializeField]
    protected GameObject tripleButtonDlgPrefab;
    [SerializeField]
    protected TutorialArrowMenu tutorialArrowMenu;
    [SerializeField]
    protected TutorialBigDialog tutorialBigDialog;
    [SerializeField]
    protected TutorialNotificationDialog tutorialNotificationDialog;
    [SerializeField]
    protected TutorialNotificationMessage tutorialNotificationMessage;
    protected UserGameActRecoverMenu userGameActRecoverMenu;
    [SerializeField]
    protected GameObject userGameActRecoverMenuPrefab;
    protected UserNameEntryComponent userNameEntry;
    [SerializeField]
    protected GameObject userNameEntryPrefab;
    [SerializeField]
    protected GameObject userPresentBoxWindowPrefab;
    protected UserStatusComponet userStatusComp;
    [SerializeField]
    protected GameObject userStatusPrefab;
    protected UserPresentBoxWindow usrPresentListWindow;
    [SerializeField]
    protected GameObject warClearRewardWindowPrefab;
    [SerializeField]
    protected ErrorDialog warningDialog;

    public void ClearLoginResultData()
    {
        this.mLoginResult = null;
    }

    public void CloseApRecoverItemListDialog()
    {
        if (this.apRecvDlgComp != null)
        {
            this.apRecvDlgComp.Close(delegate {
                UnityEngine.Object.Destroy(this.apRecvDlgComp.gameObject);
                this.apRecvDlgComp = null;
            });
        }
    }

    public void CloseClassCompatibilityMenu(System.Action callback = null)
    {
        if (this.classCompatibilityMenu != null)
        {
            this.closeClassCompatibilityMenuCallback = callback;
            this.classCompatibilityMenu.Close(new System.Action(this.EndCloseClassCompatibilityMenu));
        }
    }

    public void CloseCombineResult()
    {
        if (this.combineResEffectComp != null)
        {
            this.combineResEffectComp.Close();
            UnityEngine.Object.Destroy(this.combineResEffectComp.gameObject);
            this.combineResEffectComp = null;
        }
    }

    public void CloseConfirmDialog()
    {
        this.commonConfirmDialog.Close();
    }

    public void CloseConfirmDialog(System.Action callback)
    {
        this.commonConfirmDialog.Close(callback);
    }

    public void CloseEquipGraphListMenu(System.Action callback)
    {
        if (this.equipGraphListMenu != null)
        {
            this.closeEquipGraphListMenuCallback = callback;
            this.equipGraphListMenu.Close(new System.Action(this.EndCloseEquipGraphListMenu));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    public void CloseItemDetailDialog()
    {
        if (this.itemDetailDlgComp != null)
        {
            this.itemDetailDlgComp.Close(delegate {
                UnityEngine.Object.Destroy(this.itemDetailDlgComp.gameObject);
                this.itemDetailDlgComp = null;
            });
        }
    }

    public void CloseNotificationDialog()
    {
        this.notifiDialog.Close();
    }

    public void CloseNotificationDialog(System.Action callback)
    {
        this.notifiDialog.Close(callback);
    }

    public void ClosePresentBoxNotificationMenu()
    {
        if (this.presentBoxNotificationMenu != null)
        {
            this.presentBoxNotificationMenu.Close(new System.Action(this.EndClosePresentBoxNotificationMenu));
        }
    }

    public void CloseServantEquipFramePurchaseMenu()
    {
        if (this.servantEquipFramePurchaseMenu != null)
        {
            this.servantEquipFramePurchaseMenu.Close();
            UnityEngine.Object.Destroy(this.servantEquipFramePurchaseMenu.gameObject);
            this.servantEquipFramePurchaseMenu = null;
        }
    }

    public void CloseServantEquipStatusDialog(System.Action callback)
    {
        if (this.servantEquipStatusDialog != null)
        {
            this.closeServantEquipStatusDialogCallback = callback;
            this.servantEquipStatusDialog.Close(new System.Action(this.EndCloseServantEquipStatusDialog));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    public void CloseServantFilterSelectMenu(System.Action callback)
    {
        if (this.servantFilterSelectMenu != null)
        {
            this.closeServantFilterSelectMenuCallback = callback;
            this.servantFilterSelectMenu.Close(new System.Action(this.EndCloseServantFilterSelectMenu));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    public void CloseServantFramePurchaseMenu()
    {
        if (this.servantFramePurchaseMenu != null)
        {
            this.servantFramePurchaseMenu.Close();
            UnityEngine.Object.Destroy(this.servantFramePurchaseMenu.gameObject);
            this.servantFramePurchaseMenu = null;
        }
    }

    public void CloseServantSortSelectMenu(System.Action callback)
    {
        if (this.servantSortSelectMenu != null)
        {
            this.closeServantSortSelectMenuCallback = callback;
            this.servantSortSelectMenu.Close(new System.Action(this.EndCloseServantSortSelectMenu));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    public void CloseServantStatusDialog(System.Action callback)
    {
        if (this.servantStatusDialog != null)
        {
            this.closeServantStatusDialogCallback = callback;
            this.servantStatusDialog.Close(new System.Action(this.EndCloseServantStatusDialog));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    public void CloseStonePurchaseMenu()
    {
        if (this.stonePurchaseMenu != null)
        {
            this.stonePurchaseMenu.Close(new System.Action(this.EndCloseStonePurchaseMenu));
        }
    }

    public void CloseSupportServantEquipListMenu(System.Action callback)
    {
        if (this.supportServantEquipListMenu != null)
        {
            this.closeSupportServantEquipListMenuCallback = callback;
            this.supportServantEquipListMenu.Close(new System.Action(this.EndCloseSupportServantEquipListMenu));
        }
        else if (callback != null)
        {
            callback();
        }
    }

    public void CloseSvtFrameShortDlg(System.Action closed_act)
    {
        <CloseSvtFrameShortDlg>c__AnonStorey4B storeyb = new <CloseSvtFrameShortDlg>c__AnonStorey4B {
            closed_act = closed_act,
            <>f__this = this
        };
        if (this.svtFrameShortDialog != null)
        {
            this.svtFrameShortDialog.Close(new System.Action(storeyb.<>m__A));
        }
    }

    public void CloseTripleButtonDlg(System.Action closed_act)
    {
        <CloseTripleButtonDlg>c__AnonStorey4A storeya = new <CloseTripleButtonDlg>c__AnonStorey4A {
            closed_act = closed_act,
            <>f__this = this
        };
        if (this.tripleButtonDialog != null)
        {
            this.tripleButtonDialog.Close(new System.Action(storeya.<>m__9));
        }
    }

    public void CloseTutorialArrowMark(System.Action callback)
    {
        this.tutorialArrowMenu.Close(callback);
    }

    public void CloseTutorialNotificationDialogArrow(System.Action callbackFunc = null)
    {
        <CloseTutorialNotificationDialogArrow>c__AnonStorey49 storey = new <CloseTutorialNotificationDialogArrow>c__AnonStorey49 {
            callbackFunc = callbackFunc,
            isClose = false
        };
        this.tutorialNotificationMessage.Close(new System.Action(storey.<>m__7));
        this.CloseTutorialArrowMark(new System.Action(storey.<>m__8));
    }

    public void CloseUserGameActRecoverMenu()
    {
        if (this.userGameActRecoverMenu != null)
        {
            this.userGameActRecoverMenu.Close();
            UnityEngine.Object.Destroy(this.userGameActRecoverMenu.gameObject);
            this.userGameActRecoverMenu = null;
        }
    }

    protected void CloseUserNameEntry()
    {
        if (this.userNameEntry != null)
        {
            UnityEngine.Object.Destroy(this.userNameEntry.gameObject);
            this.userNameEntry = null;
        }
    }

    public void CloseUsrPresentList()
    {
        if (this.usrPresentListWindow != null)
        {
            this.usrPresentListWindow.Close();
            UnityEngine.Object.Destroy(this.usrPresentListWindow.gameObject);
            this.usrPresentListWindow = null;
        }
    }

    protected GameObject CreateMenu(GameObject prefab)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
        Transform transform = obj2.transform;
        Vector3 localPosition = obj2.transform.localPosition;
        Vector3 localScale = obj2.transform.localScale;
        transform.parent = this.baseMount;
        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        return obj2;
    }

    public GameObject CreateMissionNotify()
    {
        GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.missionNotifyPrefab);
        self.SafeSetParent(this.missionNotifyRoot);
        return self;
    }

    protected void EndBuyBankItemNotificationRecive()
    {
        this.stonePurchaseNotificationMenu.Close();
    }

    protected void EndCloseClassCompatibilityMenu()
    {
        if (this.classCompatibilityMenu != null)
        {
            UnityEngine.Object.Destroy(this.classCompatibilityMenu.gameObject);
            this.classCompatibilityMenu = null;
            System.Action closeClassCompatibilityMenuCallback = this.closeClassCompatibilityMenuCallback;
            if (closeClassCompatibilityMenuCallback != null)
            {
                this.closeClassCompatibilityMenuCallback = null;
                closeClassCompatibilityMenuCallback();
            }
        }
    }

    protected void EndCloseEquipGraphListMenu()
    {
        if (this.equipGraphListMenu != null)
        {
            UnityEngine.Object.Destroy(this.equipGraphListMenu.gameObject);
            this.equipGraphListMenu = null;
        }
        System.Action closeEquipGraphListMenuCallback = this.closeEquipGraphListMenuCallback;
        if (closeEquipGraphListMenuCallback != null)
        {
            this.closeEquipGraphListMenuCallback = null;
            closeEquipGraphListMenuCallback();
        }
    }

    protected void EndClosePresentBoxNotificationMenu()
    {
        if (this.presentBoxNotificationMenu != null)
        {
            UnityEngine.Object.Destroy(this.presentBoxNotificationMenu.gameObject);
            this.presentBoxNotificationMenu = null;
        }
    }

    protected void EndCloseServantEquipStatusDialog()
    {
        if (this.servantEquipStatusDialog != null)
        {
            UnityEngine.Object.Destroy(this.servantEquipStatusDialog.gameObject);
            this.servantEquipStatusDialog = null;
        }
        System.Action closeServantEquipStatusDialogCallback = this.closeServantEquipStatusDialogCallback;
        if (closeServantEquipStatusDialogCallback != null)
        {
            this.closeServantEquipStatusDialogCallback = null;
            closeServantEquipStatusDialogCallback();
        }
    }

    protected void EndCloseServantFilterSelectMenu()
    {
        if (this.servantFilterSelectMenu != null)
        {
            UnityEngine.Object.Destroy(this.servantFilterSelectMenu.gameObject);
            this.servantFilterSelectMenu = null;
        }
        System.Action closeServantFilterSelectMenuCallback = this.closeServantFilterSelectMenuCallback;
        if (closeServantFilterSelectMenuCallback != null)
        {
            this.closeServantFilterSelectMenuCallback = null;
            closeServantFilterSelectMenuCallback();
        }
    }

    protected void EndCloseServantSortSelectMenu()
    {
        if (this.servantSortSelectMenu != null)
        {
            UnityEngine.Object.Destroy(this.servantSortSelectMenu.gameObject);
            this.servantSortSelectMenu = null;
        }
        System.Action closeServantSortSelectMenuCallback = this.closeServantSortSelectMenuCallback;
        if (closeServantSortSelectMenuCallback != null)
        {
            this.closeServantSortSelectMenuCallback = null;
            closeServantSortSelectMenuCallback();
        }
    }

    protected void EndCloseServantStatusDialog()
    {
        if (this.servantStatusDialog != null)
        {
            UnityEngine.Object.Destroy(this.servantStatusDialog.gameObject);
            this.servantStatusDialog = null;
        }
        System.Action closeServantStatusDialogCallback = this.closeServantStatusDialogCallback;
        if (closeServantStatusDialogCallback != null)
        {
            this.closeServantStatusDialogCallback = null;
            closeServantStatusDialogCallback();
        }
    }

    protected void EndCloseStonePurchaseMenu()
    {
        if (this.stonePurchaseMenu != null)
        {
            UnityEngine.Object.Destroy(this.stonePurchaseMenu.gameObject);
            this.stonePurchaseMenu = null;
        }
    }

    protected void EndCloseSupportServantEquipListMenu()
    {
        if (this.supportServantEquipListMenu != null)
        {
            UnityEngine.Object.Destroy(this.supportServantEquipListMenu.gameObject);
            this.supportServantEquipListMenu = null;
        }
        System.Action closeSupportServantEquipListMenuCallback = this.closeSupportServantEquipListMenuCallback;
        if (closeSupportServantEquipListMenuCallback != null)
        {
            this.closeSupportServantEquipListMenuCallback = null;
            closeSupportServantEquipListMenuCallback();
        }
    }

    protected void EndPlayLogo()
    {
        if (this.logoMain != null)
        {
            this.logoMain.Quit();
            UnityEngine.Object.Destroy(this.logoMain.gameObject);
            this.logoMain = null;
        }
        if (this.logoMainCallback != null)
        {
            this.logoMainCallback();
        }
    }

    public static ListViewSort GetServantFilterInfo() => 
        ServantFilterSelectMenu.GetServantFilterInfo();

    public static ListViewSort GetServantSortInfo() => 
        ServantSortSelectMenu.GetServantSortInfo();

    public void HideUserStatus()
    {
        if (this.userStatusComp != null)
        {
            this.userStatusComp.HideUserStatus();
        }
    }

    public void InitTurotialArrowMark()
    {
        this.tutorialArrowMenu.Init();
    }

    public bool IsActive_ApRecvDlgComp() => 
        (this.apRecvDlgComp != null);

    public bool IsActive_UserPresentBoxWindow() => 
        (this.usrPresentListWindow != null);

    public bool IsBusyLoad() => 
        this.connectMark.IsBusy();

    public bool maskFadein(float duration, System.Action callback = null) => 
        this.maskFade.Fadein(duration, callback);

    public void maskFadeInit()
    {
        this.maskFade.Init();
    }

    public bool maskFadeIsBusy() => 
        this.maskFade.IsBusy();

    public bool maskFadeout(MaskFade.Kind kind, float duration, System.Action callback = null) => 
        this.maskFade.Fadeout(kind, duration, callback);

    public MaskFade.Kind maskFadGetFadeoutKind() => 
        this.maskFade.GetFadeoutKind();

    protected void OnEndErrorDialog(bool isDecide)
    {
        this.errorDialog.Close();
    }

    protected void OnEndRetryBootDialog(bool isDecide)
    {
        this.retryBootDialog.Close();
    }

    protected void OnEndRetryDialog(bool isDecide)
    {
        this.retryDialog.Close();
    }

    protected void OnEndWarningDialog(bool isDecide)
    {
        this.warningDialog.Close();
    }

    public void OpenApRecoverItemListDialog(int needAp, ApRecoverDlgComponent.CallbackFunc callback)
    {
        if (this.apRecvDlgComp == null)
        {
            this.apRecvDlgComp = this.CreateMenu(this.apRecoverItemListDlgPrefab).GetComponent<ApRecoverDlgComponent>();
        }
        this.apRecvDlgComp.OpenApRecvItemDlg(needAp, callback);
    }

    public void OpenClassCompatibilityMenu(System.Action callback = null)
    {
        this.OpenClassCompatibilityMenu(0, 0, callback);
    }

    public void OpenClassCompatibilityMenu(int questId, int questPhase, System.Action callback = null)
    {
        if (this.classCompatibilityMenu == null)
        {
            this.classCompatibilityMenu = this.CreateMenu(this.classCompatibilityMenuPrefab).GetComponent<ClassCompatibilityMenu>();
        }
        this.classCompatibilityMenu.Open(questId, questPhase, callback);
    }

    public void OpenConfirmDecideDlg(string title, string message, string decideTxt, string cancleTxt, CommonConfirmDialog.ClickDelegate func)
    {
        this.commonConfirmDialog.OpenDecideDlg(title, message, decideTxt, cancleTxt, func);
    }

    public void OpenConfirmDialog(string title, string message, CommonConfirmDialog.ClickDelegate func)
    {
        this.commonConfirmDialog.Open(title, message, func);
    }

    public void OpenConfirmDialog(string title, string message, string decideTxt, string cancleTxt, CommonConfirmDialog.ClickDelegate func)
    {
        this.commonConfirmDialog.Open(title, message, decideTxt, cancleTxt, func);
    }

    public void OpenDetailInfoDialog(string name, string info, string detail)
    {
        this.detailInfoDialog.Open(name, info, detail);
    }

    public void OpenDetailLongInfoDialog(string name, string info, string detail)
    {
        this.detailInfoDialog.OpenWithLongInfo(name, info, detail);
    }

    public void OpenEquipGraphListMenu(PartyListViewItem partyItem, int member, EquipGraphListMenu.CallbackFunc callback)
    {
        if (this.equipGraphListMenu == null)
        {
            this.equipGraphListMenu = this.CreateMenu(this.equipGraphListMenuPrefab).GetComponent<EquipGraphListMenu>();
        }
        this.equipGraphListMenu.Open(partyItem, member, callback);
    }

    public void OpenErrorDialog(string title, string message, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock = false)
    {
        base.StartCoroutine(this.OpenErrorDialogCR(title, message, func, isNetwrokRebootBlock));
    }

    [DebuggerHidden]
    protected IEnumerator OpenErrorDialogCR(string title, string message, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock) => 
        new <OpenErrorDialogCR>c__Iterator4 { 
            title = title,
            message = message,
            isNetwrokRebootBlock = isNetwrokRebootBlock,
            func = func,
            <$>title = title,
            <$>message = message,
            <$>isNetwrokRebootBlock = isNetwrokRebootBlock,
            <$>func = func,
            <>f__this = this
        };

    public void OpenFriendshipUp(UserServantEntity usrSvtData, int oldFriendShipRank, CombineResultEffectComponent.ClickDelegate callback)
    {
        <OpenFriendshipUp>c__AnonStorey51 storey = new <OpenFriendshipUp>c__AnonStorey51 {
            callback = callback,
            <>f__this = this
        };
        if (this.combineResEffectComp == null)
        {
            this.combineResEffectComp = this.CreateMenu(this.combineResEffectPrefab).GetComponent<CombineResultEffectComponent>();
        }
        this.combineResEffectComp.SetFriendshipUpInfo(usrSvtData, oldFriendShipRank, new CombineResultEffectComponent.ClickDelegate(storey.<>m__10));
    }

    public void OpenImageDialogWithAssets(string[] images, System.Action callback)
    {
        this.tutorialBigDialog.OpenWithAssets(images, callback);
    }

    public void OpenItemDetailDialog(ItemEntity itemData, ItemDetailInfoComponent.CallbackFunc callback)
    {
        if (this.itemDetailDlgComp == null)
        {
            this.itemDetailDlgComp = this.CreateMenu(this.itemDetailDlgPrefab).GetComponent<ItemDetailInfoComponent>();
        }
        this.itemDetailDlgComp.Open(itemData, callback);
    }

    public void OpenItemDetailDialog(string name, string msg, ItemDetailInfoComponent.CallbackFunc callback)
    {
        if (this.itemDetailDlgComp == null)
        {
            this.itemDetailDlgComp = this.CreateMenu(this.itemDetailDlgPrefab).GetComponent<ItemDetailInfoComponent>();
        }
        this.itemDetailDlgComp.OpenItemMsgInfo(name, msg, callback);
    }

    public void OpenLimitUpCombineResult(CombineResultEffectComponent.Kind kind, UserServantEntity usrSvtData, CombineResultEffectComponent.ClickDelegate callback)
    {
        this.OpenLimitUpCombineResult(kind, usrSvtData, -1, callback);
    }

    public void OpenLimitUpCombineResult(CombineResultEffectComponent.Kind kind, UserServantEntity usrSvtData, int baseCollectionLimitCnt, CombineResultEffectComponent.ClickDelegate callback)
    {
        if (this.combineResEffectComp == null)
        {
            this.combineResEffectComp = this.CreateMenu(this.combineResEffectPrefab).GetComponent<CombineResultEffectComponent>();
        }
        this.combineResEffectComp.SetLimitUpCombineInfo(kind, usrSvtData, baseCollectionLimitCnt, callback);
    }

    public void OpenNobleCombineResult(CombineResultEffectComponent.Kind kind, UserServantEntity usrSvtData, int targetId, int targetLv, CombineResultEffectComponent.ClickDelegate callback, int targetIdOld = 0, int targetLvOld = 0)
    {
        if (this.combineResEffectComp == null)
        {
            this.combineResEffectComp = this.CreateMenu(this.combineResEffectPrefab).GetComponent<CombineResultEffectComponent>();
        }
        this.combineResEffectComp.SetNobleCombineInfo(kind, usrSvtData, targetId, targetLv, callback, targetIdOld, targetLvOld);
    }

    public void OpenNotificationDialog(string title, string message, NotificationDialog.ClickDelegate func, int panel_depth = -1)
    {
        this.notifiDialog.Open(title, message, func, panel_depth);
    }

    public void OpenNotificationDialog(string title, string message, System.Action func, int panel_depth = -1)
    {
        this.closeNotificationCallback = func;
        this.notifiDialog.Open(title, message, new NotificationDialog.ClickDelegate(this.SelectNotificationDialog), panel_depth);
    }

    public void OpenPopupMessageDialog(string message)
    {
        this.popupMessageDialog.Open(message);
    }

    public void OpenPowerUp(CombineResultEffectComponent.ClickDelegate callback)
    {
        UserServantEntity usrSvtData = null;
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT);
        ServantMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        foreach (UserServantEntity entity2 in master.getList())
        {
            if (master2.getEntityFromId<ServantEntity>(entity2.getSvtId()).checkIsHeroineSvt())
            {
                usrSvtData = master.getEntityFromId<UserServantEntity>(entity2.id);
                break;
            }
        }
        if (usrSvtData != null)
        {
            this.OpenPowerUp(usrSvtData, callback);
        }
    }

    public void OpenPowerUp(UserServantEntity usrSvtData, CombineResultEffectComponent.ClickDelegate callback)
    {
        <OpenPowerUp>c__AnonStorey52 storey = new <OpenPowerUp>c__AnonStorey52 {
            callback = callback,
            <>f__this = this
        };
        if (this.combineResEffectComp == null)
        {
            this.combineResEffectComp = this.CreateMenu(this.combineResEffectPrefab).GetComponent<CombineResultEffectComponent>();
        }
        this.combineResEffectComp.SetPowerUpInfo(usrSvtData, new CombineResultEffectComponent.ClickDelegate(storey.<>m__11));
    }

    public void OpenPresentBoxNotificationMenu(UserPresentBoxEntity[] presentList, PresentBoxNotificationMenu.CallbackFunc callback)
    {
        if (this.presentBoxNotificationMenu == null)
        {
            this.presentBoxNotificationMenu = this.CreateMenu(this.presentBoxNotificationMenuPrefab).GetComponent<PresentBoxNotificationMenu>();
        }
        this.presentBoxNotificationMenu.Open(presentList, callback);
    }

    public void OpenRetryBootDialog(string title, string message, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock = false)
    {
        base.StartCoroutine(this.OpenRetryBootDialogCR(title, message, func, isNetwrokRebootBlock));
    }

    [DebuggerHidden]
    protected IEnumerator OpenRetryBootDialogCR(string title, string message, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock) => 
        new <OpenRetryBootDialogCR>c__Iterator6 { 
            title = title,
            message = message,
            isNetwrokRebootBlock = isNetwrokRebootBlock,
            func = func,
            <$>title = title,
            <$>message = message,
            <$>isNetwrokRebootBlock = isNetwrokRebootBlock,
            <$>func = func,
            <>f__this = this
        };

    public void OpenRetryDialog(string title, string message, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock = false)
    {
        base.StartCoroutine(this.OpenRetryDialogCR(title, message, null, null, func, isNetwrokRebootBlock));
    }

    public void OpenRetryDialog(string title, string message, string decideTxt, string cancleTxt, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock = false)
    {
        base.StartCoroutine(this.OpenRetryDialogCR(title, message, decideTxt, cancleTxt, func, isNetwrokRebootBlock));
    }

    [DebuggerHidden]
    protected IEnumerator OpenRetryDialogCR(string title, string message, string decideTxt, string cancleTxt, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock) => 
        new <OpenRetryDialogCR>c__Iterator5 { 
            title = title,
            message = message,
            isNetwrokRebootBlock = isNetwrokRebootBlock,
            func = func,
            decideTxt = decideTxt,
            cancleTxt = cancleTxt,
            <$>title = title,
            <$>message = message,
            <$>isNetwrokRebootBlock = isNetwrokRebootBlock,
            <$>func = func,
            <$>decideTxt = decideTxt,
            <$>cancleTxt = cancleTxt,
            <>f__this = this
        };

    public void OpenServantCombineResult(CombineResultEffectComponent.Kind kind, int infoIdx, UserServantEntity usrSvtData, int baseCollectionLv, CombineResultEffectComponent.ClickDelegate callback)
    {
        if (this.combineResEffectComp == null)
        {
            this.combineResEffectComp = this.CreateMenu(this.combineResEffectPrefab).GetComponent<CombineResultEffectComponent>();
        }
        this.combineResEffectComp.SetSvtCombineInfo(kind, infoIdx, usrSvtData, baseCollectionLv, callback);
    }

    public void OpenServantEquipFramePurchaseMenu(ServantEquipFramePurchaseMenu.CallbackFunc callback, System.Action refreshCallback = null)
    {
        if (this.servantEquipFramePurchaseMenu == null)
        {
            this.servantEquipFramePurchaseMenu = this.CreateMenu(this.servantEquipFramePurchaseMenuPrefab).GetComponent<ServantEquipFramePurchaseMenu>();
        }
        this.servantEquipFramePurchaseMenu.Open(callback, refreshCallback);
    }

    public void OpenServantEquipStatusDialog(ServantStatusDialog.Kind kind, EquipTargetInfo equipTargetInfo, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantEquipStatusDialog == null)
        {
            this.servantEquipStatusDialog = this.CreateMenu(this.servantEquipStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantEquipStatusDialog.Open(kind, equipTargetInfo, callback);
    }

    public void OpenServantEquipStatusDialog(ServantStatusDialog.Kind kind, int svtId, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantEquipStatusDialog == null)
        {
            this.servantEquipStatusDialog = this.CreateMenu(this.servantEquipStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        EquipTargetInfo equipTargetInfo = new EquipTargetInfo(svtId);
        this.servantEquipStatusDialog.Open(kind, equipTargetInfo, callback);
    }

    public void OpenServantEquipStatusDialog(ServantStatusDialog.Kind kind, long userSvtId, bool isUse, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantEquipStatusDialog == null)
        {
            this.servantEquipStatusDialog = this.CreateMenu(this.servantEquipStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantEquipStatusDialog.Open(kind, userSvtId, isUse, callback);
    }

    public void OpenServantEquipStatusDialog(ServantStatusDialog.Kind kind, UserServantEntity userSvtEntity, bool isUse, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantEquipStatusDialog == null)
        {
            this.servantEquipStatusDialog = this.CreateMenu(this.servantEquipStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantEquipStatusDialog.Open(kind, userSvtEntity, isUse, callback);
    }

    public void OpenServantFilterSelectMenu(ServantFilterSelectMenu.Kind kind, ListViewSort sort, ServantFilterSelectMenu.CallbackFunc callback)
    {
        if (this.servantFilterSelectMenu == null)
        {
            this.servantFilterSelectMenu = this.CreateMenu(this.servantFilterSelectMenuPrefab).GetComponent<ServantFilterSelectMenu>();
        }
        this.servantFilterSelectMenu.Open(kind, sort, callback);
    }

    public void OpenServantFramePurchaseMenu(ServantFramePurchaseMenu.CallbackFunc callback, System.Action refreshCallback = null)
    {
        if (this.servantFramePurchaseMenu == null)
        {
            this.servantFramePurchaseMenu = this.CreateMenu(this.servantFramePurchaseMenuPrefab).GetComponent<ServantFramePurchaseMenu>();
        }
        this.servantFramePurchaseMenu.Open(callback, refreshCallback);
    }

    public void OpenServantSortSelectMenu(ServantSortSelectMenu.Kind kind, ListViewSort sort, ServantSortSelectMenu.CallbackFunc callback)
    {
        if (this.servantSortSelectMenu == null)
        {
            this.servantSortSelectMenu = this.CreateMenu(this.servantSortSelectMenuPrefab).GetComponent<ServantSortSelectMenu>();
        }
        this.servantSortSelectMenu.Open(kind, sort, callback);
    }

    public void OpenServantStatusDialog(ServantStatusDialog.Kind kind, ServantLeaderInfo servantLeaderInfo, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantStatusDialog == null)
        {
            this.servantStatusDialog = this.CreateMenu(this.servantStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantStatusDialog.Open(kind, servantLeaderInfo, callback);
    }

    public void OpenServantStatusDialog(ServantStatusDialog.Kind kind, int svtId, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantStatusDialog == null)
        {
            this.servantStatusDialog = this.CreateMenu(this.servantStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        ServantLeaderInfo servantLeaderInfo = new ServantLeaderInfo(svtId);
        this.servantStatusDialog.Open(kind, servantLeaderInfo, callback);
    }

    public void OpenServantStatusDialog(ServantStatusDialog.Kind kind, long userSvtId, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantStatusDialog == null)
        {
            this.servantStatusDialog = this.CreateMenu(this.servantStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantStatusDialog.Open(kind, userSvtId, callback);
    }

    public void OpenServantStatusDialog(ServantStatusDialog.Kind kind, UserServantCollectionEntity userSvtCollectionEntity, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantStatusDialog == null)
        {
            this.servantStatusDialog = this.CreateMenu(this.servantStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantStatusDialog.Open(kind, userSvtCollectionEntity, callback);
    }

    public void OpenServantStatusDialog(ServantStatusDialog.Kind kind, UserServantEntity userSvtEntity, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantStatusDialog == null)
        {
            this.servantStatusDialog = this.CreateMenu(this.servantStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantStatusDialog.Open(kind, userSvtEntity, callback);
    }

    public void OpenServantStatusDialog(ServantStatusDialog.Kind kind, PartyListViewItem partyItem, int member, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantStatusDialog == null)
        {
            this.servantStatusDialog = this.CreateMenu(this.servantStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantStatusDialog.Open(kind, partyItem, member, callback);
    }

    public void OpenServantStatusDialog(ServantStatusDialog.Kind kind, long userSvtId, long[] equipIdList, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantStatusDialog == null)
        {
            this.servantStatusDialog = this.CreateMenu(this.servantStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantStatusDialog.Open(kind, userSvtId, equipIdList, callback);
    }

    public void OpenServantStatusDialog(ServantStatusDialog.Kind kind, UserServantEntity userSvtEntity, long[] equipIdList, ServantStatusDialog.ClickDelegate callback)
    {
        if (this.servantStatusDialog == null)
        {
            this.servantStatusDialog = this.CreateMenu(this.servantStatusDialogPrefab).GetComponent<ServantStatusDialog>();
        }
        this.servantStatusDialog.Open(kind, userSvtEntity, equipIdList, callback);
    }

    public void OpenSkillCombineResult(CombineResultEffectComponent.Kind kind, UserServantEntity usrSvtData, int targetId, int targetLv, CombineResultEffectComponent.ClickDelegate callback, int targetIdOld = 0, int targetLvOld = 0)
    {
        if (this.combineResEffectComp == null)
        {
            this.combineResEffectComp = this.CreateMenu(this.combineResEffectPrefab).GetComponent<CombineResultEffectComponent>();
        }
        this.combineResEffectComp.SetSkillCombineInfo(kind, usrSvtData, targetId, targetLv, callback, targetIdOld, targetLvOld);
    }

    public void OpenStonePurchaseMenu(StonePurchaseMenu.CallbackFunc callback, System.Action refreshCallback = null)
    {
        if (this.stonePurchaseMenu == null)
        {
            this.stonePurchaseMenu = this.CreateMenu(this.stonePurchaseMenuPrefab).GetComponent<StonePurchaseMenu>();
        }
        this.stonePurchaseMenu.Open(callback, refreshCallback);
    }

    public void OpenStonePurchaseReciveMenu(AccountingManager.Result result, int cumulativeAmount, string resultString)
    {
        switch (result)
        {
            case AccountingManager.Result.SUCCESS:
                AgeVerificationMenu.SaveCumulativeAmount(cumulativeAmount);
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
                this.stonePurchaseNotificationMenu.Open(StonePurchaseNotificationMenu.Kind.SUCCESS, new System.Action(this.EndBuyBankItemNotificationRecive));
                return;

            case AccountingManager.Result.CANCEL:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                this.stonePurchaseNotificationMenu.Open(StonePurchaseNotificationMenu.Kind.CANCEL, new System.Action(this.EndBuyBankItemNotificationRecive));
                return;
        }
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        this.stonePurchaseNotificationMenu.Open(StonePurchaseNotificationMenu.Kind.FAIL, new System.Action(this.EndBuyBankItemNotificationRecive));
    }

    public void OpenSupportServantEquipListMenu(SupportServantData supportServantData, int classPos, SupportServantEquipListMenu.CallbackFunc callback)
    {
        if (this.supportServantEquipListMenu == null)
        {
            this.supportServantEquipListMenu = this.CreateMenu(this.supportSelectEquipListMenuPrefab).GetComponent<SupportServantEquipListMenu>();
        }
        this.supportServantEquipListMenu.Open(supportServantData, classPos, callback);
    }

    public void OpenSvtFrameShortDlg(int haveSvtNum, int maxSvtNum, bool is_equip, bool isQuest, ServantFrameShortDlgComponent.CallbackFunc callback)
    {
        if (this.svtFrameShortDialog == null)
        {
            this.svtFrameShortDialog = this.CreateMenu(this.servantFrameShortDlgPrefab).GetComponent<ServantFrameShortDlgComponent>();
        }
        this.svtFrameShortDialog.OpenShortSvt(haveSvtNum, maxSvtNum, is_equip, isQuest, callback);
    }

    public void OpenTripleButtonDlg(string title, string message, string cancelBtnMsg, string middleBtnMsg, string rightBtnMsg, TripleButtonDlgComponent.CallbackFunc callback)
    {
        if (this.tripleButtonDialog == null)
        {
            this.tripleButtonDialog = this.CreateMenu(this.tripleButtonDlgPrefab).GetComponent<TripleButtonDlgComponent>();
        }
        this.tripleButtonDialog.Open(title, message, cancelBtnMsg, middleBtnMsg, rightBtnMsg, callback);
    }

    public void OpenTutorialArrowMark(Vector2 pos, float way, Rect rect, System.Action callback)
    {
        this.tutorialArrowMenu.Open(pos, way, rect, callback);
    }

    public void OpenTutorialArrowMark(Vector2[] posList, float way, Rect rect, System.Action callback)
    {
        this.tutorialArrowMenu.Open(posList, way, rect, callback);
    }

    public void OpenTutorialArrowMark(Vector2 pos, float way, Rect[] rects, System.Action callback)
    {
        this.tutorialArrowMenu.Open(pos, way, rects, callback);
    }

    public void OpenTutorialArrowMark(Vector2[] posList, float way, Rect[] rects, System.Action callback)
    {
        this.tutorialArrowMenu.Open(posList, way, rects, callback);
    }

    public void OpenTutorialArrowMark(Vector2[] posList, float[] ways, Rect[] rects, System.Action callback)
    {
        this.tutorialArrowMenu.Open(posList, ways, rects, callback);
    }

    public void OpenTutorialImageDialog(TutorialFlag.ImageId[] images, TutorialFlag.Id flagId = -1, System.Action callback = null)
    {
        this.tutorialBigDialog.Open(images, flagId, callback);
    }

    public void OpenTutorialImageDialog(TutorialFlag.ImageId image, TutorialFlag.Id flagId = -1, System.Action callback = null)
    {
        TutorialFlag.ImageId[] images = new TutorialFlag.ImageId[] { image };
        this.OpenTutorialImageDialog(images, flagId, callback);
    }

    public void OpenTutorialNotificationDialog(string message, TutorialFlag.Id flagId = -1, System.Action callback = null)
    {
        this.tutorialNotificationMessage.Open(message, flagId, callback);
    }

    public void OpenTutorialNotificationDialogArrow(string message, Vector2 pos, Rect rect, float way, [Optional] Vector2 messagePos, int fontSize = -1, System.Action callbackFunc = null)
    {
        Vector2[] posList = new Vector2[] { pos };
        Rect[] rects = new Rect[] { rect };
        float[] ways = new float[] { way };
        this.OpenTutorialNotificationDialogArrow(message, posList, rects, ways, messagePos, fontSize, callbackFunc);
    }

    public void OpenTutorialNotificationDialogArrow(string message, Vector2[] posList, Rect rect, float way, [Optional] Vector2 messagePos, int fontSize = -1, System.Action callbackFunc = null)
    {
        Rect[] rects = new Rect[] { rect };
        float[] ways = new float[] { way };
        this.OpenTutorialNotificationDialogArrow(message, posList, rects, ways, messagePos, fontSize, callbackFunc);
    }

    public void OpenTutorialNotificationDialogArrow(string message, Vector2[] posList, Rect[] rects, float way, [Optional] Vector2 messagePos, int fontSize = -1, System.Action callbackFunc = null)
    {
        float[] ways = new float[] { way };
        this.OpenTutorialNotificationDialogArrow(message, posList, rects, ways, messagePos, fontSize, callbackFunc);
    }

    public void OpenTutorialNotificationDialogArrow(string message, Vector2 pos, Rect[] rects, float way, [Optional] Vector2 messagePos, int fontSize = -1, System.Action callbackFunc = null)
    {
        Vector2[] posList = new Vector2[] { pos };
        float[] ways = new float[] { way };
        this.OpenTutorialNotificationDialogArrow(message, posList, rects, ways, messagePos, fontSize, callbackFunc);
    }

    public void OpenTutorialNotificationDialogArrow(string message, Vector2[] posList, Rect[] rects, float[] ways, [Optional] Vector2 messagePos, int fontSize = -1, System.Action callbackFunc = null)
    {
        this.tutorialNotificationMessage.OpenWithArrow(message, messagePos, fontSize);
        this.OpenTutorialArrowMark(posList, ways, rects, callbackFunc);
    }

    public void OpenUserGameActRecoverMenu(UserGameActRecoverMenu.CallbackFunc callback, System.Action refreshCallback = null)
    {
        if (this.userGameActRecoverMenu == null)
        {
            this.userGameActRecoverMenu = this.CreateMenu(this.userGameActRecoverMenuPrefab).GetComponent<UserGameActRecoverMenu>();
        }
        this.userGameActRecoverMenu.Open(callback, refreshCallback);
    }

    public void OpenUserNameEntry(System.Action closed_act)
    {
        <OpenUserNameEntry>c__AnonStorey4C storeyc = new <OpenUserNameEntry>c__AnonStorey4C {
            closed_act = closed_act,
            <>f__this = this
        };
        if (this.userNameEntry == null)
        {
            this.userNameEntry = this.CreateMenu(this.userNameEntryPrefab).GetComponent<UserNameEntryComponent>();
        }
        this.userNameEntry.open(new System.Action(storeyc.<>m__B));
    }

    public void OpenUsrPresentList(bool isShowBg, UserPresentBoxWindow.ClickDelegate callback, System.Action redisp_act = null)
    {
        if (this.usrPresentListWindow == null)
        {
            this.usrPresentListWindow = this.CreateMenu(this.userPresentBoxWindowPrefab).GetComponent<UserPresentBoxWindow>();
        }
        this.usrPresentListWindow.Open(isShowBg, callback, redisp_act);
    }

    public void OpenWarningDialog(string title, string message, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock = false)
    {
        base.StartCoroutine(this.OpenWarningDialogCR(title, message, func, isNetwrokRebootBlock));
    }

    [DebuggerHidden]
    protected IEnumerator OpenWarningDialogCR(string title, string message, ErrorDialog.ClickDelegate func, bool isNetwrokRebootBlock) => 
        new <OpenWarningDialogCR>c__Iterator3 { 
            title = title,
            message = message,
            isNetwrokRebootBlock = isNetwrokRebootBlock,
            func = func,
            <$>title = title,
            <$>message = message,
            <$>isNetwrokRebootBlock = isNetwrokRebootBlock,
            <$>func = func,
            <>f__this = this
        };

    public bool PlayLogo(System.Action callback)
    {
        if (!LogoMain.IsPLayLogo())
        {
            return false;
        }
        if (this.logoMain == null)
        {
            this.logoMain = this.CreateMenu(this.logoPrefab).GetComponent<LogoMain>();
        }
        this.logoMainCallback = callback;
        this.logoMain.Init(new System.Action(this.EndPlayLogo));
        return true;
    }

    public void Reboot()
    {
        this.warningDialog.Init();
        this.errorDialog.Init();
        this.retryDialog.Init();
        this.retryBootDialog.Init();
        this.commonConfirmDialog.Init();
        this.notifiDialog.Init();
        this.popupMessageDialog.Init();
        this.detailInfoDialog.Init();
        this.tutorialArrowMenu.Init();
        this.tutorialBigDialog.Init();
        if (this.servantFramePurchaseMenu != null)
        {
            UnityEngine.Object.Destroy(this.servantFramePurchaseMenu.gameObject);
            this.servantFramePurchaseMenu = null;
        }
        if (this.servantEquipFramePurchaseMenu != null)
        {
            UnityEngine.Object.Destroy(this.servantEquipFramePurchaseMenu.gameObject);
            this.servantEquipFramePurchaseMenu = null;
        }
        if (this.equipGraphListMenu != null)
        {
            UnityEngine.Object.Destroy(this.equipGraphListMenu.gameObject);
            this.equipGraphListMenu = null;
        }
        if (this.supportServantEquipListMenu != null)
        {
            UnityEngine.Object.Destroy(this.supportServantEquipListMenu.gameObject);
            this.supportServantEquipListMenu = null;
        }
        if (this.servantSortSelectMenu != null)
        {
            UnityEngine.Object.Destroy(this.servantSortSelectMenu.gameObject);
            this.servantSortSelectMenu = null;
        }
        if (this.servantFilterSelectMenu != null)
        {
            UnityEngine.Object.Destroy(this.servantFilterSelectMenu.gameObject);
            this.servantFilterSelectMenu = null;
        }
        if (this.servantStatusDialog != null)
        {
            UnityEngine.Object.Destroy(this.servantStatusDialog.gameObject);
            this.servantStatusDialog = null;
        }
        if (this.servantEquipStatusDialog != null)
        {
            UnityEngine.Object.Destroy(this.servantEquipStatusDialog.gameObject);
            this.servantEquipStatusDialog = null;
        }
        if (this.stonePurchaseMenu != null)
        {
            UnityEngine.Object.Destroy(this.stonePurchaseMenu.gameObject);
            this.stonePurchaseMenu = null;
        }
        if (this.userGameActRecoverMenu != null)
        {
            UnityEngine.Object.Destroy(this.userGameActRecoverMenu.gameObject);
            this.userGameActRecoverMenu = null;
        }
        if (this.userStatusComp != null)
        {
            this.userStatusComp.HideUserStatus();
        }
        if (this.usrPresentListWindow != null)
        {
            UnityEngine.Object.Destroy(this.usrPresentListWindow.gameObject);
            this.usrPresentListWindow = null;
        }
        if (this.presentBoxNotificationMenu != null)
        {
            UnityEngine.Object.Destroy(this.presentBoxNotificationMenu.gameObject);
            this.presentBoxNotificationMenu = null;
        }
        if (this.svtFrameShortDialog != null)
        {
            UnityEngine.Object.Destroy(this.svtFrameShortDialog.gameObject);
            this.svtFrameShortDialog = null;
        }
        if (this.userNameEntry != null)
        {
            UnityEngine.Object.Destroy(this.userNameEntry.gameObject);
            this.userNameEntry = null;
        }
        if (this.combineResEffectComp != null)
        {
            UnityEngine.Object.Destroy(this.combineResEffectComp.gameObject);
            this.combineResEffectComp = null;
        }
        if (this.classCompatibilityMenu != null)
        {
            UnityEngine.Object.Destroy(this.classCompatibilityMenu.gameObject);
            this.classCompatibilityMenu = null;
            this.closeClassCompatibilityMenuCallback = null;
        }
        if (this.apRecvDlgComp != null)
        {
            UnityEngine.Object.Destroy(this.apRecvDlgComp.gameObject);
            this.apRecvDlgComp = null;
        }
        if (this.itemDetailDlgComp != null)
        {
            UnityEngine.Object.Destroy(this.itemDetailDlgComp.gameObject);
            this.itemDetailDlgComp = null;
        }
        this.connectMark.Init();
        this.maskFade.Init();
    }

    protected void SelectNotificationDialog(bool result)
    {
        System.Action closeNotificationCallback = this.closeNotificationCallback;
        this.closeNotificationCallback = null;
        this.notifiDialog.Close(closeNotificationCallback);
    }

    public void setAllocMem(bool flag)
    {
        this.allocMem.enabled = flag;
    }

    public void SetConnect(bool isConnect)
    {
        this.connectMark.SetConnect(isConnect);
    }

    public void SetLoadMode(ConnectMark.Mode mode)
    {
        this.connectMark.SetMode(mode);
    }

    public void SetMessageShow(bool isShow)
    {
        this.connectMark.SetMessageShow(isShow);
    }

    public void setObiImgSize(int heightSize, int widthSize)
    {
        if (Application.isPlaying)
        {
            if (heightSize > 0)
            {
                this.topImg.gameObject.SetActive(true);
                this.bottomImg.gameObject.SetActive(true);
                this.topImg.width = ManagerConfig.WIDTH;
                this.topImg.height = heightSize + 4;
                this.topImg.transform.localPosition = new Vector3(0f, (ManagerConfig.HEIGHT / 2) - 1f, 0f);
                this.bottomImg.width = ManagerConfig.WIDTH;
                this.bottomImg.height = heightSize + 4;
                this.bottomImg.transform.localPosition = new Vector3(0f, (-ManagerConfig.HEIGHT / 2) + 1f, 0f);
                this.leftImg.gameObject.SetActive(false);
                this.rightImg.gameObject.SetActive(false);
            }
            else if (widthSize > 0)
            {
                this.leftImg.gameObject.SetActive(true);
                this.rightImg.gameObject.SetActive(true);
                this.leftImg.width = widthSize + 3;
                this.leftImg.height = ManagerConfig.HEIGHT;
                this.leftImg.transform.localPosition = new Vector3((float) (-ManagerConfig.WIDTH / 2), 0f, 0f);
                this.rightImg.width = widthSize + 3;
                this.rightImg.height = ManagerConfig.HEIGHT;
                this.rightImg.transform.localPosition = new Vector3((float) (ManagerConfig.WIDTH / 2), 0f, 0f);
                this.topImg.gameObject.SetActive(false);
                this.bottomImg.gameObject.SetActive(false);
            }
            else
            {
                this.topImg.gameObject.SetActive(false);
                this.bottomImg.gameObject.SetActive(false);
                this.leftImg.gameObject.SetActive(false);
                this.rightImg.gameObject.SetActive(false);
            }
        }
    }

    public void SetupLoginResultData()
    {
        string topLoginResult = SingletonMonoBehaviour<NetworkManager>.Instance.GetTopLoginResult();
        if (topLoginResult != null)
        {
            this.mLoginResult = JsonManager.Deserialize<LoginResultData>(topLoginResult);
        }
        else
        {
            this.mLoginResult = null;
        }
        SingletonMonoBehaviour<NetworkManager>.Instance.ClearTopLoginResult();
    }

    public void ShowUserStatus()
    {
        if (this.userStatusComp == null)
        {
            this.userStatusComp = this.CreateMenu(this.userStatusPrefab).GetComponent<UserStatusComponet>();
        }
        this.userStatusComp.SetUserStatusData();
    }

    private void StartCampaignBonus(System.Action end_act, int panel_depth = -1)
    {
        bool flag = false;
        if (this.mLoginResult != null)
        {
            flag = (this.mLoginResult.campaignbonus != null) && (this.mLoginResult.campaignbonus.Length > 0);
        }
        if (flag)
        {
            this.StartCampaignBonus_sub(0, end_act, panel_depth);
        }
        else
        {
            end_act.Call();
        }
    }

    private void StartCampaignBonus_sub(int idx, System.Action end_act, int panel_depth = -1)
    {
        <StartCampaignBonus_sub>c__AnonStorey4F storeyf = new <StartCampaignBonus_sub>c__AnonStorey4F {
            idx = idx,
            end_act = end_act,
            panel_depth = panel_depth,
            <>f__this = this
        };
        CampaignBonusData[] campaignbonus = this.mLoginResult.campaignbonus;
        if (storeyf.idx >= campaignbonus.Length)
        {
            storeyf.end_act.Call();
        }
        else
        {
            CampaignBonusData data = campaignbonus[storeyf.idx];
            string name = data.name;
            string message = data.detail + "\n\n";
            if ((data.items != null) && (data.items.Length > 0))
            {
                foreach (LoginBonusItemData data2 in data.items)
                {
                    message = message + string.Format(LocalizationManager.Get("CAMPAIGN_BONUS_ITEM"), data2.name, data2.num);
                }
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(name, message, new System.Action(storeyf.<>m__E), storeyf.panel_depth);
        }
    }

    public void StartFriendPointNotification(System.Action end_act)
    {
        <StartFriendPointNotification>c__AnonStorey50 storey = new <StartFriendPointNotification>c__AnonStorey50 {
            end_act = end_act
        };
        bool flag = false;
        if (this.mLoginResult != null)
        {
            flag = this.mLoginResult.addFriendPoint > 0;
        }
        if (flag)
        {
            TblUserEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<TblUserMaster>(DataNameKind.Kind.TBL_USER_GAME).getUserData(NetworkManager.UserId);
            string message = string.Format(LocalizationManager.Get("GET_FRIEND_POINT"), this.mLoginResult.addFriendPoint, entity.friendPoint, LocalizationManager.Get("SUPPORT_KIND_" + this.mLoginResult.topAddFriendPointClass));
            string rightBtnMsg = LocalizationManager.Get("GET_FRIEND_POINT_GACHA_BTN");
            string middleBtnMsg = LocalizationManager.Get("GET_FRIEND_POINT_SUPPORT_BTN");
            string cancelBtnMsg = LocalizationManager.Get("COMMON_CONFIRM_CLOSE");
            UILabel.Overflow overflowMethod = this.commonConfirmDialog.ButtonDecideLabel.overflowMethod;
            this.OpenTripleButtonDlg(string.Empty, message, cancelBtnMsg, middleBtnMsg, rightBtnMsg, new TripleButtonDlgComponent.CallbackFunc(storey.<>m__F));
        }
        else
        {
            storey.end_act.Call();
        }
    }

    public void StartLoginAndCampaignBonus(System.Action end_act, System.Action onCloseLogin, int panel_depth = -1)
    {
        this.StartLoginBonus(end_act, onCloseLogin, panel_depth);
    }

    private void StartLoginBonus(System.Action end_act, System.Action onCloseLogin, int panel_depth = -1)
    {
        <StartLoginBonus>c__AnonStorey4E storeye = new <StartLoginBonus>c__AnonStorey4E {
            onCloseLogin = onCloseLogin,
            end_act = end_act,
            panel_depth = panel_depth,
            <>f__this = this
        };
        bool flag = false;
        if (this.mLoginResult != null)
        {
            flag = this.mLoginResult.loginbonus != null;
        }
        if (flag)
        {
            LoginBonusData loginbonus = this.mLoginResult.loginbonus;
            string title = LocalizationManager.Get("LOGIN_BONUS_TITLE");
            string str2 = string.Empty;
            if ((loginbonus.totalItems != null) && (loginbonus.totalItems.Length > 0))
            {
                str2 = str2 + string.Format(LocalizationManager.Get("LOGIN_BONUS_TOTAL"), loginbonus.totalLogin);
                foreach (LoginBonusItemData data2 in loginbonus.totalItems)
                {
                    str2 = str2 + string.Format(LocalizationManager.Get("LOGIN_BONUS_TOTAL_ITEM"), data2.name, data2.num);
                }
            }
            if ((loginbonus.seqItems != null) && (loginbonus.seqItems.Length > 0))
            {
                str2 = str2 + "\n" + string.Format(LocalizationManager.Get("LOGIN_BONUS_SEQ"), loginbonus.seqLogin);
                foreach (LoginBonusItemData data3 in loginbonus.seqItems)
                {
                    str2 = str2 + string.Format(LocalizationManager.Get("LOGIN_BONUS_SEQ_ITEM"), data3.name, data3.num);
                }
            }
            str2 = str2 + loginbonus.message;
            if (!string.IsNullOrEmpty(str2))
            {
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, str2, new System.Action(storeye.<>m__D), storeye.panel_depth);
                return;
            }
        }
        this.StartCampaignBonus(storeye.end_act, storeye.panel_depth);
    }

    public void StartServantEventJoinLeaveNotification(System.Action end_act)
    {
        bool flag = false;
        if (this.mLoginResult != null)
        {
            flag = (this.mLoginResult.leaveSvt != null) && (this.mLoginResult.leaveSvt.Length > 0);
        }
        if (flag)
        {
            this.StartServantEventJoinLeaveNotification_sub(0, end_act);
        }
        else
        {
            end_act.Call();
        }
    }

    private void StartServantEventJoinLeaveNotification_sub(int idx, System.Action end_act)
    {
        <StartServantEventJoinLeaveNotification_sub>c__AnonStorey4D storeyd = new <StartServantEventJoinLeaveNotification_sub>c__AnonStorey4D {
            idx = idx,
            end_act = end_act,
            <>f__this = this
        };
        LeaveSvtData[] leaveSvt = this.mLoginResult.leaveSvt;
        if (storeyd.idx >= leaveSvt.Length)
        {
            storeyd.end_act.Call();
        }
        else
        {
            LeaveSvtData data = leaveSvt[storeyd.idx];
            EventServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventServantMaster>(DataNameKind.Kind.EVENT_SERVANT).getEntityFromId<EventServantEntity>(data.eventId, data.svtId);
            string title = string.Empty;
            string leaveMessage = entity.leaveMessage;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, leaveMessage, new System.Action(storeyd.<>m__C), -1);
        }
    }

    public void switchingAllocMem()
    {
        if (this.allocMem.enabled)
        {
            this.allocMem.enabled = false;
        }
        else
        {
            this.allocMem.enabled = true;
        }
    }

    [CompilerGenerated]
    private sealed class <CloseSvtFrameShortDlg>c__AnonStorey4B
    {
        internal CommonUI <>f__this;
        internal System.Action closed_act;

        internal void <>m__A()
        {
            if (this.closed_act != null)
            {
                this.closed_act.Call();
            }
            UnityEngine.Object.Destroy(this.<>f__this.svtFrameShortDialog.gameObject);
            this.<>f__this.svtFrameShortDialog = null;
        }
    }

    [CompilerGenerated]
    private sealed class <CloseTripleButtonDlg>c__AnonStorey4A
    {
        internal CommonUI <>f__this;
        internal System.Action closed_act;

        internal void <>m__9()
        {
            if (this.closed_act != null)
            {
                this.closed_act.Call();
            }
            UnityEngine.Object.Destroy(this.<>f__this.tripleButtonDialog.gameObject);
            this.<>f__this.tripleButtonDialog = null;
        }
    }

    [CompilerGenerated]
    private sealed class <CloseTutorialNotificationDialogArrow>c__AnonStorey49
    {
        internal System.Action callbackFunc;
        internal bool isClose;

        internal void <>m__7()
        {
            if (this.isClose)
            {
                if (this.callbackFunc != null)
                {
                    this.callbackFunc();
                }
            }
            else
            {
                this.isClose = true;
            }
        }

        internal void <>m__8()
        {
            if (this.isClose)
            {
                if (this.callbackFunc != null)
                {
                    this.callbackFunc();
                }
            }
            else
            {
                this.isClose = true;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <OpenErrorDialogCR>c__Iterator4 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ErrorDialog.ClickDelegate <$>func;
        internal bool <$>isNetwrokRebootBlock;
        internal string <$>message;
        internal string <$>title;
        internal CommonUI <>f__this;
        internal ErrorDialog.ClickDelegate <baseFunc>__0;
        internal ErrorDialog.ClickDelegate func;
        internal bool isNetwrokRebootBlock;
        internal string message;
        internal string title;

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
                    Debug.LogError("ErrorDialog: " + this.title + ": " + this.message);
                    if (!this.isNetwrokRebootBlock || !NetworkManager.IsRebootBlock)
                    {
                        break;
                    }
                    goto Label_0141;

                case 1:
                    if (!this.isNetwrokRebootBlock || !NetworkManager.IsRebootBlock)
                    {
                        break;
                    }
                    goto Label_0141;

                default:
                    goto Label_0141;
            }
            while ((this.<>f__this.warningDialog.IsBusy || this.<>f__this.errorDialog.IsBusy) || (this.<>f__this.retryDialog.IsBusy || this.<>f__this.retryBootDialog.IsBusy))
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                return true;
            }
            this.<baseFunc>__0 = new ErrorDialog.ClickDelegate(this.<>f__this.OnEndErrorDialog);
            this.<baseFunc>__0 = (ErrorDialog.ClickDelegate) Delegate.Combine(this.<baseFunc>__0, this.func);
            this.<>f__this.errorDialog.Open(this.title, this.message, this.<baseFunc>__0);
            this.$PC = -1;
        Label_0141:
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

    [CompilerGenerated]
    private sealed class <OpenFriendshipUp>c__AnonStorey51
    {
        internal CommonUI <>f__this;
        internal CombineResultEffectComponent.ClickDelegate callback;

        internal void <>m__10(bool is_decide)
        {
            if (this.callback != null)
            {
                this.callback(is_decide);
            }
            this.<>f__this.CloseCombineResult();
        }
    }

    [CompilerGenerated]
    private sealed class <OpenPowerUp>c__AnonStorey52
    {
        internal CommonUI <>f__this;
        internal CombineResultEffectComponent.ClickDelegate callback;

        internal void <>m__11(bool is_decide)
        {
            if (this.callback != null)
            {
                this.callback(is_decide);
            }
            this.<>f__this.CloseCombineResult();
        }
    }

    [CompilerGenerated]
    private sealed class <OpenRetryBootDialogCR>c__Iterator6 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ErrorDialog.ClickDelegate <$>func;
        internal bool <$>isNetwrokRebootBlock;
        internal string <$>message;
        internal string <$>title;
        internal CommonUI <>f__this;
        internal ErrorDialog.ClickDelegate <baseFunc>__0;
        internal ErrorDialog.ClickDelegate func;
        internal bool isNetwrokRebootBlock;
        internal string message;
        internal string title;

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
                    Debug.LogError("RetryBootDialog: " + this.title + ": " + this.message);
                    if (!this.isNetwrokRebootBlock || !NetworkManager.IsRebootBlock)
                    {
                        break;
                    }
                    goto Label_0141;

                case 1:
                    if (!this.isNetwrokRebootBlock || !NetworkManager.IsRebootBlock)
                    {
                        break;
                    }
                    goto Label_0141;

                default:
                    goto Label_0141;
            }
            while ((this.<>f__this.warningDialog.IsBusy || this.<>f__this.errorDialog.IsBusy) || (this.<>f__this.retryDialog.IsBusy || this.<>f__this.retryBootDialog.IsBusy))
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                return true;
            }
            this.<baseFunc>__0 = new ErrorDialog.ClickDelegate(this.<>f__this.OnEndRetryBootDialog);
            this.<baseFunc>__0 = (ErrorDialog.ClickDelegate) Delegate.Combine(this.<baseFunc>__0, this.func);
            this.<>f__this.retryBootDialog.Open(this.title, this.message, this.<baseFunc>__0);
            this.$PC = -1;
        Label_0141:
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

    [CompilerGenerated]
    private sealed class <OpenRetryDialogCR>c__Iterator5 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>cancleTxt;
        internal string <$>decideTxt;
        internal ErrorDialog.ClickDelegate <$>func;
        internal bool <$>isNetwrokRebootBlock;
        internal string <$>message;
        internal string <$>title;
        internal CommonUI <>f__this;
        internal ErrorDialog.ClickDelegate <baseFunc>__0;
        internal string cancleTxt;
        internal string decideTxt;
        internal ErrorDialog.ClickDelegate func;
        internal bool isNetwrokRebootBlock;
        internal string message;
        internal string title;

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
                    Debug.LogError("RetryDialog: " + this.title + ": " + this.message);
                    if (!this.isNetwrokRebootBlock || !NetworkManager.IsRebootBlock)
                    {
                        break;
                    }
                    goto Label_018A;

                case 1:
                    if (!this.isNetwrokRebootBlock || !NetworkManager.IsRebootBlock)
                    {
                        break;
                    }
                    goto Label_018A;

                default:
                    goto Label_018A;
            }
            while ((this.<>f__this.warningDialog.IsBusy || this.<>f__this.errorDialog.IsBusy) || (this.<>f__this.retryDialog.IsBusy || this.<>f__this.retryBootDialog.IsBusy))
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                return true;
            }
            this.<baseFunc>__0 = new ErrorDialog.ClickDelegate(this.<>f__this.OnEndRetryDialog);
            this.<baseFunc>__0 = (ErrorDialog.ClickDelegate) Delegate.Combine(this.<baseFunc>__0, this.func);
            if ((this.decideTxt != null) && (this.cancleTxt != null))
            {
                this.<>f__this.retryDialog.Open(this.title, this.message, this.decideTxt, this.cancleTxt, this.<baseFunc>__0);
            }
            else
            {
                this.<>f__this.retryDialog.Open(this.title, this.message, this.<baseFunc>__0);
            }
            this.$PC = -1;
        Label_018A:
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

    [CompilerGenerated]
    private sealed class <OpenUserNameEntry>c__AnonStorey4C
    {
        internal CommonUI <>f__this;
        internal System.Action closed_act;

        internal void <>m__B()
        {
            this.<>f__this.CloseUserNameEntry();
            this.closed_act.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <OpenWarningDialogCR>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ErrorDialog.ClickDelegate <$>func;
        internal bool <$>isNetwrokRebootBlock;
        internal string <$>message;
        internal string <$>title;
        internal CommonUI <>f__this;
        internal ErrorDialog.ClickDelegate <baseFunc>__0;
        internal ErrorDialog.ClickDelegate func;
        internal bool isNetwrokRebootBlock;
        internal string message;
        internal string title;

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
                    Debug.LogError("WarningDialog: " + this.title + ": " + this.message);
                    if (!this.isNetwrokRebootBlock || !NetworkManager.IsRebootBlock)
                    {
                        break;
                    }
                    goto Label_0141;

                case 1:
                    if (!this.isNetwrokRebootBlock || !NetworkManager.IsRebootBlock)
                    {
                        break;
                    }
                    goto Label_0141;

                default:
                    goto Label_0141;
            }
            while ((this.<>f__this.warningDialog.IsBusy || this.<>f__this.errorDialog.IsBusy) || (this.<>f__this.retryDialog.IsBusy || this.<>f__this.retryBootDialog.IsBusy))
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                return true;
            }
            this.<baseFunc>__0 = new ErrorDialog.ClickDelegate(this.<>f__this.OnEndWarningDialog);
            this.<baseFunc>__0 = (ErrorDialog.ClickDelegate) Delegate.Combine(this.<baseFunc>__0, this.func);
            this.<>f__this.warningDialog.Open(this.title, this.message, this.<baseFunc>__0);
            this.$PC = -1;
        Label_0141:
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

    [CompilerGenerated]
    private sealed class <StartCampaignBonus_sub>c__AnonStorey4F
    {
        internal CommonUI <>f__this;
        internal System.Action end_act;
        internal int idx;
        internal int panel_depth;

        internal void <>m__E()
        {
            this.<>f__this.StartCampaignBonus_sub(this.idx + 1, this.end_act, this.panel_depth);
        }
    }

    [CompilerGenerated]
    private sealed class <StartFriendPointNotification>c__AnonStorey50
    {
        internal System.Action end_act;

        internal void <>m__F(TripleButtonDlgComponent.ResultClicked result)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseTripleButtonDlg(null);
            switch (result)
            {
                case TripleButtonDlgComponent.ResultClicked.RIGHT:
                    TerminalPramsManager.SummonType = 3;
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Summon, SceneManager.FadeType.BLACK, null);
                    break;

                case TripleButtonDlgComponent.ResultClicked.MIDDLE:
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.SupportSelect, SceneManager.FadeType.BLACK, null);
                    break;

                case TripleButtonDlgComponent.ResultClicked.CANCEL:
                    this.end_act.Call();
                    break;
            }
        }
    }

    [CompilerGenerated]
    private sealed class <StartLoginBonus>c__AnonStorey4E
    {
        internal CommonUI <>f__this;
        internal System.Action end_act;
        internal System.Action onCloseLogin;
        internal int panel_depth;

        internal void <>m__D()
        {
            this.onCloseLogin.Call();
            this.<>f__this.StartCampaignBonus(this.end_act, this.panel_depth);
        }
    }

    [CompilerGenerated]
    private sealed class <StartServantEventJoinLeaveNotification_sub>c__AnonStorey4D
    {
        internal CommonUI <>f__this;
        internal System.Action end_act;
        internal int idx;

        internal void <>m__C()
        {
            this.<>f__this.StartServantEventJoinLeaveNotification_sub(this.idx + 1, this.end_act);
        }
    }

    public class CampaignBonusData
    {
        public string detail;
        public CommonUI.LoginBonusItemData[] items;
        public string name;
    }

    public class LeaveSvtData
    {
        public int eventId;
        public int svtId;
    }

    public class LoginBonusData
    {
        public string message;
        public CommonUI.LoginBonusItemData[] seqItems;
        public int seqLogin;
        public CommonUI.LoginBonusItemData[] totalItems;
        public int totalLogin;
    }

    public class LoginBonusItemData
    {
        public string name;
        public int num;
    }

    public class LoginResultData
    {
        public int addFriendPoint;
        public CommonUI.CampaignBonusData[] campaignbonus;
        public CommonUI.LeaveSvtData[] leaveSvt;
        public CommonUI.LoginBonusData loginbonus;
        public int topAddFriendPointClass;
    }
}

