using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EquipSkillInfoComponent : MonoBehaviour
{
    protected ClickDelegate clickCallbackFunc;
    private int equipSkillId;
    private int equipSkillLv;
    public UIButton iconBtn;
    protected IconLabelInfo iconLabelInfo = new IconLabelInfo();
    public NewIconComponent newIcon;
    public SkillIconComponent skillIconInfo;
    public UISprite skillIndxImg;
    public UILabel skillLvLabel;
    public UILabel skillNameLb;

    public void OnClickSkill()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        if (this.clickCallbackFunc != null)
        {
            this.clickCallbackFunc(this.equipSkillId, this.equipSkillLv);
        }
    }

    public void setEquipSkillInfo(int idx, int skillId, int skillLv, int skillIconId, bool isNew, ClickDelegate callback)
    {
        this.clickCallbackFunc = callback;
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(skillId);
        this.skillIndxImg.spriteName = "img_skill_0" + (idx + 1);
        this.skillIconInfo.Set(skillId, skillLv);
        this.skillNameLb.text = entity.name;
        this.skillLvLabel.text = string.Format(LocalizationManager.Get("MASTER_EQSKILL_LV_INFO"), skillLv, entity.maxLv);
        this.equipSkillId = skillId;
        this.equipSkillLv = skillLv;
        if (this.newIcon != null)
        {
            if (isNew)
            {
                this.newIcon.Set();
            }
            else
            {
                this.newIcon.Clear();
            }
        }
    }

    public delegate void ClickDelegate(int skillId, int skillLv);
}

