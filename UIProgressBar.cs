using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/NGUI Progress Bar")]
public class UIProgressBar : UIWidgetContainer
{
    public static UIProgressBar current;
    [SerializeField, HideInInspector]
    protected UIWidget mBG;
    protected Camera mCam;
    [HideInInspector, SerializeField]
    protected UIWidget mFG;
    [SerializeField, HideInInspector]
    protected FillDirection mFill;
    protected bool mIsDirty;
    protected float mOffset;
    protected Transform mTrans;
    [HideInInspector, SerializeField]
    protected float mValue = 1f;
    public int numberOfSteps;
    public List<EventDelegate> onChange = new List<EventDelegate>();
    public OnDragFinished onDragFinished;
    public Transform thumb;

    public virtual void ForceUpdate()
    {
        this.mIsDirty = false;
        bool flag = false;
        if (this.mFG != null)
        {
            UIBasicSprite mFG = this.mFG as UIBasicSprite;
            if (this.isHorizontal)
            {
                if ((mFG != null) && (mFG.type == UIBasicSprite.Type.Filled))
                {
                    if ((mFG.fillDirection == UIBasicSprite.FillDirection.Horizontal) || (mFG.fillDirection == UIBasicSprite.FillDirection.Vertical))
                    {
                        mFG.fillDirection = UIBasicSprite.FillDirection.Horizontal;
                        mFG.invert = this.isInverted;
                    }
                    mFG.fillAmount = this.value;
                }
                else
                {
                    this.mFG.drawRegion = !this.isInverted ? new Vector4(0f, 0f, this.value, 1f) : new Vector4(1f - this.value, 0f, 1f, 1f);
                    this.mFG.enabled = true;
                    flag = this.value < 0.001f;
                }
            }
            else if ((mFG != null) && (mFG.type == UIBasicSprite.Type.Filled))
            {
                if ((mFG.fillDirection == UIBasicSprite.FillDirection.Horizontal) || (mFG.fillDirection == UIBasicSprite.FillDirection.Vertical))
                {
                    mFG.fillDirection = UIBasicSprite.FillDirection.Vertical;
                    mFG.invert = this.isInverted;
                }
                mFG.fillAmount = this.value;
            }
            else
            {
                this.mFG.drawRegion = !this.isInverted ? new Vector4(0f, 0f, 1f, this.value) : new Vector4(0f, 1f - this.value, 1f, 1f);
                this.mFG.enabled = true;
                flag = this.value < 0.001f;
            }
        }
        if ((this.thumb != null) && ((this.mFG != null) || (this.mBG != null)))
        {
            Vector3[] vectorArray = (this.mFG == null) ? this.mBG.localCorners : this.mFG.localCorners;
            Vector4 vector = (this.mFG == null) ? this.mBG.border : this.mFG.border;
            vectorArray[0].x += vector.x;
            vectorArray[1].x += vector.x;
            vectorArray[2].x -= vector.z;
            vectorArray[3].x -= vector.z;
            vectorArray[0].y += vector.y;
            vectorArray[1].y -= vector.w;
            vectorArray[2].y -= vector.w;
            vectorArray[3].y += vector.y;
            Transform transform = (this.mFG == null) ? this.mBG.cachedTransform : this.mFG.cachedTransform;
            for (int i = 0; i < 4; i++)
            {
                vectorArray[i] = transform.TransformPoint(vectorArray[i]);
            }
            if (this.isHorizontal)
            {
                Vector3 a = Vector3.Lerp(vectorArray[0], vectorArray[1], 0.5f);
                Vector3 b = Vector3.Lerp(vectorArray[2], vectorArray[3], 0.5f);
                this.SetThumbPosition(Vector3.Lerp(a, b, !this.isInverted ? this.value : (1f - this.value)));
            }
            else
            {
                Vector3 vector4 = Vector3.Lerp(vectorArray[0], vectorArray[3], 0.5f);
                Vector3 vector5 = Vector3.Lerp(vectorArray[1], vectorArray[2], 0.5f);
                this.SetThumbPosition(Vector3.Lerp(vector4, vector5, !this.isInverted ? this.value : (1f - this.value)));
            }
        }
        if (flag)
        {
            this.mFG.enabled = false;
        }
    }

    protected virtual float LocalToValue(Vector2 localPos)
    {
        if (this.mFG == null)
        {
            return this.value;
        }
        Vector3[] localCorners = this.mFG.localCorners;
        Vector3 vector = localCorners[2] - localCorners[0];
        if (this.isHorizontal)
        {
            float num = (localPos.x - localCorners[0].x) / vector.x;
            return (!this.isInverted ? num : (1f - num));
        }
        float num2 = (localPos.y - localCorners[0].y) / vector.y;
        return (!this.isInverted ? num2 : (1f - num2));
    }

    protected virtual void OnStart()
    {
    }

    protected void OnValidate()
    {
        if (NGUITools.GetActive(this))
        {
            this.Upgrade();
            this.mIsDirty = true;
            float num = Mathf.Clamp01(this.mValue);
            if (this.mValue != num)
            {
                this.mValue = num;
            }
            if (this.numberOfSteps < 0)
            {
                this.numberOfSteps = 0;
            }
            else if (this.numberOfSteps > 20)
            {
                this.numberOfSteps = 20;
            }
            this.ForceUpdate();
        }
        else
        {
            float num2 = Mathf.Clamp01(this.mValue);
            if (this.mValue != num2)
            {
                this.mValue = num2;
            }
            if (this.numberOfSteps < 0)
            {
                this.numberOfSteps = 0;
            }
            else if (this.numberOfSteps > 20)
            {
                this.numberOfSteps = 20;
            }
        }
    }

