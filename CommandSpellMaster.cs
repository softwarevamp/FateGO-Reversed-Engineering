using System;

public class CommandSpellMaster : DataMasterBase
{
    public CommandSpellMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.COMMAND_SPELL);
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<CommandSpellEntity>(obj);
}

