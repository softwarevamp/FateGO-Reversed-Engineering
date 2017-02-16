using System;
using UnityEngine;

public class SupportSelectItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UILabel attackLabel;
    [SerializeField]
    protected UISprite backClassIcon;
    private static readonly string[] backClassIconFileList = new string[] { "icon_class1001", "icon_class001", "icon_class002", "icon_class003", "icon_class004", "icon_class005", "icon_class006", "icon_class007" };
    [SerializeField]
    protected UISprite base2Sprite;
    [SerializeField]
    protected UICommonButton baseButton;
    protected Vector3 baseRarityPosition;
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected ShiningIconComponent bounusIcon;
    [SerializeField]
    protected UILabel costLabel;
    [SerializeField]
    protected UISprite equipLimitCountSprite;
    protected ServantEntity equipServantEntity;
    [SerializeField]
    protected UIMeshSprite equipSprite;
    protected UserServantEntity equipUserServantEntity;
    [SerializeField]
    protected UILabel hpLabel;
    [SerializeField]
    protected UILabel levelLabel;
    [SerializeField]
    protected UISprite noneEquipSprite;
    [SerializeField]
    protected UISprite raritySprite;
    [SerializeField]
    protected ServantClassIconComponent servantClassIcon;
    protected ServantEntity servantEntity;
    [SerializeField]
    protected UINarrowFigureTexture servantNarrowTexture;
    [SerializeField]
    protected UILabel skillLevelListLabel;
    protected UserServantEntity userServantEntity;

    protected void Awake()
    {
        if (this.raritySprite != null)
        {
            this.baseRarityPosition = this.raritySprite.transform.localPosition;
        }
    }

    public void ClearItem()
    {
        base.gameObject.SetActive(false);
        this.servantNarrowTexture.ReleaseCharacter();
        if (this.noneEquipSprite != null)
        {
            this.noneEquipSprite.gameObject.SetActive(false);
            this.equipSprite.gameObject.SetActive(false);
        }
    }

    public void SetItem(SupportServantData supportServantData, int classPos, DispMode mode)
    {
        base.gameObject.SetActive(true);
        UserServantLearderEntity e = supportServantData.getUserServantLearderEntity(classPos);
        if (supportServantData.getEquip(classPos) != 0)
        {
            if (supportServantData.IsFriendInfo)
            {
                this.equipUserServantEntity = new UserServantEntity(e.equipTarget1);
            }
            else
            {
                this.equipUserServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(supportServantData.getEquip(classPos));
            }
            this.equipServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.equipUserServantEntity.svtId);
        }
        else
        {
            this.equipUserServantEntity = null;
            this.equipServantEntity = null;
        }
        if ((e == null) || (e.userSvtId == 0))
        {
            this.userServantEntity = null;
            this.servantEntity = null;
            this.backClassIcon.gameObject.SetActive(true);
            this.backClassIcon.spriteName = backClassIconFileList[classPos];
            if (this.baseSprite != null)
            {
                AtlasManager.SetFormationBase(this.baseSprite, 5);
            }
            if (this.base2Sprite != null)
            {
                AtlasManager.SetFormationFrameForSupportSelect(this.base2Sprite, 5);
            }
            if (this.servantClassIcon != null)
            {
                this.servantClassIcon.gameObject.SetActive(false);
            }
            if (this.levelLabel != null)
            {
                this.levelLabel.text = string.Empty;
            }
            if (this.raritySprite != null)
            {
                this.raritySprite.gameObject.SetActive(false);
            }
            if (this.skillLevelListLabel != null)
            {
                this.skillLevelListLabel.text = string.Empty;
            }
            if (this.attackLabel != null)
            {
                this.attackLabel.text = string.Empty;
            }
            if (this.hpLabel != null)
            {
                this.hpLabel.text = string.Empty;
            }
        }
        else
        {
            int[] numArray;
            int[] numArray2;
            int[] numArray3;
            string[] strArray;
            string[] strArray2;
            int svtId = e.svtId;
            bool isDisp = false;
            string levelList = string.Empty;
            if (supportServantData.IsFriendInfo)
            {
                this.userServantEntity = new UserServantEntity(e);
            }
            else
            {
                this.userServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(e.userSvtId);
            }
            this.servantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId);
            int classId = this.servantEntity.classId;
            int rarity = this.userServantEntity.getRarity();
            int exceedCount = this.userServantEntity.exceedCount;
            int frameType = this.userServantEntity.getFrameType();
            if (supportServantData.IsFriendInfo)
            {
                ServantLeaderInfo info = e.getServantLeaderInfo();
                this.servantNarrowTexture.SetCharacter(svtId, ImageLimitCount.GetCardImageLimitCount(svtId, info.limitCount, false, false), null);
                info.getSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
            }
            else
            {
                this.servantNarrowTexture.SetCharacter(svtId, this.userServantEntity.getDispCardImageLimitCount(false), null);
                this.userServantEntity.getSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
            }
            levelList = LocalizationManager.GetLevelList(numArray2);
            if (this.servantClassIcon != null)
            {
                this.servantClassIcon.gameObject.SetActive(true);
                this.servantClassIcon.setImage(classId, frameType);
            }
            if (this.levelLabel != null)
            {
                this.levelLabel.text = string.Empty + this.userServantEntity.lv;
            }
            if (this.raritySprite != null)
            {
                this.raritySprite.gameObject.SetActive(true);
                AtlasManager.SetRarityIcon(this.raritySprite, rarity, exceedCount);
                Vector3 baseRarityPosition = this.baseRarityPosition;
                if (exceedCount > 0)
                {
                    baseRarityPosition.x++;
                }
                this.raritySprite.transform.localPosition = baseRarityPosition;
            }
            if (this.skillLevelListLabel != null)
            {
                this.skillLevelListLabel.text = levelList;
            }
            if (this.attackLabel != null)
            {
                Color color = (this.userServantEntity.adjustAtk <= 0) ? Color.white : Color.yellow;
                this.attackLabel.color = color;
                int num6 = this.userServantEntity.atk + this.userServantEntity.adjustAtk;
                if (this.equipUserServantEntity != null)
                {
                    num6 += this.equipUserServantEntity.atk + this.equipUserServantEntity.adjustAtk;
                }
                this.attackLabel.text = string.Empty + num6;
            }
            if (this.hpLabel != null)
            {
                Color color2 = (this.userServantEntity.adjustHp <= 0) ? Color.white : Color.yellow;
                this.hpLabel.color = color2;
                int num7 = this.userServantEntity.hp + this.userServantEntity.adjustHp;
                if (this.equipUserServantEntity != null)
                {
                    num7 += this.equipUserServantEntity.hp + this.equipUserServantEntity.adjustHp;
                }
                this.hpLabel.text = string.Empty + num7;
            }
            if (this.baseSprite != null)
            {
                AtlasManager.SetFormationBase(this.baseSprite, frameType);
            }
            if (this.base2Sprite != null)
            {
                AtlasManager.SetFormationFrameForSupportSelect(this.base2Sprite, frameType);
            }
            this.backClassIcon.gameObject.SetActive(false);
            if (this.bounusIcon != null)
            {
                this.bounusIcon.Set(isDisp);
            }
        }
        if (this.equipUserServantEntity != null)
        {
            this.noneEquipSprite.gameObject.SetActive(false);
            this.equipSprite.gameObject.SetActive(true);
            AtlasManager.SetEquipFace(this.equipSprite, this.equipUserServantEntity.svtId);
            if (this.equipLimitCountSprite != null)
            {
                int limitCount = this.equipUserServantEntity.limitCount;
                this.equipLimitCountSprite.gameObject.SetActive((limitCount > 3) && (this.equipServantEntity.limitMax >= limitCount));
            }
        }
        else
        {
            this.noneEquipSprite.gameObject.SetActive(true);
            this.equipSprite.gameObject.SetActive(false);
            if (this.equipLimitCountSprite != null)
            {
                this.equipLimitCountSprite.gameObject.SetActive(false);
            }
        }
        if (this.baseButton != null)
        {
            this.baseButton.SetState(UICommonButtonColor.State.Normal, true);
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT,
        DRAG_INVISIBLE
    }
}

