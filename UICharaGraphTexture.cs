using System;
using UnityEngine;

public class UICharaGraphTexture : UICharaGraphRender
{
    protected AssetData assetMain;
    [SerializeField]
    protected UILabel attackLabel;
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected UITexture bodyTexture;
    protected System.Action callbackFunc;
    [SerializeField]
    protected UISprite classSprite;
    protected EquipTargetInfo equipTargetInfo;
    [SerializeField]
    protected UISprite frameBottomSprite;
    [SerializeField]
    protected UISprite frameLeftSprite;
    [SerializeField]
    protected UISprite frameRightSprite;
    [SerializeField]
    protected UISprite frameTopSprite;
    [SerializeField]
    protected UILabel hpLabel;
    protected string[] loadNameList;
    [SerializeField]
    protected UITexture nameTexture;
    [SerializeField]
    protected UISprite raritySprite;
    protected ServantLeaderInfo servantLeaderInfo;
    protected UserServantCollectionEntity userSvtCollectionEntity;
    protected UserServantEntity userSvtEntity;

    public void Destroy()
    {
        this.OnDestroy();
    }

    protected void EndLoadAsset()
    {
        if (this.loadNameList != null)
        {
            AssetData data = AssetManager.getAssetStorage(this.loadNameList[0]);
            if (data != null)
            {
                if (this.assetMain != null)
                {
                    AssetManager.releaseAsset(this.assetMain);
                }
                this.assetMain = data;
                this.loadNameList = null;
                this.SetFrame();
                this.SetTexture();
                System.Action callbackFunc = this.callbackFunc;
                if (callbackFunc != null)
                {
                    this.callbackFunc = null;
                    callbackFunc();
                }
            }
        }
    }

    protected void LoadCharacter(System.Action callbackFunc)
    {
        string[] assetName = CharaGraphManager.GetAssetName(base.svtId, base.imageLimitCount);
        if (this.loadNameList != null)
        {
            if (AssetManager.compAssetStorageList(this.loadNameList, assetName))
            {
                if (callbackFunc != null)
                {
                    this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
                }
                this.SetBeforeFrame();
                return;
            }
            AssetManager.releaseAssetStorage(this.loadNameList);
        }
        else if ((this.assetMain != null) && this.assetMain.Name.Equals(assetName[0]))
        {
            this.SetFrame();
            this.SetTexture();
            if (callbackFunc != null)
            {
                callbackFunc();
            }
            return;
        }
        if (callbackFunc != null)
        {
            this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
        }
        this.SetBeforeFrame();
        this.loadNameList = assetName;
        AssetManager.loadAssetStorage(this.loadNameList, new System.Action(this.EndLoadAsset));
    }

    protected void OnDestroy()
    {
        this.ReleaseCharacter();
    }

    public void ReleaseCharacter()
    {
        this.bodyTexture.gameObject.SetActive(false);
        this.bodyTexture.mainTexture = null;
        if (this.assetMain != null)
        {
            AssetManager.releaseAsset(this.assetMain);
            this.assetMain = null;
        }
        if (this.loadNameList != null)
        {
            AssetManager.releaseAssetStorage(this.loadNameList);
            this.loadNameList = null;
        }
    }

    public void SetActive(bool isActive)
    {
        this.bodyTexture.gameObject.SetActive(isActive);
    }

    protected void SetBeforeFrame()
    {
        if (!this.nameTexture.gameObject.activeSelf)
        {
            this.SetFrame();
        }
    }

    public void SetCharacter(EquipTargetInfo equipTargetInfo, bool isReal, System.Action callbackFunc)
    {
        base.SetCharacter(equipTargetInfo.svtId, equipTargetInfo.limitCount, false, isReal);
        this.userSvtEntity = null;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = equipTargetInfo;
        this.LoadCharacter(callbackFunc);
    }

    public void SetCharacter(EquipTargetInfo equipTargetInfo, int imageLimitCount, System.Action callbackFunc)
    {
        base.SetCharacter(equipTargetInfo.svtId, equipTargetInfo.limitCount, imageLimitCount);
        base.SetAtk(equipTargetInfo.atk, 0);
        base.SetHp(equipTargetInfo.hp, 0);
        this.userSvtEntity = null;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = equipTargetInfo;
        this.LoadCharacter(callbackFunc);
    }

