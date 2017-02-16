using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScriptLineMessage : MonoBehaviour
{
    protected float beforeTextOnlyLineHeight;
    protected float betweenLineHeight;
    [SerializeField]
    protected float defaultBetweenLineHeight = 5f;
    protected string defaultColorTag;
    [SerializeField]
    protected int defaultFontSize;
    protected float defaultTextOnlyLineHeight;
    [SerializeField]
    protected int depthOffset;
    private List<ScriptMessageLabel> dispLabelList;
    private Vector2 dispPosition;
    [SerializeField]
    protected float fontScale = 1f;
    protected int fontSize;
    protected Dictionary<string, int> fontSizes;
    [SerializeField]
    protected GameObject imagePrefab;
    protected Stack<UISprite> imageStock;
    protected bool isInit;
    [SerializeField]
    protected bool isRecycle;
    protected Stack<ScriptMessageLabel> labelStock;
    [SerializeField]
    protected GameObject mainPrefab;
    protected Stack<UILabel> mainStock;
    private Vector2 maxDispPosition;
    [SerializeField]
    protected Transform messageOffset;
    [SerializeField]
    protected Transform messageRoot;
    [SerializeField]
    protected int rubyFontSize;
    protected float rubyLineHeight;
    [SerializeField]
    protected GameObject rubyPrefab;
    protected Stack<UILabel> rubyStock;
    private Vector2 startPosition;
    protected float textOnlyLineHeight;

    public ScriptLineMessage()
    {
        Dictionary<string, int> dictionary = new Dictionary<string, int> {
            { 
                "-",
                30
            },
            { 
                "small",
                0x18
            },
            { 
                "medium",
                30
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
        this.dispLabelList = new List<ScriptMessageLabel>();
    }

    private void AddLabel(string text, string colorTag, bool isFoward)
    {
        ScriptMessageLabel item = this.FetchLabel();
        item.stepTime = 0f;
        item.colorTag = colorTag;
        if (text[0] == '[')
        {
            if (text[1] == '#')
            {
                item.main = this.FetchMainLabel();
                item.ruby = this.FetchRubyLabel();
                char[] separator = new char[] { ':' };
                string[] strArray = text.Substring(2, text.Length - 3).Split(separator);
                if (strArray.Length < 2)
                {
                    item.UpdateBouten(ref item.main, ref item.ruby, ref this.dispPosition, this.fontSize, strArray[0]);
                }
                else
                {
                    item.UpdateRuby(ref item.main, ref item.ruby, ref this.dispPosition, this.fontSize, strArray[0], strArray[1]);
                }
            }
            else if (text[1] == '^')
            {
                item.image = this.FetchImageSprite();
                string imageStr = text.Substring(2, text.Length - 3);
                char[] chArray2 = new char[] { ' ' };
                string[] strArray2 = imageStr.Split(chArray2);
                if (strArray2.Length <= 1)
                {
                    item.UpdateImage(ref item.image, ref this.dispPosition, this.fontSize, strArray2[0]);
                }
                else
                {
                    item.UpdateImage(ref item.image, ref this.dispPosition, this.fontSize, float.Parse(strArray2[1]), strArray2[0]);
                }
                item.UpdateImage(ref item.image, ref this.dispPosition, this.fontSize, imageStr);
            }
            else if (text[1] == '~')
            {
                item.image = this.FetchImageSprite();
                string s = text.Substring(2, text.Length - 3);
                item.UpdateLine(ref item.image, ref this.dispPosition, this.fontSize, int.Parse(s));
            }
            else
            {
                item.main = this.FetchMainLabel();
                item.UpdateLabel(ref item.main, ref this.dispPosition, this.fontSize, text);
            }
        }
        else
        {
            item.main = this.FetchMainLabel();
            item.UpdateLabel(ref item.main, ref this.dispPosition, this.fontSize, text);
        }
        if (this.messageOffset != null)
        {
            this.messageOffset.localPosition = new Vector3((this.startPosition.x - this.dispPosition.x) / 2f, 0f, 0f);
        }
        this.dispLabelList.Add(item);
    }

    public void ClearLabels()
    {
        this.Init();
        foreach (ScriptMessageLabel label in this.dispLabelList)
        {
            this.ReleaseLabel(label);
        }
        this.dispLabelList.Clear();
    }

    public void ClearText()
    {
        this.ClearLabels();
        this.dispPosition = this.startPosition;
        this.maxDispPosition.x = this.dispPosition.x;
        this.maxDispPosition.y = this.dispPosition.y - this.textOnlyLineHeight;
        if (this.messageOffset != null)
        {
            this.messageOffset.localPosition = Vector3.zero;
        }
        this.SetDefaultState();
    }

    public void DeleteLabels()
    {
        this.Init();
        this.ClearLabels();
        if (this.isRecycle)
        {
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
    }

    public void EffectScale(float s, float d)
    {
        Vector3 scale = new Vector3(s, s, 1f);
        TweenScale.Begin(base.gameObject, d, scale);
    }

    public void Fadeout(float d)
    {
        Transform transform = this.messageRoot.transform;
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            TweenAlpha.Begin(transform.GetChild(i).gameObject, d, 0f);
        }
    }

    protected UISprite FetchImageSprite()
    {
        UISprite component;
        if ((this.imageStock != null) && (this.imageStock.Count > 0))
        {
            component = this.imageStock.Pop();
        }
        else
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.imagePrefab);
            component = obj2.GetComponent<UISprite>();
            component.transform.parent = this.messageRoot;
            component.gameObject.layer = this.messageRoot.gameObject.layer;
            foreach (UIWidget widget in obj2.GetComponentsInChildren<UIWidget>())
            {
                widget.depth += this.depthOffset;
            }
        }
        component.transform.localPosition = Vector3.zero;
        component.transform.localScale = Vector3.one;
        return component;
    }

    private ScriptMessageLabel FetchLabel()
    {
        if ((this.labelStock != null) && (this.labelStock.Count > 0))
        {
            return this.labelStock.Pop();
        }
        return new ScriptMessageLabel();
    }

    protected UILabel FetchMainLabel()
    {
        UILabel component;
        if ((this.mainStock != null) && (this.mainStock.Count > 0))
        {
            component = this.mainStock.Pop();
        }
        else
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.mainPrefab);
            component = obj2.GetComponent<UILabel>();
            component.transform.parent = this.messageRoot;
            component.gameObject.layer = this.messageRoot.gameObject.layer;
            foreach (UIWidget widget in obj2.GetComponentsInChildren<UIWidget>())
            {
                widget.depth += this.depthOffset;
            }
        }
        component.transform.localPosition = Vector3.zero;
        component.transform.localScale = Vector3.one;
        return component;
    }

    protected UILabel FetchRubyLabel()
    {
        UILabel component;
        if ((this.rubyStock != null) && (this.rubyStock.Count > 0))
        {
            component = this.rubyStock.Pop();
        }
        else
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.rubyPrefab);
            component = obj2.GetComponent<UILabel>();
            component.transform.parent = this.messageRoot;
            component.gameObject.layer = this.messageRoot.gameObject.layer;
            foreach (UIWidget widget in obj2.GetComponentsInChildren<UIWidget>())
            {
                widget.depth += this.depthOffset;
            }
        }
        component.transform.localPosition = Vector3.zero;
        component.transform.localScale = Vector3.one;
        component.fontSize = this.rubyFontSize;
        return component;
    }

    public void FowardText(string text)
    {
        this.UpdateLabels(text, true);
    }

    public Vector2 GetPrintedSize()
    {
        Vector2 maxDispPosition = this.maxDispPosition;
        if (maxDispPosition.x < this.dispPosition.x)
        {
            maxDispPosition.x = this.dispPosition.x;
        }
        maxDispPosition -= this.startPosition;
        maxDispPosition.y = -maxDispPosition.y;
        return maxDispPosition;
    }

    public void Init()
    {
        if (!this.isInit)
        {
            this.isInit = true;
            if (this.isRecycle)
            {
                this.mainStock = new Stack<UILabel>();
                this.rubyStock = new Stack<UILabel>();
                this.imageStock = new Stack<UISprite>();
                this.labelStock = new Stack<ScriptMessageLabel>();
            }
            UILabel component = UnityEngine.Object.Instantiate<GameObject>(this.mainPrefab).GetComponent<UILabel>();
            UILabel label2 = UnityEngine.Object.Instantiate<GameObject>(this.rubyPrefab).GetComponent<UILabel>();
            if (this.defaultFontSize > 0)
            {
                this.fontSizes["-"] = this.defaultFontSize;
            }
            this.defaultFontSize = (int) (((float) this.fontSizes["-"]) * this.fontScale);
            component.fontSize = this.defaultFontSize;
            component.text = "■";
            this.defaultTextOnlyLineHeight = component.localSize.y;
            if (this.rubyFontSize > 0)
            {
                this.rubyFontSize = (int) (this.rubyFontSize * this.fontScale);
            }
            else
            {
                this.rubyFontSize = (int) (label2.fontSize * this.fontScale);
            }
            label2.fontSize = this.rubyFontSize;
            label2.text = "■";
            this.rubyLineHeight = label2.localSize.y;
            UnityEngine.Object.Destroy(component.gameObject);
            UnityEngine.Object.Destroy(label2.gameObject);
            this.startPosition = new Vector2(0f, -this.rubyLineHeight);
            if (this.messageOffset != null)
            {
                this.messageOffset.localPosition = Vector3.zero;
            }
            this.ClearText();
        }
    }

    public void Quit()
    {
        this.DeleteLabels();
    }

    private void ReleaseLabel(ScriptMessageLabel label)
    {
        if (label.main != null)
        {
            label.main.text = string.Empty;
            if (this.mainStock != null)
            {
                this.mainStock.Push(label.main);
            }
            else
            {
                UnityEngine.Object.Destroy(label.main.gameObject);
            }
            label.main = null;
        }
        if (label.ruby != null)
        {
            label.ruby.text = string.Empty;
            if (this.rubyStock != null)
            {
                this.rubyStock.Push(label.ruby);
            }
            else
            {
                UnityEngine.Object.Destroy(label.ruby.gameObject);
            }
            label.ruby = null;
        }
        if (label.image != null)
        {
            label.image.alpha = 0f;
            if (this.imageStock != null)
            {
                this.imageStock.Push(label.image);
            }
            else
            {
                UnityEngine.Object.Destroy(label.image.gameObject);
            }
            label.image = null;
        }
        label.Release();
        if (this.labelStock != null)
        {
            this.labelStock.Push(label);
        }
    }

    public void ReturnText()
    {
        if (this.maxDispPosition.x < this.dispPosition.x)
        {
            this.maxDispPosition.x = this.dispPosition.x;
        }
        this.dispPosition.x = this.startPosition.x;
        this.dispPosition.y -= this.textLineHeight + this.betweenLineHeight;
        this.fontSize = this.defaultFontSize;
        this.beforeTextOnlyLineHeight = this.textLineHeight;
        this.textOnlyLineHeight = this.defaultTextOnlyLineHeight;
        this.defaultColorTag = string.Empty;
        this.maxDispPosition.y = this.dispPosition.y - this.textOnlyLineHeight;
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
        this.beforeTextOnlyLineHeight = this.textLineHeight;
        this.defaultColorTag = string.Empty;
    }

    public void SetFontSize(string sizeName)
    {
        this.fontSize = (int) (((float) this.fontSizes[sizeName]) * this.fontScale);
        if (this.textOnlyLineHeight < this.fontSize)
        {
            this.textOnlyLineHeight = this.fontSize;
            this.maxDispPosition.y = this.dispPosition.y - this.textOnlyLineHeight;
        }
    }

    public void SetText(string text)
    {
        this.ClearText();
        this.UpdateLabels(text, false);
    }

    protected void UpdateLabels(string txt, bool isFoward)
    {
        <UpdateLabels>c__AnonStorey6D storeyd = new <UpdateLabels>c__AnonStorey6D {
            isFoward = isFoward,
            <>f__this = this,
            tmpTxt = string.Empty,
            tmpColorTag = this.defaultColorTag
        };
        int startIndex = 0;
        ProcAddLabel label = new ProcAddLabel(storeyd.<>m__79);
        ProcAddLabel2 label2 = new ProcAddLabel2(storeyd.<>m__7A);
        while (startIndex < txt.Length)
        {
            if (txt[startIndex] != '[')
            {
                goto Label_0291;
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
                this.defaultColorTag = storeyd.tmpColorTag;
                this.UpdateLabels(str2, storeyd.isFoward);
                storeyd.tmpColorTag = this.defaultColorTag;
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
                        goto Label_0288;
                    }
                    case "i":
                    {
                        label();
                        string str5 = txt.Substring(startIndex + 3, (braceIndex - startIndex) - 3);
                        label2("[^" + str5 + "]");
                        goto Label_0288;
                    }
                    case "r":
                        label();
                        this.ReturnText();
                        storeyd.tmpColorTag = this.defaultColorTag;
                        goto Label_0288;
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
                    storeyd.tmpColorTag = txt.Substring(startIndex, (braceIndex - startIndex) + 1);
                    storeyd.tmpTxt = storeyd.tmpTxt + storeyd.tmpColorTag;
                }
            }
        Label_0288:
            startIndex = braceIndex + 1;
            continue;
        Label_0291:
            storeyd.tmpTxt = storeyd.tmpTxt + txt[startIndex++];
        }
        label();
    }

    protected float textLineHeight =>
        (this.textOnlyLineHeight + this.rubyLineHeight);

    [CompilerGenerated]
    private sealed class <UpdateLabels>c__AnonStorey6D
    {
        internal ScriptLineMessage <>f__this;
        internal bool isFoward;
        internal string tmpColorTag;
        internal string tmpTxt;

        internal void <>m__79()
        {
            if (this.tmpTxt.Length > 0)
            {
                this.<>f__this.AddLabel(this.tmpTxt, this.<>f__this.defaultColorTag, this.isFoward);
                this.tmpTxt = string.Empty;
            }
            this.<>f__this.defaultColorTag = this.tmpColorTag;
        }

        internal void <>m__7A(string text)
        {
            if (text.Length > 0)
            {
                this.<>f__this.AddLabel(text, this.<>f__this.defaultColorTag, this.isFoward);
            }
            this.<>f__this.defaultColorTag = this.tmpColorTag;
        }
    }

    private delegate void ProcAddLabel();

    private delegate void ProcAddLabel2(string txt);
}

