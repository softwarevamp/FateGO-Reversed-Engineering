using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Input Field")]
public class UIInput : MonoBehaviour
{
    public Color activeTextColor = Color.white;
    public Color caretColor = new Color(1f, 1f, 1f, 0.8f);
    public int characterLimit;
    public static UIInput current;
    public bool hideInput;
    public InputType inputType;
    public KeyboardType keyboardType;
    public UILabel label;
    [NonSerialized]
    protected Texture2D mBlankTex;
    [NonSerialized]
    protected string mCached = string.Empty;
    [NonSerialized]
    protected UITexture mCaret;
    [NonSerialized]
    protected Color mDefaultColor = Color.white;
    [NonSerialized]
    protected string mDefaultText = string.Empty;
    [NonSerialized]
    protected bool mDoInit = true;
    protected static int mDrawStart;
    [NonSerialized]
    protected UITexture mHighlight;
    protected static TouchScreenKeyboard mKeyboard;
    [NonSerialized]
    protected float mLastAlpha;
    protected static string mLastIME = string.Empty;
    [NonSerialized]
    protected bool mLoadSavedValue = true;
    [NonSerialized]
    protected float mNextBlink;
    [NonSerialized]
    protected UIWidget.Pivot mPivot;
    [NonSerialized]
    protected float mPosition;
    [NonSerialized]
    protected int mSelectionEnd;
    [NonSerialized]
    protected int mSelectionStart;
    [NonSerialized]
    protected int mSelectMe = -1;
    [HideInInspector, SerializeField]
    protected string mValue;
    private static bool mWaitForKeyboard;
    public List<EventDelegate> onChange = new List<EventDelegate>();
    public OnReturnKey onReturnKey;
    public List<EventDelegate> onSubmit = new List<EventDelegate>();
    public OnValidate onValidate;
    public string savedAs;
    [NonSerialized]
    public bool selectAllTextOnFocus = true;
    public static UIInput selection;
    public Color selectionColor = new Color(1f, 0.8745098f, 0.5529412f, 0.5f);
    [SerializeField, HideInInspector]
    private GameObject selectOnTab;
    public Validation validation;

    protected virtual void Cleanup()
    {
        if (this.mHighlight != null)
        {
            this.mHighlight.enabled = false;
        }
        if (this.mCaret != null)
        {
            this.mCaret.enabled = false;
        }
        if (this.mBlankTex != null)
        {
            NGUITools.Destroy(this.mBlankTex);
            this.mBlankTex = null;
        }
    }

    protected void DoBackspace()
    {
        if (!string.IsNullOrEmpty(this.mValue))
        {
            if (this.mSelectionStart == this.mSelectionEnd)
            {
                if (this.mSelectionStart < 1)
                {
                    return;
                }
                this.mSelectionEnd--;
            }
            this.Insert(string.Empty);
        }
    }

    protected void ExecuteOnChange()
    {
        if ((current == null) && EventDelegate.IsValid(this.onChange))
        {
            current = this;
            EventDelegate.Execute(this.onChange);
            current = null;
        }
    }

    protected int GetCharUnderMouse()
    {
        float num;
        Vector3[] worldCorners = this.label.worldCorners;
        Ray currentRay = UICamera.currentRay;
        Plane plane = new Plane(worldCorners[0], worldCorners[1], worldCorners[2]);
        return (!plane.Raycast(currentRay, out num) ? 0 : (mDrawStart + this.label.GetCharacterIndexAtPosition(currentRay.GetPoint(num), false)));
    }

    protected string GetLeftText()
    {
        int length = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
        return ((!string.IsNullOrEmpty(this.mValue) && (length >= 0)) ? this.mValue.Substring(0, length) : string.Empty);
    }

    protected string GetRightText()
    {
        int startIndex = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
        return ((!string.IsNullOrEmpty(this.mValue) && (startIndex < this.mValue.Length)) ? this.mValue.Substring(startIndex) : string.Empty);
    }

