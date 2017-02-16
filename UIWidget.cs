using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/NGUI Widget")]
public class UIWidget : UIRect
{
    public float aspectRatio = 1f;
    public bool autoResizeBoxCollider;
    [NonSerialized]
    public UIDrawCall drawCall;
    [NonSerialized]
    public bool fillGeometry = true;
    [NonSerialized]
    public UIGeometry geometry = new UIGeometry();
    public bool hideIfOffScreen;
    public HitCheck hitCheck;
    public AspectRatioSource keepAspectRatio;
    [NonSerialized]
    private int mAlphaFrameID = -1;
    [HideInInspector, SerializeField]
    protected Color mColor = Color.white;
    [NonSerialized]
    protected Vector3[] mCorners = new Vector3[4];
    [HideInInspector, SerializeField]
    protected int mDepth;
    [NonSerialized]
    protected Vector4 mDrawRegion = new Vector4(0f, 0f, 1f, 1f);
    [HideInInspector, SerializeField]
    protected int mHeight = 100;
    [NonSerialized]
    private bool mIsInFront = true;
    [NonSerialized]
    private bool mIsVisibleByAlpha = true;
    [NonSerialized]
    private bool mIsVisibleByPanel = true;
    [NonSerialized]
    private float mLastAlpha;
    [NonSerialized]
    private Matrix4x4 mLocalToPanel;
    private int mMatrixFrame = -1;
    [NonSerialized]
    private bool mMoved;
    private Vector3 mOldV0;
    private Vector3 mOldV1;
    public UIDrawCall.OnRenderCallback mOnRender;
    [SerializeField, HideInInspector]
    protected Pivot mPivot = Pivot.Center;
    [NonSerialized]
    protected bool mPlayMode = true;
    [HideInInspector, SerializeField]
    protected int mWidth = 100;
    public OnDimensionsChanged onChange;
    public OnPostFillCallback onPostFill;
    [NonSerialized]
    public UIPanel panel;

    protected virtual void Awake()
    {
        base.mGo = base.gameObject;
        this.mPlayMode = Application.isPlaying;
    }

    public Bounds CalculateBounds() => 
        this.CalculateBounds(null);

