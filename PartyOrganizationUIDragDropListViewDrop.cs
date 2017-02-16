using System;
using UnityEngine;

public class PartyOrganizationUIDragDropListViewDrop : UIDragDropListViewItem
{
    protected static float ACTION_TIME = 0.2f;
    protected PartyOrganizationListViewDropObject dropObject;

    protected void DragDumpEnd()
    {
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        PartyOrganizationListViewObject viewObject = mListViewObject.GetItem().ViewObject as PartyOrganizationListViewObject;
        if (viewObject != null)
        {
            viewObject.Init(PartyOrganizationListViewObject.InitMode.DRAG_INPUT);
        }
        base.OnDragDropRelease(null);
        mListViewObject.Init(PartyOrganizationListViewDropObject.InitMode.INVISIBLE);
        mListViewObject.SetItem(null);
        base.Invoke("DragEnd", 0.1f);
    }

    protected void DragDumpStart()
    {
        TweenColor.Begin(base.dragObject, ACTION_TIME, Color.clear).method = UITweener.Method.EaseInOut;
        Vector3 pos = base.dragObject.transform.localPosition + ((Vector3) (Vector3.down * 50f));
        TweenPosition position = TweenPosition.Begin(base.dragObject, ACTION_TIME, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "DragDumpEnd";
    }

    protected void DragEnd()
    {
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        (mListViewObject.Manager as PartyOrganizationListViewManager).ItemDragEnd();
    }

    protected void DragReturnEnd()
    {
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        base.OnDragDropRelease(null);
        mListViewObject.Init(PartyOrganizationListViewDropObject.InitMode.DRAG_INPUT);
        base.Invoke("DragEnd", 0.1f);
    }

    protected void DragReturnMoveEnd()
    {
        (base.mListViewObject as PartyOrganizationListViewDropObject).Init(PartyOrganizationListViewDropObject.InitMode.DRAG_VALID);
        base.Invoke("DragReturnEnd", 0.1f);
    }

    protected void DragReturnStart()
    {
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        Vector3 pos = base.dragObject.transform.parent.InverseTransformPoint(mListViewObject.transform.position);
        TweenPosition position = TweenPosition.Begin(base.dragObject, ACTION_TIME, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "DragReturnMoveEnd";
    }

    protected void DragSwapEnd()
    {
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        base.OnDragDropRelease(null);
        mListViewObject.Init(PartyOrganizationListViewDropObject.InitMode.DRAG_INPUT);
        this.dropObject.Init(PartyOrganizationListViewDropObject.InitMode.DRAG_DELETE);
        this.dropObject.Init(PartyOrganizationListViewDropObject.InitMode.DRAG_INPUT);
        this.dropObject = null;
        base.Invoke("DragEnd", 0.1f);
    }

    protected void DragSwapMoveEnd()
    {
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        PartyOrganizationListViewItem item = mListViewObject.GetItem();
        this.dropObject.GetItem().Swap(item);
        item = mListViewObject.GetItem();
        PartyOrganizationListViewItem item2 = this.dropObject.GetItem();
        mListViewObject.Init(PartyOrganizationListViewDropObject.InitMode.DRAG_VALID);
        this.dropObject.Init(PartyOrganizationListViewDropObject.InitMode.DRAG_VALID);
        base.Invoke("DragSwapEnd", 0.1f);
    }

    protected void DragSwapStart(PartyOrganizationListViewDropObject ddo)
    {
        this.dropObject = ddo;
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        Vector3 pos = base.dragObject.transform.parent.InverseTransformPoint(this.dropObject.transform.position);
        TweenPosition.Begin(base.dragObject, ACTION_TIME, pos).method = UITweener.Method.EaseInOut;
        this.dropObject.Init(PartyOrganizationListViewDropObject.InitMode.DRAG_MOVE, new System.Action(this.DragSwapMoveEnd), 0f, mListViewObject.transform.position);
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        Debug.Log("DragDrop surface " + surface);
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        PartyOrganizationListViewManager manager = mListViewObject.Manager as PartyOrganizationListViewManager;
        PartyListViewItem partyItem = manager.GetPartyItem();
        PartyOrganizationListViewItem item = mListViewObject.GetItem();
        if ((surface != null) && (item != null))
        {
            PartyOrganizationUIDragDropListViewSurface component = surface.GetComponent<PartyOrganizationUIDragDropListViewSurface>();
            if (component != null)
            {
                ListViewDropInfo info = new ListViewDropInfo(base.mListViewObject.gameObject, surface);
                if (manager.IsDropDropSurface(info))
                {
                    PartyOrganizationListViewDropObject dropObject = component.DropObject;
                    if (dropObject != null)
                    {
                        PartyOrganizationListViewItem item3 = dropObject.GetItem();
                        if (item3 == null)
                        {
                            base.OnDragDropRelease(surface);
                            mListViewObject.Init(PartyOrganizationListViewDropObject.InitMode.INVISIBLE);
                            mListViewObject.SetItem(null);
                            dropObject.SetItem(item);
                            dropObject.Init(PartyOrganizationListViewDropObject.InitMode.INPUT);
                            this.DragEnd();
                            return;
                        }
                        if (((item3 != item) && ((item.Index != 0) || (!item3.IsFollower && !item3.IsEventJoin))) && ((item3.Index != 0) || (!item.IsFollower && !item.IsEventJoin)))
                        {
                            this.DragSwapStart(dropObject);
                            return;
                        }
                    }
                    else
                    {
                        this.DragDumpStart();
                        return;
                    }
                }
            }
        }
        this.DragReturnStart();
    }

    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        PartyOrganizationListViewDropObject mListViewObject = base.mListViewObject as PartyOrganizationListViewDropObject;
        mListViewObject.Init(PartyOrganizationListViewDropObject.InitMode.DRAG_INVISIBLE);
        mListViewObject.Manager.ItemDragStart();
    }
}

