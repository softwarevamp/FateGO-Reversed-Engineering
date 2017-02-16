using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/JoyStick")]
public class UIJoystick : MonoBehaviour
{
    public bool circularPadConstraint;
    public float deadZone = 4f;
    private bool mDragStarted;
    private Vector2 mDragStartOffset;
    private Vector3 mLastPos;
    private Plane mPlane;
    private bool mPressed;
    private Vector3 mStartLocalPos;
    private Vector3 mStartPos;
    public float padAngle;
    public Vector2 padPosition;
    public Vector3 padPositionAndAngle;
    public Vector2 range = new Vector2(100f, 100f);
    public Vector3 scale = Vector3.one;
    public float springBackSpeed = 20f;
    private bool started;
    private Vector3 startOffset;
    private Transform target;
    private Vector3 totalOffset;
    private Vector3 totalWorldOffset;

    private void LateUpdate()
    {
        if (this.started)
        {
            Vector3 localPosition = this.target.transform.localPosition;
            if (!this.circularPadConstraint)
            {
                localPosition.x = Mathf.Clamp(localPosition.x, this.mStartLocalPos.x - this.range.x, this.mStartLocalPos.x + this.range.x);
                localPosition.y = Mathf.Clamp(localPosition.y, this.mStartLocalPos.y - this.range.y, this.mStartLocalPos.y + this.range.y);
            }
            else
            {
                localPosition = Vector3.ClampMagnitude(localPosition, this.range.x);
            }
            this.target.transform.localPosition = localPosition;
            Vector3 vector2 = localPosition - this.mStartLocalPos;
            if (vector2.magnitude <= this.deadZone)
            {
                this.padPosition = Vector2.zero;
                this.padAngle = 0f;
                this.padPositionAndAngle = Vector3.zero;
            }
            else
            {
                this.padPosition.x = vector2.x / this.range.x;
                this.padPosition.y = vector2.y / this.range.y;
                this.padAngle = (Mathf.Atan2(this.padPosition.x, this.padPosition.y) * 180f) / 3.14159f;
                this.padPositionAndAngle.x = this.padPosition.x;
                this.padPositionAndAngle.y = this.padPosition.y;
                this.padPositionAndAngle.z = this.padAngle;
            }
        }
    }

    private void OnDrag(Vector2 delta)
    {
        if (base.enabled && NGUITools.GetActive(base.gameObject))
        {
            UICamera.currentTouch.clickNotification = UICamera.ClickNotification.BasedOnDelta;
            if (!this.mDragStarted)
            {
                this.mDragStarted = true;
                this.mLastPos = UICamera.lastHit.point;
            }
            Ray ray = UICamera.currentCamera.ScreenPointToRay((Vector3) UICamera.currentTouch.pos);
            float enter = 0f;
            if (this.mPlane.Raycast(ray, out enter))
            {
                Vector3 point = ray.GetPoint(enter);
                Vector3 direction = point - this.mLastPos;
                this.mLastPos = point;
                if ((direction.x != 0f) || (direction.y != 0f))
                {
                    direction = this.target.InverseTransformDirection(direction);
                    direction.Scale(this.scale);
                    direction = this.target.TransformDirection(direction);
                }
                this.totalOffset += direction;
                this.target.position = this.mStartPos + this.totalOffset;
            }
        }
    }

    private void OnPress(bool pressed)
    {
        if (base.enabled && NGUITools.GetActive(base.gameObject))
        {
            this.mPressed = pressed;
            if (pressed)
            {
                if (!this.started)
                {
                    this.started = true;
                    this.mStartPos = base.transform.position;
                    this.mStartLocalPos = base.transform.localPosition;
                }
                base.transform.position = this.mStartPos;
                this.mDragStarted = false;
                this.totalOffset = Vector3.zero;
                Transform transform = UICamera.currentCamera.transform;
                this.mPlane = new Plane((Vector3) (transform.rotation * Vector3.back), this.mLastPos);
            }
        }
    }

    private void Start()
    {
        this.target = base.transform;
    }

    private void Update()
    {
        if (!this.mPressed && Vector3AlmostEquals(this.target.position, this.mStartPos, 0.1f))
        {
            this.target.position = Vector3.Lerp(this.target.position, this.mStartPos, Time.deltaTime * this.springBackSpeed);
        }
    }

    private static bool Vector3AlmostEquals(Vector3 target, Vector3 second, float sqrMagniturePrecision)
    {
        Vector3 vector = target - second;
        return (vector.sqrMagnitude < sqrMagniturePrecision);
    }
}

