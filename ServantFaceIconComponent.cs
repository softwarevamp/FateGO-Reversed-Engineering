using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantFaceIconComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected UISprite backSprite;
    [SerializeField]
    protected UISprite classSprite;
    [SerializeField]
    protected UISprite equipLimitCountSprite;
    [SerializeField]
    protected UIMeshSprite equipSprite;
    [SerializeField]
    protected UISprite faceSprite;
    [SerializeField]
    protected UISprite frameSprite;
    [SerializeField]
    protected UIIconLabel iconLabel;
    [SerializeField]
    protected UISprite noneEquipSprite;
    [SerializeField]
    protected UISprite rarity2Sprite;
    [SerializeField]
    protected UISprite raritySprite;
    [SerializeField]
    protected ShiningIconComponent shiningIcon;
    [SerializeField]
    protected UISprite typeSprite;

    protected void Awake()
    {
        Vector3 vector = new Vector3(this.typeSprite.transform.localPosition.x, -64f, 0f);
        this.typeSprite.transform.localPosition = vector;
    }

    public void Blank()
    {
        this.Clear();
        this.frameSprite.spriteName = "img_commonbg_02";
    }

    public void Clear()
    {
        this.backSprite.spriteName = null;
        this.faceSprite.spriteName = null;
        this.frameSprite.spriteName = null;
        this.typeSprite.spriteName = null;
        if (this.classSprite != null)
        {
            this.classSprite.spriteName = null;
        }
        if (this.raritySprite != null)
        {
            this.raritySprite.spriteName = null;
        }
        if (this.iconLabel != null)
        {
            this.iconLabel.Clear();
        }
        if (this.shiningIcon != null)
        {
            this.shiningIcon.Clear();
        }
        this.ClearEquip();
    }

    protected void ClearEquip()
    {
        if (this.equipSprite != null)
        {
            this.noneEquipSprite.gameObject.SetActive(false);
            this.equipSprite.spriteName = null;
            if (this.equipLimitCountSprite != null)
            {
                this.equipLimitCountSprite.gameObject.SetActive(false);
            }
            if (this.rarity2Sprite != null)
            {
                this.rarity2Sprite.spriteName = null;
            }
        }
    }

    protected void ClearEquip(int baseSvtId)
    {
        if (this.equipSprite != null)
        {
            if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(baseSvtId).IsServant)
            {
                this.noneEquipSprite.gameObject.SetActive(true);
                this.equipSprite.spriteName = null;
                if (this.equipLimitCountSprite != null)
                {
                    this.equipLimitCountSprite.gameObject.SetActive(false);
                }
                if (this.rarity2Sprite != null)
                {
                    this.rarity2Sprite.spriteName = this.raritySprite.spriteName;
                    this.rarity2Sprite.MakePixelPerfect();
                }
            }
            else
            {
                this.noneEquipSprite.gameObject.SetActive(false);
                this.equipSprite.spriteName = null;
                if (this.equipLimitCountSprite != null)
                {
                    this.equipLimitCountSprite.gameObject.SetActive(false);
                }
                if (this.rarity2Sprite != null)
                {
                    this.rarity2Sprite.spriteName = null;
                }
            }
        }
    }

    public void FaceSpriteDisp(bool flag)
    {
        if (flag)
        {
            this.faceSprite.gameObject.SetActive(true);
            this.iconLabel.gameObject.SetActive(true);
        }
        else
        {
            this.faceSprite.gameObject.SetActive(false);
            this.iconLabel.gameObject.SetActive(false);
        }
    }

    public string GetFaceSpriteName() => 
        this.faceSprite.spriteName;

    public string GetRaritySpriteName()
    {
        if (this.raritySprite != null)
        {
            return this.raritySprite.spriteName;
        }
        return null;
    }

    public bool IsUseEquip()
    {
        if (this.equipSprite != null)
        {
            if (!string.IsNullOrEmpty(this.equipSprite.spriteName))
            {
                return true;
            }
            if (this.noneEquipSprite.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    public void NoMount()
    {
        this.Clear();
        this.backSprite.spriteName = "img_frames_nodata";
    }

    public void Set(EquipTargetInfo equipTargetInfo, IconLabelInfo info = null)
    {
        if (equipTargetInfo == null)
        {
            this.Clear();
        }
        else
        {
            this.Set(equipTargetInfo.svtId, equipTargetInfo.limitCount, 0, 0, info, CollectionStatus.Kind.GET, false, false);
            this.ClearEquip(equipTargetInfo.svtId);
        }
    }

    public void Set(long userSvtId, IconLabelInfo info = null)
    {
        if (userSvtId <= 0L)
        {
            this.Clear();
        }
        else
        {
            this.Set(SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId), info);
        }
    }

    public void Set(UserServantCollectionEntity userSvtColEntity, IconLabelInfo info = null)
    {
        if (userSvtColEntity == null)
        {
            this.Clear();
        }
        else
        {
            ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(userSvtColEntity.svtId);
            CollectionStatus.Kind status = (CollectionStatus.Kind) userSvtColEntity.status;
            int imageLimitCount = ImageLimitCount.GetCardImageLimitCount(userSvtColEntity.svtId, userSvtColEntity.maxLimitCount, true, true);
            if (((entity != null) && entity.IsEnemyCollectionDetail) && (status != CollectionStatus.Kind.NOT_GET))
            {
                status = CollectionStatus.Kind.ENEMY_COLLECTION_DETAIL;
            }
            this.Set(userSvtColEntity.svtId, userSvtColEntity.maxLimitCount, imageLimitCount, 0, info, status, userSvtColEntity.IsNew(), false);
            this.ClearEquip();
        }
    }

    public void Set(UserServantEntity userSvtEntity, IconLabelInfo info = null)
    {
        if (userSvtEntity == null)
        {
            this.Clear();
        }
        else
        {
            if (userSvtEntity.userId == NetworkManager.UserId)
            {
                this.Set(userSvtEntity.svtId, userSvtEntity.limitCount, userSvtEntity.getIconLimitCount(), userSvtEntity.exceedCount, info, CollectionStatus.Kind.GET, userSvtEntity.IsNew(), userSvtEntity.IsEventJoin());
            }
            else
            {
                this.Set(userSvtEntity.svtId, userSvtEntity.limitCount, userSvtEntity.getFrendIconLimitCount(), userSvtEntity.exceedCount, info, CollectionStatus.Kind.GET, false, false);
            }
            this.ClearEquip();
        }
    }

    public void Set(ServantLeaderInfo servantLeaderInfo, IconLabelInfo info = null, bool isNewIconDisp = false)
    {
        if (servantLeaderInfo == null)
        {
            this.NoMount();
        }
        else
        {
            CollectionStatus.Kind kind = !servantLeaderInfo.IsHideSupport() ? CollectionStatus.Kind.GET : CollectionStatus.Kind.HIDE;
            int imageLimitCount = ImageLimitCount.GetCardImageLimitCount(servantLeaderInfo.svtId, servantLeaderInfo.limitCount, false, true);
            if (isNewIconDisp)
            {
                this.Set(servantLeaderInfo.svtId, servantLeaderInfo.limitCount, imageLimitCount, servantLeaderInfo.exceedCount, info, CollectionStatus.Kind.GET, OtherUserNewManager.IsNew(servantLeaderInfo.userId), false);
            }
            else
            {
                this.Set(servantLeaderInfo.svtId, servantLeaderInfo.limitCount, imageLimitCount, servantLeaderInfo.exceedCount, info, CollectionStatus.Kind.GET, false, false);
            }
            this.SetEquip(servantLeaderInfo.svtId, servantLeaderInfo.equipTarget1);
        }
    }

    public void Set(UserServantEntity userSvtEntity, long[] equipIdList, IconLabelInfo info = null)
    {
        if (userSvtEntity == null)
        {
            this.Clear();
        }
        else
        {
            if (userSvtEntity.userId == NetworkManager.UserId)
            {
                this.Set(userSvtEntity.svtId, userSvtEntity.limitCount, userSvtEntity.getIconLimitCount(), userSvtEntity.exceedCount, info, CollectionStatus.Kind.GET, userSvtEntity.IsNew(), userSvtEntity.IsEventJoin());
            }
            else
            {
                this.Set(userSvtEntity.svtId, userSvtEntity.limitCount, userSvtEntity.getFrendIconLimitCount(), userSvtEntity.exceedCount, info, CollectionStatus.Kind.GET, false, false);
            }
            if ((equipIdList != null) && (equipIdList.Length > 0))
            {
                this.SetEquip(userSvtEntity.svtId, equipIdList[0]);
            }
            else
            {
                this.ClearEquip();
            }
        }
    }

    public void Set(int svtId, int limitCount, int imageLimitCount = -1, int exceedCount = 0, IconLabelInfo info = null, CollectionStatus.Kind collectionStatus = 2, bool isNew = false, bool isTemporarySubscription = false)
    {
        this.SetBase(svtId, limitCount, imageLimitCount, exceedCount, collectionStatus, isNew, isTemporarySubscription);
        this.ClearEquip();
        if (this.iconLabel != null)
        {
            if (collectionStatus == CollectionStatus.Kind.HIDE)
            {
                this.iconLabel.Set(info, true);
            }
            else
            {
                this.iconLabel.Set(info);
            }
        }
    }

    protected void SetBase(int svtId, int limitCount, int imageLimitCount, int exceedCount, CollectionStatus.Kind collectionStatus, bool isNew, bool isTemporarySubscription)
    {
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId);
        ServantLimitEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(svtId, limitCount);
        if (imageLimitCount < 0)
        {
            imageLimitCount = (svtId <= 0) ? 0 : ImageLimitCount.GetImageLimitCount(svtId, limitCount);
        }
        int frameType = Rarity.getFrameTypeImage(entity2.rarity);
        switch (collectionStatus)
        {
            case CollectionStatus.Kind.NOT_GET:
                this.backSprite.spriteName = "img_frames_nodata";
                AtlasManager.SetFace(this.faceSprite, 0, 0);
                this.frameSprite.spriteName = null;
                this.typeSprite.spriteName = null;
                if (this.equipSprite != null)
                {
                    this.noneEquipSprite.gameObject.SetActive(false);
                    this.equipSprite.spriteName = null;
                    if (this.equipLimitCountSprite != null)
                    {
                        this.equipLimitCountSprite.gameObject.SetActive(false);
                    }
                }
                break;

            case CollectionStatus.Kind.FIND:
                this.backSprite.spriteName = Rarity.getFaceBaseIcon(entity2.rarity);
                AtlasManager.SetFaceImage(this.faceSprite, svtId, imageLimitCount);
                this.frameSprite.spriteName = "listframes_shadow";
                AtlasManager.SetServantType(this.typeSprite, entity.type, entity2.rarity);
                break;

            case CollectionStatus.Kind.GET:
            case CollectionStatus.Kind.ENEMY_COLLECTION_DETAIL:
                this.backSprite.spriteName = Rarity.getFaceBaseIcon(entity2.rarity);
                AtlasManager.SetFaceImage(this.faceSprite, svtId, imageLimitCount);
                this.frameSprite.spriteName = "listframes_shadow";
                AtlasManager.SetServantType(this.typeSprite, entity.type, entity2.rarity);
                break;

            case CollectionStatus.Kind.HIDE:
                AtlasManager.SetFaceBaseIcon(this.backSprite, frameType);
                AtlasManager.SetHideFace(this.faceSprite);
                this.frameSprite.spriteName = "listframes_shadow";
                AtlasManager.SetServantType(this.typeSprite, entity.type, frameType);
                break;
        }
        if (this.classSprite != null)
        {
            if (((collectionStatus == CollectionStatus.Kind.NOT_GET) || (collectionStatus == CollectionStatus.Kind.HIDE)) || entity.IsServantEquip)
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
            if (collectionStatus == CollectionStatus.Kind.NOT_GET)
            {
                this.raritySprite.spriteName = null;
            }
            else
            {
                this.raritySprite.spriteName = Rarity.getIcon(entity2.rarity);
                this.raritySprite.MakePixelPerfect();
            }
        }
        if (this.shiningIcon != null)
        {
            if (isTemporarySubscription)
            {
                this.shiningIcon.Set("icon_eventjoin_01");
            }
            else if (isNew)
            {
                this.shiningIcon.Set("icon_common_new01");
            }
            else
            {
                this.shiningIcon.Clear();
            }
        }
    }

    public void SetEquip(UserServantEntity userServantEntity)
    {
        if (userServantEntity == null)
        {
            this.Clear();
        }
        else
        {
            this.SetEquip(0, userServantEntity.svtId, userServantEntity.limitCount);
        }
    }

    protected void SetEquip(int baseSvtId, EquipTargetInfo equipTarget)
    {
        if ((equipTarget != null) && (equipTarget.svtId > 0))
        {
            this.SetEquip(baseSvtId, equipTarget.svtId, equipTarget.limitCount);
        }
        else
        {
            this.ClearEquip(baseSvtId);
        }
    }

    protected void SetEquip(int baseSvtId, long userSvtId)
    {
        if (this.equipSprite != null)
        {
            if (userSvtId > 0L)
            {
                UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
                if (entity != null)
                {
                    this.SetEquip(baseSvtId, entity.svtId, entity.limitCount);
                    return;
                }
            }
            this.ClearEquip(baseSvtId);
        }
    }

    protected void SetEquip(int baseSvtId, int svtId, int limitCount)
    {
        if (this.equipSprite != null)
        {
            if (svtId > 0)
            {
                this.noneEquipSprite.gameObject.SetActive(false);
                AtlasManager.SetEquipFace(this.equipSprite, svtId);
                if (this.equipLimitCountSprite != null)
                {
                    int limitMax = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId).limitMax;
                    this.equipLimitCountSprite.gameObject.SetActive((limitMax > 0) && (limitCount >= limitMax));
                }
                if (this.rarity2Sprite != null)
                {
                    this.rarity2Sprite.spriteName = this.raritySprite.spriteName;
                    this.rarity2Sprite.MakePixelPerfect();
                }
                this.typeSprite.spriteName = null;
            }
            else
            {
                this.ClearEquip(baseSvtId);
            }
        }
    }

    public void SetEquipDangling(EquipTargetInfo info)
    {
        this.NoMount();
        if ((info != null) && (info.svtId != 0))
        {
            this.SetEquip(0, info);
        }
    }
}

