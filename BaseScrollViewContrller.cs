using System;
using System.Collections.Generic;
using UnityEngine;

public class BaseScrollViewContrller : MonoBehaviour
{
    public UIGrid m_Grid;
    public GameObject m_Item;
    private List<GameObject> m_Items = new List<GameObject>();

    public void Add(params object[] o)
    {
        GameObject item = NGUITools.AddChild(this.m_Grid.gameObject, this.m_Item);
        item.GetComponent<BaseScrollViewItem>().SetItem(o);
        this.m_Grid.Reposition();
        this.m_Items.Add(item);
    }

    public void DestroyItems()
    {
        if (this.m_Items.Count > 0)
        {
            foreach (GameObject obj2 in this.m_Items)
            {
                UnityEngine.Object.Destroy(obj2);
            }
        }
    }
}

