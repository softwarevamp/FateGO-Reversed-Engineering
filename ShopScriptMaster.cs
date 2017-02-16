using System;

public class ShopScriptMaster : DataMasterBase
{
    public ShopScriptMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.SHOP_SCRIPT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new ShopScriptEntity[1]);
        }
    }

    public ShopScriptEntity getEntityFromId(int shopId)
    {
        string key = string.Empty + shopId;
        if (base.lookup.ContainsKey(key))
        {
            return (base.lookup[key] as ShopScriptEntity);
        }
        return null;
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<ShopScriptEntity>(obj);
}

