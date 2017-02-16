using System;
using UnityEngine;

public class NpUpConfirmComponent : BaseMonoBehaviour
{
    public GameObject baseInfo;
    public UILabel baseTxt;
    public UIExtrusionLabel currentLvLb;
    public GameObject materialInfo;
    public UILabel mtTxt;
    public UIExtrusionLabel resLvLb;
    public GameObject svtFaceInfo;
    public UILabel upConfirmMsgLb;
    public UILabel upHaveQpLb;
    public UILabel upSpendQpLb;
    public UILabel upTargetNameLb;
    public UILabel upTargetRubyLb;

    public void clearSvtInfo()
    {
        GameObject gameObject = this.baseInfo.transform.GetComponentInChildren<NpMaterialSvtInfo>().gameObject;
        GameObject obj3 = this.materialInfo.transform.GetComponentInChildren<NpMaterialSvtInfo>().gameObject;
        UnityEngine.Object.DestroyObject(gameObject);
        UnityEngine.Object.DestroyObject(obj3);
    }

    public void setNpUpConfirmInfo(UserServantEntity baseData, long selectUsrSvtId, SetLevelUpData updata, bool isExceed)
    {
        this.upTargetRubyLb.text = updata.targetRuby;
        this.upTargetNameLb.text = updata.targetName;
        this.currentLvLb.text = updata.currentLv.ToString();
        this.resLvLb.text = updata.nextLv.ToString();
        base.createObject(this.svtFaceInfo, this.baseInfo.transform, null).GetComponent<NpMaterialSvtInfo>().setMaterialSvtInfo(0, baseData, baseData.id, null);
        base.createObject(this.svtFaceInfo, this.materialInfo.transform, null).GetComponent<NpMaterialSvtInfo>().setMaterialSvtInfo(0, baseData, selectUsrSvtId, null);
        this.upSpendQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), updata.spendQp);
        this.upHaveQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), updata.haveQp);
        this.upConfirmMsgLb.text = !isExceed ? LocalizationManager.Get("CONFIRM_COMBINE_MSG") : LocalizationManager.Get("NPUP_EXCEED_CONFIRM_TXT");
    }
}

