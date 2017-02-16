using System;

public class CardMaster : DataMasterBase
{
    public CardMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.CARD);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new CardEntity[1]);
        }
    }

    public static float getAtk(int type, int num)
    {
        CardMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CardMaster>(DataNameKind.Kind.CARD);
        long[] args = new long[] { (long) type, (long) (num + 1) };
        if (!master.isEntityExistsFromId(args))
        {
            return 1f;
        }
        return (((float) master.getEntityFromId<CardEntity>(type, num + 1).adjustAtk) / 1000f);
    }

    public static float getCritical(int type, int num)
    {
        CardMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CardMaster>(DataNameKind.Kind.CARD);
        long[] args = new long[] { (long) type, (long) (num + 1) };
        if (!master.isEntityExistsFromId(args))
        {
            return 0f;
        }
        return (((float) master.getEntityFromId<CardEntity>(type, num + 1).adjustCritical) / 1000f);
    }

    public static int[] getIndividualities(int type, int num)
    {
        CardMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CardMaster>(DataNameKind.Kind.CARD);
        long[] args = new long[] { (long) type, (long) num };
        if (master.isEntityExistsFromId(args))
        {
            return master.getEntityFromId<CardEntity>(type, num).individuality;
        }
        long[] numArray2 = new long[] { (long) type, 1L };
        if (master.isEntityExistsFromId(numArray2))
        {
            return master.getEntityFromId<CardEntity>(type, 1).individuality;
        }
        return new int[0];
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CardEntity>(obj);

    public static float getTdGauge(int type, int num)
    {
        CardMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CardMaster>(DataNameKind.Kind.CARD);
        long[] args = new long[] { (long) type, (long) (num + 1) };
        if (!master.isEntityExistsFromId(args))
        {
            return 0f;
        }
        return (((float) master.getEntityFromId<CardEntity>(type, num + 1).adjustTdGauge) / 1000f);
    }
}

