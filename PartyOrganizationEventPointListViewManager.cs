using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PartyOrganizationEventPointListViewManager : ListViewManager
{
    protected int callbackCount;
    protected EventMemberMargeUpValInfo[] eventMargeUpValInfoList;
    protected InitMode initMode;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(PartyListViewItem partyItem)
    {
        EventUpValInfo[] infoArray;
        base.CreateList(0);
        if (partyItem.GetEventUpVal(out infoArray))
        {
            int num = 0;
            Vector3 localPosition = base.seed.GetLocalPosition(0);
            EventDropItemUpValInfo[][] infoArray2 = new EventDropItemUpValInfo[infoArray.Length][];
            this.eventMargeUpValInfoList = new EventMemberMargeUpValInfo[infoArray.Length];
            for (int i = 0; i < infoArray.Length; i++)
            {
                if (infoArray[i] != null)
                {
                    infoArray2[i] = infoArray[i].GetDropItemList(i);
                }
            }
            for (int j = 0; j < infoArray.Length; j++)
            {
                EventMemberMargeUpValInfo info;
                PartyOrganizationListViewItem member = partyItem.GetMember(j);
                if (member.IsFollower)
                {
                    if (member.FollowerData != null)
                    {
                        goto Label_00AF;
                    }
                    continue;
                }
                if (member.UserServant == null)
                {
                    continue;
                }
            Label_00AF:
                info = new EventMemberMargeUpValInfo(j, member.SvtNameText, member.IsFollower);
                this.eventMargeUpValInfoList[j] = info;
                for (int k = 0; k < infoArray.Length; k++)
                {
                    if (infoArray2[k] != null)
                    {
                        this.eventMargeUpValInfoList[j].Add(infoArray2[k]);
                    }
                }
                if (!info.IsEmpry())
                {
                    PartyOrganizationEventPointListViewItem item = new PartyOrganizationEventPointListViewItem(num++, info.servantName, member.IsFollower) {
                        BasePosition = localPosition
                    };
                    localPosition.y += base.seed.arrangementPich.y;
                    base.itemList.Add(item);
                    int count = info.GetCount();
                    for (int m = 0; m < count; m++)
                    {
                        item = new PartyOrganizationEventPointListViewItem(num++, info.GetMargeItem(m)) {
                            BasePosition = localPosition
                        };
                        localPosition.y += base.seed.arrangementPich.y;
                        base.itemList.Add(item);
                    }
                    localPosition.y -= 8f;
                }
            }
        }
        base.emptyMessageLabel.text = LocalizationManager.Get("PARTY_ORGANIZATION_EVENT_POINT_MESSAGE_EMPTY");
        base.DispItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
        this.eventMargeUpValInfoList = null;
    }

    public PartyOrganizationEventPointListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as PartyOrganizationEventPointListViewItem);
        }
        return null;
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

    protected void RequestListObject(PartyOrganizationEventPointListViewObject.InitMode mode)
    {
        List<PartyOrganizationEventPointListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PartyOrganizationEventPointListViewObject obj2 in objectList)
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

    protected void RequestListObject(PartyOrganizationEventPointListViewObject.InitMode mode, float delay)
    {
        List<PartyOrganizationEventPointListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (PartyOrganizationEventPointListViewObject obj2 in objectList)
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
                this.RequestListObject(PartyOrganizationEventPointListViewObject.InitMode.VALID);
                break;

            case InitMode.INPUT:
                this.RequestListObject(PartyOrganizationEventPointListViewObject.InitMode.INPUT);
                break;

            case InitMode.ORGANIZATION_GUIDE_FIRST_SELECT:
            {
                List<PartyOrganizationEventPointListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0f);
                    break;
                }
                this.callbackCount = clippingObjectList.Count;
                for (int i = 0; i < clippingObjectList.Count; i++)
                {
                    PartyOrganizationEventPointListViewObject obj2 = clippingObjectList[i];
                    if (i < (clippingObjectList.Count - 1))
                    {
                        obj2.Init(PartyOrganizationEventPointListViewObject.InitMode.VALID, new System.Action(this.OnMoveEnd));
                    }
                    else
                    {
                        obj2.Init(PartyOrganizationEventPointListViewObject.InitMode.TUTORIAL_INPUT, new System.Action(this.OnMoveEnd));
                    }
                }
                break;
            }
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
        PartyOrganizationEventPointListViewObject obj2 = obj as PartyOrganizationEventPointListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(PartyOrganizationEventPointListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(PartyOrganizationEventPointListViewObject.InitMode.VALID);
        }
    }

    public List<PartyOrganizationEventPointListViewObject> ClippingObjectList
    {
        get
        {
            List<PartyOrganizationEventPointListViewObject> list = new List<PartyOrganizationEventPointListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PartyOrganizationEventPointListViewObject component = obj2.GetComponent<PartyOrganizationEventPointListViewObject>();
                    PartyOrganizationEventPointListViewItem item = component.GetItem();
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

    public List<PartyOrganizationEventPointListViewObject> ObjectList
    {
        get
        {
            List<PartyOrganizationEventPointListViewObject> list = new List<PartyOrganizationEventPointListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    PartyOrganizationEventPointListViewObject component = obj2.GetComponent<PartyOrganizationEventPointListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(PartyOrganizationEventPointListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        ORGANIZATION_GUIDE_FIRST_SELECT
    }

    public enum ResultKind
    {
        NONE,
        CANCEL,
        DECIDE
    }
}

