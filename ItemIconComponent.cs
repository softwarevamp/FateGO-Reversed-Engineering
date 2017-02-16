using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ItemIconComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected UISprite backSprite;
    [SerializeField]
    protected UISprite classSprite;
    [SerializeField]
    protected UILabel counterLabel;
    [SerializeField]
    protected UISprite frameSprite;
    [SerializeField]
    protected UISprite iconSprite;
    [SerializeField]
    protected UISprite raritySprite;
    [SerializeField]
    protected UISprite typeSprite;

    protected void Awake()
    {
        if (this.typeSprite != null)
        {
            Vector3 vector = new Vector3(this.typeSprite.transform.localPosition.x, -64f, 0f);
            this.typeSprite.transform.localPosition = vector;
        }
    }

    public void Clear()
    {
        if (this.typeSprite != null)
        {
            this.backSprite.spriteName = Rarity.getFaceBaseIcon(0);
            this.iconSprite.spriteName = null;
            this.frameSprite.spriteName = "listframes_shadow";
            this.typeSprite.spriteName = null;
        }
        else
        {
            if (this.backSprite != null)
            {
                this.backSprite.gameObject.SetActive(false);
            }
            this.iconSprite.spriteName = null;
        }
        if (this.classSprite != null)
        {
            this.classSprite.spriteName = null;
        }
        if (this.raritySprite != null)
        {
            this.raritySprite.spriteName = null;
        }
        if (this.counterLabel != null)
        {
            this.counterLabel.text = string.Empty;
        }
    }

    public void SetAlpha(float alpha)
    {
        if (this.backSprite != null)
        {
            this.backSprite.alpha = alpha;
        }
        if (this.iconSprite != null)
        {
            this.iconSprite.alpha = alpha;
        }
        if (this.frameSprite != null)
        {
            this.frameSprite.alpha = alpha;
        }
        if (this.typeSprite != null)
        {
            this.typeSprite.alpha = alpha;
        }
        if (this.classSprite != null)
        {
            this.classSprite.alpha = alpha;
        }
        if (this.raritySprite != null)
        {
            this.raritySprite.alpha = alpha;
        }
        if (this.counterLabel != null)
        {
            this.counterLabel.alpha = alpha;
        }
    }

    public void SetColor(Color col)
    {
        if (this.backSprite != null)
        {
            this.backSprite.color = col;
        }
        if (this.iconSprite != null)
        {
            this.iconSprite.color = col;
        }
        if (this.frameSprite != null)
        {
            this.frameSprite.color = col;
        }
        if (this.typeSprite != null)
        {
            this.typeSprite.color = col;
        }
        if (this.classSprite != null)
        {
            this.classSprite.color = col;
        }
        if (this.raritySprite != null)
        {
            this.raritySprite.color = col;
        }
        if (this.counterLabel != null)
        {
            this.counterLabel.color = col;
        }
    }

    public void SetCombineItem(int itemId, int count = -1)
    {
        ItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(itemId);
        this.SetCombineItemImage((ImageItem.Id) entity.imageId, entity.bgImageId, count);
    }

    public void SetCombineItemImage(ImageItem.Id imageId, int bgImageId, int count = -1)
    {
        if (this.backSprite != null)
        {
            this.backSprite.gameObject.SetActive(true);
        }
        if (this.iconSprite != null)
        {
            AtlasManager.SetItem(this.iconSprite, (int) imageId);
        }
        if (this.frameSprite != null)
        {
            this.frameSprite.spriteName = $"listframes{bgImageId}_bg";
        }
        if (this.typeSprite != null)
        {
            this.typeSprite.spriteName = $"listframes{bgImageId}_txt_item";
        }
        if (this.classSprite != null)
        {
            this.classSprite.spriteName = null;
        }
        if (this.raritySprite != null)
        {
            this.raritySprite.spriteName = null;
        }
        if (this.counterLabel != null)
        {
            this.counterLabel.text = (count < 0) ? string.Empty : (string.Empty + count);
        }
    }

    public void SetDepth(int depth)
    {
        if (this.typeSprite != null)
        {
            this.backSprite.depth = depth++;
            this.iconSprite.depth = depth++;
            this.frameSprite.depth = depth++;
            if (this.classSprite != null)
            {
                this.classSprite.depth = depth;
            }
            if (this.raritySprite != null)
            {
                this.raritySprite.depth = depth;
            }
            this.typeSprite.depth = depth++;
        }
        else
        {
            if (this.backSprite != null)
            {
                this.backSprite.depth = depth;
            }
            depth++;
            if (this.iconSprite != null)
            {
                this.iconSprite.depth = depth;
            }
            depth++;
            if (this.frameSprite != null)
            {
                this.frameSprite.depth = depth;
            }
            depth++;
            if (this.classSprite != null)
            {
                this.classSprite.depth = depth;
            }
            if (this.raritySprite != null)
            {
                this.raritySprite.depth = depth;
            }
            depth++;
        }
        if (this.counterLabel != null)
        {
            this.counterLabel.depth = depth;
        }
    }

    public void SetEquipItem(int equipItemId)
    {
        this.SetEquipItemImage(equipItemId);
    }

    public void SetEquipItemImage(int equipImageId)
    {
        if (this.iconSprite != null)
        {
            AtlasManager.SetEquipItem(this.iconSprite, equipImageId);
        }
    }

    public void SetFaceImage(int svtId, int limitCount, int count = -1)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId);
        ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, limitCount);
        int frameType = Rarity.getFrameTypeImage(entity2.rarity);
        if (this.typeSprite != null)
        {
            this.backSprite.spriteName = Rarity.getFaceBaseIcon(entity2.rarity);
            AtlasManager.SetFace(this.iconSprite, svtId, limitCount);
            this.frameSprite.spriteName = "listframes_shadow";
            AtlasManager.SetServantType(this.typeSprite, entity.type, entity2.rarity);
        }
        else
        {
            if (this.backSprite != null)
            {
                this.backSprite.gameObject.SetActive(false);
            }
            if (this.iconSprite != null)
            {
                AtlasManager.SetFace(this.iconSprite, svtId, limitCount);
            }
            if (this.frameSprite != null)
            {
                this.frameSprite.spriteName = Rarity.getFaceFrameIcon(entity2.rarity);
            }
        }
        if (this.classSprite != null)
        {
            if (entity.IsServantEquip)
            {
                this.classSprite.spriteName = null;
            }
            else
            {
                AtlasManager.SetClass(this.classSprite, entity.classId, frameType);
            }
        }
        if (this.raritySprite != null)
        {
            this.raritySprite.spriteName = Rarity.getIcon(entity2.rarity);
            this.raritySprite.MakePixelPerfect();
        }
        if (this.counterLabel != null)
        {
            this.counterLabel.text = (count < 0) ? string.Empty : (string.Empty + count);
        }
    }

    public void SetGift(Gift.Type giftType, int objectId = 0, int count = -1)
    {
        switch (giftType)
        {
            case Gift.Type.SERVANT:
            case Gift.Type.EVENT_SVT_JOIN:
            case Gift.Type.EVENT_SVT_GET:
                if (objectId <= 0)
                {
                    this.SetItemImage(ImageItem.Id.SERVANT, count);
                    break;
                }
                this.SetFaceImage(objectId, 0, count);
                break;

            case Gift.Type.ITEM:
            {
                if (objectId <= 0)
                {
                    goto Label_00FC;
                }
                ItemEntity itemEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(objectId);
                if (itemEnt.imageId <= 0)
                {
                    this.SetPresentItem(itemEnt, count);
                    return;
                }
                this.SetItemImage((ImageItem.Id) itemEnt.imageId, itemEnt.bgImageId, itemEnt.type, count);
                return;
            }
            case Gift.Type.EQUIP:
            {
                if (objectId <= 0)
                {
                    goto Label_00FC;
                }
                EquipEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EQUIP).getEntityFromId<EquipEntity>(objectId);
                UserGameEntity entity3 = UserGameMaster.getSelfUserGame();
                int equipImageId = 0;
                if (entity3.genderType == 1)
                {
                    equipImageId = entity2.maleImageId;
                }
                else if (entity3.genderType == 2)
                {
                    equipImageId = entity2.femaleImageId;
                }
                this.SetEquipItemImage(equipImageId);
                return;
            }
            default:
                goto Label_00FC;
        }
        return;
    Label_00FC:
        this.SetItemImage(ImageItem.Id.NONE, count);
    }

    public void SetItem(UserItemEntity usrItemEntity)
    {
        ItemEntity itemEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(usrItemEntity.itemId);
        this.SetItem(itemEntity, usrItemEntity.num);
    }

    public void SetItem(ItemEntity itemEntity, int count = -1)
    {
        this.SetItemImage((ImageItem.Id) itemEntity.imageId, itemEntity.bgImageId, itemEntity.type, count);
    }

    public void SetItem(int itemId, int count = -1)
    {
        ItemEntity itemEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(itemId);
        this.SetItem(itemEntity, count);
    }

    public void SetItemImage(ImageItem.Id imageId)
    {
        this.SetItemImage(imageId, 0, -1, -1);
    }

    public void SetItemImage(ImageItem.Id imageId, int count)
    {
        this.SetItemImage(imageId, 0, -1, count);
    }

    public void SetItemImage(ImageItem.Id imageId, int bgImageId, int type, int count)
    {
        if (this.typeSprite != null)
        {
            this.backSprite.spriteName = $"listframes{bgImageId}_bg";
            AtlasManager.SetItem(this.iconSprite, (int) imageId);
            this.frameSprite.spriteName = "listframes_shadow";
            if (type == 0x10)
            {
                this.typeSprite.spriteName = $"listframes{bgImageId}_txt_quest";
            }
            else if (type == 14)
            {
                this.typeSprite.spriteName = $"listframes{bgImageId}_txt_point";
            }
            else
            {
                this.typeSprite.spriteName = $"listframes{bgImageId}_txt_item";
            }
        }
        else
        {
            if (this.backSprite != null)
            {
                this.backSprite.gameObject.SetActive(true);
            }
            if (this.iconSprite != null)
            {
                AtlasManager.SetItem(this.iconSprite, (int) imageId);
            }
            if (this.frameSprite != null)
            {
                this.frameSprite.spriteName = "img_common_frame01";
            }
        }
        if (this.classSprite != null)
        {
            this.classSprite.spriteName = null;
        }
        if (this.raritySprite != null)
        {
            this.raritySprite.spriteName = null;
        }
        if (this.counterLabel != null)
        {
            if ((type == 1) || (type == 0x10))
            {
                this.counterLabel.text = (count < 0) ? string.Empty : $"+{count:#,0}";
            }
            else if (type == 14)
            {
                this.counterLabel.text = (count < 0) ? string.Empty : $"+{count:#,0}";
            }
            else
            {
                this.counterLabel.text = (count < 0) ? string.Empty : ("x" + count);
            }
        }
    }

    public void SetPointEvent(int eventId)
    {
        int pointEventImageId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).GetPointEventImageId(eventId);
        this.SetItemImage((ImageItem.Id) pointEventImageId);
    }

    public void SetPresentItem(ItemEntity itemEnt, int count = -1)
    {
        switch (itemEnt.type)
        {
            case 1:
                this.SetItemImage(ImageItem.Id.QP, itemEnt.bgImageId, itemEnt.type, count);
                return;

            case 2:
                this.SetItemImage(ImageItem.Id.STONE, itemEnt.bgImageId, itemEnt.type, count);
                return;

            case 5:
                this.SetItemImage(ImageItem.Id.MANA, itemEnt.bgImageId, itemEnt.type, count);
                return;

            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
            case 11:
                this.SetItemImage(ImageItem.Id.SKILL, itemEnt.bgImageId, itemEnt.type, count);
                return;

            case 12:
                this.SetItemImage(ImageItem.Id.NP, itemEnt.bgImageId, itemEnt.type, count);
                return;
        }
        this.SetItemImage(ImageItem.Id.NONE, count);
    }

    public void SetPurchase(Purchase.Type purchaseType, int targetId, int imageId)
    {
        if (imageId > 0)
        {
            this.SetItemImage((ImageItem.Id) imageId);
        }
        else
        {
            switch (purchaseType)
            {
                case Purchase.Type.ITEM:
                {
                    ItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(targetId);
                    this.SetItemImage((ImageItem.Id) entity.imageId, entity.bgImageId, entity.type, 0);
                    return;
                }
                case Purchase.Type.SERVANT:
                    this.SetFaceImage(targetId, 0, -1);
                    return;
            }
            this.SetItemImage((ImageItem.Id) imageId);
        }
    }
}