    public void SetCharacter(ServantLeaderInfo servantLeaderInfo, bool isReal, System.Action callbackFunc)
    {
        base.SetCharacter(servantLeaderInfo.svtId, servantLeaderInfo.limitCount, false, isReal);
        this.userSvtEntity = null;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = servantLeaderInfo;
        this.equipTargetInfo = null;
        this.LoadCharacter(callbackFunc);
    }

    public void SetCharacter(ServantLeaderInfo servantLeaderInfo, int imageLimitCount, System.Action callbackFunc)
    {
        base.SetCharacter(servantLeaderInfo.svtId, servantLeaderInfo.limitCount, imageLimitCount);
        base.SetAtk(servantLeaderInfo.atk, servantLeaderInfo.adjustAtk);
        base.SetHp(servantLeaderInfo.hp, servantLeaderInfo.adjustHp);
        this.userSvtEntity = null;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = servantLeaderInfo;
        this.equipTargetInfo = null;
        this.LoadCharacter(callbackFunc);
    }

    public void SetCharacter(UserServantCollectionEntity userSvtCollectionEntity, bool isReal, System.Action callbackFunc)
    {
        base.SetCharacter(userSvtCollectionEntity.svtId, userSvtCollectionEntity.maxLimitCount, true, isReal);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(base.svtId, base.limitCount);
        base.SetAtk(entity.atkMax, 0);
        base.SetHp(entity.hpMax, 0);
        this.userSvtEntity = null;
        this.userSvtCollectionEntity = userSvtCollectionEntity;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = null;
        this.LoadCharacter(callbackFunc);
    }

    public void SetCharacter(UserServantCollectionEntity userSvtCollectionEntity, int imageLimitCount, System.Action callbackFunc)
    {
        base.SetCharacter(userSvtCollectionEntity.svtId, userSvtCollectionEntity.maxLimitCount, imageLimitCount);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(base.svtId, base.limitCount);
        base.SetAtk(entity.atkMax, 0);
        base.SetHp(entity.hpMax, 0);
        this.userSvtEntity = null;
        this.userSvtCollectionEntity = userSvtCollectionEntity;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = null;
        this.LoadCharacter(callbackFunc);
    }

    public void SetCharacter(UserServantEntity userSvtEntity, bool isReal, System.Action callbackFunc)
    {
        base.SetCharacter(userSvtEntity.svtId, userSvtEntity.limitCount, true, isReal);
        base.SetAtk(userSvtEntity.atk, userSvtEntity.adjustAtk);
        base.SetHp(userSvtEntity.hp, userSvtEntity.adjustHp);
        this.userSvtEntity = userSvtEntity;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = null;
        this.LoadCharacter(callbackFunc);
    }

    public void SetCharacter(UserServantEntity userSvtEntity, int imageLimitCount, System.Action callbackFunc)
    {
        base.SetCharacter(userSvtEntity.svtId, userSvtEntity.limitCount, imageLimitCount);
        base.SetAtk(userSvtEntity.atk, userSvtEntity.adjustAtk);
        base.SetHp(userSvtEntity.hp, userSvtEntity.adjustHp);
        this.userSvtEntity = userSvtEntity;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = null;
        this.LoadCharacter(callbackFunc);
    }

    public void SetCharacter(int svtId, int limitCount, bool isOwn, bool isReal, System.Action callbackFunc)
    {
        base.SetCharacter(svtId, limitCount, isOwn, isReal);
        ServantLimitEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(base.svtId, base.limitCount);
        base.SetAtk(entity.atkBase, 0);
        base.SetHp(entity.hpBase, 0);
        this.userSvtEntity = null;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = null;
        this.LoadCharacter(callbackFunc);
    }

    public void SetDepth(int d)
    {
        this.baseSprite.depth = d;
        this.bodyTexture.depth = d + 1;
        this.frameTopSprite.depth = d + 2;
        this.frameBottomSprite.depth = d + 2;
        this.frameLeftSprite.depth = d + 3;
        this.frameRightSprite.depth = d + 3;
        this.raritySprite.depth = d + 4;
        this.classSprite.depth = d + 4;
        this.nameTexture.depth = d + 5;
        this.attackLabel.depth = d + 6;
        this.hpLabel.depth = d + 6;
    }

