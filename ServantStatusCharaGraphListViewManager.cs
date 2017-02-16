using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantStatusCharaGraphListViewManager : ListViewManager
{
    protected ServantStatusCharaGraphListViewObject actionObject;
    protected UIDragDropListViewBackMask backMask;
    protected int callbackCount;
    protected InitMode initMode;
    [SerializeField]
    protected ServantStatusDialog servantStatusDialog;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(ServantStatusListViewItem mainInfo)
    {
        int selectIndex = 0;
        if (mainInfo.UserGame != null)
        {
            bool flag = ImageLimitCount.IsAprilFool && (mainInfo.Servant.IsServant || mainInfo.Servant.IsEnemyCollectionDetail);
            int cardImageLimitCount = mainInfo.CardImageLimitCount;
            int maxCardImageLimitCount = mainInfo.MaxCardImageLimitCount;
            base.CreateList(!flag ? (maxCardImageLimitCount + 1) : (maxCardImageLimitCount + 2));
            for (int i = 0; i <= maxCardImageLimitCount; i++)
            {
                ServantStatusCharaGraphListViewItem item = new ServantStatusCharaGraphListViewItem(i, mainInfo, i);
                if (i == cardImageLimitCount)
                {
                    selectIndex = item.Index;
                }
                base.itemList.Add(item);
            }
            if (flag)
            {
                ServantStatusCharaGraphListViewItem item2 = new ServantStatusCharaGraphListViewItem(maxCardImageLimitCount + 1, mainInfo, BalanceConfig.OtherImageLimitCount);
                if (BalanceConfig.OtherImageLimitCount == cardImageLimitCount)
                {
                    selectIndex = item2.Index;
                }
                base.itemList.Add(item2);
            }
        }
        else
        {
            int imageLimitCount = mainInfo.CardImageLimitCount;
            base.CreateList(1);
            ServantStatusCharaGraphListViewItem item3 = new ServantStatusCharaGraphListViewItem(0, mainInfo, imageLimitCount);
            base.itemList.Add(item3);
        }
        this.backMask = this.GetDragRoot().GetComponent<UIDragDropListViewBackMask>();
        base.SortItem(selectIndex, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public ServantStatusCharaGraphListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ServantStatusCharaGraphListViewItem);
        }
        return null;
    }

    public ServantStatusCharaGraphListViewItem GetSelectItem()
    {
        int pageIndex = (base.indicator as ServantStatusCharaGraphListViewIndicator).GetPageIndex();
        if (pageIndex >= 0)
        {
            return this.GetItem(pageIndex);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(null);
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.backMask.DragStart();
        this.actionObject = obj as ServantStatusCharaGraphListViewObject;
        this.actionObject.Init(ServantStatusCharaGraphListViewObject.InitMode.MAXIM, new System.Action(this.OnEndMaxim), 0.1f);
    }

    protected void OnClickMaxim()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.backMask.DragEnd(new EventDelegate.Callback(this.OnClickMaxim));
        this.backMask.DragStart();
        this.actionObject.Init(ServantStatusCharaGraphListViewObject.InitMode.USUALLY, new System.Action(this.OnEndUsually), 0.1f);
    }

    protected void OnEndMaxim()
    {
        this.backMask.DragStart(new EventDelegate.Callback(this.OnClickMaxim));
    }

    protected void OnEndUsually()
    {
        this.backMask.DragEnd();
        this.RequestListObject(ServantStatusCharaGraphListViewObject.InitMode.INPUT);
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

    protected void RequestListObject(ServantStatusCharaGraphListViewObject.InitMode mode)
    {
        List<ServantStatusCharaGraphListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantStatusCharaGraphListViewObject obj2 in objectList)
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

    protected void RequestListObject(ServantStatusCharaGraphListViewObject.InitMode mode, float delay)
    {
        List<ServantStatusCharaGraphListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantStatusCharaGraphListViewObject obj2 in objectList)
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
                this.RequestListObject(ServantStatusCharaGraphListViewObject.InitMode.VALID);
                break;

            case InitMode.INPUT:
                this.RequestListObject(ServantStatusCharaGraphListViewObject.InitMode.INPUT);
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
        ServantStatusCharaGraphListViewObject obj2 = obj as ServantStatusCharaGraphListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ServantStatusCharaGraphListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ServantStatusCharaGraphListViewObject.InitMode.VALID);
        }
    }

    public void SetVisibleHighPriorityObject(bool isVisible)
    {
        this.servantStatusDialog.SetVisibleHighPriorityObject(isVisible);
    }

    public List<ServantStatusCharaGraphListViewObject> ClippingObjectList
    {
        get
        {
            List<ServantStatusCharaGraphListViewObject> list = new List<ServantStatusCharaGraphListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ServantStatusCharaGraphListViewObject component = obj2.GetComponent<ServantStatusCharaGraphListViewObject>();
                    ServantStatusCharaGraphListViewItem item = component.GetItem();
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

    public List<ServantStatusCharaGraphListViewObject> ObjectList
    {
        get
        {
            List<ServantStatusCharaGraphListViewObject> list = new List<ServantStatusCharaGraphListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ServantStatusCharaGraphListViewObject component = obj2.GetComponent<ServantStatusCharaGraphListViewObject>();
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
        VALID,
        INPUT
    }

    public enum Kind
    {
        NORMAL
    }
}

