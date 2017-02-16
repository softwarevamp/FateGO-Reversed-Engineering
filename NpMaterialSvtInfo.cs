using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class NpMaterialSvtInfo : MonoBehaviour
{
    protected ClickDelegate callbackFunc;
    public UIIconLabel iconLabel;
    private IconLabelInfo iconLabelInfo = new IconLabelInfo();
    private int index;
    public GameObject npLvInfo;
    private UserServantEntity selectUsrSvtEnt;
    private long selectUsrSvtId;
    public UILabel statusLb;
    public ServantFaceIconComponent svtFaseIconComp;

    public int getIndex() => 
        this.index;

    public void OnClickMaterialStatus()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(NpCombineControl.TargetType.MATERIAL_STATUS, this.selectUsrSvtId);
        }
    }

    public void OnClickMaterialSvt()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(NpCombineControl.TargetType.MATERIAL_SELECT, this.selectUsrSvtId);
        }
    }

    public void setMaterialSvtInfo(int idx, UserServantEntity baseData, long selectUsrSvtId, ClickDelegate callback)
    {
        this.index = idx;
        this.selectUsrSvtId = selectUsrSvtId;
        this.npLvInfo.SetActive(false);
        this.selectUsrSvtEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(selectUsrSvtId);
        this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.selectUsrSvtEnt.lv, this.selectUsrSvtEnt.getLevelMax(), 0, false, false);
        this.svtFaseIconComp.Set(selectUsrSvtId, this.iconLabelInfo);
        this.setNpLvInfo();
        this.callbackFunc = callback;
    }

    public void setNpLvInfo()
    {
        int num;
        int num2;
        int num3;
        int num4;
        int num5;
        string str;
        string str2;
        int num6;
        int num7;
        this.selectUsrSvtEnt.getTreasureDeviceInfo(out num, out num2, out num3, out num4, out num5, out str, out str2, out num6, out num7, 1, -1);
        this.iconLabel.Set(IconLabelInfo.IconKind.NP_LEVEL, (num <= 0) ? -1 : num2, num3, 0, 0L, false, false);
        this.npLvInfo.SetActive(true);
    }

    public delegate void ClickDelegate(NpCombineControl.TargetType type, long usrSvtId);
}

