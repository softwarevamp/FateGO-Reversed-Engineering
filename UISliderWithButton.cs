using System;
using UnityEngine;

public class UISliderWithButton : UIProgressBar
{
    [HideInInspector, SerializeField]
    private Direction direction = Direction.Upgraded;
    [SerializeField, HideInInspector]
    private Transform foreground;
    private bool isTouchEnable = true;
    [SerializeField]
    protected UILabel maxLabel;
    protected int maxStep;
    [SerializeField]
    protected UILabel maxValue;
    [SerializeField]
    protected UILabel minLabel;
    protected int minStep;
    [SerializeField]
    protected UICommonButton minusButton;
    [SerializeField]
    protected UILabel minValue;
    [HideInInspector, SerializeField]
    protected bool mInverted;
    protected int nowStep;
    [SerializeField]
    protected UICommonButton plusButton;
    [HideInInspector, SerializeField]
    private float rawValue = 1f;
    private int valueChange;
    private const int ValueMinus = -1;
    private const int ValueNone = 0;
    private const int ValuePlus = 1;

    public void grayMode()
    {
        this.isTouchEnable = false;
        this.plusButton.SetColliderEnable(false, true);
        this.minusButton.SetColliderEnable(false, true);
        TweenColor.Begin(base.thumb.gameObject, 0.5f, Color.gray);
        TweenColor.Begin(this.plusButton.gameObject, 0.5f, Color.gray);
        TweenColor.Begin(this.minusButton.gameObject, 0.5f, Color.gray);
        TweenColor.Begin(this.minLabel.gameObject, 0.5f, Color.gray);
        TweenColor.Begin(this.maxLabel.gameObject, 0.5f, Color.gray);
        TweenColor.Begin(this.minValue.gameObject, 0.5f, Color.gray);
        TweenColor.Begin(this.maxValue.gameObject, 0.5f, Color.gray);
        TweenColor.Begin(base.mBG.gameObject, 0.5f, Color.gray);
        TweenColor.Begin(base.mFG.gameObject, 0.5f, Color.gray);
    }

    public void init(int max)
    {
        this.minLabel.text = LocalizationManager.Get("SHOP_BULK_WINDOW_MIN_LABEL");
        this.maxLabel.text = LocalizationManager.Get("SHOP_BULK_WINDOW_MAX_LABEL");
        this.minStep = 1;
        this.nowStep = 1;
        this.valueChange = 0;
        this.maxStep = max;
        base.numberOfSteps = this.maxStep;
        base.value = 0f;
        this.minValue.text = this.minStep.ToString();
        this.maxValue.text = LocalizationManager.GetNumberFormat(this.maxStep);
        this.ForceUpdate();
    }

    public void normalMode()
    {
        this.isTouchEnable = true;
        this.plusButton.SetColliderEnable(true, true);
        this.minusButton.SetColliderEnable(true, true);
        TweenColor.Begin(base.thumb.gameObject, 0.5f, Color.white);
        TweenColor.Begin(this.plusButton.gameObject, 0.5f, Color.white);
        TweenColor.Begin(this.minusButton.gameObject, 0.5f, Color.white);
        TweenColor.Begin(this.minLabel.gameObject, 0.5f, Color.white);
        TweenColor.Begin(this.maxLabel.gameObject, 0.5f, Color.white);
        TweenColor.Begin(this.minValue.gameObject, 0.5f, Color.white);
        TweenColor.Begin(this.maxValue.gameObject, 0.5f, Color.white);
        TweenColor.Begin(base.mBG.gameObject, 0.5f, Color.white);
        TweenColor.Begin(base.mFG.gameObject, 0.5f, Color.white);
    }

    public void OnClickMinusButton()
    {
        if (this.isTouchEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.valueChange = -1;
            float num = 1f / ((float) (this.maxStep - 1));
            if ((base.value - num) < 0f)
            {
                base.value = 0f;
            }
            else
            {
                base.value -= num;
            }
        }
    }

    public void OnClickPlusButton()
    {
        if (this.isTouchEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.valueChange = 1;
            float num = 1f / ((float) (this.maxStep - 1));
            if ((base.value + num) > 1f)
            {
                base.value = 1f;
            }
            else
            {
                base.value += num;
            }
        }
    }

    protected void OnDragBackground(GameObject go, Vector2 delta)
    {
        if ((UICamera.currentScheme != UICamera.ControlScheme.Controller) && this.isTouchEnable)
        {
            base.mCam = UICamera.currentCamera;
            base.value = base.ScreenToValue(UICamera.lastTouchPosition);
        }
    }

    protected void OnDragForeground(GameObject go, Vector2 delta)
    {
        if ((UICamera.currentScheme != UICamera.ControlScheme.Controller) && this.isTouchEnable)
        {
            base.mCam = UICamera.currentCamera;
            base.value = base.mOffset + base.ScreenToValue(UICamera.lastTouchPosition);
        }
    }

