using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PointSummonInfoComponent : MonoBehaviour
{
    protected ClickDelegate clickCallbackFunc;
    public UIButton freeGachaBtn;
    public UISprite freeGachaBtnImg;
    public UISprite freeGachaTxtImg;
    private int gachaTime = 1;
    private bool isFree;
    public UIButton multiBtn;
    public UISprite multiBtnImg;
    public GameObject multiGachaBtnInfo;
    public UISprite multiNumImg;
    private GachaEntity pointGachaData;
    public UILabel pointGachaDetailLb;
    public UILabel pointNumLb;
    public UISprite pointSummonBg;
    private int price;
    private GachaRqParamData requestData;
    public UIButton singleBtn;
    public UISprite singleBtnImg;
    public GameObject singleGachaBtnInfo;
    public UISprite singleNumImg;

    public bool getIsFree() => 
        this.isFree;

    public GachaRqParamData getRequetParam() => 
        this.requestData;

    public void init()
    {
        this.singleGachaBtnInfo.SetActive(false);
        this.multiGachaBtnInfo.SetActive(false);
        this.isFree = false;
    }

    public void OnClickGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.requestData = new GachaRqParamData();
        this.requestData.gachaType = 3;
        this.requestData.gachaId = this.pointGachaData.id;
        this.requestData.warId = this.pointGachaData.warId;
        this.requestData.gachaTime = 1;
        this.requestData.gachaResourceNum = this.price;
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.requestData);
        }
    }

    public void OnClickMutiGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        Debug.Log("--**-- OnClickMutiGacha");
        this.requestData = new GachaRqParamData();
        this.requestData.gachaType = 3;
        this.requestData.gachaId = this.pointGachaData.id;
        this.requestData.warId = this.pointGachaData.warId;
        this.requestData.gachaTime = this.gachaTime;
        this.requestData.gachaResourceNum = this.price * this.gachaTime;
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.requestData);
        }
    }

    public void setDispPointBtn(bool isDisp)
    {
        this.freeGachaBtn.enabled = isDisp;
        this.singleBtn.enabled = isDisp;
        this.multiBtn.enabled = isDisp;
        Color color = !isDisp ? Color.gray : Color.white;
        this.freeGachaBtnImg.color = color;
        this.singleBtnImg.color = color;
        this.multiBtnImg.color = color;
        this.pointSummonBg.color = color;
        this.pointGachaDetailLb.color = color;
        this.pointNumLb.color = color;
        this.freeGachaTxtImg.color = color;
        this.singleNumImg.color = color;
        this.multiNumImg.color = color;
    }

    private void setMultiGachaNum(int price, int usrFriendPoint)
    {
        this.gachaTime = usrFriendPoint / price;
        if (this.gachaTime >= 2)
        {
            this.singleGachaBtnInfo.SetActive(false);
            this.multiGachaBtnInfo.SetActive(true);
            string str = "summon_txt_f0" + this.gachaTime;
            if (this.gachaTime >= 10)
            {
                this.gachaTime = 10;
                str = "summon_txt_f10";
            }
            this.multiNumImg.spriteName = str;
        }
        else
        {
            this.singleGachaBtnInfo.SetActive(true);
            this.freeGachaTxtImg.spriteName = "summon_txt_f01";
        }
    }

    public void setPointSummonDispInfo()
    {
        this.init();
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        long[] args = new long[] { entity.userId, (long) this.pointGachaData.condQuestId };
        UserQuestEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_QUEST).getEntityFromId<UserQuestEntity>(args);
        if ((entity2 != null) && (entity2.getClearNum() > 0))
        {
            int friendPoint = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<TblUserMaster>(DataNameKind.Kind.TBL_USER_GAME).getUserData(entity.userId).friendPoint;
            DateTime time = NetworkManager.getLocalDateTime();
            Debug.Log("!!** setSummonData nowTime: " + time);
            long[] numArray2 = new long[] { entity.userId, (long) this.pointGachaData.id };
            UserGachaEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserGachaMaster>(DataNameKind.Kind.USER_GACHA).getEntityFromId<UserGachaEntity>(numArray2);
            if (entity4 != null)
            {
                long freeDrawAt = entity4.freeDrawAt;
                DateTime time2 = NetworkManager.getLocalDateTime(freeDrawAt);
                if (time2.Hour >= BalanceConfig.DailyFreeGachaResetTime)
                {
                    freeDrawAt += 0x15180L;
                }
                freeDrawAt -= ((((time2.Hour - BalanceConfig.DailyFreeGachaResetTime) * 60) + time2.Minute) * 60) + time2.Second;
                DateTime time3 = NetworkManager.getLocalDateTime(freeDrawAt);
                Debug.Log("!!** setSummonData freeDrawLocalTime: " + time3);
                if (freeDrawAt <= 0L)
                {
                    this.isFree = true;
                }
                else
                {
                    Debug.Log("!!** setSummonData befFreeDrawLocalTime: " + time2);
                    if ((time2 <= time3) && (time3 <= time))
                    {
                        this.isFree = true;
                    }
                }
            }
            else
            {
                this.isFree = true;
            }
            Debug.Log("!!** setSummonData isDailyGacha: " + this.isFree);
            if (this.isFree)
            {
                this.pointGachaDetailLb.text = LocalizationManager.Get("DAILY_SUMMON_DETAIL");
                this.freeGachaTxtImg.spriteName = "summon_txt_free";
                this.singleGachaBtnInfo.SetActive(true);
            }
            else
            {
                this.pointGachaDetailLb.text = string.Format(LocalizationManager.Get("POINT_SUMMON_DETAIL"), this.price);
                this.setMultiGachaNum(this.price, friendPoint);
            }
            this.pointNumLb.text = friendPoint.ToString();
        }
        else
        {
            this.init();
        }
    }

    public void setPointSummonInfo(GachaEntity pointGachaEnt, ClickDelegate callback)
    {
        this.init();
        this.clickCallbackFunc = callback;
        this.isFree = false;
        this.price = pointGachaEnt.getPrice();
        this.pointGachaData = pointGachaEnt;
        this.setPointSummonDispInfo();
    }

    public delegate void ClickDelegate(GachaRqParamData paramData);
}

