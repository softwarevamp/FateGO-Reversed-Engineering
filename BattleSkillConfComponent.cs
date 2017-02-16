using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleSkillConfComponent : BattleWindowComponent
{
    public GameObject cancelButton;
    public GameObject closeButton;
    private BattleData data;
    private bool isPlayedSe;
    public GameObject okButton;
    public UILabel skillChargeLabel;
    public UILabel skillConfLabel;
    public BattleServantSkillIConComponent skillIcon;
    private BattleSkillInfoData skillInfo;
    public UILabel skillNameLabel;
    public GameObject target;

    public void OnClick()
    {
        this.onClickCancel();
    }

    public void onClickCancel()
    {
        if (this.target != null)
        {
            BattlePerformancePlayer component = this.target.GetComponent<BattlePerformancePlayer>();
            if (component != null)
            {
                component.onClickSkillCancel();
            }
            BattlePerformanceMaster master = this.target.GetComponent<BattlePerformanceMaster>();
            if (master != null)
            {
                master.onClickSkillCancel();
            }
        }
    }

    public void onClickOK()
    {
        if (!this.isPlayedSe)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE2);
            this.isPlayedSe = true;
        }
        if (this.target != null)
        {
            BattlePerformancePlayer component = this.target.GetComponent<BattlePerformancePlayer>();
            if (component != null)
            {
                component.onClickSkillOK(this.skillInfo);
            }
            BattlePerformanceMaster master = this.target.GetComponent<BattlePerformanceMaster>();
            if (master != null)
            {
                master.onClickSkillOK(this.skillInfo);
            }
        }
    }

    public void setInit(BattleData data)
    {
        this.data = data;
        base.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
    }

    public void SetSkillConf(BattleSkillInfoData skillInfo, bool cancelOk = true)
    {
        this.isPlayedSe = false;
        this.skillInfo = skillInfo;
        this.okButton.SetActive(false);
        Debug.Log(string.Concat(new object[] { "BattleSkillConfComponent::SetSkillConf(", skillInfo.skillId, "):", skillInfo.skilllv, " - ", skillInfo.chargeTurn }));
        if (!skillInfo.isUseSkill)
        {
            this.skillNameLabel.text = string.Empty;
            this.skillConfLabel.text = string.Empty;
        }
        else
        {
            BattleServantData data = this.data.getServantData(skillInfo.svtUniqueId);
            if (this.skillIcon != null)
            {
                bool isActSkill = true;
                if (data != null)
                {
                    isActSkill = data.isUseSkill();
                }
                this.skillIcon.SetSkillInfo(skillInfo, isActSkill);
            }
            if (this.cancelButton != null)
            {
                Collider component = this.cancelButton.GetComponent<Collider>();
                if (component != null)
                {
                    component.enabled = cancelOk;
                }
            }
            if (this.closeButton != null)
            {
                Collider collider2 = this.closeButton.GetComponent<Collider>();
                if (collider2 != null)
                {
                    collider2.enabled = cancelOk;
                }
            }
            SkillEntity entity = null;
            SkillLvEntity entity2 = null;
            Debug.Log("00 ");
            entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(this.skillInfo.skillId);
            entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(this.skillInfo.skillId, this.skillInfo.skilllv);
            if (entity != null)
            {
                this.skillNameLabel.text = $"{entity.getName()}  Lv.{this.skillInfo.skilllv}";
                WrapControlText.textAdjust(this.skillConfLabel, entity2.getDetail(this.skillInfo.skilllv));
                Debug.Log("10 skillEnt.isActive ():" + entity.isActive());
                if (entity.isActive())
                {
                    if (data != null)
                    {
                        Collider collider3 = this.okButton.GetComponent<Collider>();
                        this.okButton.SetActive(true);
                        if ((skillInfo.chargeTurn <= 0) && data.isUseSkill())
                        {
                            collider3.enabled = true;
                        }
                        else
                        {
                            collider3.enabled = false;
                        }
                    }
                    else
                    {
                        Collider collider4 = this.okButton.GetComponent<Collider>();
                        this.okButton.SetActive(true);
                        if (skillInfo.chargeTurn <= 0)
                        {
                            collider4.enabled = true;
                        }
                        else
                        {
                            collider4.enabled = false;
                        }
                    }
                    if (this.skillChargeLabel != null)
                    {
                        this.skillChargeLabel.text = string.Format(LocalizationManager.Get("BATTLE_SKILLCHARGETURN"), entity2.chargeTurn);
                    }
                }
            }
            else
            {
                Debug.Log("skillEnt is Nothing ");
            }
        }
    }
}

