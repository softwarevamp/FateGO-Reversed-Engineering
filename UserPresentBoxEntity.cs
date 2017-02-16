using System;
using System.Runtime.InteropServices;

public class UserPresentBoxEntity : DataEntityBase
{
    public long createdAt;
    public long fromId;
    public int fromType;
    public int giftType;
    public bool isAuto;
    public string message;
    public int messageId;
    public int messageRefType;
    public int num;
    public int objectId;
    public long presentId;
    public long receiveUserId;
    public int status;

    public long expireAt() => 
        (this.createdAt + BalanceConfig.PresentBoxValidTime);

    public void GetInfo(out string nameText, out string countText)
    {
        switch (this.giftType)
        {
            case 1:
            {
                ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.objectId);
                nameText = entity2.name;
                if (!entity2.IsServant)
                {
                    if (entity2.IsServantEquip)
                    {
                        countText = string.Format(LocalizationManager.Get("SERVANT_EQUIP_UNIT"), this.num);
                    }
                    else if (entity2.IsCombineMaterial || entity2.IsServantEquipMaterial)
                    {
                        countText = string.Format(LocalizationManager.Get("SERVANT_MATERIAL_UNIT"), this.num);
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
        object[] objArray1 = new object[] { string.Empty, this.receiveUserId, ":", this.presentId };
        return string.Concat(objArray1);
    }

    public bool IsExpired() => 
        (this.expireAt() <= NetworkManager.getTime());
}

