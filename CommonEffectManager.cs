using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class CommonEffectManager : SingletonMonoBehaviour<CommonEffectManager>
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$mapD;
    [SerializeField]
    protected GameObject communicationCharaEffectPrefab;
    protected static string effectAssetStoragePath = "Effect/";
    [SerializeField]
    protected GameObject loadEffectPrefab;
    protected static GameObject testBackEffectPrefab;
    protected static GameObject testCharaBackEffectPrefab;
    protected static GameObject testCharaEffectPrefab;
    protected static GameObject testEffectPrefab;

    public static void Create(GameObject parent, string effectName)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, Vector3.zero, null, null, false);
    }

    public static CommonEffectComponent Create(GameObject parent, AssetData data, GameObject prefab) => 
        Create(parent, data, prefab, Vector3.zero, false);

    public static void Create(GameObject parent, string effectName, CommonEffectLoadComponent.LoadEndHandler callback)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, Vector3.zero, null, callback, false);
    }

    public static void Create(GameObject parent, string effectName, bool isSkip)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, Vector3.zero, null, null, isSkip);
    }

    public static CommonEffectComponent Create(GameObject parent, string effectName, GameObject prefab) => 
        Create(parent, effectName, prefab, Vector3.zero, false);

    public static void Create(GameObject parent, string effectName, Vector3 pos)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, pos, null, null, false);
    }

    public static CommonEffectComponent Create(GameObject parent, AssetData data, GameObject prefab, bool isSkip) => 
        Create(parent, data, prefab, Vector3.zero, isSkip);

    public static CommonEffectComponent Create(GameObject parent, AssetData data, GameObject prefab, Vector3 pos) => 
        Create(parent, data, prefab, pos, false);

    public static void Create(GameObject parent, string effectName, CommonEffectLoadComponent.LoadEndHandler callback, bool isSkip)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, Vector3.zero, null, callback, isSkip);
    }

    public static CommonEffectComponent Create(GameObject parent, string effectName, GameObject prefab, bool isSkip) => 
        Create(parent, effectName, prefab, Vector3.zero, isSkip);

    public static CommonEffectComponent Create(GameObject parent, string effectName, GameObject prefab, Vector3 pos) => 
        Create(parent, effectName, prefab, Vector3.zero, false);

    public static void Create(GameObject parent, string effectName, Vector3 pos, CommonEffectLoadComponent.LoadEndHandler callback)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, pos, null, callback, false);
    }

    public static void Create(GameObject parent, string effectName, Vector3 pos, bool isSkip)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, pos, null, null, isSkip);
    }

    public static CommonEffectComponent Create(GameObject parent, AssetData data, GameObject prefab, Vector3 pos, bool isSkip)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
        CommonEffectComponent component = obj2.GetComponent<CommonEffectComponent>();
        Transform transform = obj2.transform;
        Vector3 localScale = prefab.transform.localScale;
        transform.parent = parent.transform;
        transform.localPosition = pos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.Init(data, false, false);
        return component;
    }

    public static CommonEffectComponent Create(GameObject parent, string effectName, GameObject prefab, Vector3 pos, bool isSkip)
    {
        effectName = GetAssetName(effectName);
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
        CommonEffectComponent component = obj2.GetComponent<CommonEffectComponent>();
        Transform transform = obj2.transform;
        Vector3 localScale = prefab.transform.localScale;
        transform.parent = parent.transform;
        transform.localPosition = pos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.Init(effectName, false, false);
        return component;
    }

    public static void Create(GameObject parent, string effectName, Vector3 pos, CommonEffectLoadComponent.LoadEndHandler callback, bool isSkip)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, pos, null, callback, isSkip);
    }

    public static void CreateParam(GameObject parent, string effectName, object param)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, Vector3.zero, param, null, false);
    }

    public static void CreateParam(GameObject parent, string effectName, object param, CommonEffectLoadComponent.LoadEndHandler callback)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, Vector3.zero, param, callback, false);
    }

    public static void CreateParam(GameObject parent, string effectName, object param, bool isSkip)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, Vector3.zero, param, null, isSkip);
    }

    public static void CreateParam(GameObject parent, string effectName, Vector3 pos, object param)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, pos, param, null, false);
    }

    public static void CreateParam(GameObject parent, string effectName, object param, CommonEffectLoadComponent.LoadEndHandler callback, bool isSkip)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, Vector3.zero, param, callback, isSkip);
    }

    public static void CreateParam(GameObject parent, string effectName, Vector3 pos, object param, CommonEffectLoadComponent.LoadEndHandler callback)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, pos, param, callback, false);
    }

    public static void CreateParam(GameObject parent, string effectName, Vector3 pos, object param, bool isSkip)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, pos, param, null, isSkip);
    }

    public static void CreateParam(GameObject parent, string effectName, Vector3 pos, object param, CommonEffectLoadComponent.LoadEndHandler callback, bool isSkip)
    {
        SingletonMonoBehaviour<CommonEffectManager>.Instance.LoadCreateLocal(parent, effectName, pos, param, callback, isSkip);
    }

    public static void Destroy(GameObject parent)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child.GetComponent<CommonEffectLoadComponent>() != null)
            {
                list.Add(child.gameObject);
            }
            else if (child.GetComponent<CommonEffectComponent>() != null)
            {
                list.Add(child.gameObject);
            }
        }
        foreach (GameObject obj2 in list)
        {
            UnityEngine.Object.Destroy(obj2);
        }
    }

    public static void Destroy(GameObject parent, string effectName)
    {
        if (effectName == null)
        {
            Stop(parent, false, false);
        }
        else
        {
            effectName = GetAssetName(effectName);
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                CommonEffectLoadComponent component = child.GetComponent<CommonEffectLoadComponent>();
                if ((component != null) && effectName.Equals(component.EffectName))
                {
                    list.Add(child.gameObject);
                }
                else
                {
                    CommonEffectComponent component2 = child.GetComponent<CommonEffectComponent>();
                    if ((component2 != null) && effectName.Equals(component2.EffectName))
                    {
                        list.Add(child.gameObject);
                    }
                }
            }
            foreach (GameObject obj2 in list)
            {
                UnityEngine.Object.Destroy(obj2);
            }
        }
    }

    public static CommonEffectComponent[] Get(GameObject parent)
    {
        List<CommonEffectComponent> list = new List<CommonEffectComponent>();
        if (parent != null)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                CommonEffectComponent item = parent.transform.GetChild(i).GetComponent<CommonEffectComponent>();
                if (item != null)
                {
                    list.Add(item);
                }
            }
        }
        return list.ToArray();
    }

    public static string GetAssetName(string effectName) => 
        (effectAssetStoragePath + effectName);

    public static bool IsBusy(GameObject parent)
    {
        if (parent != null)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (child.GetComponent<CommonEffectLoadComponent>() != null)
                {
                    return true;
                }
                if (child.GetComponent<CommonEffectComponent>() != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsBusy(GameObject parent, string effectName)
    {
        if (effectName == null)
        {
            return IsBusy(parent);
        }
        effectName = GetAssetName(effectName);
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            CommonEffectLoadComponent component = child.GetComponent<CommonEffectLoadComponent>();
            if ((component != null) && effectName.Equals(component.EffectName))
            {
                return true;
            }
            CommonEffectComponent component2 = child.GetComponent<CommonEffectComponent>();
            if ((component2 != null) && effectName.Equals(component2.EffectName))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsStart(GameObject parent)
    {
        if (parent != null)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                if (child.GetComponent<CommonEffectLoadComponent>() != null)
                {
                    return false;
                }
                CommonEffectComponent component = child.GetComponent<CommonEffectComponent>();
                if ((component != null) && !component.IsStart)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public static bool IsStart(GameObject parent, string effectName)
    {
        if (parent != null)
        {
            if (effectName == null)
            {
                return IsStart(parent);
            }
            effectName = GetAssetName(effectName);
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                CommonEffectLoadComponent component = child.GetComponent<CommonEffectLoadComponent>();
                if ((component != null) && effectName.Equals(component.EffectName))
                {
                    return false;
                }
                CommonEffectComponent component2 = child.GetComponent<CommonEffectComponent>();
                if (((component2 != null) && effectName.Equals(component2.EffectName)) && !component2.IsStart)
                {
                    return false;
                }
            }
        }
        return true;
    }

    protected void LoadCreateLocal(GameObject parent, string effectName, Vector3 pos, object param, CommonEffectLoadComponent.LoadEndHandler callback, bool isSkip)
    {
        if (!effectName.StartsWith("Talk/Test"))
        {
            if (effectName == "Talk/communicationCharaEffect")
            {
                effectName = "Talk/bit_talk_10";
            }
            goto Label_0139;
        }
        bool flag = true;
        GameObject prefab = null;
        string key = effectName;
        if (key != null)
        {
            int num;
            if (<>f__switch$mapD == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(4) {
                    { 
                        "Talk/TestEffect",
                        0
                    },
                    { 
                        "Talk/TestBackEffect",
                        1
                    },
                    { 
                        "Talk/TestCharaEffect",
                        2
                    },
                    { 
                        "Talk/TestCharaBackEffect",
                        3
                    }
                };
                <>f__switch$mapD = dictionary;
            }
            if (<>f__switch$mapD.TryGetValue(key, out num))
            {
                switch (num)
                {
                    case 0:
                        prefab = testEffectPrefab;
                        goto Label_00CD;

                    case 1:
                        prefab = testBackEffectPrefab;
                        goto Label_00CD;

                    case 2:
                        prefab = testCharaEffectPrefab;
                        goto Label_00CD;

                    case 3:
                        prefab = testCharaBackEffectPrefab;
                        goto Label_00CD;
                }
            }
        }
        flag = false;
    Label_00CD:
        if (flag)
        {
            CommonEffectComponent effect = null;
            if (prefab != null)
            {
                effect = Create(parent, prefab.name, prefab, pos, isSkip);
            }
            if ((effect != null) && (param != null))
            {
                effect.SetParam(param);
            }
            if (callback != null)
            {
                callback(effect);
            }
            return;
        }
    Label_0139:
        effectName = GetAssetName(effectName);
        GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.loadEffectPrefab);
        CommonEffectLoadComponent component2 = obj3.GetComponent<CommonEffectLoadComponent>();
        Transform transform = obj3.transform;
        transform.parent = parent.transform;
        transform.localPosition = pos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        component2.Init(effectName, param, callback, isSkip);
    }

    public static void Resume(GameObject parent, bool isSkip = false)
    {
        if (parent != null)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                CommonEffectLoadComponent component = child.GetComponent<CommonEffectLoadComponent>();
                if (component != null)
                {
                    component.Resume(isSkip);
                }
                else
                {
                    CommonEffectComponent component2 = child.GetComponent<CommonEffectComponent>();
                    if (component2 != null)
                    {
                        component2.Resume(isSkip);
                    }
                }
            }
        }
    }

    public static void Resume(GameObject parent, string effectName, bool isSkip = false)
    {
        if (parent != null)
        {
            if (effectName == null)
            {
                Stop(parent, isSkip, false);
            }
            else
            {
                effectName = GetAssetName(effectName);
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    Transform child = parent.transform.GetChild(i);
                    CommonEffectLoadComponent component = child.GetComponent<CommonEffectLoadComponent>();
                    if ((component != null) && effectName.Equals(component.EffectName))
                    {
                        component.Resume(isSkip);
                    }
                    else
                    {
                        CommonEffectComponent component2 = child.GetComponent<CommonEffectComponent>();
                        if ((component2 != null) && effectName.Equals(component2.EffectName))
                        {
                            component2.Resume(isSkip);
                        }
                    }
                }
            }
        }
    }

    public static void SetTestEffectPrefab(GameObject effectPrefab, GameObject backEffectPrefab, GameObject charaEffectPrefab, GameObject charaBackEffectPrefab)
    {
        testEffectPrefab = effectPrefab;
        testBackEffectPrefab = backEffectPrefab;
        testCharaEffectPrefab = charaEffectPrefab;
        testCharaBackEffectPrefab = charaBackEffectPrefab;
    }

    public static bool Stop(GameObject parent, bool isSkip = false, bool isLoadStop = false)
    {
        if (parent == null)
        {
            return true;
        }
        bool flag = true;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (isSkip || flag)
            {
                CommonEffectLoadComponent component = child.GetComponent<CommonEffectLoadComponent>();
                if (component != null)
                {
                    if (isSkip || isLoadStop)
                    {
                        component.Stop();
                    }
                    else
                    {
                        flag = false;
                    }
                    continue;
                }
            }
            CommonEffectComponent component2 = child.GetComponent<CommonEffectComponent>();
            if (component2 != null)
            {
                if (isSkip)
                {
                    UnityEngine.Object.Destroy(component2.gameObject);
                }
                else
                {
                    component2.Stop(true);
                }
            }
        }
        return flag;
    }

    public static bool Stop(GameObject parent, string effectName, bool isSkip = false, bool isLoadStop = false)
    {
        if (effectName == null)
        {
            return Stop(parent, isSkip, isLoadStop);
        }
        effectName = GetAssetName(effectName);
        bool flag = true;
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (isSkip || flag)
            {
                CommonEffectLoadComponent component = child.GetComponent<CommonEffectLoadComponent>();
                if ((component != null) && effectName.Equals(component.EffectName))
                {
                    if (isSkip || isLoadStop)
                    {
                        component.Stop();
                    }
                    else
                    {
                        flag = false;
                    }
                    continue;
                }
            }
            CommonEffectComponent component2 = child.GetComponent<CommonEffectComponent>();
            if ((component2 != null) && effectName.Equals(component2.EffectName))
            {
                if (isSkip)
                {
                    UnityEngine.Object.Destroy(component2.gameObject);
                }
                else
                {
                    component2.Stop(true);
                }
            }
        }
        return flag;
    }
}

