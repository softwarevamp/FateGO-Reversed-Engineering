using System;
using System.Collections;
using UnityEngine;

public class BattleCutInComponent : BaseMonoBehaviour
{
    private Vector3 endTr;
    public UILabel label;
    public UISprite sprite;
    private Vector3 startTr;
    private Hashtable table = new Hashtable();
    private float totaltime;

    public void onEndComp()
    {
        UnityEngine.Object.Destroy(base.gameObject);
    }

    public void onStartComp()
    {
        this.table.Clear();
        this.table.Add("delay", 1f);
        this.table.Add("position", this.startTr);
        this.table.Add("oncomplete", "onEndComp");
        this.table.Add("time", this.totaltime);
        iTween.MoveTo(base.gameObject, this.table);
    }

    public void setData(int Id, int type, Vector3 startt, Vector3 endt, float time, string param)
    {
        this.startTr = startt;
        this.endTr = endt;
        this.totaltime = time;
        if (type == 1)
        {
            this.sprite.transform.localScale = new Vector3(-1f, 1f);
        }
        if (this.label != null)
        {
            this.label.text = param;
        }
    }

    public void startAction()
    {
        this.table.Clear();
        this.table.Add("position", this.endTr);
        this.table.Add("oncomplete", "onStartComp");
        this.table.Add("time", this.totaltime);
        iTween.MoveTo(base.gameObject, this.table);
    }
}

