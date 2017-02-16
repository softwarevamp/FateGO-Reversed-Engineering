using System;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/ListViewDropObject")]
public class ListViewDropObject : ListViewObject
{
    public override bool ClearItem()
    {
        if (!base.isBusy)
        {
            base.linkItem = null;
            base.SetVisible(false);
            this.SetInput(false);
            return true;
        }
        return false;
    }

    public override GameObject CreateDragObject()
    {
        GameObject parent = (UIDragDropRoot.root == null) ? base.transform.parent.gameObject : UIDragDropRoot.root.gameObject;
        GameObject obj3 = NGUITools.AddChild(parent, base.dragObjectPrefab);
        ListViewDropObject component = obj3.GetComponent<ListViewDropObject>();
        component.linkItem = base.linkItem;
        component.transform.position = base.transform.position;
        component.transform.eulerAngles = base.transform.eulerAngles;
        component.transform.localScale = Vector3.one;
        Vector3 position = base.transform.TransformPoint(1f, 1f, 0f);
        Vector3 vector2 = component.transform.InverseTransformPoint(position);
        vector2.z = 1f;
        component.transform.localScale = vector2;
        component.gameObject.layer = parent.layer;
        Vector3 localPosition = component.transform.localPosition;
        localPosition.z = 0f;
        component.transform.localPosition = localPosition;
        component.SetBaseTransform();
        component.SetVisible(true);
        component.SetInput(false);
        return obj3;
    }

    public void ReleaseItem()
    {
        if (base.linkItem != null)
        {
            base.linkItem = null;
            base.SetVisible(false);
            this.SetInput(false);
        }
    }

    public override void SetItem(ListViewItem item)
    {
        this.SetItem(item, null);
    }

    public override void SetItem(ListViewItem item, ListViewItemSeed seed)
    {
        base.linkItem = item;
        base.SetVisible(false);
        this.SetInput(false);
        base.gameObject.SendMessage("SetBaseTransform");
    }
}