    protected float ScreenToValue(Vector2 screenPos)
    {
        float num;
        Transform cachedTransform = this.cachedTransform;
        Plane plane = new Plane((Vector3) (cachedTransform.rotation * Vector3.back), cachedTransform.position);
        Ray ray = this.cachedCamera.ScreenPointToRay((Vector3) screenPos);
        if (!plane.Raycast(ray, out num))
        {
            return this.value;
        }
        return this.LocalToValue(cachedTransform.InverseTransformPoint(ray.GetPoint(num)));
    }

    protected void SetThumbPosition(Vector3 worldPos)
    {
        Transform parent = this.thumb.parent;
        if (parent != null)
        {
            worldPos = parent.InverseTransformPoint(worldPos);
            worldPos.x = Mathf.Round(worldPos.x);
            worldPos.y = Mathf.Round(worldPos.y);
            worldPos.z = 0f;
            if (Vector3.Distance(this.thumb.localPosition, worldPos) > 0.001f)
            {
                this.thumb.localPosition = worldPos;
            }
        }
        else if (Vector3.Distance(this.thumb.position, worldPos) > 1E-05f)
        {
            this.thumb.position = worldPos;
        }
    }

    protected void Start()
    {
        this.Upgrade();
        if (Application.isPlaying)
        {
            if (this.mBG != null)
            {
                this.mBG.autoResizeBoxCollider = true;
            }
            this.OnStart();
            if ((current == null) && (this.onChange != null))
            {
                current = this;
                EventDelegate.Execute(this.onChange);
                current = null;
            }
        }
        this.ForceUpdate();
    }

    protected void Update()
    {
        if (this.mIsDirty)
        {
            this.ForceUpdate();
        }
    }

    protected virtual void Upgrade()
    {
    }

    public float alpha
    {
        get
        {
            if (this.mFG != null)
            {
                return this.mFG.alpha;
            }
            if (this.mBG != null)
            {
                return this.mBG.alpha;
            }
            return 1f;
        }
        set
        {
            if (this.mFG != null)
            {
                this.mFG.alpha = value;
                if (this.mFG.GetComponent<Collider>() != null)
                {
                    this.mFG.GetComponent<Collider>().enabled = this.mFG.alpha > 0.001f;
                }
                else if (this.mFG.GetComponent<Collider2D>() != null)
                {
                    this.mFG.GetComponent<Collider2D>().enabled = this.mFG.alpha > 0.001f;
                }
            }
            if (this.mBG != null)
            {
                this.mBG.alpha = value;
                if (this.mBG.GetComponent<Collider>() != null)
                {
                    this.mBG.GetComponent<Collider>().enabled = this.mBG.alpha > 0.001f;
                }
                else if (this.mBG.GetComponent<Collider2D>() != null)
                {
                    this.mBG.GetComponent<Collider2D>().enabled = this.mBG.alpha > 0.001f;
                }
            }
            if (this.thumb != null)
            {
                UIWidget component = this.thumb.GetComponent<UIWidget>();
                if (component != null)
                {
                    component.alpha = value;
                    if (component.GetComponent<Collider>() != null)
                    {
                        component.GetComponent<Collider>().enabled = component.alpha > 0.001f;
                    }
                    else if (component.GetComponent<Collider2D>() != null)
                    {
                        component.GetComponent<Collider2D>().enabled = component.alpha > 0.001f;
                    }
                }
            }
        }
    }

    public UIWidget backgroundWidget
    {
        get => 
            this.mBG;
        set
        {
            if (this.mBG != value)
            {
                this.mBG = value;
                this.mIsDirty = true;
            }
        }
    }

    public Camera cachedCamera
    {
        get
        {
            if (this.mCam == null)
            {
                this.mCam = NGUITools.FindCameraForLayer(base.gameObject.layer);
            }
            return this.mCam;
        }
    }

    public Transform cachedTransform
    {
        get
        {
            if (this.mTrans == null)
            {
                this.mTrans = base.transform;
            }
            return this.mTrans;
        }
    }

    public FillDirection fillDirection
    {
        get => 
            this.mFill;
        set
        {
            if (this.mFill != value)
            {
                this.mFill = value;
                this.ForceUpdate();
            }
        }
    }

    public UIWidget foregroundWidget
    {
        get => 
            this.mFG;
        set
        {
            if (this.mFG != value)
            {
                this.mFG = value;
                this.mIsDirty = true;
            }
        }
    }

    protected bool isHorizontal =>
        ((this.mFill == FillDirection.LeftToRight) || (this.mFill == FillDirection.RightToLeft));

    protected bool isInverted =>
        ((this.mFill == FillDirection.RightToLeft) || (this.mFill == FillDirection.TopToBottom));

    public float value
    {
        get
        {
            if (this.numberOfSteps > 1)
            {
                return (Mathf.Round(this.mValue * (this.numberOfSteps - 1)) / ((float) (this.numberOfSteps - 1)));
            }
            return this.mValue;
        }
        set
        {
            float num = Mathf.Clamp01(value);
            if (this.mValue != num)
            {
                float num2 = this.value;
                this.mValue = num;
                if (num2 != this.value)
                {
                    this.ForceUpdate();
                    if (((current == null) && NGUITools.GetActive(this)) && EventDelegate.IsValid(this.onChange))
                    {
                        current = this;
                        EventDelegate.Execute(this.onChange);
                        current = null;
                    }
                }
            }
        }
    }

    public enum FillDirection
    {
        LeftToRight,
        RightToLeft,
        BottomToTop,
        TopToBottom
    }

    public delegate void OnDragFinished();
}

