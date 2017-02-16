using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaterialCollectionServantListViewManager : ListViewManager
{
    protected int callbackCount;
    [SerializeField]
    protected UISprite completeSprite;
    protected InitMode initMode;
    protected static ListViewSort servantEquipSortInfo = new ListViewSort("MaterialCollectionServant2", ListViewSort.SortKind.ID, true);
    protected static ListViewSort servantSortInfo = new ListViewSort("MaterialCollectionServant1", ListViewSort.SortKind.ID, true);
    protected const string SORT_SAVE_KEY = "MaterialCollectionServant";
    [SerializeField]
    protected MaterialCollectionServantSortSelectMenu sortSelectMenu;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(Kind kind)
    {
        int num;
        int num2;
        UserServantCollectionMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION);
        UserServantCollectionEntity[] entityArray = null;
        Kind kind2 = kind;
        if ((kind2 == Kind.SERVANT) || (kind2 != Kind.SERVANT_EQUIP))
        {
            entityArray = master.getCollectionList(out num, out num2, false);
            base.sort = servantSortInfo;
            base.sort.Load();
            base.sort.SetServantEquip(false);
        }
        else
        {
            entityArray = master.getCollectionList(out num, out num2, true);
            base.sort = servantEquipSortInfo;
            base.sort.Load();
            base.sort.SetServantEquip(true);
        }
        base.CreateList(0);
        for (int i = 0; i < entityArray.Length; i++)
        {
            MaterialCollectionServantListViewItem item = new MaterialCollectionServantListViewItem(i, entityArray[i]);
            base.itemList.Add(item);
        }
        if (this.completeSprite != null)
        {
            this.completeSprite.gameObject.SetActive(num >= entityArray.Length);
        }
        base.emptyMessageLabel.text = LocalizationManager.Get("SERVANT_SORT_FILTER_RESULT_EMPTY");
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    protected void EndSelectSortKind(bool isDecide)
    {
        this.sortSelectMenu.Close();
        if (isDecide)
        {
            base.SortItem(-1, false, -1);
        }
    }

    public MaterialCollectionServantListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as MaterialCollectionServantListViewItem);
        }
        return null;
    }

    public void ModifyItem(int index)
    {
        this.RequestListObject(MaterialCollectionServantListViewObject.InitMode.MODIFY);
    }

    protected void OnClickListView(ListViewObject obj)
    {
    }

    protected void OnClickSelectListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickSelect " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SERVANT_STATUS, obj.Index);
        }
    }

    public void OnClickSortAscendingOrder()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            base.sort.SetAscendingOrder(!base.sort.IsAscendingOrder);
            base.SortItem(-1, false, -1);
        }
    }

    public void OnClickSortKind()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.sortSelectMenu.Open(!base.sort.IsServantEquip ? MaterialCollectionServantSortSelectMenu.Kind.SERVANT : MaterialCollectionServantSortSelectMenu.Kind.SERVANT_EQUIP, base.sort, new MaterialCollectionServantSortSelectMenu.CallbackFunc(this.EndSelectSortKind));
        }
    }

    protected void OnLongPushListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnLongPush " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SERVANT_STATUS, obj.Index);
        }
    }

    protected void OnMoveEnd()
    {
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

    protected void RefrashListDisp()
    {
        List<MaterialCollectionServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].SetInput(base.isInput);
            }
        }
    }

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        List<MaterialCollectionServantListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            MaterialCollectionServantListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(MaterialCollectionServantListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
            }
            else
            {
                this.callbackCount--;
            }
        }
        if (num == 0)
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0f);
        }
    }

    protected void RequestListObject(MaterialCollectionServantListViewObject.InitMode mode)
    {
        List<MaterialCollectionServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (MaterialCollectionServantListViewObject obj2 in objectList)
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

    protected void RequestListObject(MaterialCollectionServantListViewObject.InitMode mode, float delay)
    {
        List<MaterialCollectionServantListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (MaterialCollectionServantListViewObject obj2 in objectList)
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
        switch (mode)
        {
            case InitMode.VALID:
                this.RequestListObject(MaterialCollectionServantListViewObject.InitMode.VALID);
                break;

            case InitMode.INPUT:
                this.RequestListObject(MaterialCollectionServantListViewObject.InitMode.INPUT);
                break;
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
        MaterialCollectionServantListViewObject obj2 = obj as MaterialCollectionServantListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(MaterialCollectionServantListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(MaterialCollectionServantListViewObject.InitMode.VALID);
        }
    }

    public List<MaterialCollectionServantListViewObject> ClippingObjectList
    {
        get
        {
            List<MaterialCollectionServantListViewObject> list = new List<MaterialCollectionServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    MaterialCollectionServantListViewObject component = obj2.GetComponent<MaterialCollectionServantListViewObject>();
                    MaterialCollectionServantListViewItem item = component.GetItem();
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

    public List<MaterialCollectionServantListViewObject> ObjectList
    {
        get
        {
            List<MaterialCollectionServantListViewObject> list = new List<MaterialCollectionServantListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    MaterialCollectionServantListViewObject component = obj2.GetComponent<MaterialCollectionServantListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(MaterialCollectionServantListViewManager.ResultKind kind, int index);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT
    }

    public enum Kind
    {
        SERVANT,
        SERVANT_EQUIP
    }

    public enum ResultKind
    {
        NONE,
        SERVANT_STATUS
    }
}