    protected string GetSelection()
    {
        if (string.IsNullOrEmpty(this.mValue) || (this.mSelectionStart == this.mSelectionEnd))
        {
            return string.Empty;
        }
        int startIndex = Mathf.Min(this.mSelectionStart, this.mSelectionEnd);
        int num2 = Mathf.Max(this.mSelectionStart, this.mSelectionEnd);
        return this.mValue.Substring(startIndex, num2 - startIndex);
    }

    protected void Init()
    {
        if (this.mDoInit && (this.label != null))
        {
            this.mDoInit = false;
            this.mDefaultText = this.label.text;
            this.mDefaultColor = this.label.color;
            this.label.supportEncoding = false;
            if (this.label.alignment == NGUIText.Alignment.Justified)
            {
                this.label.alignment = NGUIText.Alignment.Left;
                Debug.LogWarning("Input fields using labels with justified alignment are not supported at this time", this);
            }
            this.mPivot = this.label.pivot;
            this.mPosition = this.label.cachedTransform.localPosition.x;
            this.UpdateLabel();
        }
    }

    protected virtual void Insert(string text)
    {
        string leftText = this.GetLeftText();
        string rightText = this.GetRightText();
        int length = rightText.Length;
        StringBuilder builder = new StringBuilder((leftText.Length + rightText.Length) + text.Length);
        builder.Append(leftText);
        int num2 = 0;
        int num3 = text.Length;
        while (num2 < num3)
        {
            char addedChar = text[num2];
            if (addedChar == '\b')
            {
                this.DoBackspace();
            }
            else
            {
                if ((this.characterLimit > 0) && ((builder.Length + length) >= this.characterLimit))
                {
                    break;
                }
                if (this.onValidate != null)
                {
                    addedChar = this.onValidate(builder.ToString(), builder.Length, addedChar);
                }
                else if (this.validation != Validation.None)
                {
                    addedChar = this.Validate(builder.ToString(), builder.Length, addedChar);
                }
                if (addedChar != '\0')
                {
                    builder.Append(addedChar);
                }
            }
            num2++;
        }
        this.mSelectionStart = builder.Length;
        this.mSelectionEnd = this.mSelectionStart;
        int num4 = 0;
        int num5 = rightText.Length;
        while (num4 < num5)
        {
            char ch2 = rightText[num4];
            if (this.onValidate != null)
            {
                ch2 = this.onValidate(builder.ToString(), builder.Length, ch2);
            }
            else if (this.validation != Validation.None)
            {
                ch2 = this.Validate(builder.ToString(), builder.Length, ch2);
            }
            if (ch2 != '\0')
            {
                builder.Append(ch2);
            }
            num4++;
        }
        this.mValue = builder.ToString();
        this.UpdateLabel();
        this.ExecuteOnChange();
    }

    public void LoadValue()
    {
        if (!string.IsNullOrEmpty(this.savedAs))
        {
            string str = this.mValue.Replace(@"\n", "\n");
            this.mValue = string.Empty;
            this.value = !PlayerPrefs.HasKey(this.savedAs) ? str : PlayerPrefs.GetString(this.savedAs);
        }
    }

    protected void OnDeselectEvent()
    {
        if (this.mDoInit)
        {
            this.Init();
        }
        if ((this.label != null) && NGUITools.GetActive(this))
        {
            this.mValue = this.value;
            if (mKeyboard != null)
            {
                mWaitForKeyboard = false;
                mKeyboard.active = false;
                mKeyboard = null;
            }
            if (string.IsNullOrEmpty(this.mValue))
            {
                this.label.text = this.mDefaultText;
                this.label.color = this.mDefaultColor;
            }
            else
            {
                this.label.text = this.mValue;
            }
            Input.imeCompositionMode = IMECompositionMode.Auto;
            this.RestoreLabelPivot();
        }
        selection = null;
        this.UpdateLabel();
    }

    private void OnDisable()
    {
        this.Cleanup();
    }

    protected virtual void OnDrag(Vector2 delta)
    {
        if ((this.label != null) && ((UICamera.currentScheme == UICamera.ControlScheme.Mouse) || (UICamera.currentScheme == UICamera.ControlScheme.Touch)))
        {
            this.selectionEnd = this.GetCharUnderMouse();
        }
    }

