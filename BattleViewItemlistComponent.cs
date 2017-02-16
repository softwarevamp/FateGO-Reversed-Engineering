using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleViewItemlistComponent : BaseMonoBehaviour
{
    private int colmax = 5;
    public float height = 120f;
    private List<GameObject> itemObjectList = new List<GameObject>();
    public Transform listRoot;
    public string noItemLabelKey = "BATTLE_ITEMLIST_NOTGET";
    public UILabel notgetLabel;
    public GameObject prefabResultItem;
    public float width = 125f;

    public void setHide()
    {
        if (this.listRoot != null)
        {
            this.listRoot.gameObject.SetActive(false);
        }
    }

    public void setListData(BattleDropItem[] itemlist, BattleDropItemComponent.ClickDelegate callBack, int baseDepth = 0)
    {
        foreach (GameObject obj2 in this.itemObjectList)
        {
            UnityEngine.Object.Destroy(obj2);
        }
        this.itemObjectList.Clear();
        if (itemlist != null)
        {
            int num = itemlist.Length / this.colmax;
            Debug.Log("itemlist.Length:" + itemlist.Length);
            for (int i = 0; i < itemlist.Length; i++)
            {
                BattleDropItem indata = itemlist[i];
                Vector3 vector = new Vector3();
                int num3 = i / this.colmax;
                vector.y = (num3 * this.height) * -1f;
                if (num3 != num)
                {
                    int num4 = i % this.colmax;
                    vector.x = num4 * this.width;
                }
                else
                {
                    int num5 = i % this.colmax;
                    vector.x = num5 * this.width;
                }
                GameObject item = base.createObject(this.prefabResultItem, this.listRoot, null);
                item.transform.localPosition = vector;
                if (item != null)
                {
                    BattleDropItemComponent component = item.GetComponent<BattleDropItemComponent>();
                    component.Set(indata);
                    component.SetCallBack(callBack);
                    component.Show();
                }
                this.itemObjectList.Add(item);
            }
            if (this.notgetLabel != null)
            {
                this.notgetLabel.text = LocalizationManager.Get(this.noItemLabelKey);
                this.notgetLabel.gameObject.SetActive(itemlist.Length <= 0);
            }
        }
        else
        {
            this.notgetLabel.text = LocalizationManager.Get(this.noItemLabelKey);
            this.notgetLabel.gameObject.SetActive(true);
        }
    }

    public void setShow()
    {
        if (this.listRoot != null)
        {
            this.listRoot.gameObject.SetActive(true);
        }
    }
}

