using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleSelectServantComponent : BaseMonoBehaviour
{
    public UISprite deckindexSprite;
    public BattleHpGaugeBarComponent hpGauge;
    public UILabel hpLabel;
    private bool isUse;
    public UILabel nameLabel;
    public BattleNpGaugeSystemComponent npGauge;
    private BattleSelectMainSubServantWindow.POSITION position;
    public GameObject root;
    private CallBack selectCallBack;
    public ServantFaceIconComponent servantIcon;
    public GameObject targetObject;
    private int uniqueId;

    public void OnServantClick()
    {
        if (this.isUse)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.selectCallBack(this.position, this.uniqueId);
        }
    }

    public void setData(BattleServantData svtData, BattleSelectMainSubServantWindow.POSITION position, int index, CallBack call)
    {
        this.selectCallBack = call;
        this.root.SetActive(true);
        IconLabelInfo info = new IconLabelInfo();
        info.Set(IconLabelInfo.IconKind.LEVEL, svtData.getLevel(), svtData.getMaxLevel(), 0, false, false);
        this.servantIcon.Set(svtData.getSvtId(), svtData.getLimitCount(), svtData.iconLimitCount, svtData.exceedCount, info, CollectionStatus.Kind.GET, false, false);
        this.nameLabel.text = svtData.getServantShortName();
        this.hpLabel.text = string.Empty + svtData.getNowHp();
        this.hpGauge.setInitValue(svtData.getNowHp(), svtData.getMaxHp());
        this.npGauge.setLineCount(3);
        this.npGauge.setMaxParam(svtData.getCountMaxNp());
        Debug.LogError("^^^^^^^^^^^   " + svtData.getNp());
        this.npGauge.setNowParam(svtData.getNp());
        this.npGauge.setUseNp(svtData.isAddNpGauge());
        this.targetObject.SetActive(false);
        this.uniqueId = svtData.getUniqueID();
        this.position = position;
        this.isUse = true;
    }

    public void setNone()
    {
        this.isUse = false;
        this.root.SetActive(false);
    }

    public void setTarget(int selectUniqueId)
    {
        if (this.isUse)
        {
            this.targetObject.SetActive(this.uniqueId == selectUniqueId);
        }
    }

    public delegate void CallBack(BattleSelectMainSubServantWindow.POSITION position, int uniqueId);
}

