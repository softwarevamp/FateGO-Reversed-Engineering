using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ProgramEffectManager : SingletonMonoBehaviour<ProgramEffectManager>
{
    [SerializeField]
    protected GameObject[] charaSpecialEffectList;
    [SerializeField]
    protected GameObject[] mainSpecialEffectList;

    public static GameObject Create(GameObject parent, GameObject prefab, Vector3 pos, float time, Color color, bool isSkip = false)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
        ProgramEffectComponent component = obj2.GetComponent<ProgramEffectComponent>();
        Transform transform = obj2.transform;
        Vector3 localScale = obj2.transform.localScale;
        transform.parent = parent.transform;
        transform.localPosition = pos;
        transform.localRotation = Quaternion.identity;
        transform.localScale = localScale;
        component.Init(time, color, isSkip);
        return obj2;
    }

    public static GameObject CreateCharaEffect(GameObject parent, string effectName, Vector3 pos, float time, Color color, bool isSkip = false)
    {
        GameObject charaEffectPrefab = GetCharaEffectPrefab(effectName);
        if (charaEffectPrefab != null)
        {
            return Create(parent, charaEffectPrefab, pos, time, color, isSkip);
        }
        return null;
    }

    public static GameObject CreateMainEffect(GameObject parent, string effectName, Vector3 pos, float time, Color color, bool isSkip = false)
    {
        GameObject mainEffectPrefab = GetMainEffectPrefab(effectName);
        if (mainEffectPrefab != null)
        {
            return Create(parent, mainEffectPrefab, pos, time, color, isSkip);
        }
        return null;
    }

    public static void Destory(GameObject parent)
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            Transform child = parent.transform.GetChild(i);
            if (child.GetComponent<ProgramEffectComponent>() != null)
            {
                list.Add(child.gameObject);
            }
        }
        foreach (GameObject obj2 in list)
        {
            UnityEngine.Object.Destroy(obj2);
        }
    }

    public static void Destory(GameObject parent, string effectName)
    {
        if (effectName == null)
        {
            CommonEffectManager.Stop(parent, false, false);
        }
        else
        {
            List<GameObject> list = new List<GameObject>();
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                Transform child = parent.transform.GetChild(i);
                ProgramEffectComponent component = child.GetComponent<ProgramEffectComponent>();
                if ((component != null) && effectName.Equals(component.EffectName))
                {
                    list.Add(child.gameObject);
                }
            }
            foreach (GameObject obj2 in list)
            {
                UnityEngine.Object.Destroy(obj2);
            }
        }
    }

    public static ProgramEffectComponent[] Get(GameObject parent)
    {
        List<ProgramEffectComponent> list = new List<ProgramEffectComponent>();
        if (parent != null)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                ProgramEffectComponent item = parent.transform.GetChild(i).GetComponent<ProgramEffectComponent>();
                if (item != null)
                {
                    list.Add(item);
                }
            }
        }
        return list.ToArray();
    }

    public static GameObject GetCharaEffectPrefab(string effectName)
    {
        if (SingletonMonoBehaviour<ProgramEffectManager>.Instance != null)
        {
            foreach (GameObject obj2 in SingletonMonoBehaviour<ProgramEffectManager>.Instance.charaSpecialEffectList)
            {
                ProgramEffectComponent component = obj2.GetComponent<ProgramEffectComponent>();
                if (effectName.Equals(component.EffectName))
                {
                    return obj2;
                }
            }
            Debug.LogError("chara program effect name error : [" + effectName + "]");
        }
        return null;
    }

    public static GameObject GetMainEffectPrefab(string effectName)
    {
        if (SingletonMonoBehaviour<ProgramEffectManager>.Instance != null)
        {
            foreach (GameObject obj2 in SingletonMonoBehaviour<ProgramEffectManager>.Instance.mainSpecialEffectList)
            {
                ProgramEffectComponent component = obj2.GetComponent<ProgramEffectComponent>();
                if (effectName.Equals(component.EffectName))
                {
                    return obj2;
                }
            }
            Debug.LogError("main program effect name error : [" + effectName + "]");
        }
        return null;
    }

    public static bool IsBusy(GameObject parent)
    {
        if (parent != null)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                if (parent.transform.GetChild(i).GetComponent<ProgramEffectComponent>() != null)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool IsBusy(GameObject parent, string effectName)
    {
        if (parent != null)
        {
            if (effectName == null)
            {
                return IsBusy(parent);
            }
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                ProgramEffectComponent component = parent.transform.GetChild(i).GetComponent<ProgramEffectComponent>();
                if ((component != null) && effectName.Equals(component.EffectName))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static void Stop(GameObject parent)
    {
        if (parent != null)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                ProgramEffectComponent component = parent.transform.GetChild(i).GetComponent<ProgramEffectComponent>();
                if (component != null)
                {
                    component.Stop();
                }
            }
        }
    }

    public static void Stop(GameObject parent, string effectName)
    {
        if (effectName == null)
        {
            CommonEffectManager.Stop(parent, false, false);
        }
        else
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                ProgramEffectComponent component = parent.transform.GetChild(i).GetComponent<ProgramEffectComponent>();
                if ((component != null) && effectName.Equals(component.EffectName))
                {
                    component.Stop();
                }
            }
        }
    }
}

