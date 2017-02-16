using System;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Interaction/Drag Object")]
public class UIDragObject : MonoBehaviour
{
    public UIRect contentRect;
    public DragEffect dragEffect = DragEffect.MomentumAndSpring;
    private Bounds mBounds;
    private Vector3 mLastPos;
    private Vector3 mMomentum = Vector3.zero;
    public float momentumAmount = 35f;
    private Plane mPlane;
    private bool mPressed;
    private Vector3 mScroll = Vector3.zero;
    private bool mStarted;
    private Vector3 mTargetPos;
    private int mTouchID;
    public UIPanel panelRegion;
    public bool restrictWithinPanel;
    [SerializeField]
    protected Vector3 scale = new Vector3(1f, 1f, 0f);
    public Vector3 scrollMomentum = Vector3.zero;
    [HideInInspector, SerializeField]
    private float scrollWheelFactor;
    public Transform target;

    public void CancelMovement()
    {
        if (this.target != null)
        {
            Vector3 localPosition = this.target.localPosition;
            localPosition.x = Mathf.RoundToInt(localPosition.x);
            localPosition.y = Mathf.RoundToInt(localPosition.y);
            localPosition.z = Mathf.RoundToInt(localPosition.z);
            this.target.localPosition = localPosition;
        }
        this.mTargetPos = (this.target == null) ? Vector3.zero : this.target.position;
        this.mMomentum = Vector3.zero;
        this.mScroll = Vector3.zero;
    }

    public void CancelSpring()
    {
        SpringPosition component = this.target.GetComponent<SpringPosition>();
        if (component != null)
        {
            component.enabled = false;
        }
    }

    private void FindPanel()
    {
        this.panelRegion = (this.target == null) ? null : UIPanel.Find(this.target.transform.parent);
        if (this.panelRegion == null)
        {
            this.restrictWithinPanel = false;
        }
    }

