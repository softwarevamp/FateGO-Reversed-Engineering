using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class PartyListViewManager : ListViewManager
{
    protected int callbackCount;
    [SerializeField]
    protected GameObject explanationBase;
    [SerializeField]
    protected UILabel explanationLabel;
    protected InitMode initMode;
    [SerializeField]
    protected UILabel maxCostLabel;
    protected PartyListViewItem.MenuKind menuKind;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(PartyListViewItem.MenuKind kind, UserDeckEntity[] userDeckEntityList, int partyNum, FollowerInfo followerInfo = null, int followerClassId = 0, EventUpValSetupInfo setupInfo = null)
    {
        this.menuKind = kind;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        this.maxCostLabel.text = string.Empty + entity.costMax;
        PartyListViewItem.MenuKind kind2 = kind;
        base.CreateList(0);
        for (int i = 0; i < userDeckEntityList.Length; i++)
        {
            if (userDeckEntityList[i] != null)
            {
                PartyListViewItem item = new PartyListViewItem(this.menuKind, i, entity.mainDeckId, entity.costMax, userDeckEntityList[i], followerInfo, followerClassId, setupInfo);
                base.itemList.Add(item);
            }
        }
        PartyListViewIndicator indicator = base.indicator as PartyListViewIndicator;
        indicator.SetKind(kind);
        indicator.SetIndexMax(BalanceConfig.DeckMax);
        indicator.SetEventId(setupInfo);
        base.SortItem(partyNum, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public PartyListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as PartyListViewItem);
        }
        return null;
    }

    public void MoveCenterItem(int index, bool isAnimation = true)
    {
        if (this.callbackCount <= 0)
        {
            base.MoveCenterItem(index, isAnimation);
        }
    }

    protected void OnClickListViewChangeEquip1(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeEquip1 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_EQUIP, obj.Index, 0);
        }
    }

    protected void OnClickListViewChangeEquip2(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeEquip2 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_EQUIP, obj.Index, 1);
        }
    }

    protected void OnClickListViewChangeEquip3(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeEquip3 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_EQUIP, obj.Index, 2);
        }
    }

    protected void OnClickListViewChangeEquip4(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeEquip4 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_EQUIP, obj.Index, 3);
        }
    }

    protected void OnClickListViewChangeEquip5(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeEquip5 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_EQUIP, obj.Index, 4);
        }
    }

    protected void OnClickListViewChangeEquip6(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeEquip6 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_EQUIP, obj.Index, 5);
        }
    }

    protected void OnClickListViewChangeParty(PartyListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeParty " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.CHANGE_PARTY, obj.Index, 0);
        }
    }

    protected void OnClickListViewChangeServant1(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeServant1 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_SERVANT, obj.Index, 0);
        }
    }

    protected void OnClickListViewChangeServant2(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeServant2 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_SERVANT, obj.Index, 1);
        }
    }

    protected void OnClickListViewChangeServant3(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeServant3 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_SERVANT, obj.Index, 2);
        }
    }

    protected void OnClickListViewChangeServant4(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeServant4 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_SERVANT, obj.Index, 3);
        }
    }

    protected void OnClickListViewChangeServant5(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeServant5 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_SERVANT, obj.Index, 4);
        }
    }

    protected void OnClickListViewChangeServant6(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClickChangeServant6 " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.SELECT_SERVANT, obj.Index, 5);
        }
    }

    protected void OnModifyListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnModifyListView " + obj.Index);
        CallbackFunc callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc(ResultKind.MODIFY_STATUS, obj.Index, 0);
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

    protected void RequestListObject(PartyListViewObject.InitMode mode)
    {
        List<PartyListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PartyListViewObject obj2 in objectList)
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

    protected void RequestListObject(PartyListViewObject.InitMode mode, float delay)
    {
        List<PartyListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PartyListViewObject obj2 in objectList)
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
                this.RequestListObject(PartyListViewObject.InitMode.VALID);
                break;

            case InitMode.INPUT:
                this.RequestListObject(PartyListViewObject.InitMode.INPUT);
                break;

            case InitMode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT:
                this.RequestListObject(PartyListViewObject.InitMode.ORGANIZATION_GUIDE_DECK_EMPTY_SELECT);
                break;
        }
        this.explanationBase.SetActive(true);
        this.explanationLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_PARTY_SELECT_EXPLANATION");
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
        PartyListViewObject obj2 = obj as PartyListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(PartyListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(PartyListViewObject.InitMode.VALID);
        }
    }

    public List<PartyListViewObject> ClippingObjectList
    {
        get
        {
            List<PartyListViewObject> list = new List<PartyListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PartyListViewObject component = obj2.GetComponent<PartyListViewObject>();
                    PartyListViewItem item = component.GetItem();
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

    public List<PartyListViewObject> ObjectList
    {
        get
        {
            List<PartyListViewObject> list = new List<PartyListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PartyListViewObject component = obj2.GetComponent<PartyListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(PartyListViewManager.ResultKind kind, int n, int m);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        ORGANIZATION_GUIDE_DECK_EMPTY_SELECT
    }

    public enum ResultKind
    {
        NONE,
        SELECT_SERVANT,
        SELECT_EQUIP,
        CHANGE_PARTY,
        MODIFY_STATUS
    }
}

