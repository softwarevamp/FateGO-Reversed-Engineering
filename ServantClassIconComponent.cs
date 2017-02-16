using System;
using UnityEngine;

public class ServantClassIconComponent : MonoBehaviour
{
    [SerializeField]
    protected UISprite iconSprite;
    [SerializeField]
    protected UISprite nameSprite;

    public void Clear()
    {
        if (this.iconSprite != null)
        {
            this.iconSprite.spriteName = null;
        }
        if (this.nameSprite != null)
        {
            this.nameSprite.spriteName = null;
        }
    }

    public void Set(int classId)
    {
        this.setImage(classId, 2);
    }

    public void Set(int classId, int rarity)
    {
        this.setImage(classId, Rarity.getFrameTypeImage(rarity));
    }

    public void Set(int svtId, int limitCount, int exceedCount)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId);
        ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, limitCount);
        int classId = entity.classId;
        int frameType = Rarity.getFrameTypeImage(entity2.rarity);
        int rarity = entity2.rarity;
        this.setImage(classId, frameType);
    }

    public void SetColor(Color col)
    {
        if (this.iconSprite != null)
        {
            this.iconSprite.color = col;
        }
        if (this.nameSprite != null)
        {
            this.nameSprite.color = col;
        }
    }

    public void setImage(int classId, int frameType)
    {
        ServantClassEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantClassMaster>(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(classId);
        if (this.iconSprite != null)
        {
            if (entity != null)
            {
                AtlasManager.SetClassIcon(this.iconSprite, entity.iconImageId, frameType);
            }
            else
            {
                this.iconSprite.spriteName = null;
            }
        }
        if (this.nameSprite != null)
        {
            if (entity != null)
            {
                AtlasManager.SetClassTextIcon(this.nameSprite, entity.iconImageId, frameType);
            }
            else
            {
                this.nameSprite.spriteName = null;
            }
        }
    }
}

