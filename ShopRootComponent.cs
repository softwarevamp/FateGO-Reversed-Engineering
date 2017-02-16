using System;
using UnityEngine;

public class ShopRootComponent : SceneRootComponent
{
    [SerializeField]
    protected ShopBuyBulkItemConfirmMenu buyBulkItemConfirmMenu;
    private int buyCount = 1;
    [SerializeField]
    protected ShopBuyItemConfirmMenu buyItemConfirmMenu;
    [SerializeField]
    protected GameObject buyItemListViewBase;
    [SerializeField]
    protected ShopBuyItemListViewManager buyItemListViewManager;
    protected static readonly float CLOSE_TIME = 0.3f;
    [SerializeField]
    protected EventBannerComponent eventBanner;
    [SerializeField]
    protected GameObject eventListViewBase;
    [SerializeField]
    protected ShopEventListViewManager eventListViewManager;
    protected static int FIGURE_ID = 0x7a4a4;
    protected SceneJumpInfo jumpInfo;
    [SerializeField]
    protected UILabel manaInfoLabel;
    [SerializeField]
    protected CommonMessageManager messageManager;
    protected static readonly float OPEN_TIME = 0.3f;
    protected string requestVoiceData;
    protected int selectEventNum;
    protected int selectItemNum;
    protected long[] selectServantIdList;
    [SerializeField]
    protected ServantSellConfirmMenu servantSellConfirmMenu;
    [SerializeField]
    protected ServantSellMenu servantSellMenu;
    [SerializeField]
    protected UIPanel shopEventItemDrawBase;
    [SerializeField]
    protected StandFigureBack standFigureBack;
    protected State state;
    [SerializeField]
    protected UILabel stoneInfoLabel;
    [SerializeField]
    protected TitleInfoControl titleInfo;
    [SerializeField]
    protected GameObject topListViewBase;
    [SerializeField]
    protected ShopTopListViewManager topListViewManager;
    protected string voiceData;
    protected SePlayer voicePlayer;

    public void BackBuyAccountItem()
    {
        if (this.state == State.INPUT_BUY_ACCOUNT_ITEM)
        {
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.RETRY, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.state = State.QUIT_BUY_ACCOUNT_ITEM;
        }
    }

    public void BackBuyAccountItemConfirm()
    {
        if (this.state == State.INPUT_BUY_ACCOUNT_ITEM_CONFIRM)
        {
            this.state = State.INPUT_BUY_ACCOUNT_ITEM;
            this.buyItemConfirmMenu.Close();
            this.buyItemListViewManager.CreateList(ShopBuyItemListViewManager.Kind.ACCOUNT);
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
        }
    }