    protected void OnKey(KeyCode key)
    {
        if (base.enabled)
        {
            float num = (base.numberOfSteps <= 1f) ? 0.125f : (1f / ((float) (base.numberOfSteps - 1)));
            switch (base.mFill)
            {
                case UIProgressBar.FillDirection.LeftToRight:
                    if (key != KeyCode.LeftArrow)
                    {
                        if (key == KeyCode.RightArrow)
                        {
                            base.value = base.mValue + num;
                        }
                        break;
                    }
                    base.value = base.mValue - num;
                    break;

                case UIProgressBar.FillDirection.RightToLeft:
                    if (key != KeyCode.LeftArrow)
                    {
                        if (key == KeyCode.RightArrow)
                        {
                            base.value = base.mValue - num;
                        }
                        break;
                    }
                    base.value = base.mValue + num;
                    break;

                case UIProgressBar.FillDirection.BottomToTop:
                    if (key != KeyCode.DownArrow)
                    {
                        if (key == KeyCode.UpArrow)
                        {
                            base.value = base.mValue + num;
                        }
                        break;
                    }
                    base.value = base.mValue - num;
                    break;

                case UIProgressBar.FillDirection.TopToBottom:
                    if (key != KeyCode.DownArrow)
                    {
                        if (key == KeyCode.UpArrow)
                        {
                            base.value = base.mValue - num;
                        }
                        break;
                    }
                    base.value = base.mValue + num;
                    break;
            }
        }
    }

    protected void OnPressBackground(GameObject go, bool isPressed)
    {
        if ((UICamera.currentScheme != UICamera.ControlScheme.Controller) && this.isTouchEnable)
        {
            base.mCam = UICamera.currentCamera;
            base.value = base.ScreenToValue(UICamera.lastTouchPosition);
            if (!isPressed && (base.onDragFinished != null))
            {
                base.onDragFinished();
            }
        }
    }

    protected void OnPressForeground(GameObject go, bool isPressed)
    {
        if ((UICamera.currentScheme != UICamera.ControlScheme.Controller) && this.isTouchEnable)
        {
            if (isPressed)
            {
                base.thumb.localScale = new Vector3(2f, 2f, 1f);
                base.mBG.transform.localScale = new Vector3(1f, 2f, 1f);
                base.mFG.transform.localScale = new Vector3(1f, 2f, 1f);
            }
            else
            {
                base.thumb.localScale = new Vector3(1f, 1f, 1f);
                base.mBG.transform.localScale = new Vector3(1f, 1f, 1f);
                base.mFG.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            base.mCam = UICamera.currentCamera;
            if (isPressed)
            {
                base.mOffset = (base.mFG != null) ? (base.value - base.ScreenToValue(UICamera.lastTouchPosition)) : 0f;
            }
            else if (base.onDragFinished != null)
            {
                base.onDragFinished();
            }
        }
    }

    protected override void OnStart()
    {
        GameObject go = ((base.mBG == null) || ((base.mBG.GetComponent<Collider>() == null) && (base.mBG.GetComponent<Collider2D>() == null))) ? base.gameObject : base.mBG.gameObject;
        UIEventListener listener = UIEventListener.Get(go);
        listener.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
        listener.onDrag = (UIEventListener.VectorDelegate) Delegate.Combine(listener.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
        if (((base.thumb != null) && ((base.thumb.GetComponent<Collider>() != null) || (base.thumb.GetComponent<Collider2D>() != null))) && ((base.mFG == null) || (base.thumb != base.mFG.cachedTransform)))
        {
            UIEventListener listener2 = UIEventListener.Get(base.thumb.gameObject);
            listener2.onPress = (UIEventListener.BoolDelegate) Delegate.Combine(listener2.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
            listener2.onDrag = (UIEventListener.VectorDelegate) Delegate.Combine(listener2.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
        }
    }

    public int sliderValueChange()
    {
        if (this.valueChange == 0)
        {
            this.nowStep = ((int) (base.value * (this.maxStep - this.minStep))) + this.minStep;
        }
        else
        {
            this.nowStep += this.valueChange;
            if (this.nowStep < this.minStep)
            {
                this.nowStep = this.minStep;
            }
            else if (this.nowStep > this.maxStep)
            {
                this.nowStep = this.maxStep;
            }
        }
        this.valueChange = 0;
        return this.nowStep;
    }

    protected override void Upgrade()
    {
        if (this.direction != Direction.Upgraded)
        {
            base.mValue = this.rawValue;
            if (this.foreground != null)
            {
                base.mFG = this.foreground.GetComponent<UIWidget>();
            }
            if (this.direction == Direction.Horizontal)
            {
                base.mFill = !this.mInverted ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft;
            }
            else
            {
                base.mFill = !this.mInverted ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom;
            }
            this.direction = Direction.Upgraded;
        }
    }

    [Obsolete("Use 'fillDirection' instead")]
    public bool inverted
    {
        get => 
            base.isInverted;
        set
        {
        }
    }

    [Obsolete("Use 'value' instead")]
    public float sliderValue
    {
        get => 
            base.value;
        set
        {
            base.value = value;
        }
    }

    private enum Direction
    {
        Horizontal,
        Vertical,
        Upgraded
    }
}

