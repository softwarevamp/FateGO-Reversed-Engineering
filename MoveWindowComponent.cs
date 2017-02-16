using System;
using System.Collections;
using UnityEngine;

public class MoveWindowComponent : BaseMonoBehaviour
{
    public Vector3 closeposition;
    public bool closepositionadjust = true;
    public Collider[] contentscolliderlist;
    public bool flgopen;
    public Vector3 openposition;
    private Hashtable table = new Hashtable();

    public void Awake()
    {
        this.setContentsCollider(false);
        if (this.closepositionadjust)
        {
            this.closeposition = base.gameObject.transform.localPosition;
        }
    }

    public void closeWindow()
    {
        this.setContentsCollider(false);
        this.table.Clear();
        this.table.Add("isLocal", true);
        this.table.Add("position", this.closeposition);
        this.table.Add("oncomplete", "onCloseComplete");
        this.table.Add("time", 1f);
        iTween.MoveTo(base.gameObject, this.table);
    }

    public void onCloseComplete()
    {
        this.flgopen = false;
    }

    public void onOpenComplete()
    {
        this.setContentsCollider(true);
        this.flgopen = true;
    }

    public void openWindow()
    {
        this.table.Clear();
        this.table.Add("isLocal", true);
        this.table.Add("position", this.openposition);
        this.table.Add("oncomplete", "onOpenComplete");
        this.table.Add("time", 1f);
        iTween.MoveTo(base.gameObject, this.table);
    }

    public void setContentsCollider(bool flg)
    {
        if (this.contentscolliderlist != null)
        {
            for (int i = 0; i < this.contentscolliderlist.Length; i++)
            {
                this.contentscolliderlist[i].enabled = flg;
            }
        }
    }

    public void tglWindow()
    {
        if (this.flgopen)
        {
            this.closeWindow();
        }
        else
        {
            this.openWindow();
        }
    }
}

