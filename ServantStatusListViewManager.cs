using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantStatusListViewManager : MonoBehaviour
{
    protected int callbackCount;
    protected static ServantStatusListViewItemDraw.Kind[] equipKindList = new ServantStatusListViewItemDraw.Kind[] { ServantStatusListViewItemDraw.Kind.EQUIP_MAIN, ServantStatusListViewItemDraw.Kind.SKILL, ServantStatusListViewItemDraw.Kind.FLAVOR_TEXT };
    protected InitMode initMode;
    protected bool isInput = true;
    protected bool isScrollRefresh;
    protected static int LIST_BLANK = 10;
    public GameObject listParent;
    protected ServantStatusListViewItem mainInfo;
    protected static ServantStatusListViewItemDraw.Kind[] normalKindList = new ServantStatusListViewItemDraw.Kind[] { ServantStatusListViewItemDraw.Kind.MAIN, ServantStatusListViewItemDraw.Kind.EQUIP, ServantStatusListViewItemDraw.Kind.SKILL, ServantStatusListViewItemDraw.Kind.CLASS_SKILL, ServantStatusListViewItemDraw.Kind.NP, ServantStatusListViewItemDraw.Kind.COMMAND, ServantStatusListViewItemDraw.Kind.FACE };
    protected List<ServantStatusListViewObject> objectList = new List<ServantStatusListViewObject>();
    protected Vector3 oldScrollPosition;
    public UIScrollBar scrollBar;
    public UIScrollView scrollView;
    [SerializeField]
    protected GameObject[] statusObjectList;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    public void CreateList(ServantStatusListViewItem mainInfo)
    {
        this.DestroyList();
        if (this.scrollView != null)
        {
            UIPanel component = this.scrollView.GetComponent<UIPanel>();
            if (component != null)
            {
                Vector2 vector = (Vector2) (component.clipOffset * -1f);
                this.scrollView.transform.localPosition = (Vector3) vector;
                this.scrollView.ResetPosition();
            }
        }
        this.mainInfo = mainInfo;
        ServantStatusListViewItemDraw.Kind[] equipKindList = null;
        if (SvtType.IsServantEquip((SvtType.Type) this.mainInfo.Servant.type))
        {
            equipKindList = ServantStatusListViewManager.equipKindList;
        }
        else
        {
            equipKindList = normalKindList;
        }
        int num = (equipKindList == null) ? 0 : equipKindList.Length;
        if (this.statusObjectList != null)
        {
            Vector3 vector2 = new Vector3(0f, 0f, 0f);
            for (int i = 0; i < num; i++)
            {
                ServantStatusListViewItemDraw.Kind kind = equipKindList[i];
                int num3 = (int) kind;
                if ((num3 > 0) && (num3 <= this.statusObjectList.Length))
                {
                    switch (kind)
                    {
                        case ServantStatusListViewItemDraw.Kind.EQUIP:
                        {
                            if ((this.mainInfo.IsEquipShowMode && this.mainInfo.Servant.IsServant) && !this.mainInfo.IsCollection)
                            {
                                break;
                            }
                            continue;
                        }
                        case ServantStatusListViewItemDraw.Kind.SKILL:
                        {
                            int[] numArray;
                            int[] numArray2;
                            int[] numArray3;
                            string[] strArray;
                            string[] strArray2;
                            if (!this.mainInfo.Servant.IsServant && !this.mainInfo.Servant.IsServantEquip)
                            {
                                continue;
                            }
                            this.mainInfo.GetSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
                            bool flag = false;
                            for (int j = 0; j < numArray.Length; j++)
                            {
                                if (numArray[j] > 0)
                                {
                                    flag = true;
                                    break;
                                }
                            }
                            if (flag)
                            {
                                break;
                            }
                            continue;
                        }
                        case ServantStatusListViewItemDraw.Kind.CLASS_SKILL:
                        {
                            int[] numArray4 = this.mainInfo.Servant.getClassPassive();
                            if ((numArray4 != null) && (numArray4.Length > 0))
                            {
                                break;
                            }
                            continue;
                        }
                        case ServantStatusListViewItemDraw.Kind.FLAVOR_TEXT:
                        {
                            bool flag2 = false;
                            for (int k = 0; k < this.mainInfo.ServantCommentDataList.Length; k++)
                            {
                                if (!this.mainInfo.ServantCommentDataList[k].IsConst())
                                {
                                    flag2 = true;
                                    break;
                                }
                            }
                            if (flag2)
                            {
                                break;
                            }
                            continue;
                        }
                        case ServantStatusListViewItemDraw.Kind.NP:
                            int num6;
                            int num7;
                            int num8;
                            int num9;
                            int num10;
                            string str;
                            string str2;
                            int num11;
                            int num12;
                            this.mainInfo.GetNpInfo(out num6, out num7, out num8, out num9, out num10, out str, out str2, out num11, out num12);
                            if (num6 <= 0)
                            {
                                continue;
                            }
                            break;
                    }
                    GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.statusObjectList[num3 - 1]);
                    if (obj2 != null)
                    {
                        ServantStatusListViewObject item = obj2.GetComponent<ServantStatusListViewObject>();
                        item.SetItem(this.mainInfo);
                        int size = item.GetSize();
                        vector2.y -= size / 2;
                        obj2.transform.parent = this.listParent.transform;
                        obj2.transform.localPosition = vector2;
                        obj2.transform.localRotation = Quaternion.identity;
                        obj2.transform.localScale = Vector3.one;
                        obj2.layer = this.listParent.layer;
                        item.SetBaseTransform();
                        item.SetManager(this);
                        this.objectList.Add(item);
                        vector2.y -= (size / 2) + LIST_BLANK;
                    }
                }
            }
        }
        if (this.scrollView != null)
        {
            this.scrollView.ResetPosition();
        }
    }

    public void DestroyList()
    {
        foreach (ServantStatusListViewObject obj2 in this.objectList)
        {
            UnityEngine.Object.Destroy(obj2.gameObject);
        }
        this.objectList.Clear();
        this.mainInfo = null;
        if (this.scrollView != null)
        {
            this.scrollView.ResetPosition();
        }
    }

    protected void OnClickCommandCharaLevel1(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.COMMAND1);
            }
        }
    }

    protected void OnClickCommandCharaLevel2(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.COMMAND2);
            }
        }
    }

    protected void OnClickCommandCharaLevel3(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.COMMAND3);
            }
        }
    }

    protected void OnClickFaceCharaLevel1(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.FACE1);
            }
        }
    }

    protected void OnClickFaceCharaLevel2(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.FACE2);
            }
        }
    }

    protected void OnClickFaceCharaLevel3(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.FACE3);
            }
        }
    }

    protected void OnClickFaceCharaLevel4(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.FACE4);
            }
        }
    }

    protected void OnClickListView(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.SELECT);
            }
        }
    }

    protected void OnClickListViewEquip1(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.EQUIP1);
            }
        }
    }

    protected void OnClickListViewEquipExp(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            if (this.mainInfo.IsEquip)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenPopupMessageDialog(string.Format(LocalizationManager.Get("SERVANT_STATUS_TOTAL_EXP"), LocalizationManager.GetNumberFormat(this.mainInfo.EquipExp)));
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
        }
    }

    protected void OnClickListViewExp(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenPopupMessageDialog(string.Format(LocalizationManager.Get("SERVANT_STATUS_TOTAL_EXP"), LocalizationManager.GetNumberFormat(this.mainInfo.Exp)));
        }
    }

    protected void OnClickListViewFriendship(ServantStatusListViewObject obj)
    {
        if ((this.initMode == InitMode.INPUT) && (this.mainInfo.UserServantCollection != null))
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenPopupMessageDialog(string.Format(LocalizationManager.Get("SERVANT_STATUS_TOTAL_FRIENDSHIP"), LocalizationManager.GetNumberFormat(this.mainInfo.UserServantCollection.friendship)));
        }
    }

    protected void OnEnable()
    {
        this.isScrollRefresh = true;
    }

    protected void OnLongPushListViewEquip1(ServantStatusListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc(obj.GetKind(), ResultKind.EQUIP1_STATUS);
            }
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
                if (this.scrollView != null)
                {
                    this.scrollView.UpdateScrollbars(true);
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

    protected void RequestListObject(ServantStatusListViewObject.InitMode mode)
    {
        List<ServantStatusListViewObject> objectList = this.objectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantStatusListViewObject obj2 in objectList)
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

    protected void RequestListObject(ServantStatusListViewObject.InitMode mode, float delay)
    {
        List<ServantStatusListViewObject> objectList = this.objectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantStatusListViewObject obj2 in objectList)
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
        this.callbackCount = this.objectList.Count;
        switch (mode)
        {
            case InitMode.INPUT:
                this.IsInput = true;
                this.RequestListObject(ServantStatusListViewObject.InitMode.INPUT);
                break;

            case InitMode.VALID:
                this.IsInput = false;
                this.RequestListObject(ServantStatusListViewObject.InitMode.VALID);
                break;

            case InitMode.BATTLE:
                this.RequestListObject(ServantStatusListViewObject.InitMode.BATTLE);
                break;

            case InitMode.COMMAND:
                this.RequestListObject(ServantStatusListViewObject.InitMode.COMMAND);
                break;

            case InitMode.FACE:
                this.RequestListObject(ServantStatusListViewObject.InitMode.FACE);
                break;

            case InitMode.MODIFY:
                this.RequestListObject(ServantStatusListViewObject.InitMode.MODIFY);
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

    protected void SetObjectItem(ServantStatusListViewObject obj)
    {
        InitMode initMode = this.initMode;
        if (initMode == InitMode.INPUT)
        {
            obj.Init(ServantStatusListViewObject.InitMode.INPUT);
        }
        else if (initMode == InitMode.MODIFY)
        {
            obj.Init(ServantStatusListViewObject.InitMode.MODIFY);
        }
        else
        {
            obj.Init(ServantStatusListViewObject.InitMode.VALID);
        }
    }

    public bool IsInput
    {
        get => 
            this.isInput;
        set
        {
            this.isInput = value;
            if (this.scrollBar != null)
            {
                this.scrollBar.alpha = this.scrollBar.alpha;
            }
        }
    }

    public delegate void CallbackFunc(ServantStatusListViewItemDraw.Kind kind, ServantStatusListViewManager.ResultKind result);

    public enum InitMode
    {
        NONE,
        INPUT,
        VALID,
        BATTLE,
        COMMAND,
        FACE,
        MODIFY
    }

    public enum ResultKind
    {
        SELECT,
        EQUIP1,
        EQUIP1_STATUS,
        COMMAND1,
        COMMAND2,
        COMMAND3,
        FACE1,
        FACE2,
        FACE3,
        FACE4
    }
}

