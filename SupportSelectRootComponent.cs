using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SupportSelectRootComponent : SceneRootComponent
{
    [SerializeField]
    private UICommonButton allResetButton;
    [SerializeField]
    private UICommonButton allResetEquipButton;
    [SerializeField]
    private UICommonButton allResetServantButton;
    protected string[] cacheAssetNameList;
    [SerializeField]
    protected CancelConfirmMenu cancelConfirmMenu;
    private int classPos;
    protected static float EDIT_SCENE_FADE_TIME = 0.5f;
    protected State state;
    protected SupportInfoJump supportInfoJump;
    [SerializeField]
    protected SupportSelectConfirmMenu supportSelectConfirmMenu;
    [SerializeField]
    protected SupportSelectMenu supportSelectMenu;
    protected SupportServantData supportServantData = new SupportServantData();
    protected SupportServantEquipListMenu supportServantEquipListMenu;
    [SerializeField]
    protected GameObject supportServantEquipListMenuPrefab;
    [SerializeField]
    protected SupportServantSelectMenu supportServantSelectMenu;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected TitleInfoControl titleInfo2;

    public override void beginFinish()
    {
        this.supportSelectMenu.Init();
        this.supportServantSelectMenu.Init();
        this.supportSelectConfirmMenu.Init();
        this.cancelConfirmMenu.Init();
        this.state = State.INIT;
        this.SetCacheAssetNameList(null);
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        this.state = State.INIT;
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginResume()
    {
        Debug.Log("Call beginResume");
        base.beginResume();
    }

    public override void beginStartUp(object data)
    {
        this.supportInfoJump = data as SupportInfoJump;
        if (this.supportInfoJump != null)
        {
            if (this.supportInfoJump.GetFriendInfo() != null)
            {
                this.supportServantData.Init(this.supportInfoJump.GetFriendInfo(), this.supportInfoJump.Kind, this.supportInfoJump.IsSelect);
            }
            else
            {
                this.supportServantData.Init(this.supportInfoJump.GetFollowerInfo(), this.supportInfoJump.Kind, this.supportInfoJump.IsSelect);
            }
            this.buttonDispSetting(false);
            this.titleInfo.setTitleInfo(base.myFSM, TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.FRIEND_SUPPORT_INFO);
            this.state = State.INFO;
            this.supportSelectMenu.Open(this.supportServantData, new SupportSelectMenu.CallbackFunc(this.EndSupportSelectMenu));
        }
        else
        {
            this.supportServantData.Init();
            this.buttonDispSetting(true);
            this.resetButtonUpdate();
            this.titleInfo.setTitleInfo(base.myFSM, TitleInfoControl.BackKind.CLOSE, TitleInfoControl.TitleKind.SUPPORT_SELECT);
            this.state = State.SELECT;
            this.supportSelectMenu.Open(this.supportServantData, new SupportSelectMenu.CallbackFunc(this.EndSupportSelectMenu));
        }
        this.RefreshInfo();
        base.beginStartUp();
    }

    private void buttonDispSetting(bool flag)
    {
        this.allResetServantButton.gameObject.SetActive(flag);
        this.allResetEquipButton.gameObject.SetActive(flag);
        this.allResetButton.gameObject.SetActive(flag);
    }

    public void EndAllReset(bool isDecide)
    {
        this.supportSelectConfirmMenu.Close(null);
        if (isDecide)
        {
            for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
            {
                if (this.supportServantData.getServant(i) > 0L)
                {
                    this.supportServantData.removeServantData(i);
                }
                if (this.supportServantData.getEquip(i) > 0L)
                {
                    this.supportServantData.removeEquipData(i);
                }
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.resetButtonUpdate();
            this.supportSelectMenu.Reset(-1);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        }
    }

    public void EndCancelConfirmMenu(bool isDecide)
    {
        this.cancelConfirmMenu.Close();
        if (isDecide)
        {
            this.ReturnScene("ok");
        }
        else
        {
            this.ReturnScene("ng");
        }
    }

    protected void EndCloseServantEquipListCancel()
    {
        this.supportSelectMenu.Reset(this.classPos);
    }

    protected void EndCloseServantEquipListDecide()
    {
        this.supportSelectMenu.Redisp();
    }

    private void EndConfirmMenu(bool flag)
    {
        this.supportSelectConfirmMenu.Close(null);
        this.state = State.SELECT;
    }

    public void EndEquipAllReset(bool isDecide)
    {
        this.supportSelectConfirmMenu.Close(null);
        if (isDecide)
        {
            for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
            {
                if (this.supportServantData.getEquip(i) > 0L)
                {
                    this.supportServantData.removeEquipData(i);
                }
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.resetButtonUpdate();
            this.supportSelectMenu.Reset(-1);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        }
    }

    public void EndServantAllReset(bool isDecide)
    {
        this.supportSelectConfirmMenu.Close(null);
        if (isDecide)
        {
            for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
            {
                if (this.supportServantData.getServant(i) > 0L)
                {
                    this.supportServantData.removeServantData(i);
                }
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.resetButtonUpdate();
            this.supportSelectMenu.Reset(-1);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        }
    }

    protected void EndShowEquip(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipStatusDialog(null);
        this.supportSelectMenu.Redisp();
    }

    protected void EndShowServant(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(null);
        this.supportSelectMenu.Redisp();
    }

    protected void EndSupportSelectMenu(SupportSelectMenu.ResultKind result, int classPos)
    {
        UserServantLearderEntity entity;
        ServantStatusDialog.Kind kind;
        this.classPos = classPos;
        switch (result)
        {
            case SupportSelectMenu.ResultKind.DECIDE:
                if (!this.supportServantData.IsNoServant)
                {
                    this.supportServantSelectMenu.ModifyItem();
                    if (!NetworkManager.getRequest<FollowerSetupRequest>(new NetworkManager.ResultCallbackFunc(this.ReturnScene)).beginRequest(this.supportServantData))
                    {
                        this.ReturnScene("ok");
                    }
                    return;
                }
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                this.supportSelectConfirmMenu.Open(SupportSelectConfirmMenu.Kind.NO_SERVANT, new SupportSelectConfirmMenu.CallbackFunc(this.EndConfirmMenu));
                return;

            case SupportSelectMenu.ResultKind.CLOSE:
                return;

            case SupportSelectMenu.ResultKind.SELECT_SERVANT:
                this.state = State.INPUT_SERVANT_SELECT;
                this.supportServantSelectMenu.Open(this.supportServantData, classPos, new SupportServantSelectMenu.CallbackFunc(this.OnBackSelect));
                return;

            case SupportSelectMenu.ResultKind.SELECT_EQUIP:
                this.state = State.INPUT_EQUIP_SELECT;
                SingletonMonoBehaviour<CommonUI>.Instance.OpenSupportServantEquipListMenu(this.supportServantData, classPos, new SupportServantEquipListMenu.CallbackFunc(this.EndSupportServantEquipListMenu));
                return;

            case SupportSelectMenu.ResultKind.SELECT_FOLLOWER:
            {
                ServantLeaderInfo info = this.supportServantData.getUserServantLearderEntity(this.classPos).getServantLeaderInfo();
                if (((info == null) || (info.svtId == 0)) || (this.supportInfoJump == null))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.supportSelectMenu.Active();
                    return;
                }
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                this.supportInfoJump.SetSelectClassId(this.classPos);
                if (!this.supportInfoJump.ReturnScene(SceneManager.FadeType.BLACK, this.supportInfoJump))
                {
                    SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, this.supportInfoJump);
                    return;
                }
                return;
            }
            case SupportSelectMenu.ResultKind.SUPPORT_INFO_SERVANT:
            {
                this.state = State.INFO;
                ServantLeaderInfo servantLeaderInfo = this.supportServantData.getUserServantLearderEntity(this.classPos).getServantLeaderInfo();
                if ((servantLeaderInfo == null) || (servantLeaderInfo.svtId == 0))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.supportSelectMenu.Active();
                    return;
                }
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(this.supportServantData.Kind, servantLeaderInfo, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                return;
            }
            case SupportSelectMenu.ResultKind.SUPPORT_INFO_EQUIP:
                this.state = State.INFO;
                if (this.supportServantData.getEquip(this.classPos) == 0)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                    this.supportSelectMenu.Active();
                    return;
                }
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                entity = this.supportServantData.getUserServantLearderEntity(this.classPos);
                if (this.supportServantData.Kind != ServantStatusDialog.Kind.FOLLOWER)
                {
                    kind = ServantStatusDialog.Kind.FRIEND_SERVANT_EQUIP;
                    break;
                }
                kind = ServantStatusDialog.Kind.FOLLOWER_SERVANT_EQUIP;
                break;

            default:
                return;
        }
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(kind, entity.equipTarget1, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
    }

    protected void EndSupportServantEquipListMenu(ResultKind result, int classPos, SupportServantEquipListViewItem item)
    {
        this.state = State.SELECT;
        if (result == ResultKind.DECIDE)
        {
            long svtId = (item == null) ? 0L : item.UserServant.id;
            this.supportServantData.setEquipData(classPos, svtId);
            SingletonMonoBehaviour<CommonUI>.Instance.CloseSupportServantEquipListMenu(new System.Action(this.EndCloseServantEquipListDecide));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseSupportServantEquipListMenu(new System.Action(this.EndCloseServantEquipListCancel));
        }
        this.resetButtonUpdate();
    }

    public void Init()
    {
    }

    private bool isUpdate(bool servant = true, bool equip = true)
    {
        for (int i = 0; i < BalanceConfig.SupportDeckMax; i++)
        {
            if (servant && (this.supportServantData.getServant(i) != this.supportServantData.getOldServant(i)))
            {
                return true;
            }
            if (equip && (this.supportServantData.getEquip(i) != this.supportServantData.getOldEquip(i)))
            {
                return true;
            }
        }
        return false;
    }

    public void OnBackSelect(ResultKind result, int classPos, UserServantEntity entity)
    {
        this.state = State.SELECT;
        switch (result)
        {
            case ResultKind.DECIDE:
                this.supportServantData.setServantData(classPos, entity);
                break;

            case ResultKind.REMOVE:
                this.supportServantData.removeServantData(classPos);
                break;
        }
        this.supportSelectMenu.Redisp();
        this.resetButtonUpdate();
    }

    public void OnClickAllReset()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        this.supportSelectConfirmMenu.Open(SupportSelectConfirmMenu.Kind.ALL_CLEAR, new SupportSelectConfirmMenu.CallbackFunc(this.EndAllReset));
    }

    public void OnClickBack()
    {
        if (this.supportInfoJump != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.supportInfoJump.SetSelectClassId(-1);
            if (!this.supportInfoJump.ReturnScene(SceneManager.FadeType.BLACK, this.supportInfoJump))
            {
                SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, this.supportInfoJump);
            }
        }
        else if (this.isUpdate(true, true))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.cancelConfirmMenu.Open(CancelConfirmMenu.Kind.CANCEL, this.supportServantData, new CancelConfirmMenu.CallbackFunc(this.EndCancelConfirmMenu));
        }
        else
        {
            this.ReturnScene("ok");
        }
    }

    public void OnClickEquipAllReset()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        this.supportSelectConfirmMenu.Open(SupportSelectConfirmMenu.Kind.EQUIP_CLEAR, new SupportSelectConfirmMenu.CallbackFunc(this.EndEquipAllReset));
    }

    public void OnClickServantAllReset()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
        this.supportSelectConfirmMenu.Open(SupportSelectConfirmMenu.Kind.SERVANT_CLEAR, new SupportSelectConfirmMenu.CallbackFunc(this.EndServantAllReset));
    }

    public void Quit()
    {
        this.RefreshInfo();
        this.state = State.INIT;
    }

    public void RefreshInfo()
    {
    }

    private void resetButtonUpdate()
    {
        int num = 0;
        if (this.supportServantData.getServantSum() > 0)
        {
            this.allResetServantButton.SetState(UICommonButtonColor.State.Normal, true);
        }
        else
        {
            this.allResetServantButton.SetState(UICommonButtonColor.State.Disabled, true);
            num++;
        }
        if (this.supportServantData.getEquipSum() > 0)
        {
            this.allResetEquipButton.SetState(UICommonButtonColor.State.Normal, true);
        }
        else
        {
            this.allResetEquipButton.SetState(UICommonButtonColor.State.Disabled, true);
            num++;
        }
        if (num == 2)
        {
            this.allResetButton.SetState(UICommonButtonColor.State.Disabled, true);
        }
        else
        {
            this.allResetButton.SetState(UICommonButtonColor.State.Normal, true);
        }
    }

    private void ReturnScene(string result)
    {
        Debug.Log("ReturnScene");
        this.state = State.INIT;
        if (result == "ng")
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            this.supportSelectMenu.Reset(-1);
        }
        else
        {
            if (this.isUpdate(true, true))
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            }
            if (SingletonMonoBehaviour<SceneManager>.Instance.IsStackScene())
            {
                SingletonMonoBehaviour<SceneManager>.Instance.popScene(SceneManager.FadeType.BLACK, null);
            }
            else
            {
                SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Formation, SceneManager.FadeType.BLACK, null);
            }
        }
    }

    protected void SetCacheAssetNameList(string[] list)
    {
        string[] cacheAssetNameList = this.cacheAssetNameList;
        if (list != null)
        {
            AssetManager.loadAssetStorage(list, null);
        }
        this.cacheAssetNameList = list;
        if (cacheAssetNameList != null)
        {
            AssetManager.releaseAssetStorage(cacheAssetNameList);
        }
    }

    public enum ResultKind
    {
        CANCEL,
        DECIDE,
        REMOVE
    }

    protected enum State
    {
        INIT,
        SELECT,
        INPUT_SERVANT_SELECT,
        INPUT_EQUIP_SELECT,
        INFO,
        QUIT_SCENE
    }
}

