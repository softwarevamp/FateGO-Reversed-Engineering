using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantStatusVoiceListViewManager : ListViewManager
{
    private static readonly int BATTLE_VOICE_MAX_NUM = 0x17;
    protected int callbackCount;
    [SerializeField]
    protected UILabel explanationLabel;
    protected InitMode initMode;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(ServantStatusListViewItem mainInfo)
    {
        string key = "VOICE_EMPTY_MESSAGE";
        base.CreateList(0);
        UserServantCollectionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, mainInfo.SvtId);
        VoiceMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<VoiceMaster>(DataNameKind.Kind.VOICE);
        int[] numArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD).getVoiceLimitCountList(mainInfo.SvtId, entity.maxLimitCount);
        int firstPriority = 0;
        foreach (int num2 in numArray)
        {
            bool[] flagArray;
            string[] strArray;
            VoiceEntity[] entityArray = master.getEnableEntity(out flagArray, out strArray, mainInfo.SvtId, num2);
            for (int i = 0; i < entityArray.Length; i++)
            {
                VoiceEntity voiceEntitiy = entityArray[i];
                bool flag = true;
                if ((num2 != numArray[0]) && (voiceEntitiy.svtVoiceType == 2))
                {
                    if ((voiceEntitiy.condType == 7) && (voiceEntitiy.condValue != 2))
                    {
                        foreach (ListViewItem item in base.itemList)
                        {
                            ServantStatusVoiceListViewItem item2 = item as ServantStatusVoiceListViewItem;
                            if (((item2.PlayType == SvtVoiceType.Type.GROETH) && (item2.CondType == CondType.Kind.SVT_LIMIT)) && (item2.CondValue == voiceEntitiy.condValue))
                            {
                                item2.SetLimitCount(flagArray[i], mainInfo.SvtId, num2, voiceEntitiy, strArray[i], firstPriority);
                                flag = false;
                                break;
                            }
                        }
                    }
                    else if (voiceEntitiy.condType == 0x12)
                    {
                        foreach (ListViewItem item3 in base.itemList)
                        {
                            ServantStatusVoiceListViewItem item4 = item3 as ServantStatusVoiceListViewItem;
                            if ((item4.PlayType == SvtVoiceType.Type.GROETH) && (item4.CondType == CondType.Kind.SVT_COUNT_STOP))
                            {
                                item4.SetLimitCount(flagArray[i], mainInfo.SvtId, num2, voiceEntitiy, strArray[i], firstPriority);
                                flag = false;
                                break;
                            }
                        }
                    }
                }
                if (flag)
                {
                    base.itemList.Add(new ServantStatusVoiceListViewItem(base.itemList.Count, flagArray[i], mainInfo.SvtId, num2, entityArray[i], strArray[i], firstPriority));
                }
            }
            firstPriority++;
            if (mainInfo.SvtId == BalanceConfig.ServantIdJekyll)
            {
                VoiceEntity entity3 = master.getEntityFromId("B050");
                UserServantCollectionEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, mainInfo.SvtId);
                if ((entity3.condType != 0x11) || entity4.IsPlayed(entity3.condValue))
                {
                    bool[] flagArray2;
                    string[] strArray2;
                    VoiceEntity[] entityArray2 = master.getEnableEntity(out flagArray2, out strArray2, BalanceConfig.ServantIdHyde, num2);
                    for (int j = 0; j < entityArray2.Length; j++)
                    {
                        base.itemList.Add(new ServantStatusVoiceListViewItem(base.itemList.Count, flagArray2[j], BalanceConfig.ServantIdHyde, num2, entityArray2[j], strArray2[j], firstPriority));
                    }
                    firstPriority++;
                }
            }
            else if ((mainInfo.SvtId == BalanceConfig.ServantIdMashu1) && TutorialFlag.Get(TutorialFlag.Id.TUTORIAL_LABEL_MASHU_CHANGE))
            {
                bool[] flagArray3;
                string[] strArray3;
                VoiceEntity[] entityArray3 = master.getEnableEntity(out flagArray3, out strArray3, BalanceConfig.ServantIdMashu2, num2);
                for (int k = 0; k < entityArray3.Length; k++)
                {
                    base.itemList.Add(new ServantStatusVoiceListViewItem(base.itemList.Count, flagArray3[k], BalanceConfig.ServantIdMashu2, num2, entityArray3[k], strArray3[k], firstPriority));
                }
                firstPriority++;
            }
        }
        this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_VOICE_EXPLANATION");
        base.emptyMessageLabel.text = LocalizationManager.Get(key);
        base.SortItem(-1, false, 3);
    }

    public void DestroyList()
    {
        base.DestroyList();
        base.sort.Save();
    }

    public ServantStatusVoiceListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as ServantStatusVoiceListViewItem);
        }
        return null;
    }

    protected void OnClickListView(ListViewObject obj)
    {
        Debug.Log("Manager ListView OnClick " + obj.Index);
        ServantStatusVoiceListViewItem item = obj.GetItem() as ServantStatusVoiceListViewItem;
        if (item.IsCanPlay)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(!item.IsPlay ? ResultKind.PLAY : ResultKind.STOP, obj.Index);
            }
        }
    }

    protected void OnMoveEnd()
    {
        if (this.callbackCount > 0)
        {
            this.callbackCount--;
            if (this.callbackCount == 0)
            {
                base.DragMaskEnd();
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

    protected void RequestListObject(ServantStatusVoiceListViewObject.InitMode mode)
    {
        List<ServantStatusVoiceListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantStatusVoiceListViewObject obj2 in objectList)
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

    protected void RequestListObject(ServantStatusVoiceListViewObject.InitMode mode, float delay)
    {
        List<ServantStatusVoiceListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantStatusVoiceListViewObject obj2 in objectList)
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

    public void SetMode(InitMode mode, CallbackFunc callback)
    {
        this.callbackFunc = callback;
        this.SetMode(mode, -1);
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.callbackFunc2 = callback;
        this.SetMode(mode, -1);
    }

    public void SetMode(InitMode mode, int select = -1)
    {
        if (mode != InitMode.PLAY)
        {
            this.initMode = mode;
            this.callbackCount = base.ObjectSum;
            base.IsInput = mode == InitMode.INPUT;
        }
        switch (mode)
        {
            case InitMode.INPUT:
                this.RequestListObject(ServantStatusVoiceListViewObject.InitMode.INPUT);
                break;

            case InitMode.PLAY:
            {
                foreach (ListViewItem item in base.itemList)
                {
                    (item as ServantStatusVoiceListViewItem).SetPLay(item.Index == select);
                }
                List<ServantStatusVoiceListViewObject> objectList = this.ObjectList;
                for (int i = 0; i < objectList.Count; i++)
                {
                    objectList[i].Init(ServantStatusVoiceListViewObject.InitMode.PLAY);
                }
                break;
            }
            case InitMode.MODIFY:
            {
                List<ServantStatusVoiceListViewObject> clippingObjectList = this.ClippingObjectList;
                if (clippingObjectList.Count <= 0)
                {
                    this.callbackCount = 1;
                    base.Invoke("OnMoveEnd", 0.2f);
                    break;
                }
                this.callbackCount = clippingObjectList.Count;
                for (int j = 0; j < clippingObjectList.Count; j++)
                {
                    clippingObjectList[j].Init(ServantStatusVoiceListViewObject.InitMode.MODIFY, new System.Action(this.OnMoveEnd), 0.1f);
                }
                break;
            }
        }
        Debug.Log("SetMode " + mode);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        ServantStatusVoiceListViewObject obj2 = obj as ServantStatusVoiceListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(ServantStatusVoiceListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(ServantStatusVoiceListViewObject.InitMode.VALID);
        }
    }

    public List<ServantStatusVoiceListViewObject> ClippingObjectList
    {
        get
        {
            List<ServantStatusVoiceListViewObject> list = new List<ServantStatusVoiceListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ServantStatusVoiceListViewObject component = obj2.GetComponent<ServantStatusVoiceListViewObject>();
                    ServantStatusVoiceListViewItem item = component.GetItem();
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

    public List<ServantStatusVoiceListViewObject> ObjectList
    {
        get
        {
            List<ServantStatusVoiceListViewObject> list = new List<ServantStatusVoiceListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    ServantStatusVoiceListViewObject component = obj2.GetComponent<ServantStatusVoiceListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(ServantStatusVoiceListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        INPUT,
        PLAY,
        MODIFY
    }

    public enum ResultKind
    {
        NONE,
        PLAY,
        STOP
    }
}

