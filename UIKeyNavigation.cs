using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Key Navigation")]
public class UIKeyNavigation : MonoBehaviour
{
    public Constraint constraint;
    public static BetterList<UIKeyNavigation> list = new BetterList<UIKeyNavigation>();
    public GameObject onClick;
    public GameObject onDown;
    public GameObject onLeft;
    public GameObject onRight;
    public GameObject onUp;
    public bool startsSelected;

    protected GameObject Get(Vector3 myDir, bool horizontal)
    {
        Transform transform = base.transform;
        myDir = transform.TransformDirection(myDir);
        Vector3 center = GetCenter(base.gameObject);
        float maxValue = float.MaxValue;
        GameObject gameObject = null;
        for (int i = 0; i < list.size; i++)
        {
            UIKeyNavigation navigation = list[i];
            if (navigation != this)
            {
                UIButton component = navigation.GetComponent<UIButton>();
                if ((component == null) || component.isEnabled)
                {
                    Vector3 direction = GetCenter(navigation.gameObject) - center;
                    if (Vector3.Dot(myDir, direction.normalized) >= 0.707f)
                    {
                        direction = transform.InverseTransformDirection(direction);
                        if (horizontal)
                        {
                            direction.y *= 2f;
                        }
                        else
                        {
                            direction.x *= 2f;
                        }
                        float sqrMagnitude = direction.sqrMagnitude;
                        if (sqrMagnitude <= maxValue)
                        {
                            gameObject = navigation.gameObject;
                            maxValue = sqrMagnitude;
                        }
                    }
                }
            }
        }
        return gameObject;
    }

    protected static Vector3 GetCenter(GameObject go)
    {
        UIWidget component = go.GetComponent<UIWidget>();
        UICamera camera = UICamera.FindCameraForLayer(go.layer);
        if (camera != null)
        {
            Vector3 position = go.transform.position;
            if (component != null)
            {
                Vector3[] worldCorners = component.worldCorners;
                position = (Vector3) ((worldCorners[0] + worldCorners[2]) * 0.5f);
            }
            position = camera.cachedCamera.WorldToScreenPoint(position);
            position.z = 0f;
            return position;
        }
        if (component != null)
        {
            Vector3[] vectorArray2 = component.worldCorners;
            return (Vector3) ((vectorArray2[0] + vectorArray2[2]) * 0.5f);
        }
        return go.transform.position;
    }

    protected GameObject GetDown()
    {
        if (NGUITools.GetActive(this.onDown))
        {
            return this.onDown;
        }
        if ((this.constraint != Constraint.Horizontal) && (this.constraint != Constraint.Explicit))
        {
            return this.Get(Vector3.down, false);
        }
        return null;
    }

    protected GameObject GetLeft()
    {
        if (NGUITools.GetActive(this.onLeft))
        {
            return this.onLeft;
        }
        if ((this.constraint != Constraint.Vertical) && (this.constraint != Constraint.Explicit))
        {
            return this.Get(Vector3.left, true);
        }
        return null;
    }

    private GameObject GetRight()
    {
        if (NGUITools.GetActive(this.onRight))
        {
            return this.onRight;
        }
        if ((this.constraint != Constraint.Vertical) && (this.constraint != Constraint.Explicit))
        {
            return this.Get(Vector3.right, true);
        }
        return null;
    }

    protected GameObject GetUp()
    {
        if (NGUITools.GetActive(this.onUp))
        {
            return this.onUp;
        }
        if ((this.constraint != Constraint.Horizontal) && (this.constraint != Constraint.Explicit))
        {
            return this.Get(Vector3.up, false);
        }
        return null;
    }

    protected virtual void OnClick()
    {
        if (NGUITools.GetActive(this) && NGUITools.GetActive(this.onClick))
        {
            UICamera.selectedObject = this.onClick;
        }
    }

    protected virtual void OnDisable()
    {
        list.Remove(this);
    }

    protected virtual void OnEnable()
    {
        list.Add(this);
        if (this.startsSelected && ((UICamera.selectedObject == null) || !NGUITools.GetActive(UICamera.selectedObject)))
        {
            UICamera.currentScheme = UICamera.ControlScheme.Controller;
            UICamera.selectedObject = base.gameObject;
        }
    }

    protected virtual void OnKey(KeyCode key)
    {
        if (NGUITools.GetActive(this))
        {
            GameObject up = null;
            switch (key)
            {
                case KeyCode.UpArrow:
                    up = this.GetUp();
                    break;

                case KeyCode.DownArrow:
                    up = this.GetDown();
                    break;

                case KeyCode.RightArrow:
                    up = this.GetRight();
                    break;

                case KeyCode.LeftArrow:
                    up = this.GetLeft();
                    break;

                case KeyCode.Tab:
                    if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                    {
                        up = this.GetRight();
                        if (up == null)
                        {
                            up = this.GetDown();
                        }
                        if (up == null)
                        {
                            up = this.GetUp();
                        }
                        if (up == null)
                        {
                            up = this.GetLeft();
                        }
                        break;
                    }
                    up = this.GetLeft();
                    if (up == null)
                    {
                        up = this.GetUp();
                    }
                    if (up == null)
                    {
                        up = this.GetDown();
                    }
                    if (up == null)
                    {
                        up = this.GetRight();
                    }
                    break;
            }
            if (up != null)
            {
                UICamera.selectedObject = up;
            }
        }
    }

    public enum Constraint
    {
        None,
        Vertical,
        Horizontal,
        Explicit
    }
}

