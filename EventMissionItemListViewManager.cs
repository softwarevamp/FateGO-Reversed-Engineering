using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EventMissionItemListViewManager : ListViewManager
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache19;
    [CompilerGenerated]
    private static System.Action <>f__am$cache1A;
    [CompilerGenerated]
    private static System.Action <>f__am$cache1B;
    [CompilerGenerated]
    private static System.Action <>f__am$cache1C;
    protected System.Action actionCallback;
    protected ListViewSort baseSortInfo;
    protected int callbackCount;
    protected static AssetData effectAssetData;
    protected float endEffectTime = 1.5f;
    public EventGachaRootComponent eventRootComponent;
    [SerializeField]
    protected UICommonButton filterBtn;
    [SerializeField]
    protected UISprite filterBtnTxt;
    protected FilterStatus filterStatus;
    private GetSvts[] getSvtList;
    protected InitMode initMode;
    public Transform instantiatingPOS;
    protected bool isEndEvent;
    private const int messageDlgDepth = 150;
    private EventMissionEntity missionToRecieve;
    protected float openItemTime = 0.5f;
    protected EventMissionItemListViewItem openTargetItem;
    protected ListViewSort operationSortInfo;
    private GameObject servantRewardActionObject;
    public GameObject servantRewardActionPrefab;
    private System.Action ShowMSG;
    private ServantRewardAction svtRewardComp;
    [SerializeField]
    protected PlayMakerFSM targetFSM;
    protected int targetMissionId;
    [SerializeField]
    protected GameObject touchBlocker;

    protected event System.Action callbackFunc2;

    public void AcceptReward()
    {
        string name;
        GiftEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GiftMaster>(DataNameKind.Kind.GIFT).getDataById(this.missionToRecieve.giftId);
        if (this.missionToRecieve.rewardType == 1)
        {
            if (Gift.IsServant(entity.type))
            {
                ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity.objectId);
                name = entity2.name;
                if (SvtType.IsCombineMaterial((SvtType.Type) entity2.type) || SvtType.IsStatusUp((SvtType.Type) entity2.type))
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, string.Format(LocalizationManager.Get("MISSION_ACTION_SUCCESS_SEND_PRESENT_BOX"), name), delegate {
                        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
                        this.targetFSM.SendEvent("END_EFFECT");
                    }, 150);
                }
                else if ((this.getSvtList != null) && (this.getSvtList.Length > 0))
                {
                    this.startSvtGetEffect(this.getSvtList[0].userSvtId, this.getSvtList[0].isNew);
                }
                else
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, string.Format(LocalizationManager.Get("MISSION_ACTION_SUCCESS"), name), delegate {
                        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
                        this.targetFSM.SendEvent("END_EFFECT");
                    }, 150);
                }
            }
            else
            {
                name = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).getEntityFromId(entity.objectId).name + string.Format(LocalizationManager.Get("MISSION_ACTION_SUCCESS_MULTIPLE"), entity.num.ToString("#,0"));
                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, string.Format(LocalizationManager.Get("MISSION_ACTION_SUCCESS"), name), delegate {
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
                    this.targetFSM.SendEvent("END_EFFECT");
                }, 150);
            }
        }
        else
        {
            name = this.missionToRecieve.getSetRewardData().detail;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, string.Format(LocalizationManager.Get("MISSION_ACTION_SUCCESS"), name), delegate {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
                this.targetFSM.SendEvent("END_EFFECT");
            }, 150);
        }
    }

    private void actionAfterCallback()
    {
        if (this.actionCallback != null)
        {
            System.Action actionCallback = this.actionCallback;
            this.actionCallback = null;
            actionCallback();
        }
    }

    public void checkAcceptable()
    {
        GiftEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<GiftMaster>(DataNameKind.Kind.GIFT).getDataById(this.missionToRecieve.giftId);
        if (this.missionToRecieve.rewardType == 1)
        {
            if (Gift.IsServant(entity.type))
            {
                ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(entity.objectId);
                if (SvtType.IsCombineMaterial((SvtType.Type) entity2.type) || SvtType.IsStatusUp((SvtType.Type) entity2.type))
                {
                    UserPresentBoxEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserPresentBoxMaster>(DataNameKind.Kind.USER_PRESENT_BOX).getVaildList(NetworkManager.UserId);
                    ItemEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).getEntityFromId(entity.objectId);
                    int length = entityArray.Length;
                    if (length >= BalanceConfig.PresentBoxMax)
                    {
                        if (<>f__am$cache19 == null)
                        {
                            <>f__am$cache19 = delegate {
                                if (<>f__am$cache1B == null)
                                {
                                    <>f__am$cache1B = () => SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
                                }
                                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, LocalizationManager.Get("MISSION_ACTION_PRESENT_BOX_FULL_WARNING"), <>f__am$cache1B, 150);
                            };
                        }
                        this.ShowMSG = <>f__am$cache19;
                        this.targetFSM.SendEvent("SHOW_MSG");
                    }
                    else if ((length + entity.num) > BalanceConfig.PresentBoxMax)
                    {
                        if (<>f__am$cache1A == null)
                        {
                            <>f__am$cache1A = delegate {
                                if (<>f__am$cache1C == null)
                                {
                                    <>f__am$cache1C = () => SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
                                }
                                SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, LocalizationManager.Get("MISSION_ACTION_PRESENT_BOX_OVER_WARNING"), <>f__am$cache1C, 150);
                            };
                        }
                        this.ShowMSG = <>f__am$cache1A;
                        this.targetFSM.SendEvent("SHOW_MSG");
                    }
                    else
                    {
                        this.targetFSM.SendEvent("REWARD_ACCEPTABLE");
                    }
                }
                else
                {
                    this.targetFSM.SendEvent("REWARD_ACCEPTABLE");
                }
            }
            else
            {
                this.targetFSM.SendEvent("REWARD_ACCEPTABLE");
            }
        }
        else
        {
            this.targetFSM.SendEvent("REWARD_ACCEPTABLE");
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

    public void CreateList(EventMissionEntity[] missionList, int eventId)
    {
        base.CreateList(0);
        EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(eventId);
        long num = NetworkManager.getTime();
        this.isEndEvent = (num > entity.endedAt) && (num <= entity.finishedAt);
        if (!this.isEndEvent)
        {
            UserEventMissionEntity[] usrMissionList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMissionMaster>(DataNameKind.Kind.USER_EVENT_MISSION).getUserEventMissionList(eventId);
            for (int i = 0; i < missionList.Length; i++)
            {
                EventMissionEntity missionData = missionList[i];
                EventMissionItemListViewItem item = new EventMissionItemListViewItem(missionData, usrMissionList);
                base.itemList.Add(item);
            }
        }
        else if (UserMissionProgressManager.ReadData())
        {
            List<UserMissionProgressInfo> missionProgInfoList = UserMissionProgressManager.GetMissionProgInfoList();
            if ((missionProgInfoList != null) && (missionProgInfoList.Count > 0))
            {
                for (int j = 0; j < missionProgInfoList.Count; j++)
                {
                    UserMissionProgressInfo userMissionInfo = missionProgInfoList[j];
                    EventMissionItemListViewItem item2 = new EventMissionItemListViewItem(userMissionInfo);
                    base.itemList.Add(item2);
                }
            }
        }
        if (base.sort != null)
        {
            this.baseSortInfo = base.sort;
        }
        this.setFilterId(EventRewardSaveData.FilterId);
        base.emptyMessageLabel.text = LocalizationManager.Get("MISSION_EMPTY_TXT");
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    private void endloadEffect(AssetData data)
    {
        if (data != null)
        {
            effectAssetData = data;
            if (!MissionActionManager.checkScroll(this.missionToRecieve.id, MissionProgressType.Type.ACHIEVE))
            {
                this.RefrashListDisp();
            }
            else if (this.filterStatus != FilterStatus.INIT)
            {
                this.filterStatus = FilterStatus.INIT;
                this.setList();
            }
            SingletonMonoBehaviour<AutomatedAction>.Instance.SetMissionAction(this.missionToRecieve.id, MissionProgressType.Type.ACHIEVE);
        }
    }

    public static GameObject getEffect(string name, Transform parentTr)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(effectAssetData.GetObject<GameObject>(name));
        obj2.transform.parent = parentTr;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        return obj2;
    }

    public EventMissionItemListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as EventMissionItemListViewItem);
        }
        return null;
    }

    private void loadOpenMissionEffect()
    {
        string name = "Effect/EventMission";
        AssetManager.loadAssetStorage(name, new AssetLoader.LoadEndDataHandler(this.endloadEffect));
    }

    private void missionRewardCallback(string result)
    {
        if (result.Equals("ng"))
        {
            this.targetFSM.SendEvent("REQUEST_NG");
        }
        else if (!result.Equals("ng"))
        {
            resData[] dataArray = JsonManager.DeserializeArray<resData>("[" + result + "]");
            this.getSvtList = dataArray[0].getSvts;
            EventRewardSaveData.MissionId = this.missionToRecieve.id;
            this.targetFSM.SendEvent("REQUEST_OK");
        }
        else
        {
            this.targetFSM.SendEvent("REQUEST_NG");
        }
    }

    public void ModifyItem()
    {
        if (this.isEndEvent && (base.itemList != null))
        {
            long[] args = new long[] { NetworkManager.UserId, (long) this.missionToRecieve.id };
            UserEventMissionEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EVENT_MISSION).getEntityFromId<UserEventMissionEntity>(args);
            foreach (ListViewItem item in base.itemList)
            {
                EventMissionItemListViewItem item2 = item as EventMissionItemListViewItem;
                if (item2.MissionId == this.missionToRecieve.id)
                {
                    EventRewardSaveData.MissionId = this.missionToRecieve.id;
                    item2.ModifyItem(entity.missionProgressType == 5);
                }
                if (item2.ViewObject != null)
                {
                    item2.ViewObject.SetItem(item2);
                }
            }
        }
    }

    private void modifyOpenItem()
    {
        if (this.openTargetItem != null)
        {
            foreach (ListViewItem item in base.itemList)
            {
                EventMissionItemListViewItem item2 = item as EventMissionItemListViewItem;
                if (item2.MissionId == this.openTargetItem.MissionId)
                {
                    item2.SetOpenMissionItem(true);
                }
                if (item2.ViewObject != null)
                {
                    item2.ViewObject.SetItem(item2);
                }
            }
        }
        this.SetMode(InitMode.INPUT, delegate {
            this.RefrashListDisp();
            base.Invoke("actionAfterCallback", this.endEffectTime);
        });
    }

    public void OnClickFilterList()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        switch (this.filterStatus)
        {
            case FilterStatus.INIT:
                this.filterStatus = FilterStatus.CLEAR;
                break;

            case FilterStatus.CLEAR:
                this.filterStatus = FilterStatus.PROGRESS;
                break;

            case FilterStatus.PROGRESS:
                this.filterStatus = FilterStatus.NOTOPEN;
                break;

            case FilterStatus.NOTOPEN:
                this.filterStatus = FilterStatus.ACHIVE;
                break;

            case FilterStatus.ACHIVE:
                this.filterStatus = FilterStatus.INIT;
                break;
        }
        this.setList();
    }

    protected void OnClickListView(ListViewObject obj)
    {
        EventMissionItemListViewItem selectItem = (obj as EventMissionItemListViewObject).GetItem();
        if (selectItem.IsShowRewardInfo)
        {
            this.showRewardDetailInfo(selectItem);
        }
        else if (selectItem.CurrentStatus == EventMissionItemListViewItem.ProgStatus.CLEAR)
        {
            this.recieveReward(selectItem.EventMissionEntity);
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
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

    private void recieveReward(EventMissionEntity missionEntity)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.missionToRecieve = missionEntity;
        this.targetFSM.SendEvent("MISSIONN_REWARD");
    }

    protected void RefrashListDisp()
    {
        List<EventMissionItemListViewObject> objectList = this.ObjectList;
        foreach (ListViewItem item in base.itemList)
        {
            (item as EventMissionItemListViewItem).checkMissionCond();
        }
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
        base.DragMaskStart();
        List<EventMissionItemListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            EventMissionItemListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(EventMissionItemListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
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

    protected void RequestListObject(EventMissionItemListViewObject.InitMode mode)
    {
        List<EventMissionItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (EventMissionItemListViewObject obj2 in objectList)
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

    protected void RequestListObject(EventMissionItemListViewObject.InitMode mode, float delay)
    {
        List<EventMissionItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (EventMissionItemListViewObject obj2 in objectList)
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

    public void requestMissionReward()
    {
        NetworkManager.getRequest<EventMissionClearRewardRequest>(new NetworkManager.ResultCallbackFunc(this.missionRewardCallback)).beginRequest(this.missionToRecieve.id);
    }

    public void setAfterAction()
    {
        if (!this.isEndEvent)
        {
            this.loadOpenMissionEffect();
        }
    }

    private void setFilterId(int id)
    {
        this.filterStatus = (FilterStatus) id;
        this.setList();
    }

    private void setFilterName(string targetFile)
    {
        this.filterBtnTxt.spriteName = targetFile;
        this.filterBtnTxt.MakePixelPerfect();
    }

    private void setList()
    {
        this.operationSortInfo = new ListViewSort(this.baseSortInfo);
        switch (this.filterStatus)
        {
            case FilterStatus.INIT:
                this.setFilterName("btn_txt_all");
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_CLEAR, true);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_PROGRESS, true);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_NOSTART, true);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_COMPLETE, true);
                break;

            case FilterStatus.CLEAR:
                this.setFilterName("btn_txt_receipt");
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_CLEAR, true);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_PROGRESS, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_NOSTART, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_COMPLETE, false);
                break;

            case FilterStatus.PROGRESS:
                this.setFilterName("btn_txt_progress");
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_CLEAR, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_PROGRESS, true);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_NOSTART, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_COMPLETE, false);
                break;

            case FilterStatus.NOTOPEN:
                this.setFilterName("btn_txt_notopen");
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_CLEAR, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_PROGRESS, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_NOSTART, true);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_COMPLETE, false);
                break;

            case FilterStatus.ACHIVE:
                this.setFilterName("btn_txt_completed");
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_CLEAR, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_PROGRESS, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_NOSTART, false);
                this.operationSortInfo.SetFilter(ListViewSort.FilterKind.MISSION_COMPLETE, true);
                break;
        }
        this.baseSortInfo.Set(this.operationSortInfo);
        base.SortItem(-1, false, -1);
        EventRewardSaveData.FilterId = (int) this.filterStatus;
    }

    public void setMissionListIdx()
    {
        if (EventRewardSaveData.FilterId == 0)
        {
            int count = base.itemList.Count;
            int index = 0;
            int missionId = EventRewardSaveData.MissionId;
            for (int i = 0; i < count; i++)
            {
                EventMissionItemListViewItem item = base.itemList[i] as EventMissionItemListViewItem;
                if ((missionId > 0) && (item.MissionId == missionId))
                {
                    index = i;
                    break;
                }
            }
            base.SetTopItem(index);
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
            case InitMode.INPUT:
            {
                bool flag = false;
                foreach (ListViewItem item in base.itemList)
                {
                    EventMissionItemListViewItem item2 = item as EventMissionItemListViewItem;
                    if (((item2.CurrentStatus == EventMissionItemListViewItem.ProgStatus.ACHIVE) || (item2.CurrentStatus == EventMissionItemListViewItem.ProgStatus.PROGRESS)) || (item2.CurrentStatus == EventMissionItemListViewItem.ProgStatus.CLEAR))
                    {
                        flag = true;
                    }
                }
                if (flag)
                {
                }
                if (mode == InitMode.INPUT)
                {
                    this.RequestListObject(EventMissionItemListViewObject.InitMode.INPUT);
                }
                else
                {
                    this.RequestListObject(EventMissionItemListViewObject.InitMode.VALID);
                }
                break;
            }
        }
    }

    public void SetMode(InitMode mode, System.Action callback)
    {
        this.callbackFunc2 = callback;
        this.SetMode(mode);
    }

    public void setNextMissionInfo(int missionID, System.Action callback)
    {
        this.targetMissionId = missionID;
        if (callback != null)
        {
            this.actionCallback = callback;
        }
        int count = base.itemList.Count;
        int index = 0;
        for (int i = 0; i < count; i++)
        {
            if ((base.itemList[i] as EventMissionItemListViewItem).EventMissionEntity.id == missionID)
            {
                index = i;
                break;
            }
        }
        Debug.Log(string.Concat(new object[] { "jump to ", index, " * ", missionID }));
        base.setCallbackAfterScroll(() => this.SetOpenItem());
        base.MoveTopItem(index, true);
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        EventMissionItemListViewObject obj2 = obj as EventMissionItemListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(EventMissionItemListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(EventMissionItemListViewObject.InitMode.VALID);
        }
    }

    public void SetOpenItem()
    {
        if (base.itemList != null)
        {
            List<EventMissionItemListViewObject> objectList = this.ObjectList;
            GameObject obj2 = null;
            if (objectList.Count > 0)
            {
                foreach (EventMissionItemListViewObject obj3 in objectList)
                {
                    EventMissionItemListViewItem item = obj3.GetItem();
                    if (item.MissionId == this.targetMissionId)
                    {
                        EventRewardSaveData.MissionId = this.targetMissionId;
                        this.openTargetItem = item;
                        if (item.IsOpenMission)
                        {
                            obj2 = getEffect("ef_mission_extric01", obj3.transform);
                        }
                        break;
                    }
                }
            }
            base.Invoke("modifyOpenItem", this.openItemTime);
        }
    }

    private void showRewardDetailInfo(EventMissionItemListViewItem selectItem)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        switch (selectItem.eventRewardType)
        {
            case RewardType.Type.GIFT:
                if (selectItem.Type == Gift.Type.ITEM)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenItemDetailDialog(selectItem.ItemEntity, new ItemDetailInfoComponent.CallbackFunc(this.closeItemDetail));
                }
                if (selectItem.Type.IsServant())
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.DROP, selectItem.SvtEntity.id, new ServantStatusDialog.ClickDelegate(this.closeSvtDetail));
                }
                break;

            case RewardType.Type.SET:
                SingletonMonoBehaviour<CommonUI>.Instance.OpenItemDetailDialog(selectItem.NameText, selectItem.ExtraDetailTXt, new ItemDetailInfoComponent.CallbackFunc(this.closeItemDetail));
                break;
        }
    }

    private void startSvtGetEffect(long userSvtID, bool isEventSvtGet)
    {
        <startSvtGetEffect>c__AnonStorey85 storey = new <startSvtGetEffect>c__AnonStorey85 {
            userSvtID = userSvtID,
            isEventSvtGet = isEventSvtGet,
            <>f__this = this
        };
        this.touchBlocker.SetActive(true);
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, new System.Action(storey.<>m__FE));
    }

    private void SvtEffCaller(System.Action callback)
    {
        <SvtEffCaller>c__AnonStorey86 storey = new <SvtEffCaller>c__AnonStorey86 {
            callback = callback,
            <>f__this = this
        };
        this.svtRewardComp.Play(new System.Action(storey.<>m__FF), SceneManager.DEFAULT_FADE_TIME);
    }

    public void unAcceptableDlg()
    {
        this.ShowMSG();
        this.targetFSM.SendEvent("END_NOTICE");
    }

    public List<EventMissionItemListViewObject> ClippingObjectList
    {
        get
        {
            List<EventMissionItemListViewObject> list = new List<EventMissionItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    EventMissionItemListViewObject component = obj2.GetComponent<EventMissionItemListViewObject>();
                    EventMissionItemListViewItem item = component.GetItem();
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

    public List<EventMissionItemListViewObject> ObjectList
    {
        get
        {
            List<EventMissionItemListViewObject> list = new List<EventMissionItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    EventMissionItemListViewObject component = obj2.GetComponent<EventMissionItemListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    [CompilerGenerated]
    private sealed class <startSvtGetEffect>c__AnonStorey85
    {
        internal EventMissionItemListViewManager <>f__this;
        internal bool isEventSvtGet;
        internal long userSvtID;

        internal void <>m__103()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, delegate {
                this.<>f__this.touchBlocker.SetActive(false);
                this.<>f__this.targetFSM.SendEvent("END_EFFECT");
            });
        }

        internal void <>m__104()
        {
            this.<>f__this.touchBlocker.SetActive(false);
            this.<>f__this.targetFSM.SendEvent("END_EFFECT");
        }

        internal void <>m__FE()
        {
            UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.userSvtID);
            if (this.<>f__this.servantRewardActionObject == null)
            {
                this.<>f__this.servantRewardActionObject = UnityEngine.Object.Instantiate<GameObject>(this.<>f__this.servantRewardActionPrefab);
                this.<>f__this.servantRewardActionObject.transform.parent = this.<>f__this.instantiatingPOS;
                this.<>f__this.servantRewardActionObject.transform.localPosition = Vector3.zero;
                this.<>f__this.servantRewardActionObject.transform.localScale = Vector3.one;
            }
            this.<>f__this.svtRewardComp = this.<>f__this.servantRewardActionObject.GetComponent<ServantRewardAction>();
            if (this.isEventSvtGet)
            {
                this.<>f__this.svtRewardComp.Setup(entity.svtId, entity.id, entity.limitCount, 1, true, ServantRewardAction.PLAY_FLAG.EVENT_SVT_GET | ServantRewardAction.PLAY_FLAG.FADE_IN);
            }
            else
            {
                this.<>f__this.svtRewardComp.Setup(entity.svtId, entity.id, entity.limitCount, 1, false, ServantRewardAction.PLAY_FLAG.FADE_IN);
            }
            this.<>f__this.SvtEffCaller((System.Action) (() => SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, delegate {
                this.<>f__this.touchBlocker.SetActive(false);
                this.<>f__this.targetFSM.SendEvent("END_EFFECT");
            })));
        }
    }

    [CompilerGenerated]
    private sealed class <SvtEffCaller>c__AnonStorey86
    {
        internal EventMissionItemListViewManager <>f__this;
        internal System.Action callback;

        internal void <>m__FF()
        {
            UnityEngine.Object.DestroyImmediate(this.<>f__this.servantRewardActionObject);
            this.callback.Call();
        }
    }

    public enum FilterStatus
    {
        INIT,
        CLEAR,
        PROGRESS,
        NOTOPEN,
        ACHIVE
    }

    public enum InitMode
    {
        NONE,
        INTO,
        VALID,
        INPUT,
        ENTER,
        EXIT,
        MODIFY
    }

    private class resData
    {
        public GetSvts[] getSvts;
    }
}

