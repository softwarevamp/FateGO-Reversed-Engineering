using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BoxGachaItemListViewManager : ListViewManager
{
    private int allCurrentNum;
    private int allMaxNum;
    [SerializeField]
    protected GameObject baseView;
    protected int callbackCount;
    private int currentBaseId;
    private int currentBoxGachaId;
    private int currentResetNum;
    protected InitMode initMode;
    private bool isReset;
    [SerializeField]
    protected BoxCollider resetInfoCol;
    [SerializeField]
    protected GameObject resetInfoObj;
    [SerializeField]
    protected PlayMakerFSM targetFSM;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void ClickResetGachaBtn()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        long[] args = new long[] { NetworkManager.UserId, (long) this.currentBoxGachaId };
        UserBoxGachaEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_BOX_GACHA).getEntityFromId<UserBoxGachaEntity>(args);
        string message = string.Format(LocalizationManager.Get("BOX_GACHA_RESET_MSG"), entity.resetNum + 1);
        SingletonMonoBehaviour<CommonUI>.Instance.OpenConfirmDecideDlg(LocalizationManager.Get("BOX_GACHA_RESET_TITLE"), message, LocalizationManager.Get("BOX_GACHA_EXE_TXT"), LocalizationManager.Get("COMMON_CONFIRM_NO"), new CommonConfirmDialog.ClickDelegate(this.closeEventSvtConfirmDlg));
    }

    private void closeEventSvtConfirmDlg(bool isDecide)
    {
        if (isDecide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(() => this.targetFSM.SendEvent("EXE_RESETGACHA"));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
        }
    }

    public void closeItemDetail(bool isDecide)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        SingletonMonoBehaviour<CommonUI>.Instance.CloseItemDetailDialog();
    }

    private void closeSvtDetail(bool isDecide)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(null);
    }

    public void CreateList(BoxGachaBaseEntity[] baseData, int eventId, int boxGachaId, int baseId, int[] resIds, bool isReset = false)
    {
        this.resetInfoObj.SetActive(false);
        this.baseView.transform.localPosition = new Vector3(0f, -10f, 0f);
        base.scrollView.GetComponent<UIPanel>().SetRect(0f, 0f, 540f, 340f);
        this.isReset = isReset;
        this.allMaxNum = 0;
        this.allCurrentNum = 0;
        this.currentBoxGachaId = boxGachaId;
        this.currentBaseId = baseId;
        base.CreateList(0);
        for (int i = 0; i < baseData.Length; i++)
        {
            bool isDraw = false;
            BoxGachaBaseEntity data = baseData[i];
            if ((resIds != null) && (resIds.Length > 0))
            {
                for (int j = 0; j < resIds.Length; j++)
                {
                    if (data.no == resIds[j])
                    {
                        isDraw = true;
                    }
                }
            }
            BoxGachaItemListViewItem item = new BoxGachaItemListViewItem(data, eventId, boxGachaId, isDraw);
            base.itemList.Add(item);
        }
        this.RefrashListDisp();
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public int GetGachaItemCurrentNum() => 
        this.allCurrentNum;

    public int GetGachaItemMaxNum() => 
        this.allMaxNum;

    public BoxGachaItemListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as BoxGachaItemListViewItem);
        }
        return null;
    }

    public void itemColliderCtr(bool isDisp)
    {
        if (this.resetInfoObj.activeSelf)
        {
            this.resetInfoCol.enabled = isDisp;
        }
        if (base.itemList != null)
        {
            foreach (ListViewItem item in base.itemList)
            {
                BoxGachaItemListViewItem item2 = item as BoxGachaItemListViewItem;
                if (item2.ViewObject != null)
                {
                    item2.ViewObject.GetComponent<Collider>().enabled = isDisp;
                }
            }
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        BoxGachaItemListViewItem item = (obj as BoxGachaItemListViewObject).GetItem();
        switch (item.GachaBaseType)
        {
            case RewardType.Type.GIFT:
                if (item.GiftType == Gift.Type.ITEM)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenItemDetailDialog(item.ItemEntity, new ItemDetailInfoComponent.CallbackFunc(this.closeItemDetail));
                }
                if (((item.GiftType == Gift.Type.SERVANT) || (item.GiftType == Gift.Type.EVENT_SVT_JOIN)) || (item.GiftType == Gift.Type.EVENT_SVT_GET))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.DROP, item.SvtEntity.id, new ServantStatusDialog.ClickDelegate(this.closeSvtDetail));
                }
                break;

            case RewardType.Type.EXTRA:
            case RewardType.Type.SET:
                SingletonMonoBehaviour<CommonUI>.Instance.OpenItemDetailDialog(item.NameTxt, item.ExtraDetailTXt, new ItemDetailInfoComponent.CallbackFunc(this.closeItemDetail));
                break;
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
        if (this.isReset)
        {
            this.resetInfoObj.SetActive(true);
            this.baseView.transform.localPosition = new Vector3(0f, -80f, 0f);
            base.scrollView.GetComponent<UIPanel>().SetRect(0f, 35f, 540f, 270f);
        }
        foreach (ListViewItem item in base.itemList)
        {
            BoxGachaItemListViewItem item2 = item as BoxGachaItemListViewItem;
            this.allMaxNum += item2.MaxNum;
            this.allCurrentNum += item2.CurrentNum;
        }
    }

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        base.DragMaskStart();
        List<BoxGachaItemListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            BoxGachaItemListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(BoxGachaItemListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
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

    protected void RequestListObject(BoxGachaItemListViewObject.InitMode mode)
    {
        List<BoxGachaItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (BoxGachaItemListViewObject obj2 in objectList)
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

    protected void RequestListObject(BoxGachaItemListViewObject.InitMode mode, float delay)
    {
        List<BoxGachaItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (BoxGachaItemListViewObject obj2 in objectList)
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

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        BoxGachaItemListViewObject obj2 = obj as BoxGachaItemListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(BoxGachaItemListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(BoxGachaItemListViewObject.InitMode.VALID);
        }
    }

    public List<BoxGachaItemListViewObject> ClippingObjectList
    {
        get
        {
            List<BoxGachaItemListViewObject> list = new List<BoxGachaItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    BoxGachaItemListViewObject component = obj2.GetComponent<BoxGachaItemListViewObject>();
                    BoxGachaItemListViewItem item = component.GetItem();
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

    public List<BoxGachaItemListViewObject> ObjectList
    {
        get
        {
            List<BoxGachaItemListViewObject> list = new List<BoxGachaItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    BoxGachaItemListViewObject component = obj2.GetComponent<BoxGachaItemListViewObject>();
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
}

