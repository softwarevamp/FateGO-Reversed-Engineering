using System;
using UnityEngine;

public class MasterFullFigureManager : SingletonMonoBehaviour<MasterFullFigureManager>
{
    [SerializeField]
    protected GameObject masterFigurePrefab;

    public UIMasterFullFigureTexture CreateLocal(GameObject parent)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.masterFigurePrefab);
        UIMasterFullFigureTexture component = obj2.GetComponent<UIMasterFullFigureTexture>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        obj2.name = "UIMasterFullFigure";
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.SetLayer(parent.gameObject.layer);
        return component;
    }

    public UIMasterFullFigureTexture CreateLocal(GameObject parent, UIMasterFullFigureRender.DispType dispType, int genderType, int equipId, int depth, System.Action callbackFunc)
    {
        UIMasterFullFigureTexture texture = this.CreateLocal(parent);
        object[] objArray1 = new object[] { "UIMasterFullFigure(", genderType, "-", equipId, ")" };
        texture.gameObject.name = string.Concat(objArray1);
        texture.SetCharacter(dispType, genderType, equipId, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public static UIMasterFullFigureTexture CreatePrefab(GameObject parent) => 
        SingletonMonoBehaviour<MasterFullFigureManager>.Instance.CreateLocal(parent);

    public static UIMasterFullFigureTexture CreatePrefab(GameObject parent, UIMasterFullFigureRender.DispType dispType, int genderType, int equipId, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<MasterFullFigureManager>.Instance.CreateLocal(parent, dispType, genderType, equipId, depth, callbackFunc);

    public static void DownloadAsset(int genderType, int equipId, System.Action callback)
    {
        AssetManager.downloadAssetStorage(GetAssetName(genderType, equipId), callback);
    }

    public static string[] GetAssetName(int genderType, int equipId) => 
        UIMasterFullFigureRender.GetAssetNameList(genderType, equipId);

    public static void LoadAsset(int genderType, int equipId, System.Action callback)
    {
        AssetManager.loadAssetStorage(GetAssetName(genderType, equipId), callback);
    }

    public static void ReleaseAsset(int genderType, int equipId)
    {
        AssetManager.releaseAssetStorage(GetAssetName(genderType, equipId));
    }
}

