using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScriptMessageManager : MonoBehaviour
{
    protected float beforeTextOnlyLineHeight;
    protected float betweenLineHeight;
    [SerializeField]
    protected Vector2 defaultAllDispCenter = new Vector2(0f, 0f);
    [SerializeField]
    protected Vector2 defaultAllDispSize = new Vector2(940f, 560f);
    [SerializeField]
    protected float defaultBetweenLineHeight = 4f;
    protected string defaultColorTag;
    protected int defaultFontSize;
    [SerializeField]
    protected float defaultKeyDelayTime = 0.2f;
    [SerializeField]
    protected float defaultScrollTime = 0.3f;
    [SerializeField]
    protected float defaultStepTime = 0.03125f;
    protected float defaultTextOnlyLineHeight;
    [SerializeField]
    protected Vector2 defaultWindowDispCenter = new Vector2(0f, -218f);
    [SerializeField]
    protected Vector2 defaultWindowDispSize = new Vector2(880f, 104f);
    protected float dispCountTimer;
    protected int dispIndex;
    private List<ScriptMessageLabel> dispLabelList;
    [SerializeField]
    protected UIPanel dispPanel;
    private Vector2 dispPosition;
    protected Vector2 dispSize = new Vector2();
    [SerializeField]
    protected float fastScrollTime = 0.1f;
    protected int fontSize;
    protected Dictionary<string, int> fontSizes;
    [SerializeField]
    protected GameObject imagePrefab;
    protected Stack<UISprite> imageStock;
    protected bool isBusy;
    protected bool isFastMessageRequest;
    protected bool isMessageOff;
    protected bool isMessageOut;
    protected bool isScroll;
    protected bool isWaitNextTouchDelay;
    protected bool isWaitNextTouchRequest;
    protected bool isWindowBack;
    protected Stack<ScriptMessageLabel> labelStock;
    [SerializeField]
    protected GameObject mainPrefab;
    protected Stack<UILabel> mainStock;
    [SerializeField]
    protected UIWidget messageBackBase;
    [SerializeField]
    protected GameObject messageBase;
    [SerializeField]
    protected Transform messageScroll;
    [SerializeField]
    protected Transform messageShake;
    [SerializeField]
    protected UISprite messageWindowBack;
    [SerializeField]
    protected GameObject nextTouchRootObject;
    [SerializeField]
    protected GameObject rootObject;
    protected UIPanel rootPanel;
    protected int rubyFontSize;
    protected float rubyLineHeight;
    [SerializeField]
    protected GameObject rubyPrefab;
    protected Stack<UILabel> rubyStock;
    private Vector3 scrollPosition;
    protected float shakeCycle;
    protected float shakeTime;
    protected float shakeX;
    protected float shakeY;
    private Vector2 startPosition;
    protected float stepTime;
    protected string talkName;
    [SerializeField]
    protected UISprite talkNameBack;
    [SerializeField]
    protected int talkNameBackBaseWidth = 80;
    [SerializeField]
    protected int talkNameBackDefaultWidth = 220;
    [SerializeField]
    protected UISprite talkNameIcon;
    [SerializeField]
    protected ScriptLineMessage talkNameManager;
    [SerializeField]
    protected GameObject talkNameRootObject;
    protected float textOnlyLineHeight;
    [SerializeField]
    protected UITouchPress touchPress;

    public ScriptMessageManager()
    {
        Dictionary<string, int> dictionary = new Dictionary<string, int> {
            { 
                "-",
                0x1d
            },
            { 
                "small",
                0x18
            },
            { 
                "medium",
                0x1d
            },
            { 
                "large",
                0x30
            },
            { 
                "x-large",
                0x40
            }
        };
        this.fontSizes = dictionary;
        this.defaultColorTag = string.Empty;
        this.mainStock = new Stack<UILabel>();
        this.rubyStock = new Stack<UILabel>();
        this.imageStock = new Stack<UISprite>();
        this.labelStock = new Stack<ScriptMessageLabel>();
        this.dispLabelList = new List<ScriptMessageLabel>();
        this.dispCountTimer = -1f;
    }

    private void AddLabel(string text, float tm, string colorTag, bool isFoward)
    {
        ScriptMessageLabel label = this.FetchLabel();
        label.stepTime = tm;
        label.colorTag = colorTag;
        Vector2 dispPosition = this.dispPosition;
        if (text[0] == '[')
        {
            if (text[1] == '#')
            {
                this.PreProcLabel(label, true, false);
                char[] separator = new char[] { ':' };
                string[] strArray = text.Substring(2, text.Length - 3).Split(separator);
                if (strArray.Length < 2)
                {
                    label.UpdateBouten(ref label.main, ref label.ruby, ref this.dispPosition, this.fontSize, strArray[0]);
                }
                else
                {
                    label.UpdateRuby(ref label.main, ref label.ruby, ref this.dispPosition, this.fontSize, strArray[0], strArray[1]);
                }
                if (tm >= 0f)
                {
                    label.main.text = string.Empty;
                    label.ruby.text = string.Empty;
                }
            }
            else if (text[1] == '^')
            {
                label.image = this.FetchImageSprite();
                char[] chArray2 = new char[] { ' ' };
                string[] strArray2 = text.Substring(2, text.Length - 3).Split(chArray2);
                if (strArray2.Length <= 1)
                {
                    label.UpdateImage(ref label.image, ref this.dispPosition, this.fontSize, strArray2[0]);
                }
                else
                {
                    label.UpdateImage(ref label.image, ref this.dispPosition, this.fontSize, float.Parse(strArray2[1]), strArray2[0]);
                }
                if (tm >= 0f)
                {
                    label.image.alpha = 0f;
                }
            }
            else if (text[1] == '~')
            {
                this.PreProcLabel(label, false, true);
                string s = text.Substring(2, text.Length - 3);
                label.UpdateLine(ref label.image, ref this.dispPosition, this.fontSize, int.Parse(s));
                if (tm >= 0f)
                {
                    label.image.alpha = 0f;
                }
            }
            else
            {
                this.PreProcLabel(label, false, false);
                label.UpdateLabel(ref label.main, ref this.dispPosition, this.fontSize, text);
                if (tm >= 0f)
                {
                    label.main.text = string.Empty;
                }
            }
        }
        else
        {
            this.PreProcLabel(label, false, false);
            label.UpdateLabel(ref label.main, ref this.dispPosition, this.fontSize, text);
            if (tm >= 0f)
            {
                label.main.text = string.Empty;
            }
        }
        if (isFoward)
        {
            label.mainPosition.x -= label.widthSize;
            if (label.rubyText != string.Empty)
            {
                label.rubyPosition.x -= label.widthSize;
            }
            this.dispPosition = dispPosition;
        }
        this.dispLabelList.Add(label);
    }

    public void AddText(string text)
    {
        this.UpdateLabels(text, false, false);
    }

    public void AddTextStretch(string text)
    {
        this.UpdateLabels(text, true, false);
    }

    public void ClearLabels()
    {
        foreach (ScriptMessageLabel label in this.dispLabelList)
        {
            this.ReleaseLabel(label);
        }
        this.dispLabelList.Clear();
        this.dispIndex = 0;
        this.isBusy = false;
        this.dispCountTimer = -1f;
    }

    public void ClearTalkName()
    {
        this.talkNameRootObject.SetActive(false);
        this.talkName = string.Empty;
        this.talkNameManager.ClearText();
        this.talkNameIcon.alpha = 0f;
    }

    public void ClearText()
    {
        this.ClearLabels();
        this.dispPosition = this.startPosition;
        this.scrollPosition.x = -this.dispSize.x / 2f;
        this.scrollPosition.y = this.dispSize.y / 2f;
        this.messageScroll.localPosition = this.scrollPosition;
        this.SetDefaultState();
        this.beforeTextOnlyLineHeight = this.textLineHeight;
        this.isBusy = false;
        this.isScroll = false;
        this.dispCountTimer = -1f;
        this.isWaitNextTouchRequest = false;
        this.isWaitNextTouchDelay = false;
        this.isFastMessageRequest = false;
        this.talkNameRootObject.SetActive(false);
        this.talkName = string.Empty;
        this.talkNameManager.ClearText();
        this.talkNameIcon.alpha = 0f;
        this.nextTouchRootObject.SetActive(false);
    }

    public void DeleteLabels()
    {
        this.ClearLabels();
        while (this.labelStock.Count > 0)
        {
            this.labelStock.Pop().Destroy();
        }
        while (this.mainStock.Count > 0)
        {
            UnityEngine.Object.Destroy(this.mainStock.Pop().gameObject);
        }
        while (this.rubyStock.Count > 0)
        {
            UnityEngine.Object.Destroy(this.rubyStock.Pop().gameObject);
        }
        while (this.imageStock.Count > 0)
        {
            UnityEngine.Object.Destroy(this.imageStock.Pop().gameObject);
        }
    }

    private void EndScroll()
    {
        this.isFastMessageRequest = false;
        this.isScroll = false;
    }

    protected UISprite FetchImageSprite()
    {
        UISprite component;
        if (this.imageStock.Count > 0)
        {
            component = this.imageStock.Pop();
        }
        else
        {
            component = UnityEngine.Object.Instantiate<GameObject>(this.imagePrefab).GetComponent<UISprite>();
            component.transform.parent = this.messageScroll;
            component.gameObject.layer = this.messageScroll.gameObject.layer;
        }
        component.transform.localPosition = Vector3.zero;
        component.transform.localScale = Vector3.one;
        return component;
    }

    private ScriptMessageLabel FetchLabel()
    {
        if (this.labelStock.Count > 0)
        {
            return this.labelStock.Pop();
        }
        return new ScriptMessageLabel();
    }

    protected UILabel FetchMainLabel()
    {
        UILabel component;
        if (this.mainStock.Count > 0)
        {
            component = this.mainStock.Pop();
        }
        else
        {
            component = UnityEngine.Object.Instantiate<GameObject>(this.mainPrefab).GetComponent<UILabel>();
            component.transform.parent = this.messageScroll;
            component.gameObject.layer = this.messageScroll.gameObject.layer;
        }
        component.transform.localPosition = Vector3.zero;
        component.transform.localScale = Vector3.one;
        return component;
    }

    protected UILabel FetchRubyLabel()
    {
        UILabel component;
        if (this.rubyStock.Count > 0)
        {
            component = this.rubyStock.Pop();
        }
        else
        {
            component = UnityEngine.Object.Instantiate<GameObject>(this.rubyPrefab).GetComponent<UILabel>();
            component.transform.parent = this.messageScroll;
            component.gameObject.layer = this.messageScroll.gameObject.layer;
        }
        component.transform.localPosition = Vector3.zero;
        component.transform.localScale = Vector3.one;
        component.fontSize = this.rubyFontSize;
        return component;
    }

    public int InitScreen()
    {
        this.isMessageOut = false;
        this.isMessageOff = false;
        this.rootPanel.alpha = 0f;
        this.rootObject.SetActive(true);
        return this.SetScreen(0, 0, 0, 0, true);
    }

    public bool IsChangeTalkName(string text) => 
        (this.talkName != text);

    public bool IsLongPress() => 
        this.touchPress.IsLongPress;

    public bool IsPageScroll() => 
        ((this.dispPosition.x > this.startPosition.x) || (this.scrollPosition.y < ((this.startPosition.y - this.dispPosition.y) + (this.dispSize.y / 2f))));

    public bool IsReturnScroll() => 
        (((-this.dispPosition.y + this.beforeTextOnlyLineHeight) - this.scrollPosition.y) > (this.dispSize.y - (this.dispSize.y / 2f)));

    public bool IsReturnScroll2() => 
        (((-this.dispPosition.y + (this.beforeTextOnlyLineHeight * 2f)) - this.scrollPosition.y) > (this.dispSize.y - (this.dispSize.y / 2f)));

    public bool IsScroll() => 
        this.isScroll;

    public bool IsShake() => 
        (this.shakeCycle > 0f);

    public bool IsWaitTouch() => 
        this.isWaitNextTouchRequest;

    public bool MessageUpdate()
    {
        if (this.isBusy)
        {
            if (this.dispCountTimer >= 0f)
            {
                float deltaTime = RealTime.deltaTime;
                if (deltaTime < 0.5f)
                {
                    this.dispCountTimer += deltaTime;
                }
            }
            else
            {
                this.dispCountTimer = 0f;
            }
            bool flag = false;
            for (int i = this.dispIndex; i < this.dispLabelList.Count; i++)
            {
                ScriptMessageLabel label = this.dispLabelList[i];
                int len = (!this.isFastMessageRequest && (label.stepTime != 0f)) ? ((int) (this.dispCountTimer / label.stepTime)) : label.dispLength;
                flag = true;
                if (len < label.dispLength)
                {
                    if (label.main != null)
                    {
                        label.main.text = label.colorTag + ScriptMessageLabel.SubstrByDisp(label.mainText, len);
                        float num4 = ((float) len) / ((float) label.dispLength);
                        if (label.rubyText.Length > 0)
                        {
                            label.ruby.text = label.colorTag + ScriptMessageLabel.SubstrByDisp(label.rubyText, (int) Math.Round((double) (ScriptMessageLabel.StrlenByDisp(label.rubyText) * num4)));
                        }
                    }
                    break;
                }
                if (label.main != null)
                {
                    label.main.text = label.colorTag + label.mainText;
                    if (label.rubyText.Length > 0)
                    {
                        label.ruby.text = label.colorTag + label.rubyText;
                    }
                }
                else if (label.imageText.Length > 0)
                {
                    label.image.alpha = 1f;
                }
                if (this.isFastMessageRequest || (label.stepTime == 0f))
                {
                    this.dispCountTimer = 0f;
                }
                else
                {
                    this.dispCountTimer -= label.dispLength * label.stepTime;
                }
                this.dispIndex = i + 1;
            }
            this.isBusy = flag;
        }
        return this.isBusy;
    }

    public void OffScreen()
    {
        this.isMessageOut = false;
        this.rootPanel.alpha = 0f;
    }

    public void OnClickWindow()
    {
        if (!this.isWaitNextTouchRequest && !this.isFastMessageRequest)
        {
        }
    }

    protected void OnDelayWaitNextTouch()
    {
        this.isWaitNextTouchRequest = false;
    }

    public void OnLongPressWindow()
    {
        if (this.isWaitNextTouchRequest && !this.isWaitNextTouchDelay)
        {
            this.isWaitNextTouchDelay = true;
            this.nextTouchRootObject.SetActive(false);
            base.Invoke("OnDelayWaitNextTouch", this.defaultKeyDelayTime);
        }
    }

    public void OnPressWindow()
    {
        if (this.isWaitNextTouchRequest)
        {
            if (!this.isWaitNextTouchDelay)
            {
                this.isWaitNextTouchDelay = true;
                this.nextTouchRootObject.SetActive(false);
                base.Invoke("OnDelayWaitNextTouch", this.defaultKeyDelayTime);
            }
        }
        else if (!this.isFastMessageRequest)
        {
            this.isFastMessageRequest = true;
        }
    }

    protected void OnShake()
    {
        if ((this.shakeCycle > 0f) && ((this.shakeTime == 0f) || (Time.time < this.shakeTime)))
        {
            this.messageShake.localPosition = new Vector3(UnityEngine.Random.Range(-this.shakeX, this.shakeX), UnityEngine.Random.Range(-this.shakeY, this.shakeY), 0f);
            base.Invoke("OnShake", this.shakeCycle);
        }
        else
        {
            base.CancelInvoke("OnShake");
            this.messageShake.localPosition = Vector3.zero;
            this.shakeCycle = 0f;
        }
    }

    public void PageScroll(bool isFast = false)
    {
        if (this.dispPosition.x > this.startPosition.x)
        {
            this.dispPosition.x = this.startPosition.x;
            this.dispPosition.y -= this.textLineHeight + this.betweenLineHeight;
        }
        this.scrollPosition.y = (this.startPosition.y - this.dispPosition.y) + (this.dispSize.y / 2f);
        this.fontSize = this.defaultFontSize;
        this.textOnlyLineHeight = this.defaultTextOnlyLineHeight;
        this.beforeTextOnlyLineHeight = this.textLineHeight;
        this.defaultColorTag = string.Empty;
        this.StartScroll(isFast);
    }

    private void PreProcLabel(ScriptMessageLabel label, bool hasRuby, bool hasImage)
    {
        if (hasImage)
        {
            if (this.imageStock.Count > 0)
            {
                label.image = this.imageStock.Pop();
            }
            else
            {
                label.image = UnityEngine.Object.Instantiate<GameObject>(this.imagePrefab).GetComponent<UISprite>();
                label.image.transform.parent = this.messageScroll;
                label.image.gameObject.layer = this.messageScroll.gameObject.layer;
            }
            label.image.transform.localPosition = Vector3.zero;
            label.image.transform.localScale = Vector3.one;
        }
        else
        {
            if (this.mainStock.Count > 0)
            {
                label.main = this.mainStock.Pop();
            }
            else
            {
                label.main = UnityEngine.Object.Instantiate<GameObject>(this.mainPrefab).GetComponent<UILabel>();
                label.main.transform.parent = this.messageScroll;
                label.main.gameObject.layer = this.messageScroll.gameObject.layer;
            }
            label.main.transform.localPosition = Vector3.zero;
            label.main.transform.localScale = Vector3.one;
            if (hasRuby)
            {
                if (this.rubyStock.Count > 0)
                {
                    label.ruby = this.rubyStock.Pop();
                }
                else
                {
                    label.ruby = UnityEngine.Object.Instantiate<GameObject>(this.rubyPrefab).GetComponent<UILabel>();
                    label.ruby.transform.parent = this.messageScroll;
                    label.ruby.gameObject.layer = this.messageScroll.gameObject.layer;
                }
                label.ruby.transform.localPosition = Vector3.zero;
                label.ruby.transform.localScale = Vector3.one;
                label.ruby.fontSize = this.rubyFontSize;
            }
        }
    }

    public void QuitScreen()
    {
        this.rootPanel.alpha = 0f;
        this.DeleteLabels();
        this.talkNameManager.DeleteLabels();
        this.rootObject.SetActive(false);
        SingletonMonoBehaviour<ScriptManager>.Instance.closeTalk(null);
    }

    private void ReleaseLabel(ScriptMessageLabel label)
    {
        if (label.main != null)
        {
            label.main.text = string.Empty;
            this.mainStock.Push(label.main);
            label.main = null;
        }
        if (label.ruby != null)
        {
            label.ruby.text = string.Empty;
            this.rubyStock.Push(label.ruby);
            label.ruby = null;
        }
        if (label.image != null)
        {
            label.image.alpha = 0f;
            this.imageStock.Push(label.image);
            label.image = null;
        }
        label.Release();
        this.labelStock.Push(label);
    }

    public void RequestFastMessage()
    {
        this.isFastMessageRequest = true;
    }

    public void ResetLongPress()
    {
        this.touchPress.PressReset();
        this.isFastMessageRequest = false;
    }

    public bool ReturnScroll(bool isFast = false)
    {
        if (!this.IsReturnScroll())
        {
            return false;
        }
        this.scrollPosition.y = ((-this.dispPosition.y + this.beforeTextOnlyLineHeight) - this.dispSize.y) + (this.dispSize.y / 2f);
        this.StartScroll(isFast);
        return true;
    }

    public bool ReturnScroll2(bool isFast = false)
    {
        if (!this.IsReturnScroll2())
        {
            return false;
        }
        this.scrollPosition.y = ((-this.dispPosition.y + (this.beforeTextOnlyLineHeight * 2f)) - this.dispSize.y) + (this.dispSize.y / 2f);
        this.StartScroll(isFast);
        return true;
    }

    public void ReturnText()
    {
        this.beforeTextOnlyLineHeight = this.textLineHeight;
        this.dispPosition.x = this.startPosition.x;
        this.dispPosition.y -= this.textLineHeight + this.betweenLineHeight;
        this.SetDefaultState();
    }

    public void SetBetweenLineHeight(float height)
    {
        this.betweenLineHeight = (height < 0f) ? this.defaultBetweenLineHeight : height;
    }

    protected void SetDefaultState()
    {
        this.fontSize = this.defaultFontSize;
        this.textOnlyLineHeight = this.defaultTextOnlyLineHeight;
        this.betweenLineHeight = this.defaultBetweenLineHeight;
        this.defaultColorTag = string.Empty;
        this.stepTime = this.defaultStepTime;
    }

    public void SetFontSize(string sizeName)
    {
        if (sizeName != null)
        {
            this.fontSize = this.fontSizes[sizeName];
        }
        else
        {
            this.fontSize = this.fontSizes["-"];
        }
        if (this.textOnlyLineHeight < this.fontSize)
        {
            this.textOnlyLineHeight = this.fontSize;
        }
    }

    public void SetMessageOffMode(bool flag)
    {
        this.isMessageOff = flag;
        this.rootPanel.alpha = (!this.isMessageOut || this.isMessageOff) ? ((float) 0) : ((float) 1);
    }

    public int SetScreen(int x, int y, int w, int h, bool isWindowBack)
    {
        this.isMessageOut = false;
        this.rootPanel.alpha = 0f;
        if (w == 0)
        {
            if (isWindowBack)
            {
                this.dispSize = this.defaultWindowDispSize;
                this.messageBase.transform.localPosition = new Vector3(this.defaultWindowDispCenter.x, this.defaultWindowDispCenter.y);
            }
            else
            {
                this.dispSize = this.defaultAllDispSize;
                this.messageBase.transform.localPosition = new Vector3(this.defaultAllDispCenter.x, this.defaultAllDispCenter.y);
            }
        }
        else
        {
            this.dispSize.Set((float) w, (float) h);
            this.messageBase.transform.localPosition = new Vector3((float) x, (float) y, 0f);
        }
        this.startPosition = new Vector2(0f, -(this.rubyLineHeight + 2f));
        this.isWindowBack = isWindowBack;
        if (isWindowBack)
        {
            this.dispPanel.baseClipRegion = new Vector4(0f, 0f, (float) ManagerConfig.WIDTH, this.dispSize.y);
        }
        else
        {
            this.dispPanel.baseClipRegion = new Vector4(0f, 0f, (float) ManagerConfig.WIDTH, this.dispSize.y);
        }
        this.messageWindowBack.alpha = !isWindowBack ? ((float) 0) : ((float) 1);
        this.ClearText();
        return (x - ((int) (this.dispSize.x / 2f)));
    }

    public void SetSpeed(float n)
    {
        if (n > 0f)
        {
            this.stepTime = 1f / n;
        }
        else if (n < 0f)
        {
            this.stepTime = this.defaultStepTime;
        }
        else
        {
            this.stepTime = 0f;
        }
    }

    public bool SetTalkName(string imageName, string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            this.ClearTalkName();
        }
        else
        {
            this.talkName = text;
            if (this.isWindowBack)
            {
                this.talkNameRootObject.SetActive(true);
                this.talkNameManager.SetText(text);
                Vector2 printedSize = this.talkNameManager.GetPrintedSize();
                this.talkNameBack.width = ((printedSize.x <= this.talkNameBackDefaultWidth) ? this.talkNameBackDefaultWidth : ((int) printedSize.x)) + this.talkNameBackBaseWidth;
                if (imageName != null)
                {
                    this.talkNameIcon.spriteName = imageName;
                    this.talkNameIcon.alpha = 1f;
                }
                else
                {
                    this.talkNameIcon.alpha = 0f;
                }
            }
            else
            {
                if (imageName != null)
                {
                    text = "[image " + imageName + " 1.5]" + text;
                }
                this.fontSize = (int) (this.defaultFontSize * 1f);
                this.dispPosition.x = this.startPosition.x - 50f;
                this.UpdateLabels(text, true, false);
                this.ReturnText();
                this.talkNameIcon.alpha = 0f;
            }
            this.isMessageOut = true;
            this.rootPanel.alpha = (!this.isMessageOut || this.isMessageOff) ? ((float) 0) : ((float) 1);
        }
        return true;
    }

    public void SetText(string text)
    {
        this.ClearText();
        this.UpdateLabels(text, true, false);
    }

    public void Shake(float duration, float cycle, float x, float y)
    {
        this.shakeTime = (duration <= 0f) ? 0f : (Time.time + duration);
        this.shakeCycle = cycle;
        this.shakeX = x;
        this.shakeY = y;
        this.OnShake();
    }

    private void Start()
    {
        this.rootPanel = this.rootObject.GetComponent<UIPanel>();
        this.defaultFontSize = this.fontSizes["-"];
        UILabel component = UnityEngine.Object.Instantiate<GameObject>(this.mainPrefab).GetComponent<UILabel>();
        UILabel label2 = UnityEngine.Object.Instantiate<GameObject>(this.rubyPrefab).GetComponent<UILabel>();
        component.fontSize = this.defaultFontSize;
        component.text = "■";
        this.defaultTextOnlyLineHeight = component.localSize.y;
        this.rubyFontSize = label2.fontSize;
        label2.text = "■";
        this.rubyLineHeight = label2.localSize.y;
        UnityEngine.Object.Destroy(component.gameObject);
        UnityEngine.Object.Destroy(label2.gameObject);
        this.SetScreen(0, 0, 0, 0, true);
    }

    protected void StartScroll(bool isFast)
    {
        float num = !isFast ? this.defaultScrollTime : this.fastScrollTime;
        if (num > 0f)
        {
            this.isScroll = true;
            TweenPosition position = TweenPosition.Begin(this.messageScroll.gameObject, !isFast ? this.defaultScrollTime : this.fastScrollTime, this.scrollPosition);
            position.method = UITweener.Method.EaseInOut;
            position.eventReceiver = base.gameObject;
            position.callWhenFinished = "EndScroll";
        }
        else
        {
            TweenPosition component = this.messageScroll.GetComponent<TweenPosition>();
            if (component != null)
            {
                component.enabled = false;
            }
            this.messageScroll.transform.localPosition = this.scrollPosition;
            this.EndScroll();
        }
    }

    protected void UpdateLabels(string txt, bool isStretch, bool isFoward)
    {
        <UpdateLabels>c__AnonStorey6C storeyc = new <UpdateLabels>c__AnonStorey6C {
            isFoward = isFoward,
            <>f__this = this
        };
        if (!this.isBusy)
        {
            this.isBusy = true;
            this.dispCountTimer = -1f;
        }
        this.isMessageOut = true;
        this.rootPanel.alpha = (!this.isMessageOut || this.isMessageOff) ? ((float) 0) : ((float) 1);
        storeyc.stepTime = !isStretch ? this.stepTime : 0f;
        storeyc.tmpTxt = string.Empty;
        storeyc.tmpColorTag = this.defaultColorTag;
        int startIndex = 0;
        ProcAddLabel label = new ProcAddLabel(storeyc.<>m__77);
        ProcAddLabel2 label2 = new ProcAddLabel2(storeyc.<>m__78);
        while (startIndex < txt.Length)
        {
            if (txt[startIndex] != '[')
            {
                goto Label_02FC;
            }
            int braceIndex = ScriptMessageLabel.GetBraceIndex(txt, startIndex + 1);
            if (braceIndex == -1)
            {
                throw new Exception("括弧がありません");
            }
            if (txt[startIndex + 1] == '%')
            {
                label();
                string str2 = ScriptReplaceString.GetString(int.Parse(txt.Substring(startIndex + 2, (braceIndex - startIndex) - 2)));
                this.defaultColorTag = storeyc.tmpColorTag;
                this.UpdateLabels(str2, isStretch, storeyc.isFoward);
                storeyc.tmpColorTag = this.defaultColorTag;
            }
            else if (txt[startIndex + 1] == '&')
            {
                label();
                label2(ScriptMessageLabel.GetTagSplitString(txt, startIndex + 2, ScriptReplaceString.GetPlayerGenderIndex()));
            }
            else if (txt[startIndex + 1] == '#')
            {
                label();
                label2(txt.Substring(startIndex, (braceIndex - startIndex) + 1));
            }
            else
            {
                string commandName = ScriptMessageLabel.GetCommandName(txt, startIndex + 1);
                switch (commandName)
                {
                    case "image":
                    {
                        label();
                        string str4 = txt.Substring(startIndex + 7, (braceIndex - startIndex) - 7);
                        label2("[^" + str4 + "]");
                        goto Label_02F3;
                    }
                    case "i":
                    {
                        label();
                        string str5 = txt.Substring(startIndex + 3, (braceIndex - startIndex) - 3);
                        label2("[^" + str5 + "]");
                        goto Label_02F3;
                    }
                    case "r":
                        label();
                        this.ReturnText();
                        storeyc.tmpColorTag = this.defaultColorTag;
                        goto Label_02F3;
                }
                if (commandName.StartsWith("line"))
                {
                    label();
                    string str6 = txt.Substring(startIndex + 5, (braceIndex - startIndex) - 5);
                    if (str6.Length > 0)
                    {
                        label2("[~" + str6 + "]");
                    }
                    else
                    {
                        label2("[~1]");
                    }
                }
                else
                {
                    storeyc.tmpColorTag = txt.Substring(startIndex, (braceIndex - startIndex) + 1);
                    storeyc.tmpTxt = storeyc.tmpTxt + storeyc.tmpColorTag;
                }
            }
        Label_02F3:
            startIndex = braceIndex + 1;
            continue;
        Label_02FC:
            storeyc.tmpTxt = storeyc.tmpTxt + txt[startIndex++];
        }
        label();
    }

    public void WaitNextTouch()
    {
        if (!this.isWaitNextTouchRequest && !this.touchPress.IsLongPress)
        {
            this.isWaitNextTouchRequest = true;
            this.isWaitNextTouchDelay = false;
            this.nextTouchRootObject.SetActive(true);
        }
    }

    public bool IsBusy =>
        this.isBusy;

    public bool IsWindowMode =>
        this.isWindowBack;

    protected float textLineHeight =>
        (this.textOnlyLineHeight + this.rubyLineHeight);

    [CompilerGenerated]
    private sealed class <UpdateLabels>c__AnonStorey6C
    {
        internal ScriptMessageManager <>f__this;
        internal bool isFoward;
        internal float stepTime;
        internal string tmpColorTag;
        internal string tmpTxt;

        internal void <>m__77()
        {
            if (this.tmpTxt.Length > 0)
            {
                this.<>f__this.AddLabel(this.tmpTxt, this.stepTime, this.<>f__this.defaultColorTag, this.isFoward);
                this.tmpTxt = string.Empty;
            }
            this.<>f__this.defaultColorTag = this.tmpColorTag;
        }

        internal void <>m__78(string text)
        {
            if (text.Length > 0)
            {
                this.<>f__this.AddLabel(text, this.stepTime, this.<>f__this.defaultColorTag, this.isFoward);
            }
            this.<>f__this.defaultColorTag = this.tmpColorTag;
        }
    }

    private delegate void ProcAddLabel();

    private delegate void ProcAddLabel2(string txt);
}

