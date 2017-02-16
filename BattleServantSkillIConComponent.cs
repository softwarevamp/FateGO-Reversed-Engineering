using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleServantSkillIConComponent : BaseMonoBehaviour
{
    private bool flashFlg;
    public GameObject flashIcon;
    public UISprite noActSprite;
    public GameObject root;
    public SHOW_TYPE showType;
    public UISprite skillIcon;
    private BattleSkillInfoData skillInfo;
    public GameObject target;
    public UILabel turnNoLabel;

    public void changeTurnCount(int turn)
    {
        if ((this.turnNoLabel != null) && !this.skillInfo.isPassive)
        {
            if (0 < turn)
            {
                if (this.turnNoLabel != null)
                {
                    this.turnNoLabel.gameObject.SetActive(true);
                    this.turnNoLabel.text = string.Empty + turn;
                }
            }
            else
            {
                this.turnNoLabel.gameObject.SetActive(false);
            }
        }
    }

    public void OnClick()
    {
        Debug.Log("OnClick::");
        if (this.target != null)
        {
            BattleServantParamComponent component = this.target.GetComponent<BattleServantParamComponent>();
            if (component != null)
            {
                component.clickSkillIcon(this.skillInfo);
            }
            BattlePerformanceMaster master = this.target.GetComponent<BattlePerformanceMaster>();
            if (master != null)
            {
                master.clickSkillIcon(this.skillInfo);
            }
        }
    }

    public void setCollider(bool flg)
    {
        Collider component = base.gameObject.GetComponent<Collider>();
        if (component != null)
        {
            component.enabled = flg;
        }
        if ((base.gameObject.GetComponent<TweenScale>() != null) && !flg)
        {
            base.gameObject.transform.localScale = Vector3.one;
        }
    }

    public void setflashFlg(bool flg)
    {
        this.flashFlg = flg;
        this.updateFlashSkill();
    }

    public void setNoSkill(int val = 0)
    {
        if (val == 0)
        {
            this.root.SetActive(false);
        }
        else if (val == -1)
        {
            this.setCollider(false);
        }
    }

    public void SetSkillInfo(BattleSkillInfoData skillInfo, bool isActSkill = true)
    {
        this.skillInfo = skillInfo;
        this.root.SetActive(true);
        AtlasManager.SetSkillIcon(this.skillIcon, skillInfo.skillId);
        if ((this.turnNoLabel != null) && skillInfo.isPassive)
        {
            this.turnNoLabel.gameObject.SetActive(false);
        }
        if (this.showType == SHOW_TYPE.NOTOUCH)
        {
            this.setCollider(false);
        }
        else if (!skillInfo.isPassive)
        {
            if (0 < skillInfo.chargeTurn)
            {
                this.skillIcon.color = Color.gray;
                this.setCollider(true);
            }
            else
            {
                this.skillIcon.color = Color.white;
                this.setCollider(true);
            }
            this.updateFlashSkill();
            this.changeTurnCount(skillInfo.chargeTurn);
        }
        if (this.noActSprite != null)
        {
            this.noActSprite.gameObject.SetActive(false);
            if (!isActSkill)
            {
                this.noActSprite.gameObject.SetActive(true);
                this.setflashFlg(false);
            }
        }
    }

    public void showChageEffect()
    {
        if ((this.skillInfo != null) && this.skillInfo.isCharge)
        {
            base.createObject("effect/ef_command_flash01", this.root.transform, null);
            this.skillInfo.isCharge = false;
        }
    }

    public void updateFlashSkill()
    {
        if (this.flashIcon != null)
        {
            if (this.skillInfo == null)
            {
                this.flashIcon.SetActive(false);
            }
            else if (!this.flashFlg)
            {
                this.flashIcon.SetActive(false);
            }
            else if (this.flashFlg)
            {
                this.flashIcon.SetActive(this.skillInfo.chargeTurn <= 0);
            }
        }
    }

    public enum SHOW_TYPE
    {
        NONE,
        NOMAL,
        NOTOUCH
    }
}

