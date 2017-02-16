using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class BaseMonoBehaviour : MonoBehaviour
{
    public GameObject createObject(string resouceurl, Transform root = null, Transform pos = null)
    {
        Vector3 zero = Vector3.zero;
        Vector3 up = Vector3.up;
        if (resouceurl == null)
        {
            return null;
        }
        UnityEngine.Object original = Resources.Load(resouceurl);
        if (original == null)
        {
            Debug.Log(" No Resouce:" + resouceurl);
            return null;
        }
        GameObject obj3 = UnityEngine.Object.Instantiate(original) as GameObject;
        obj3.transform.parent = root;
        obj3.transform.localPosition = zero;
        obj3.transform.eulerAngles = up;
        obj3.transform.localScale = new Vector3(1f, 1f, 1f);
        if (pos != null)
        {
            obj3.transform.position = pos.position;
        }
        return obj3;
    }

    public GameObject createObject(GameObject prefab, Transform root = null, Transform pos = null)
    {
        Vector3 zero = Vector3.zero;
        Vector3 up = Vector3.up;
        if (prefab == null)
        {
            Debug.Log(" prefab No Resouce:");
            return null;
        }
        GameObject obj2 = (GameObject) UnityEngine.Object.Instantiate(prefab, zero, Quaternion.Euler(up));
        obj2.transform.parent = root;
        obj2.transform.localPosition = zero;
        obj2.transform.eulerAngles = up;
        obj2.transform.localScale = new Vector3(1f, 1f, 1f);
        if (pos != null)
        {
            obj2.transform.position = pos.position;
        }
        return obj2;
    }
}

