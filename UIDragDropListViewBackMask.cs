using System;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/UIDragDropListViewBackMask")]
public class UIDragDropListViewBackMask : MonoBehaviour
{
    [SerializeField]
    protected GameObject maskObject;

    public void DragEnd()
    {
        this.maskObject.GetComponent<Collider>().enabled = false;
    }

    public void DragEnd(EventDelegate.Callback call)
    {
        this.maskObject.GetComponent<Collider>().enabled = false;
        this.maskObject.GetComponent<UITouchPress>().onClick.Remove(new EventDelegate(call));
    }

    public void DragStart()
    {
        this.maskObject.GetComponent<Collider>().enabled = true;
    }

    public void DragStart(EventDelegate.Callback call)
    {
        this.maskObject.GetComponent<Collider>().enabled = true;
        this.maskObject.GetComponent<UITouchPress>().onClick.Add(new EventDelegate(call));
    }
}

