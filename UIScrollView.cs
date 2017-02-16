using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(UIPanel)), AddComponentMenu("NGUI/Interaction/Scroll View")]
public class UIScrollView : MonoBehaviour
{
    [HideInInspector]
    public UICenterOnChild centerOnChild;
    public UIWidget.Pivot contentPivot;
    public Vector2 customMovement = new Vector2(1f, 0f);
    public bool disableDragIfFits;
    public DragEffect dragEffect = DragEffect.MomentumAndSpring;
    public UIProgressBar horizontalScrollBar;
    public bool iOSDragEmulation = true;
    public static BetterList<UIScrollView> list = new BetterList<UIScrollView>();
    protected Bounds mBounds;
    protected bool mCalculatedBounds;
    protected int mDragID = -10;
    protected bool mDragStarted;
    protected Vector2 mDragStartOffset = Vector2.zero;
    protected bool mIgnoreCallbacks;
    protected Vector3 mLastPos;
    protected Vector3 mMomentum = Vector3.zero;
    public float momentumAmount = 35f;
    public Movement movement;
    protected UIPanel mPanel;
    protected Plane mPlane;
    protected bool mPressed;
    protected float mScroll;
    protected bool mShouldMove;
    [NonSerialized]
    private bool mStarted;
    protected Transform mTrans;
    public OnDragNotification onDragFinished;
    public OnDragNotification onDragStarted;
    public OnDragNotification onMomentumMove;
    public OnDragNotification onStoppedMoving;
    [SerializeField, HideInInspector]
    private Vector2 relativePositionOnReset = Vector2.zero;
    public bool restrictWithinPanel = true;
    [HideInInspector, SerializeField]
    private Vector3 scale = new Vector3(1f, 0f, 0f);
    public float scrollWheelFactor = 0.25f;
    public ShowCondition showScrollBars = ShowCondition.OnlyIfNeeded;
    public bool smoothDragStart = true;
    public UIProgressBar verticalScrollBar;

    private void Awake()
    {
        this.mTrans = base.transform;
        this.mPanel = base.GetComponent<UIPanel>();
        if (this.mPanel.clipping == UIDrawCall.Clipping.None)
        {
            this.mPanel.clipping = UIDrawCall.Clipping.ConstrainButDontClip;
        }
        if ((this.movement != Movement.Custom) && (this.scale.sqrMagnitude > 0.001f))
        {
            if ((this.scale.x == 1f) && (this.scale.y == 0f))
            {
                this.movement = Movement.Horizontal;
            }
            else if ((this.scale.x == 0f) && (this.scale.y == 1f))
            {
                this.movement = Movement.Vertical;
            }
            else if ((this.scale.x == 1f) && (this.scale.y == 1f))
            {
                this.movement = Movement.Unrestricted;
            }
            else
            {
                this.movement = Movement.Custom;
                this.customMovement.x = this.scale.x;
                this.customMovement.y = this.scale.y;
            }
            this.scale = Vector3.zero;
        }
        if ((this.contentPivot == UIWidget.Pivot.TopLeft) && (this.relativePositionOnReset != Vector2.zero))
        {
            this.contentPivot = NGUIMath.GetPivot(new Vector2(this.relativePositionOnReset.x, 1f - this.relativePositionOnReset.y));
            this.relativePositionOnReset = Vector2.zero;
        }
    }

