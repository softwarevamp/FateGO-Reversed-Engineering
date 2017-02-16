using System;
using UnityEngine;

public class SetRarityDialogControl : BaseMonoBehaviour
{
    public GameObject allDispConfirmInfo;
    public UILabel allHaveQpLb;
    public UILabel allRareConfirmMsgLb;
    public UILabel allRareMsgLb;
    public UILabel allSpendQpLb;
    public UILabel allStatusUpInfoLb;
    public UIGrid allSvtGrid;
    public UILabel allTitleLb;
    public UILabel cancelBtnLb;
    public UILabel cancelLb;
    private Vector3 center;
    public GameObject changeSceneBtnInfo;
    private static readonly float CLOSE_TIME = 0.2666667f;
    public PlayMakerFSM combineFsm;
    public GameObject confirmBtnInfo;
    public UILabel confirmHaveQpLb;
    public UILabel confirmMsgLb;
    public UILabel confirmSpendQpLb;
    public UISprite currentExpVarSprite;
    public UIExtrusionLabel currentLvLb;
    public UILabel decideBtnLb;
    private static readonly float DIALOG_CLOSE_SCALE = 0.95f;
    private static readonly float DIALOG_INITIAL_SCALE = 0.9f;
    private string eventMsg;
    public GameObject expInfo;
    public UISprite getExpValSprite;
    public UILabel haveQpLb;
    public UILabel info2Lb;
    public UILabel info3Lb;
    public UISprite levelUpInfoSprite;
    public UILabel lvInfo1Lb;
    public UILabel lvInfo2Lb;
    public UIPanel mPanel;
    public GameObject normalConfirmInfo;
    public UILabel normalTitleLb;
    public GameObject npUpConfirmInfo;
    public NpUpConfirmComponent npUpInfoComp;
    public UILabel npUpTitleLb;
    private static readonly float OPEN_TIME = 0.2666667f;
    public UILabel rareConfirmMsgLb;
    public UILabel rareMaterialMsgLb;
    public GameObject rariryConfirmInfo;
    public UIGrid raritySvtGrid;
    public UILabel rarityTitleLb;
    public UIExtrusionLabel resLvLb;
    public UILabel shopBtnLb;
    public GameObject skillUpConfirmInfo;
    public UILabel spendQpLb;
    public UILabel statusUpInfoLb;
    public GameObject svtFaceInfo;
    public UILabel terminalBtnLb;
    public UILabel titleLb;
    public UILabel upConfirmMsgLb;
    public UILabel upHaveQpLb;
    public UISprite upImg;
    public UILabel upSpendQpLb;
    public UILabel upTargetNameLb;
    public UILabel upTargetRubyLb;
    public UILabel upTitleLb;
    public UILabel warningLb;

