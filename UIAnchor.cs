using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/Anchor")]
public class UIAnchor : MonoBehaviour
{
    public GameObject container;
    private Animation mAnim;
    private Rect mRect = new Rect();
    private UIRoot mRoot;
    private bool mStarted;
    private Transform mTrans;
    public Vector2 pixelOffset = Vector2.zero;
    public Vector2 relativeOffset = Vector2.zero;
    public bool runOnlyOnce = true;
    public Side side = Side.Center;
    public Camera uiCamera;
    [HideInInspector, SerializeField]
    private UIWidget widgetContainer;

    private void Awake()
    {
        this.mTrans = base.transform;
        this.mAnim = base.GetComponent<Animation>();
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
        this.mRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
        if (this.uiCamera == null)
        {
            this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
        this.Update();
        this.mStarted = true;
    }

    private void Update()
    {
        if (((this.mAnim == null) || !this.mAnim.enabled) || !this.mAnim.isPlaying)
        {
            bool flag = false;
            UIWidget component = this.container?.GetComponent<UIWidget>();
            UIPanel panel = ((this.container != null) || (component != null)) ? this.container.GetComponent<UIPanel>() : null;
            if (component != null)
            {
                Bounds bounds = component.CalculateBounds(this.container.transform.parent);
                this.mRect.x = bounds.min.x;
                this.mRect.y = bounds.min.y;
                this.mRect.width = bounds.size.x;
                this.mRect.height = bounds.size.y;
            }
            else if (panel != null)
            {
                if (panel.clipping == UIDrawCall.Clipping.None)
                {
                    float num = (this.mRoot == null) ? 0.5f : ((((float) this.mRoot.activeHeight) / ((float) Screen.height)) * 0.5f);
                    this.mRect.xMin = -Screen.width * num;
                    this.mRect.yMin = -Screen.height * num;
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
                Transform parent = this.container.transform.parent;
                Bounds bounds2 = (parent == null) ? NGUIMath.CalculateRelativeWidgetBounds(this.container.transform) : NGUIMath.CalculateRelativeWidgetBounds(parent, this.container.transform);
                this.mRect.x = bounds2.min.x;
                this.mRect.y = bounds2.min.y;
                this.mRect.width = bounds2.size.x;
                this.mRect.height = bounds2.size.y;
            }
            else if (this.uiCamera != null)
            {
                flag = true;
                this.mRect = this.uiCamera.pixelRect;
            }
            else
            {
                return;
            }
            float x = (this.mRect.xMin + this.mRect.xMax) * 0.5f;
            float y = (this.mRect.yMin + this.mRect.yMax) * 0.5f;
            Vector3 position = new Vector3(x, y, 0f);
            if (this.side != Side.Center)
            {
                if (((this.side == Side.Right) || (this.side == Side.TopRight)) || (this.side == Side.BottomRight))
                {
                    position.x = this.mRect.xMax;
                }
                else if (((this.side == Side.Top) || (this.side == Side.Center)) || (this.side == Side.Bottom))
                {
                    position.x = x;
                }
                else
                {
                    position.x = this.mRect.xMin;
                }
                if (((this.side == Side.Top) || (this.side == Side.TopRight)) || (this.side == Side.TopLeft))
                {
                    position.y = this.mRect.yMax;
                }
                else if (((this.side == Side.Left) || (this.side == Side.Center)) || (this.side == Side.Right))
                {
                    position.y = y;
                }
                else
                {
                    position.y = this.mRect.yMin;
                }
            }
            float width = this.mRect.width;
            float height = this.mRect.height;
            position.x += this.pixelOffset.x + (this.relativeOffset.x * width);
            position.y += this.pixelOffset.y + (this.relativeOffset.y * height);
            if (flag)
            {
                if (this.uiCamera.orthographic)
                {
                    position.x = Mathf.Round(position.x);
                    position.y = Mathf.Round(position.y);
                }
                position.z = this.uiCamera.WorldToScreenPoint(this.mTrans.position).z;
                position = this.uiCamera.ScreenToWorldPoint(position);
            }
            else
            {
                position.x = Mathf.Round(position.x);
                position.y = Mathf.Round(position.y);
                if (panel != null)
                {
                    position = panel.cachedTransform.TransformPoint(position);
                }
                else if (this.container != null)
                {
                    Transform transform2 = this.container.transform.parent;
                    if (transform2 != null)
                    {
                        position = transform2.TransformPoint(position);
                    }
                }
                position.z = this.mTrans.position.z;
            }
            if (this.mTrans.position != position)
            {
                this.mTrans.position = position;
            }
            if (this.runOnlyOnce && Application.isPlaying)
            {
                base.enabled = false;
            }
        }
    }

    public enum Side
    {
        BottomLeft,
        Left,
        TopLeft,
        Top,
        TopRight,
        Right,
        BottomRight,
        Bottom,
        Center
    }
}

