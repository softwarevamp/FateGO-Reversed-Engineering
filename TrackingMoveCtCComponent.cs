using System;
using UnityEngine;

public class TrackingMoveCtCComponent : MonoBehaviour
{
    public Vector3 addpos = Vector3.zero;
    public Camera after;
    public Camera before;
    public GameObject targetObject;
    private bool updateFlg;

    public void Set(Camera a, Camera b, GameObject c, Vector3 d)
    {
        this.before = a;
        this.after = b;
        this.targetObject = c;
        this.addpos = d;
    }

    public void startAct()
    {
        this.updateFlg = true;
        this.upDatePos();
    }

    public void stopAct()
    {
        this.updateFlg = false;
    }

    private void Update()
    {
        if (this.updateFlg)
        {
            this.upDatePos();
        }
    }

    private void upDatePos()
    {
        if (this.targetObject != null)
        {
            Vector3 position = this.before.WorldToViewportPoint(this.targetObject.transform.position + this.addpos);
            Vector3 vector2 = this.after.ViewportToWorldPoint(position);
            base.transform.position = vector2;
        }
    }
}