    private void LateUpdate()
    {
        if (this.target != null)
        {
            float deltaTime = RealTime.deltaTime;
            this.mMomentum -= this.mScroll;
            this.mScroll = NGUIMath.SpringLerp(this.mScroll, Vector3.zero, 20f, deltaTime);
            if (this.mMomentum.magnitude >= 0.0001f)
            {
                if (!this.mPressed)
                {
                    if (this.panelRegion == null)
                    {
                        this.FindPanel();
                    }
                    this.Move(NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime));
                    if (this.restrictWithinPanel && (this.panelRegion != null))
                    {
                        this.UpdateBounds();
                        if (this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, this.dragEffect == DragEffect.None))
                        {
                            this.CancelMovement();
                        }
                        else
                        {
                            this.CancelSpring();
                        }
                    }
                    NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
                    if (this.mMomentum.magnitude < 0.0001f)
                    {
                        this.CancelMovement();
                    }
                }
                else
                {
                    NGUIMath.SpringDampen(ref this.mMomentum, 9f, deltaTime);
                }
            }
        }
    }

    private void Move(Vector3 worldDelta)
    {
        if (this.panelRegion != null)
        {
            this.mTargetPos += worldDelta;
            this.target.position = this.mTargetPos;
            Vector3 localPosition = this.target.localPosition;
            localPosition.x = Mathf.Round(localPosition.x);
            localPosition.y = Mathf.Round(localPosition.y);
            this.target.localPosition = localPosition;
            UIScrollView component = this.panelRegion.GetComponent<UIScrollView>();
            if (component != null)
            {
                component.UpdateScrollbars(true);
            }
        }
        else
        {
            this.target.position += worldDelta;
        }
    }

    private void OnDisable()
    {
        this.mStarted = false;
    }

    private void OnDrag(Vector2 delta)
    {
        if (((this.mPressed && (this.mTouchID == UICamera.currentTouchID)) && (base.enabled && NGUITools.GetActive(base.gameObject))) && (this.target != null))
        {
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
            Ray ray = UICamera.currentCamera.ScreenPointToRay((Vector3) UICamera.currentTouch.pos);
            float enter = 0f;
            if (this.mPlane.Raycast(ray, out enter))
            {
                Vector3 point = ray.GetPoint(enter);
                Vector3 direction = point - this.mLastPos;
                this.mLastPos = point;
                if (!this.mStarted)
                {
                    this.mStarted = true;
                    direction = Vector3.zero;
                }
                if ((direction.x != 0f) || (direction.y != 0f))
                {
                    direction = this.target.InverseTransformDirection(direction);
                    direction.Scale(this.scale);
                    direction = this.target.TransformDirection(direction);
                }
                if (this.dragEffect != DragEffect.None)
                {
                    this.mMomentum = Vector3.Lerp(this.mMomentum, this.mMomentum + ((Vector3) (direction * (0.01f * this.momentumAmount))), 0.67f);
                }
                Vector3 localPosition = this.target.localPosition;
                this.Move(direction);
                if (this.restrictWithinPanel)
                {
                    this.mBounds.center += this.target.localPosition - localPosition;
                    if ((this.dragEffect != DragEffect.MomentumAndSpring) && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, true))
                    {
                        this.CancelMovement();
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        if (this.scrollWheelFactor != 0f)
        {
            this.scrollMomentum = (Vector3) (this.scale * this.scrollWheelFactor);
            this.scrollWheelFactor = 0f;
        }
        if (((this.contentRect == null) && (this.target != null)) && Application.isPlaying)
        {
            UIWidget component = this.target.GetComponent<UIWidget>();
            if (component != null)
            {
                this.contentRect = component;
            }
        }
    }

    private void OnPress(bool pressed)
    {
        if ((base.enabled && NGUITools.GetActive(base.gameObject)) && (this.target != null))
        {
            if (pressed)
            {
                if (!this.mPressed)
                {
                    this.mTouchID = UICamera.currentTouchID;
                    this.mPressed = true;
                    this.mStarted = false;
                    this.CancelMovement();
                    if (this.restrictWithinPanel && (this.panelRegion == null))
                    {
                        this.FindPanel();
                    }
                    if (this.restrictWithinPanel)
                    {
                        this.UpdateBounds();
                    }
                    this.CancelSpring();
                    Transform transform = UICamera.currentCamera.transform;
                    this.mPlane = new Plane((Vector3) (((this.panelRegion == null) ? transform.rotation : this.panelRegion.cachedTransform.rotation) * Vector3.back), UICamera.lastWorldPosition);
                }
            }
            else if (this.mPressed && (this.mTouchID == UICamera.currentTouchID))
            {
                this.mPressed = false;
                if ((this.restrictWithinPanel && (this.dragEffect == DragEffect.MomentumAndSpring)) && this.panelRegion.ConstrainTargetToBounds(this.target, ref this.mBounds, false))
                {
                    this.CancelMovement();
                }
            }
        }
    }

    private void OnScroll(float delta)
    {
        if (base.enabled && NGUITools.GetActive(base.gameObject))
        {
            this.mScroll -= (Vector3) (this.scrollMomentum * (delta * 0.05f));
        }
    }

    private void UpdateBounds()
    {
        if (this.contentRect != null)
        {
            Matrix4x4 worldToLocalMatrix = this.panelRegion.cachedTransform.worldToLocalMatrix;
            Vector3[] worldCorners = this.contentRect.worldCorners;
            for (int i = 0; i < 4; i++)
            {
                worldCorners[i] = worldToLocalMatrix.MultiplyPoint3x4(worldCorners[i]);
            }
            this.mBounds = new Bounds(worldCorners[0], Vector3.zero);
            for (int j = 1; j < 4; j++)
            {
                this.mBounds.Encapsulate(worldCorners[j]);
            }
        }
        else
        {
            this.mBounds = NGUIMath.CalculateRelativeWidgetBounds(this.panelRegion.cachedTransform, this.target);
        }
    }

    public Vector3 dragMovement
    {
        get => 
            this.scale;
        set
        {
            this.scale = value;
        }
    }

    public enum DragEffect
    {
        None,
        Momentum,
        MomentumAndSpring
    }
}

