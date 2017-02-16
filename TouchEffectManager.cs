using System;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffectManager : SingletonMonoBehaviour<TouchEffectManager>
{
    private static int blockCount;
    private static float dragCount = -1f;
    [SerializeField]
    protected GameObject dragPrefab;
    [SerializeField]
    protected float dragUnitLong = 100f;
    [SerializeField]
    protected Camera effectCamera;
    [SerializeField]
    protected GameObject effectParent;
    [SerializeField]
    protected GameObject tapPrefab;

    public void CreateLocal(GameObject prefab, Vector2 p)
    {
        if ((prefab != null) && (blockCount <= 0))
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
            CommonEffectComponent component = obj2.GetComponent<CommonEffectComponent>();
            Transform transform = obj2.transform;
            Vector3 localScale = prefab.transform.localScale;
            obj2.name = "TouchEffect";
            transform.parent = this.effectParent.transform;
            transform.position = this.effectCamera.ScreenToWorldPoint((Vector3) p);
            transform.localRotation = Quaternion.identity;
            transform.localScale = localScale;
            component.Init(false, false);
        }
    }

    public static void Drag(Vector2 v)
    {
        if (((SingletonMonoBehaviour<TouchEffectManager>.getInstance() != null) && (SingletonMonoBehaviour<TouchEffectManager>.Instance.dragUnitLong > 0f)) && (dragCount >= 0f))
        {
            dragCount += Vector2.Distance(Vector2.zero, v);
            if (dragCount > SingletonMonoBehaviour<TouchEffectManager>.Instance.dragUnitLong)
            {
                SingletonMonoBehaviour<TouchEffectManager>.Instance.CreateLocal(SingletonMonoBehaviour<TouchEffectManager>.Instance.dragPrefab, UICamera.lastTouchPosition);
                dragCount = 0f;
            }
        }
    }

    public static void Press(Vector2 p)
    {
        if (SingletonMonoBehaviour<TouchEffectManager>.getInstance() != null)
        {
            SingletonMonoBehaviour<TouchEffectManager>.Instance.CreateLocal(SingletonMonoBehaviour<TouchEffectManager>.Instance.tapPrefab, p);
            dragCount = 0f;
        }
    }

    public static void SetBlock(bool isBlock)
    {
        if (SingletonMonoBehaviour<TouchEffectManager>.getInstance() != null)
        {
            if (isBlock)
            {
                if (blockCount <= 0)
                {
                    blockCount = 1;
                    SingletonMonoBehaviour<TouchEffectManager>.Instance.StopLocal();
                }
                else
                {
                    blockCount++;
                }
            }
            else if (blockCount > 0)
            {
                blockCount--;
            }
        }
    }

    public void StopLocal()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < this.effectParent.transform.childCount; i++)
        {
            Transform child = this.effectParent.transform.GetChild(i);
            if (child.GetComponent<CommonEffectComponent>() != null)
            {
                list.Add(child.gameObject);
            }
        }
        foreach (GameObject obj2 in list)
        {
            UnityEngine.Object.Destroy(obj2);
        }
    }

    public static void UnPress()
    {
        dragCount = -1f;
    }
}

