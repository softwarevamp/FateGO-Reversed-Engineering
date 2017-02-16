using System;
using System.Runtime.InteropServices;

public class SkillEntity : DataEntityBase
{
    public int category;
    public int[] effectList;
    public int iconId;
    public int id;
    public int maxLv;
    public int motion;
    public string name;
    public string ruby;
    public int type;

    public int getEffectChargeTurn(int lv = 0)
    {
        if (this.type == 1)
        {
            SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(this.id, (lv <= 0) ? 1 : lv);
            if (entity != null)
            {
                return entity.chargeTurn;
            }
        }
        return -1;
    }

    public string getEffectExplanation(int lv = 0)
    {
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId<SkillLvEntity>(this.id, (lv <= 0) ? 1 : lv);
        if (entity != null)
        {
            return entity.getDetail(lv);
        }
        return LocalizationManager.GetUnknownName();
    }

    public int[] getEffectList() => 
        this.effectList;

    public string getEffectTitle(int lv = 0)
    {
        if (lv > 0)
        {
            return string.Format(LocalizationManager.Get("SKILL_EFFECT_LEVEL_TITLE"), this.name, lv);
        }
        return string.Format(LocalizationManager.Get("SKILL_EFFECT_TITLE"), this.name);
    }

    public bool getEventUpVal(ref EventUpValInfo eventUpVallInfo)
    {
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId(this.id, 1);
        return ((entity != null) && entity.getEventUpVal(ref eventUpVallInfo));
    }

    public bool getEventUpVal(int wearersSvtId, EventUpValSetupInfo setupInfo)
    {
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId(this.id, 1);
        return ((entity != null) && entity.getEventUpVal(wearersSvtId, setupInfo));
    }

    public string getName() => 
        this.name;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public void getSkillMessageInfo(out string name, out string detail, int lv = 0)
    {
        name = this.name;
        detail = this.getEffectExplanation(lv);
    }

    public bool isActive() => 
        (this.type == 1);

    public bool isPassive() => 
        (this.type == 2);
}

