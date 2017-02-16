using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class ScriptMessageLabel
{
    protected static string[] codeClassSplitStringList;
    public string colorTag = string.Empty;
    public int dispLength;
    public int fontSize;
    public UISprite image;
    public string imageText = string.Empty;
    public UILabel main;
    public Vector2 mainPosition;
    public string mainText = string.Empty;
    public UILabel ruby;
    public Vector2 rubyPosition;
    public int rubySize;
    public string rubyText = string.Empty;
    public float stepTime;
    public float widthSize;

    public void ClearLogDraw(GameObject mainObject, GameObject rubyObject, GameObject imageObject)
    {
        UILabel component = mainObject.GetComponent<UILabel>();
        UILabel label2 = rubyObject.GetComponent<UILabel>();
        UISprite sprite = imageObject.GetComponent<UISprite>();
        component.text = string.Empty;
        label2.text = string.Empty;
        sprite.alpha = 0f;
    }

    public static int ConvertCharaIndex(string s)
    {
        int num = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int num3 = s[i];
            if ((num3 >= 0x41) && (num3 <= 90))
            {
                num = (num << 8) | (num3 - 0x41);
            }
            else
            {
                return -1;
            }
        }
        return num;
    }

    public static int ConvertInteger(string s)
    {
        int num = 1;
        int num2 = 0;
        for (int i = 0; i < s.Length; i++)
        {
            char ch = s[i];
            if ((ch >= '0') && (ch <= '9'))
            {
                num2 += ((num2 * 10) + ch) - 0x30;
            }
            else if ((ch >= 0xff10) && (ch <= 0xff19))
            {
                num2 += ((num2 * 10) + ch) - 0xff10;
            }
            else
            {
                if ((ch != '-') && (ch != 0xff0d))
                {
                    return 0;
                }
                if (num != 1)
                {
                    return 0;
                }
                num = -1;
            }
        }
        return (num2 * num);
    }

    public void Destroy()
    {
        if (this.main != null)
        {
            UnityEngine.Object.Destroy(this.main.gameObject);
            this.main = null;
        }
        if (this.ruby != null)
        {
            UnityEngine.Object.Destroy(this.ruby.gameObject);
            this.ruby = null;
        }
        if (this.image != null)
        {
            UnityEngine.Object.Destroy(this.image.gameObject);
            this.image = null;
        }
    }

    public static int GetBraceIndex(string txt, int start)
    {
        int num = start;
        int num2 = 0;
        while (num < txt.Length)
        {
            if (txt[num] == '[')
            {
                num2++;
            }
            else if (txt[num] == ']')
            {
                if (num2 == 0)
                {
                    return num;
                }
                num2--;
            }
            num++;
        }
        return num;
    }

    public static string GetCommandName(string txt, int start)
    {
        int num = start;
        string str = string.Empty;
        while (num < txt.Length)
        {
            char ch = txt[num++];
            switch (ch)
            {
                case ' ':
                case '[':
                    return str;

                case ']':
                    return str;
            }
            str = str + ch;
        }
        return str;
    }

    public ScriptMessageLabel GetLogLabel() => 
        new ScriptMessageLabel { 
            mainText = this.mainText,
            rubyText = this.rubyText,
            imageText = this.imageText,
            fontSize = this.fontSize,
            rubySize = this.rubySize,
            colorTag = this.colorTag,
            widthSize = this.widthSize,
            mainPosition = this.mainPosition,
            rubyPosition = this.rubyPosition
        };

    public float GetLogRangeY() => 
        (this.mainPosition.y + this.fontSize);

    public static string GetTagSplitString(string txt, int start, int index)
    {
        int num = start;
        string str = string.Empty;
        string str2 = string.Empty;
        int num2 = 0;
        int num3 = 1;
        while (num < txt.Length)
        {
            char ch = txt[num++];
            if (ch == ']')
            {
                if (--num2 < 0)
                {
                    break;
                }
            }
            else if (ch == '[')
            {
                num2++;
            }
            else if ((ch == ':') && (num2 == 0))
            {
                if (num3 == index)
                {
                    str2 = str;
                }
                num3++;
                str = string.Empty;
                continue;
            }
            str = str + ch;
            if (ch == '"')
            {
                while (num < txt.Length)
                {
                    ch = txt[num++];
                    if (ch == '"')
                    {
                        break;
                    }
                    str = str + ch;
                }
                str = str + '"';
            }
        }
        if (num3 == index)
        {
            return str;
        }
        return str2;
    }

    public static void GetTalkName(out string imageName, out string className, out string charaName, out int charaIndex, string talkName)
    {
        if (codeClassSplitStringList == null)
        {
            codeClassSplitStringList = new string[] { LocalizationManager.Get("SCRIPT_ACTION_CODE_CLASS_SPLIT") };
        }
        string[] strArray = talkName.Split(codeClassSplitStringList, StringSplitOptions.RemoveEmptyEntries);
        imageName = null;
        className = string.Empty;
        charaIndex = -1;
        for (int i = 0; i < (strArray.Length - 1); i++)
        {
            int num2 = strArray[i][0];
            if ((num2 >= 0x41) && (num2 <= 90))
            {
                charaIndex = ConvertCharaIndex(strArray[i]);
            }
            else
            {
                int id = ConvertInteger(strArray[i]);
                ServantClassEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_CLASS).getEntityFromId<ServantClassEntity>(id);
                if (entity != null)
                {
                    imageName = $"icon_class_{id:D3}";
                    className = entity.name + "：";
                }
            }
        }
        charaName = (strArray.Length <= 0) ? string.Empty : strArray[strArray.Length - 1];
    }

    public static void GetTextByLen(string text, int len, out string dispText, out int length)
    {
        int num = 0;
        int num2 = 0;
        if (text == null)
        {
            dispText = string.Empty;
            length = 0;
        }
        else
        {
            while (num < text.Length)
            {
                if (num2 == len)
                {
                    break;
                }
                if (text[num] == '[')
                {
                    if (text[num + 1] == '#')
                    {
                        char[] separator = new char[] { ':' };
                        int num3 = StrlenByDisp(text.Substring(num + 2, text.Length - 2).Split(separator)[0]);
                        num2 += num3;
                    }
                    else
                    {
                        int num4 = num + 2;
                        int num5 = 0;
                        while (num4 < text.Length)
                        {
                            if (text[num4] == '[')
                            {
                                num5++;
                            }
                            else if (text[num4] == ']')
                            {
                                if (num5 == 0)
                                {
                                    break;
                                }
                                num5--;
                            }
                            num4++;
                        }
                        num = num4;
                    }
                }
                else
                {
                    num2++;
                }
                num++;
            }
            dispText = text.Substring(0, num);
            length = num2;
        }
    }

    public void Release()
    {
        this.mainText = string.Empty;
        this.rubyText = string.Empty;
        this.imageText = string.Empty;
        if (this.main != null)
        {
            this.main.text = string.Empty;
        }
        if (this.ruby != null)
        {
            this.ruby.text = string.Empty;
        }
        if (this.image != null)
        {
            this.image.alpha = 0f;
        }
    }

    public void SetLogDraw(GameObject mainObject, GameObject rubyObject, GameObject imageObject)
    {
        UILabel component = mainObject.GetComponent<UILabel>();
        UILabel label2 = rubyObject.GetComponent<UILabel>();
        UISprite sprite = imageObject.GetComponent<UISprite>();
        if (this.imageText != string.Empty)
        {
            sprite.alpha = 1f;
            if (this.imageText == "line")
            {
                AtlasManager.SetMark(sprite, this.imageText);
                sprite.MakePixelPerfect();
                sprite.SetDimensions((int) this.widthSize, this.fontSize);
                if ((this.colorTag == string.Empty) || (this.colorTag == "[-]"))
                {
                    sprite.color = Color.white;
                }
                else
                {
                    int num = Convert.ToInt32(this.colorTag.Substring(1, 2), 0x10);
                    int num2 = Convert.ToInt32(this.colorTag.Substring(3, 2), 0x10);
                    int num3 = Convert.ToInt32(this.colorTag.Substring(5, 2), 0x10);
                    sprite.color = new Color(((float) num) / 256f, ((float) num2) / 256f, ((float) num3) / 256f);
                }
            }
            else
            {
                AtlasManager.SetMark(sprite, this.imageText);
                sprite.MakePixelPerfect();
                sprite.SetDimensions((int) this.widthSize, this.fontSize);
                sprite.color = Color.white;
            }
            component.text = string.Empty;
            label2.text = string.Empty;
        }
        else
        {
            component.fontSize = this.fontSize;
            component.text = this.mainText;
            if (this.rubyText != string.Empty)
            {
                label2.fontSize = this.rubySize;
                label2.text = this.rubyText;
                label2.transform.localPosition = new Vector3(this.rubyPosition.x - this.mainPosition.x, this.rubyPosition.y - this.mainPosition.y, 0f);
            }
            else
            {
                label2.text = string.Empty;
            }
            sprite.alpha = 0f;
        }
    }

    public static int StrlenByDisp(string text)
    {
        string str;
        int num;
        GetTextByLen(text, -1, out str, out num);
        return num;
    }

    public static string SubstrByDisp(string text, int len)
    {
        string str;
        int num;
        GetTextByLen(text, len, out str, out num);
        return str;
    }

    public void UpdateBouten(ref UILabel main, ref UILabel ruby, ref Vector2 pos, int fontSize, string mainStr)
    {
        this.mainText = this.colorTag + mainStr;
        this.rubyText = this.colorTag;
        this.imageText = string.Empty;
        this.fontSize = fontSize;
        this.rubySize = fontSize;
        for (int i = 0; i < this.mainText.Length; i++)
        {
            this.rubyText = this.rubyText + "・";
        }
        main.fontSize = fontSize;
        main.text = this.mainText;
        main.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
        this.mainPosition = pos;
        ruby.fontSize = fontSize;
        ruby.text = this.rubyText;
        float x = ruby.localSize.x;
        float num3 = main.localSize.x;
        this.rubyPosition.x = (pos.x + (num3 / 2f)) - (x / 2f);
        this.rubyPosition.y = pos.y + ((ruby.localSize.y * 2f) / 3f);
        ruby.transform.localPosition = new Vector3(this.rubyPosition.x, this.rubyPosition.y, 0f);
        this.widthSize = main.localSize.x;
        pos.x += this.widthSize;
        this.dispLength = StrlenByDisp(this.mainText);
    }

    public void UpdateImage(ref UISprite image, ref Vector2 pos, int fontSize, string imageStr)
    {
        this.mainText = string.Empty;
        this.rubyText = string.Empty;
        this.imageText = imageStr;
        this.fontSize = fontSize;
        AtlasManager.SetMark(image, imageStr);
        image.MakePixelPerfect();
        int w = (int) ((this.fontSize * image.localSize.x) / image.localSize.y);
        image.SetDimensions(w, this.fontSize);
        image.color = Color.white;
        image.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
        this.mainPosition = pos;
        this.widthSize = w;
        pos.x += this.widthSize;
        this.dispLength = 1;
    }

    public void UpdateImage(ref UISprite image, ref Vector2 pos, int fontSize, float scale, string imageStr)
    {
        this.mainText = string.Empty;
        this.rubyText = string.Empty;
        this.imageText = imageStr;
        this.fontSize = (int) (fontSize * scale);
        AtlasManager.SetMark(image, imageStr);
        image.MakePixelPerfect();
        int w = (int) ((this.fontSize * image.localSize.x) / image.localSize.y);
        image.SetDimensions(w, this.fontSize);
        image.color = Color.white;
        this.mainPosition.x = pos.x;
        this.mainPosition.y = pos.y + (this.fontSize - fontSize);
        image.transform.localPosition = new Vector3(this.mainPosition.x, this.mainPosition.y, 0f);
        this.widthSize = w;
        pos.x += this.widthSize;
        this.dispLength = 1;
    }

    public void UpdateLabel(ref UILabel main, ref Vector2 pos, int fontSize, string mainStr)
    {
        this.mainText = this.colorTag + mainStr;
        this.rubyText = string.Empty;
        this.imageText = string.Empty;
        this.fontSize = fontSize;
        main.fontSize = fontSize;
        main.text = this.mainText;
        main.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
        this.mainPosition = pos;
        this.widthSize = main.localSize.x;
        pos.x += this.widthSize;
        this.dispLength = StrlenByDisp(this.mainText);
    }

    public void UpdateLine(ref UISprite image, ref Vector2 pos, int fontSize, int length)
    {
        this.mainText = string.Empty;
        this.rubyText = string.Empty;
        this.imageText = "line";
        this.fontSize = fontSize;
        AtlasManager.SetMark(image, this.imageText);
        image.MakePixelPerfect();
        int w = (int) ((((fontSize * image.localSize.x) / image.localSize.y) * length) / 10f);
        image.SetDimensions(w, fontSize);
        if ((this.colorTag == string.Empty) || (this.colorTag == "[-]"))
        {
            image.color = Color.white;
        }
        else
        {
            int num2 = Convert.ToInt32(this.colorTag.Substring(1, 2), 0x10);
            int num3 = Convert.ToInt32(this.colorTag.Substring(3, 2), 0x10);
            int num4 = Convert.ToInt32(this.colorTag.Substring(5, 2), 0x10);
            image.color = new Color(((float) num2) / 256f, ((float) num3) / 256f, ((float) num4) / 256f);
        }
        image.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
        this.mainPosition = pos;
        this.widthSize = w;
        pos.x += w;
        this.dispLength = 1;
    }

    public void UpdateRuby(ref UILabel main, ref UILabel ruby, ref Vector2 pos, int fontSize, string mainStr, string rubyStr)
    {
        this.mainText = this.colorTag + mainStr;
        this.rubyText = this.colorTag + rubyStr;
        this.imageText = string.Empty;
        this.fontSize = fontSize;
        this.rubySize = ruby.fontSize;
        main.fontSize = fontSize;
        main.text = this.mainText;
        main.transform.localPosition = new Vector3(pos.x, pos.y, 0f);
        this.mainPosition = pos;
        ruby.text = this.rubyText;
        float x = ruby.localSize.x;
        float num2 = main.localSize.x;
        this.rubyPosition.x = (pos.x + (num2 / 2f)) - (x / 2f);
        this.rubyPosition.y = pos.y + ruby.localSize.y;
        ruby.transform.localPosition = new Vector3(this.rubyPosition.x, this.rubyPosition.y, 0f);
        this.widthSize = main.localSize.x;
        pos.x += this.widthSize;
        this.dispLength = StrlenByDisp(this.mainText);
    }
}

