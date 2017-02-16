using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantStatusFlavorTextListViewManager : MonoBehaviour
{
    protected int callbackCount;
    protected InitMode initMode;
    protected bool isInput = true;
    protected bool isScrollRefresh;
    protected static int LIST_BLANK = 10;
    [SerializeField]
    protected GameObject listParent;
    protected ServantStatusListViewItem mainInfo;
    protected List<ServantStatusFlavorTextListViewObject> objectList = new List<ServantStatusFlavorTextListViewObject>();
    protected Vector3 oldScrollPosition;
    [SerializeField]
    protected GameObject paramObject;
    [SerializeField]
    protected GameObject profile2Object;
    [SerializeField]
    protected GameObject profileObject;
    [SerializeField]
    protected UIScrollBar scrollBar;
    [SerializeField]
    protected UIScrollView scrollView;
    [SerializeField]
    protected GameObject terminalObject;
    [SerializeField]
    protected GameObject voiceObject;

    protected event CallbackFunc callbackFunc;

    protected event System.Action callbackFunc2;

    protected ServantStatusFlavorTextListViewObject AddObjectList(ref Vector3 basePositon, GameObject prefab)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(prefab);
        if (obj2 != null)
        {
            ServantStatusFlavorTextListViewObject component = obj2.GetComponent<ServantStatusFlavorTextListViewObject>();
            int size = component.GetSize();
            basePositon.y -= size / 2;
            obj2.transform.parent = this.listParent.transform;
            obj2.transform.localPosition = basePositon;
            obj2.transform.localRotation = Quaternion.identity;
            obj2.transform.localScale = Vector3.one;
            obj2.layer = this.listParent.layer;
            component.SetBaseTransform();
            component.SetItem(this.mainInfo, 0, true, false, string.Empty, string.Empty);
            component.SetManager(this);
            this.objectList.Add(component);
            basePositon.y -= (size / 2) + LIST_BLANK;
            return component;
        }
        return null;
    }

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
        SvtType.Type type = (SvtType.Type) this.mainInfo.Servant.type;
        Vector3 basePositon = new Vector3(0f, 0f, 0f);
        ServantCommentEntity[] servantCommentDataList = this.mainInfo.ServantCommentDataList;
        this.AddObjectList(ref basePositon, this.voiceObject);
        for (int i = 0; i < servantCommentDataList.Length; i++)
        {
            ServantCommentEntity entity = servantCommentDataList[i];
            if (entity.IsConst())
            {
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.profileObject);
                if (obj2 != null)
                {
                    ServantStatusFlavorTextListViewObject item = obj2.GetComponent<ServantStatusFlavorTextListViewObject>();
                    item.SetItem(this.mainInfo, entity.id, true, entity.IsNew(), entity.comment, string.Empty);
                    int size = item.GetSize();
                    basePositon.y -= size / 2;
                    obj2.transform.parent = this.listParent.transform;
                    obj2.transform.localPosition = basePositon;
                    obj2.transform.localRotation = Quaternion.identity;
                    obj2.transform.localScale = Vector3.one;
                    obj2.layer = this.listParent.layer;
                    item.SetBaseTransform();
                    item.SetManager(this);
                    this.objectList.Add(item);
                    basePositon.y -= (size / 2) + LIST_BLANK;
                }
                break;
            }
        }
        if (SvtType.IsServant(type) || SvtType.IsEnemyCollectionDetail(type))
        {
            this.AddObjectList(ref basePositon, this.paramObject);
        }
        for (int j = 0; j < servantCommentDataList.Length; j++)
        {
            ServantCommentEntity entity2 = servantCommentDataList[j];
            if (!entity2.IsConst())
            {
                GameObject obj4 = UnityEngine.Object.Instantiate<GameObject>(this.profile2Object);
                if (obj4 != null)
                {
                    ServantStatusFlavorTextListViewObject obj5 = obj4.GetComponent<ServantStatusFlavorTextListViewObject>();
                    string str = string.Format(LocalizationManager.Get("SERVANT_STATUS_PROFILE_CONDITION"), CondType.OpenConditionText((CondType.Kind) entity2.condType, entity2.condValue));
                    if (entity2.IsOpen(-1))
                    {
                        obj5.SetItem(this.mainInfo, entity2.id, true, entity2.IsNew(), entity2.comment, str);
                    }
                    else
                    {
                        obj5.SetItem(this.mainInfo, entity2.id, false, false, LocalizationManager.GetUnknownName(), str);
                    }
                    int num4 = obj5.GetSize();
                    basePositon.y -= num4 / 2;
                    obj4.transform.parent = this.listParent.transform;
                    obj4.transform.localPosition = basePositon;
                    obj4.transform.localRotation = Quaternion.identity;
                    obj4.transform.localScale = Vector3.one;
                    obj4.layer = this.listParent.layer;
                    obj5.SetBaseTransform();
                    obj5.SetManager(this);
                    this.objectList.Add(obj5);
                    basePositon.y -= (num4 / 2) + LIST_BLANK;
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
        foreach (ServantStatusFlavorTextListViewObject obj2 in this.objectList)
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

    protected void OnClickListView(ServantStatusFlavorTextListViewObject obj)
    {
        if (obj.GetKind() == ServantStatusFlavorTextListViewItemDraw.Kind.TEXT)
        {
            CallbackFunc callbackFunc = this.callbackFunc;
            if (callbackFunc != null)
            {
                this.callbackFunc = null;
                callbackFunc(obj.Id);
            }
        }
    }

    protected void OnClickListViewVoice(ServantStatusFlavorTextListViewObject obj)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(0);
        }
    }

    protected void OnEnable()
    {
        this.isScrollRefresh = true;
    }

    protected void OnMoveEnd()
    {
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

    protected void RequestListObject(ServantStatusFlavorTextListViewObject.InitMode mode)
    {
        List<ServantStatusFlavorTextListViewObject> objectList = this.objectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantStatusFlavorTextListViewObject obj2 in objectList)
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

    protected void RequestListObject(ServantStatusFlavorTextListViewObject.InitMode mode, float delay)
    {
        List<ServantStatusFlavorTextListViewObject> objectList = this.objectList;
        if (objectList.Count > 0)
        {
            this.callbackCount = objectList.Count;
            foreach (ServantStatusFlavorTextListViewObject obj2 in objectList)
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
        this.IsInput = mode == InitMode.INPUT;
        switch (mode)
        {
            case InitMode.INPUT:
                this.RequestListObject(ServantStatusFlavorTextListViewObject.InitMode.INPUT);
                break;

            case InitMode.VALID:
                this.RequestListObject(ServantStatusFlavorTextListViewObject.InitMode.VALID);
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

    protected void SetObjectItem(ServantStatusFlavorTextListViewObject obj)
    {
        if (this.initMode == InitMode.INPUT)
        {
            obj.Init(ServantStatusFlavorTextListViewObject.InitMode.INPUT);
        }
        else
        {
            obj.Init(ServantStatusFlavorTextListViewObject.InitMode.VALID);
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

    public delegate void CallbackFunc(int result);

    public enum InitMode
    {
        NONE,
        INPUT,
        VALID
    }
}