    protected virtual void OnPress(bool isPressed)
    {
        if (((isPressed && this.isSelected) && (this.label != null)) && ((UICamera.currentScheme == UICamera.ControlScheme.Mouse) || (UICamera.currentScheme == UICamera.ControlScheme.Touch)))
        {
            this.selectionEnd = this.GetCharUnderMouse();
            if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
            {
                this.selectionStart = this.mSelectionEnd;
            }
        }
    }

    protected virtual void OnSelect(bool isSelected)
    {
        if (isSelected)
        {
            this.OnSelectEvent();
        }
        else
        {
            this.OnDeselectEvent();
        }
    }

    protected void OnSelectEvent()
    {
        selection = this;
        if (this.mDoInit)
        {
            this.Init();
        }
        if ((this.label != null) && NGUITools.GetActive(this))
        {
            this.mSelectMe = Time.frameCount;
        }
    }

    public void RemoveFocus()
    {
        this.isSelected = false;
    }

    protected void RestoreLabelPivot()
    {
        if ((this.label != null) && (this.label.pivot != this.mPivot))
        {
            this.label.pivot = this.mPivot;
        }
    }

    protected void SaveToPlayerPrefs(string val)
    {
        if (!string.IsNullOrEmpty(this.savedAs))
        {
            if (string.IsNullOrEmpty(val))
            {
                PlayerPrefs.DeleteKey(this.savedAs);
            }
            else
            {
                PlayerPrefs.SetString(this.savedAs, val);
            }
        }
    }

    public void SaveValue()
    {
        this.SaveToPlayerPrefs(this.mValue);
    }

