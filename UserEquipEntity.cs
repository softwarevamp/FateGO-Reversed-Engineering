using System;
using System.Runtime.InteropServices;

public class UserEquipEntity : DataEntityBase
{
    public int equipId;
    public int exp;
    public long id;
    public int lv;
    public int skillLv1;
    public int skillLv2;
    public int skillLv3;
    public long userId;

    public void getEquipInfo(out int condUsrLv, out int maxLv, out string equipName, out string detail, out int genderImageId)
    {
        EquipEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<EquipMaster>(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(this.equipId);
        if (entity != null)
        {
            condUsrLv = entity.condUserLv;
            maxLv = entity.maxLv;
            equipName = entity.name;
            detail = entity.detail;
            genderImageId = 0;
            UserGameEntity entity2 = UserGameMaster.getSelfUserGame();
            if (entity2.genderType == 1)
            {
                genderImageId = entity.maleImageId;
            }
            else if (entity2.genderType == 2)
            {
                genderImageId = entity.femaleImageId;
            }
        }
        else
        {
            condUsrLv = 0;
            maxLv = 0;
            equipName = string.Empty;
            detail = string.Empty;
            genderImageId = 0;
        }
    }

    public void getExpInfo(out int exp, out int lateExp, out float barExp)
    {
        int maxLv = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EquipMaster>(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(this.equipId).maxLv;
        if (this.lv < maxLv)
        {
            EquipExpMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EquipExpMaster>(DataNameKind.Kind.EQUIP_EXP);
            int num2 = 0;
            if (this.lv > 1)
            {
                num2 = master2.getEntityFromId<EquipExpEntity>(this.equipId, this.lv - 1).exp;
            }
            EquipExpEntity entity3 = master2.getEntityFromId<EquipExpEntity>(this.equipId, this.lv);
            exp = this.exp - num2;
            lateExp = entity3.exp - this.exp;
            barExp = ((float) exp) / ((float) (entity3.exp - num2));
        }
        else
        {
            exp = 0;
            lateExp = 0;
            barExp = 1f;
        }
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int[] getSkillIdList()
    {
        int[] numArray = new int[BalanceConfig.UserEquipSkillListMax];
        EquipSkillMaster master = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<EquipSkillMaster>(DataNameKind.Kind.EQUIP_SKILL);
        for (int i = 1; i <= numArray.Length; i++)
        {
            int num2 = 1;
            while (true)
            {
                EquipSkillEntity entity = master.getEntityFromId<EquipSkillEntity>(this.equipId, i);
                if (entity == null)
                {
                    break;
                }
                if (entity.isUse(this.lv))
                {
                    numArray[i - 1] = entity.getSkillId();
                    break;
                }
                numArray[i - 1] = -1;
                num2++;
            }
        }
        return numArray;
    }

    public int getSkillLv(int index)
    {
        EquipExpEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EquipExpMaster>(DataNameKind.Kind.EQUIP_EXP).getEntityFromId<EquipExpEntity>(this.equipId, this.lv);
        if (index == 0)
        {
            return entity.skillLv1;
        }
        if (index == 1)
        {
            return entity.skillLv2;
        }
        if (index == 2)
        {
            return entity.skillLv3;
        }
        return 0;
    }

    public int[] getSkillLvList()
    {
        EquipExpEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EquipExpMaster>(DataNameKind.Kind.EQUIP_EXP).getEntityFromId<EquipExpEntity>(this.equipId, this.lv);
        return new int[] { entity.skillLv1, entity.skillLv2, entity.skillLv3 };
    }

    public bool IsNew()
    {
        if (NetworkManager.UserId != this.userId)
        {
            return false;
        }
        return UserEquipNewManager.IsNew(this.equipId, this.lv);
    }

    public void SetOld()
    {
        UserEquipNewManager.SetOld(this.equipId, this.lv);
    }
}

