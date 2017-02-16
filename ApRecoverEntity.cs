using System;

public class ApRecoverEntity : DataEntityBase
{
    public int id;
    public int priority;
    public int recoverType;
    public int targetId;

    public CommandSpellEntity getApRecvCmdSpellData()
    {
        CommandSpellEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<CommandSpellMaster>(DataNameKind.Kind.COMMAND_SPELL).getEntityFromId<CommandSpellEntity>(this.targetId);
        if (entity != null)
        {
            return entity;
        }
        return null;
    }

    public ItemEntity getApRecvItemData()
    {
        ItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.targetId);
        if (entity != null)
        {
            return entity;
        }
        return null;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

