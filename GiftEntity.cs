using System;
using System.Runtime.InteropServices;

public class GiftEntity : DataEntityBase
{
    public int id;
    public int limitCount;
    public int lv;
    public int num;
    public int objectId;
    public int priority;
    public int type;

    public GiftEntity()
    {
    }

    public GiftEntity(GiftEntity cSrc)
    {
        this.id = cSrc.id;
        this.type = cSrc.type;
        this.objectId = cSrc.objectId;
        this.num = cSrc.num;
        this.limitCount = cSrc.limitCount;
        this.lv = cSrc.lv;
    }

    public void GetInfo(out string nameText, out string countText)
    {
        switch (this.type)
        {
            case 1:
            case 6:
            case 7:
            {
                ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.objectId);
                nameText = entity2.name;
                if (!entity2.IsServant)
                {
                    if (entity2.IsServantEquip)
                    {
                        countText = string.Format(LocalizationManager.Get("SERVANT_EQUIP_UNIT"), this.num);
                    }
                    else
                    {
                        countText = string.Format(LocalizationManager.Get("EXCEPT_SERVANT_UNIT"), this.num);
                    }
                    break;
                }
                countText = string.Format(LocalizationManager.Get("SERVANT_UNIT"), this.num);
                break;
            }
            case 2:
            {
                ItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.objectId);
                nameText = entity.name;
                countText = entity.GetCountText(this.num);
                break;
            }
            case 3:
                nameText = LocalizationManager.Get("FRIENDSHIP_NAME");
                countText = string.Format(LocalizationManager.Get("FRIENDSHIP_UNIT"), this.num);
                break;

            case 4:
                nameText = LocalizationManager.Get("USER_EXP_NAME");
                countText = string.Format(LocalizationManager.Get("USER_EXP_UNIT"), this.num);
                break;

            case 5:
            {
                EquipEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(this.objectId);
                nameText = entity3.name;
                countText = string.Format(LocalizationManager.Get("UNIT_INFO"), this.num);
                break;
            }
            default:
                nameText = LocalizationManager.GetUnknownName();
                countText = string.Empty;
                break;
        }
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.type, ":", this.objectId };
        return string.Concat(objArray1);
    }

    public bool isQp()
    {
        if (this.type == 2)
        {
            ItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.objectId);
            if (entity != null)
            {
                return ((entity.type == 1) || (0x10 == entity.type));
            }
        }
        return false;
    }
}