    public void BackBuyEventBulkItemConfirm()
    {
        if (this.state == State.INPUT_BUY_EVENT_ITEM_CONFIRM)
        {
            this.buyBulkItemConfirmMenu.Close();
            if (this.buyItemListViewManager.ModifyList())
            {
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.MODIFY);
            }
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
            this.state = State.INPUT_BUY_EVENT_ITEM;
        }
    }

    public void BackBuyEventItem()
    {
        if (this.state == State.INPUT_BUY_EVENT_ITEM)
        {
            if (((this.jumpInfo != null) && (this.jumpInfo.Name == "EventItem")) && ((this.jumpInfo.Id != 0) && this.jumpInfo.ReturnScene()))
            {
                TerminalPramsManager.IsAutoResume = true;
                this.state = State.QUIT_BUY_EVENT_ITEM;
            }
            else
            {
                this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.RETRY, new System.Action(this.OnMoveEnd));
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
                TweenAlpha.Begin(this.shopEventItemDrawBase.gameObject, CLOSE_TIME, 0f);
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
                this.state = State.QUIT_BUY_EVENT_ITEM;
            }
        }
    }

    public void BackBuyEventItemConfirm()
    {
        if (this.state == State.INPUT_BUY_EVENT_ITEM_CONFIRM)
        {
            this.buyItemConfirmMenu.Close();
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            ShopEntity shop = item.Shop;
            if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopMaster>(DataNameKind.Kind.SHOP).IsOpenNoQuestEvent(shop))
            {
                TweenAlpha.Begin(this.shopEventItemDrawBase.gameObject, CLOSE_TIME, 0f);
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
                this.state = State.INPUT_EVENT;
                this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.INPUT, new ShopEventListViewManager.CallbackFunc(this.OnSelectEvent));
                this.buyItemListViewManager.DestroyList();
                this.buyItemListViewBase.SetActive(false);
                base.myFSM.SendEvent("NO_QUEST_SHOP_FINISH");
            }
            else
            {
                item.Modify(shop);
                if (this.buyItemListViewManager.ModifyList())
                {
                    this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.MODIFY);
                }
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
                this.state = State.INPUT_BUY_EVENT_ITEM;
                base.myFSM.SendEvent("NORMAL_FINISH");
            }
        }
    }

    public void BackBuyEventItemConfirmCancel()
    {
        if (this.state == State.INPUT_BUY_EVENT_ITEM_CONFIRM)
        {
            this.buyItemConfirmMenu.Close();
            if (this.buyItemListViewManager.ModifyList())
            {
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.MODIFY);
            }
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
            this.state = State.INPUT_BUY_EVENT_ITEM;
        }
    }

    public void BackBuyManaBulkItemConfirm()
    {
        if (this.state == State.INPUT_BUY_MANA_ITEM_CONFIRM)
        {
            this.buyBulkItemConfirmMenu.Close();
            if (this.buyItemListViewManager.ModifyList())
            {
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.MODIFY);
            }
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
            this.state = State.INPUT_BUY_MANA_ITEM;
        }
    }

    public void BackBuyManaItem()
    {
        if (this.state == State.INPUT_BUY_MANA_ITEM)
        {
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.RETRY, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.state = State.QUIT_BUY_MANA_ITEM;
        }
    }

    public void BackBuyManaItemConfirm()
    {
        if (this.state == State.INPUT_BUY_MANA_ITEM_CONFIRM)
        {
            this.buyItemConfirmMenu.Close();
            if (this.buyItemListViewManager.ModifyList())
            {
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.MODIFY);
            }
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
            this.state = State.INPUT_BUY_MANA_ITEM;
        }
    }

    public void BackBuyQpItem()
    {
        if (this.state == State.INPUT_BUY_QP_ITEM)
        {
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.RETRY, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.state = State.QUIT_BUY_QP_ITEM;
        }
    }

    public void BackBuyQpItemConfirm()
    {
        if (this.state == State.INPUT_BUY_QP_ITEM_CONFIRM)
        {
            this.buyItemConfirmMenu.Close();
            if (this.buyItemListViewManager.ModifyList())
            {
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.MODIFY);
            }
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
            this.state = State.INPUT_BUY_QP_ITEM;
        }
    }

    public void BackBuyStoneItem()
    {
        if (this.state == State.INPUT_BUY_STONE_ITEM)
        {
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.RETRY, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.state = State.QUIT_BUY_STONE_ITEM;
        }
    }

    public void BackBuyStoneItemConfirm()
    {
        if (this.state == State.INPUT_BUY_STONE_ITEM_CONFIRM)
        {
            this.buyItemConfirmMenu.Close();
            if (this.buyItemListViewManager.ModifyList())
            {
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.MODIFY);
            }
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
            this.state = State.INPUT_BUY_STONE_ITEM;
        }
    }

    public void BackEvent()
    {
        if (this.state == State.INPUT_EVENT)
        {
            if (((this.jumpInfo != null) && (this.jumpInfo.Name == "EventItem")) && this.jumpInfo.ReturnScene())
            {
                this.state = State.QUIT_EVENT;
            }
            else
            {
                this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.RETRY, new System.Action(this.OnMoveEnd));
                this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.EXIT, new System.Action(this.OnMoveEnd));
                this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
                this.state = State.QUIT_EVENT;
            }
        }
    }

    public void BackSellServant()
    {
        if (this.state == State.INPUT_SELL_SERVANT)
        {
            this.state = State.QUIT_SELL_SERVANT;
            this.standFigureBack.Fadein(null);
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.RETRY, new System.Action(this.OnMoveEnd));
            this.servantSellMenu.Close(new System.Action(this.OnMoveEnd));
        }
    }

    public void BackSellServantConfirm()
    {
        if (this.state == State.INPUT_SELL_SERVANT_CONFIRM)
        {
            this.state = State.INPUT_SELL_SERVANT;
            this.servantSellConfirmMenu.Close();
            this.servantSellMenu.Open(new ServantSellMenu.CallbackFunc(this.OnSelectSellServant), null);
        }
    }

    public override void beginFinish()
    {
        this.Quit();
    }

    public override void beginInitialize()
    {
        base.beginInitialize();
        base.setMainMenuBar(MainMenuBar.Kind.SHOP, 30);
        base.hideUserStatus();
        this.eventBanner.SetBanner();
        this.servantSellMenu.Init(ServantOperationManager.Kind.SERVANT);
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginResume()
    {
        Debug.Log("Call beginResume");
        base.hideUserStatus();
        base.beginResume();
    }

    public override void beginStartUp(object data)
    {
        this.jumpInfo = null;
        if ((data != null) && (data is SceneJumpInfo))
        {
            this.jumpInfo = data as SceneJumpInfo;
        }
        SoundManager.playBgm("BGM_CHALDEA_1");
        this.titleInfo.setTitleInfo(base.myFSM, true, null, TitleInfoControl.TitleKind.SHOP);
        this.titleInfo.SetHelpBtn(false);
        this.RefreshInfo();
        this.standFigureBack.Set(FIGURE_ID, 0, Face.Type.NORMAL, null);
        MainMenuBar.setMenuActive(true, null);
        this.LoadBanner();
    }

    public void BuyBankItem()
    {
        if ((this.state == State.INPUT_TOP) && SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
        {
            this.state = State.INPUT_BUY_BANK_ITEM;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenStonePurchaseMenu(new StonePurchaseMenu.CallbackFunc(this.EndBuyBankItem), new System.Action(this.RefreshInfo));
        }
    }

    public void BuyServantEquipFrame()
    {
        if (this.state == State.INPUT_TOP)
        {
            this.state = State.INPUT_BUY_SERVANT_EQUIP_FRAME;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipFramePurchaseMenu(new ServantEquipFramePurchaseMenu.CallbackFunc(this.EndBuyServantEquipFrame), new System.Action(this.RefreshInfo));
        }
    }

    public void BuyServantFrame()
    {
        if (this.state == State.INPUT_TOP)
        {
            this.state = State.INPUT_BUY_SERVANT_FRAME;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenServantFramePurchaseMenu(new ServantFramePurchaseMenu.CallbackFunc(this.EndBuyServantFrame), new System.Action(this.RefreshInfo));
        }
    }

    protected void EndBuyBankItem(StonePurchaseMenu.Result result)
    {
        this.state = State.INPUT_TOP;
        if (result == StonePurchaseMenu.Result.CANCEL)
        {
            this.PlayVoice("I020");
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseStonePurchaseMenu();
        this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
        base.myFSM.SendEvent((result != StonePurchaseMenu.Result.PURCHASE) ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    protected void EndBuyServantEquipFrame(ServantEquipFramePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipFramePurchaseMenu();
        this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
        this.state = State.INPUT_TOP;
        switch (result)
        {
            case ServantEquipFramePurchaseMenu.Result.CANCEL:
                base.myFSM.SendEvent("MENU_CANCEL");
                break;

            case ServantEquipFramePurchaseMenu.Result.ERROR:
                base.myFSM.SendEvent("REQUEST_NG");
                break;

            case ServantEquipFramePurchaseMenu.Result.PURCHASE:
                base.myFSM.SendEvent("REQUEST_OK");
                break;
        }
    }

    protected void EndBuyServantFrame(ServantFramePurchaseMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantFramePurchaseMenu();
        this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
        this.state = State.INPUT_TOP;
        switch (result)
        {
            case ServantFramePurchaseMenu.Result.CANCEL:
                base.myFSM.SendEvent("MENU_CANCEL");
                break;

            case ServantFramePurchaseMenu.Result.ERROR:
                base.myFSM.SendEvent("REQUEST_NG");
                break;

            case ServantFramePurchaseMenu.Result.PURCHASE:
                base.myFSM.SendEvent("REQUEST_OK");
                break;
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
            int num = UnityEngine.Random.Range(0, 2);
            this.PlayVoice((num != 0) ? "I040" : "I010");
        }
    }

    protected void EndRecoverUserGameRecover(UserGameActRecoverMenu.Result result)
    {
        switch (result)
        {
            case UserGameActRecoverMenu.Result.CANCEL:
                base.myFSM.SendEvent("MENU_CANCEL");
                break;

            case UserGameActRecoverMenu.Result.ERROR:
                base.myFSM.SendEvent("REQUEST_NG");
                break;

            case UserGameActRecoverMenu.Result.RECOVER:
                base.myFSM.SendEvent("REQUEST_OK");
                break;
        }
        this.state = State.INPUT_TOP;
        SingletonMonoBehaviour<CommonUI>.Instance.CloseUserGameActRecoverMenu();
        this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
    }

    protected void EndRequestBuyAccountItem(string result)
    {
        Debug.Log("EndRequestBuyStoneItem [" + result + "]");
        this.RefreshInfo();
        if (result != "ng")
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected void EndRequestSellServant(string result)
    {
        Debug.Log("EndRequestSellServant [" + result + "]");
        this.servantSellMenu.Init();
        this.servantSellConfirmMenu.Init();
        ServantOperationManager.Kind tabKind = this.servantSellMenu.GetTabKind();
        if (result != "ng")
        {
            this.servantSellConfirmMenu.Open((tabKind != ServantOperationManager.Kind.SERVANT) ? ServantSellConfirmMenu.Kind.SELL_SERVANT_EQUIP : ServantSellConfirmMenu.Kind.SELL_SERVANT, this.selectServantIdList, new ServantSellConfirmMenu.CallbackFunc(this.EndSellResultServant));
        }
        else
        {
            this.servantSellConfirmMenu.Open((tabKind != ServantOperationManager.Kind.SERVANT) ? ServantSellConfirmMenu.Kind.SELL_ERROR_SERVANT_EQUIP : ServantSellConfirmMenu.Kind.SELL_ERROR_SERVANT, this.selectServantIdList, new ServantSellConfirmMenu.CallbackFunc(this.EndSellResultServant));
        }
        this.RefreshInfo();
    }

    protected void EndRequestShop(string result)
    {
        Debug.Log("EndRequestShop [" + result + "]");
        if (result != "ng")
        {
            bool flag;
            bool flag2;
            string str3;
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            item.GetSendType(out flag, out flag2);
            ShopEntity shop = item.Shop;
            string sendBulkNameText = item.SendBulkNameText;
            string numberFormat = LocalizationManager.GetNumberFormat((int) (shop.setNum * this.buyCount));
            if (item.IsExchangeQP())
            {
                sendBulkNameText = string.Format(sendBulkNameText, numberFormat);
            }
            else if (numberFormat != null)
            {
                sendBulkNameText = sendBulkNameText + string.Format(LocalizationManager.Get("SHOP_BUY_SUCCESS_MULTIPLE"), numberFormat);
            }
            if (flag && flag2)
            {
                str3 = string.Format(LocalizationManager.Get("SHOP_BUY_SUCCESS_SEND_MIX"), sendBulkNameText);
            }
            else if (flag2)
            {
                str3 = string.Format(LocalizationManager.Get("SHOP_BUY_SUCCESS_SEND_PRESENT_BOX"), sendBulkNameText);
            }
            else
            {
                str3 = string.Format(LocalizationManager.Get("SHOP_BUY_SUCCESS"), sendBulkNameText);
            }
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, str3, new System.Action(this.OnEndDialogRequestShop), -1);
            this.RefreshInfo();
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_NG");
        }
    }

    protected void EndSellResultServant(int count)
    {
        Debug.Log("EndSellResultServant:");
        base.myFSM.SendEvent("REQUEST_OK");
    }

    public void Init()
    {
        if (this.state == State.INIT)
        {
            if (this.jumpInfo != null)
            {
                if (this.jumpInfo.Name == "EventItem")
                {
                    this.topListViewBase.SetActive(true);
                    this.topListViewManager.CreateList(ShopTopListViewManager.Kind.NORMAL);
                    this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTERED, 0);
                    this.eventListViewBase.SetActive(true);
                    this.eventListViewManager.CreateList(ShopEventListViewManager.Kind.NORMAL);
                    ShopEventListViewItem item = this.eventListViewManager.SearchItem(this.jumpInfo.Id);
                    this.selectEventNum = (item == null) ? 0 : item.Index;
                    if (this.jumpInfo.Id > 0)
                    {
                        this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.ENTERED);
                        this.buyItemListViewBase.SetActive(true);
                        this.buyItemListViewManager.CreateList(this.jumpInfo.Id);
                        this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
                        if (this.buyItemListViewManager.EventItemCount > 0)
                        {
                            this.shopEventItemDrawBase.gameObject.SetActive(true);
                            this.shopEventItemDrawBase.alpha = 0f;
                            TweenAlpha.Begin(this.shopEventItemDrawBase.gameObject, OPEN_TIME, 1f);
                        }
                        else
                        {
                            this.shopEventItemDrawBase.gameObject.SetActive(false);
                        }
                        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
                        this.state = State.INIT_BUY_EVENT_ITEM2;
                        base.myFSM.SendEvent("MENU_EVENT_ITEM");
                    }
                    else
                    {
                        this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
                        this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
                        this.state = State.INIT_EVENT2;
                        base.myFSM.SendEvent("MENU_EVENT");
                    }
                    return;
                }
                if (this.jumpInfo.Name == "SellServant")
                {
                    this.topListViewBase.SetActive(true);
                    this.topListViewManager.CreateList(ShopTopListViewManager.Kind.NORMAL);
                    this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTERED, 0);
                    this.standFigureBack.Fadeout(null);
                    this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP_SELL_SERVANT);
                    this.titleInfo.SetHelpBtn(false);
                    this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
                    this.servantSellMenu.Init((ServantOperationManager.Kind) this.jumpInfo.Id);
                    this.servantSellMenu.Open(null, new System.Action(this.OnMoveEnd));
                    this.state = State.INIT_SELL_SERVANT;
                    base.myFSM.SendEvent("MENU_SELL_SERVANT");
                    this.jumpInfo = null;
                    return;
                }
            }
            this.topListViewBase.SetActive(true);
            this.topListViewManager.CreateList(ShopTopListViewManager.Kind.NORMAL);
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.state = State.INIT_TOP;
        }
    }

    protected void LoadBanner()
    {
        AtlasManager.LoadBanner(delegate {
            AccountingManager.SetEnableStore(true);
            this.LoadVoice();
            base.beginStartUp();
        });
    }

    protected void LoadVoice()
    {
        if ((this.requestVoiceData == null) && (this.voiceData == null))
        {
            this.requestVoiceData = "ChrVoice_" + FIGURE_ID;
            SoundManager.loadAudioAssetStorage(this.requestVoiceData, new System.Action(this.EndLoadVoice), SoundManager.CueType.ALL);
        }
    }

    public void OnClickBack()
    {
        State state = this.state;
        switch (state)
        {
            case State.INPUT_TOP:
                SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
                if ((this.jumpInfo == null) || !this.jumpInfo.IsEnableReturnScene())
                {
                    base.myFSM.SendEvent("CLICK_BACK");
                    return;
                }
                base.myFSM.SendEvent("CLICK_RETURN");
                return;

            case State.INPUT_BUY_QP_ITEM:
            case State.INPUT_SELL_SERVANT:
            case State.INPUT_BUY_BANK_ITEM:
                break;

            default:
                switch (state)
                {
                    case State.INPUT_BUY_MANA_ITEM:
                    case State.INPUT_BUY_STONE_ITEM:
                    case State.INPUT_EVENT:
                    case State.INPUT_BUY_EVENT_ITEM:
                    case State.INPUT_BUY_ACCOUNT_ITEM:
                        break;

                    default:
                        return;
                }
                break;
        }
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        base.myFSM.SendEvent("CLICK_BACK");
    }

    protected void OnEndDialogRequestShop()
    {
        ShopEntity shop = this.buyItemListViewManager.GetItem(this.selectItemNum).Shop;
        ShopScriptEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopScriptMaster>(DataNameKind.Kind.SHOP_SCRIPT).getEntityFromId(shop.id);
        if ((entity2 != null) && !string.IsNullOrEmpty(entity2.scriptId))
        {
            ScriptManager.PlayShop(entity2.scriptId, new ScriptManager.CallbackFunc(this.OnEndPlayScriptRequestShop), false);
        }
        else
        {
            base.myFSM.SendEvent("REQUEST_OK");
        }
    }

    protected void OnEndFadeRequestShop()
    {
        SoundManager.playBgm("BGM_CHALDEA_1");
        base.myFSM.SendEvent("REQUEST_OK");
    }

    protected void OnEndPlayScriptRequestShop(bool isExit)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(1f, new System.Action(this.OnEndFadeRequestShop));
    }

    private void OnMoveEnd()
    {
        switch (this.state)
        {
            case State.INIT_TOP:
                this.state = State.INPUT_TOP;
                if (!TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_SHOP))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenTutorialImageDialog(TutorialFlag.ImageId.SHOP_TOP, TutorialFlag.Id.TUTORIAL_LABEL_SHOP, null);
                }
                this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
                this.titleInfo.SetHelpBtn(true);
                break;

            case State.INIT_BUY_QP_ITEM:
                this.state = State.INIT_BUY_QP_ITEM2;
                break;

            case State.INIT_BUY_QP_ITEM2:
                this.state = State.INPUT_BUY_QP_ITEM;
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
                break;

            case State.QUIT_BUY_QP_ITEM:
                this.state = State.QUIT_BUY_QP_ITEM2;
                break;

            case State.QUIT_BUY_QP_ITEM2:
                this.state = State.INPUT_TOP;
                this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
                this.buyItemListViewManager.DestroyList();
                this.buyItemListViewBase.SetActive(false);
                this.titleInfo.SetHelpBtn(true);
                break;

            case State.INIT_BUY_MANA_ITEM:
                this.state = State.INIT_BUY_MANA_ITEM2;
                break;

            case State.INIT_BUY_MANA_ITEM2:
                this.state = State.INPUT_BUY_MANA_ITEM;
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
                break;

            case State.QUIT_BUY_MANA_ITEM:
                this.state = State.QUIT_BUY_MANA_ITEM2;
                break;

            case State.QUIT_BUY_MANA_ITEM2:
                this.state = State.INPUT_TOP;
                this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
                this.buyItemListViewManager.DestroyList();
                this.buyItemListViewBase.SetActive(false);
                this.titleInfo.SetHelpBtn(true);
                break;

            case State.INIT_BUY_STONE_ITEM:
                this.state = State.INIT_BUY_STONE_ITEM2;
                break;

            case State.INIT_BUY_STONE_ITEM2:
                this.state = State.INPUT_BUY_STONE_ITEM;
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
                break;

            case State.QUIT_BUY_STONE_ITEM:
                this.state = State.QUIT_BUY_STONE_ITEM2;
                break;

            case State.QUIT_BUY_STONE_ITEM2:
                this.state = State.INPUT_TOP;
                this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
                this.buyItemListViewManager.DestroyList();
                this.buyItemListViewBase.SetActive(false);
                this.titleInfo.SetHelpBtn(true);
                break;

            case State.INIT_EVENT:
                this.state = State.INIT_EVENT2;
                break;

            case State.INIT_EVENT2:
                this.state = State.INPUT_EVENT;
                this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.INPUT, new ShopEventListViewManager.CallbackFunc(this.OnSelectEvent));
                break;

            case State.QUIT_EVENT:
                this.state = State.QUIT_EVENT2;
                break;

            case State.QUIT_EVENT2:
                this.state = State.INPUT_TOP;
                this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
                this.eventListViewManager.DestroyList();
                this.eventListViewBase.SetActive(false);
                this.titleInfo.SetHelpBtn(true);
                break;

            case State.INIT_BUY_EVENT_ITEM:
                this.state = State.INIT_BUY_EVENT_ITEM2;
                break;

            case State.INIT_BUY_EVENT_ITEM2:
                this.state = State.INPUT_BUY_EVENT_ITEM;
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
                break;

            case State.QUIT_BUY_EVENT_ITEM:
                this.state = State.QUIT_BUY_EVENT_ITEM2;
                break;

            case State.QUIT_BUY_EVENT_ITEM2:
                this.state = State.INPUT_EVENT;
                this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.INPUT, new ShopEventListViewManager.CallbackFunc(this.OnSelectEvent));
                this.buyItemListViewManager.DestroyList();
                this.buyItemListViewBase.SetActive(false);
                break;

            case State.INIT_BUY_ACCOUNT_ITEM:
                this.state = State.INIT_BUY_ACCOUNT_ITEM2;
                break;

            case State.INIT_BUY_ACCOUNT_ITEM2:
                this.state = State.INPUT_BUY_ACCOUNT_ITEM;
                this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INPUT, new ShopBuyItemListViewManager.CallbackFunc(this.OnSelectBuyItem));
                break;

            case State.QUIT_BUY_ACCOUNT_ITEM:
                this.state = State.QUIT_BUY_ACCOUNT_ITEM2;
                break;

            case State.QUIT_BUY_ACCOUNT_ITEM2:
                this.state = State.INPUT_TOP;
                this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
                this.buyItemListViewManager.DestroyList();
                this.buyItemListViewBase.SetActive(false);
                this.titleInfo.SetHelpBtn(true);
                break;

            case State.INIT_SELL_SERVANT:
                this.state = State.INIT_SELL_SERVANT2;
                break;

            case State.INIT_SELL_SERVANT2:
                this.state = State.INPUT_SELL_SERVANT;
                this.servantSellMenu.Open(new ServantSellMenu.CallbackFunc(this.OnSelectSellServant), null);
                break;

            case State.QUIT_SELL_SERVANT:
                this.state = State.QUIT_SELL_SERVANT2;
                break;

            case State.QUIT_SELL_SERVANT2:
                this.state = State.INPUT_TOP;
                this.servantSellMenu.Init();
                this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.INPUT, new ShopTopListViewManager.CallbackFunc(this.OnSelectTop));
                this.titleInfo.SetHelpBtn(true);
                break;
        }
    }

    protected void OnSelectBuyItem(int n)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.selectItemNum = n;
        ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
        ShopEntity shop = item.Shop;
        if (shop.IsSoldOut())
        {
            base.myFSM.SendEvent("MENU_SELECT_ITEM");
        }
        else if (!shop.IsEnable(0L))
        {
            base.myFSM.SendEvent("MENU_SELECT_ITEM");
        }
        else if ((shop.limitNum - item.ToTalNum) == 1)
        {
            base.myFSM.SendEvent("MENU_SELECT_ITEM");
        }
        else
        {
            switch (shop.GetPayType())
            {
                case PayType.Type.MANA:
                    if (SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).mana >= (shop.GetPrice() * 2))
                    {
                        base.myFSM.SendEvent("MENU_SELECT_BULK_ITEM");
                        return;
                    }
                    base.myFSM.SendEvent("MENU_SELECT_ITEM");
                    return;

                case PayType.Type.EVENT_ITEM:
                    if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, shop.GetItemID()).num >= (shop.GetPrice() * 2))
                    {
                        base.myFSM.SendEvent("MENU_SELECT_BULK_ITEM");
                        return;
                    }
                    base.myFSM.SendEvent("MENU_SELECT_ITEM");
                    return;
            }
            base.myFSM.SendEvent("MENU_SELECT_ITEM");
        }
    }

    protected void OnSelectEvent(int n)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.selectEventNum = n;
        base.myFSM.SendEvent("MENU_SELECT_EVENT");
    }

    protected void OnSelectSellServant(ServantSellMenu.ResultKind kind, long[] list)
    {
        if (this.state == State.INPUT_SELL_SERVANT)
        {
            this.selectServantIdList = list;
            if (kind == ServantSellMenu.ResultKind.DECIDE)
            {
                if (this.selectServantIdList != null)
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                }
                else
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
                }
                base.myFSM.SendEvent("MENU_SELECT_ITEM");
            }
        }
    }

    protected void OnSelectTop(string result)
    {
        if (result != null)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            if ((result == "MENU_BUY_BANK_ITEM") || (result == "MENU_BUY_MANA_ITEM"))
            {
                this.PlayVoice("I030");
            }
            base.myFSM.SendEvent(result);
        }
    }

    public bool PlayCancelVoice()
    {
        this.PlayVoice("I020");
        return true;
    }

    protected void PlayVoice(string name)
    {
        if (this.voiceData != null)
        {
            if (this.voicePlayer != null)
            {
                this.voicePlayer.StopSe(0f);
            }
            this.voicePlayer = SoundManager.playVoice(this.voiceData, "0_" + name, SoundManager.DEFAULT_VOLUME, null);
        }
    }

    public void Quit()
    {
        this.standFigureBack.Init();
        this.requestVoiceData = null;
        if (this.voicePlayer != null)
        {
            this.voicePlayer.StopSe(0f);
            this.voicePlayer = null;
        }
        if (this.voiceData != null)
        {
            SoundManager.releaseAudioAssetStorage(this.voiceData);
            this.voiceData = null;
        }
        this.topListViewManager.DestroyList();
        this.buyItemListViewManager.DestroyList();
        this.eventListViewManager.DestroyList();
        this.topListViewBase.SetActive(false);
        this.buyItemListViewBase.SetActive(false);
        this.eventListViewBase.SetActive(false);
        TweenAlpha alpha = TweenAlpha.Begin(this.shopEventItemDrawBase.gameObject, CLOSE_TIME, 0f);
        alpha.value = 0f;
        alpha.enabled = false;
        this.buyItemConfirmMenu.Init();
        this.buyBulkItemConfirmMenu.Init();
        this.servantSellConfirmMenu.Init();
        this.servantSellMenu.Init();
        this.state = State.INIT;
    }

    public void RecoverUserGameAct()
    {
        if (this.state == State.INPUT_TOP)
        {
            this.state = State.INPUT_RECOVER_USER_GAME_ACT;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenUserGameActRecoverMenu(new UserGameActRecoverMenu.CallbackFunc(this.EndRecoverUserGameRecover), new System.Action(this.RefreshInfo));
        }
    }

    public void RefreshInfo()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.stoneInfoLabel.text = LocalizationManager.GetNumberFormat(entity.stone);
        this.manaInfoLabel.text = LocalizationManager.GetNumberFormat(entity.mana);
    }

    public void RequestBuyAccountItem()
    {
        ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
        if (item != null)
        {
            StoneShopEntity stoneShop = item.StoneShop;
            if (stoneShop != null)
            {
                NetworkManager.getRequest<PurchaseByStoneRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestBuyAccountItem)).beginRequest(stoneShop.id, 1);
                return;
            }
        }
        base.myFSM.SendEvent("REQUEST_NG");
    }

    public void RequestEventShop()
    {
        this.RequestShop();
    }

    public void RequestSellServant()
    {
        if (this.selectServantIdList != null)
        {
            Debug.Log("SELL SERVANT LIST " + this.selectServantIdList.ToString());
            NetworkManager.getRequest<SellServantRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestSellServant)).beginRequest(this.selectServantIdList);
        }
        else
        {
            this.EndRequestSellServant("ng");
        }
    }

    public void RequestShop()
    {
        ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
        if (item != null)
        {
            ShopEntity shop = item.Shop;
            if (shop != null)
            {
                NetworkManager.getRequest<PurchaseRequest>(new NetworkManager.ResultCallbackFunc(this.EndRequestShop)).beginRequest(shop.id, this.buyCount);
                return;
            }
        }
        base.myFSM.SendEvent("REQUEST_NG");
    }

    public bool ReturnCallScene()
    {
        if ((this.jumpInfo != null) && this.jumpInfo.ReturnScene())
        {
            return true;
        }
        MainMenuBar.requestTerminalSceneChange();
        return false;
    }

    public void SelectBuyAccountItem()
    {
        if ((this.state == State.INPUT_TOP) && SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
        {
            this.buyItemListViewBase.SetActive(true);
            this.buyItemListViewManager.CreateList(ShopBuyItemListViewManager.Kind.ACCOUNT);
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            this.shopEventItemDrawBase.alpha = 0f;
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.titleInfo.SetHelpBtn(false);
            this.state = State.INIT_BUY_ACCOUNT_ITEM;
        }
    }

    public void SelectBuyAccountItemConfirm()
    {
        if (this.state == State.INPUT_BUY_ACCOUNT_ITEM)
        {
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            this.buyItemConfirmMenu.Open(item.StoneShop, new ShopBuyItemConfirmMenu.CallbackFunc(this.SelectedBuyAccountItemConfirm));
            this.state = State.INPUT_BUY_ACCOUNT_ITEM_CONFIRM;
        }
    }

    public void SelectBuyEventBulkItemConfirm()
    {
        if (this.state == State.INPUT_BUY_EVENT_ITEM)
        {
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            this.buyBulkItemConfirmMenu.Open(item.Shop, item, new ShopBuyBulkItemConfirmMenu.CallbackFunc(this.SelectedBuyEventItemConfirm));
            this.state = State.INPUT_BUY_EVENT_ITEM_CONFIRM;
        }
    }

    public void SelectBuyEventItem()
    {
        if ((this.state == State.INPUT_EVENT) && SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
        {
            ShopEventListViewItem item = this.eventListViewManager.GetItem(this.selectEventNum);
            this.buyItemListViewBase.SetActive(true);
            this.buyItemListViewManager.CreateList(item.EventId);
            this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            if (this.buyItemListViewManager.EventItemCount > 0)
            {
                this.shopEventItemDrawBase.gameObject.SetActive(true);
                this.shopEventItemDrawBase.alpha = 0f;
                TweenAlpha.Begin(this.shopEventItemDrawBase.gameObject, OPEN_TIME, 1f);
            }
            else
            {
                this.shopEventItemDrawBase.gameObject.SetActive(false);
            }
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.titleInfo.SetHelpBtn(false);
            this.state = State.INIT_BUY_EVENT_ITEM;
        }
    }

    public void SelectBuyEventItemConfirm()
    {
        if (this.state == State.INPUT_BUY_EVENT_ITEM)
        {
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            this.buyItemConfirmMenu.Open(item.Shop, new ShopBuyItemConfirmMenu.CallbackFunc(this.SelectedBuyEventItemConfirm));
            this.state = State.INPUT_BUY_EVENT_ITEM_CONFIRM;
        }
    }

    public void SelectBuyManaBulkItemConfirm()
    {
        if (this.state == State.INPUT_BUY_MANA_ITEM)
        {
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            this.buyBulkItemConfirmMenu.Open(item.Shop, item, new ShopBuyBulkItemConfirmMenu.CallbackFunc(this.SelectedBuyManaItemConfirm));
            this.state = State.INPUT_BUY_MANA_ITEM_CONFIRM;
        }
    }

    public void SelectBuyManaItem()
    {
        if ((this.state == State.INPUT_TOP) && SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
        {
            this.buyItemListViewBase.SetActive(true);
            this.buyItemListViewManager.CreateList(ShopBuyItemListViewManager.Kind.MANA);
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            this.shopEventItemDrawBase.alpha = 0f;
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.titleInfo.SetHelpBtn(false);
            this.state = State.INIT_BUY_MANA_ITEM;
        }
    }

    public void SelectBuyManaItemConfirm()
    {
        if (this.state == State.INPUT_BUY_MANA_ITEM)
        {
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            this.buyItemConfirmMenu.Open(item.Shop, new ShopBuyItemConfirmMenu.CallbackFunc(this.SelectedBuyManaItemConfirm));
            this.state = State.INPUT_BUY_MANA_ITEM_CONFIRM;
        }
    }

    public void SelectBuyQpItem()
    {
        if ((this.state == State.INPUT_TOP) && SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
        {
            this.buyItemListViewBase.SetActive(true);
            this.buyItemListViewManager.CreateList(ShopBuyItemListViewManager.Kind.QP);
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            this.shopEventItemDrawBase.alpha = 0f;
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.titleInfo.SetHelpBtn(false);
            this.state = State.INIT_BUY_QP_ITEM;
        }
    }

    public void SelectBuyQpItemConfirm()
    {
        if (this.state == State.INPUT_BUY_QP_ITEM)
        {
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            this.buyItemConfirmMenu.Open(item.Shop, new ShopBuyItemConfirmMenu.CallbackFunc(this.SelectedBuyQpItemConfirm));
            this.state = State.INPUT_BUY_QP_ITEM_CONFIRM;
        }
    }

    public void SelectBuyStoneItem()
    {
        if ((this.state == State.INPUT_TOP) && SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
        {
            this.buyItemListViewBase.SetActive(true);
            this.buyItemListViewManager.CreateList(ShopBuyItemListViewManager.Kind.STONE);
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.buyItemListViewManager.SetMode(ShopBuyItemListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            this.shopEventItemDrawBase.alpha = 0f;
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.titleInfo.SetHelpBtn(false);
            this.state = State.INIT_BUY_STONE_ITEM;
        }
    }

    public void SelectBuyStoneItemConfirm()
    {
        if (this.state == State.INPUT_BUY_STONE_ITEM)
        {
            ShopBuyItemListViewItem item = this.buyItemListViewManager.GetItem(this.selectItemNum);
            this.buyItemConfirmMenu.Open(item.Shop, new ShopBuyItemConfirmMenu.CallbackFunc(this.SelectedBuyStoneItemConfirm));
            this.state = State.INPUT_BUY_STONE_ITEM_CONFIRM;
        }
    }

    protected void SelectedBuyAccountItemConfirm(int count)
    {
        base.myFSM.SendEvent((count <= 0) ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    protected void SelectedBuyEventItemConfirm(int count)
    {
        this.buyCount = count;
        base.myFSM.SendEvent((count <= 0) ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    protected void SelectedBuyManaItemConfirm(int count)
    {
        this.buyCount = count;
        base.myFSM.SendEvent((count <= 0) ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    protected void SelectedBuyQpItemConfirm(int count)
    {
        base.myFSM.SendEvent((count <= 0) ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    protected void SelectedBuyStoneItemConfirm(int count)
    {
        base.myFSM.SendEvent((count <= 0) ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    public void SelectEvent()
    {
        if ((this.state == State.INPUT_TOP) && SingletonMonoBehaviour<NetworkManager>.Instance.CheckServerLimitTime())
        {
            this.eventListViewBase.SetActive(true);
            this.eventListViewManager.CreateList(ShopEventListViewManager.Kind.NORMAL);
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.eventListViewManager.SetMode(ShopEventListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP);
            this.titleInfo.SetHelpBtn(false);
            this.state = State.INIT_EVENT;
        }
    }

    public void SelectSellServant()
    {
        if (this.state == State.INPUT_TOP)
        {
            this.state = State.INIT_SELL_SERVANT;
            this.standFigureBack.Fadeout(null);
            this.titleInfo.changeTitleInfo(true, TitleInfoControl.TitleKind.SHOP_SELL_SERVANT);
            this.titleInfo.SetHelpBtn(false);
            this.topListViewManager.SetMode(ShopTopListViewManager.InitMode.ENTER, new System.Action(this.OnMoveEnd));
            this.servantSellMenu.Open(null, new System.Action(this.OnMoveEnd));
        }
    }

    public void SelectSellServantConfirm()
    {
        if (this.state == State.INPUT_SELL_SERVANT)
        {
            this.state = State.INPUT_SELL_SERVANT_CONFIRM;
            ServantOperationManager.Kind tabKind = this.servantSellMenu.GetTabKind();
            if (this.selectServantIdList != null)
            {
                this.servantSellConfirmMenu.Open((tabKind != ServantOperationManager.Kind.SERVANT) ? ServantSellConfirmMenu.Kind.SELET_SERVANT_EQUIP : ServantSellConfirmMenu.Kind.SELET_SERVANT, this.selectServantIdList, new ServantSellConfirmMenu.CallbackFunc(this.SellServantConfirm));
            }
            else
            {
                this.servantSellConfirmMenu.Open((tabKind != ServantOperationManager.Kind.SERVANT) ? ServantSellConfirmMenu.Kind.NO_SELECT_SERVANT_EQUIP : ServantSellConfirmMenu.Kind.NO_SELECT_SERVANT, null, new ServantSellConfirmMenu.CallbackFunc(this.SellServantConfirm));
            }
        }
    }

    protected void SellServantConfirm(int count)
    {
        base.myFSM.SendEvent((count <= 0) ? "MENU_CANCEL" : "MENU_DECIDE");
    }

    protected enum State
    {
        INIT,
        INIT_TOP,
        INPUT_TOP,
        INIT_BUY_QP_ITEM,
        INIT_BUY_QP_ITEM2,
        INPUT_BUY_QP_ITEM,
        QUIT_BUY_QP_ITEM,
        QUIT_BUY_QP_ITEM2,
        INIT_BUY_MANA_ITEM,
        INIT_BUY_MANA_ITEM2,
        INPUT_BUY_MANA_ITEM,
        QUIT_BUY_MANA_ITEM,
        QUIT_BUY_MANA_ITEM2,
        INIT_BUY_STONE_ITEM,
        INIT_BUY_STONE_ITEM2,
        INPUT_BUY_STONE_ITEM,
        QUIT_BUY_STONE_ITEM,
        QUIT_BUY_STONE_ITEM2,
        INIT_EVENT,
        INIT_EVENT2,
        INPUT_EVENT,
        QUIT_EVENT,
        QUIT_EVENT2,
        INIT_BUY_EVENT_ITEM,
        INIT_BUY_EVENT_ITEM2,
        INPUT_BUY_EVENT_ITEM,
        QUIT_BUY_EVENT_ITEM,
        QUIT_BUY_EVENT_ITEM2,
        INIT_BUY_ACCOUNT_ITEM,
        INIT_BUY_ACCOUNT_ITEM2,
        INPUT_BUY_ACCOUNT_ITEM,
        QUIT_BUY_ACCOUNT_ITEM,
        QUIT_BUY_ACCOUNT_ITEM2,
        INIT_SELL_SERVANT,
        INIT_SELL_SERVANT2,
        INPUT_SELL_SERVANT,
        QUIT_SELL_SERVANT,
        QUIT_SELL_SERVANT2,
        INPUT_BUY_BANK_ITEM,
        INPUT_BUY_SERVANT_FRAME,
        INPUT_BUY_SERVANT_EQUIP_FRAME,
        INPUT_RECOVER_USER_GAME_ACT,
        INIT_BUY_QP_ITEM_CONFIRM,
        INPUT_BUY_QP_ITEM_CONFIRM,
        QUIT_BUY_QP_ITEM_CONFIRM,
        INIT_BUY_MANA_ITEM_CONFIRM,
        INPUT_BUY_MANA_ITEM_CONFIRM,
        QUIT_BUY_MANA_ITEM_CONFIRM,
        INIT_BUY_STONE_ITEM_CONFIRM,
        INPUT_BUY_STONE_ITEM_CONFIRM,
        QUIT_BUY_STONE_ITEM_CONFIRM,
        INIT_BUY_EVENT_ITEM_CONFIRM,
        INPUT_BUY_EVENT_ITEM_CONFIRM,
        QUIT_BUY_EVENT_ITEM_CONFIRM,
        INIT_BUY_ACCOUNT_ITEM_CONFIRM,
        INPUT_BUY_ACCOUNT_ITEM_CONFIRM,
        QUIT_BUY_ACCOUNT_ITEM_CONFIRM,
        INIT_SELL_SERVANT_CONFIRM,
        INPUT_SELL_SERVANT_CONFIRM,
        QUIT_SELL_SERVANT_CONFIRM
    }
}

