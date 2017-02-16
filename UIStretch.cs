using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Stretch"), ExecuteInEditMode]
public class UIStretch : MonoBehaviour
{
    public Vector2 borderPadding = Vector2.zero;
    public GameObject container;
    public Vector2 initialSize = Vector2.one;
    private Animation mAnim;
    private UIPanel mPanel;
    private Rect mRect;
    private UIRoot mRoot;
    private UISprite mSprite;
    private bool mStarted;
    private Transform mTrans;
    private UIWidget mWidget;
    public Vector2 relativeSize = Vector2.one;
    public bool runOnlyOnce = true;
    public Style style;
    public Camera uiCamera;
    [HideInInspector, SerializeField]
    private UIWidget widgetContainer;

    private void Awake()
    {
        this.mAnim = base.GetComponent<Animation>();
        this.mRect = new Rect();
        this.mTrans = base.transform;
        this.mWidget = base.GetComponent<UIWidget>();
        this.mSprite = base.GetComponent<UISprite>();
        this.mPanel = base.GetComponent<UIPanel>();
        UICamera.onScreenResize = (UICamera.OnScreenResize) Delegate.Combine(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
    }

    private void OnDestroy()
    {
        UICamera.onScreenResize = (UICamera.OnScreenResize) Delegate.Remove(UICamera.onScreenResize, new UICamera.OnScreenResize(this.ScreenSizeChanged));
    }

    private void ScreenSizeChanged()
    {
        if (this.mStarted && this.runOnlyOnce)
        {
            this.Update();
        }
    }

    private void Start()
    {
        if ((this.container == null) && (this.widgetContainer != null))
        {
            this.container = this.widgetContainer.gameObject;
            this.widgetContainer = null;
        }
        if (this.uiCamera == null)
        {
            this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
        this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
        this.Update();
        this.mStarted = true;
    }

    private void Update()
    {
        if (((this.mAnim == null) || !this.mAnim.isPlaying) && (this.style != Style.None))
        {
            UIWidget component = this.container?.GetComponent<UIWidget>();
            UIPanel panel = ((this.container != null) || (component != null)) ? this.container.GetComponent<UIPanel>() : null;
            float pixelSizeAdjustment = 1f;
            if (component != null)
            {
                Bounds bounds = component.CalculateBounds(base.transform.parent);
                this.mRect.x = bounds.min.x;
                this.mRect.y = bounds.min.y;
                this.mRect.width = bounds.size.x;
                this.mRect.height = bounds.size.y;
            }
            else if (panel != null)
            {
                if (panel.clipping == UIDrawCall.Clipping.None)
                {
                    float num2 = (this.mRoot == null) ? 0.5f : ((((float) this.mRoot.activeHeight) / ((float) Screen.height)) * 0.5f);
                    this.mRect.xMin = -Screen.width * num2;
                    this.mRect.yMin = -Screen.height * num2;
                    this.mRect.xMax = -this.mRect.xMin;
                    this.mRect.yMax = -this.mRect.yMin;
                }
                else
                {
                    Vector4 finalClipRegion = panel.finalClipRegion;
                    this.mRect.x = finalClipRegion.x - (finalClipRegion.z * 0.5f);
                    this.mRect.y = finalClipRegion.y - (finalClipRegion.w * 0.5f);
                    this.mRect.width = finalClipRegion.z;
                    this.mRect.height = finalClipRegion.w;
                }
            }
            else if (this.container != null)
            {
                Transform parent = base.transform.parent;
                Bounds bounds2 = (parent == null) ? NGUIMath.CalculateRelativeWidgetBounds(this.container.transform) : NGUIMath.CalculateRelativeWidgetBounds(parent, this.container.transform);
                this.mRect.x = bounds2.min.x;
                this.mRect.y = bounds2.min.y;
                this.mRect.width = bounds2.size.x;
                this.mRect.height = bounds2.size.y;
            }
            else if (this.uiCamera != null)
            {
                this.mRect = this.uiCamera.pixelRect;
                if (this.mRoot != null)
                {
                    pixelSizeAdjustment = this.mRoot.pixelSizeAdjustment;
                }
            }
            else
            {
                return;
            }
            float width = this.mRect.width;
            float height = this.mRect.height;
            if ((pixelSizeAdjustment != 1f) && (height > 1f))
            {
                float num5 = ((float) this.mRoot.activeHeight) / height;
                width *= num5;
                height *= num5;
            }
            Vector3 one = (this.mWidget == null) ? this.mTrans.localScale : new Vector3((float) this.mWidget.width, (float) this.mWidget.height);
            if (this.style == Style.BasedOnHeight)
            {
                one.x = this.relativeSize.x * height;
                one.y = this.relativeSize.y * height;
            }
            else if (this.style == Style.FillKeepingRatio)
            {
                float num6 = width / height;
                float num7 = this.initialSize.x / this.initialSize.y;
                if (num7 < num6)
                {
                    float num8 = width / this.initialSize.x;
                    one.x = width;
                    one.y = this.initialSize.y * num8;
                }
                else
                {
                    float num9 = height / this.initialSize.y;
                    one.x = this.initialSize.x * num9;
                    one.y = height;
                }
            }
            else if (this.style == Style.FitInternalKeepingRatio)
            {
                float num10 = width / height;
                float num11 = this.initialSize.x / this.initialSize.y;
                if (num11 > num10)
                {
                    float num12 = width / this.initialSize.x;
                    one.x = width;
                    one.y = this.initialSize.y * num12;
                }
                else
                {
                    float num13 = height / this.initialSize.y;
                    one.x = this.initialSize.x * num13;
                    one.y = height;
                }
            }
            else
            {
                if (this.style != Style.Vertical)
                {
                    one.x = this.relativeSize.x * width;
                }
                if (this.style != Style.Horizontal)
                {
                    one.y = this.relativeSize.y * height;
                }
            }
            if (this.mSprite != null)
            {
                float num14 = (this.mSprite.atlas == null) ? 1f : this.mSprite.atlas.pixelSize;
                one.x -= this.borderPadding.x * num14;
                one.y -= this.borderPadding.y * num14;
                if (this.style != Style.Vertical)
                {
                    this.mSprite.width = Mathf.RoundToInt(one.x);
                }
                if (this.style != Style.Horizontal)
                {
                    this.mSprite.height = Mathf.RoundToInt(one.y);
                }
                one = Vector3.one;
            }
            else if (this.mWidget != null)
            {
                if (this.style != Style.Vertical)
                {
                    this.mWidget.width = Mathf.RoundToInt(one.x - this.borderPadding.x);
                }
                if (this.style != Style.Horizontal)
                {
                    this.mWidget.height = Mathf.RoundToInt(one.y - this.borderPadding.y);
                }
                one = Vector3.one;
            }
            else if (this.mPanel != null)
            {
                Vector4 baseClipRegion = this.mPanel.baseClipRegion;
                if (this.style != Style.Vertical)
                {
                    baseClipRegion.z = one.x - this.borderPadding.x;
                }
                if (this.style != Style.Horizontal)
                {
                    baseClipRegion.w = one.y - this.borderPadding.y;
                }
                this.mPanel.baseClipRegion = baseClipRegion;
                one = Vector3.one;
            }
            else
            {
                if (this.style != Style.Vertical)
                {
                    one.x -= this.borderPadding.x;
                }
                if (this.style != Style.Horizontal)
                {
                    one.y -= this.borderPadding.y;
                }
            }
            if (this.mTrans.localScale != one)
            {
                this.mTrans.localScale = one;
            }
            if (this.runOnlyOnce && Application.isPlaying)
            {
                base.enabled = false;
            }
        }
    }

    public enum Style
    {
        None,
        Horizontal,
        Vertical,
        Both,
        BasedOnHeight,
        FillKeepingRatio,
        FitInternalKeepingRatio
    }
}

