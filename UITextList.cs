using System;
using System.Text;
using UnityEngine;

[AddComponentMenu("NGUI/UI/Text List")]
public class UITextList : MonoBehaviour
{
    protected int mLastHeight;
    protected int mLastWidth;
    protected BetterList<Paragraph> mParagraphs = new BetterList<Paragraph>();
    protected float mScroll;
    protected char[] mSeparator = new char[] { '\n' };
    protected int mTotalLines;
    public int paragraphHistory = 50;
    public UIProgressBar scrollBar;
    public Style style;
    public UILabel textLabel;

    public void Add(string text)
    {
        this.Add(text, true);
    }

    protected void Add(string text, bool updateVisible)
    {
        Paragraph item = null;
        if (this.mParagraphs.size < this.paragraphHistory)
        {
            item = new Paragraph();
        }
        else
        {
            item = this.mParagraphs[0];
            this.mParagraphs.RemoveAt(0);
        }
        item.text = text;
        this.mParagraphs.Add(item);
        this.Rebuild();
    }

    public void Clear()
    {
        this.mParagraphs.Clear();
        this.UpdateVisibleText();
    }

    public void OnDrag(Vector2 delta)
    {
        int scrollHeight = this.scrollHeight;
        if (scrollHeight != 0)
        {
            float num2 = delta.y / this.lineHeight;
            this.scrollValue = this.mScroll + (num2 / ((float) scrollHeight));
        }
    }

    public void OnScroll(float val)
    {
        int scrollHeight = this.scrollHeight;
        if (scrollHeight != 0)
        {
            val *= this.lineHeight;
            this.scrollValue = this.mScroll - (val / ((float) scrollHeight));
        }
    }

    private void OnScrollBar()
    {
        this.mScroll = UIProgressBar.current.value;
        this.UpdateVisibleText();
    }

    protected void Rebuild()
    {
        if (this.isValid)
        {
            this.textLabel.UpdateNGUIText();
            NGUIText.rectHeight = 0xf4240;
            this.mTotalLines = 0;
            for (int i = 0; i < this.mParagraphs.size; i++)
            {
                string str;
                Paragraph paragraph = this.mParagraphs.buffer[i];
                NGUIText.WrapText(paragraph.text, out str);
                char[] separator = new char[] { '\n' };
                paragraph.lines = str.Split(separator);
                this.mTotalLines += paragraph.lines.Length;
            }
            this.mTotalLines = 0;
            int index = 0;
            int size = this.mParagraphs.size;
            while (index < size)
            {
                this.mTotalLines += this.mParagraphs.buffer[index].lines.Length;
                index++;
            }
            if (this.scrollBar != null)
            {
                UIScrollBar scrollBar = this.scrollBar as UIScrollBar;
                if (scrollBar != null)
                {
                    scrollBar.barSize = (this.mTotalLines != 0) ? (1f - (((float) this.scrollHeight) / ((float) this.mTotalLines))) : 1f;
                }
            }
            this.UpdateVisibleText();
        }
    }

    private void Start()
    {
        if (this.textLabel == null)
        {
            this.textLabel = base.GetComponentInChildren<UILabel>();
        }
        if (this.scrollBar != null)
        {
            EventDelegate.Add(this.scrollBar.onChange, new EventDelegate.Callback(this.OnScrollBar));
        }
        this.textLabel.overflowMethod = UILabel.Overflow.ClampContent;
        if (this.style == Style.Chat)
        {
            this.textLabel.pivot = UIWidget.Pivot.BottomLeft;
            this.scrollValue = 1f;
        }
        else
        {
            this.textLabel.pivot = UIWidget.Pivot.TopLeft;
            this.scrollValue = 0f;
        }
    }

    private void Update()
    {
        if (this.isValid && ((this.textLabel.width != this.mLastWidth) || (this.textLabel.height != this.mLastHeight)))
        {
            this.mLastWidth = this.textLabel.width;
            this.mLastHeight = this.textLabel.height;
            this.Rebuild();
        }
    }

    protected void UpdateVisibleText()
    {
        if (this.isValid)
        {
            if (this.mTotalLines == 0)
            {
                this.textLabel.text = string.Empty;
            }
            else
            {
                int num = Mathf.FloorToInt(((float) this.textLabel.height) / this.lineHeight);
                int num2 = Mathf.Max(0, this.mTotalLines - num);
                int num3 = Mathf.RoundToInt(this.mScroll * num2);
                if (num3 < 0)
                {
                    num3 = 0;
                }
                StringBuilder builder = new StringBuilder();
                int index = 0;
                int size = this.mParagraphs.size;
                while ((num > 0) && (index < size))
                {
                    Paragraph paragraph = this.mParagraphs.buffer[index];
                    int num6 = 0;
                    int length = paragraph.lines.Length;
                    while ((num > 0) && (num6 < length))
                    {
                        string str = paragraph.lines[num6];
                        if (num3 > 0)
                        {
                            num3--;
                        }
                        else
                        {
                            if (builder.Length > 0)
                            {
                                builder.Append("\n");
                            }
                            builder.Append(str);
                            num--;
                        }
                        num6++;
                    }
                    index++;
                }
                this.textLabel.text = builder.ToString();
            }
        }
    }

    public bool isValid =>
        ((this.textLabel != null) && (this.textLabel.ambigiousFont != null));

    protected float lineHeight =>
        ((this.textLabel == null) ? 20f : (this.textLabel.fontSize + this.textLabel.effectiveSpacingY));

    protected int scrollHeight
    {
        get
        {
            if (!this.isValid)
            {
                return 0;
            }
            int num = Mathf.FloorToInt(((float) this.textLabel.height) / this.lineHeight);
            return Mathf.Max(0, this.mTotalLines - num);
        }
    }

    public float scrollValue
    {
        get => 
            this.mScroll;
        set
        {
            value = Mathf.Clamp01(value);
            if (this.isValid && (this.mScroll != value))
            {
                if (this.scrollBar != null)
                {
                    this.scrollBar.value = value;
                }
                else
                {
                    this.mScroll = value;
                    this.UpdateVisibleText();
                }
            }
        }
    }

    protected class Paragraph
    {
        public string[] lines;
        public string text;
    }

    public enum Style
    {
        Text,
        Chat
    }
}

