using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaterialSvtInfo : MonoBehaviour
{
    protected ClickDelegate callbackFunc;
    private IconLabelInfo iconLabelInfo = new IconLabelInfo();
    private int index;
    private UserServantEntity selectUsrSvtEnt;
    private long selectUsrSvtId;
    public UILabel statusLb;
    public ServantFaceIconComponent svtFaseIconComp;

    private void checkIsLimitTarget(UserServantEntity baseData, long selectUsrSvtId, bool isOver)
    {
        if ((baseData.svtId == this.selectUsrSvtEnt.svtId) && !baseData.isLimitCountMax())
        {
            if (!isOver)
            {
                this.statusLb.text = LocalizationManager.Get("SAME_SVTEQUIP_COMBINE");
            }
            else
            {
                this.statusLb.text = string.Empty;
            }
        }
    }

    public int getIndex() => 
        this.index;

    public void OnClickMaterialStatus()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ServantCombineControl.TargetType.MATERIAL_STATUS, this.selectUsrSvtId);
        }
    }

    public void OnClickMaterialSvt()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ServantCombineControl.TargetType.MATERIAL_SELECT, this.selectUsrSvtId);
        }
    }

    public void setMaterialSvtInfo(int idx, UserServantEntity baseData, long selectUsrSvtId, bool isShowStatus, bool overFlg, ClickDelegate callback)
    {
        this.index = idx;
        this.selectUsrSvtId = selectUsrSvtId;
        this.selectUsrSvtEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(selectUsrSvtId);
        this.iconLabelInfo.Set(IconLabelInfo.IconKind.LEVEL, this.selectUsrSvtEnt.lv, this.selectUsrSvtEnt.getLevelMax(), 0, false, false);
        this.svtFaseIconComp.Set(selectUsrSvtId, this.iconLabelInfo);
        this.statusLb.text = string.Empty;
        this.statusLb.gameObject.SetActive(isShowStatus);
        this.checkIsLimitTarget(baseData, selectUsrSvtId, overFlg);
        this.callbackFunc = callback;
    }

    public delegate void ClickDelegate(ServantCombineControl.TargetType type, long usrSvtId);
}

