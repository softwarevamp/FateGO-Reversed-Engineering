using System;
using System.Collections;
using UnityEngine;

public class MoveLabelTextControl : MonoBehaviour
{
    private Vector3 lbStartPosition;
    private Vector2 parentSize;
    private Hashtable table = new Hashtable();
    private UILabel targetLb;

    private void onMoveComplete()
    {
        this.targetLb.transform.localPosition = this.lbStartPosition;
        Debug.Log("!!!!! currentInfoLb Text start_x: " + this.targetLb.transform.localPosition.x);
    }

    public void setMoveTextOver(UIPanel parent, UILabel targetLb)
    {
        iTween.Stop();
        this.targetLb = targetLb;
        int length = targetLb.text.Length;
        int fontSize = targetLb.fontSize;
        int num3 = length * fontSize;
        Debug.Log("!!!!! currentInfoLb Label Size: " + targetLb.width);
        Debug.Log(string.Concat(new object[] { "!!!!! currentInfoLb Text Size: ", length, " _FontSize: ", fontSize }));
        Debug.Log("!!!!! currentInfoLb Text Real Size: " + num3);
        this.parentSize = parent.GetViewSize();
        this.lbStartPosition = targetLb.transform.localPosition;
        Debug.Log("!!!!! currentInfoLb lbStartPosition: " + this.lbStartPosition);
        if (targetLb.transform.localPosition.x != this.lbStartPosition.x)
        {
            targetLb.transform.localPosition = this.lbStartPosition;
        }
        if (num3 > this.parentSize.x)
        {
            targetLb.AssumeNaturalSize();
            GameObject gameObject = targetLb.gameObject;
            float x = targetLb.transform.localPosition.x;
            Debug.Log("!!!!! currentInfoLb Text lb_x: " + x);
            float num6 = num3;
            Debug.Log("!!!!! currentInfoLb Text lb_size: " + num6);
            float num7 = x + -num6;
            Debug.Log("!!!!! currentInfoLb Text move_x: " + num7);
            this.table.Clear();
            this.table.Add("isLocal", true);
            this.table.Add("x", num7);
            this.table.Add("oncomplete", "onMoveComplete");
            this.table.Add("oncompletetarget", base.gameObject);
            this.table.Add("easetype", "linear");
            this.table.Add("time", 10f);
            this.table.Add("delay", 2f);
            this.table.Add("looptype", iTween.LoopType.loop);
            iTween.MoveTo(gameObject, this.table);
        }
    }
}

