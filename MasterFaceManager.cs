using System;
using UnityEngine;

public class MasterFaceManager : SingletonMonoBehaviour<MasterFaceManager>
{
    [SerializeField]
    protected GameObject masterFacePrefab;

    public UIMasterFaceTexture CreateLocal(GameObject parent)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.masterFacePrefab);
        UIMasterFaceTexture component = obj2.GetComponent<UIMasterFaceTexture>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        obj2.name = "UIMasterFace";
        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.SetLayer(parent.gameObject.layer);
        return component;
    }

    public UIMasterFaceTexture CreateLocal(GameObject parent, UIMasterFaceRender.DispType dispType, int genderType, int equipId, int depth, System.Action callbackFunc)
    {
        UIMasterFaceTexture texture = this.CreateLocal(parent);
        object[] objArray1 = new object[] { "UIMasterFace(", genderType, "-", equipId, ")" };
        texture.gameObject.name = string.Concat(objArray1);
        texture.SetCharacter(dispType, genderType, equipId, callbackFunc);
        texture.SetDepth(depth);
        return texture;
    }

    public static UIMasterFaceTexture CreatePrefab(GameObject parent) => 
        SingletonMonoBehaviour<MasterFaceManager>.Instance.CreateLocal(parent);

    public static UIMasterFaceTexture CreatePrefab(GameObject parent, UIMasterFaceRender.DispType dispType, int genderType, int equipId, int depth, System.Action callbackFunc) => 
        SingletonMonoBehaviour<MasterFaceManager>.Instance.CreateLocal(parent, dispType, genderType, equipId, depth, callbackFunc);

    public static void DownloadAsset(UIMasterFaceRender.DispType dispType, int genderType, int equipId, System.Action callback)
    {
        AssetManager.downloadAssetStorage(GetAssetName(dispType, genderType, equipId), callback);
    }

    public static string[] GetAssetName(UIMasterFaceRender.DispType dispType, int genderType, int equipId) => 
        UIMasterFaceRender.GetAssetNameList(dispType, genderType, equipId);

    public static void LoadAsset(UIMasterFaceRender.DispType dispType, int genderType, int equipId, System.Action callback)
    {
        AssetManager.loadAssetStorage(GetAssetName(dispType, genderType, equipId), callback);
    }

    public static void ReleaseAsset(UIMasterFaceRender.DispType dispType, int genderType, int equipId)
    {
        AssetManager.releaseAssetStorage(GetAssetName(dispType, genderType, equipId));
    }
}