    public void close()
    {
        TweenAlpha.Begin(base.gameObject, CLOSE_TIME, 0f).method = UITweener.Method.EaseOutQuad;
        TweenScale scale = TweenScale.Begin(base.gameObject, CLOSE_TIME, new Vector3(DIALOG_CLOSE_SCALE, DIALOG_CLOSE_SCALE, DIALOG_CLOSE_SCALE));
        if (scale != null)
        {
            scale.method = UITweener.Method.EaseOutQuad;
            scale.eventReceiver = base.gameObject;
            scale.callWhenFinished = "EndCloseDlg";
        }
        else
        {
            base.gameObject.transform.localScale = Vector3.zero;
        }
        int childCount = this.raritySvtGrid.transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                UnityEngine.Object.DestroyObject(this.raritySvtGrid.transform.GetChild(i).gameObject);
            }
        }
        int num3 = this.allSvtGrid.transform.childCount;
        if (num3 > 0)
        {
            for (int j = num3 - 1; j >= 0; j--)
            {
                UnityEngine.Object.DestroyObject(this.allSvtGrid.transform.GetChild(j).gameObject);
            }
        }
        if (this.npUpConfirmInfo.activeSelf)
        {
            this.npUpInfoComp.clearSvtInfo();
        }
    }

    private void EndCloseDlg()
    {
        base.gameObject.SetActive(false);
    }

    private void MoveAlpha()
    {
        base.gameObject.transform.localScale = new Vector3(DIALOG_INITIAL_SCALE, DIALOG_INITIAL_SCALE, DIALOG_INITIAL_SCALE);
        TweenScale.Begin(base.gameObject, OPEN_TIME, Vector3.one);
        base.gameObject.GetComponent<UIWidget>().alpha = 0f;
        TweenAlpha alpha = TweenAlpha.Begin(base.gameObject, OPEN_TIME, 1f);
        if (alpha != null)
        {
            alpha.method = UITweener.Method.EaseOutQuad;
            alpha.eventReceiver = base.gameObject;
        }
    }

    public void OnClickCancel()
    {
        this.close();
        this.combineFsm.SendEvent("CLOSE_RARITYDLG");
    }

    public void OnClickDlgOk()
    {
        this.close();
        this.combineFsm.SendEvent(this.eventMsg);
    }

    public void OnClickTerminalGo()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.close();
        this.combineFsm.SendEvent("GOTO_TERMINAL");
    }

    public void setAllDispConfirmCombine(UserServantEntity baseData, long[] list, int spendQp, int haveQp, bool hpMaxFlg, bool atkMaxFlg)
    {
        this.expInfo.SetActive(false);
        this.allDispConfirmInfo.SetActive(true);
        this.rariryConfirmInfo.SetActive(false);
        this.normalConfirmInfo.SetActive(false);
        this.skillUpConfirmInfo.SetActive(false);
        this.npUpConfirmInfo.SetActive(false);
        base.gameObject.SetActive(true);
        this.MoveAlpha();
        this.allTitleLb.text = LocalizationManager.Get("CONFIRM_TITLE_SVT_COMBINE");
        this.cancelBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_CANCEL");
        this.decideBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_DECIDE");
        this.setCenter();
        float length = list.Length;
        float num2 = this.allSvtGrid.cellWidth * 0.5f;
        for (int i = 0; i < list.Length; i++)
        {
            Debug.Log("showHighRaritySvt : " + list[i]);
            base.createObject(this.svtFaceInfo, this.allSvtGrid.transform, null).GetComponent<MaterialSvtInfo>().setMaterialSvtInfo(i, baseData, list[i], false, false, null);
        }
        this.allSvtGrid.transform.localPosition = new Vector3((this.center.x - (num2 * (length - 1f))) * 0.9f, 30f, this.center.z);
        this.allSvtGrid.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        this.allSvtGrid.repositionNow = true;
        this.allSpendQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), spendQp);
        this.allHaveQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), haveQp);
        this.allStatusUpInfoLb.gameObject.SetActive(false);
        if (hpMaxFlg || atkMaxFlg)
        {
            string str = this.setStatusUpMaxMsg(hpMaxFlg, atkMaxFlg);
            string format = !baseData.isLevelMax() ? LocalizationManager.Get("ALL_STATUS_MAX_MSG") : LocalizationManager.Get("ALL_STATUS_MAXLV_MSG");
            WrapControlText.textAdjust(this.allStatusUpInfoLb, string.Format(format, str));
            this.allStatusUpInfoLb.gameObject.SetActive(true);
        }
        this.allRareMsgLb.text = LocalizationManager.Get("RARE_MATERIAL_INFO_MSG");
        this.allRareConfirmMsgLb.text = LocalizationManager.Get("CONFIRM_COMBINE_MSG");
        this.setSendEventMsg(DispType.SHOW_CONFIRM);
    }

    private void setCenter()
    {
        Vector3[] worldCorners = this.mPanel.worldCorners;
        for (int i = 0; i < 4; i++)
        {
            Vector3 position = worldCorners[i];
            worldCorners[i] = this.mPanel.transform.InverseTransformPoint(position);
        }
        this.center = Vector3.Lerp(worldCorners[0], worldCorners[2], 0.5f);
        Debug.Log("**!! Center: " + this.center);
    }

    public void setConfirmCombine(UserServantEntity baseData, string msg, int spendQp, int haveQp, bool isStatusUp)
    {
        this.expInfo.SetActive(false);
        this.normalConfirmInfo.SetActive(true);
        this.rariryConfirmInfo.SetActive(false);
        this.skillUpConfirmInfo.SetActive(false);
        this.allDispConfirmInfo.SetActive(false);
        this.npUpConfirmInfo.SetActive(false);
        base.gameObject.SetActive(true);
        this.MoveAlpha();
        this.normalTitleLb.text = msg;
        this.cancelBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_CANCEL");
        this.decideBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_DECIDE");
        this.confirmSpendQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), spendQp);
        this.confirmHaveQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), haveQp);
        this.confirmMsgLb.text = LocalizationManager.Get("CONFIRM_COMBINE_MSG");
        this.statusUpInfoLb.gameObject.SetActive(false);
        bool hpMaxFlg = baseData.isAdjustHpMax();
        bool atkMaxFlg = baseData.isAdjustAtkMax();
        if ((hpMaxFlg || atkMaxFlg) && isStatusUp)
        {
            string str = this.setStatusUpMaxMsg(hpMaxFlg, atkMaxFlg);
            string format = !baseData.isLevelMax() ? LocalizationManager.Get("ALL_STATUS_MAX_MSG") : LocalizationManager.Get("ALL_STATUS_MAXLV_MSG");
            WrapControlText.textAdjust(this.statusUpInfoLb, string.Format(format, str));
            this.statusUpInfoLb.gameObject.SetActive(true);
        }
        this.setSendEventMsg(DispType.SHOW_CONFIRM);
    }

    public void setConfirmInfo(UserServantEntity baseData, long[] list, string msg, int spendQp, int haveQp, bool isStatusUp)
    {
        bool hpMaxFlg = baseData.isAdjustHpMax();
        bool atkMaxFlg = baseData.isAdjustAtkMax();
        if ((hpMaxFlg || atkMaxFlg) && isStatusUp)
        {
            this.setAllDispConfirmCombine(baseData, list, spendQp, haveQp, hpMaxFlg, atkMaxFlg);
        }
        else
        {
            this.setConfirmRarityInfo(baseData, list, msg, spendQp, haveQp);
        }
    }

    public void setConfirmRarityInfo(UserServantEntity baseData, long[] list, string msg, int spendQp, int haveQp)
    {
        this.expInfo.SetActive(false);
        this.rariryConfirmInfo.SetActive(true);
        this.normalConfirmInfo.SetActive(false);
        this.skillUpConfirmInfo.SetActive(false);
        this.allDispConfirmInfo.SetActive(false);
        this.npUpConfirmInfo.SetActive(false);
        base.gameObject.SetActive(true);
        this.MoveAlpha();
        this.rarityTitleLb.text = msg;
        this.cancelBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_CANCEL");
        this.decideBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_DECIDE");
        this.setCenter();
        float length = list.Length;
        float num2 = this.raritySvtGrid.cellWidth * 0.5f;
        for (int i = 0; i < list.Length; i++)
        {
            Debug.Log("showHighRaritySvt : " + list[i]);
            base.createObject(this.svtFaceInfo, this.raritySvtGrid.transform, null).GetComponent<MaterialSvtInfo>().setMaterialSvtInfo(i, baseData, list[i], false, false, null);
        }
        this.raritySvtGrid.transform.localPosition = new Vector3((this.center.x - (num2 * (length - 1f))) * 0.9f, this.center.y, this.center.z);
        this.raritySvtGrid.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        this.raritySvtGrid.repositionNow = true;
        this.spendQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), spendQp);
        this.haveQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), haveQp);
        this.rareMaterialMsgLb.text = LocalizationManager.Get("RARE_MATERIAL_INFO_MSG");
        this.rareConfirmMsgLb.text = LocalizationManager.Get("CONFIRM_COMBINE_MSG");
        this.setSendEventMsg(DispType.SHOW_CONFIRM);
    }

    private void setDispInfoTxt(bool isDisp)
    {
        this.rariryConfirmInfo.SetActive(isDisp);
        this.normalConfirmInfo.SetActive(isDisp);
        this.skillUpConfirmInfo.SetActive(isDisp);
        this.allDispConfirmInfo.SetActive(isDisp);
    }

    public void setNpCombineInfo(UserServantEntity baseData, long selectUsrSvtId, SetLevelUpData updata, bool isExceed)
    {
        this.expInfo.SetActive(false);
        this.skillUpConfirmInfo.SetActive(false);
        this.npUpConfirmInfo.SetActive(true);
        this.rariryConfirmInfo.SetActive(false);
        this.normalConfirmInfo.SetActive(false);
        this.allDispConfirmInfo.SetActive(false);
        base.gameObject.SetActive(true);
        this.MoveAlpha();
        this.npUpInfoComp.setNpUpConfirmInfo(baseData, selectUsrSvtId, updata, isExceed);
        this.npUpTitleLb.text = LocalizationManager.Get("CONFIRM_TITLE_TD_COMBINE");
        this.cancelBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_CANCEL");
        this.decideBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_DECIDE");
        this.setSendEventMsg(DispType.SHOW_CONFIRM);
    }

    private void setSendEventMsg(DispType type)
    {
        switch (type)
        {
            case DispType.IS_RARE:
                this.eventMsg = "CONFIRM_MATERIAL";
                break;

            case DispType.EXE_COMBINE:
                this.eventMsg = "EXE_SVTCOMBINE";
                break;

            case DispType.SHOW_CONFIRM:
                this.eventMsg = "CLICK_CONFIRM";
                break;

            case DispType.SHORT_QP:
                this.eventMsg = "GOTO_SVTSELL";
                break;
        }
    }

    public void setSkillNpCombineInfo(SetLevelUpData updata, string titleMsg)
    {
        this.expInfo.SetActive(false);
        this.skillUpConfirmInfo.SetActive(true);
        this.npUpConfirmInfo.SetActive(false);
        this.rariryConfirmInfo.SetActive(false);
        this.normalConfirmInfo.SetActive(false);
        this.allDispConfirmInfo.SetActive(false);
        base.gameObject.SetActive(true);
        this.MoveAlpha();
        this.upTitleLb.text = titleMsg;
        this.upTargetRubyLb.text = updata.targetRuby;
        this.upTargetNameLb.text = updata.targetName;
        this.currentLvLb.text = updata.currentLv.ToString();
        this.resLvLb.text = updata.nextLv.ToString();
        this.upSpendQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), updata.spendQp);
        this.upHaveQpLb.text = string.Format(LocalizationManager.Get("NEED_QP"), updata.haveQp);
        this.upConfirmMsgLb.text = LocalizationManager.Get("CONFIRM_COMBINE_MSG");
        this.cancelBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_CANCEL");
        this.decideBtnLb.text = LocalizationManager.Get("COMMON_CONFIRM_DECIDE");
        this.setSendEventMsg(DispType.SHOW_CONFIRM);
    }

    private string setStatusUpMaxMsg(bool hpMaxFlg, bool atkMaxFlg)
    {
        string str = string.Empty;
        if (hpMaxFlg && atkMaxFlg)
        {
            return LocalizationManager.Get("ADJUST_ALL_TXT");
        }
        if (hpMaxFlg)
        {
            return LocalizationManager.Get("ADJUST_HP_TXT");
        }
        if (atkMaxFlg)
        {
            str = LocalizationManager.Get("ADJUST_ATK_TXT");
        }
        return str;
    }

    public enum DispType
    {
        IS_RARE,
        EXE_COMBINE,
        SHOW_CONFIRM,
        SHORT_QP
    }
}

