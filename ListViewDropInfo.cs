using System;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/ListViewDropInfo")]
public class ListViewDropInfo
{
    protected GameObject dropSurfaceObject;
    protected GameObject listViewItemObject;

    public ListViewDropInfo(GameObject listViewItemObject, GameObject dropSurfaceObject)
    {
        this.listViewItemObject = listViewItemObject;
        this.dropSurfaceObject = dropSurfaceObject;
    }

    public void SendMessage(string methodName)
    {
        this.listViewItemObject.GetComponent<ListViewObject>().SendMessage(methodName, this);
    }

    public void SendMessageOnDropItem()
    {
        this.listViewItemObject.GetComponent<ListViewObject>().SendMessage("OnDropItem", this);
    }

    public GameObject DropSurfaceObject =>
        this.dropSurfaceObject;

    public ListViewItem ListViewItem
    {
        get
        {
            if (this.listViewItemObject != null)
            {
                ListViewObject component = this.listViewItemObject.GetComponent<ListViewObject>();
                if (component != null)
                {
                    return component.GetItem();
                }
            }
            return null;
        }
    }

    public GameObject ListViewItemObject =>
        this.listViewItemObject;

    public ListViewManager ListViewManager
    {
        get
        {
            if (this.listViewItemObject != null)
            {
                ListViewObject component = this.listViewItemObject.GetComponent<ListViewObject>();
                if (component != null)
                {
                    return component.Manager;
                }
            }
            return null;
        }
    }

    public ListViewObject ListViewObject
    {
        get
        {
            if (this.listViewItemObject != null)
            {
                return this.listViewItemObject.GetComponent<ListViewObject>();
            }
            return null;
        }
    }
}

