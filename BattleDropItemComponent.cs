using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleDropItemComponent : MonoBehaviour
{
    protected ClickDelegate callbackFunc;
    private Gift.Type gifttype;
    public ItemIconComponent item;
    private BattleDropItem itemData;
    public GameObject newflag;
    public ServantFaceIconComponent servant;

    public void OnClick()
    {
        if (this.gifttype.IsServant())
        {
            if (0L < this.itemData.userSvtId)
            {
                this.callbackFunc(this.itemData);
            }
        }
        else if ((this.gifttype == Gift.Type.ITEM) && (0 < this.itemData.objectId))
        {
            this.callbackFunc(this.itemData);
        }
    }

    public void Set(BattleDropItem indata)
    {
        this.itemData = indata;
        this.gifttype = (Gift.Type) this.itemData.type;
        if (this.gifttype.IsServant())
        {
            this.SetServant();
        }
        else if (this.gifttype == Gift.Type.ITEM)
        {
            this.SetItem();
        }
    }

    public void SetCallBack(ClickDelegate call)
    {
        this.callbackFunc = call;
    }

    public void SetDepth(int basedepth)
    {
    }

    public void SetItem()
    {
        this.servant.Clear();
        this.servant.gameObject.SetActive(false);
        if (this.itemData.num <= 1)
        {
            this.item.SetItem(this.itemData.objectId, -1);
        }
        else
        {
            this.item.SetItem(this.itemData.objectId, this.itemData.num);
        }
        this.item.gameObject.SetActive(true);
    }

    public void SetServant()
    {
        this.item.Clear();
        this.item.gameObject.SetActive(false);
        if (0L < this.itemData.userSvtId)
        {
            this.servant.Set(this.itemData.userSvtId, null);
        }
        else
        {
            Debug.Log("svtId:" + this.itemData.objectId);
            Debug.Log("limitCount:" + this.itemData.limitCount);
            this.servant.Set(this.itemData.objectId, this.itemData.limitCount, 0, 0, null, CollectionStatus.Kind.GET, false, false);
        }
        this.servant.gameObject.SetActive(true);
    }

    public void SetTouch(bool flg)
    {
        Collider component = base.gameObject.GetComponent<Collider>();
        if (component != null)
        {
            component.enabled = flg;
        }
    }

    public void Show()
    {
        base.gameObject.SetActive(true);
    }

    public delegate void ClickDelegate(BattleDropItem item);
}

