using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SummonInfoControl : MonoBehaviour
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache33;
    [CompilerGenerated]
    private static System.Action <>f__am$cache34;
    private VaildGachaInfo beforeSummonData;
    public UISprite chargeGachaNumImg;
    public GameObject chargeSummonInfo;
    protected ClickDelegate clickCallbackFunc;
    protected System.Action closeCallbackFunc;
    private GameObject currentBtnInfo;
    private VaildGachaInfo currentSummonData;
    public GameObject freeSummonInfo;
    private int gachaTime = 1;
    public UILabel haveChargeInfoLb;
    private int haveStoneNum;
    public UILabel haveTicketInfoLb;
    private bool isFree;
    private bool isTicket;
    public UIButton multiPayBtn;
    private int multiPayGachaTime;
    public UISprite multiPayNumImg;
    private int multiPayPrice;
    public UILabel multiPointInfoLb;
    public UISprite multiPointNumImg;
    public GameObject multiPtSummonInfo;
    private int multiShopIdIdx;
    public UILabel mutiPayInfoLb;
    public UILabel needChargeInfoLb;
    public UILabel needTicketInfoLb;
    public UILabel onePayInfoLb;
    public UISprite onePayNumImg;
    public UILabel onePointInfoLb;
    protected static readonly float OPEN_TIME = 0.2f;
    private int payGachaPrice;
    public GameObject paySummonInfo;
    private int price;
    private GachaRqParamData requestData;
    private int shopIdIdx;
    public UIButton singlePayBtn;
    public UILabel singlePayInfoLb;
    public UISprite singlePayNumImg;
    public GameObject singlePaySummonInfo;
    public UILabel singlePtInfoLb;
    public GameObject singlePtSummonInfo;
    private SummonState state;
    public UIButton summonDetailBtn;
    public GameObject summonDetailInfo;
    private int summonType;
    public UILabel ticketMultiInfoLb;
    private int ticketNum;
    public GameObject ticketSummonInfo;
    public UIButton tutorialPayBtn;
    private UserGameEntity usrData;

    private string GetCurrentSummonDataUrl()
    {
        string url = string.Empty;
        string jsonstr = PlayerPrefs.GetString("announcement");
        if (jsonstr == null)
        {
            return null;
        }
        foreach (AnnouncementData data in JsonManager.DeserializeArray<AnnouncementData>(JsonManager.getDictionary(jsonstr)["announcement"]))
        {
            if (((data.gachaId != null) && (data.gachaId.Length > 0)) && (data.type == 6))
            {
                foreach (int num2 in data.gachaId)
                {
                    if (num2 == this.currentSummonData.id)
                    {
                        url = data.url;
                    }
                }
            }
        }
        return url;
    }

    public bool getIsFree() => 
        this.isFree;

    public int getSummonPrice() => 
        this.payGachaPrice;

    public int getUsrStoneNum() => 
        this.haveStoneNum;

    public int getUsrTicketNum() => 
        this.ticketNum;

    private void initSummonBtnDisp()
    {
        this.freeSummonInfo.SetActive(false);
        this.singlePtSummonInfo.SetActive(false);
        this.multiPtSummonInfo.SetActive(false);
        this.ticketSummonInfo.SetActive(false);
        this.singlePaySummonInfo.SetActive(false);
        this.paySummonInfo.SetActive(false);
        this.chargeSummonInfo.SetActive(false);
        this.isFree = false;
    }

    public void initSummonInfo()
    {
        this.isFree = false;
    }

    private void MoveAlpha(GameObject target)
    {
        if (!target.activeSelf)
        {
            target.SetActive(true);
        }
        target.GetComponent<UIWidget>().alpha = 0f;
        TweenAlpha alpha = TweenAlpha.Begin(target, OPEN_TIME, 1f);
        if (alpha != null)
        {
            alpha.method = UITweener.Method.EaseInOut;
            alpha.eventReceiver = base.gameObject;
        }
    }

    public void OnClickChargeGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.requestData = new GachaRqParamData();
        this.requestData.gachaType = this.summonType;
        this.requestData.gachaId = this.currentSummonData.id;
        this.requestData.warId = this.currentSummonData.warId;
        this.requestData.gachaTime = this.gachaTime;
        this.requestData.gachaResourceNum = this.price;
        this.requestData.shopIdIdx = this.shopIdIdx;
        this.payGachaPrice = this.price;
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.requestData);
        }
    }

    public void OnClickDetail(System.Action callback)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        string currentSummonDataUrl = this.GetCurrentSummonDataUrl();
        Debug.Log("**!! OnClickDetail !path: " + currentSummonDataUrl);
        Debug.Log("**!! OnClickDetail *CurrentSummonData: " + this.currentSummonData.id);
        this.closeCallbackFunc = callback;
        NoticeInfoComponent.TermID = "7";
        if (<>f__am$cache33 == null)
        {
            <>f__am$cache33 = delegate {
            };
        }
        WebViewManager.OpenWebView(LocalizationManager.Get("WEB_VIEW_TITLE_SUMMON"), <>f__am$cache33);
        if (<>f__am$cache34 == null)
        {
            <>f__am$cache34 = delegate {
            };
        }
        WebViewManager.OpenUniWebView(LocalizationManager.Get("WEB_VIEW_TITLE_SUMMON"), currentSummonDataUrl, <>f__am$cache34);
    }

    public void OnClickGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        int num = 1;
        this.requestData = new GachaRqParamData();
        this.requestData.gachaType = this.summonType;
        this.requestData.gachaId = this.currentSummonData.id;
        this.requestData.warId = this.currentSummonData.warId;
        this.requestData.gachaTime = num;
        this.requestData.gachaResourceNum = this.price;
        this.requestData.shopIdIdx = 1;
        if (this.isTicket && (this.summonType == 1))
        {
            this.requestData.gachaType = 5;
            this.requestData.gachaResourceNum = this.gachaTime;
            this.requestData.ticketItemId = this.currentSummonData.ticketItemId;
        }
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.requestData);
        }
    }

    public void OnClickMultiPayGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.requestData = new GachaRqParamData();
        this.requestData.gachaType = this.summonType;
        this.requestData.gachaId = this.currentSummonData.id;
        this.requestData.warId = this.currentSummonData.warId;
        this.requestData.gachaTime = this.multiPayGachaTime;
        this.requestData.gachaResourceNum = this.multiPayPrice;
        this.requestData.shopIdIdx = this.multiShopIdIdx;
        this.payGachaPrice = this.multiPayPrice;
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.requestData);
        }
    }

    public void OnClickMutiGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.requestData = new GachaRqParamData();
        this.requestData.gachaType = this.summonType;
        this.requestData.gachaId = this.currentSummonData.id;
        this.requestData.warId = this.currentSummonData.warId;
        this.requestData.gachaTime = this.gachaTime;
        this.requestData.gachaResourceNum = this.price * this.gachaTime;
        this.requestData.shopIdIdx = 1;
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.requestData);
        }
    }

    public void OnClickSinglePayGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.requestData = new GachaRqParamData();
        this.requestData.gachaType = this.summonType;
        this.requestData.gachaId = this.currentSummonData.id;
        this.requestData.warId = this.currentSummonData.warId;
        this.requestData.gachaTime = this.gachaTime;
        this.requestData.gachaResourceNum = this.price;
        this.requestData.shopIdIdx = this.shopIdIdx;
        this.payGachaPrice = this.price;
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.requestData);
        }
    }

    public void setAlphaSummonBtn()
    {
        if ((this.beforeSummonData != null) && !this.beforeSummonData.Equals(this.currentSummonData))
        {
            this.MoveAlpha(this.currentBtnInfo);
        }
    }

    public void setChargeSummonInfo()
    {
        this.initSummonBtnDisp();
        this.haveStoneNum = this.usrData.stone;
        this.HaveFreeStoneNum = this.usrData.freeStone;
        this.HaveChargeStoneNum = this.usrData.chargeStone;
        this.chargeSummonInfo.SetActive(true);
        GachaEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.GACHA).getEntityFromId<GachaEntity>(this.currentSummonData.id);
        int num = entity.shopId1;
        int num2 = entity.shopId2;
        this.currentBtnInfo = this.chargeSummonInfo;
        if (num > 0)
        {
            this.gachaTime = entity.drawNum1;
            this.price = entity.getPayOneTimePrice();
            this.shopIdIdx = 1;
        }
        if (num2 > 0)
        {
            this.gachaTime = entity.drawNum2;
            this.price = entity.getPayMultiTimePrice();
            this.shopIdIdx = 2;
        }
        string str = "btn_txt_summon0" + this.gachaTime;
        if (this.gachaTime >= 10)
        {
            str = "btn_txt_summon10";
        }
        this.chargeGachaNumImg.spriteName = str;
        this.needChargeInfoLb.text = $"{this.price}";
        this.haveChargeInfoLb.text = this.usrData.chargeStone.ToString("#,0");
    }

    private void setCurrentSummonInfo(GameObject currentInfo)
    {
        this.currentBtnInfo = currentInfo;
    }

    public void setEnableSummonBnt(bool isEnable)
    {
        this.beforeSummonData = this.currentSummonData;
        this.currentBtnInfo.SetActive(isEnable);
        this.summonDetailInfo.SetActive(isEnable);
    }

    public void setFreeSummonInfo()
    {
        this.initSummonBtnDisp();
        long num = NetworkManager.getTime();
        long[] args = new long[] { this.usrData.userId, (long) this.currentSummonData.id };
        UserGachaEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserGachaMaster>(DataNameKind.Kind.USER_GACHA).getEntityFromId<UserGachaEntity>(args);
        if (entity != null)
        {
            long freeDrawAt = entity.freeDrawAt;
            int dailyFreeGachaResetTime = BalanceConfig.DailyFreeGachaResetTime;
            int num4 = 0x18 - dailyFreeGachaResetTime;
            long num5 = (num4 * 60) * 60;
            long t = freeDrawAt + num5;
            long num7 = num + num5;
            DateTime time = NetworkManager.getUtc_8DateTime(t);
            TimeSpan span = (TimeSpan) (NetworkManager.getUtc_8DateTime(num7).Date - time.Date);
            if (span.Days > 0)
            {
                this.isFree = true;
            }
        }
        Debug.Log("!!** setFreeSummonInfo isFree: " + this.isFree);
        if (this.isFree)
        {
            this.freeSummonInfo.SetActive(true);
            this.currentBtnInfo = this.freeSummonInfo;
        }
        else
        {
            this.setPointSummonInfo();
        }
    }

    public void setPaySummonInfo()
    {
        this.initSummonBtnDisp();
        long[] args = new long[] { this.usrData.userId, (long) this.currentSummonData.ticketItemId };
        UserItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId<UserItemEntity>(args);
        if ((entity != null) && (entity.num > 0))
        {
            this.ticketNum = entity.num;
            this.isTicket = true;
        }
        else
        {
            this.isTicket = false;
        }
        GachaEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.GACHA).getEntityFromId<GachaEntity>(this.currentSummonData.id);
        if (this.currentSummonData.id == ((ConstantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.CONSTANT)).GetValue("TUTORIAL_GACHA_ID"))
        {
            this.ticketSummonInfo.SetActive(false);
            this.haveStoneNum = this.usrData.stone;
            this.HaveFreeStoneNum = this.usrData.freeStone;
            this.HaveChargeStoneNum = this.usrData.chargeStone;
            int num = entity2.shopId1;
            int num2 = entity2.shopId2;
            int num3 = ((ConstantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.CONSTANT)).GetValue("USER_FREE_STONE");
            this.paySummonInfo.SetActive(false);
            this.singlePaySummonInfo.SetActive(true);
            this.currentBtnInfo = this.singlePaySummonInfo;
            if (this.haveStoneNum < num3)
            {
                this.gachaTime = entity2.drawNum1;
                this.price = entity2.getPayOneTimePrice();
                this.shopIdIdx = 1;
            }
            else
            {
                this.gachaTime = entity2.drawNum2;
                this.price = entity2.getPayMultiTimePrice();
                this.shopIdIdx = 2;
            }
            string str = "btn_txt_summon0" + this.gachaTime;
            if (this.gachaTime >= 10)
            {
                str = "btn_txt_summon10";
            }
            this.singlePayNumImg.spriteName = str;
            this.singlePayInfoLb.text = $"{this.price}";
        }
        else if (this.isTicket)
        {
            this.ticketSummonInfo.SetActive(true);
            this.currentBtnInfo = this.ticketSummonInfo;
            this.singlePaySummonInfo.SetActive(false);
            this.paySummonInfo.SetActive(false);
            this.needTicketInfoLb.text = $"{1}";
            this.haveTicketInfoLb.text = this.ticketNum.ToString("#,0");
            this.multiPayGachaTime = entity2.drawNum2;
            this.multiPayPrice = entity2.getPayMultiTimePrice();
            this.ticketMultiInfoLb.text = $"{this.multiPayPrice}";
            this.multiShopIdIdx = 2;
            this.haveStoneNum = this.usrData.stone;
            this.HaveFreeStoneNum = this.usrData.freeStone;
            this.HaveChargeStoneNum = this.usrData.chargeStone;
        }
        else
        {
            this.ticketSummonInfo.SetActive(false);
            this.haveStoneNum = this.usrData.stone;
            this.HaveFreeStoneNum = this.usrData.freeStone;
            this.HaveChargeStoneNum = this.usrData.chargeStone;
            int num4 = entity2.shopId1;
            int num5 = entity2.shopId2;
            if ((num4 <= 0) || (num5 <= 0))
            {
                this.paySummonInfo.SetActive(false);
                this.singlePaySummonInfo.SetActive(true);
                this.currentBtnInfo = this.singlePaySummonInfo;
                if (num4 > 0)
                {
                    this.gachaTime = entity2.drawNum1;
                    this.price = entity2.getPayOneTimePrice();
                    this.shopIdIdx = 1;
                }
                if (num5 > 0)
                {
                    this.gachaTime = entity2.drawNum2;
                    this.price = entity2.getPayMultiTimePrice();
                    this.shopIdIdx = 2;
                }
                string str2 = "btn_txt_summon0" + this.gachaTime;
                if (this.gachaTime >= 10)
                {
                    str2 = "btn_txt_summon10";
                }
                this.singlePayNumImg.spriteName = str2;
                this.singlePayInfoLb.text = $"{this.price}";
            }
            else
            {
                this.singlePaySummonInfo.SetActive(false);
                this.paySummonInfo.SetActive(true);
                this.currentBtnInfo = this.paySummonInfo;
                this.gachaTime = entity2.drawNum1;
                this.multiPayGachaTime = entity2.drawNum2;
                this.price = entity2.getPayOneTimePrice();
                this.multiPayPrice = entity2.getPayMultiTimePrice();
                this.onePayNumImg.spriteName = "btn_txt_summon0" + this.gachaTime;
                this.onePayInfoLb.text = $"{this.price}";
                this.shopIdIdx = 1;
                string str3 = "btn_txt_summon0" + this.multiPayGachaTime;
                if (this.multiPayGachaTime >= 10)
                {
                    str3 = "btn_txt_summon10";
                }
                this.multiPayNumImg.spriteName = str3;
                this.mutiPayInfoLb.text = $"{this.multiPayPrice}";
                this.multiShopIdIdx = 2;
            }
        }
    }

    public void setPointSummonInfo()
    {
        int friendPoint = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<TblUserMaster>(DataNameKind.Kind.TBL_USER_GAME).getUserData(this.usrData.userId).friendPoint;
        this.price = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.GACHA).getEntityFromId<GachaEntity>(this.currentSummonData.id).getPrice();
        this.gachaTime = friendPoint / this.price;
        if (this.gachaTime >= 2)
        {
            this.singlePtSummonInfo.SetActive(false);
            this.multiPtSummonInfo.SetActive(true);
            this.currentBtnInfo = this.multiPtSummonInfo;
            string str = "btn_txt_summon0" + this.gachaTime;
            if (this.gachaTime >= 10)
            {
                this.gachaTime = 10;
                str = "btn_txt_summon10";
            }
            this.multiPointNumImg.spriteName = str;
            this.onePointInfoLb.text = this.price.ToString("#,0");
            this.multiPointInfoLb.text = (this.price * this.gachaTime).ToString("#,0");
        }
        else
        {
            this.multiPtSummonInfo.SetActive(false);
            this.singlePtSummonInfo.SetActive(true);
            this.currentBtnInfo = this.singlePtSummonInfo;
            this.singlePtInfoLb.text = $"{this.price}";
        }
    }

    public void setSummonDispInfo()
    {
        this.usrData = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        PayType.Type summonType = (PayType.Type) this.summonType;
        switch (summonType)
        {
            case PayType.Type.STONE:
                this.summonDetailInfo.SetActive(true);
                this.setPaySummonInfo();
                return;

            case PayType.Type.FRIEND_POINT:
                this.summonDetailInfo.SetActive(false);
                this.setFreeSummonInfo();
                return;
        }
        if (summonType == PayType.Type.CHARGE_STONE)
        {
            this.summonDetailInfo.SetActive(true);
            this.setChargeSummonInfo();
        }
    }

    public void setSummonInfo(VaildGachaInfo gachaData, ClickDelegate callback)
    {
        this.isFree = false;
        this.summonType = gachaData.type;
        this.clickCallbackFunc = callback;
        if (gachaData != null)
        {
            this.currentSummonData = gachaData;
        }
        this.setSummonDispInfo();
    }

    public void setTutorialBtnEnable(bool isEnable)
    {
        this.multiPayBtn.enabled = isEnable;
        this.summonDetailBtn.enabled = isEnable;
    }

    public void setTutorialExeBtnEnable(bool isEnable)
    {
        this.singlePayBtn.enabled = isEnable;
        this.tutorialPayBtn.enabled = isEnable;
    }

    public int tutoSummonSinglePrice() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.GACHA).getEntityFromId<GachaEntity>(((ConstantMaster) SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.CONSTANT)).GetValue("TUTORIAL_GACHA_ID")).getPayOneTimePrice();

    public int HaveChargeStoneNum { get; internal set; }

    public int HaveFreeStoneNum { get; internal set; }

    public delegate void ClickDelegate(GachaRqParamData paramData);

    public enum SummonState
    {
        FREE_SUMMON,
        SINGLE_POINT_SUMMON,
        MULTI_POINT_SUMMON,
        SINGLE_PAY_SUMMON,
        MULTI_PAY_SUMMON,
        TICKET_SUMMON
    }
}

