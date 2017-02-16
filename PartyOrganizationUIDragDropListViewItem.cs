using System;
using UnityEngine;

public class PartyOrganizationUIDragDropListViewItem : UIDragDropListViewItem
{
    protected static float ACTION_TIME = 0.2f;

    protected void DragEnd()
    {
        PartyOrganizationListViewObject mListViewObject = base.mListViewObject as PartyOrganizationListViewObject;
        (mListViewObject.Manager as PartyOrganizationListViewManager).ItemDragEnd();
    }

    protected void DragReturnEnd()
    {
        PartyOrganizationListViewObject mListViewObject = base.mListViewObject as PartyOrganizationListViewObject;
        base.OnDragDropRelease(null);
        mListViewObject.Init(PartyOrganizationListViewObject.InitMode.INPUT);
        base.Invoke("DragEnd", 0.1f);
    }

    protected void DragReturnStart()
    {
        PartyOrganizationListViewObject mListViewObject = base.mListViewObject as PartyOrganizationListViewObject;
        Vector3 pos = base.dragObject.transform.parent.InverseTransformPoint(mListViewObject.transform.position);
        TweenPosition position = TweenPosition.Begin(base.dragObject, ACTION_TIME, pos);
        position.method = UITweener.Method.EaseInOut;
        position.eventReceiver = base.gameObject;
        position.callWhenFinished = "DragReturnEnd";
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        Debug.Log("DragDrop surface " + surface);
        PartyOrganizationListViewObject mListViewObject = base.mListViewObject as PartyOrganizationListViewObject;
        PartyOrganizationListViewItem item = mListViewObject.GetItem();
        PartyOrganizationListViewManager manager = mListViewObject.Manager as PartyOrganizationListViewManager;
        if ((surface != null) && (item != null))
        {
            PartyOrganizationUIDragDropListViewSurface component = surface.GetComponent<PartyOrganizationUIDragDropListViewSurface>();
            if (component != null)
            {
                ListViewDropInfo info = new ListViewDropInfo(base.mListViewObject.gameObject, surface);
                if (manager.IsItemDropSurface(info))
                {
                    PartyOrganizationListViewDropObject dropObject = component.DropObject;
                    if (dropObject != null)
                    {
                        PartyOrganizationListViewItem item2 = dropObject.GetItem();
                        if (item2 != null)
                        {
                            PartyOrganizationListViewObject viewObject = item2.ViewObject as PartyOrganizationListViewObject;
                            if (viewObject != null)
                            {
                                viewObject.Init(PartyOrganizationListViewObject.InitMode.INPUT);
                            }
                        }
                        base.OnDragDropRelease(surface);
                        mListViewObject.Init(PartyOrganizationListViewObject.InitMode.INVALID);
                        dropObject.SetItem(item);
                        dropObject.Init(PartyOrganizationListViewDropObject.InitMode.INPUT);
                        base.Invoke("DragEnd", 0.1f);
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
        PartyOrganizationListViewObject mListViewObject = base.mListViewObject as PartyOrganizationListViewObject;
        mListViewObject.Init(PartyOrganizationListViewObject.InitMode.INVALID);
        mListViewObject.Manager.ItemDragStart();
    }
}

