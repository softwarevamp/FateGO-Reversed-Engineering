using System;
using UnityEngine;

public class UIDragDropListViewSurface : MonoBehaviour
{
    public void DragEnd()
    {
        base.GetComponent<Collider>().enabled = false;
    }

    public void DragStart()
    {
        base.GetComponent<Collider>().enabled = true;
    }
}

