using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Tooltip")]
public class UITooltip : MonoBehaviour
{
    public float appearSpeed = 10f;
    public UISprite background;
    protected float mCurrent;
    protected GameObject mHover;
    protected static UITooltip mInstance;
    protected Vector3 mPos;
    protected Vector3 mSize = Vector3.zero;
    protected float mTarget;
    protected Transform mTrans;
    protected UIWidget[] mWidgets;
    public bool scalingTransitions = true;
    public UILabel text;
    public Camera uiCamera;

    private void Awake()
    {
        mInstance = this;
    }

    public static void Hide()
    {
        if (mInstance != null)
        {
            mInstance.mHover = null;
            mInstance.mTarget = 0f;
        }
    }

    private void OnDestroy()
    {
        mInstance = null;
    }

    protected virtual void SetAlpha(float val)
    {
        int index = 0;
        int length = this.mWidgets.Length;
        while (index < length)
        {
            UIWidget widget = this.mWidgets[index];
            Color color = widget.color;
            color.a = val;
            widget.color = color;
            index++;
        }
    }

    protected virtual void SetText(string tooltipText)
    {
        if ((this.text != null) && !string.IsNullOrEmpty(tooltipText))
        {
            this.mTarget = 1f;
            this.mHover = UICamera.hoveredObject;
            this.text.text = tooltipText;
            this.mPos = Input.mousePosition;
            Transform transform = this.text.transform;
            Vector3 localPosition = transform.localPosition;
            Vector3 localScale = transform.localScale;
            this.mSize = (Vector3) this.text.printedSize;
            this.mSize.x *= localScale.x;
            this.mSize.y *= localScale.y;
            if (this.background != null)
            {
                Vector4 border = this.background.border;
                this.mSize.x += (border.x + border.z) + ((localPosition.x - border.x) * 2f);
                this.mSize.y += (border.y + border.w) + ((-localPosition.y - border.y) * 2f);
                this.background.width = Mathf.RoundToInt(this.mSize.x);
                this.background.height = Mathf.RoundToInt(this.mSize.y);
            }
            if (this.uiCamera != null)
            {
                this.mPos.x = Mathf.Clamp01(this.mPos.x / ((float) Screen.width));
                this.mPos.y = Mathf.Clamp01(this.mPos.y / ((float) Screen.height));
                float num = this.uiCamera.orthographicSize / this.mTrans.parent.lossyScale.y;
                float num2 = (Screen.height * 0.5f) / num;
                Vector2 vector4 = new Vector2((num2 * this.mSize.x) / ((float) Screen.width), (num2 * this.mSize.y) / ((float) Screen.height));
                this.mPos.x = Mathf.Min(this.mPos.x, 1f - vector4.x);
                this.mPos.y = Mathf.Max(this.mPos.y, vector4.y);
                this.mTrans.position = this.uiCamera.ViewportToWorldPoint(this.mPos);
                this.mPos = this.mTrans.localPosition;
                this.mPos.x = Mathf.Round(this.mPos.x);
                this.mPos.y = Mathf.Round(this.mPos.y);
                this.mTrans.localPosition = this.mPos;
            }
            else
            {
                if ((this.mPos.x + this.mSize.x) > Screen.width)
                {
                    this.mPos.x = Screen.width - this.mSize.x;
                }
                if ((this.mPos.y - this.mSize.y) < 0f)
                {
                    this.mPos.y = this.mSize.y;
                }
                this.mPos.x -= Screen.width * 0.5f;
                this.mPos.y -= Screen.height * 0.5f;
            }
        }
        else
        {
            this.mHover = null;
            this.mTarget = 0f;
        }
    }

    public static void Show(string text)
    {
        if (mInstance != null)
        {
            mInstance.SetText(text);
        }
    }

    [Obsolete("Use UITooltip.Show instead")]
    public static void ShowText(string text)
    {
        if (mInstance != null)
        {
            mInstance.SetText(text);
        }
    }

    protected virtual void Start()
    {
        this.mTrans = base.transform;
        this.mWidgets = base.GetComponentsInChildren<UIWidget>();
        this.mPos = this.mTrans.localPosition;
        if (this.uiCamera == null)
        {
            this.uiCamera = NGUITools.FindCameraForLayer(base.gameObject.layer);
        }
        this.SetAlpha(0f);
    }

    protected virtual void Update()
    {
        if (this.mHover != UICamera.hoveredObject)
        {
            this.mHover = null;
            this.mTarget = 0f;
        }
        if (this.mCurrent != this.mTarget)
        {
            this.mCurrent = Mathf.Lerp(this.mCurrent, this.mTarget, RealTime.deltaTime * this.appearSpeed);
            if (Mathf.Abs((float) (this.mCurrent - this.mTarget)) < 0.001f)
            {
                this.mCurrent = this.mTarget;
            }
            this.SetAlpha(this.mCurrent * this.mCurrent);
            if (this.scalingTransitions)
            {
                Vector3 vector = (Vector3) (this.mSize * 0.25f);
                vector.y = -vector.y;
                Vector3 vector2 = (Vector3) (Vector3.one * (1.5f - (this.mCurrent * 0.5f)));
                Vector3 vector3 = Vector3.Lerp(this.mPos - vector, this.mPos, this.mCurrent);
                this.mTrans.localPosition = vector3;
                this.mTrans.localScale = vector2;
            }
        }
    }

    public static bool isVisible =>
        ((mInstance != null) && (mInstance.mTarget == 1f));
}

