using System;
using System.Runtime.InteropServices;

public class ServantEntity : DataEntityBase
{
    public int attackAttri;
    public int attri;
    public int baseSvtId;
    public string battleName;
    public int battleSize;
    public int[] cardIds;
    public int classId;
    public int[] classPassive;
    public int collectionNo;
    public int combineLimitId;
    public int combineMaterialId;
    public int combineSkillId;
    public int combineTdId;
    public int cost;
    public int cvId;
    public int expType;
    public string foldername;
    public int friendshipId;
    public int genderType;
    public float headupY;
    public int hpGaugeY;
    public int id;
    public int illustratorId;
    public int[] individuality;
    public int limitMax;
    public string name;
    public int rewardLv;
    public string ruby;
    public int sellMana;
    public int sellQp;
    public int starRate;
    public int type;

    public bool checkIsCombineMaterialSvt() => 
        (this.type == 3);

    public bool checkIsHeroineSvt() => 
        (this.type == 2);

    public string getClassName()
    {
        if (!this.IsServantEquip)
        {
            ServantClassEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(this.classId);
            if (entity != null)
            {
                return entity.name;
            }
        }
        return string.Empty;
    }

    public int[] getClassPassive() => 
        this.classPassive;

    public void getClassSkillInfo(out int[] idList, out string[] titleList, out string[] explanationList)
    {
        int num = (this.classPassive == null) ? 0 : this.classPassive.Length;
        idList = new int[num];
        titleList = new string[num];
        explanationList = new string[num];
        SkillMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL);
        for (int i = 0; i < num; i++)
        {
            int id = this.classPassive[i];
            SkillEntity entity = master.getEntityFromId<SkillEntity>(id);
            idList[i] = id;
            if (entity != null)
            {
                entity.getSkillMessageInfo(out titleList[i], out explanationList[i], 0);
            }
        }
    }

    public bool getEventUpVal(ref EventUpValInfo eventUpVallInfo)
    {
        bool flag = false;
        if (this.classPassive != null)
        {
            SkillMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL);
            int length = this.classPassive.Length;
            for (int i = 0; i < length; i++)
            {
                int id = this.classPassive[i];
                SkillEntity entity = master.getEntityFromId<SkillEntity>(id);
                if ((entity != null) && entity.getEventUpVal(ref eventUpVallInfo))
                {
                    flag = true;
                }
            }
        }
        return flag;
    }

    public bool getEventUpVal(int wearersSvtId, EventUpValSetupInfo setupInfo)
    {
        if (this.classPassive != null)
        {
            SkillMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL);
            int length = this.classPassive.Length;
            for (int i = 0; i < length; i++)
            {
                int id = this.classPassive[i];
                SkillEntity entity = master.getEntityFromId<SkillEntity>(id);
                if ((entity != null) && entity.getEventUpVal(wearersSvtId, setupInfo))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public float getHeadUpY() => 
        (((float) this.hpGaugeY) / 1000f);

    public int[] getIndividuality()
    {
        if (this.individuality == null)
        {
            return new int[0];
        }
        return this.individuality;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public bool IsCombineMaterial =>
        SvtType.IsCombineMaterial((SvtType.Type) this.type);

    public bool IsEnemy =>
        SvtType.IsEnemy((SvtType.Type) this.type);

    public bool IsEnemyCollectionDetail =>
        SvtType.IsEnemyCollectionDetail((SvtType.Type) this.type);

    public bool IsExpUp =>
        SvtType.IsExpUp((SvtType.Type) this.type);

    public bool IsKeepServant =>
        SvtType.IsKeepServant((SvtType.Type) this.type);

    public bool IsKeepServantEquip =>
        SvtType.IsKeepServantEquip((SvtType.Type) this.type);

    public bool IsOrganization =>
        SvtType.IsOrganization((SvtType.Type) this.type);

    public bool IsServant =>
        SvtType.IsServant((SvtType.Type) this.type);

    public bool IsServantCollection =>
        SvtType.IsServantCollection((SvtType.Type) this.type);

    public bool IsServantEquip =>
        SvtType.IsServantEquip((SvtType.Type) this.type);

    public bool IsServantEquipMaterial =>
        SvtType.IsSvtEqMaterial((SvtType.Type) this.type);

    public bool IsStatusUp =>
        SvtType.IsStatusUp((SvtType.Type) this.type);

    public bool IsSvtEqMaterial =>
        SvtType.IsSvtEqMaterial((SvtType.Type) this.type);
}

