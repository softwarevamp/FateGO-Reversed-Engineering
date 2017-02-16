using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FollowerSelectItemListViewManager : ListViewManager
{
    protected int callbackCount;
    protected const string CLASS_ID_SAVE_KEY = "FollowerSelectClass";
    [SerializeField]
    protected ClassButtonControlComponent classButtonControl;
    protected static int followerClassId;
    protected InitMode initMode;
    protected static int oldFollowerClassId;
    protected int questId;
    protected int questPhase;
    protected const string SORT_SAVE_KEY = "FollowerSelect";
    protected static ListViewSort supportSortInfo = new ListViewSort("FollowerSelect", ListViewSort.SortKind.LOGIN_ACCESS, false);
    [SerializeField]
    protected FollowerSelectItemListViewObject tutorialListViewObject;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public bool ChangeClass(int classPos)
    {
        bool isInput = base.isInput;
        base.isInput = false;
        this.classButtonControl.setCursor(classPos);
        base.isInput = isInput;
        this.SortClass(classPos);
        return (base.sort.Kind != ListViewSort.SortKind.LOGIN_ACCESS);
    }

    public void CreateList(int questId, int questPhase, int friendPointUpVal, EventUpValSetupInfo setupInfo)
    {
        this.questId = questId;
        this.questPhase = questPhase;
        base.sort = supportSortInfo;
        if (base.sort.IsRequestLoad)
        {
            followerClassId = oldFollowerClassId = PlayerPrefs.GetInt("FollowerSelectClass", 0);
        }
        base.sort.Load();
        this.classButtonControl.init(new ClassButtonControlComponent.CallbackFunc(this.OnChangeClass), false);
        base.isInput = false;
        this.classButtonControl.setCursor(followerClassId);
        QuestPhaseEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.QUEST_PHASE).getEntityFromId<QuestPhaseEntity>(questId, questPhase);
        base.CreateList(0);
        if (entity != null)
        {
            FollowerInfo[] questFollowerList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<NpcFollowerMaster>(DataNameKind.Kind.NPC_FOLLOWER).GetQuestFollowerList(questId, questPhase);
            for (int i = 0; i < questFollowerList.Length; i++)
            {
                FollowerSelectItemListViewItem item = new FollowerSelectItemListViewItem(base.itemList.Count, questFollowerList[i], followerClassId, friendPointUpVal, setupInfo);
                base.itemList.Add(item);
            }
            if (!entity.isNpcOnly)
            {
                UserFollowerEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_FOLLOWER).getSingleEntity<UserFollowerEntity>();
                if (entity2 != null)
                {
                    FollowerInfo[] followerInfo = entity2.followerInfo;
                    for (int j = 0; j < followerInfo.Length; j++)
                    {
                        FollowerSelectItemListViewItem item2 = new FollowerSelectItemListViewItem(base.itemList.Count, followerInfo[j], followerClassId, friendPointUpVal, setupInfo);
                        base.itemList.Add(item2);
                    }
                }
            }
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.sort = supportSortInfo;
        if (base.sort.IsRequestLoad)
        {
            followerClassId = oldFollowerClassId = PlayerPrefs.GetInt("FollowerSelectClass", 0);
        }
        base.sort.Load();
        base.DestroyList();
        base.sort.Save();
        if (base.sort.IsRequestSave && (followerClassId != oldFollowerClassId))
        {
            oldFollowerClassId = followerClassId;
            PlayerPrefs.SetInt("FollowerSelectClass", followerClassId);
            PlayerPrefs.Save();
        }
        this.classButtonControl.init(new ClassButtonControlComponent.CallbackFunc(this.OnChangeClass), false);
        base.isInput = false;
        this.classButtonControl.setCursor(followerClassId);
        this.tutorialListViewObject.ClearItem();
        this.tutorialListViewObject.gameObject.SetActive(false);
    }

    protected void EndClassCompatibilityMenu()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseClassCompatibilityMenu(null);
    }

    public int GetClassId() => 
        followerClassId;

    public FollowerSelectItemListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as FollowerSelectItemListViewItem);
        }
        return null;
    }

    public static void InitLoad()
    {
        supportSortInfo.InitLoad();
    }

    public void OnChangeClass(int classPos)
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.SortClass(classPos);
        }
    }

    public void OnClickClassComparibility()
    {
        if (base.isInput)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenClassCompatibilityMenu(new System.Action(this.EndClassCompatibilityMenu));
        }
    }

    protected void OnClickListView(ListViewObject obj)
    {
    }

    protected void OnClickSelectListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnClick " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.NONE, obj.Index);
            }
        }
    }

    protected void OnClickSkill1ListView(ListViewObject obj)
    {
    }

    protected void OnClickSkill2ListView(ListViewObject obj)
    {
    }

    protected void OnClickSkill3ListView(ListViewObject obj)
    {
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
            ListViewSort.SortKind kind = base.sort.Kind;
            switch (kind)
            {
                case ListViewSort.SortKind.LEVEL:
                    base.sort.SetKind(ListViewSort.SortKind.HP);
                    break;

                case ListViewSort.SortKind.HP:
                    base.sort.SetKind(ListViewSort.SortKind.ATK);
                    break;

                case ListViewSort.SortKind.ATK:
                    base.sort.SetKind(ListViewSort.SortKind.LOGIN_ACCESS);
                    break;

                default:
                    if (kind == ListViewSort.SortKind.LOGIN_ACCESS)
                    {
                        base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    }
                    else
                    {
                        base.sort.SetKind(ListViewSort.SortKind.LEVEL);
                    }
                    break;
            }
            base.SortItem(-1, false, -1);
        }
    }

    protected void OnClickSupportListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnClickSupprot " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SUPPORT_INFO, obj.Index);
            }
        }
    }

    protected void OnLongPushListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnLongPush " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SERVANT_STATUS, obj.Index);
            }
        }
    }

    protected void OnLongPushSkill1ListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnLongPushSkill1 " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SERVANT_SKILL1_STATUS, obj.Index);
            }
        }
    }

    protected void OnLongPushSkill2ListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnLongPushSkill2 " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SERVANT_SKILL2_STATUS, obj.Index);
            }
        }
    }

    protected void OnLongPushSkill3ListView(ListViewObject obj)
    {
        if (base.isInput)
        {
            Debug.Log("Manager ListView OnLongPushSkill3 " + obj.Index);
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(ResultKind.SERVANT_SKILL3_STATUS, obj.Index);
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

    protected void RequestInto()
    {
        base.ClippingItems(true, false);
        base.DragMaskStart();
        List<FollowerSelectItemListViewObject> objectList = this.ObjectList;
        this.callbackCount = objectList.Count;
        int num = 0;
        for (int i = 0; i < objectList.Count; i++)
        {
            FollowerSelectItemListViewObject obj2 = objectList[i];
            if (base.ClippingItem(obj2))
            {
                num++;
                obj2.Init(FollowerSelectItemListViewObject.InitMode.INTO, new System.Action(this.OnMoveEnd), 0.1f);
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

    protected void RequestListObject(FollowerSelectItemListViewObject.InitMode mode)
    {
        List<FollowerSelectItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (FollowerSelectItemListViewObject obj2 in objectList)
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

    protected void RequestListObject(FollowerSelectItemListViewObject.InitMode mode, float delay)
    {
        List<FollowerSelectItemListViewObject> objectList = this.ObjectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (FollowerSelectItemListViewObject obj2 in objectList)
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

    public void SetClassId(int followerClassId)
    {
        FollowerSelectItemListViewManager.followerClassId = followerClassId;
        if (base.itemList != null)
        {
            foreach (ListViewItem item in base.itemList)
            {
                (item as FollowerSelectItemListViewItem).SetClassId(followerClassId);
            }
        }
    }

    public void SetMode(InitMode mode)
    {
        if (mode == InitMode.MODIFY)
        {
            List<FollowerSelectItemListViewObject> objectList = this.ObjectList;
            for (int i = 0; i < objectList.Count; i++)
            {
                objectList[i].Init(FollowerSelectItemListViewObject.InitMode.MODIFY);
            }
        }
        else
        {
            this.initMode = mode;
            this.callbackCount = base.ObjectSum;
            base.IsInput = mode == InitMode.INPUT;
            switch (mode)
            {
                case InitMode.INTO:
                    base.Invoke("RequestInto", 0f);
                    break;

                case InitMode.INPUT:
                    this.RequestListObject(FollowerSelectItemListViewObject.InitMode.INPUT);
                    break;

                case InitMode.NPC_GUIDE_FIRST_SELECT:
                {
                    base.IsInput = true;
                    List<FollowerSelectItemListViewObject> clippingObjectList = this.ClippingObjectList;
                    if (clippingObjectList.Count <= 0)
                    {
                        this.callbackCount = 1;
                        base.Invoke("OnMoveEnd", 0f);
                        break;
                    }
                    this.callbackCount = clippingObjectList.Count;
                    for (int j = 0; j < clippingObjectList.Count; j++)
                    {
                        FollowerSelectItemListViewObject obj3 = clippingObjectList[j];
                        if (j > 0)
                        {
                            obj3.Init(FollowerSelectItemListViewObject.InitMode.VALID, new System.Action(this.OnMoveEnd));
                        }
                        else
                        {
                            obj3.Init(FollowerSelectItemListViewObject.InitMode.INVISIBLE, new System.Action(this.OnMoveEnd));
                            this.tutorialListViewObject.gameObject.SetActive(true);
                            this.tutorialListViewObject.SetManager(this);
                            this.tutorialListViewObject.SetItem(obj3.GetItem());
                            this.tutorialListViewObject.Init(FollowerSelectItemListViewObject.InitMode.TUTORIAL_INPUT);
                        }
                    }
                    break;
                }
            }
            Debug.Log("SetMode " + mode);
        }
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
        FollowerSelectItemListViewObject obj2 = obj as FollowerSelectItemListViewObject;
        InitMode initMode = this.initMode;
        if (initMode == InitMode.INPUT)
        {
            obj2.Init(FollowerSelectItemListViewObject.InitMode.INPUT);
        }
        else if (initMode == InitMode.MODIFY)
        {
            obj2.Init(FollowerSelectItemListViewObject.InitMode.MODIFY);
        }
        else
        {
            obj2.Init(FollowerSelectItemListViewObject.InitMode.VALID);
        }
    }

    protected void SortClass(int classPos)
    {
        this.SetClassId(classPos);
        if (base.itemSortList != null)
        {
            switch (base.sort.Kind)
            {
                case ListViewSort.SortKind.LOGIN_ACCESS:
                    foreach (ListViewItem item in base.itemSortList)
                    {
                        item.SetSortValue(base.sort);
                    }
                    if (base.itemSortList.Count > 0)
                    {
                        base.itemSortList[0].IsTermination = true;
                        base.itemSortList[base.itemSortList.Count - 1].IsTermination = true;
                    }
                    this.SetMode(InitMode.MODIFY);
                    return;
            }
            base.SortItem(-1, false, -1);
        }
    }

    public List<FollowerSelectItemListViewObject> ClippingObjectList
    {
        get
        {
            List<FollowerSelectItemListViewObject> list = new List<FollowerSelectItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    FollowerSelectItemListViewObject component = obj2.GetComponent<FollowerSelectItemListViewObject>();
                    FollowerSelectItemListViewItem item = component.GetItem();
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

    public List<FollowerSelectItemListViewObject> ObjectList
    {
        get
        {
            List<FollowerSelectItemListViewObject> list = new List<FollowerSelectItemListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    FollowerSelectItemListViewObject component = obj2.GetComponent<FollowerSelectItemListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    public delegate void CallbackFunc(FollowerSelectItemListViewManager.ResultKind kind, int result);

    public enum InitMode
    {
        NONE,
        INTO,
        INPUT,
        MODIFY,
        NPC_GUIDE_FIRST_SELECT
    }

    public enum ResultKind
    {
        NONE,
        SERVANT_STATUS,
        SERVANT_SKILL1_STATUS,
        SERVANT_SKILL2_STATUS,
        SERVANT_SKILL3_STATUS,
        SUPPORT_INFO
    }
}

