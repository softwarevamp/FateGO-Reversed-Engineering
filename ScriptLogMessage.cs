using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScriptLogMessage : MonoBehaviour
{
    protected ScriptMessageLabel analyzeLabel;
    [SerializeField]
    protected ScriptBackLog backLogDialog;
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
    private Vector2 dispPosition;
    [SerializeField]
    protected float fontScale = 1f;
    protected int fontSize;
    protected Dictionary<string, int> fontSizes;
    [SerializeField]
    protected GameObject imagePrefab;
    protected bool isInit;
    [SerializeField]
    protected bool isRecycle;
    [SerializeField]
    protected GameObject mainPrefab;
    [SerializeField]
    protected int rubyFontSize;
    protected float rubyLineHeight;
    [SerializeField]
    protected GameObject rubyPrefab;
    private Vector2 startPosition;
    protected string talkName;
    protected float textOnlyLineHeight;
    [SerializeField]
    protected Transform workLabelRoot;

    public ScriptLogMessage()
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
    }

    private void AddLabel(string text, string colorTag, bool isFoward)
    {
        ScriptMessageLabel analyzeLabel = this.analyzeLabel;
        analyzeLabel.stepTime = 0f;
        analyzeLabel.colorTag = colorTag;
        Vector2 dispPosition = this.dispPosition;
        if (text[0] == '[')
        {
            if (text[1] == '#')
            {
                this.FetchMainLabel();
                this.FetchRubyLabel();
                char[] separator = new char[] { ':' };
                string[] strArray = text.Substring(2, text.Length - 3).Split(separator);
                if (strArray.Length < 2)
                {
                    analyzeLabel.UpdateBouten(ref analyzeLabel.main, ref analyzeLabel.ruby, ref this.dispPosition, this.fontSize, strArray[0]);
                }
                else
                {
                    analyzeLabel.UpdateRuby(ref analyzeLabel.main, ref analyzeLabel.ruby, ref this.dispPosition, this.fontSize, strArray[0], strArray[1]);
                }
            }
            else if (text[1] == '^')
            {
                this.FetchImageSprite();
                char[] chArray2 = new char[] { ' ' };
                string[] strArray2 = text.Substring(2, text.Length - 3).Split(chArray2);
                if (strArray2.Length <= 1)
                {
                    analyzeLabel.UpdateImage(ref analyzeLabel.image, ref this.dispPosition, this.fontSize, strArray2[0]);
                }
                else
                {
                    analyzeLabel.UpdateImage(ref analyzeLabel.image, ref this.dispPosition, this.fontSize, float.Parse(strArray2[1]), strArray2[0]);
                }
            }
            else if (text[1] == '~')
            {
                this.FetchImageSprite();
                string s = text.Substring(2, text.Length - 3);
                analyzeLabel.UpdateLine(ref analyzeLabel.image, ref this.dispPosition, this.fontSize, int.Parse(s));
            }
            else
            {
                analyzeLabel.main = this.FetchMainLabel();
                analyzeLabel.UpdateLabel(ref analyzeLabel.main, ref this.dispPosition, this.fontSize, text);
            }
        }
        else
        {
            this.FetchMainLabel();
            analyzeLabel.UpdateLabel(ref analyzeLabel.main, ref this.dispPosition, this.fontSize, text);
        }
        if (isFoward)
        {
            analyzeLabel.mainPosition.x -= analyzeLabel.widthSize;
            if (analyzeLabel.rubyText != string.Empty)
            {
                analyzeLabel.rubyPosition.x -= analyzeLabel.widthSize;
            }
            this.dispPosition = dispPosition;
        }
        this.backLogDialog.AddLog(analyzeLabel.GetLogLabel());
    }

    public void AddText(string text)
    {
        this.UpdateLabels(text, false);
    }

    public void ClearLabels()
    {
        this.Init();
    }

    public void ClearTalkName()
    {
        this.talkName = string.Empty;
    }

    public void ClearText()
    {
        this.ClearLabels();
        this.dispPosition = this.startPosition;
        this.SetDefaultState();
    }

    public void DeleteLabels()
    {
        this.Init();
    }

    protected UISprite FetchImageSprite()
    {
        UISprite image = this.analyzeLabel.image;
        image.transform.localPosition = Vector3.zero;
        image.transform.localScale = Vector3.one;
        return image;
    }

    protected UILabel FetchMainLabel()
    {
        UILabel main = this.analyzeLabel.main;
        main.transform.localPosition = Vector3.zero;
        main.transform.localScale = Vector3.one;
        return main;
    }

    protected UILabel FetchRubyLabel()
    {
        UILabel ruby = this.analyzeLabel.ruby;
        ruby.transform.localPosition = Vector3.zero;
        ruby.transform.localScale = Vector3.one;
        ruby.fontSize = this.rubyFontSize;
        return ruby;
    }

    public void FowardText(string text)
    {
        this.UpdateLabels(text, true);
    }

    public void Init()
    {
        if (!this.isInit)
        {
            this.isInit = true;
            this.analyzeLabel = new ScriptMessageLabel();
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.mainPrefab);
            obj2.transform.parent = this.workLabelRoot;
            obj2.layer = this.workLabelRoot.gameObject.layer;
            this.analyzeLabel.main = obj2.GetComponent<UILabel>();
            GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.rubyPrefab);
            obj3.transform.parent = this.workLabelRoot;
            obj3.layer = this.workLabelRoot.gameObject.layer;
            this.analyzeLabel.ruby = obj3.GetComponent<UILabel>();
            GameObject obj4 = UnityEngine.Object.Instantiate<GameObject>(this.imagePrefab);
            obj4.transform.parent = this.workLabelRoot;
            obj4.layer = this.workLabelRoot.gameObject.layer;
            this.analyzeLabel.image = obj4.GetComponent<UISprite>();
            UILabel main = this.analyzeLabel.main;
            UILabel ruby = this.analyzeLabel.ruby;
            if (this.defaultFontSize > 0)
            {
                this.fontSizes["-"] = this.defaultFontSize;
            }
            this.defaultFontSize = (int) (((float) this.fontSizes["-"]) * this.fontScale);
            main.fontSize = this.defaultFontSize;
            main.text = "■";
            this.defaultTextOnlyLineHeight = main.localSize.y;
            if (this.rubyFontSize > 0)
            {
                this.rubyFontSize = (int) (this.rubyFontSize * this.fontScale);
            }
            else
            {
                this.rubyFontSize = (int) (ruby.fontSize * this.fontScale);
            }
            ruby.fontSize = this.rubyFontSize;
            ruby.text = "■";
            this.rubyLineHeight = ruby.localSize.y;
            this.startPosition = new Vector2(0f, -this.rubyLineHeight);
            this.ClearText();
        }
    }

    public bool IsChangeTalkName(string text) => 
        (this.talkName != text);

    public void Quit()
    {
        if (this.isInit)
        {
            this.isInit = false;
            this.analyzeLabel.Destroy();
            this.analyzeLabel = null;
        }
    }

    public void ReturnText()
    {
        this.dispPosition.x = this.startPosition.x;
        this.dispPosition.y -= this.textLineHeight + this.betweenLineHeight;
        this.fontSize = this.defaultFontSize;
        this.beforeTextOnlyLineHeight = this.textLineHeight;
        this.textOnlyLineHeight = this.defaultTextOnlyLineHeight;
        this.defaultColorTag = string.Empty;
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
        }
    }

    public void SetHomePosition(int x)
    {
        if (this.dispPosition.x > this.startPosition.x)
        {
            this.ReturnText();
        }
        this.startPosition.x = x;
        this.dispPosition.x = x;
    }

    public bool SetTalkName(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            this.ClearTalkName();
        }
        else
        {
            this.talkName = text;
            this.fontSize = (int) (this.defaultFontSize * 1f);
            this.dispPosition.x = this.startPosition.x - 50f;
            this.UpdateLabels(text, false);
            this.ReturnText();
        }
        return true;
    }

    public void SetText(string text)
    {
        this.ClearText();
        this.UpdateLabels(text, false);
    }

    protected void UpdateLabels(string txt, bool isFoward)
    {
        <UpdateLabels>c__AnonStorey6E storeye = new <UpdateLabels>c__AnonStorey6E {
            isFoward = isFoward,
            <>f__this = this,
            tmpTxt = string.Empty,
            tmpColorTag = this.defaultColorTag
        };
        int startIndex = 0;
        ProcAddLabel label = new ProcAddLabel(storeye.<>m__7B);
        ProcAddLabel2 label2 = new ProcAddLabel2(storeye.<>m__7C);
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
                this.defaultColorTag = storeye.tmpColorTag;
                this.UpdateLabels(str2, storeye.isFoward);
                storeye.tmpColorTag = this.defaultColorTag;
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
                        storeye.tmpColorTag = this.defaultColorTag;
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
                    storeye.tmpColorTag = txt.Substring(startIndex, (braceIndex - startIndex) + 1);
                    storeye.tmpTxt = storeye.tmpTxt + storeye.tmpColorTag;
                }
            }
        Label_0288:
            startIndex = braceIndex + 1;
            continue;
        Label_0291:
            storeye.tmpTxt = storeye.tmpTxt + txt[startIndex++];
        }
        label();
    }

    protected float textLineHeight =>
        (this.textOnlyLineHeight + this.rubyLineHeight);

    [CompilerGenerated]
    private sealed class <UpdateLabels>c__AnonStorey6E
    {
        internal ScriptLogMessage <>f__this;
        internal bool isFoward;
        internal string tmpColorTag;
        internal string tmpTxt;

        internal void <>m__7B()
        {
            if (this.tmpTxt.Length > 0)
            {
                this.<>f__this.AddLabel(this.tmpTxt, this.<>f__this.defaultColorTag, this.isFoward);
                this.tmpTxt = string.Empty;
            }
            this.<>f__this.defaultColorTag = this.tmpColorTag;
        }

        internal void <>m__7C(string text)
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

