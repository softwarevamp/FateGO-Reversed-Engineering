using System;
using UnityEngine;

public class MasterFigureManagerOld : SingletonMonoBehaviour<MasterFigureManagerOld>
{
    [SerializeField]
    protected GameObject masterFigurePrefab;

    public UIMasterFigureTextureOld CreateLocal(GameObject parent)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.masterFigurePrefab);
        UIMasterFigureTextureOld component = obj2.GetComponent<UIMasterFigureTextureOld>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        obj2.name = "UIMasterFigureOld";
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.SetLayer(parent.gameObject.layer);
        return component;
    }

    public UIMasterFigureTextureOld CreateLocal(GameObject parent, UIMasterFigureRenderOld.DispType dispType, int genderType, int equipId, int depth, System.Action callbackFunc)
    {
        UIMasterFigureTextureOld old = this.CreateLocal(parent);
        object[] objArray1 = new object[] { "UIMasterFigureOld(", genderType, "-", equipId, ")" };
        old.gameObject.name = string.Concat(objArray1);
        old.SetCharacter(dispType, genderType, equipId, callbackFunc);
        old.SetDepth(depth);
        return old;
    }

    public static UIMasterFigureTextureOld CreatePrefab(GameObject parent) => 
        SingletonMonoBehaviour<MasterFigureManagerOld>.Instance.CreateLocal(parent);

    public static UIMasterFigureTextureOld CreatePrefab(GameObject parent, UIMasterFigureRenderOld.DispType dispType, int genderType, int equipId, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<MasterFigureManagerOld>.Instance.CreateLocal(parent, dispType, genderType, equipId, depth, callbackFunc);

    public static void DownloadAsset(int genderType, int equipId, System.Action callback)
    {
        AssetManager.downloadAssetStorage(GetAssetName(genderType, equipId), callback);
    }

    public static string[] GetAssetName(int genderType, int equipId) => 
        UIMasterFigureRenderOld.GetAssetNameList(genderType, equipId);

    public static void LoadAsset(int genderType, int equipId, System.Action callback)
    {
        AssetManager.loadAssetStorage(GetAssetName(genderType, equipId), callback);
    }

    public static void ReleaseAsset(int genderType, int equipId)
    {
        AssetManager.releaseAssetStorage(GetAssetName(genderType, equipId));
    }
}

