using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Root"), ExecuteInEditMode]
public class UIRoot : MonoBehaviour
{
    public bool adjustByDPI;
    public bool fitHeight = true;
    public bool fitWidth;
    public static List<UIRoot> list = new List<UIRoot>();
    public int manualHeight = 720;
    public int manualWidth = 0x500;
    public int maximumHeight = 0x600;
    public int minimumHeight = 320;
    private Transform mTrans;
    public Scaling scalingStyle;
    public bool shrinkPortraitUI;

    protected virtual void Awake()
    {
        this.mTrans = base.transform;
    }

    public static void Broadcast(string funcName)
    {
        int num = 0;
        int count = list.Count;
        while (num < count)
        {
            UIRoot root = list[num];
            if (root != null)
            {
                root.BroadcastMessage(funcName, SendMessageOptions.DontRequireReceiver);
            }
            num++;
        }
    }

    public static void Broadcast(string funcName, object param)
    {
        if (param == null)
        {
            Debug.LogError("SendMessage is bugged when you try to pass 'null' in the parameter field. It behaves as if no parameter was specified.");
        }
        else
        {
            int num = 0;
            int count = list.Count;
            while (num < count)
            {
                UIRoot root = list[num];
                if (root != null)
                {
                    root.BroadcastMessage(funcName, param, SendMessageOptions.DontRequireReceiver);
                }
                num++;
            }
        }
    }

    public float GetPixelSizeAdjustment(int height)
    {
        height = Mathf.Max(2, height);
        if (this.activeScaling == Scaling.Constrained)
        {
            return (((float) this.activeHeight) / ((float) height));
        }
        if (height < this.minimumHeight)
        {
            return (((float) this.minimumHeight) / ((float) height));
        }
        if (height > this.maximumHeight)
        {
            return (((float) this.maximumHeight) / ((float) height));
        }
        return 1f;
    }

    public static float GetPixelSizeAdjustment(GameObject go)
    {
        UIRoot root = NGUITools.FindInParents<UIRoot>(go);
        return ((root == null) ? 1f : root.pixelSizeAdjustment);
    }

    protected virtual void OnDisable()
    {
        list.Remove(this);
    }

    protected virtual void OnEnable()
    {
        list.Add(this);
    }

    protected virtual void Start()
    {
        UIOrthoCamera componentInChildren = base.GetComponentInChildren<UIOrthoCamera>();
        if (componentInChildren != null)
        {
            Debug.LogWarning("UIRoot should not be active at the same time as UIOrthoCamera. Disabling UIOrthoCamera.", componentInChildren);
            Camera component = componentInChildren.gameObject.GetComponent<Camera>();
            componentInChildren.enabled = false;
            if (component != null)
            {
                component.orthographicSize = 1f;
            }
        }
        else
        {
            this.Update();
        }
    }

    private void Update()
    {
        if (this.mTrans != null)
        {
            float activeHeight = this.activeHeight;
            if (activeHeight > 0f)
            {
                float x = 2f / activeHeight;
                Vector3 localScale = this.mTrans.localScale;
                if (((Mathf.Abs((float) (localScale.x - x)) > float.Epsilon) || (Mathf.Abs((float) (localScale.y - x)) > float.Epsilon)) || (Mathf.Abs((float) (localScale.z - x)) > float.Epsilon))
                {
                    this.mTrans.localScale = new Vector3(x, x, x);
                }
            }
        }
    }

    public int activeHeight
    {
        get
        {
            if (this.activeScaling == Scaling.Flexible)
            {
                Vector2 screenSize = NGUITools.screenSize;
                float num = screenSize.x / screenSize.y;
                if (screenSize.y < this.minimumHeight)
                {
                    screenSize.y = this.minimumHeight;
                    screenSize.x = screenSize.y * num;
                }
                else if (screenSize.y > this.maximumHeight)
                {
                    screenSize.y = this.maximumHeight;
                    screenSize.x = screenSize.y * num;
                }
                int num2 = Mathf.RoundToInt((!this.shrinkPortraitUI || (screenSize.y <= screenSize.x)) ? screenSize.y : (screenSize.y / num));
                return (!this.adjustByDPI ? num2 : NGUIMath.AdjustByDPI((float) num2));
            }
            Constraint constraint = this.constraint;
            if (constraint != Constraint.FitHeight)
            {
                Vector2 vector2 = NGUITools.screenSize;
                float num3 = vector2.x / vector2.y;
                float num4 = ((float) this.manualWidth) / ((float) this.manualHeight);
                switch (constraint)
                {
                    case Constraint.Fit:
                        return ((num4 <= num3) ? this.manualHeight : Mathf.RoundToInt(((float) this.manualWidth) / num3));

                    case Constraint.Fill:
                        return ((num4 >= num3) ? this.manualHeight : Mathf.RoundToInt(((float) this.manualWidth) / num3));

                    case Constraint.FitWidth:
                        return Mathf.RoundToInt(((float) this.manualWidth) / num3);
                }
            }
            return this.manualHeight;
        }
    }

    public Scaling activeScaling
    {
        get
        {
            Scaling scalingStyle = this.scalingStyle;
            if (scalingStyle == Scaling.ConstrainedOnMobiles)
            {
                return Scaling.Constrained;
            }
            return scalingStyle;
        }
    }

    public Constraint constraint
    {
        get
        {
            if (this.fitWidth)
            {
                if (this.fitHeight)
                {
                    return Constraint.Fit;
                }
                return Constraint.FitWidth;
            }
            if (this.fitHeight)
            {
                return Constraint.FitHeight;
            }
            return Constraint.Fill;
        }
    }

    public float pixelSizeAdjustment
    {
        get
        {
            int height = Mathf.RoundToInt(NGUITools.screenSize.y);
            return ((height != -1) ? this.GetPixelSizeAdjustment(height) : 1f);
        }
    }

    public enum Constraint
    {
        Fit,
        Fill,
        FitWidth,
        FitHeight
    }

    public enum Scaling
    {
        Flexible,
        Constrained,
        ConstrainedOnMobiles
    }
}

