using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class WrapControlText
{
    private const int fontSizeStep = 1;
    private const int maxFontSize = 0x16;
    private const float sizeAlphabetBig = 0.9f;
    private const float sizeAlphabetSmall = 0.6f;
    private const float sizeHankaku = 0.5f;
    private const float sizeKigou = 1f;
    private const float sizeNumber = 0.9f;
    private const float sizeSmall = 0.5f;
    private const float sizeZenkaku = 1f;
    private const int wordAdjust = 4;

    private static string mojiCheck(string org, out float len, out string remain)
    {
        string str = string.Empty;
        string text = org;
        bool flag = false;
        bool flag2 = false;
        bool flag3 = true;
        bool isLast = false;
        len = 0f;
        remain = string.Empty;
        string s = text.Substring(0, 1);
        byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(s);
        if (text.Length == 1)
        {
            isLast = true;
        }
        if (bytes.Length > 1)
        {
            len = 1f;
            remain = remainString(isLast, text);
            return s;
        }
    Label_0067:
        if (text.Length == 1)
        {
            isLast = true;
        }
        if (flag3)
        {
            flag3 = false;
        }
        else
        {
            s = text.Substring(0, 1);
            bytes = Encoding.GetEncoding("UTF-8").GetBytes(s);
            if (bytes.Length > 1)
            {
                remain = text;
                return str;
            }
        }
        int num = bytes[0];
        if (num >= 0x30)
        {
            if ((num < 0x30) || (num > 0x39))
            {
                if ((num < 0x41) || (num > 90))
                {
                    if ((num < 0x61) || (num > 0x7a))
                    {
                        if (flag2 || flag)
                        {
                            remain = text;
                            return str;
                        }
                        len = 0.5f;
                        remain = remainString(isLast, text);
                        return s;
                    }
                    if (!flag2 && flag)
                    {
                        remain = text;
                        return str;
                    }
                    flag2 = true;
                    str = str + s;
                    len += 0.6f;
                    text = remainString(isLast, text);
                    if (isLast)
                    {
                        remain = string.Empty;
                        return str;
                    }
                }
                else
                {
                    if (!flag2 && flag)
                    {
                        remain = text;
                        return str;
                    }
                    flag2 = true;
                    str = str + s;
                    len += 0.9f;
                    text = remainString(isLast, text);
                    if (isLast)
                    {
                        remain = string.Empty;
                        return str;
                    }
                }
            }
            else
            {
                if (!flag && flag2)
                {
                    remain = text;
                    return str;
                }
                flag = true;
                str = str + s;
                len += 0.9f;
                text = remainString(isLast, text);
                if (isLast)
                {
                    remain = string.Empty;
                    return str;
                }
            }
            goto Label_0067;
        }
        if (flag2 || flag)
        {
            remain = text;
            return str;
        }
        switch (num)
        {
            case 0x20:
            case 40:
            case 0x29:
                len = 0.5f;
                break;

            default:
                len = 1f;
                break;
        }
        remain = remainString(isLast, text);
        return s;
    }

    private static string remainString(bool isLast, string text)
    {
        if (isLast)
        {
            return string.Empty;
        }
        return text.Substring(1);
    }

    public static void textAdjust(UILabel label, string text)
    {
        int num2;
        string str3;
        List<List<WordBuf>> list = new List<List<WordBuf>>();
        int num3 = 0x16;
        string str = LocalizationManager.Get("KINSOKU_TOP_STR");
        string str2 = LocalizationManager.Get("KINSOKU_LAST_STR");
        char[] separator = new char[] { '\n' };
        string[] strArray = text.Split(separator);
        int length = strArray.Length;
        string remain = string.Empty;
        float num5 = 0f;
        for (int i = 0; i < strArray.Length; i++)
        {
            WordBuf buf;
            List<WordBuf> item = new List<WordBuf>();
            str3 = strArray[i];
            if (str3.Length > 0)
            {
                while (true)
                {
                    buf = new WordBuf();
                    buf.word = mojiCheck(str3, out buf.len, out remain);
                    item.Add(buf);
                    if (remain.Equals(string.Empty))
                    {
                        goto Label_00D4;
                    }
                    str3 = remain;
                    num5 += buf.len;
                }
            }
            buf = new WordBuf {
                word = string.Empty
            };
            item.Add(buf);
        Label_00D4:
            list.Add(item);
        }
        while (true)
        {
            int num = (int) (((float) label.width) / ((float) num3));
            num2 = (int) (((float) label.height) / (num3 * 1.1f));
            if (num2 < length)
            {
                num2 = length;
            }
            if (((num * num2) - 4) > num5)
            {
                break;
            }
            num3--;
        }
        label.fontSize = num3;
        List<string> list3 = new List<string>();
        int num7 = 0;
        bool flag = false;
        foreach (List<WordBuf> list4 in list)
        {
            string str5;
            str3 = string.Empty;
            float num9 = 0f;
            int num10 = 0;
            int num11 = 0;
        Label_017A:
            if ((num9 + (list4[num11].len * num3)) < label.width)
            {
                num9 += list4[num11].len * num3;
                num11++;
                if (num11 != list4.Count)
                {
                    goto Label_017A;
                }
                for (int m = 0; m < (num11 - num10); m++)
                {
                    str3 = str3 + list4[m + num10].word;
                }
                list3.Add(str3);
                num7++;
                if (num7 > num2)
                {
                    flag = true;
                }
                num10 = 0;
                num9 = 0f;
                str3 = string.Empty;
                remain = string.Empty;
                continue;
            }
        Label_0237:
            str5 = list4[num11].word.Substring(0, 1);
            if (str.IndexOf(str5) >= 0)
            {
                num11--;
                goto Label_0237;
            }
        Label_0276:
            str5 = list4[num11 - 1].word.Substring(0, 1);
            if (str2.IndexOf(str5) >= 0)
            {
                num11--;
                goto Label_0276;
            }
            for (int k = 0; k < (num11 - num10); k++)
            {
                str3 = str3 + list4[k + num10].word;
            }
            list3.Add(str3);
            num7++;
            if (num7 > num2)
            {
                flag = true;
            }
            num10 = num11;
            num9 = 0f;
            str3 = string.Empty;
            goto Label_017A;
        }
        str3 = string.Empty;
        for (int j = 0; j < num7; j++)
        {
            str3 = str3 + list3[j];
            if (j < (num7 - 1))
            {
                str3 = str3 + '\n';
            }
        }
        if (flag)
        {
            num3 = (int) (((float) label.height) / ((float) num7));
            label.fontSize = num3;
        }
        label.text = str3;
    }

    private class WordBuf
    {
        public float len = 0f;
        public string word = string.Empty;
    }
}

