using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventPointItemListViewManager : ListViewManager
{
    protected int callbackCount;
    protected InitMode initMode;
    [SerializeField]
    protected PlayMakerFSM targetFSM;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void closeItemDetail(bool isDecide)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        SingletonMonoBehaviour<CommonUI>.Instance.CloseItemDetailDialog();
    }

    private void closeSvtDetail(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(null);
    }

    public void CreateList(EventRewardEntity[] rewardList, int currentPoint)
    {
        base.CreateList(0);
        for (int i = 0; i < rewardList.Length; i++)
        {
            EventRewardEntity rewardData = rewardList[i];
            bool isGet = currentPoint >= rewardData.point;
            EventPointItemListViewItem item = new EventPointItemListViewItem(rewardData, isGet);
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public EventPointItemListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as EventPointItemListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        EventPointItemListViewItem item = (obj as EventPointItemListViewObject).GetItem();
        switch (item.eventRewardType)
        {
            case RewardType.Type.GIFT:
                if (item.Type == Gift.Type.ITEM)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenItemDetailDialog(item.ItemEntity, new ItemDetailInfoComponent.CallbackFunc(this.closeItemDetail));
                }
                if (item.Type.IsServant())
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.DROP, item.SvtEntity.id, new ServantStatusDialog.ClickDelegate(this.closeSvtDetail));
                }
                break;

            case RewardType.Type.EXTRA:
            case RewardType.Type.SET:
                SingletonMonoBehaviour<CommonUI>.Instance.OpenItemDetailDialog(item.NameText, item.RewardDetailTXt, new ItemDetailInfoComponent.CallbackFunc(this.closeItemDetail));
                break;
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

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        base.DragMaskStart();
        List<EventPointItemListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            EventPointItemListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(EventPointItemListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
            }
            else
            {
                this.callbackCount--;
            }
        }
        if (num == 0)
        {
            this.callbackCount = 1;
            base.Invoke("OnMoveEnd", 0.6f);
        }
    }

    protected void RequestListObject(EventPointItemListViewObject.InitMode mode)
    {
        List<EventPointItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (EventPointItemListViewObject obj2 in objectList)
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

    protected void RequestListObject(EventPointItemListViewObject.InitMode mode, float delay)
    {
        List<EventPointItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (EventPointItemListViewObject obj2 in objectList)
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

    public void setNextRewardInfo()
    {
        int count = base.itemList.Count;
        int index = 0;
        for (int i = 0; i < count; i++)
        {
            EventPointItemListViewItem item = base.itemList[i] as EventPointItemListViewItem;
            if (!item.IsGetReward)
            {
                index = i;
                break;
            }
        }
        base.SetTopItem(index);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        EventPointItemListViewObject obj2 = obj as EventPointItemListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(EventPointItemListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(EventPointItemListViewObject.InitMode.VALID);
        }
    }

    public List<EventPointItemListViewObject> ClippingObjectList
    {
        get
        {
            List<EventPointItemListViewObject> list = new List<EventPointItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    EventPointItemListViewObject component = obj2.GetComponent<EventPointItemListViewObject>();
                    EventPointItemListViewItem item = component.GetItem();
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

    public List<EventPointItemListViewObject> ObjectList
    {
        get
        {
            List<EventPointItemListViewObject> list = new List<EventPointItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    EventPointItemListViewObject component = obj2.GetComponent<EventPointItemListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc();

    public enum InitMode
    {
        NONE,
        INTO,
        INPUT,
        ENTER,
        EXIT,
        MODIFY
    }

    public enum Kind
    {
        NORMAL
    }
}

