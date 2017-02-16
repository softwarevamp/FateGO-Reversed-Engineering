using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PaySummonInfoComponent : MonoBehaviour
{
    protected ClickDelegate clickCallbackFunc;
    private VaildGachaInfo currentGachaData;
    private int gachaTime = 1;
    private int haveStoneNum;
    private bool isTicket;
    public GameObject payBtnInfo;
    public UISprite paySummonBg;
    public UISprite paySummonBntImg;
    public UIButton paySummonBtn;
    public UILabel paySummonDetailLb;
    public UISprite payTitle;
    private int price;
    private GachaRqParamData requestData;
    public GameObject stoneInfo;
    public UILabel stoneNumLb;
    public GameObject ticketInfo;
    private int ticketNum;
    public UILabel ticketNumLb;
    public UISprite ticketTitle;

    private void ClearAlpha(GameObject target)
    {
        float duration = 0.2f;
        if (target != null)
        {
            TweenScale.Begin(target, duration, Vector3.zero);
        }
        TweenAlpha alpha = TweenAlpha.Begin(target, duration, 0f);
        if (alpha != null)
        {
            alpha.method = UITweener.Method.EaseInOut;
            alpha.eventReceiver = base.gameObject;
        }
    }

    public int getSummonPrice() => 
        this.price;

    public int getUsrStoneNum() => 
        this.haveStoneNum;

    public int getUsrTicketNum() => 
        this.ticketNum;

    public void init()
    {
        this.ticketTitle.gameObject.SetActive(false);
        this.ticketInfo.SetActive(false);
        this.ticketNum = 0;
        this.price = 0;
        this.gachaTime = 1;
    }

    private void MoveAlpha(GameObject target)
    {
        float duration = 0.2f;
        target.transform.localScale = Vector3.zero;
        if (!target.activeSelf)
        {
            target.SetActive(true);
        }
        TweenScale.Begin(target, duration, Vector3.one);
        target.GetComponent<UIWidget>().alpha = 0f;
        TweenAlpha alpha = TweenAlpha.Begin(target, duration, 1f);
        if (alpha != null)
        {
            alpha.method = UITweener.Method.EaseInOut;
            alpha.eventReceiver = base.gameObject;
        }
    }

    public void OnClickGacha()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.requestData = new GachaRqParamData();
        this.requestData.gachaId = this.currentGachaData.id;
        this.requestData.warId = this.currentGachaData.warId;
        this.requestData.gachaTime = 1;
        this.requestData.ticketItemId = 0;
        if (this.isTicket)
        {
            this.requestData.gachaType = 5;
            this.requestData.gachaResourceNum = this.gachaTime;
            this.requestData.ticketItemId = this.currentGachaData.ticketItemId;
        }
        else
        {
            this.requestData.gachaType = 1;
            this.requestData.gachaResourceNum = this.price;
        }
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.requestData);
        }
    }

    public void setAlphaSummonBtn(bool isDisp)
    {
        if (isDisp)
        {
            this.MoveAlpha(this.payBtnInfo);
        }
        else
        {
            this.ClearAlpha(this.payBtnInfo);
        }
    }

    public void setCurrentBannerInfo(VaildGachaInfo gachaData)
    {
        this.setEnableSummonBtn(gachaData.isOpen);
        this.currentGachaData = gachaData;
    }

    public void setDispSummonBtn(bool isDisp)
    {
        this.paySummonBtn.enabled = isDisp;
        Color color = !isDisp ? Color.gray : Color.white;
        this.paySummonBntImg.color = color;
        if (this.isTicket)
        {
            this.ticketTitle.color = color;
        }
        else
        {
            this.payTitle.color = color;
            this.paySummonDetailLb.color = color;
            this.stoneNumLb.color = color;
            UISprite componentInChildren = this.stoneInfo.GetComponentInChildren<UISprite>();
            if (componentInChildren != null)
            {
                componentInChildren.color = color;
            }
        }
        this.paySummonBg.color = color;
    }

    public void setEnableSummonBtn(bool isOpen)
    {
        this.paySummonBtn.enabled = isOpen;
        Color color = !isOpen ? Color.gray : Color.white;
        this.paySummonBntImg.color = color;
        if (this.isTicket)
        {
            this.ticketTitle.color = color;
        }
        else
        {
            this.payTitle.color = color;
            this.paySummonDetailLb.color = color;
            this.stoneNumLb.color = color;
            UISprite componentInChildren = this.stoneInfo.GetComponentInChildren<UISprite>();
            if (componentInChildren != null)
            {
                componentInChildren.color = color;
            }
        }
        this.paySummonBg.color = color;
    }

    public void setPaySummonDispInfo()
    {
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        long[] args = new long[] { entity.userId, (long) this.currentGachaData.ticketItemId };
        UserItemEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId<UserItemEntity>(args);
        if ((entity2 != null) && (entity2.num > 0))
        {
            this.ticketNum = entity2.num;
            this.isTicket = true;
        }
        else
        {
            this.isTicket = false;
        }
        if (this.isTicket)
        {
            this.paySummonDetailLb.text = LocalizationManager.Get("TICKET_SUMMON_DETAIL");
            this.ticketTitle.gameObject.SetActive(true);
            this.ticketNumLb.text = string.Format(LocalizationManager.Get("TICKET_NUM"), this.ticketNum);
            this.ticketInfo.SetActive(true);
            this.payTitle.gameObject.SetActive(false);
            this.stoneInfo.SetActive(false);
        }
        else
        {
            this.paySummonDetailLb.text = string.Format(LocalizationManager.Get("PAY_SUMMON_DETAIL"), this.price);
            this.ticketTitle.gameObject.SetActive(false);
            this.ticketInfo.SetActive(false);
            this.payTitle.gameObject.SetActive(true);
            this.stoneInfo.SetActive(true);
            this.haveStoneNum = entity.stone;
            this.stoneNumLb.text = entity.stone.ToString();
        }
    }

    public void setPaySummonInfo(VaildGachaInfo gachaData, ClickDelegate callback)
    {
        this.init();
        this.clickCallbackFunc = callback;
        if (gachaData != null)
        {
            this.price = gachaData.price;
            this.setEnableSummonBtn(gachaData.isOpen);
            this.currentGachaData = gachaData;
            this.setPaySummonDispInfo();
        }
        else
        {
            this.setEnableSummonBtn(false);
        }
    }

    public delegate void ClickDelegate(GachaRqParamData paramData);
}