    protected void SetPivotToLeft()
    {
        Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.mPivot);
        pivotOffset.x = 0f;
        this.label.pivot = NGUIMath.GetPivot(pivotOffset);
    }

    protected void SetPivotToRight()
    {
        Vector2 pivotOffset = NGUIMath.GetPivotOffset(this.mPivot);
        pivotOffset.x = 1f;
        this.label.pivot = NGUIMath.GetPivot(pivotOffset);
    }

    private void Start()
    {
        if (this.selectOnTab != null)
        {
            if (base.GetComponent<UIKeyNavigation>() == null)
            {
                base.gameObject.AddComponent<UIKeyNavigation>().onDown = this.selectOnTab;
            }
            this.selectOnTab = null;
            NGUITools.SetDirty(this);
        }
        if (this.mLoadSavedValue && !string.IsNullOrEmpty(this.savedAs))
        {
            this.LoadValue();
        }
        else
        {
            this.value = this.mValue.Replace(@"\n", "\n");
        }
    }

    public void Submit()
    {
        if (NGUITools.GetActive(this))
        {
            this.mValue = this.value;
            if (current == null)
            {
                current = this;
                EventDelegate.Execute(this.onSubmit);
                current = null;
            }
            this.SaveToPlayerPrefs(this.mValue);
        }
    }

    protected virtual void Update()
    {
        if (this.isSelected)
        {
            if (this.mDoInit)
            {
                this.Init();
            }
            if (mWaitForKeyboard)
            {
                if ((mKeyboard != null) && !mKeyboard.active)
                {
                    return;
                }
                mWaitForKeyboard = false;
            }
            if ((this.mSelectMe != -1) && (this.mSelectMe != Time.frameCount))
            {
                this.mSelectMe = -1;
                this.mSelectionEnd = !string.IsNullOrEmpty(this.mValue) ? this.mValue.Length : 0;
                mDrawStart = 0;
                this.mSelectionStart = !this.selectAllTextOnFocus ? this.mSelectionEnd : 0;
                this.label.color = this.activeTextColor;
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                    case RuntimePlatform.Android:
                    case RuntimePlatform.WP8Player:
                    case RuntimePlatform.BlackBerryPlayer:
                    case RuntimePlatform.MetroPlayerARM:
                    case RuntimePlatform.MetroPlayerX64:
                    case RuntimePlatform.MetroPlayerX86:
                        string mValue;
                        TouchScreenKeyboardType keyboardType;
                        if (this.inputShouldBeHidden)
                        {
                            TouchScreenKeyboard.hideInput = true;
                            keyboardType = (TouchScreenKeyboardType) this.keyboardType;
                            mValue = "|";
                        }
                        else if (this.inputType == InputType.Password)
                        {
                            TouchScreenKeyboard.hideInput = false;
                            keyboardType = TouchScreenKeyboardType.Default;
                            mValue = this.mValue;
                            this.mSelectionStart = this.mSelectionEnd;
                        }
                        else
                        {
                            TouchScreenKeyboard.hideInput = false;
                            keyboardType = (TouchScreenKeyboardType) this.keyboardType;
                            mValue = this.mValue;
                            this.mSelectionStart = this.mSelectionEnd;
                        }
                        mWaitForKeyboard = true;
                        mKeyboard = (this.inputType != InputType.Password) ? TouchScreenKeyboard.Open(mValue, keyboardType, !this.inputShouldBeHidden && (this.inputType == InputType.AutoCorrect), this.label.multiLine && !this.hideInput, false, false, this.defaultText) : TouchScreenKeyboard.Open(mValue, keyboardType, false, false, true);
                        break;

                    default:
                    {
                        Vector2 vector = ((UICamera.current == null) || (UICamera.current.cachedCamera == null)) ? this.label.worldCorners[0] : UICamera.current.cachedCamera.WorldToScreenPoint(this.label.worldCorners[0]);
                        vector.y = Screen.height - vector.y;
                        Input.imeCompositionMode = IMECompositionMode.On;
                        Input.compositionCursorPos = vector;
                        break;
                    }
                }
                this.UpdateLabel();
                if (string.IsNullOrEmpty(Input.inputString))
                {
                    return;
                }
            }
            if (mKeyboard != null)
            {
                string text = mKeyboard.text;
                if (this.inputShouldBeHidden)
                {
                    if (text != "|")
                    {
                        if (!string.IsNullOrEmpty(text))
                        {
                            this.Insert(text.Substring(1));
                        }
                        else
                        {
                            this.DoBackspace();
                        }
                        mKeyboard.text = "|";
                    }
                }
                else if (this.mCached != text)
                {
                    this.mCached = text;
                    this.value = text;
                }
                if (mKeyboard.done || !mKeyboard.active)
                {
                    if (!mKeyboard.wasCanceled)
                    {
                        this.Submit();
                    }
                    mKeyboard = null;
                    this.isSelected = false;
                    this.mCached = string.Empty;
                }
            }
            else
            {
                string compositionString = Input.compositionString;
                if (string.IsNullOrEmpty(compositionString) && !string.IsNullOrEmpty(Input.inputString))
                {
                    foreach (char ch in Input.inputString)
                    {
                        if ((((ch >= ' ') && (ch != 0xf700)) && ((ch != 0xf701) && (ch != 0xf702))) && (ch != 0xf703))
                        {
                            this.Insert(ch.ToString());
                        }
                    }
                }
                if (mLastIME != compositionString)
                {
                    this.mSelectionEnd = !string.IsNullOrEmpty(compositionString) ? (this.mValue.Length + compositionString.Length) : this.mSelectionStart;
                    mLastIME = compositionString;
                    this.UpdateLabel();
                    this.ExecuteOnChange();
                }
            }
            if ((this.mCaret != null) && (this.mNextBlink < RealTime.time))
            {
                this.mNextBlink = RealTime.time + 0.5f;
                this.mCaret.enabled = !this.mCaret.enabled;
            }
            if (this.isSelected && (this.mLastAlpha != this.label.finalAlpha))
            {
                this.UpdateLabel();
            }
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if ((this.onReturnKey == OnReturnKey.NewLine) || (((((this.onReturnKey == OnReturnKey.Default) && this.label.multiLine) && (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.RightControl))) && (this.label.overflowMethod != UILabel.Overflow.ClampContent)) && (this.validation == Validation.None)))
                {
                    this.Insert("\n");
                }
                else
                {
                    UICamera.currentScheme = UICamera.ControlScheme.Controller;
                    UICamera.currentKey = KeyCode.Return;
                    this.Submit();
                    UICamera.currentKey = KeyCode.None;
                }
            }
        }
    }

    public void UpdateLabel()
    {
        if (this.label != null)
        {
            string str2;
            if (this.mDoInit)
            {
                this.Init();
            }
            bool isSelected = this.isSelected;
            string str = this.value;
            bool flag2 = string.IsNullOrEmpty(str) && string.IsNullOrEmpty(Input.compositionString);
            this.label.color = (!flag2 || isSelected) ? this.activeTextColor : this.mDefaultColor;
            if (flag2)
            {
                str2 = !isSelected ? this.mDefaultText : string.Empty;
                this.RestoreLabelPivot();
            }
            else
            {
                if (this.inputType == InputType.Password)
                {
                    str2 = string.Empty;
                    string str3 = "*";
                    if (((this.label.bitmapFont != null) && (this.label.bitmapFont.bmFont != null)) && (this.label.bitmapFont.bmFont.GetGlyph(0x2a) == null))
                    {
                        str3 = "x";
                    }
                    int num = 0;
                    int num2 = str.Length;
                    while (num < num2)
                    {
                        str2 = str2 + str3;
                        num++;
                    }
                }
                else
                {
                    str2 = str;
                }
                int length = !isSelected ? 0 : Mathf.Min(str2.Length, this.cursorPosition);
                string str4 = str2.Substring(0, length);
                if (isSelected)
                {
                    str4 = str4 + Input.compositionString;
                }
                str2 = str4 + str2.Substring(length, str2.Length - length);
                if ((isSelected && (this.label.overflowMethod == UILabel.Overflow.ClampContent)) && (this.label.maxLineCount == 1))
                {
                    int num4 = this.label.CalculateOffsetToFit(str2);
                    if (num4 == 0)
                    {
                        mDrawStart = 0;
                        this.RestoreLabelPivot();
                    }
                    else if (length < mDrawStart)
                    {
                        mDrawStart = length;
                        this.SetPivotToLeft();
                    }
                    else if (num4 < mDrawStart)
                    {
                        mDrawStart = num4;
                        this.SetPivotToLeft();
                    }
                    else
                    {
                        num4 = this.label.CalculateOffsetToFit(str2.Substring(0, length));
                        if (num4 > mDrawStart)
                        {
                            mDrawStart = num4;
                            this.SetPivotToRight();
                        }
                    }
                    if (mDrawStart != 0)
                    {
                        str2 = str2.Substring(mDrawStart, str2.Length - mDrawStart);
                    }
                }
                else
                {
                    mDrawStart = 0;
                    this.RestoreLabelPivot();
                }
            }
            this.label.text = str2;
            if (isSelected && ((mKeyboard == null) || this.inputShouldBeHidden))
            {
                int start = this.mSelectionStart - mDrawStart;
                int end = this.mSelectionEnd - mDrawStart;
                if (this.mBlankTex == null)
                {
                    this.mBlankTex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        {
                            this.mBlankTex.SetPixel(j, i, Color.white);
                        }
                    }
                    this.mBlankTex.Apply();
                }
                if (start != end)
                {
                    if (this.mHighlight == null)
                    {
                        this.mHighlight = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject);
                        this.mHighlight.name = "Input Highlight";
                        this.mHighlight.mainTexture = this.mBlankTex;
                        this.mHighlight.fillGeometry = false;
                        this.mHighlight.pivot = this.label.pivot;
                        this.mHighlight.SetAnchor(this.label.cachedTransform);
                    }
                    else
                    {
                        this.mHighlight.pivot = this.label.pivot;
                        this.mHighlight.mainTexture = this.mBlankTex;
                        this.mHighlight.MarkAsChanged();
                        this.mHighlight.enabled = true;
                    }
                }
                if (this.mCaret == null)
                {
                    this.mCaret = NGUITools.AddWidget<UITexture>(this.label.cachedGameObject);
                    this.mCaret.name = "Input Caret";
                    this.mCaret.mainTexture = this.mBlankTex;
                    this.mCaret.fillGeometry = false;
                    this.mCaret.pivot = this.label.pivot;
                    this.mCaret.SetAnchor(this.label.cachedTransform);
                }
                else
                {
                    this.mCaret.pivot = this.label.pivot;
                    this.mCaret.mainTexture = this.mBlankTex;
                    this.mCaret.MarkAsChanged();
                    this.mCaret.enabled = true;
                }
                if (start != end)
                {
                    this.label.PrintOverlay(start, end, this.mCaret.geometry, this.mHighlight.geometry, this.caretColor, this.selectionColor);
                    this.mHighlight.enabled = this.mHighlight.geometry.hasVertices;
                }
                else
                {
                    this.label.PrintOverlay(start, end, this.mCaret.geometry, null, this.caretColor, this.selectionColor);
                    if (this.mHighlight != null)
                    {
                        this.mHighlight.enabled = false;
                    }
                }
                this.mNextBlink = RealTime.time + 0.5f;
                this.mLastAlpha = this.label.finalAlpha;
            }
            else
            {
                this.Cleanup();
            }
        }
    }

    public string Validate(string val)
    {
        if (string.IsNullOrEmpty(val))
        {
            return string.Empty;
        }
        StringBuilder builder = new StringBuilder(val.Length);
        for (int i = 0; i < val.Length; i++)
        {
            char addedChar = val[i];
            if (this.onValidate != null)
            {
                addedChar = this.onValidate(builder.ToString(), builder.Length, addedChar);
            }
            else if (this.validation != Validation.None)
            {
                addedChar = this.Validate(builder.ToString(), builder.Length, addedChar);
            }
            if (addedChar != '\0')
            {
                builder.Append(addedChar);
            }
        }
        if ((this.characterLimit > 0) && (builder.Length > this.characterLimit))
        {
            return builder.ToString(0, this.characterLimit);
        }
        return builder.ToString();
    }

    protected char Validate(string text, int pos, char ch)
    {
        if ((this.validation == Validation.None) || !base.enabled)
        {
            return ch;
        }
        if (this.validation == Validation.Integer)
        {
            if ((ch >= '0') && (ch <= '9'))
            {
                return ch;
            }
            if (((ch == '-') && (pos == 0)) && !text.Contains("-"))
            {
                return ch;
            }
        }
        else if (this.validation == Validation.Float)
        {
            if ((ch >= '0') && (ch <= '9'))
            {
                return ch;
            }
            if (((ch == '-') && (pos == 0)) && !text.Contains("-"))
            {
                return ch;
            }
            if ((ch == '.') && !text.Contains("."))
            {
                return ch;
            }
        }
        else if (this.validation == Validation.Alphanumeric)
        {
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                return ch;
            }
            if ((ch >= 'a') && (ch <= 'z'))
            {
                return ch;
            }
            if ((ch >= '0') && (ch <= '9'))
            {
                return ch;
            }
        }
        else if (this.validation == Validation.Username)
        {
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                return (char) ((ch - 'A') + 0x61);
            }
            if ((ch >= 'a') && (ch <= 'z'))
            {
                return ch;
            }
            if ((ch >= '0') && (ch <= '9'))
            {
                return ch;
            }
        }
        else if (this.validation == Validation.Name)
        {
            char ch2 = (text.Length <= 0) ? ' ' : text[Mathf.Clamp(pos, 0, text.Length - 1)];
            char ch3 = (text.Length <= 0) ? '\n' : text[Mathf.Clamp(pos + 1, 0, text.Length - 1)];
            if ((ch >= 'a') && (ch <= 'z'))
            {
                if (ch2 == ' ')
                {
                    return (char) ((ch - 'a') + 0x41);
                }
                return ch;
            }
            if ((ch >= 'A') && (ch <= 'Z'))
            {
                if ((ch2 != ' ') && (ch2 != '\''))
                {
                    return (char) ((ch - 'A') + 0x61);
                }
                return ch;
            }
            if (ch == '\'')
            {
                if (((ch2 != ' ') && (ch2 != '\'')) && ((ch3 != '\'') && !text.Contains("'")))
                {
                    return ch;
                }
            }
            else if ((((ch == ' ') && (ch2 != ' ')) && ((ch2 != '\'') && (ch3 != ' '))) && (ch3 != '\''))
            {
                return ch;
            }
        }
        return '\0';
    }

    public UITexture caret =>
        this.mCaret;

    public int cursorPosition
    {
        get
        {
            if ((mKeyboard != null) && !this.inputShouldBeHidden)
            {
                return this.value.Length;
            }
            return (!this.isSelected ? this.value.Length : this.mSelectionEnd);
        }
        set
        {
            if (this.isSelected && ((mKeyboard == null) || this.inputShouldBeHidden))
            {
                this.mSelectionEnd = value;
                this.UpdateLabel();
            }
        }
    }

    public string defaultText
    {
        get
        {
            if (this.mDoInit)
            {
                this.Init();
            }
            return this.mDefaultText;
        }
        set
        {
            if (this.mDoInit)
            {
                this.Init();
            }
            this.mDefaultText = value;
            this.UpdateLabel();
        }
    }

    public bool inputShouldBeHidden =>
        (((this.hideInput && (this.label != null)) && !this.label.multiLine) && (this.inputType != InputType.Password));

    public bool isSelected
    {
        get => 
            (selection == this);
        set
        {
            if (!value)
            {
                if (this.isSelected)
                {
                    UICamera.selectedObject = null;
                }
            }
            else
            {
                UICamera.selectedObject = base.gameObject;
            }
        }
    }

    [Obsolete("Use UIInput.isSelected instead")]
    public bool selected
    {
        get => 
            this.isSelected;
        set
        {
            this.isSelected = value;
        }
    }

    public int selectionEnd
    {
        get
        {
            if ((mKeyboard != null) && !this.inputShouldBeHidden)
            {
                return this.value.Length;
            }
            return (!this.isSelected ? this.value.Length : this.mSelectionEnd);
        }
        set
        {
            if (this.isSelected && ((mKeyboard == null) || this.inputShouldBeHidden))
            {
                this.mSelectionEnd = value;
                this.UpdateLabel();
            }
        }
    }

    public int selectionStart
    {
        get
        {
            if ((mKeyboard != null) && !this.inputShouldBeHidden)
            {
                return 0;
            }
            return (!this.isSelected ? this.value.Length : this.mSelectionStart);
        }
        set
        {
            if (this.isSelected && ((mKeyboard == null) || this.inputShouldBeHidden))
            {
                this.mSelectionStart = value;
                this.UpdateLabel();
            }
        }
    }

    [Obsolete("Use UIInput.value instead")]
    public string text
    {
        get => 
            this.value;
        set
        {
            this.value = value;
        }
    }

    public string value
    {
        get
        {
            if (this.mDoInit)
            {
                this.Init();
            }
            return this.mValue;
        }
        set
        {
            if (this.mDoInit)
            {
                this.Init();
            }
            mDrawStart = 0;
            if (Application.platform == RuntimePlatform.BlackBerryPlayer)
            {
                value = value.Replace(@"\b", "\b");
            }
            value = this.Validate(value);
            if ((this.isSelected && (mKeyboard != null)) && (this.mCached != value))
            {
                mKeyboard.text = value;
                this.mCached = value;
            }
            if (this.mValue != value)
            {
                this.mValue = value;
                this.mLoadSavedValue = false;
                if (this.isSelected)
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.mSelectionStart = 0;
                        this.mSelectionEnd = 0;
                    }
                    else
                    {
                        this.mSelectionStart = value.Length;
                        this.mSelectionEnd = this.mSelectionStart;
                    }
                }
                else
                {
                    this.SaveToPlayerPrefs(value);
                }
                this.UpdateLabel();
                this.ExecuteOnChange();
            }
        }
    }

    public enum InputType
    {
        Standard,
        AutoCorrect,
        Password
    }

    public enum KeyboardType
    {
        Default,
        ASCIICapable,
        NumbersAndPunctuation,
        URL,
        NumberPad,
        PhonePad,
        NamePhonePad,
        EmailAddress
    }

    public enum OnReturnKey
    {
        Default,
        Submit,
        NewLine
    }

    public delegate char OnValidate(string text, int charIndex, char addedChar);

    public enum Validation
    {
        None,
        Integer,
        Float,
        Alphanumeric,
        Username,
        Name
    }
}

