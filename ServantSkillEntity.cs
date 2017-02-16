using System;
using System.Runtime.InteropServices;

public class ServantSkillEntity : DataEntityBase
{
    public int condLimitCount;
    public int condLv;
    public int condQuestId;
    public int num;
    public int priority;
    public int skillId;
    public int svtId;

    public void getAcquisitionMethodExplanation(out string title, out string explanation)
    {
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(this.skillId);
        if (entity != null)
        {
            title = entity.getEffectTitle(0);
            explanation = LocalizationManager.Get("COND_TYPE_TITLE");
            if (this.condLv > 0)
            {
                if (explanation != string.Empty)
                {
                    explanation = explanation + "\n";
                }
                explanation = explanation + CondType.OpenConditionTextServantLevel(this.condLv);
            }
            if (this.condLimitCount > 0)
            {
                if (explanation != string.Empty)
                {
                    explanation = explanation + "\n";
                }
                explanation = explanation + CondType.OpenConditionTextServantLimit(this.condLimitCount);
            }
            if (this.condQuestId > 0)
            {
                if (explanation != string.Empty)
                {
                    explanation = explanation + "\n";
                }
                explanation = explanation + CondType.OpenConditionTextQuestClear(this.condQuestId);
            }
            if (explanation == string.Empty)
            {
                explanation = LocalizationManager.Get("COND_TYPE_NONE");
            }
        }
        else
        {
            title = LocalizationManager.GetUnknownName();
            explanation = string.Empty;
        }
    }

    public void getEffectExplanation(out int charge, out string title, out string explanation, int lv, bool isEquip = false)
    {
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(this.skillId);
        if (entity != null)
        {
            if (isEquip)
            {
                charge = -1;
                title = entity.getEffectTitle(0);
            }
            else
            {
                charge = entity.getEffectChargeTurn(lv);
                title = entity.getEffectTitle(lv);
            }
            explanation = entity.getEffectExplanation(lv);
        }
        else
        {
            charge = -1;
            title = LocalizationManager.GetUnknownName();
            explanation = string.Empty;
        }
    }

    public bool getEventUpVal(ref EventUpValInfo eventUpVallInfo, int skillLv)
    {
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId(this.skillId, skillLv);
        return ((entity != null) && entity.getEventUpVal(ref eventUpVallInfo));
    }

    public bool getEventUpVal(int wearersSvtId, EventUpValSetupInfo setupInfo, int skillLv)
    {
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId(this.skillId, skillLv);
        return ((entity != null) && entity.getEventUpVal(wearersSvtId, setupInfo));
    }

    public int getFriendPointUpVal(int skillLv)
    {
        SkillLvEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillLvMaster>(DataNameKind.Kind.SKILL_LEVEL).getEntityFromId(this.skillId, skillLv);
        if (entity != null)
        {
            return entity.getFriendPointUpVal();
        }
        return 0;
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.svtId, ":", this.num, ":", this.priority };
        return string.Concat(objArray1);
    }

    public int getServantID() => 
        this.svtId;

    public int getServantIdx() => 
        this.num;

    public int getSkillId() => 
        this.skillId;

    public string getSkillName()
    {
        SkillEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(this.skillId);
        if (entity != null)
        {
            return entity.name;
        }
        return LocalizationManager.GetUnknownName();
    }

    public bool isUse(long userId, int lv, int limit, int beforeClearQuestId = -1)
    {
        if (this.condLv > lv)
        {
            return false;
        }
        if (this.condLimitCount > limit)
        {
            return false;
        }
        if ((this.condQuestId > 0) && !CondType.IsQuestClear(userId, this.condQuestId, beforeClearQuestId))
        {
            return false;
        }
        return true;
    }
}