    private void CheckScrollbars()
    {
        if (this.horizontalScrollBar != null)
        {
            EventDelegate.Add(this.horizontalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
            this.horizontalScrollBar.alpha = ((this.showScrollBars != ShowCondition.Always) && !this.shouldMoveHorizontally) ? 0f : 1f;
        }
        if (this.verticalScrollBar != null)
        {
            EventDelegate.Add(this.verticalScrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
            this.verticalScrollBar.alpha = ((this.showScrollBars != ShowCondition.Always) && !this.shouldMoveVertically) ? 0f : 1f;
        }
    }

    public void DisableSpring()
    {
        SpringPanel component = base.GetComponent<SpringPanel>();
        if (component != null)
        {
            component.enabled = false;
        }
    }

    public void Drag()
    {
        if ((UICamera.currentScheme != UICamera.ControlScheme.Controller) && ((base.enabled && NGUITools.GetActive(base.gameObject)) && this.mShouldMove))
        {
            if (this.mDragID == -10)
            {
                this.mDragID = UICamera.currentTouchID;
            }
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
            if (this.smoothDragStart && !this.mDragStarted)
            {
                this.mDragStarted = true;
                this.mDragStartOffset = UICamera.currentTouch.totalDelta;
                if (this.onDragStarted != null)
                {
                    this.onDragStarted();
                }
            }
            Ray ray = !this.smoothDragStart ? UICamera.currentCamera.ScreenPointToRay((Vector3) UICamera.currentTouch.pos) : UICamera.currentCamera.ScreenPointToRay((Vector3) (UICamera.currentTouch.pos - this.mDragStartOffset));
            float enter = 0f;
            if (this.mPlane.Raycast(ray, out enter))
            {
                Vector3 point = ray.GetPoint(enter);
                Vector3 direction = point - this.mLastPos;
                this.mLastPos = point;
                if (((direction.x != 0f) || (direction.y != 0f)) || (direction.z != 0f))
                {
                    direction = this.mTrans.InverseTransformDirection(direction);
                    if (this.movement == Movement.Horizontal)
                    {
                        direction.y = 0f;
                        direction.z = 0f;
                    }
                    else if (this.movement == Movement.Vertical)
                    {
                        direction.x = 0f;
                        direction.z = 0f;
                    }
                    else if (this.movement == Movement.Unrestricted)
                    {
                        direction.z = 0f;
                    }
                    else
                    {
                        direction.Scale((Vector3) this.customMovement);
                    }
                    direction = this.mTrans.TransformDirection(direction);
                }
                if (this.dragEffect == DragEffect.None)
                {
                    this.mMomentum = Vector3.zero;
                }
                else
                {
                    this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + ((Vector3) (direction * (0.01f * this.momentumAmount))), 0.67f);
                }
                if (!this.iOSDragEmulation || (this.dragEffect != DragEffect.MomentumAndSpring))
                {
                    this.MoveAbsolute(direction);
                }
                else if (this.mPanel.CalculateConstrainOffset(this.bounds.min, this.bounds.max).magnitude > 1f)
                {
                    this.MoveAbsolute((Vector3) (direction * 0.5f));
                    this.mMomentum = (Vector3) (this.mMomentum * 0.5f);
                }
                else
                {
                    this.MoveAbsolute(direction);
                }
                if ((this.restrictWithinPanel && (this.mPanel.clipping != UIDrawCall.Clipping.None)) && (this.dragEffect != DragEffect.MomentumAndSpring))
                {
                    this.RestrictWithinBounds(true, this.canMoveHorizontally, this.canMoveVertically);
                }
            }
        }
    }

    public void InvalidateBounds()
    {
        this.mCalculatedBounds = false;
    }

    public bool IsLimitOverPosition()
    {
        Bounds bounds = this.bounds;
        Vector3 vector = this.panel.CalculateConstrainOffset(bounds.min, bounds.max);
        if (this.movement != Movement.Horizontal)
        {
            vector.x = 0f;
        }
        if (this.movement != Movement.Vertical)
        {
            vector.y = 0f;
        }
        return (vector.sqrMagnitude > 0.1f);
    }

    private void LateUpdate()
    {
        if (Application.isPlaying)
        {
            float deltaTime = RealTime.deltaTime;
            if ((this.showScrollBars != ShowCondition.Always) && ((this.verticalScrollBar != null) || (this.horizontalScrollBar != null)))
            {
                bool shouldMoveVertically = false;
                bool shouldMoveHorizontally = false;
                if (((this.showScrollBars != ShowCondition.WhenDragging) || (this.mDragID != -10)) || (this.mMomentum.magnitude > 0.01f))
                {
                    shouldMoveVertically = this.shouldMoveVertically;
                    shouldMoveHorizontally = this.shouldMoveHorizontally;
                }
                if (this.verticalScrollBar != null)
                {
                    float num2 = this.verticalScrollBar.alpha + (!shouldMoveVertically ? (-deltaTime * 3f) : (deltaTime * 6f));
                    num2 = Mathf.Clamp01(num2);
                    if (this.verticalScrollBar.alpha != num2)
                    {
                        this.verticalScrollBar.alpha = num2;
                    }
                }
                if (this.horizontalScrollBar != null)
                {
                    float num3 = this.horizontalScrollBar.alpha + (!shouldMoveHorizontally ? (-deltaTime * 3f) : (deltaTime * 6f));
                    num3 = Mathf.Clamp01(num3);
                    if (this.horizontalScrollBar.alpha != num3)
                    {
                        this.horizontalScrollBar.alpha = num3;
                    }
                }
            }
            if (this.mShouldMove)
            {
                if (!this.mPressed)
                {
                    if ((this.mMomentum.magnitude > 0.0001f) || (this.mScroll != 0f))
                    {
                        if (this.movement == Movement.Horizontal)
                        {
                            this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, 0f, 0f));
                        }
                        else if (this.movement == Movement.Vertical)
                        {
                            this.mMomentum -= this.mTrans.TransformDirection(new Vector3(0f, this.mScroll * 0.05f, 0f));
                        }
                        else if (this.movement == Movement.Unrestricted)
                        {
                            this.mMomentum -= this.mTrans.TransformDirection(new Vector3(this.mScroll * 0.05f, this.mScroll * 0.05f, 0f));
                        }
                        else
                        {
                            this.mMomentum -= this.mTrans.TransformDirection(new Vector3((this.mScroll * this.customMovement.x) * 0.05f, (this.mScroll * this.customMovement.y) * 0.05f, 0f));
                        }
                        this.mScroll = NGUIMath.SpringLerp(this.mScroll, 0f, 20f, deltaTime);
                        Vector3 absolute = NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
                        this.MoveAbsolute(absolute);
                        if (this.restrictWithinPanel && (this.mPanel.clipping != UIDrawCall.Clipping.None))
                        {
                            if (NGUITools.GetActive(this.centerOnChild))
                            {
                                if (this.centerOnChild.nextPageThreshold != 0f)
                                {
                                    this.mMomentum = Vector3.zero;
                                    this.mScroll = 0f;
                                }
                                else
                                {
                                    this.centerOnChild.Recenter();
                                }
                            }
                            else
                            {
                                this.RestrictWithinBounds(false, this.canMoveHorizontally, this.canMoveVertically);
                            }
                        }
                        if (this.onMomentumMove != null)
                        {
                            this.onMomentumMove();
                        }
                    }
                    else
                    {
                        this.mScroll = 0f;
                        this.mMomentum = Vector3.zero;
                        SpringPanel component = base.GetComponent<SpringPanel>();
                        if ((component == null) || !component.enabled)
                        {
                            this.mShouldMove = false;
                            if (this.onStoppedMoving != null)
                            {
                                this.onStoppedMoving();
                            }
                        }
                    }
                }
                else
                {
                    this.mScroll = 0f;
                    NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
                }
            }
        }
    }

    public void MoveAbsolute(Vector3 absolute)
    {
        Vector3 vector = this.mTrans.InverseTransformPoint(absolute);
        Vector3 vector2 = this.mTrans.InverseTransformPoint(Vector3.zero);
        this.MoveRelative(vector - vector2);
    }

    public virtual void MoveRelative(Vector3 relative)
    {
        this.mTrans.localPosition += relative;
        Vector2 clipOffset = this.mPanel.clipOffset;
        clipOffset.x -= relative.x;
        clipOffset.y -= relative.y;
        this.mPanel.clipOffset = clipOffset;
        this.UpdateScrollbars(false);
    }

    private void OnDisable()
    {
        list.Remove(this);
    }

    private void OnEnable()
    {
        list.Add(this);
        if (this.mStarted && Application.isPlaying)
        {
            this.CheckScrollbars();
        }
    }

    public void OnScrollBar()
    {
        if (!this.mIgnoreCallbacks)
        {
            this.mIgnoreCallbacks = true;
            float x = (this.horizontalScrollBar == null) ? 0f : this.horizontalScrollBar.value;
            float y = (this.verticalScrollBar == null) ? 0f : this.verticalScrollBar.value;
            this.SetDragAmount(x, y, false);
            this.mIgnoreCallbacks = false;
        }
    }

    public void Press(bool pressed)
    {
        if (UICamera.currentScheme != UICamera.ControlScheme.Controller)
        {
            if (this.smoothDragStart && pressed)
            {
                this.mDragStarted = false;
                this.mDragStartOffset = Vector2.zero;
            }
            if (base.enabled && NGUITools.GetActive(base.gameObject))
            {
                if (!pressed && (this.mDragID == UICamera.currentTouchID))
                {
                    this.mDragID = -10;
                }
                this.mCalculatedBounds = false;
                this.mShouldMove = this.shouldMove;
                if (this.mShouldMove)
                {
                    this.mPressed = pressed;
                    if (pressed)
                    {
                        this.mMomentum = Vector3.zero;
                        this.mScroll = 0f;
                        this.DisableSpring();
                        this.mLastPos = UICamera.lastWorldPosition;
                        this.mPlane = new Plane((Vector3) (this.mTrans.rotation * Vector3.back), this.mLastPos);
                        Vector2 clipOffset = this.mPanel.clipOffset;
                        clipOffset.x = Mathf.Round(clipOffset.x);
                        clipOffset.y = Mathf.Round(clipOffset.y);
                        this.mPanel.clipOffset = clipOffset;
                        Vector3 localPosition = this.mTrans.localPosition;
                        localPosition.x = Mathf.Round(localPosition.x);
                        localPosition.y = Mathf.Round(localPosition.y);
                        this.mTrans.localPosition = localPosition;
                        if (!this.smoothDragStart)
                        {
                            this.mDragStarted = true;
                            this.mDragStartOffset = Vector2.zero;
                            if (this.onDragStarted != null)
                            {
                                this.onDragStarted();
                            }
                        }
                    }
                    else if (this.centerOnChild != null)
                    {
                        this.centerOnChild.Recenter();
                    }
                    else
                    {
                        if (this.restrictWithinPanel && (this.mPanel.clipping != UIDrawCall.Clipping.None))
                        {
                            this.RestrictWithinBounds(this.dragEffect == DragEffect.None, this.canMoveHorizontally, this.canMoveVertically);
                        }
                        if (this.mDragStarted && (this.onDragFinished != null))
                        {
                            this.onDragFinished();
                        }
                        if (!this.mShouldMove && (this.onStoppedMoving != null))
                        {
                            this.onStoppedMoving();
                        }
                    }
                }
            }
        }
    }

    [ContextMenu("Reset Clipping Position")]
    public void ResetPosition()
    {
        if (NGUITools.GetActive(this))
        {
            this.mCalculatedBounds = false;
            Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
            this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, false);
            this.SetDragAmount(pivotOffset.x, 1f - pivotOffset.y, true);
        }
    }

    public bool RestrictWithinBounds(bool instant) => 
        this.RestrictWithinBounds(instant, true, true);

    public bool RestrictWithinBounds(bool instant, bool horizontal, bool vertical)
    {
        Bounds bounds = this.bounds;
        Vector3 relative = this.mPanel.CalculateConstrainOffset(bounds.min, bounds.max);
        if (!horizontal)
        {
            relative.x = 0f;
        }
        if (!vertical)
        {
            relative.y = 0f;
        }
        if (relative.sqrMagnitude <= 0.1f)
        {
            return false;
        }
        if (!instant && (this.dragEffect == DragEffect.MomentumAndSpring))
        {
            Vector3 pos = this.mTrans.localPosition + relative;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);
            SpringPanel.Begin(this.mPanel.gameObject, pos, 13f).strength = 8f;
        }
        else
        {
            this.MoveRelative(relative);
            if (Mathf.Abs(relative.x) > 0.01f)
            {
                this.mMomentum.x = 0f;
            }
            if (Mathf.Abs(relative.y) > 0.01f)
            {
                this.mMomentum.y = 0f;
            }
            if (Mathf.Abs(relative.z) > 0.01f)
            {
                this.mMomentum.z = 0f;
            }
            this.mScroll = 0f;
        }
        return true;
    }

    public void Scroll(float delta)
    {
        if ((base.enabled && NGUITools.GetActive(base.gameObject)) && (this.scrollWheelFactor != 0f))
        {
            this.DisableSpring();
            this.mShouldMove |= this.shouldMove;
            if (Mathf.Sign(this.mScroll) != Mathf.Sign(delta))
            {
                this.mScroll = 0f;
            }
            this.mScroll += delta * this.scrollWheelFactor;
        }
    }

    public virtual void SetDragAmount(float x, float y, bool updateScrollbars)
    {
        if (this.mPanel == null)
        {
            this.mPanel = base.GetComponent<UIPanel>();
        }
        this.DisableSpring();
        Bounds bounds = this.bounds;
        if ((bounds.min.x != bounds.max.x) && (bounds.min.y != bounds.max.y))
        {
            Vector4 finalClipRegion = this.mPanel.finalClipRegion;
            float num = finalClipRegion.z * 0.5f;
            float num2 = finalClipRegion.w * 0.5f;
            float a = bounds.min.x + num;
            float b = bounds.max.x - num;
            float num5 = bounds.min.y + num2;
            float num6 = bounds.max.y - num2;
            if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
            {
                a -= this.mPanel.clipSoftness.x;
                b += this.mPanel.clipSoftness.x;
                num5 -= this.mPanel.clipSoftness.y;
                num6 += this.mPanel.clipSoftness.y;
            }
            float num7 = Mathf.Lerp(a, b, x);
            float num8 = Mathf.Lerp(num6, num5, y);
            if (!updateScrollbars)
            {
                Vector3 localPosition = this.mTrans.localPosition;
                if (this.canMoveHorizontally)
                {
                    localPosition.x += finalClipRegion.x - num7;
                }
                if (this.canMoveVertically)
                {
                    localPosition.y += finalClipRegion.y - num8;
                }
                this.mTrans.localPosition = localPosition;
            }
            if (this.canMoveHorizontally)
            {
                finalClipRegion.x = num7;
            }
            if (this.canMoveVertically)
            {
                finalClipRegion.y = num8;
            }
            Vector4 baseClipRegion = this.mPanel.baseClipRegion;
            this.mPanel.clipOffset = new Vector2(finalClipRegion.x - baseClipRegion.x, finalClipRegion.y - baseClipRegion.y);
            if (updateScrollbars)
            {
                this.UpdateScrollbars(this.mDragID == -10);
            }
        }
    }

    private void Start()
    {
        this.mStarted = true;
        if (Application.isPlaying)
        {
            this.CheckScrollbars();
        }
    }

    private void Update()
    {
        this.iOSDragEmulation = this.IsLimitOverPosition();
    }

    public void UpdatePosition()
    {
        if (!this.mIgnoreCallbacks && ((this.horizontalScrollBar != null) || (this.verticalScrollBar != null)))
        {
            this.mIgnoreCallbacks = true;
            this.mCalculatedBounds = false;
            Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.contentPivot);
            float x = (this.horizontalScrollBar == null) ? pivotOffset.x : this.horizontalScrollBar.value;
            float y = (this.verticalScrollBar == null) ? (1f - pivotOffset.y) : this.verticalScrollBar.value;
            this.SetDragAmount(x, y, false);
            this.UpdateScrollbars(true);
            this.mIgnoreCallbacks = false;
        }
    }

    public void UpdateScrollbars()
    {
        this.UpdateScrollbars(true);
    }

    public virtual void UpdateScrollbars(bool recalculateBounds)
    {
        if (this.mPanel != null)
        {
            if ((this.horizontalScrollBar != null) || (this.verticalScrollBar != null))
            {
                if (recalculateBounds)
                {
                    this.mCalculatedBounds = false;
                    this.mShouldMove = this.shouldMove;
                }
                Bounds bounds = this.bounds;
                Vector2 min = bounds.min;
                Vector2 max = bounds.max;
                if ((this.horizontalScrollBar != null) && (max.x > min.x))
                {
                    Vector4 finalClipRegion = this.mPanel.finalClipRegion;
                    int num = Mathf.RoundToInt(finalClipRegion.z);
                    if ((num & 1) != 0)
                    {
                        num--;
                    }
                    float f = num * 0.5f;
                    f = Mathf.Round(f);
                    if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
                    {
                        f -= this.mPanel.clipSoftness.x;
                    }
                    float contentSize = max.x - min.x;
                    float viewSize = f * 2f;
                    float x = min.x;
                    float contentMax = max.x;
                    float num7 = finalClipRegion.x - f;
                    float num8 = finalClipRegion.x + f;
                    x = num7 - x;
                    contentMax -= num8;
                    this.UpdateScrollbars(this.horizontalScrollBar, x, contentMax, contentSize, viewSize, false);
                }
                if ((this.verticalScrollBar != null) && (max.y > min.y))
                {
                    Vector4 vector4 = this.mPanel.finalClipRegion;
                    int num9 = Mathf.RoundToInt(vector4.w);
                    if ((num9 & 1) != 0)
                    {
                        num9--;
                    }
                    float num10 = num9 * 0.5f;
                    num10 = Mathf.Round(num10);
                    if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
                    {
                        num10 -= this.mPanel.clipSoftness.y;
                    }
                    float num11 = max.y - min.y;
                    float num12 = num10 * 2f;
                    float y = min.y;
                    float num14 = max.y;
                    float num15 = vector4.y - num10;
                    float num16 = vector4.y + num10;
                    y = num15 - y;
                    num14 -= num16;
                    this.UpdateScrollbars(this.verticalScrollBar, y, num14, num11, num12, true);
                }
            }
            else if (recalculateBounds)
            {
                this.mCalculatedBounds = false;
            }
        }
    }

    protected void UpdateScrollbars(UIProgressBar slider, float contentMin, float contentMax, float contentSize, float viewSize, bool inverted)
    {
        if (slider != null)
        {
            float num;
            this.mIgnoreCallbacks = true;
            if (viewSize < contentSize)
            {
                contentMin = Mathf.Clamp01(contentMin / contentSize);
                contentMax = Mathf.Clamp01(contentMax / contentSize);
                num = contentMin + contentMax;
                slider.value = !inverted ? ((num <= 0.001f) ? 1f : (contentMin / num)) : ((num <= 0.001f) ? 0f : (1f - (contentMin / num)));
            }
            else
            {
                contentMin = Mathf.Clamp01(-contentMin / contentSize);
                contentMax = Mathf.Clamp01(-contentMax / contentSize);
                num = contentMin + contentMax;
                slider.value = !inverted ? ((num <= 0.001f) ? 1f : (contentMin / num)) : ((num <= 0.001f) ? 0f : (1f - (contentMin / num)));
                if (contentSize > 0f)
                {
                    contentMin = Mathf.Clamp01(contentMin / contentSize);
                    contentMax = Mathf.Clamp01(contentMax / contentSize);
                    num = contentMin + contentMax;
                }
            }
            UIScrollBar bar = slider as UIScrollBar;
            if (bar != null)
            {
                bar.barSize = 1f - num;
            }
            this.mIgnoreCallbacks = false;
        }
    }

    public virtual Bounds bounds
    {
        get
        {
            if (!this.mCalculatedBounds)
            {
                this.mCalculatedBounds = true;
                this.mTrans = base.transform;
                this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.mTrans, this.mTrans);
            }
            return this.mBounds;
        }
    }

    public bool canMoveHorizontally =>
        (((this.movement == Movement.Horizontal) || (this.movement == Movement.Unrestricted)) || ((this.movement == Movement.Custom) && (this.customMovement.x != 0f)));

    public bool canMoveVertically =>
        (((this.movement == Movement.Vertical) || (this.movement == Movement.Unrestricted)) || ((this.movement == Movement.Custom) && (this.customMovement.y != 0f)));

    public Vector3 currentMomentum
    {
        get => 
            this.mMomentum;
        set
        {
            this.mMomentum = value;
            this.mShouldMove = true;
        }
    }

    public bool isDragging =>
        (this.mPressed && this.mDragStarted);

    public UIPanel panel =>
        this.mPanel;

    protected virtual bool shouldMove
    {
        get
        {
            if (!this.disableDragIfFits)
            {
                return true;
            }
            if (this.mPanel == null)
            {
                this.mPanel = base.GetComponent<UIPanel>();
            }
            Vector4 finalClipRegion = this.mPanel.finalClipRegion;
            Bounds bounds = this.bounds;
            float num = (finalClipRegion.z != 0f) ? (finalClipRegion.z * 0.5f) : ((float) Screen.width);
            float num2 = (finalClipRegion.w != 0f) ? (finalClipRegion.w * 0.5f) : ((float) Screen.height);
            if (this.canMoveHorizontally)
            {
                if (bounds.min.x < (finalClipRegion.x - num))
                {
                    return true;
                }
                if (bounds.max.x > (finalClipRegion.x + num))
                {
                    return true;
                }
            }
            if (this.canMoveVertically)
            {
                if (bounds.min.y < (finalClipRegion.y - num2))
                {
                    return true;
                }
                if (bounds.max.y > (finalClipRegion.y + num2))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public virtual bool shouldMoveHorizontally
    {
        get
        {
            float x = this.bounds.size.x;
            if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
            {
                x += this.mPanel.clipSoftness.x * 2f;
            }
            return (Mathf.RoundToInt(x - this.mPanel.width) > 0);
        }
    }

    public virtual bool shouldMoveVertically
    {
        get
        {
            float y = this.bounds.size.y;
            if (this.mPanel.clipping == UIDrawCall.Clipping.SoftClip)
            {
                y += this.mPanel.clipSoftness.y * 2f;
            }
            return (Mathf.RoundToInt(y - this.mPanel.height) > 0);
        }
    }

    public enum DragEffect
    {
        None,
        Momentum,
        MomentumAndSpring
    }

    public enum Movement
    {
        Horizontal,
        Vertical,
        Unrestricted,
        Custom
    }

    public delegate void OnDragNotification();

    public enum ShowCondition
    {
        Always,
        OnlyIfNeeded,
        WhenDragging
    }
}

