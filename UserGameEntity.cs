using System;
using System.Runtime.InteropServices;

public class UserGameEntity : DataEntityBase
{
    public long activeDeckId;
    public int actMax;
    public long actRecoverAt;
    public long birthDay;
    public int carryOverActPoint;
    public int chargeStone;
    public long commandSpellRecoverAt;
    public int costMax;
    public long createdAt;
    public int exp;
    public long favoriteUserSvtId;
    public int freeStone;
    public string friendCode;
    public int friendKeep;
    public int genderType;
    public string getPay;
    public int lv;
    public long mainDeckId;
    public int mana;
    public string name;
    public int qp;
    public int stone;
    public int stoneVerifiAt;
    public int svtEquipKeep;
    public int svtKeep;
    public long sweepNum;
    public long tutorial1;
    public long tutorial2;
    public int tutorialProgress;
    public long userEquipId;
    public long userId;

    public bool CheckHasQp(int needQp) => 
        (this.qp < needQp);

    public int getAct()
    {
        int num = this.getBaseAct();
        if (num == this.actMax)
        {
            num += this.carryOverActPoint;
        }
        return num;
    }

    public long getActAllRecoverTime()
    {
        long num = this.actRecoverAt - NetworkManager.getTime();
        return ((num <= 0L) ? 0L : num);
    }

    public int getActMax() => 
        this.actMax;

    public long getActNextRecoverTime()
    {
        long num = this.actRecoverAt - NetworkManager.getTime();
        if (num > 0L)
        {
            long num2 = num % ((long) BalanceConfig.UerGameActRecoverCost);
            return ((num2 <= 0L) ? ((long) BalanceConfig.UerGameActRecoverCost) : num2);
        }
        return 0L;
    }

    public int getBaseAct()
    {
        long num = this.actRecoverAt - NetworkManager.getTime();
        if (num > 0L)
        {
            long num2 = ((num + BalanceConfig.UerGameActRecoverCost) - 1L) / ((long) BalanceConfig.UerGameActRecoverCost);
            return ((this.actMax <= num2) ? 0 : (this.actMax - ((int) num2)));
        }
        return this.actMax;
    }

    public int getCarryOverAct()
    {
        if (this.IsNeedRecoverAct())
        {
            return 0;
        }
        return this.carryOverActPoint;
    }

    public bool getCmdSpellInfo(out int count, out long recoverTime)
    {
        long num = this.commandSpellRecoverAt - NetworkManager.getTime();
        int commandSpellMax = BalanceConfig.CommandSpellMax;
        int commandSpellRecoverCost = BalanceConfig.CommandSpellRecoverCost;
        if (num > 0L)
        {
            long num4 = ((num + commandSpellRecoverCost) - 1L) / ((long) commandSpellRecoverCost);
            count = (commandSpellMax <= num4) ? 0 : (commandSpellMax - ((int) num4));
            long num5 = num % ((long) commandSpellRecoverCost);
            recoverTime = (num5 <= 0L) ? ((long) commandSpellRecoverCost) : num5;
        }
        else
        {
            count = commandSpellMax;
            recoverTime = 0L;
        }
        return true;
    }

    public int getCommandSpell()
    {
        long num = this.commandSpellRecoverAt - NetworkManager.getTime();
        int num2 = ConstantMaster.getValue("MAX_COMMAND_SPELL");
        if (num > 0L)
        {
            int num3 = ConstantMaster.getValue("ONE_COMMAND_SPELL");
            long num4 = ((num + num3) - 1L) / ((long) num3);
            return ((num2 <= num4) ? 0 : (num2 - ((int) num4)));
        }
        return num2;
    }

    public static int GetEquipImageId(int genderType, long equipId)
    {
        EquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(equipId);
        if (genderType == 2)
        {
            return entity.femaleImageId;
        }
        return entity.maleImageId;
    }

    public bool getExpInfo(out int exp, out int lateExp, out float barExp)
    {
        if (this.lv < BalanceConfig.UserLevelMax)
        {
            UserExpMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserExpMaster>(DataNameKind.Kind.USER_EXP);
            int num = 0;
            if (this.lv > 1)
            {
                num = master.getEntityFromId<UserExpEntity>((int) (this.lv - 1)).exp;
            }
            UserExpEntity entity2 = master.getEntityFromId<UserExpEntity>(this.lv);
            exp = this.exp - num;
            lateExp = entity2.exp - this.exp;
            barExp = ((float) exp) / ((float) (entity2.exp - num));
            return true;
        }
        exp = 0;
        lateExp = 0;
        barExp = 1f;
        return false;
    }

    public int GetFriendPoint() => 
        SingletonMonoBehaviour<DataManager>.getInstance().getMasterData<TblUserMaster>(DataNameKind.Kind.TBL_USER_GAME).getUserData(this.userId).friendPoint;

    public int getLv() => 
        this.lv;

    public string[] getPey()
    {
        string[] separator = new string[] { "," };
        return this.getPay.Split(separator, StringSplitOptions.RemoveEmptyEntries);
    }

    public override string getPrimarykey() => 
        (string.Empty + this.userId);

    public static int GetSpellImageId(int genderType, long equipId)
    {
        EquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(equipId);
        if (genderType == 2)
        {
            return entity.femaleSpellId;
        }
        return entity.maleSpellId;
    }

    public bool getTutorialFlag(int flagId) => 
        TutorialFlag.Get(this, flagId);

    public long getUserId() => 
        this.userId;

    public string getUsername() => 
        this.name;

    public bool IsNeedRecoverAct()
    {
        long num = this.actRecoverAt - NetworkManager.getTime();
        return (num > 0L);
    }

    public void setTutorialFlag(int flagId)
    {
        TutorialFlag.Set(this, flagId);
    }

    public int EquipImageId
    {
        get
        {
            UserEquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(this.userEquipId);
            return GetEquipImageId(this.genderType, (long) entity.equipId);
        }
    }

    public int SpellImageId
    {
        get
        {
            UserEquipEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EQUIP).getEntityFromId<UserEquipEntity>(this.userEquipId);
            return ((entity == null) ? 0 : GetSpellImageId(this.genderType, (long) entity.equipId));
        }
    }
}

