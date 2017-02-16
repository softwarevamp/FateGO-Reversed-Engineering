using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class NGUIText
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map36;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map37;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map38;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map39;
    public static Alignment alignment = Alignment.Left;
    public static float baseline = 0f;
    public static UIFont bitmapFont;
    public static Font dynamicFont;
    public static bool encoding = false;
    public static float finalLineHeight = 0f;
    public static int finalSize = 0;
    public static float finalSpacingX = 0f;
    public static float fontScale = 1f;
    public static int fontSize = 0x10;
    public static FontStyle fontStyle = FontStyle.Normal;
    public static GlyphInfo glyph = new GlyphInfo();
    public static bool gradient = false;
    public static Color gradientBottom = Color.white;
    public static Color gradientTop = Color.white;
    private static float mAlpha = 1f;
    public static int maxLines = 0;
    private static float[] mBoldOffset = new float[] { -0.25f, 0f, 0.25f, 0f, 0f, -0.25f, 0f, 0.25f };
    private static BetterList<Color> mColors = new BetterList<Color>();
    private static Color mInvisible = new Color(0f, 0f, 0f, 0f);
    private static BetterList<float> mSizes = new BetterList<float>();
    private static CharacterInfo mTempChar;
    public static float pixelDensity = 1f;
    public static bool premultiply = false;
    public static int rectHeight = 0xf4240;
    public static int rectWidth = 0xf4240;
    public static int regionHeight = 0xf4240;
    public static int regionWidth = 0xf4240;
    private static Color32 s_c0;
    private static Color32 s_c1;
    public static float spacingX = 0f;
    public static float spacingY = 0f;
    public static SymbolStyle symbolStyle;
    public static Color tint = Color.white;
    public static bool useSymbols = false;

    public static void Align(BetterList<Vector3> verts, int indexOffset, float printedWidth)
    {
        switch (alignment)
        {
            case Alignment.Center:
            {
                float num3 = (rectWidth - printedWidth) * 0.5f;
                if (num3 >= 0f)
                {
                    int num4 = Mathf.RoundToInt(rectWidth - printedWidth);
                    int num5 = Mathf.RoundToInt((float) rectWidth);
                    bool flag = (num4 & 1) == 1;
                    bool flag2 = (num5 & 1) == 1;
                    if ((flag && !flag2) || (!flag && flag2))
                    {
                        num3 += 0.5f * fontScale;
                    }
                    for (int i = indexOffset; i < verts.size; i++)
                    {
                        verts.buffer[i].x += num3;
                    }
                    break;
                }
                return;
            }
            case Alignment.Right:
            {
                float num = rectWidth - printedWidth;
                if (num >= 0f)
                {
                    for (int j = indexOffset; j < verts.size; j++)
                    {
                        verts.buffer[j].x += num;
                    }
                    break;
                }
                return;
            }
            case Alignment.Justified:
                if (printedWidth >= (rectWidth * 0.65f))
                {
                    float num7 = (rectWidth - printedWidth) * 0.5f;
                    if (num7 < 1f)
                    {
                        return;
                    }
                    int num8 = (verts.size - indexOffset) / 4;
                    if (num8 < 1)
                    {
                        return;
                    }
                    float num9 = 1f / ((float) (num8 - 1));
                    float num10 = ((float) rectWidth) / printedWidth;
                    int index = indexOffset + 4;
                    for (int k = 1; index < verts.size; k++)
                    {
                        float x = verts.buffer[index].x;
                        float f = verts.buffer[index + 2].x;
                        float num15 = f - x;
                        float a = x * num10;
                        float num17 = a + num15;
                        float b = f * num10;
                        float num19 = b - num15;
                        float t = k * num9;
                        x = Mathf.Lerp(a, num19, t);
                        f = Mathf.Lerp(num17, b, t);
                        x = Mathf.Round(x);
                        f = Mathf.Round(f);
                        verts.buffer[index++].x = x;
                        verts.buffer[index++].x = x;
                        verts.buffer[index++].x = f;
                        verts.buffer[index++].x = f;
                    }
                    break;
                }
                return;
        }
    }

    public static int CalculateOffsetToFit(string text)
    {
        if (string.IsNullOrEmpty(text) || (NGUIText.regionWidth < 1))
        {
            return 0;
        }
        Prepare(text);
        int length = text.Length;
        int ch = 0;
        int prev = 0;
        int index = 0;
        int num5 = text.Length;
        while (index < num5)
        {
            BMSymbol symbol = !useSymbols ? null : GetSymbol(text, index, length);
            if (symbol == null)
            {
                ch = text[index];
                float glyphWidth = GetGlyphWidth(ch, prev);
                if (glyphWidth != 0f)
                {
                    mSizes.Add(finalSpacingX + glyphWidth);
                }
                prev = ch;
            }
            else
            {
                mSizes.Add(finalSpacingX + (symbol.advance * fontScale));
                int num7 = 0;
                int num8 = symbol.sequence.Length - 1;
                while (num7 < num8)
                {
                    mSizes.Add(0f);
                    num7++;
                }
                index += symbol.sequence.Length - 1;
                prev = 0;
            }
            index++;
        }
        float regionWidth = NGUIText.regionWidth;
        int size = mSizes.size;
        while ((size > 0) && (regionWidth > 0f))
        {
            regionWidth -= mSizes[--size];
        }
        mSizes.Clear();
        if (regionWidth < 0f)
        {
            size++;
        }
        return size;
    }

    public static Vector2 CalculatePrintedSize(string text)
    {
        Vector2 zero = Vector2.zero;
        if (!string.IsNullOrEmpty(text))
        {
            if (encoding)
            {
                text = StripSymbols(text);
            }
            Prepare(text);
            float num = 0f;
            float num2 = 0f;
            float num3 = 0f;
            int length = text.Length;
            int ch = 0;
            int prev = 0;
            for (int i = 0; i < length; i++)
            {
                ch = text[i];
                if (ch == 10)
                {
                    if (num > num3)
                    {
                        num3 = num;
                    }
                    num = 0f;
                    num2 += finalLineHeight;
                }
                else if (ch >= 0x20)
                {
                    BMSymbol symbol = !useSymbols ? null : GetSymbol(text, i, length);
                    if (symbol == null)
                    {
                        float glyphWidth = GetGlyphWidth(ch, prev);
                        if (glyphWidth != 0f)
                        {
                            glyphWidth += finalSpacingX;
                            if (Mathf.RoundToInt(num + glyphWidth) > regionWidth)
                            {
                                if (num > num3)
                                {
                                    num3 = num - finalSpacingX;
                                }
                                num = glyphWidth;
                                num2 += finalLineHeight;
                            }
                            else
                            {
                                num += glyphWidth;
                            }
                            prev = ch;
                        }
                    }
                    else
                    {
                        float num9 = finalSpacingX + (symbol.advance * fontScale);
                        if (Mathf.RoundToInt(num + num9) > regionWidth)
                        {
                            if (num > num3)
                            {
                                num3 = num - finalSpacingX;
                            }
                            num = num9;
                            num2 += finalLineHeight;
                        }
                        else
                        {
                            num += num9;
                        }
                        i += symbol.sequence.Length - 1;
                        prev = 0;
                    }
                }
            }
            zero.x = (num <= num3) ? num3 : (num - finalSpacingX);
            zero.y = num2 + finalLineHeight;
        }
        return zero;
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static string EncodeAlpha(float a) => 
        NGUIMath.DecimalToHex8(Mathf.Clamp(Mathf.RoundToInt(a * 255f), 0, 0xff));

    [DebuggerHidden, DebuggerStepThrough]
    public static string EncodeColor(Color c) => 
        EncodeColor24(c);

    [DebuggerHidden, DebuggerStepThrough]
    public static string EncodeColor(string text, Color c)
    {
        string[] textArray1 = new string[] { "[c][", EncodeColor24(c), "]", text, "[-][/c]" };
        return string.Concat(textArray1);
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static string EncodeColor24(Color c)
    {
        int num = 0xffffff & (NGUIMath.ColorToInt(c) >> 8);
        return NGUIMath.DecimalToHex24(num);
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static string EncodeColor32(Color c) => 
        NGUIMath.DecimalToHex32(NGUIMath.ColorToInt(c));

    [DebuggerStepThrough, DebuggerHidden]
    public static void EndLine(ref StringBuilder s)
    {
        int num = s.Length - 1;
        if ((num > 0) && IsSpace(s[num]))
        {
            s[num] = '\n';
        }
        else
        {
            s.Append('\n');
        }
    }

    public static int GetApproximateCharacterIndex(BetterList<Vector3> verts, BetterList<int> indices, Vector2 pos)
    {
        float maxValue = float.MaxValue;
        float num2 = float.MaxValue;
        int num3 = 0;
        for (int i = 0; i < verts.size; i++)
        {
            Vector3 vector = verts[i];
            float num5 = Mathf.Abs((float) (pos.y - vector.y));
            if (num5 <= num2)
            {
                Vector3 vector2 = verts[i];
                float num6 = Mathf.Abs((float) (pos.x - vector2.x));
                if (num5 < num2)
                {
                    num2 = num5;
                    maxValue = num6;
                    num3 = i;
                }
                else if (num6 < maxValue)
                {
                    maxValue = num6;
                    num3 = i;
                }
            }
        }
        return indices[num3];
    }

    public static string GetEndOfLineThatFits(string text)
    {
        int length = text.Length;
        int startIndex = CalculateOffsetToFit(text);
        return text.Substring(startIndex, length - startIndex);
    }

    public static int GetExactCharacterIndex(BetterList<Vector3> verts, BetterList<int> indices, Vector2 pos)
    {
        for (int i = 0; i < indices.size; i++)
        {
            int num2 = i << 1;
            int num3 = num2 + 1;
            Vector3 vector = verts[num2];
            float x = vector.x;
            if (pos.x >= x)
            {
                Vector3 vector2 = verts[num3];
                float num5 = vector2.x;
                if (pos.x <= num5)
                {
                    Vector3 vector3 = verts[num2];
                    float y = vector3.y;
                    if (pos.y >= y)
                    {
                        Vector3 vector4 = verts[num3];
                        float num7 = vector4.y;
                        if (pos.y <= num7)
                        {
                            return indices[i];
                        }
                    }
                }
            }
        }
        return 0;
    }

    public static GlyphInfo GetGlyph(int ch, int prev)
    {
        if (bitmapFont != null)
        {
            bool flag = false;
            if (ch == 0x2009)
            {
                flag = true;
                ch = 0x20;
            }
            BMGlyph glyph = bitmapFont.bmFont.GetGlyph(ch);
            if (glyph != null)
            {
                int num = (prev == 0) ? 0 : glyph.GetKerning(prev);
                NGUIText.glyph.v0.x = (prev == 0) ? ((float) glyph.offsetX) : ((float) (glyph.offsetX + num));
                NGUIText.glyph.v1.y = -glyph.offsetY;
                NGUIText.glyph.v1.x = NGUIText.glyph.v0.x + glyph.width;
                NGUIText.glyph.v0.y = NGUIText.glyph.v1.y - glyph.height;
                NGUIText.glyph.u0.x = glyph.x;
                NGUIText.glyph.u0.y = glyph.y + glyph.height;
                NGUIText.glyph.u2.x = glyph.x + glyph.width;
                NGUIText.glyph.u2.y = glyph.y;
                NGUIText.glyph.u1.x = NGUIText.glyph.u0.x;
                NGUIText.glyph.u1.y = NGUIText.glyph.u2.y;
                NGUIText.glyph.u3.x = NGUIText.glyph.u2.x;
                NGUIText.glyph.u3.y = NGUIText.glyph.u0.y;
                int advance = glyph.advance;
                if (flag)
                {
                    advance = advance >> 1;
                }
                NGUIText.glyph.advance = advance + num;
                NGUIText.glyph.channel = glyph.channel;
                if (fontScale != 1f)
                {
                    NGUIText.glyph.v0 = (Vector2) (NGUIText.glyph.v0 * fontScale);
                    NGUIText.glyph.v1 = (Vector2) (NGUIText.glyph.v1 * fontScale);
                    NGUIText.glyph.advance *= fontScale;
                }
                return NGUIText.glyph;
            }
        }
        else if ((dynamicFont != null) && dynamicFont.GetCharacterInfo((char) ch, out mTempChar, finalSize, fontStyle))
        {
            NGUIText.glyph.v0.x = mTempChar.minX;
            NGUIText.glyph.v1.x = mTempChar.maxX;
            NGUIText.glyph.v0.y = mTempChar.maxY - baseline;
            NGUIText.glyph.v1.y = mTempChar.minY - baseline;
            NGUIText.glyph.u0 = mTempChar.uvTopLeft;
            NGUIText.glyph.u1 = mTempChar.uvBottomLeft;
            NGUIText.glyph.u2 = mTempChar.uvBottomRight;
            NGUIText.glyph.u3 = mTempChar.uvTopRight;
            NGUIText.glyph.advance = mTempChar.advance;
            NGUIText.glyph.channel = 0;
            NGUIText.glyph.v0.x = Mathf.Round(NGUIText.glyph.v0.x);
            NGUIText.glyph.v0.y = Mathf.Round(NGUIText.glyph.v0.y);
            NGUIText.glyph.v1.x = Mathf.Round(NGUIText.glyph.v1.x);
            NGUIText.glyph.v1.y = Mathf.Round(NGUIText.glyph.v1.y);
            float num3 = fontScale * pixelDensity;
            if (num3 != 1f)
            {
                NGUIText.glyph.v0 = (Vector2) (NGUIText.glyph.v0 * num3);
                NGUIText.glyph.v1 = (Vector2) (NGUIText.glyph.v1 * num3);
                NGUIText.glyph.advance *= num3;
            }
            return NGUIText.glyph;
        }
        return null;
    }

    public static float GetGlyphWidth(int ch, int prev)
    {
        if (bitmapFont != null)
        {
            bool flag = false;
            if (ch == 0x2009)
            {
                flag = true;
                ch = 0x20;
            }
            BMGlyph glyph = bitmapFont.bmFont.GetGlyph(ch);
            if (glyph != null)
            {
                int advance = glyph.advance;
                if (flag)
                {
                    advance = advance >> 1;
                }
                return (fontScale * ((prev == 0) ? ((float) glyph.advance) : ((float) (advance + glyph.GetKerning(prev)))));
            }
        }
        else if ((dynamicFont != null) && dynamicFont.GetCharacterInfo((char) ch, out mTempChar, finalSize, fontStyle))
        {
            return ((mTempChar.advance * fontScale) * pixelDensity);
        }
        return 0f;
    }

    public static BMSymbol GetSymbol(string text, int index, int textLength) => 
        bitmapFont?.MatchSymbol(text, index, textLength);

    [DebuggerHidden, DebuggerStepThrough]
    public static bool IsHex(char ch) => 
        ((((ch >= '0') && (ch <= '9')) || ((ch >= 'a') && (ch <= 'f'))) || ((ch >= 'A') && (ch <= 'F')));

    [DebuggerHidden, DebuggerStepThrough]
    private static bool IsSpace(int ch) => 
        ((((ch == 0x20) || (ch == 0x200a)) || (ch == 0x200b)) || (ch == 0x2009));

    [DebuggerStepThrough, DebuggerHidden]
    public static float ParseAlpha(string text, int index)
    {
        int num = (NGUIMath.HexToDecimal(text[index + 1]) << 4) | NGUIMath.HexToDecimal(text[index + 2]);
        return Mathf.Clamp01(((float) num) / 255f);
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static Color ParseColor(string text, int offset) => 
        ParseColor24(text, offset);

    [DebuggerStepThrough, DebuggerHidden]
    public static Color ParseColor24(string text, int offset)
    {
        int num = (NGUIMath.HexToDecimal(text[offset]) << 4) | NGUIMath.HexToDecimal(text[offset + 1]);
        int num2 = (NGUIMath.HexToDecimal(text[offset + 2]) << 4) | NGUIMath.HexToDecimal(text[offset + 3]);
        int num3 = (NGUIMath.HexToDecimal(text[offset + 4]) << 4) | NGUIMath.HexToDecimal(text[offset + 5]);
        float num4 = 0.003921569f;
        return new Color(num4 * num, num4 * num2, num4 * num3);
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static Color ParseColor32(string text, int offset)
    {
        int num = (NGUIMath.HexToDecimal(text[offset]) << 4) | NGUIMath.HexToDecimal(text[offset + 1]);
        int num2 = (NGUIMath.HexToDecimal(text[offset + 2]) << 4) | NGUIMath.HexToDecimal(text[offset + 3]);
        int num3 = (NGUIMath.HexToDecimal(text[offset + 4]) << 4) | NGUIMath.HexToDecimal(text[offset + 5]);
        int num4 = (NGUIMath.HexToDecimal(text[offset + 6]) << 4) | NGUIMath.HexToDecimal(text[offset + 7]);
        float num5 = 0.003921569f;
        return new Color(num5 * num, num5 * num2, num5 * num3, num5 * num4);
    }

    public static bool ParseSymbol(string text, ref int index)
    {
        int sub = 1;
        bool bold = false;
        bool italic = false;
        bool underline = false;
        bool strike = false;
        bool ignoreColor = false;
        return ParseSymbol(text, ref index, null, false, ref sub, ref bold, ref italic, ref underline, ref strike, ref ignoreColor);
    }

    public static bool ParseSymbol(string text, ref int index, BetterList<Color> colors, bool premultiply, ref int sub, ref bool bold, ref bool italic, ref bool underline, ref bool strike, ref bool ignoreColor)
    {
        string str5;
        Dictionary<string, int> dictionary;
        int num4;
        int length = text.Length;
        if (((index + 3) > length) || (text[index] != '['))
        {
            return false;
        }
        if (text[index + 2] == ']')
        {
            if (text[index + 1] == '-')
            {
                if ((colors != null) && (colors.size > 1))
                {
                    colors.RemoveAt(colors.size - 1);
                }
                index += 3;
                return true;
            }
            str5 = text.Substring(index, 3);
            if (str5 != null)
            {
                if (<>f__switch$map36 == null)
                {
                    dictionary = new Dictionary<string, int>(5) {
                        { 
                            "[b]",
                            0
                        },
                        { 
                            "[i]",
                            1
                        },
                        { 
                            "[u]",
                            2
                        },
                        { 
                            "[s]",
                            3
                        },
                        { 
                            "[c]",
                            4
                        }
                    };
                    <>f__switch$map36 = dictionary;
                }
                if (<>f__switch$map36.TryGetValue(str5, out num4))
                {
                    switch (num4)
                    {
                        case 0:
                            bold = true;
                            index += 3;
                            return true;

                        case 1:
                            italic = true;
                            index += 3;
                            return true;

                        case 2:
                            underline = true;
                            index += 3;
                            return true;

                        case 3:
                            strike = true;
                            index += 3;
                            return true;

                        case 4:
                            ignoreColor = true;
                            index += 3;
                            return true;
                    }
                }
            }
        }
        if ((index + 4) > length)
        {
            return false;
        }
        if (text[index + 3] == ']')
        {
            str5 = text.Substring(index, 4);
            if (str5 != null)
            {
                if (<>f__switch$map37 == null)
                {
                    dictionary = new Dictionary<string, int>(5) {
                        { 
                            "[/b]",
                            0
                        },
                        { 
                            "[/i]",
                            1
                        },
                        { 
                            "[/u]",
                            2
                        },
                        { 
                            "[/s]",
                            3
                        },
                        { 
                            "[/c]",
                            4
                        }
                    };
                    <>f__switch$map37 = dictionary;
                }
                if (<>f__switch$map37.TryGetValue(str5, out num4))
                {
                    switch (num4)
                    {
                        case 0:
                            bold = false;
                            index += 4;
                            return true;

                        case 1:
                            italic = false;
                            index += 4;
                            return true;

                        case 2:
                            underline = false;
                            index += 4;
                            return true;

                        case 3:
                            strike = false;
                            index += 4;
                            return true;

                        case 4:
                            ignoreColor = false;
                            index += 4;
                            return true;
                    }
                }
            }
            char ch = text[index + 1];
            char ch2 = text[index + 2];
            if (IsHex(ch) && IsHex(ch2))
            {
                int num2 = (NGUIMath.HexToDecimal(ch) << 4) | NGUIMath.HexToDecimal(ch2);
                mAlpha = ((float) num2) / 255f;
                index += 4;
                return true;
            }
        }
        if ((index + 5) > length)
        {
            return false;
        }
        if (text[index + 4] == ']')
        {
            str5 = text.Substring(index, 5);
            if (str5 != null)
            {
                if (<>f__switch$map38 == null)
                {
                    dictionary = new Dictionary<string, int>(2) {
                        { 
                            "[sub]",
                            0
                        },
                        { 
                            "[sup]",
                            1
                        }
                    };
                    <>f__switch$map38 = dictionary;
                }
                if (<>f__switch$map38.TryGetValue(str5, out num4))
                {
                    if (num4 == 0)
                    {
                        sub = 1;
                        index += 5;
                        return true;
                    }
                    if (num4 == 1)
                    {
                        sub = 2;
                        index += 5;
                        return true;
                    }
                }
            }
        }
        if ((index + 6) > length)
        {
            return false;
        }
        if (text[index + 5] == ']')
        {
            str5 = text.Substring(index, 6);
            if (str5 != null)
            {
                if (<>f__switch$map39 == null)
                {
                    dictionary = new Dictionary<string, int>(3) {
                        { 
                            "[/sub]",
                            0
                        },
                        { 
                            "[/sup]",
                            1
                        },
                        { 
                            "[/url]",
                            2
                        }
                    };
                    <>f__switch$map39 = dictionary;
                }
                if (<>f__switch$map39.TryGetValue(str5, out num4))
                {
                    switch (num4)
                    {
                        case 0:
                            sub = 0;
                            index += 6;
                            return true;

                        case 1:
                            sub = 0;
                            index += 6;
                            return true;

                        case 2:
                            index += 6;
                            return true;
                    }
                }
            }
        }
        if (((text[index + 1] == 'u') && (text[index + 2] == 'r')) && ((text[index + 3] == 'l') && (text[index + 4] == '=')))
        {
            int num3 = text.IndexOf(']', index + 4);
            if (num3 != -1)
            {
                index = num3 + 1;
                return true;
            }
            index = text.Length;
            return true;
        }
        if ((index + 8) > length)
        {
            return false;
        }
        if (text[index + 7] == ']')
        {
            Color color = ParseColor24(text, index + 1);
            if (EncodeColor24(color) != text.Substring(index + 1, 6).ToUpper())
            {
                return false;
            }
            if (colors != null)
            {
                Color color3 = colors[colors.size - 1];
                color.a = color3.a;
                if (premultiply && (color.a != 1f))
                {
                    color = Color.Lerp(mInvisible, color, color.a);
                }
                colors.Add(color);
            }
            index += 8;
            return true;
        }
        if ((index + 10) > length)
        {
            return false;
        }
        if (text[index + 9] != ']')
        {
            return false;
        }
        Color c = ParseColor32(text, index + 1);
        if (EncodeColor32(c) != text.Substring(index + 1, 8).ToUpper())
        {
            return false;
        }
        if (colors != null)
        {
            if (premultiply && (c.a != 1f))
            {
                c = Color.Lerp(mInvisible, c, c.a);
            }
            colors.Add(c);
        }
        index += 10;
        return true;
    }

    public static void Prepare(string text)
    {
        if (dynamicFont != null)
        {
            dynamicFont.RequestCharactersInTexture(text, finalSize, fontStyle);
        }
    }

    public static void Print(string text, BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        if (!string.IsNullOrEmpty(text))
        {
            int size = verts.size;
            Prepare(text);
            mColors.Add(Color.white);
            mAlpha = 1f;
            int ch = 0;
            int prev = 0;
            float x = 0f;
            float num5 = 0f;
            float num6 = 0f;
            float finalSize = NGUIText.finalSize;
            Color a = NGUIText.tint * gradientBottom;
            Color b = NGUIText.tint * gradientTop;
            Color32 tint = NGUIText.tint;
            int length = text.Length;
            Rect uvRect = new Rect();
            float num9 = 0f;
            float num10 = 0f;
            float num11 = finalSize * pixelDensity;
            bool flag = false;
            int sub = 0;
            bool bold = false;
            bool italic = false;
            bool underline = false;
            bool strike = false;
            bool ignoreColor = false;
            float num18 = 0f;
            if (bitmapFont != null)
            {
                uvRect = bitmapFont.uvRect;
                num9 = uvRect.width / ((float) bitmapFont.texWidth);
                num10 = uvRect.height / ((float) bitmapFont.texHeight);
            }
            for (int i = 0; i < length; i++)
            {
                float num14;
                float num15;
                float num16;
                float num17;
                ch = text[i];
                num18 = x;
                if (ch == 10)
                {
                    if (x > num6)
                    {
                        num6 = x;
                    }
                    if (alignment != Alignment.Left)
                    {
                        Align(verts, size, x - finalSpacingX);
                        size = verts.size;
                    }
                    x = 0f;
                    num5 += finalLineHeight;
                    prev = 0;
                    continue;
                }
                if (ch < 0x20)
                {
                    prev = ch;
                    continue;
                }
                if (encoding && ParseSymbol(text, ref i, mColors, premultiply, ref sub, ref bold, ref italic, ref underline, ref strike, ref ignoreColor))
                {
                    Color color4;
                    if (ignoreColor)
                    {
                        color4 = mColors[mColors.size - 1];
                        color4.a *= mAlpha * NGUIText.tint.a;
                    }
                    else
                    {
                        color4 = NGUIText.tint * mColors[mColors.size - 1];
                        color4.a *= mAlpha;
                    }
                    tint = color4;
                    int num20 = 0;
                    int num21 = mColors.size - 2;
                    while (num20 < num21)
                    {
                        Color color8 = mColors[num20];
                        color4.a *= color8.a;
                        num20++;
                    }
                    if (gradient)
                    {
                        a = gradientBottom * color4;
                        b = gradientTop * color4;
                    }
                    i--;
                    continue;
                }
                BMSymbol symbol = !useSymbols ? null : GetSymbol(text, i, length);
                if (symbol != null)
                {
                    num14 = x + (symbol.offsetX * fontScale);
                    num15 = num14 + (symbol.width * fontScale);
                    num16 = -(num5 + (symbol.offsetY * fontScale));
                    num17 = num16 - (symbol.height * fontScale);
                    if (Mathf.RoundToInt(x + (symbol.advance * fontScale)) > regionWidth)
                    {
                        if (x == 0f)
                        {
                            return;
                        }
                        if ((alignment != Alignment.Left) && (size < verts.size))
                        {
                            Align(verts, size, x - finalSpacingX);
                            size = verts.size;
                        }
                        num14 -= x;
                        num15 -= x;
                        num17 -= finalLineHeight;
                        num16 -= finalLineHeight;
                        x = 0f;
                        num5 += finalLineHeight;
                        num18 = 0f;
                    }
                    verts.Add(new Vector3(num14, num17));
                    verts.Add(new Vector3(num14, num16));
                    verts.Add(new Vector3(num15, num16));
                    verts.Add(new Vector3(num15, num17));
                    x += finalSpacingX + (symbol.advance * fontScale);
                    i += symbol.length - 1;
                    prev = 0;
                    if (uvs != null)
                    {
                        Rect rect2 = symbol.uvRect;
                        float xMin = rect2.xMin;
                        float yMin = rect2.yMin;
                        float xMax = rect2.xMax;
                        float yMax = rect2.yMax;
                        uvs.Add(new Vector2(xMin, yMin));
                        uvs.Add(new Vector2(xMin, yMax));
                        uvs.Add(new Vector2(xMax, yMax));
                        uvs.Add(new Vector2(xMax, yMin));
                    }
                    if (cols != null)
                    {
                        if (symbolStyle == SymbolStyle.Colored)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                cols.Add(tint);
                            }
                        }
                        else
                        {
                            Color32 white = Color.white;
                            white.a = tint.a;
                            for (int k = 0; k < 4; k++)
                            {
                                cols.Add(white);
                            }
                        }
                    }
                    continue;
                }
                GlyphInfo glyph = GetGlyph(ch, prev);
                if (glyph != null)
                {
                    prev = ch;
                    if (sub != 0)
                    {
                        glyph.v0.x *= 0.75f;
                        glyph.v0.y *= 0.75f;
                        glyph.v1.x *= 0.75f;
                        glyph.v1.y *= 0.75f;
                        if (sub == 1)
                        {
                            glyph.v0.y -= (fontScale * fontSize) * 0.4f;
                            glyph.v1.y -= (fontScale * fontSize) * 0.4f;
                        }
                        else
                        {
                            glyph.v0.y += (fontScale * fontSize) * 0.05f;
                            glyph.v1.y += (fontScale * fontSize) * 0.05f;
                        }
                    }
                    num14 = glyph.v0.x + x;
                    num17 = glyph.v0.y - num5;
                    num15 = glyph.v1.x + x;
                    num16 = glyph.v1.y - num5;
                    float advance = glyph.advance;
                    if (finalSpacingX < 0f)
                    {
                        advance += finalSpacingX;
                    }
                    if (Mathf.RoundToInt(x + advance) > regionWidth)
                    {
                        if (x == 0f)
                        {
                            return;
                        }
                        if ((alignment != Alignment.Left) && (size < verts.size))
                        {
                            Align(verts, size, x - finalSpacingX);
                            size = verts.size;
                        }
                        num14 -= x;
                        num15 -= x;
                        num17 -= finalLineHeight;
                        num16 -= finalLineHeight;
                        x = 0f;
                        num5 += finalLineHeight;
                        num18 = 0f;
                    }
                    if (IsSpace(ch))
                    {
                        if (underline)
                        {
                            ch = 0x5f;
                        }
                        else if (strike)
                        {
                            ch = 0x2d;
                        }
                    }
                    x += (sub != 0) ? ((finalSpacingX + glyph.advance) * 0.75f) : (finalSpacingX + glyph.advance);
                    if (!IsSpace(ch))
                    {
                        if (uvs != null)
                        {
                            if (bitmapFont != null)
                            {
                                glyph.u0.x = uvRect.xMin + (num9 * glyph.u0.x);
                                glyph.u2.x = uvRect.xMin + (num9 * glyph.u2.x);
                                glyph.u0.y = uvRect.yMax - (num10 * glyph.u0.y);
                                glyph.u2.y = uvRect.yMax - (num10 * glyph.u2.y);
                                glyph.u1.x = glyph.u0.x;
                                glyph.u1.y = glyph.u2.y;
                                glyph.u3.x = glyph.u2.x;
                                glyph.u3.y = glyph.u0.y;
                            }
                            int num29 = 0;
                            int num30 = !bold ? 1 : 4;
                            while (num29 < num30)
                            {
                                uvs.Add(glyph.u0);
                                uvs.Add(glyph.u1);
                                uvs.Add(glyph.u2);
                                uvs.Add(glyph.u3);
                                num29++;
                            }
                        }
                        if (cols != null)
                        {
                            if ((glyph.channel == 0) || (glyph.channel == 15))
                            {
                                if (gradient)
                                {
                                    float t = num11 + (glyph.v0.y / fontScale);
                                    float num32 = num11 + (glyph.v1.y / fontScale);
                                    t /= num11;
                                    num32 /= num11;
                                    s_c0 = Color.Lerp(a, b, t);
                                    s_c1 = Color.Lerp(a, b, num32);
                                    int num33 = 0;
                                    int num34 = !bold ? 1 : 4;
                                    while (num33 < num34)
                                    {
                                        cols.Add(s_c0);
                                        cols.Add(s_c1);
                                        cols.Add(s_c1);
                                        cols.Add(s_c0);
                                        num33++;
                                    }
                                }
                                else
                                {
                                    int num35 = 0;
                                    int num36 = !bold ? 4 : 0x10;
                                    while (num35 < num36)
                                    {
                                        cols.Add(tint);
                                        num35++;
                                    }
                                }
                            }
                            else
                            {
                                Color color6 = (Color) tint;
                                color6 = (Color) (color6 * 0.49f);
                                switch (glyph.channel)
                                {
                                    case 1:
                                        color6.b += 0.51f;
                                        break;

                                    case 2:
                                        color6.g += 0.51f;
                                        break;

                                    case 4:
                                        color6.r += 0.51f;
                                        break;

                                    case 8:
                                        color6.a += 0.51f;
                                        break;
                                }
                                Color32 item = color6;
                                int num37 = 0;
                                int num38 = !bold ? 4 : 0x10;
                                while (num37 < num38)
                                {
                                    cols.Add(item);
                                    num37++;
                                }
                            }
                        }
                        if (!bold)
                        {
                            if (!italic)
                            {
                                verts.Add(new Vector3(num14, num17));
                                verts.Add(new Vector3(num14, num16));
                                verts.Add(new Vector3(num15, num16));
                                verts.Add(new Vector3(num15, num17));
                            }
                            else
                            {
                                float num39 = (fontSize * 0.1f) * ((num16 - num17) / ((float) fontSize));
                                verts.Add(new Vector3(num14 - num39, num17));
                                verts.Add(new Vector3(num14 + num39, num16));
                                verts.Add(new Vector3(num15 + num39, num16));
                                verts.Add(new Vector3(num15 - num39, num17));
                            }
                        }
                        else
                        {
                            for (int m = 0; m < 4; m++)
                            {
                                float num41 = mBoldOffset[m * 2];
                                float num42 = mBoldOffset[(m * 2) + 1];
                                float num43 = !italic ? 0f : ((fontSize * 0.1f) * ((num16 - num17) / ((float) fontSize)));
                                verts.Add(new Vector3((num14 + num41) - num43, num17 + num42));
                                verts.Add(new Vector3((num14 + num41) + num43, num16 + num42));
                                verts.Add(new Vector3((num15 + num41) + num43, num16 + num42));
                                verts.Add(new Vector3((num15 + num41) - num43, num17 + num42));
                            }
                        }
                        if (underline || strike)
                        {
                            GlyphInfo info2 = GetGlyph(!strike ? 0x5f : 0x2d, prev);
                            if (info2 != null)
                            {
                                if (uvs != null)
                                {
                                    if (bitmapFont != null)
                                    {
                                        info2.u0.x = uvRect.xMin + (num9 * info2.u0.x);
                                        info2.u2.x = uvRect.xMin + (num9 * info2.u2.x);
                                        info2.u0.y = uvRect.yMax - (num10 * info2.u0.y);
                                        info2.u2.y = uvRect.yMax - (num10 * info2.u2.y);
                                    }
                                    float num44 = (info2.u0.x + info2.u2.x) * 0.5f;
                                    int num45 = 0;
                                    int num46 = !bold ? 1 : 4;
                                    while (num45 < num46)
                                    {
                                        uvs.Add(new Vector2(num44, info2.u0.y));
                                        uvs.Add(new Vector2(num44, info2.u2.y));
                                        uvs.Add(new Vector2(num44, info2.u2.y));
                                        uvs.Add(new Vector2(num44, info2.u0.y));
                                        num45++;
                                    }
                                }
                                if (flag && strike)
                                {
                                    num17 = (-num5 + info2.v0.y) * 0.75f;
                                    num16 = (-num5 + info2.v1.y) * 0.75f;
                                }
                                else
                                {
                                    num17 = -num5 + info2.v0.y;
                                    num16 = -num5 + info2.v1.y;
                                }
                                if (bold)
                                {
                                    for (int n = 0; n < 4; n++)
                                    {
                                        float num48 = mBoldOffset[n * 2];
                                        float num49 = mBoldOffset[(n * 2) + 1];
                                        verts.Add(new Vector3(num18 + num48, num17 + num49));
                                        verts.Add(new Vector3(num18 + num48, num16 + num49));
                                        verts.Add(new Vector3(x + num48, num16 + num49));
                                        verts.Add(new Vector3(x + num48, num17 + num49));
                                    }
                                }
                                else
                                {
                                    verts.Add(new Vector3(num18, num17));
                                    verts.Add(new Vector3(num18, num16));
                                    verts.Add(new Vector3(x, num16));
                                    verts.Add(new Vector3(x, num17));
                                }
                                if (gradient)
                                {
                                    float num50 = num11 + (info2.v0.y / fontScale);
                                    float num51 = num11 + (info2.v1.y / fontScale);
                                    num50 /= num11;
                                    num51 /= num11;
                                    s_c0 = Color.Lerp(a, b, num50);
                                    s_c1 = Color.Lerp(a, b, num51);
                                    int num52 = 0;
                                    int num53 = !bold ? 1 : 4;
                                    while (num52 < num53)
                                    {
                                        cols.Add(s_c0);
                                        cols.Add(s_c1);
                                        cols.Add(s_c1);
                                        cols.Add(s_c0);
                                        num52++;
                                    }
                                }
                                else
                                {
                                    int num54 = 0;
                                    int num55 = !bold ? 4 : 0x10;
                                    while (num54 < num55)
                                    {
                                        cols.Add(tint);
                                        num54++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if ((alignment != Alignment.Left) && (size < verts.size))
            {
                Align(verts, size, x - finalSpacingX);
                size = verts.size;
            }
            mColors.Clear();
        }
    }

    public static void PrintApproximateCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
    {
        if (string.IsNullOrEmpty(text))
        {
            text = " ";
        }
        Prepare(text);
        float x = 0f;
        float num2 = 0f;
        float num3 = 0f;
        float num4 = (fontSize * fontScale) * 0.5f;
        int length = text.Length;
        int size = verts.size;
        int ch = 0;
        int prev = 0;
        for (int i = 0; i < length; i++)
        {
            ch = text[i];
            verts.Add(new Vector3(x, -num2 - num4));
            indices.Add(i);
            if (ch == 10)
            {
                if (x > num3)
                {
                    num3 = x;
                }
                if (alignment != Alignment.Left)
                {
                    Align(verts, size, x - finalSpacingX);
                    size = verts.size;
                }
                x = 0f;
                num2 += finalLineHeight;
                prev = 0;
            }
            else if (ch < 0x20)
            {
                prev = 0;
            }
            else if (encoding && ParseSymbol(text, ref i))
            {
                i--;
            }
            else
            {
                BMSymbol symbol = !useSymbols ? null : GetSymbol(text, i, length);
                if (symbol == null)
                {
                    float glyphWidth = GetGlyphWidth(ch, prev);
                    if (glyphWidth != 0f)
                    {
                        glyphWidth += finalSpacingX;
                        if (Mathf.RoundToInt(x + glyphWidth) > regionWidth)
                        {
                            if (x == 0f)
                            {
                                return;
                            }
                            if ((alignment != Alignment.Left) && (size < verts.size))
                            {
                                Align(verts, size, x - finalSpacingX);
                                size = verts.size;
                            }
                            x = glyphWidth;
                            num2 += finalLineHeight;
                        }
                        else
                        {
                            x += glyphWidth;
                        }
                        verts.Add(new Vector3(x, -num2 - num4));
                        indices.Add(i + 1);
                        prev = ch;
                    }
                }
                else
                {
                    float num11 = (symbol.advance * fontScale) + finalSpacingX;
                    if (Mathf.RoundToInt(x + num11) > regionWidth)
                    {
                        if (x == 0f)
                        {
                            return;
                        }
                        if ((alignment != Alignment.Left) && (size < verts.size))
                        {
                            Align(verts, size, x - finalSpacingX);
                            size = verts.size;
                        }
                        x = num11;
                        num2 += finalLineHeight;
                    }
                    else
                    {
                        x += num11;
                    }
                    verts.Add(new Vector3(x, -num2 - num4));
                    indices.Add(i + 1);
                    i += symbol.sequence.Length - 1;
                    prev = 0;
                }
            }
        }
        if ((alignment != Alignment.Left) && (size < verts.size))
        {
            Align(verts, size, x - finalSpacingX);
        }
    }

    public static void PrintCaretAndSelection(string text, int start, int end, BetterList<Vector3> caret, BetterList<Vector3> highlight)
    {
        if (string.IsNullOrEmpty(text))
        {
            text = " ";
        }
        Prepare(text);
        int num = end;
        if (start > end)
        {
            end = start;
            start = num;
        }
        float x = 0f;
        float num3 = 0f;
        float num4 = 0f;
        float num5 = fontSize * fontScale;
        int indexOffset = (caret == null) ? 0 : caret.size;
        int size = (highlight == null) ? 0 : highlight.size;
        int length = text.Length;
        int index = 0;
        int ch = 0;
        int prev = 0;
        bool flag = false;
        bool flag2 = false;
        Vector2 zero = Vector2.zero;
        Vector2 vector2 = Vector2.zero;
        while (index < length)
        {
            if (((caret != null) && !flag2) && (num <= index))
            {
                flag2 = true;
                caret.Add(new Vector3(x - 1f, -num3 - num5));
                caret.Add(new Vector3(x - 1f, -num3));
                caret.Add(new Vector3(x + 1f, -num3));
                caret.Add(new Vector3(x + 1f, -num3 - num5));
            }
            ch = text[index];
            if (ch == 10)
            {
                if (x > num4)
                {
                    num4 = x;
                }
                if ((caret != null) && flag2)
                {
                    if (alignment != Alignment.Left)
                    {
                        Align(caret, indexOffset, x - finalSpacingX);
                    }
                    caret = null;
                }
                if (highlight != null)
                {
                    if (flag)
                    {
                        flag = false;
                        highlight.Add((Vector3) vector2);
                        highlight.Add((Vector3) zero);
                    }
                    else if ((start <= index) && (end > index))
                    {
                        highlight.Add(new Vector3(x, -num3 - num5));
                        highlight.Add(new Vector3(x, -num3));
                        highlight.Add(new Vector3(x + 2f, -num3));
                        highlight.Add(new Vector3(x + 2f, -num3 - num5));
                    }
                    if ((alignment != Alignment.Left) && (size < highlight.size))
                    {
                        Align(highlight, size, x - finalSpacingX);
                        size = highlight.size;
                    }
                }
                x = 0f;
                num3 += finalLineHeight;
                prev = 0;
            }
            else if (ch < 0x20)
            {
                prev = 0;
            }
            else if (encoding && ParseSymbol(text, ref index))
            {
                index--;
            }
            else
            {
                BMSymbol symbol = !useSymbols ? null : GetSymbol(text, index, length);
                float num12 = (symbol == null) ? GetGlyphWidth(ch, prev) : (symbol.advance * fontScale);
                if (num12 != 0f)
                {
                    float num13 = x;
                    float num14 = x + num12;
                    float y = -num3 - num5;
                    float num16 = -num3;
                    if (Mathf.RoundToInt(num14 + finalSpacingX) > regionWidth)
                    {
                        if (x == 0f)
                        {
                            return;
                        }
                        if (x > num4)
                        {
                            num4 = x;
                        }
                        if ((caret != null) && flag2)
                        {
                            if (alignment != Alignment.Left)
                            {
                                Align(caret, indexOffset, x - finalSpacingX);
                            }
                            caret = null;
                        }
                        if (highlight != null)
                        {
                            if (flag)
                            {
                                flag = false;
                                highlight.Add((Vector3) vector2);
                                highlight.Add((Vector3) zero);
                            }
                            else if ((start <= index) && (end > index))
                            {
                                highlight.Add(new Vector3(x, -num3 - num5));
                                highlight.Add(new Vector3(x, -num3));
                                highlight.Add(new Vector3(x + 2f, -num3));
                                highlight.Add(new Vector3(x + 2f, -num3 - num5));
                            }
                            if ((alignment != Alignment.Left) && (size < highlight.size))
                            {
                                Align(highlight, size, x - finalSpacingX);
                                size = highlight.size;
                            }
                        }
                        num13 -= x;
                        num14 -= x;
                        y -= finalLineHeight;
                        num16 -= finalLineHeight;
                        x = 0f;
                        num3 += finalLineHeight;
                    }
                    x += num12 + finalSpacingX;
                    if (highlight != null)
                    {
                        if ((start > index) || (end <= index))
                        {
                            if (flag)
                            {
                                flag = false;
                                highlight.Add((Vector3) vector2);
                                highlight.Add((Vector3) zero);
                            }
                        }
                        else if (!flag)
                        {
                            flag = true;
                            highlight.Add(new Vector3(num13, y));
                            highlight.Add(new Vector3(num13, num16));
                        }
                    }
                    zero = new Vector2(num14, y);
                    vector2 = new Vector2(num14, num16);
                    prev = ch;
                }
            }
            index++;
        }
        if (caret != null)
        {
            if (!flag2)
            {
                caret.Add(new Vector3(x - 1f, -num3 - num5));
                caret.Add(new Vector3(x - 1f, -num3));
                caret.Add(new Vector3(x + 1f, -num3));
                caret.Add(new Vector3(x + 1f, -num3 - num5));
            }
            if (alignment != Alignment.Left)
            {
                Align(caret, indexOffset, x - finalSpacingX);
            }
        }
        if (highlight != null)
        {
            if (flag)
            {
                highlight.Add((Vector3) vector2);
                highlight.Add((Vector3) zero);
            }
            else if ((start < index) && (end == index))
            {
                highlight.Add(new Vector3(x, -num3 - num5));
                highlight.Add(new Vector3(x, -num3));
                highlight.Add(new Vector3(x + 2f, -num3));
                highlight.Add(new Vector3(x + 2f, -num3 - num5));
            }
            if ((alignment != Alignment.Left) && (size < highlight.size))
            {
                Align(highlight, size, x - finalSpacingX);
            }
        }
    }

    public static void PrintExactCharacterPositions(string text, BetterList<Vector3> verts, BetterList<int> indices)
    {
        if (string.IsNullOrEmpty(text))
        {
            text = " ";
        }
        Prepare(text);
        float num = fontSize * fontScale;
        float x = 0f;
        float num3 = 0f;
        float num4 = 0f;
        int length = text.Length;
        int size = verts.size;
        int ch = 0;
        int prev = 0;
        for (int i = 0; i < length; i++)
        {
            ch = text[i];
            if (ch == 10)
            {
                if (x > num4)
                {
                    num4 = x;
                }
                if (alignment != Alignment.Left)
                {
                    Align(verts, size, x - finalSpacingX);
                    size = verts.size;
                }
                x = 0f;
                num3 += finalLineHeight;
                prev = 0;
            }
            else if (ch < 0x20)
            {
                prev = 0;
            }
            else if (encoding && ParseSymbol(text, ref i))
            {
                i--;
            }
            else
            {
                BMSymbol symbol = !useSymbols ? null : GetSymbol(text, i, length);
                if (symbol == null)
                {
                    float glyphWidth = GetGlyphWidth(ch, prev);
                    if (glyphWidth != 0f)
                    {
                        float num11 = glyphWidth + finalSpacingX;
                        if (Mathf.RoundToInt(x + num11) > regionWidth)
                        {
                            if (x == 0f)
                            {
                                return;
                            }
                            if ((alignment != Alignment.Left) && (size < verts.size))
                            {
                                Align(verts, size, x - finalSpacingX);
                                size = verts.size;
                            }
                            x = 0f;
                            num3 += finalLineHeight;
                            prev = 0;
                            i--;
                        }
                        else
                        {
                            indices.Add(i);
                            verts.Add(new Vector3(x, -num3 - num));
                            verts.Add(new Vector3(x + num11, -num3));
                            prev = ch;
                            x += num11;
                        }
                    }
                }
                else
                {
                    float num12 = (symbol.advance * fontScale) + finalSpacingX;
                    if (Mathf.RoundToInt(x + num12) > regionWidth)
                    {
                        if (x == 0f)
                        {
                            return;
                        }
                        if ((alignment != Alignment.Left) && (size < verts.size))
                        {
                            Align(verts, size, x - finalSpacingX);
                            size = verts.size;
                        }
                        x = 0f;
                        num3 += finalLineHeight;
                        prev = 0;
                        i--;
                    }
                    else
                    {
                        indices.Add(i);
                        verts.Add(new Vector3(x, -num3 - num));
                        verts.Add(new Vector3(x + num12, -num3));
                        i += symbol.sequence.Length - 1;
                        x += num12;
                        prev = 0;
                    }
                }
            }
        }
        if ((alignment != Alignment.Left) && (size < verts.size))
        {
            Align(verts, size, x - finalSpacingX);
        }
    }

    [DebuggerStepThrough, DebuggerHidden]
    private static void ReplaceSpaceWithNewline(ref StringBuilder s)
    {
        int num = s.Length - 1;
        if ((num > 0) && IsSpace(s[num]))
        {
            s[num] = '\n';
        }
    }

    public static string StripSymbols(string text)
    {
        if (text != null)
        {
            int startIndex = 0;
            int length = text.Length;
            while (startIndex < length)
            {
                char ch = text[startIndex];
                if (ch == '[')
                {
                    int sub = 0;
                    bool bold = false;
                    bool italic = false;
                    bool underline = false;
                    bool strike = false;
                    bool ignoreColor = false;
                    int index = startIndex;
                    if (ParseSymbol(text, ref index, null, false, ref sub, ref bold, ref italic, ref underline, ref strike, ref ignoreColor))
                    {
                        text = text.Remove(startIndex, index - startIndex);
                        length = text.Length;
                        continue;
                    }
                }
                startIndex++;
            }
        }
        return text;
    }

    public static void Update()
    {
        Update(true);
    }

    public static void Update(bool request)
    {
        finalSize = Mathf.RoundToInt(((float) fontSize) / pixelDensity);
        finalSpacingX = spacingX * fontScale;
        finalLineHeight = (fontSize + spacingY) * fontScale;
        useSymbols = (((bitmapFont != null) && bitmapFont.hasSymbols) && encoding) && (symbolStyle != SymbolStyle.None);
        if ((dynamicFont != null) && request)
        {
            dynamicFont.RequestCharactersInTexture(")_-", finalSize, fontStyle);
            if (!dynamicFont.GetCharacterInfo(')', out mTempChar, finalSize, fontStyle) || (mTempChar.maxY == 0f))
            {
                dynamicFont.RequestCharactersInTexture("A", finalSize, fontStyle);
                if (!dynamicFont.GetCharacterInfo('A', out mTempChar, finalSize, fontStyle))
                {
                    baseline = 0f;
                    return;
                }
            }
            float maxY = mTempChar.maxY;
            float minY = mTempChar.minY;
            baseline = Mathf.Round(maxY + (((finalSize - maxY) + minY) * 0.5f));
        }
    }

    public static bool WrapText(string text, out string finalText) => 
        WrapText(text, out finalText, false);

    public static bool WrapText(string text, out string finalText, bool keepCharCount)
    {
        if (((NGUIText.regionWidth < 1) || (regionHeight < 1)) || (finalLineHeight < 1f))
        {
            finalText = string.Empty;
            return false;
        }
        float num = (maxLines <= 0) ? ((float) regionHeight) : Mathf.Min((float) regionHeight, finalLineHeight * maxLines);
        int b = (maxLines <= 0) ? 0xf4240 : maxLines;
        b = Mathf.FloorToInt(Mathf.Min((float) b, num / finalLineHeight) + 0.01f);
        if (b == 0)
        {
            finalText = string.Empty;
            return false;
        }
        if (string.IsNullOrEmpty(text))
        {
            text = " ";
        }
        Prepare(text);
        StringBuilder s = new StringBuilder();
        int length = text.Length;
        float regionWidth = NGUIText.regionWidth;
        int startIndex = 0;
        int index = 0;
        int num7 = 1;
        int prev = 0;
        bool flag = true;
        bool flag2 = true;
        bool flag3 = false;
        while (index < length)
        {
            float num9;
            char ch = text[index];
            if (ch > '⿿')
            {
                flag3 = true;
            }
            if (ch == '\n')
            {
                if (num7 == b)
                {
                    break;
                }
                regionWidth = NGUIText.regionWidth;
                if (startIndex < index)
                {
                    s.Append(text.Substring(startIndex, (index - startIndex) + 1));
                }
                else
                {
                    s.Append(ch);
                }
                flag = true;
                num7++;
                startIndex = index + 1;
                prev = 0;
                goto Label_038A;
            }
            if (encoding && ParseSymbol(text, ref index))
            {
                index--;
                goto Label_038A;
            }
            BMSymbol symbol = !useSymbols ? null : GetSymbol(text, index, length);
            if (symbol == null)
            {
                float glyphWidth = GetGlyphWidth(ch, prev);
                if (glyphWidth == 0f)
                {
                    goto Label_038A;
                }
                num9 = finalSpacingX + glyphWidth;
            }
            else
            {
                num9 = finalSpacingX + (symbol.advance * fontScale);
            }
            regionWidth -= num9;
            if ((IsSpace(ch) && !flag3) && (startIndex < index))
            {
                int num11 = (index - startIndex) + 1;
                if (((num7 == b) && (regionWidth <= 0f)) && (index < length))
                {
                    char ch2 = text[index];
                    if ((ch2 < ' ') || IsSpace(ch2))
                    {
                        num11--;
                    }
                }
                s.Append(text.Substring(startIndex, num11));
                flag = false;
                startIndex = index + 1;
                prev = ch;
            }
            if (Mathf.RoundToInt(regionWidth) < 0)
            {
                if (flag || (num7 == b))
                {
                    s.Append(text.Substring(startIndex, Mathf.Max(0, index - startIndex)));
                    bool flag4 = IsSpace(ch);
                    if (!flag4 && !flag3)
                    {
                        flag2 = false;
                    }
                    if (num7++ == b)
                    {
                        startIndex = index;
                        break;
                    }
                    if (keepCharCount)
                    {
                        ReplaceSpaceWithNewline(ref s);
                    }
                    else
                    {
                        EndLine(ref s);
                    }
                    flag = true;
                    if (flag4)
                    {
                        startIndex = index + 1;
                        regionWidth = NGUIText.regionWidth;
                    }
                    else
                    {
                        startIndex = index;
                        regionWidth = NGUIText.regionWidth - num9;
                    }
                    prev = 0;
                    goto Label_0372;
                }
                flag = true;
                regionWidth = NGUIText.regionWidth;
                index = startIndex - 1;
                prev = 0;
                if (num7++ == b)
                {
                    break;
                }
                if (keepCharCount)
                {
                    ReplaceSpaceWithNewline(ref s);
                }
                else
                {
                    EndLine(ref s);
                }
                goto Label_038A;
            }
            prev = ch;
        Label_0372:
            if (symbol != null)
            {
                index += symbol.length - 1;
                prev = 0;
            }
        Label_038A:
            index++;
        }
        if (startIndex < index)
        {
            s.Append(text.Substring(startIndex, index - startIndex));
        }
        finalText = s.Replace(@"\n", "\n").ToString();
        return (flag2 && ((index == length) || (num7 <= Mathf.Min(maxLines, b))));
    }

    public enum Alignment
    {
        Automatic,
        Left,
        Center,
        Right,
        Justified
    }

    public class GlyphInfo
    {
        public float advance;
        public int channel;
        public Vector2 u0;
        public Vector2 u1;
        public Vector2 u2;
        public Vector2 u3;
        public Vector2 v0;
        public Vector2 v1;
    }

    public enum SymbolStyle
    {
        None,
        Normal,
        Colored
    }
}

