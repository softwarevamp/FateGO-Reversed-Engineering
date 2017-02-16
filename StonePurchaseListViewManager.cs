using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class StonePurchaseListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;
    [SerializeField]
    protected PlayMakerFSM targetFSM;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind)
    {
        int index = 0;
        if (kind == Kind.BANK)
        {
            BankShopEntity[] enableEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BankShopMaster>(DataNameKind.Kind.BANK_SHOP).GetEnableEntitiyList();
            string[] productList = SingletonMonoBehaviour<AccountingManager>.Instance.GetProductList();
            base.CreateList(0);
            for (int i = 0; i < enableEntitiyList.Length; i++)
            {
                bool flag;
                if (productList != null)
                {
                    flag = false;
                    for (int j = 0; j < productList.Length; j++)
                    {
                        if (productList[j].Equals(enableEntitiyList[i].googleShopId))
                        {
                            flag = true;
                            break;
                        }
                    }
                }
                else
                {
                    flag = true;
                }
                if (flag)
                {
                    StonePurchaseListViewItem item = new StonePurchaseListViewItem(index, enableEntitiyList[i]);
                    UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getUserIdEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
                    if ((item.BankShop.id != 1) || (int.Parse(entity.getPey()[item.BankShop.id - 1]) < item.BankShop.firstPayId))
                    {
                        base.itemList.Add(item);
                        index++;
                    }
                }
            }
        }
        base.scrollView.contentPivot = (base.itemList.Count <= 2) ? UIWidget.Pivot.Center : UIWidget.Pivot.Top;
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public StonePurchaseListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as StonePurchaseListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(obj.Index);
        }
    }

    protected void OnMoveEnd()
    {
        Debug.Log("OnMoveEnd " + this.callbackCount);
        if (this.callbackCount > 0)
        {
            this.callbackCount--;
            if (this.callbackCount == 0)
            {
                if (base.scrollView != null)
                {
                    base.scrollView.UpdateScrollbars(true);
                }
                System.Action action = this.callbackFunc2;
                this.callbackFunc2 = null;
                if (action != null)
                {
                    action();
                }
            }
        }
    }

    protected void RequestListObject(StonePurchaseListViewObject.InitMode mode)
    {
        List<StonePurchaseListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (StonePurchaseListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd));
            }
        }
        else
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0f);
        }
    }

    protected void RequestListObject(StonePurchaseListViewObject.InitMode mode, float delay)
    {
        List<StonePurchaseListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (StonePurchaseListViewObject obj2 in objectList)
            {
                obj2.Init(mode, new System.Action(this.OnMoveEnd), delay);
            }
        }
        else
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", delay);
        }
    }

    public void SetMode(InitMode mode)
    {
        this.initMode = mode;
        this.callbackCount = base.ObjectSum;
        base.IsInput = mode == InitMode.INPUT;
        if (mode == InitMode.INPUT)
        {
            this.RequestListObject(StonePurchaseListViewObject.InitMode.INPUT);
        }
        Debug.Log("SetMode " + mode);
    }

    public void SetMode(InitMode mode, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.SetMode(mode);
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.callbackFunc2 = callback;
        this.SetMode(mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        StonePurchaseListViewObject obj2 = obj as StonePurchaseListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(StonePurchaseListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(StonePurchaseListViewObject.InitMode.VALID);
        }
    }

    public List<StonePurchaseListViewObject> ClippingObjectList
    {
        get
        {
            List<StonePurchaseListViewObject> list = new List<StonePurchaseListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    StonePurchaseListViewObject component = obj2.GetComponent<StonePurchaseListViewObject>();
                    StonePurchaseListViewItem item = component.GetItem();
                    if (item.IsTermination)
                    {
                        if (base.ClippingItem(item))
                        {
                            list.Add(component);
                        }
                    }
                    else
                    {
                        list.Add(component);
                    }
                }
            }
            return list;
        }
    }

    public List<StonePurchaseListViewObject> ObjectList
    {
        get
        {
            List<StonePurchaseListViewObject> list = new List<StonePurchaseListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    StonePurchaseListViewObject component = obj2.GetComponent<StonePurchaseListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(int result);

    public enum InitMode
    {
        NONE,
        INPUT
    }

    public enum Kind
    {
        BANK
    }
}

