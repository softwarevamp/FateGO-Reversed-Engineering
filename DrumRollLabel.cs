using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class DrumRollLabel : MonoBehaviour
{
    public float changetime = 0.3f;
    public DigitRollLabel[] diglabel;
    public UILabel label;
    private int nextvalue;
    private int nowvalue;

    private event CompleteEventHandler callComplete;

    public void changeParam(int nextparam, CompleteEventHandler callevent = null)
    {
        iTween component = base.gameObject.GetComponent<iTween>();
        if (component != null)
        {
            UnityEngine.Object.Destroy(component);
        }
        object[] args = new object[] { "from", this.nowvalue, "to", nextparam, "onupdate", "updateValues", "oncomplete", "completeValues", "time", this.changetime };
        iTween.ValueTo(base.gameObject, iTween.Hash(args));
        this.nextvalue = nextparam;
        this.callComplete = callevent;
        for (int i = 0; i < this.diglabel.Length; i++)
        {
            this.diglabel[i].changeSpeed(this.diglabel.Length - i);
        }
    }

    public void completeValues()
    {
        string str = string.Format("{0,3}", this.nextvalue);
        for (int i = 0; i < this.diglabel.Length; i++)
        {
            this.diglabel[i].endChange(str.Substring((this.diglabel.Length - 1) - i, 1));
        }
        this.nowvalue = this.nextvalue;
        if (this.callComplete != null)
        {
            this.callComplete();
        }
    }

    public int getCount() => 
        this.nowvalue;

    public void setParam(int param)
    {
        this.nowvalue = param;
        this.nextvalue = param;
    }

    public void updateValues(int value)
    {
        string str = string.Format("{0," + this.diglabel.Length + "}", value);
        this.nowvalue = value;
        for (int i = 0; i < this.diglabel.Length; i++)
        {
            string text = str.Substring((this.diglabel.Length - 1) - i, 1);
            this.diglabel[i].startChange(text);
        }
    }

    public delegate void CompleteEventHandler();
}