    protected void SetFrame()
    {
        string frameSprite = base.GetFrameSprite();
        if (string.IsNullOrEmpty(frameSprite))
        {
            this.frameLeftSprite.spriteName = null;
            this.frameRightSprite.spriteName = null;
            this.frameTopSprite.spriteName = null;
            this.frameBottomSprite.spriteName = null;
            this.raritySprite.spriteName = null;
            this.classSprite.spriteName = null;
        }
        else if (base.baseKind == UICharaGraphRender.Kind.SERVANT)
        {
            AtlasManager.SetCharaGraphaOption(this.frameLeftSprite, frameSprite + "L");
            AtlasManager.SetCharaGraphaOption(this.frameRightSprite, frameSprite + "R");
            AtlasManager.SetCharaGraphaOption(this.frameTopSprite, frameSprite + "T");
            AtlasManager.SetCharaGraphaOption(this.frameBottomSprite, frameSprite + "B");
            AtlasManager.SetCharaGraphaOption(this.raritySprite, base.GetRaritySprite());
            AtlasManager.SetCharaGraphaOption(this.classSprite, base.GetClassSprite());
        }
        else if (base.baseKind == UICharaGraphRender.Kind.SERVANT_EQUIP)
        {
            this.frameLeftSprite.spriteName = null;
            this.frameRightSprite.spriteName = null;
            AtlasManager.SetCharaGraphaOption(this.frameTopSprite, "e" + frameSprite + "T");
            AtlasManager.SetCharaGraphaOption(this.frameBottomSprite, "e" + frameSprite + "B");
            AtlasManager.SetCharaGraphaOption(this.raritySprite, base.GetRaritySprite());
            this.classSprite.spriteName = null;
        }
        else
        {
            this.frameLeftSprite.spriteName = null;
            this.frameRightSprite.spriteName = null;
            this.frameTopSprite.spriteName = null;
            this.frameBottomSprite.spriteName = null;
            this.raritySprite.spriteName = null;
            this.classSprite.spriteName = null;
        }
        if (SvtType.IsEnemyCollectionDetail(base.svtType))
        {
            this.attackLabel.text = string.Empty;
            this.hpLabel.text = string.Empty;
        }
        else if (SvtType.IsStatusUp(base.svtType))
        {
            int num = base.atk * BalanceConfig.StatusUpAdjustAtk;
            int num2 = base.hp * BalanceConfig.StatusUpAdjustHp;
            this.attackLabel.text = (num <= 0) ? (string.Empty + num) : ("+" + num);
            this.hpLabel.text = (num2 <= 0) ? (string.Empty + num2) : ("+" + num2);
        }
        else if (SvtType.IsKeepServantEquip(base.svtType))
        {
            this.attackLabel.text = (base.atk <= 0) ? (string.Empty + base.atk) : ("+" + base.atk);
            this.hpLabel.text = (base.hp <= 0) ? (string.Empty + base.hp) : ("+" + base.hp);
        }
        else
        {
            this.attackLabel.text = (base.atk < 0) ? string.Empty : (string.Empty + base.atk);
            this.hpLabel.text = (base.hp < 0) ? string.Empty : (string.Empty + base.hp);
        }
        this.attackLabel.color = (base.adjustAtk <= 0) ? Color.white : Color.yellow;
        this.hpLabel.color = (base.adjustHp <= 0) ? Color.white : Color.yellow;
    }

    protected void SetTexture()
    {
        this.bodyTexture.mainTexture = base.GetBodyTexture(this.assetMain);
        this.bodyTexture.uvRect = base.GetBodyUvRect();
        Texture2D nameTexture = base.GetNameTexture(this.assetMain);
        this.nameTexture.gameObject.SetActive(nameTexture != null);
        this.nameTexture.mainTexture = nameTexture;
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        this.bodyTexture.color = c;
    }

    public bool IsLoad =>
        (this.loadNameList != null);
}

