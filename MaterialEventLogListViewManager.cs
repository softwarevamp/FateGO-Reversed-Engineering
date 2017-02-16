using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MaterialEventLogListViewManager : ListViewManager
{
    public const float EXIT_DELAY = 0f;
    public const float EXIT_TIME = 0.25f;
    protected InitMode initMode;
    public const float INTO_DELAY = 0f;
    public const float INTO_TIME = 0.25f;
    public const int ITEM_ALIGNMENT_COUNT = 4;
    private BoxCollider mBoxCollider;
    private bool mIsDoing_Slide;
    public const float OUT_POS_OFS_X = 512f;

    private void Awake()
    {
        this.mBoxCollider = base.gameObject.GetComponent<BoxCollider>();
        this.mBoxCollider.enabled = false;
        base.scrollView.disableDragIfFits = false;
    }

    public void CreateList(MaterialEventLogListViewItem.Kind kind, List<MaterialEventLogListViewItem.Info> infs)
    {
        this.DestroyList();
        base.CreateList(0);
        int count = infs.Count;
        if (count < 4)
        {
            count += 4 - infs.Count;
        }
        else
        {
            count++;
        }
        for (int i = 0; i < count; i++)
        {
            MaterialEventLogListViewItem item = new MaterialEventLogListViewItem(i, kind, null);
            if (i < infs.Count)
            {
                item.info = infs[i];
            }
            base.itemList.Add(item);
        }
        base.SortItem(-1, false, -1);
    }

    public void DestroyList()
    {
        base.DestroyList();
    }

    public PartyServantListViewItem GetItem(int index)
    {
        if (base.itemList != null)
        {
            return (base.itemList[index] as PartyServantListViewItem);
        }
        return null;
    }

    public UIScrollView GetScrollView() => 
        base.scrollView;

    protected void OnClickListView(ListViewObject obj)
    {
        MaterialEventLogListViewItem arg = obj.GetItem() as MaterialEventLogListViewItem;
        MaterialEventLogListViewItem.Info info = arg.info;
        if (info != null)
        {
            if ((info.flag & MaterialEventLogListViewItem.Flag.NOPLAY_DECIDE_SE) == MaterialEventLogListViewItem.Flag.NONE)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            }
            info.on_click_act.Call<MaterialEventLogListViewItem>(arg);
        }
    }

    protected void RequestListObject(MaterialEventLogListViewObject.InitMode mode, System.Action end_act = null)
    {
        <RequestListObject>c__AnonStorey89 storey = new <RequestListObject>c__AnonStorey89 {
            end_act = end_act,
            <>f__this = this
        };
        List<MaterialEventLogListViewObject> objectList = this.ObjectList;
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].Init(mode);
        }
        if (this.initMode == InitMode.INTO)
        {
            this.mIsDoing_Slide = true;
            SlideFadeObject obj4 = base.gameObject.gameObject.SafeGetComponent<SlideFadeObject>();
            float time = TerminalPramsManager.GetIntpTime_AutoResume(0.25f);
            obj4.SlideIn((float) 512f, time, 0f, new System.Action(storey.<>m__11C));
        }
        else if (this.initMode == InitMode.EXIT)
        {
            this.mIsDoing_Slide = true;
            SlideFadeObject obj6 = base.gameObject.gameObject.SafeGetComponent<SlideFadeObject>();
            float num3 = TerminalPramsManager.GetIntpTime_AutoResume(0.25f);
            obj6.SlideOut((float) 512f, num3, 0f, new System.Action(storey.<>m__11D));
        }
        else
        {
            storey.end_act.Call();
        }
    }

    public bool SetMode(InitMode mode, System.Action end_act = null)
    {
        bool flag = false;
        if (this.mIsDoing_Slide)
        {
            flag = true;
        }
        if ((mode == InitMode.INTO) && (this.initMode != InitMode.NONE))
        {
            flag = true;
        }
        if ((mode == InitMode.EXIT) && (this.initMode != InitMode.INPUT))
        {
            flag = true;
        }
        if (flag)
        {
            end_act.Call();
            return false;
        }
        this.initMode = mode;
        base.IsInput = mode == InitMode.INPUT;
        this.mBoxCollider.enabled = base.IsInput;
        switch (mode)
        {
            case InitMode.VALID:
            case InitMode.INTO:
            case InitMode.EXIT:
                this.RequestListObject(MaterialEventLogListViewObject.InitMode.VALID, end_act);
                break;

            case InitMode.INPUT:
                this.RequestListObject(MaterialEventLogListViewObject.InitMode.INPUT, end_act);
                break;

            default:
                this.RequestListObject(MaterialEventLogListViewObject.InitMode.INVISIBLE, end_act);
                break;
        }
        return true;
    }

    protected override void SetObjectItem(ListViewObject obj, ListViewItem item)
    {
        MaterialEventLogListViewObject obj2 = obj as MaterialEventLogListViewObject;
        if (this.initMode == InitMode.INPUT)
        {
            obj2.Init(MaterialEventLogListViewObject.InitMode.INPUT);
        }
        else
        {
            obj2.Init(MaterialEventLogListViewObject.InitMode.VALID);
        }
    }

    public bool IsDoing_Slide =>
        this.mIsDoing_Slide;

    public List<MaterialEventLogListViewObject> ObjectList
    {
        get
        {
            List<MaterialEventLogListViewObject> list = new List<MaterialEventLogListViewObject>();
            foreach (GameObject obj2 in base.objectList)
            {
                if (obj2 != null)
                {
                    MaterialEventLogListViewObject component = obj2.GetComponent<MaterialEventLogListViewObject>();
                    list.Add(component);
                }
            }
            return list;
        }
    }

    [CompilerGenerated]
    private sealed class <RequestListObject>c__AnonStorey89
    {
        internal MaterialEventLogListViewManager <>f__this;
        internal System.Action end_act;

        internal void <>m__11C()
        {
            this.<>f__this.mIsDoing_Slide = false;
            this.<>f__this.SetMode(MaterialEventLogListViewManager.InitMode.INPUT, this.end_act);
        }

        internal void <>m__11D()
        {
            this.<>f__this.mIsDoing_Slide = false;
            this.<>f__this.SetMode(MaterialEventLogListViewManager.InitMode.NONE, () => this.end_act.Call());
        }

        internal void <>m__11E()
        {
            this.end_act.Call();
        }
    }

    public enum InitMode
    {
        NONE,
        VALID,
        INPUT,
        INTO,
        EXIT
    }
}

