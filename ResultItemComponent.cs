using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ResultItemComponent : MonoBehaviour
{
    [SerializeField]
    protected GameObject baseObject;
    [SerializeField]
    protected ItemIconComponent itemIcon;

    public void Clear()
    {
        this.baseObject.SetActive(false);
        this.itemIcon.Clear();
    }

    public void Set(Gift.Type type, int objectId, int count = -1)
    {
        if (objectId > 0)
        {
            this.baseObject.SetActive(true);
            int num = -1;
            if (type.IsItem())
            {
                num = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(objectId).type;
            }
            if (num == 1)
            {
                this.itemIcon.SetGift(type, objectId, count);
            }
            else
            {
                this.itemIcon.SetGift(type, objectId, -1);
            }
        }
        else
        {
            this.Clear();
        }
    }

    public void SetExtra(int imgId)
    {
        if (imgId > 0)
        {
            this.baseObject.SetActive(true);
            this.itemIcon.SetItemImage((ImageItem.Id) imgId);
        }
        else
        {
            this.Clear();
        }
    }
}

