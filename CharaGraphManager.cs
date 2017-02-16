using System;
using UnityEngine;

public class CharaGraphManager : SingletonMonoBehaviour<CharaGraphManager>
{
    [SerializeField]
    protected GameObject charaEnemyCollectionDetailGraphPrefab;
    [SerializeField]
    protected GameObject charaEquipGraphPrefab;
    [SerializeField]
    protected GameObject charaGraphPrefab;

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, int svtId)
    {
        GameObject obj2;
        ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(svtId);
        if (entity.IsEnemyCollectionDetail)
        {
            obj2 = UnityEngine.Object.Instantiate<GameObject>(this.charaEnemyCollectionDetailGraphPrefab);
        }
        else if (entity.IsServantEquip)
        {
            obj2 = UnityEngine.Object.Instantiate<GameObject>(this.charaEquipGraphPrefab);
        }
        else
        {
            obj2 = UnityEngine.Object.Instantiate<GameObject>(this.charaGraphPrefab);
        }
        UICharaGraphTexture component = obj2.GetComponent<UICharaGraphTexture>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        obj2.name = "UICharaGraphTexture";
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.SetLayer(parent.layer);
        return component;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, EquipTargetInfo equipTargetInfo, bool isReal, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, equipTargetInfo.svtId);
        texture.gameObject.name = "UICharaGraphTexture(" + equipTargetInfo.svtId + ")";
        texture.SetCharacter(equipTargetInfo, isReal, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, EquipTargetInfo equipTargetInfo, int imageLimitCount, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, equipTargetInfo.svtId);
        object[] objArray1 = new object[] { "UICharaGraphTexture(", equipTargetInfo.svtId, "-I", imageLimitCount, ")" };
        texture.gameObject.name = string.Concat(objArray1);
        texture.SetCharacter(equipTargetInfo, imageLimitCount, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, ServantLeaderInfo servantLeaderInfo, bool isReal, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, servantLeaderInfo.svtId);
        texture.gameObject.name = "UICharaGraphTexture(" + servantLeaderInfo.svtId + ")";
        texture.SetCharacter(servantLeaderInfo, isReal, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, ServantLeaderInfo servantLeaderInfo, int imageLimitCount, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, servantLeaderInfo.svtId);
        object[] objArray1 = new object[] { "UICharaGraphTexture(", servantLeaderInfo.svtId, "-I", imageLimitCount, ")" };
        texture.gameObject.name = string.Concat(objArray1);
        texture.SetCharacter(servantLeaderInfo, imageLimitCount, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, UserServantCollectionEntity userSvtCollectionEntity, bool isReal, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, userSvtCollectionEntity.svtId);
        texture.gameObject.name = "UICharaGraphTexture(" + userSvtCollectionEntity.svtId + ")";
        texture.SetCharacter(userSvtCollectionEntity, isReal, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, UserServantCollectionEntity userSvtCollectionEntity, int imageLimitCount, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, userSvtCollectionEntity.svtId);
        object[] objArray1 = new object[] { "UICharaGraphTexture(", userSvtCollectionEntity.svtId, "-I", imageLimitCount, ")" };
        texture.gameObject.name = string.Concat(objArray1);
        texture.SetCharacter(userSvtCollectionEntity, imageLimitCount, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, UserServantEntity userSvtEntity, bool isReal, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, userSvtEntity.svtId);
        texture.gameObject.name = "UICharaGraphTexture(" + userSvtEntity.svtId + ")";
        texture.SetCharacter(userSvtEntity, isReal, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, UserServantEntity userSvtEntity, int imageLimitCount, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, userSvtEntity.svtId);
        object[] objArray1 = new object[] { "UICharaGraphTexture(", userSvtEntity.svtId, "-I", imageLimitCount, ")" };
        texture.gameObject.name = string.Concat(objArray1);
        texture.SetCharacter(userSvtEntity, imageLimitCount, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public UICharaGraphTexture CreateTextureLocal(GameObject parent, int svtId, int limitCount, bool isOwn, bool isReal, int depth, System.Action callbackFunc)
    {
        UICharaGraphTexture texture = this.CreateTextureLocal(parent, svtId);
        object[] objArray1 = new object[] { "UICharaGraphTexture(", svtId, limitCount, ")" };
        texture.gameObject.name = string.Concat(objArray1);
        texture.SetCharacter(svtId, limitCount, isOwn, isReal, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, EquipTargetInfo equipTargetInfo, bool isReal, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, equipTargetInfo, isReal, depth, callbackFunc);

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, EquipTargetInfo equipTargetInfo, int imageLimitCount, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, equipTargetInfo, imageLimitCount, depth, callbackFunc);

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, ServantLeaderInfo servantLeaderInfo, bool isReal, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, servantLeaderInfo, isReal, depth, callbackFunc);

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, ServantLeaderInfo servantLeaderInfo, int imageLimitCount, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, servantLeaderInfo, imageLimitCount, depth, callbackFunc);

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, long userSvtId, bool isReal, int depth, System.Action callbackFunc)
    {
        UserServantEntity userSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        return SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, userSvtEntity, isReal, depth, callbackFunc);
    }

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, UserServantCollectionEntity userSvtCollectionEntity, bool isReal, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, userSvtCollectionEntity, isReal, depth, callbackFunc);

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, UserServantCollectionEntity userSvtCollectionEntity, int imageLimitCount, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, userSvtCollectionEntity, imageLimitCount, depth, callbackFunc);

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, UserServantEntity userSvtEntity, bool isReal, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, userSvtEntity, isReal, depth, callbackFunc);

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, UserServantEntity userSvtEntity, int imageLimitCount, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, userSvtEntity, imageLimitCount, depth, callbackFunc);

    public static UICharaGraphTexture CreateTexturePrefab(GameObject parent, int svtId, int limitCount, bool isOwn, bool isReal, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<CharaGraphManager>.Instance.CreateTextureLocal(parent, svtId, limitCount, isOwn, isReal, depth, callbackFunc);

    public static void DownloadAsset(int svtId, int imageLimitCount, System.Action callback)
    {
        AssetManager.downloadAssetStorage(GetAssetName(svtId, imageLimitCount), callback);
    }

    public static void DownloadAsset(int svtId, int limitCount, int lv, System.Action callback)
    {
        AssetManager.downloadAssetStorage(GetAssetName(svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount)), callback);
    }

    public static string[] GetAssetName(int svtId, int imageLimitCount) => 
        UICharaGraphRender.GetAssetNameList(svtId, imageLimitCount);

    public static string[] GetAssetName(int svtId, int limitCount, int lv) => 
        UICharaGraphRender.GetAssetNameList(svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount));

    public static void LoadAsset(int svtId, int imageLimitCount, System.Action callback)
    {
        AssetManager.loadAssetStorage(GetAssetName(svtId, imageLimitCount), callback);
    }

    public static void LoadAsset(int svtId, int limitCount, int lv, System.Action callback)
    {
        AssetManager.loadAssetStorage(GetAssetName(svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount)), callback);
    }

    public static void ReleaseAsset(int svtId, int imageLimitCount)
    {
        AssetManager.releaseAssetStorage(GetAssetName(svtId, imageLimitCount));
    }

    public static void ReleaseAsset(int svtId, int limitCount, int lv)
    {
        AssetManager.releaseAssetStorage(GetAssetName(svtId, ImageLimitCount.GetImageLimitCount(svtId, limitCount)));
    }
}

