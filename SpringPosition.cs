using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Spring Position")]
public class SpringPosition : MonoBehaviour
{
    [HideInInspector, SerializeField]
    public string callWhenFinished;
    public static SpringPosition current;
    [HideInInspector, SerializeField]
    private GameObject eventReceiver;
    public bool ignoreTimeScale;
    private UIScrollView mSv;
    private float mThreshold;
    private Transform mTrans;
    public OnFinished onFinished;
    public float strength = 10f;
    public Vector3 target = Vector3.zero;
    public bool updateScrollView;
    public bool worldSpace;

    public static SpringPosition Begin(GameObject go, Vector3 pos, float strength)
    {
        SpringPosition component = go.GetComponent<SpringPosition>();
        if (component == null)
        {
            component = go.AddComponent<SpringPosition>();
        }
        component.target = pos;
        component.strength = strength;
        component.onFinished = null;
        if (!component.enabled)
        {
            component.mThreshold = 0f;
            component.enabled = true;
        }
        return component;
    }

    private void NotifyListeners()
    {
        current = this;
        if (this.onFinished != null)
        {
            this.onFinished();
        }
        if ((this.eventReceiver != null) && !string.IsNullOrEmpty(this.callWhenFinished))
        {
            this.eventReceiver.SendMessage(this.callWhenFinished, this, SendMessageOptions.DontRequireReceiver);
        }
        current = null;
    }

    private void Start()
    {
        this.mTrans = base.transform;
        if (this.updateScrollView)
        {
            this.mSv = NGUITools.FindInParents<UIScrollView>(base.gameObject);
        }
    }

    private void Update()
    {
        float deltaTime = !this.ignoreTimeScale ? Time.deltaTime : RealTime.deltaTime;
        if (this.worldSpace)
        {
            if (this.mThreshold == 0f)
            {
                Vector3 vector = this.target - this.mTrans.position;
                this.mThreshold = vector.sqrMagnitude * 0.001f;
            }
            this.mTrans.position = NGUIMath.SpringLerp(this.mTrans.position, this.target, this.strength, deltaTime);
            Vector3 vector2 = this.target - this.mTrans.position;
            if (this.mThreshold >= vector2.sqrMagnitude)
            {
                this.mTrans.position = this.target;
                this.NotifyListeners();
                base.enabled = false;
            }
        }
        else
        {
            if (this.mThreshold == 0f)
            {
                Vector3 vector3 = this.target - this.mTrans.localPosition;
                this.mThreshold = vector3.sqrMagnitude * 1E-05f;
            }
            this.mTrans.localPosition = NGUIMath.SpringLerp(this.mTrans.localPosition, this.target, this.strength, deltaTime);
            Vector3 vector4 = this.target - this.mTrans.localPosition;
            if (this.mThreshold >= vector4.sqrMagnitude)
            {
                this.mTrans.localPosition = this.target;
                this.NotifyListeners();
                base.enabled = false;
            }
        }
        if (this.mSv != null)
        {
            this.mSv.UpdateScrollbars(true);
        }
    }

    public delegate void OnFinished();
}