    public Bounds CalculateBounds(Transform relativeParent)
    {
        if (relativeParent == null)
        {
            Vector3[] localCorners = this.localCorners;
            Bounds bounds = new Bounds(localCorners[0], Vector3.zero);
            for (int j = 1; j < 4; j++)
            {
                bounds.Encapsulate(localCorners[j]);
            }
            return bounds;
        }
        Matrix4x4 worldToLocalMatrix = relativeParent.worldToLocalMatrix;
        Vector3[] worldCorners = this.worldCorners;
        Bounds bounds2 = new Bounds(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[0]), Vector3.zero);
        for (int i = 1; i < 4; i++)
        {
            bounds2.Encapsulate(worldToLocalMatrix.MultiplyPoint3x4(worldCorners[i]));
        }
        return bounds2;
    }

    public float CalculateCumulativeAlpha(int frameID)
    {
        UIRect parent = base.parent;
        return ((parent == null) ? this.mColor.a : (parent.CalculateFinalAlpha(frameID) * this.mColor.a));
    }

    public override float CalculateFinalAlpha(int frameID)
    {
        if (this.mAlphaFrameID != frameID)
        {
            this.mAlphaFrameID = frameID;
            this.UpdateFinalAlpha(frameID);
        }
        return base.finalAlpha;
    }

    public void CheckLayer()
    {
        if ((this.panel != null) && (this.panel.gameObject.layer != base.gameObject.layer))
        {
            Debug.LogWarning("You can't place widgets on a layer different than the UIPanel that manages them.\nIf you want to move widgets to a different layer, parent them to a new panel instead.", this);
            base.gameObject.layer = this.panel.gameObject.layer;
        }
    }

    public UIPanel CreatePanel()
    {
        if ((base.mStarted && (this.panel == null)) && (base.enabled && NGUITools.GetActive(base.gameObject)))
        {
            this.panel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
            if (this.panel != null)
            {
                base.mParentFound = false;
                this.panel.AddWidget(this);
                this.CheckLayer();
                this.Invalidate(true);
            }
        }
        return this.panel;
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static int FullCompareFunc(UIWidget left, UIWidget right)
    {
        int num = UIPanel.CompareFunc(left.panel, right.panel);
        return ((num != 0) ? num : PanelCompareFunc(left, right));
    }

    public override Vector3[] GetSides(Transform relativeTo)
    {
        Vector2 pivotOffset = this.pivotOffset;
        float x = -pivotOffset.x * this.mWidth;
        float y = -pivotOffset.y * this.mHeight;
        float num3 = x + this.mWidth;
        float num4 = y + this.mHeight;
        float num5 = (x + num3) * 0.5f;
        float num6 = (y + num4) * 0.5f;
        Transform cachedTransform = base.cachedTransform;
        this.mCorners[0] = cachedTransform.TransformPoint(x, num6, 0f);
        this.mCorners[1] = cachedTransform.TransformPoint(num5, num4, 0f);
        this.mCorners[2] = cachedTransform.TransformPoint(num3, num6, 0f);
        this.mCorners[3] = cachedTransform.TransformPoint(num5, y, 0f);
        if (relativeTo != null)
        {
            for (int i = 0; i < 4; i++)
            {
                this.mCorners[i] = relativeTo.InverseTransformPoint(this.mCorners[i]);
            }
        }
        return this.mCorners;
    }

    public override void Invalidate(bool includeChildren)
    {
        base.mChanged = true;
        this.mAlphaFrameID = -1;
        if (this.panel != null)
        {
            bool visibleByPanel = (!this.hideIfOffScreen && !this.panel.hasCumulativeClipping) || this.panel.IsVisible(this);
            this.UpdateVisibility(this.CalculateCumulativeAlpha(Time.frameCount) > 0.001f, visibleByPanel);
            this.UpdateFinalAlpha(Time.frameCount);
            if (includeChildren)
            {
                base.Invalidate(true);
            }
        }
    }

    public virtual void MakePixelPerfect()
    {
        Vector3 localPosition = base.cachedTransform.localPosition;
        localPosition.z = Mathf.Round(localPosition.z);
        localPosition.x = Mathf.Round(localPosition.x);
        localPosition.y = Mathf.Round(localPosition.y);
        base.cachedTransform.localPosition = localPosition;
        Vector3 localScale = base.cachedTransform.localScale;
        float x = Mathf.Sign(localScale.x);
        base.cachedTransform.localScale = new Vector3(x, Mathf.Sign(localScale.y), 1f);
    }

    public virtual void MarkAsChanged()
    {
        if (NGUITools.GetActive(this))
        {
            base.mChanged = true;
            if (((this.panel != null) && base.enabled) && (NGUITools.GetActive(base.gameObject) && !this.mPlayMode))
            {
                this.SetDirty();
                this.CheckLayer();
            }
        }
    }

    protected override void OnAnchor()
    {
        float num;
        float num2;
        float num3;
        float num4;
        Transform cachedTransform = base.cachedTransform;
        Transform parent = cachedTransform.parent;
        Vector3 localPosition = cachedTransform.localPosition;
        Vector2 pivotOffset = this.pivotOffset;
        if (((base.leftAnchor.target == base.bottomAnchor.target) && (base.leftAnchor.target == base.rightAnchor.target)) && (base.leftAnchor.target == base.topAnchor.target))
        {
            Vector3[] sides = base.leftAnchor.GetSides(parent);
            if (sides != null)
            {
                num = NGUIMath.Lerp(sides[0].x, sides[2].x, base.leftAnchor.relative) + base.leftAnchor.absolute;
                num3 = NGUIMath.Lerp(sides[0].x, sides[2].x, base.rightAnchor.relative) + base.rightAnchor.absolute;
                num2 = NGUIMath.Lerp(sides[3].y, sides[1].y, base.bottomAnchor.relative) + base.bottomAnchor.absolute;
                num4 = NGUIMath.Lerp(sides[3].y, sides[1].y, base.topAnchor.relative) + base.topAnchor.absolute;
                this.mIsInFront = true;
            }
            else
            {
                Vector3 localPos = base.GetLocalPos(base.leftAnchor, parent);
                num = localPos.x + base.leftAnchor.absolute;
                num2 = localPos.y + base.bottomAnchor.absolute;
                num3 = localPos.x + base.rightAnchor.absolute;
                num4 = localPos.y + base.topAnchor.absolute;
                this.mIsInFront = !this.hideIfOffScreen || (localPos.z >= 0f);
            }
        }
        else
        {
            this.mIsInFront = true;
            if (base.leftAnchor.target != null)
            {
                Vector3[] vectorArray2 = base.leftAnchor.GetSides(parent);
                if (vectorArray2 != null)
                {
                    num = NGUIMath.Lerp(vectorArray2[0].x, vectorArray2[2].x, base.leftAnchor.relative) + base.leftAnchor.absolute;
                }
                else
                {
                    num = base.GetLocalPos(base.leftAnchor, parent).x + base.leftAnchor.absolute;
                }
            }
            else
            {
                num = localPosition.x - (pivotOffset.x * this.mWidth);
            }
            if (base.rightAnchor.target != null)
            {
                Vector3[] vectorArray3 = base.rightAnchor.GetSides(parent);
                if (vectorArray3 != null)
                {
                    num3 = NGUIMath.Lerp(vectorArray3[0].x, vectorArray3[2].x, base.rightAnchor.relative) + base.rightAnchor.absolute;
                }
                else
                {
                    num3 = base.GetLocalPos(base.rightAnchor, parent).x + base.rightAnchor.absolute;
                }
            }
            else
            {
                num3 = (localPosition.x - (pivotOffset.x * this.mWidth)) + this.mWidth;
            }
            if (base.bottomAnchor.target != null)
            {
                Vector3[] vectorArray4 = base.bottomAnchor.GetSides(parent);
                if (vectorArray4 != null)
                {
                    num2 = NGUIMath.Lerp(vectorArray4[3].y, vectorArray4[1].y, base.bottomAnchor.relative) + base.bottomAnchor.absolute;
                }
                else
                {
                    num2 = base.GetLocalPos(base.bottomAnchor, parent).y + base.bottomAnchor.absolute;
                }
            }
            else
            {
                num2 = localPosition.y - (pivotOffset.y * this.mHeight);
            }
            if (base.topAnchor.target != null)
            {
                Vector3[] vectorArray5 = base.topAnchor.GetSides(parent);
                if (vectorArray5 != null)
                {
                    num4 = NGUIMath.Lerp(vectorArray5[3].y, vectorArray5[1].y, base.topAnchor.relative) + base.topAnchor.absolute;
                }
                else
                {
                    num4 = base.GetLocalPos(base.topAnchor, parent).y + base.topAnchor.absolute;
                }
            }
            else
            {
                num4 = (localPosition.y - (pivotOffset.y * this.mHeight)) + this.mHeight;
            }
        }
        Vector3 vector4 = new Vector3(Mathf.Lerp(num, num3, pivotOffset.x), Mathf.Lerp(num2, num4, pivotOffset.y), localPosition.z);
        int minWidth = Mathf.FloorToInt((num3 - num) + 0.5f);
        int minHeight = Mathf.FloorToInt((num4 - num2) + 0.5f);
        if ((this.keepAspectRatio != AspectRatioSource.Free) && (this.aspectRatio != 0f))
        {
            if (this.keepAspectRatio == AspectRatioSource.BasedOnHeight)
            {
                minWidth = Mathf.RoundToInt(minHeight * this.aspectRatio);
            }
            else
            {
                minHeight = Mathf.RoundToInt(((float) minWidth) / this.aspectRatio);
            }
        }
        if (minWidth < this.minWidth)
        {
            minWidth = this.minWidth;
        }
        if (minHeight < this.minHeight)
        {
            minHeight = this.minHeight;
        }
        if (Vector3.SqrMagnitude(localPosition - vector4) > 0.001f)
        {
            base.cachedTransform.localPosition = vector4;
            if (this.mIsInFront)
            {
                base.mChanged = true;
            }
        }
        if ((this.mWidth != minWidth) || (this.mHeight != minHeight))
        {
            this.mWidth = minWidth;
            this.mHeight = minHeight;
            if (this.mIsInFront)
            {
                base.mChanged = true;
            }
            if (this.autoResizeBoxCollider)
            {
                this.ResizeCollider();
            }
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if (!paused)
        {
            this.MarkAsChanged();
        }
    }

    private void OnDestroy()
    {
        this.RemoveFromPanel();
    }

    protected override void OnDisable()
    {
        this.RemoveFromPanel();
        base.OnDisable();
    }

    public virtual void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
    }

    protected override void OnInit()
    {
        base.OnInit();
        this.RemoveFromPanel();
        this.mMoved = true;
        if (((this.mWidth == 100) && (this.mHeight == 100)) && (base.cachedTransform.localScale.magnitude > 8f))
        {
            this.UpgradeFrom265();
            base.cachedTransform.localScale = Vector3.one;
        }
        base.Update();
    }

    protected override void OnStart()
    {
        this.CreatePanel();
    }

    protected override void OnUpdate()
    {
        if (this.panel == null)
        {
            this.CreatePanel();
        }
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static int PanelCompareFunc(UIWidget left, UIWidget right)
    {
        if (left.mDepth < right.mDepth)
        {
            return -1;
        }
        if (left.mDepth > right.mDepth)
        {
            return 1;
        }
        Material material = left.material;
        Material material2 = right.material;
        if (material == material2)
        {
            return 0;
        }
        if (material != null)
        {
            return -1;
        }
        if (material2 != null)
        {
            return 1;
        }
        return ((material.GetInstanceID() >= material2.GetInstanceID()) ? 1 : -1);
    }

    public override void ParentHasChanged()
    {
        base.ParentHasChanged();
        if (this.panel != null)
        {
            UIPanel panel = UIPanel.Find(base.cachedTransform, true, base.cachedGameObject.layer);
            if (this.panel != panel)
            {
                this.RemoveFromPanel();
                this.CreatePanel();
            }
        }
    }

    public void RemoveFromPanel()
    {
        if (this.panel != null)
        {
            this.panel.RemoveWidget(this);
            this.panel = null;
        }
        this.drawCall = null;
    }

    public void ResizeCollider()
    {
        if (NGUITools.GetActive(this))
        {
            NGUITools.UpdateWidgetCollider(base.gameObject);
        }
    }

    public void SetDimensions(int w, int h)
    {
        if ((this.mWidth != w) || (this.mHeight != h))
        {
            this.mWidth = w;
            this.mHeight = h;
            if (this.keepAspectRatio == AspectRatioSource.BasedOnWidth)
            {
                this.mHeight = Mathf.RoundToInt(((float) this.mWidth) / this.aspectRatio);
            }
            else if (this.keepAspectRatio == AspectRatioSource.BasedOnHeight)
            {
                this.mWidth = Mathf.RoundToInt(this.mHeight * this.aspectRatio);
            }
            else if (this.keepAspectRatio == AspectRatioSource.Free)
            {
                this.aspectRatio = ((float) this.mWidth) / ((float) this.mHeight);
            }
            this.mMoved = true;
            if (this.autoResizeBoxCollider)
            {
                this.ResizeCollider();
            }
            this.MarkAsChanged();
        }
    }

    public void SetDirty()
    {
        if (this.drawCall != null)
        {
            this.drawCall.isDirty = true;
        }
        else if (this.isVisible && this.hasVertices)
        {
            this.CreatePanel();
        }
    }

    public override void SetRect(float x, float y, float width, float height)
    {
        Vector2 pivotOffset = this.pivotOffset;
        float num = Mathf.Lerp(x, x + width, pivotOffset.x);
        float num2 = Mathf.Lerp(y, y + height, pivotOffset.y);
        int minWidth = Mathf.FloorToInt(width + 0.5f);
        int minHeight = Mathf.FloorToInt(height + 0.5f);
        if (pivotOffset.x == 0.5f)
        {
            minWidth = (minWidth >> 1) << 1;
        }
        if (pivotOffset.y == 0.5f)
        {
            minHeight = (minHeight >> 1) << 1;
        }
        Transform cachedTransform = base.cachedTransform;
        Vector3 localPosition = cachedTransform.localPosition;
        localPosition.x = Mathf.Floor(num + 0.5f);
        localPosition.y = Mathf.Floor(num2 + 0.5f);
        if (minWidth < this.minWidth)
        {
            minWidth = this.minWidth;
        }
        if (minHeight < this.minHeight)
        {
            minHeight = this.minHeight;
        }
        cachedTransform.localPosition = localPosition;
        this.width = minWidth;
        this.height = minHeight;
        if (base.isAnchored)
        {
            cachedTransform = cachedTransform.parent;
            if (base.leftAnchor.target != null)
            {
                base.leftAnchor.SetHorizontal(cachedTransform, x);
            }
            if (base.rightAnchor.target != null)
            {
                base.rightAnchor.SetHorizontal(cachedTransform, x + width);
            }
            if (base.bottomAnchor.target != null)
            {
                base.bottomAnchor.SetVertical(cachedTransform, y);
            }
            if (base.topAnchor.target != null)
            {
                base.topAnchor.SetVertical(cachedTransform, y + height);
            }
        }
    }

    protected void UpdateFinalAlpha(int frameID)
    {
        if (!this.mIsVisibleByAlpha || !this.mIsInFront)
        {
            base.finalAlpha = 0f;
        }
        else
        {
            UIRect parent = base.parent;
            base.finalAlpha = (parent == null) ? this.mColor.a : (parent.CalculateFinalAlpha(frameID) * this.mColor.a);
        }
    }

    public bool UpdateGeometry(int frame)
    {
        float num = this.CalculateFinalAlpha(frame);
        if (this.mIsVisibleByAlpha && (this.mLastAlpha != num))
        {
            base.mChanged = true;
        }
        this.mLastAlpha = num;
        if (base.mChanged)
        {
            base.mChanged = false;
            if ((this.mIsVisibleByAlpha && (num > 0.001f)) && (this.shader != null))
            {
                bool hasVertices = this.geometry.hasVertices;
                if (this.fillGeometry)
                {
                    this.geometry.Clear();
                    this.OnFill(this.geometry.verts, this.geometry.uvs, this.geometry.cols);
                }
                if (!this.geometry.hasVertices)
                {
                    return hasVertices;
                }
                if (this.mMatrixFrame != frame)
                {
                    this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
                    this.mMatrixFrame = frame;
                }
                this.geometry.ApplyTransform(this.mLocalToPanel);
                this.mMoved = false;
                return true;
            }
            if (this.geometry.hasVertices)
            {
                if (this.fillGeometry)
                {
                    this.geometry.Clear();
                }
                this.mMoved = false;
                return true;
            }
        }
        else if (this.mMoved && this.geometry.hasVertices)
        {
            if (this.mMatrixFrame != frame)
            {
                this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
                this.mMatrixFrame = frame;
            }
            this.geometry.ApplyTransform(this.mLocalToPanel);
            this.mMoved = false;
            return true;
        }
        this.mMoved = false;
        return false;
    }

    public bool UpdateTransform(int frame)
    {
        if ((!this.mMoved && !this.panel.widgetsAreStatic) && base.cachedTransform.hasChanged)
        {
            base.mTrans.hasChanged = false;
            this.mLocalToPanel = this.panel.worldToLocal * base.cachedTransform.localToWorldMatrix;
            this.mMatrixFrame = frame;
            Vector2 pivotOffset = this.pivotOffset;
            float x = -pivotOffset.x * this.mWidth;
            float y = -pivotOffset.y * this.mHeight;
            float num3 = x + this.mWidth;
            float num4 = y + this.mHeight;
            Transform cachedTransform = base.cachedTransform;
            Vector3 v = cachedTransform.TransformPoint(x, y, 0f);
            Vector3 vector3 = cachedTransform.TransformPoint(num3, num4, 0f);
            v = this.panel.worldToLocal.MultiplyPoint3x4(v);
            vector3 = this.panel.worldToLocal.MultiplyPoint3x4(vector3);
            if ((Vector3.SqrMagnitude(this.mOldV0 - v) > 1E-06f) || (Vector3.SqrMagnitude(this.mOldV1 - vector3) > 1E-06f))
            {
                this.mMoved = true;
                this.mOldV0 = v;
                this.mOldV1 = vector3;
            }
        }
        if (this.mMoved && (this.onChange != null))
        {
            this.onChange();
        }
        return (this.mMoved || base.mChanged);
    }

    public bool UpdateVisibility(bool visibleByAlpha, bool visibleByPanel)
    {
        if ((this.mIsVisibleByAlpha == visibleByAlpha) && (this.mIsVisibleByPanel == visibleByPanel))
        {
            return false;
        }
        base.mChanged = true;
        this.mIsVisibleByAlpha = visibleByAlpha;
        this.mIsVisibleByPanel = visibleByPanel;
        return true;
    }

    protected virtual void UpgradeFrom265()
    {
        Vector3 localScale = base.cachedTransform.localScale;
        this.mWidth = Mathf.Abs(Mathf.RoundToInt(localScale.x));
        this.mHeight = Mathf.Abs(Mathf.RoundToInt(localScale.y));
        NGUITools.UpdateWidgetCollider(base.gameObject, true);
    }

    public void WriteToBuffers(BetterList<Vector3> v, BetterList<Vector2> u, BetterList<Color32> c, BetterList<Vector3> n, BetterList<Vector4> t)
    {
        this.geometry.WriteToBuffers(v, u, c, n, t);
    }

    public override float alpha
    {
        get => 
            this.mColor.a;
        set
        {
            if (this.mColor.a != value)
            {
                this.mColor.a = value;
                this.Invalidate(true);
            }
        }
    }

    public virtual Vector4 border
    {
        get => 
            Vector4.zero;
        set
        {
        }
    }

    public Color color
    {
        get => 
            this.mColor;
        set
        {
            if (this.mColor != value)
            {
                bool includeChildren = this.mColor.a != value.a;
                this.mColor = value;
                this.Invalidate(includeChildren);
            }
        }
    }

    public int depth
    {
        get => 
            this.mDepth;
        set
        {
            if (this.mDepth != value)
            {
                if (this.panel != null)
                {
                    this.panel.RemoveWidget(this);
                }
                this.mDepth = value;
                if (this.panel != null)
                {
                    this.panel.AddWidget(this);
                    if (!Application.isPlaying)
                    {
                        this.panel.SortWidgets();
                        this.panel.RebuildAllDrawCalls();
                    }
                }
            }
        }
    }

    public virtual Vector4 drawingDimensions
    {
        get
        {
            Vector2 pivotOffset = this.pivotOffset;
            float a = -pivotOffset.x * this.mWidth;
            float num2 = -pivotOffset.y * this.mHeight;
            float b = a + this.mWidth;
            float num4 = num2 + this.mHeight;
            return new Vector4((this.mDrawRegion.x != 0f) ? Mathf.Lerp(a, b, this.mDrawRegion.x) : a, (this.mDrawRegion.y != 0f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.y) : num2, (this.mDrawRegion.z != 1f) ? Mathf.Lerp(a, b, this.mDrawRegion.z) : b, (this.mDrawRegion.w != 1f) ? Mathf.Lerp(num2, num4, this.mDrawRegion.w) : num4);
        }
    }

    public Vector4 drawRegion
    {
        get => 
            this.mDrawRegion;
        set
        {
            if (this.mDrawRegion != value)
            {
                this.mDrawRegion = value;
                if (this.autoResizeBoxCollider)
                {
                    this.ResizeCollider();
                }
                this.MarkAsChanged();
            }
        }
    }

    public bool hasBoxCollider
    {
        get
        {
            BoxCollider component = base.GetComponent<Collider>() as BoxCollider;
            return ((component != null) || (base.GetComponent<BoxCollider2D>() != null));
        }
    }

    public bool hasVertices =>
        ((this.geometry != null) && this.geometry.hasVertices);

    public int height
    {
        get => 
            this.mHeight;
        set
        {
            int minHeight = this.minHeight;
            if (value < minHeight)
            {
                value = minHeight;
            }
            if ((this.mHeight != value) && (this.keepAspectRatio != AspectRatioSource.BasedOnWidth))
            {
                if (this.isAnchoredVertically)
                {
                    if ((base.bottomAnchor.target != null) && (base.topAnchor.target != null))
                    {
                        if (((this.mPivot == Pivot.BottomLeft) || (this.mPivot == Pivot.Bottom)) || (this.mPivot == Pivot.BottomRight))
                        {
                            NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float) (value - this.mHeight));
                        }
                        else if (((this.mPivot == Pivot.TopLeft) || (this.mPivot == Pivot.Top)) || (this.mPivot == Pivot.TopRight))
                        {
                            NGUIMath.AdjustWidget(this, 0f, (float) (this.mHeight - value), 0f, 0f);
                        }
                        else
                        {
                            int num2 = value - this.mHeight;
                            num2 -= num2 & 1;
                            if (num2 != 0)
                            {
                                NGUIMath.AdjustWidget(this, 0f, -num2 * 0.5f, 0f, num2 * 0.5f);
                            }
                        }
                    }
                    else if (base.bottomAnchor.target != null)
                    {
                        NGUIMath.AdjustWidget(this, 0f, 0f, 0f, (float) (value - this.mHeight));
                    }
                    else
                    {
                        NGUIMath.AdjustWidget(this, 0f, (float) (this.mHeight - value), 0f, 0f);
                    }
                }
                else
                {
                    this.SetDimensions(this.mWidth, value);
                }
            }
        }
    }

    public bool isVisible =>
        (((this.mIsVisibleByPanel && this.mIsVisibleByAlpha) && (this.mIsInFront && (base.finalAlpha > 0.001f))) && NGUITools.GetActive(this));

    public Vector3 localCenter
    {
        get
        {
            Vector3[] localCorners = this.localCorners;
            return Vector3.Lerp(localCorners[0], localCorners[2], 0.5f);
        }
    }

    public override Vector3[] localCorners
    {
        get
        {
            Vector2 pivotOffset = this.pivotOffset;
            float x = -pivotOffset.x * this.mWidth;
            float y = -pivotOffset.y * this.mHeight;
            float num3 = x + this.mWidth;
            float num4 = y + this.mHeight;
            this.mCorners[0] = new Vector3(x, y);
            this.mCorners[1] = new Vector3(x, num4);
            this.mCorners[2] = new Vector3(num3, num4);
            this.mCorners[3] = new Vector3(num3, y);
            return this.mCorners;
        }
    }

    public virtual Vector2 localSize
    {
        get
        {
            Vector3[] localCorners = this.localCorners;
            return (localCorners[2] - localCorners[0]);
        }
    }

    public virtual Texture mainTexture
    {
        get
        {
            Material material = this.material;
            return material?.mainTexture;
        }
        set
        {
            throw new NotImplementedException(base.GetType() + " has no mainTexture setter");
        }
    }

    public virtual Material material
    {
        get => 
            null;
        set
        {
            throw new NotImplementedException(base.GetType() + " has no material setter");
        }
    }

    public virtual int minHeight =>
        2;

    public virtual int minWidth =>
        2;

    public UIDrawCall.OnRenderCallback onRender
    {
        get => 
            this.mOnRender;
        set
        {
            if (this.mOnRender != value)
            {
                if (((this.drawCall != null) && (this.drawCall.onRender != null)) && (this.mOnRender != null))
                {
                    this.drawCall.onRender = (UIDrawCall.OnRenderCallback) Delegate.Remove(this.drawCall.onRender, this.mOnRender);
                }
                this.mOnRender = value;
                if (this.drawCall != null)
                {
                    this.drawCall.onRender = (UIDrawCall.OnRenderCallback) Delegate.Combine(this.drawCall.onRender, value);
                }
            }
        }
    }

    public Pivot pivot
    {
        get => 
            this.mPivot;
        set
        {
            if (this.mPivot != value)
            {
                Vector3 vector = this.worldCorners[0];
                this.mPivot = value;
                base.mChanged = true;
                Vector3 vector2 = this.worldCorners[0];
                Transform cachedTransform = base.cachedTransform;
                Vector3 position = cachedTransform.position;
                float z = cachedTransform.localPosition.z;
                position.x += vector.x - vector2.x;
                position.y += vector.y - vector2.y;
                base.cachedTransform.position = position;
                position = base.cachedTransform.localPosition;
                position.x = Mathf.Round(position.x);
                position.y = Mathf.Round(position.y);
                position.z = z;
                base.cachedTransform.localPosition = position;
            }
        }
    }

    public Vector2 pivotOffset =>
        NGUIMath.GetPivotOffset(this.pivot);

    public Pivot rawPivot
    {
        get => 
            this.mPivot;
        set
        {
            if (this.mPivot != value)
            {
                this.mPivot = value;
                if (this.autoResizeBoxCollider)
                {
                    this.ResizeCollider();
                }
                this.MarkAsChanged();
            }
        }
    }

    public int raycastDepth
    {
        get
        {
            if (this.panel == null)
            {
                this.CreatePanel();
            }
            return ((this.panel == null) ? this.mDepth : (this.mDepth + (this.panel.depth * 0x3e8)));
        }
    }

    [Obsolete("There is no relative scale anymore. Widgets now have width and height instead")]
    public Vector2 relativeSize =>
        Vector2.one;

    public virtual Shader shader
    {
        get
        {
            Material material = this.material;
            return material?.shader;
        }
        set
        {
            throw new NotImplementedException(base.GetType() + " has no shader setter");
        }
    }

    public int width
    {
        get => 
            this.mWidth;
        set
        {
            int minWidth = this.minWidth;
            if (value < minWidth)
            {
                value = minWidth;
            }
            if ((this.mWidth != value) && (this.keepAspectRatio != AspectRatioSource.BasedOnHeight))
            {
                if (this.isAnchoredHorizontally)
                {
                    if ((base.leftAnchor.target != null) && (base.rightAnchor.target != null))
                    {
                        if (((this.mPivot == Pivot.BottomLeft) || (this.mPivot == Pivot.Left)) || (this.mPivot == Pivot.TopLeft))
                        {
                            NGUIMath.AdjustWidget(this, 0f, 0f, (float) (value - this.mWidth), 0f);
                        }
                        else if (((this.mPivot == Pivot.BottomRight) || (this.mPivot == Pivot.Right)) || (this.mPivot == Pivot.TopRight))
                        {
                            NGUIMath.AdjustWidget(this, (float) (this.mWidth - value), 0f, 0f, 0f);
                        }
                        else
                        {
                            int num2 = value - this.mWidth;
                            num2 -= num2 & 1;
                            if (num2 != 0)
                            {
                                NGUIMath.AdjustWidget(this, -num2 * 0.5f, 0f, num2 * 0.5f, 0f);
                            }
                        }
                    }
                    else if (base.leftAnchor.target != null)
                    {
                        NGUIMath.AdjustWidget(this, 0f, 0f, (float) (value - this.mWidth), 0f);
                    }
                    else
                    {
                        NGUIMath.AdjustWidget(this, (float) (this.mWidth - value), 0f, 0f, 0f);
                    }
                }
                else
                {
                    this.SetDimensions(value, this.mHeight);
                }
            }
        }
    }

    public Vector3 worldCenter =>
        base.cachedTransform.TransformPoint(this.localCenter);

    public override Vector3[] worldCorners
    {
        get
        {
            Vector2 pivotOffset = this.pivotOffset;
            float x = -pivotOffset.x * this.mWidth;
            float y = -pivotOffset.y * this.mHeight;
            float num3 = x + this.mWidth;
            float num4 = y + this.mHeight;
            Transform cachedTransform = base.cachedTransform;
            this.mCorners[0] = cachedTransform.TransformPoint(x, y, 0f);
            this.mCorners[1] = cachedTransform.TransformPoint(x, num4, 0f);
            this.mCorners[2] = cachedTransform.TransformPoint(num3, num4, 0f);
            this.mCorners[3] = cachedTransform.TransformPoint(num3, y, 0f);
            return this.mCorners;
        }
    }

    public enum AspectRatioSource
    {
        Free,
        BasedOnWidth,
        BasedOnHeight
    }

    public delegate bool HitCheck(Vector3 worldPos);

    public delegate void OnDimensionsChanged();

    public delegate void OnPostFillCallback(UIWidget widget, int bufferOffset, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols);

    public enum Pivot
    {
        TopLeft,
        Top,
        TopRight,
        Left,
        Center,
        Right,
        BottomLeft,
        Bottom,
        BottomRight
    }
}

