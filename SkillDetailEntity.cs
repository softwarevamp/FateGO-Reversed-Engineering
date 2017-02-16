using System;

public class SkillDetailEntity : DataEntityBase
{
    public string detail;
    public int id;

    public static string getDetail(int id)
    {
        SkillDetailMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillDetailMaster>(DataNameKind.Kind.SKILL_DETAIL);
        long[] args = new long[] { (long) id };
        if (master.isEntityExistsFromId(args))
        {
            return master.getEntityFromId<SkillDetailEntity>(id).detail;
        }
        Debug.LogWarning("TreasureDvcDetailEntity[" + id + "] is null");
        return LocalizationManager.GetUnknownName();
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

