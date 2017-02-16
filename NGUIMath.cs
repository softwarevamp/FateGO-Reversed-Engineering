using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public static class NGUIMath
{
    public static int AdjustByDPI(float height)
    {
        float dpi = Screen.dpi;
        RuntimePlatform platform = Application.platform;
        if (dpi == 0f)
        {
            dpi = ((platform != RuntimePlatform.Android) && (platform != RuntimePlatform.IPhonePlayer)) ? 96f : 160f;
        }
        int num2 = Mathf.RoundToInt(height * (96f / dpi));
        if ((num2 & 1) == 1)
        {
            num2++;
        }
        return num2;
    }

    public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top)
    {
        AdjustWidget(w, left, bottom, right, top, 2, 2, 0x186a0, 0x186a0);
    }

    public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top, int minWidth, int minHeight)
    {
        AdjustWidget(w, left, bottom, right, top, minWidth, minHeight, 0x186a0, 0x186a0);
    }

    public static void AdjustWidget(UIWidget w, float left, float bottom, float right, float top, int minWidth, int minHeight, int maxWidth, int maxHeight)
    {
        Vector2 pivotOffset = w.pivotOffset;
        Transform cachedTransform = w.cachedTransform;
        Quaternion localRotation = cachedTransform.localRotation;
        int num = Mathf.FloorToInt(left + 0.5f);
        int num2 = Mathf.FloorToInt(bottom + 0.5f);
        int num3 = Mathf.FloorToInt(right + 0.5f);
        int num4 = Mathf.FloorToInt(top + 0.5f);
        if ((pivotOffset.x == 0.5f) && ((num == 0) || (num3 == 0)))
        {
            num = (num >> 1) << 1;
            num3 = (num3 >> 1) << 1;
        }
        if ((pivotOffset.y == 0.5f) && ((num2 == 0) || (num4 == 0)))
        {
            num2 = (num2 >> 1) << 1;
            num4 = (num4 >> 1) << 1;
        }
        Vector3 vector2 = (Vector3) (localRotation * new Vector3((float) num, (float) num4));
        Vector3 vector3 = (Vector3) (localRotation * new Vector3((float) num3, (float) num4));
        Vector3 vector4 = (Vector3) (localRotation * new Vector3((float) num, (float) num2));
        Vector3 vector5 = (Vector3) (localRotation * new Vector3((float) num3, (float) num2));
        Vector3 vector6 = (Vector3) (localRotation * new Vector3((float) num, 0f));
        Vector3 vector7 = (Vector3) (localRotation * new Vector3((float) num3, 0f));
        Vector3 vector8 = (Vector3) (localRotation * new Vector3(0f, (float) num4));
        Vector3 vector9 = (Vector3) (localRotation * new Vector3(0f, (float) num2));
        Vector3 zero = Vector3.zero;
        if ((pivotOffset.x == 0f) && (pivotOffset.y == 1f))
        {
            zero.x = vector2.x;
            zero.y = vector2.y;
        }
        else if ((pivotOffset.x == 1f) && (pivotOffset.y == 0f))
        {
            zero.x = vector5.x;
            zero.y = vector5.y;
        }
        else if ((pivotOffset.x == 0f) && (pivotOffset.y == 0f))
        {
            zero.x = vector4.x;
            zero.y = vector4.y;
        }
        else if ((pivotOffset.x == 1f) && (pivotOffset.y == 1f))
        {
            zero.x = vector3.x;
            zero.y = vector3.y;
        }
        else if ((pivotOffset.x == 0f) && (pivotOffset.y == 0.5f))
        {
            zero.x = vector6.x + ((vector8.x + vector9.x) * 0.5f);
            zero.y = vector6.y + ((vector8.y + vector9.y) * 0.5f);
        }
        else if ((pivotOffset.x == 1f) && (pivotOffset.y == 0.5f))
        {
            zero.x = vector7.x + ((vector8.x + vector9.x) * 0.5f);
            zero.y = vector7.y + ((vector8.y + vector9.y) * 0.5f);
        }
        else if ((pivotOffset.x == 0.5f) && (pivotOffset.y == 1f))
        {
            zero.x = vector8.x + ((vector6.x + vector7.x) * 0.5f);
            zero.y = vector8.y + ((vector6.y + vector7.y) * 0.5f);
        }
        else if ((pivotOffset.x == 0.5f) && (pivotOffset.y == 0f))
        {
            zero.x = vector9.x + ((vector6.x + vector7.x) * 0.5f);
            zero.y = vector9.y + ((vector6.y + vector7.y) * 0.5f);
        }
        else if ((pivotOffset.x == 0.5f) && (pivotOffset.y == 0.5f))
        {
            zero.x = (((vector6.x + vector7.x) + vector8.x) + vector9.x) * 0.5f;
            zero.y = (((vector8.y + vector9.y) + vector6.y) + vector7.y) * 0.5f;
        }
        minWidth = Mathf.Max(minWidth, w.minWidth);
        minHeight = Mathf.Max(minHeight, w.minHeight);
        int num5 = (w.width + num3) - num;
        int h = (w.height + num4) - num2;
        Vector3 vector11 = Vector3.zero;
        int num7 = num5;
        if (num5 < minWidth)
        {
            num7 = minWidth;
        }
        else if (num5 > maxWidth)
        {
            num7 = maxWidth;
        }
        if (num5 != num7)
        {
            if (num != 0)
            {
                vector11.x -= Mathf.Lerp((float) (num7 - num5), 0f, pivotOffset.x);
            }
            else
            {
                vector11.x += Mathf.Lerp(0f, (float) (num7 - num5), pivotOffset.x);
            }
            num5 = num7;
        }
        int num8 = h;
        if (h < minHeight)
        {
            num8 = minHeight;
        }
        else if (h > maxHeight)
        {
            num8 = maxHeight;
        }
        if (h != num8)
        {
            if (num2 != 0)
            {
                vector11.y -= Mathf.Lerp((float) (num8 - h), 0f, pivotOffset.y);
            }
            else
            {
                vector11.y += Mathf.Lerp(0f, (float) (num8 - h), pivotOffset.y);
            }
            h = num8;
        }
        if (pivotOffset.x == 0.5f)
        {
            num5 = (num5 >> 1) << 1;
        }
        if (pivotOffset.y == 0.5f)
        {
            h = (h >> 1) << 1;
        }
        Vector3 vector12 = (cachedTransform.localPosition + zero) + (localRotation * vector11);
        cachedTransform.localPosition = vector12;
        w.SetDimensions(num5, h);
        if (w.isAnchored)
        {
            cachedTransform = cachedTransform.parent;
            float localPos = vector12.x - (pivotOffset.x * num5);
            float num10 = vector12.y - (pivotOffset.y * h);
            if (w.leftAnchor.target != null)
            {
                w.leftAnchor.SetHorizontal(cachedTransform, localPos);
            }
            if (w.rightAnchor.target != null)
            {
                w.rightAnchor.SetHorizontal(cachedTransform, localPos + num5);
            }
            if (w.bottomAnchor.target != null)
            {
                w.bottomAnchor.SetVertical(cachedTransform, num10);
            }
            if (w.topAnchor.target != null)
            {
                w.topAnchor.SetVertical(cachedTransform, num10 + h);
            }
        }
    }

    public static Bounds CalculateAbsoluteWidgetBounds(Transform trans)
    {
        if (trans == null)
        {
            return new Bounds(Vector3.zero, Vector3.zero);
        }
        UIWidget[] componentsInChildren = trans.GetComponentsInChildren<UIWidget>();
        if (componentsInChildren.Length == 0)
        {
            return new Bounds(trans.position, Vector3.zero);
        }
        Vector3 center = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
        Vector3 point = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        int index = 0;
        int length = componentsInChildren.Length;
        while (index < length)
        {
            UIWidget widget = componentsInChildren[index];
            if (widget.enabled)
            {
                Vector3[] worldCorners = widget.worldCorners;
                for (int i = 0; i < 4; i++)
                {
                    Vector3 vector3 = worldCorners[i];
                    if (vector3.x > point.x)
                    {
                        point.x = vector3.x;
                    }
                    if (vector3.y > point.y)
                    {
                        point.y = vector3.y;
                    }
                    if (vector3.z > point.z)
                    {
                        point.z = vector3.z;
                    }
                    if (vector3.x < center.x)
                    {
                        center.x = vector3.x;
                    }
                    if (vector3.y < center.y)
                    {
                        center.y = vector3.y;
                    }
                    if (vector3.z < center.z)
                    {
                        center.z = vector3.z;
                    }
                }
            }
            index++;
        }
        Bounds bounds = new Bounds(center, Vector3.zero);
        bounds.Encapsulate(point);
        return bounds;
    }

    public static Bounds CalculateRelativeWidgetBounds(Transform trans) => 
        CalculateRelativeWidgetBounds(trans, trans, false, true);

    public static Bounds CalculateRelativeWidgetBounds(Transform trans, bool considerInactive) => 
        CalculateRelativeWidgetBounds(trans, trans, considerInactive, true);

    public static Bounds CalculateRelativeWidgetBounds(Transform relativeTo, Transform content) => 
        CalculateRelativeWidgetBounds(relativeTo, content, false, true);

    public static Bounds CalculateRelativeWidgetBounds(Transform relativeTo, Transform content, bool considerInactive, bool considerParents = true)
    {
        if ((content != null) && (relativeTo != null))
        {
            bool isSet = false;
            Matrix4x4 worldToLocalMatrix = relativeTo.worldToLocalMatrix;
            Vector3 vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
            CalculateRelativeWidgetBounds(content, considerInactive, true, ref worldToLocalMatrix, ref vMin, ref vMax, ref isSet, considerParents);
            if (isSet)
            {
                Bounds bounds = new Bounds(vMin, Vector3.zero);
                bounds.Encapsulate(vMax);
                return bounds;
            }
        }
        return new Bounds(Vector3.zero, Vector3.zero);
    }

    [DebuggerStepThrough, DebuggerHidden]
    private static void CalculateRelativeWidgetBounds(Transform content, bool considerInactive, bool isRoot, ref Matrix4x4 toLocal, ref Vector3 vMin, ref Vector3 vMax, ref bool isSet, bool considerParents)
    {
        if ((content != null) && (considerInactive || NGUITools.GetActive(content.gameObject)))
        {
            UIPanel panel = !isRoot ? content.GetComponent<UIPanel>() : null;
            if ((panel == null) || panel.enabled)
            {
                if ((panel != null) && (panel.clipping != UIDrawCall.Clipping.None))
                {
                    Vector3[] worldCorners = panel.worldCorners;
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 vector = toLocal.MultiplyPoint3x4(worldCorners[i]);
                        if (vector.x > vMax.x)
                        {
                            vMax.x = vector.x;
                        }
                        if (vector.y > vMax.y)
                        {
                            vMax.y = vector.y;
                        }
                        if (vector.z > vMax.z)
                        {
                            vMax.z = vector.z;
                        }
                        if (vector.x < vMin.x)
                        {
                            vMin.x = vector.x;
                        }
                        if (vector.y < vMin.y)
                        {
                            vMin.y = vector.y;
                        }
                        if (vector.z < vMin.z)
                        {
                            vMin.z = vector.z;
                        }
                        isSet = true;
                    }
                }
                else
                {
                    UIWidget component = content.GetComponent<UIWidget>();
                    if ((component != null) && component.enabled)
                    {
                        Vector3[] vectorArray2 = component.worldCorners;
                        for (int j = 0; j < 4; j++)
                        {
                            Vector3 vector2 = toLocal.MultiplyPoint3x4(vectorArray2[j]);
                            if (vector2.x > vMax.x)
                            {
                                vMax.x = vector2.x;
                            }
                            if (vector2.y > vMax.y)
                            {
                                vMax.y = vector2.y;
                            }
                            if (vector2.z > vMax.z)
                            {
                                vMax.z = vector2.z;
                            }
                            if (vector2.x < vMin.x)
                            {
                                vMin.x = vector2.x;
                            }
                            if (vector2.y < vMin.y)
                            {
                                vMin.y = vector2.y;
                            }
                            if (vector2.z < vMin.z)
                            {
                                vMin.z = vector2.z;
                            }
                            isSet = true;
                        }
                        if (!considerParents)
                        {
                            return;
                        }
                    }
                    int index = 0;
                    int childCount = content.childCount;
                    while (index < childCount)
                    {
                        CalculateRelativeWidgetBounds(content.GetChild(index), considerInactive, false, ref toLocal, ref vMin, ref vMax, ref isSet, true);
                        index++;
                    }
                }
            }
        }
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static int ClampIndex(int val, int max) => 
        ((val >= 0) ? ((val >= max) ? (max - 1) : val) : 0);

    [DebuggerHidden, DebuggerStepThrough]
    public static int ColorToInt(Color c)
    {
        int num = 0;
        num |= Mathf.RoundToInt(c.r * 255f) << 0x18;
        num |= Mathf.RoundToInt(c.g * 255f) << 0x10;
        num |= Mathf.RoundToInt(c.b * 255f) << 8;
        return (num | Mathf.RoundToInt(c.a * 255f));
    }

    public static Vector2 ConstrainRect(Vector2 minRect, Vector2 maxRect, Vector2 minArea, Vector2 maxArea)
    {
        Vector2 zero = Vector2.zero;
        float num = maxRect.x - minRect.x;
        float num2 = maxRect.y - minRect.y;
        float num3 = maxArea.x - minArea.x;
        float num4 = maxArea.y - minArea.y;
        if (num > num3)
        {
            float num5 = num - num3;
            minArea.x -= num5;
            maxArea.x += num5;
        }
        if (num2 > num4)
        {
            float num6 = num2 - num4;
            minArea.y -= num6;
            maxArea.y += num6;
        }
        if (minRect.x < minArea.x)
        {
            zero.x += minArea.x - minRect.x;
        }
        if (maxRect.x > maxArea.x)
        {
            zero.x -= maxRect.x - maxArea.x;
        }
        if (minRect.y < minArea.y)
        {
            zero.y += minArea.y - minRect.y;
        }
        if (maxRect.y > maxArea.y)
        {
            zero.y -= maxRect.y - maxArea.y;
        }
        return zero;
    }

    public static Rect ConvertToPixels(Rect rect, int width, int height, bool round)
    {
        Rect rect2 = rect;
        if (round)
        {
            rect2.xMin = Mathf.RoundToInt(rect.xMin * width);
            rect2.xMax = Mathf.RoundToInt(rect.xMax * width);
            rect2.yMin = Mathf.RoundToInt((1f - rect.yMax) * height);
            rect2.yMax = Mathf.RoundToInt((1f - rect.yMin) * height);
            return rect2;
        }
        rect2.xMin = rect.xMin * width;
        rect2.xMax = rect.xMax * width;
        rect2.yMin = (1f - rect.yMax) * height;
        rect2.yMax = (1f - rect.yMin) * height;
        return rect2;
    }

    public static Rect ConvertToTexCoords(Rect rect, int width, int height)
    {
        Rect rect2 = rect;
        if ((width != 0f) && (height != 0f))
        {
            rect2.xMin = rect.xMin / ((float) width);
            rect2.xMax = rect.xMax / ((float) width);
            rect2.yMin = 1f - (rect.yMax / ((float) height));
            rect2.yMax = 1f - (rect.yMin / ((float) height));
        }
        return rect2;
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static string DecimalToHex24(int num)
    {
        num &= 0xffffff;
        return num.ToString("X6");
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static string DecimalToHex32(int num) => 
        num.ToString("X8");

    [DebuggerHidden, DebuggerStepThrough]
    public static string DecimalToHex8(int num)
    {
        num &= 0xff;
        return num.ToString("X2");
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static char DecimalToHexChar(int num)
    {
        if (num > 15)
        {
            return 'F';
        }
        if (num < 10)
        {
            return (char) (0x30 + num);
        }
        return (char) ((0x41 + num) - 10);
    }

    private static float DistancePointToLineSegment(Vector2 point, Vector2 a, Vector2 b)
    {
        Vector2 vector2 = b - a;
        float sqrMagnitude = vector2.sqrMagnitude;
        if (sqrMagnitude == 0f)
        {
            Vector2 vector3 = point - a;
            return vector3.magnitude;
        }
        float num2 = Vector2.Dot(point - a, b - a) / sqrMagnitude;
        if (num2 < 0f)
        {
            Vector2 vector4 = point - a;
            return vector4.magnitude;
        }
        if (num2 > 1f)
        {
            Vector2 vector5 = point - b;
            return vector5.magnitude;
        }
        Vector2 vector = a + ((Vector2) (num2 * (b - a)));
        Vector2 vector6 = point - vector;
        return vector6.magnitude;
    }

    public static unsafe float DistanceToRectangle(Vector2[] screenPoints, Vector2 mousePos)
    {
        bool flag = false;
        int val = 4;
        for (int i = 0; i < 5; i++)
        {
            Vector3 vector = *((Vector3*) &(screenPoints[RepeatIndex(i, 4)]));
            Vector3 vector2 = *((Vector3*) &(screenPoints[RepeatIndex(val, 4)]));
            if (((vector.y > mousePos.y) != (vector2.y > mousePos.y)) && (mousePos.x < ((((vector2.x - vector.x) * (mousePos.y - vector.y)) / (vector2.y - vector.y)) + vector.x)))
            {
                flag = !flag;
            }
            val = i;
        }
        if (flag)
        {
            return 0f;
        }
        float num4 = -1f;
        for (int j = 0; j < 4; j++)
        {
            Vector3 a = *((Vector3*) &(screenPoints[j]));
            Vector3 b = *((Vector3*) &(screenPoints[RepeatIndex(j + 1, 4)]));
            float num3 = DistancePointToLineSegment(mousePos, a, b);
            if ((num3 < num4) || (num4 < 0f))
            {
                num4 = num3;
            }
        }
        return num4;
    }

    public static float DistanceToRectangle(Vector3[] worldPoints, Vector2 mousePos, Camera cam)
    {
        Vector2[] screenPoints = new Vector2[4];
        for (int i = 0; i < 4; i++)
        {
            screenPoints[i] = cam.WorldToScreenPoint(worldPoints[i]);
        }
        return DistanceToRectangle(screenPoints, mousePos);
    }

    public static UIWidget.Pivot GetPivot(Vector2 offset)
    {
        if (offset.x == 0f)
        {
            if (offset.y == 0f)
            {
                return UIWidget.Pivot.BottomLeft;
            }
            if (offset.y == 1f)
            {
                return UIWidget.Pivot.TopLeft;
            }
            return UIWidget.Pivot.Left;
        }
        if (offset.x == 1f)
        {
            if (offset.y == 0f)
            {
                return UIWidget.Pivot.BottomRight;
            }
            if (offset.y == 1f)
            {
                return UIWidget.Pivot.TopRight;
            }
            return UIWidget.Pivot.Right;
        }
        if (offset.y == 0f)
        {
            return UIWidget.Pivot.Bottom;
        }
        if (offset.y == 1f)
        {
            return UIWidget.Pivot.Top;
        }
        return UIWidget.Pivot.Center;
    }

    public static Vector2 GetPivotOffset(UIWidget.Pivot pv)
    {
        Vector2 zero = Vector2.zero;
        if (((pv == UIWidget.Pivot.Top) || (pv == UIWidget.Pivot.Center)) || (pv == UIWidget.Pivot.Bottom))
        {
            zero.x = 0.5f;
        }
        else if (((pv == UIWidget.Pivot.TopRight) || (pv == UIWidget.Pivot.Right)) || (pv == UIWidget.Pivot.BottomRight))
        {
            zero.x = 1f;
        }
        else
        {
            zero.x = 0f;
        }
        if (((pv == UIWidget.Pivot.Left) || (pv == UIWidget.Pivot.Center)) || (pv == UIWidget.Pivot.Right))
        {
            zero.y = 0.5f;
            return zero;
        }
        if (((pv == UIWidget.Pivot.TopLeft) || (pv == UIWidget.Pivot.Top)) || (pv == UIWidget.Pivot.TopRight))
        {
            zero.y = 1f;
            return zero;
        }
        zero.y = 0f;
        return zero;
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static Color HexToColor(uint val) => 
        IntToColor((int) val);

    [DebuggerHidden, DebuggerStepThrough]
    public static int HexToDecimal(char ch)
    {
        char ch2 = ch;
        switch (ch2)
        {
            case '0':
                return 0;

            case '1':
                return 1;

            case '2':
                return 2;

            case '3':
                return 3;

            case '4':
                return 4;

            case '5':
                return 5;

            case '6':
                return 6;

            case '7':
                return 7;

            case '8':
                return 8;

            case '9':
                return 9;

            case 'A':
                break;

            case 'B':
                goto Label_00A5;

            case 'C':
                goto Label_00A8;

            case 'D':
                goto Label_00AB;

            case 'E':
                goto Label_00AE;

            case 'F':
                goto Label_00B1;

            default:
                switch (ch2)
                {
                    case 'a':
                        break;

                    case 'b':
                        goto Label_00A5;

                    case 'c':
                        goto Label_00A8;

                    case 'd':
                        goto Label_00AB;

                    case 'e':
                        goto Label_00AE;

                    case 'f':
                        goto Label_00B1;

                    default:
                        return 15;
                }
                break;
        }
        return 10;
    Label_00A5:
        return 11;
    Label_00A8:
        return 12;
    Label_00AB:
        return 13;
    Label_00AE:
        return 14;
    Label_00B1:
        return 15;
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static string IntToBinary(int val, int bits)
    {
        string str = string.Empty;
        int num = bits;
        while (num > 0)
        {
            switch (num)
            {
                case 8:
                case 0x10:
                case 0x18:
                    str = str + " ";
                    break;
            }
            str = str + (((val & (((int) 1) << --num)) == 0) ? '0' : '1');
        }
        return str;
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static Color IntToColor(int val)
    {
        float num = 0.003921569f;
        Color black = Color.black;
        black.r = num * ((val >> 0x18) & 0xff);
        black.g = num * ((val >> 0x10) & 0xff);
        black.b = num * ((val >> 8) & 0xff);
        black.a = num * (val & 0xff);
        return black;
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static float Lerp(float from, float to, float factor) => 
        ((from * (1f - factor)) + (to * factor));

    public static Rect MakePixelPerfect(Rect rect)
    {
        rect.xMin = Mathf.RoundToInt(rect.xMin);
        rect.yMin = Mathf.RoundToInt(rect.yMin);
        rect.xMax = Mathf.RoundToInt(rect.xMax);
        rect.yMax = Mathf.RoundToInt(rect.yMax);
        return rect;
    }

    public static Rect MakePixelPerfect(Rect rect, int width, int height)
    {
        rect = ConvertToPixels(rect, width, height, true);
        rect.xMin = Mathf.RoundToInt(rect.xMin);
        rect.yMin = Mathf.RoundToInt(rect.yMin);
        rect.xMax = Mathf.RoundToInt(rect.xMax);
        rect.yMax = Mathf.RoundToInt(rect.yMax);
        return ConvertToTexCoords(rect, width, height);
    }

    public static void MoveRect(UIRect rect, float x, float y)
    {
        int num = Mathf.FloorToInt(x + 0.5f);
        int num2 = Mathf.FloorToInt(y + 0.5f);
        Transform cachedTransform = rect.cachedTransform;
        cachedTransform.localPosition += new Vector3((float) num, (float) num2);
        int num3 = 0;
        if (rect.leftAnchor.target != null)
        {
            num3++;
            rect.leftAnchor.absolute += num;
        }
        if (rect.rightAnchor.target != null)
        {
            num3++;
            rect.rightAnchor.absolute += num;
        }
        if (rect.bottomAnchor.target != null)
        {
            num3++;
            rect.bottomAnchor.absolute += num2;
        }
        if (rect.topAnchor.target != null)
        {
            num3++;
            rect.topAnchor.absolute += num2;
        }
        if (num3 != 0)
        {
            rect.UpdateAnchors();
        }
    }

    public static void MoveWidget(UIRect w, float x, float y)
    {
        MoveRect(w, x, y);
    }

    public static void OverlayPosition(this Transform trans, Transform target)
    {
        Camera myCam = NGUITools.FindCameraForLayer(trans.gameObject.layer);
        Camera worldCam = NGUITools.FindCameraForLayer(target.gameObject.layer);
        if ((myCam != null) && (worldCam != null))
        {
            trans.OverlayPosition(target.position, worldCam, myCam);
        }
    }

    public static void OverlayPosition(this Transform trans, Vector3 worldPos, Camera worldCam)
    {
        Camera myCam = NGUITools.FindCameraForLayer(trans.gameObject.layer);
        if (myCam != null)
        {
            trans.OverlayPosition(worldPos, worldCam, myCam);
        }
    }

    public static void OverlayPosition(this Transform trans, Vector3 worldPos, Camera worldCam, Camera myCam)
    {
        worldPos = worldCam.WorldToViewportPoint(worldPos);
        worldPos = myCam.ViewportToWorldPoint(worldPos);
        Transform parent = trans.parent;
        trans.localPosition = (parent == null) ? worldPos : parent.InverseTransformPoint(worldPos);
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static int RepeatIndex(int val, int max)
    {
        if (max < 1)
        {
            return 0;
        }
        while (val < 0)
        {
            val += max;
        }
        while (val >= max)
        {
            val -= max;
        }
        return val;
    }

    public static void ResizeWidget(UIWidget w, UIWidget.Pivot pivot, float x, float y, int minWidth, int minHeight)
    {
        ResizeWidget(w, pivot, x, y, 2, 2, 0x186a0, 0x186a0);
    }

    public static void ResizeWidget(UIWidget w, UIWidget.Pivot pivot, float x, float y, int minWidth, int minHeight, int maxWidth, int maxHeight)
    {
        if (pivot == UIWidget.Pivot.Center)
        {
            int num = Mathf.RoundToInt(x - w.width);
            int num2 = Mathf.RoundToInt(y - w.height);
            num -= num & 1;
            num2 -= num2 & 1;
            if ((num | num2) != 0)
            {
                num = num >> 1;
                num2 = num2 >> 1;
                AdjustWidget(w, (float) -num, (float) -num2, (float) num, (float) num2, minWidth, minHeight);
            }
        }
        else
        {
            Vector3 vector = new Vector3(x, y);
            vector = (Vector3) (Quaternion.Inverse(w.cachedTransform.localRotation) * vector);
            switch (pivot)
            {
                case UIWidget.Pivot.TopLeft:
                    AdjustWidget(w, vector.x, 0f, 0f, vector.y, minWidth, minHeight, maxWidth, maxHeight);
                    break;

                case UIWidget.Pivot.Top:
                    AdjustWidget(w, 0f, 0f, 0f, vector.y, minWidth, minHeight, maxWidth, maxHeight);
                    break;

                case UIWidget.Pivot.TopRight:
                    AdjustWidget(w, 0f, 0f, vector.x, vector.y, minWidth, minHeight, maxWidth, maxHeight);
                    break;

                case UIWidget.Pivot.Left:
                    AdjustWidget(w, vector.x, 0f, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
                    break;

                case UIWidget.Pivot.Right:
                    AdjustWidget(w, 0f, 0f, vector.x, 0f, minWidth, minHeight, maxWidth, maxHeight);
                    break;

                case UIWidget.Pivot.BottomLeft:
                    AdjustWidget(w, vector.x, vector.y, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
                    break;

                case UIWidget.Pivot.Bottom:
                    AdjustWidget(w, 0f, vector.y, 0f, 0f, minWidth, minHeight, maxWidth, maxHeight);
                    break;

                case UIWidget.Pivot.BottomRight:
                    AdjustWidget(w, 0f, vector.y, vector.x, 0f, minWidth, minHeight, maxWidth, maxHeight);
                    break;
            }
        }
    }

    public static float RotateTowards(float from, float to, float maxAngle)
    {
        float f = WrapAngle(to - from);
        if (Mathf.Abs(f) > maxAngle)
        {
            f = maxAngle * Mathf.Sign(f);
        }
        return (from + f);
    }

    public static Vector2 ScreenToParentPixels(Vector2 pos, Transform relativeTo)
    {
        int layer = relativeTo.gameObject.layer;
        if (relativeTo.parent != null)
        {
            relativeTo = relativeTo.parent;
        }
        Camera camera = NGUITools.FindCameraForLayer(layer);
        if (camera == null)
        {
            Debug.LogWarning("No camera found for layer " + layer);
            return pos;
        }
        Vector3 position = camera.ScreenToWorldPoint((Vector3) pos);
        return ((relativeTo == null) ? position : relativeTo.InverseTransformPoint(position));
    }

    public static Vector2 ScreenToPixels(Vector2 pos, Transform relativeTo)
    {
        int layer = relativeTo.gameObject.layer;
        Camera camera = NGUITools.FindCameraForLayer(layer);
        if (camera == null)
        {
            Debug.LogWarning("No camera found for layer " + layer);
            return pos;
        }
        Vector3 position = camera.ScreenToWorldPoint((Vector3) pos);
        return relativeTo.InverseTransformPoint(position);
    }

    public static Vector2 SpringDampen(ref Vector2 velocity, float strength, float deltaTime)
    {
        if (deltaTime > 1f)
        {
            deltaTime = 1f;
        }
        float f = 1f - (strength * 0.001f);
        int num2 = Mathf.RoundToInt(deltaTime * 1000f);
        float num3 = Mathf.Pow(f, (float) num2);
        Vector2 vector = (Vector2) (velocity * ((num3 - 1f) / Mathf.Log(f)));
        velocity = (Vector2) (velocity * num3);
        return (Vector2) (vector * 0.06f);
    }

    public static Vector3 SpringDampen(ref Vector3 velocity, float strength, float deltaTime)
    {
        if (deltaTime > 1f)
        {
            deltaTime = 1f;
        }
        float f = 1f - (strength * 0.001f);
        int num2 = Mathf.RoundToInt(deltaTime * 1000f);
        float num3 = Mathf.Pow(f, (float) num2);
        Vector3 vector = (Vector3) (velocity * ((num3 - 1f) / Mathf.Log(f)));
        velocity = (Vector3) (velocity * num3);
        return (Vector3) (vector * 0.06f);
    }

    public static float SpringLerp(float strength, float deltaTime)
    {
        if (deltaTime > 1f)
        {
            deltaTime = 1f;
        }
        int num = Mathf.RoundToInt(deltaTime * 1000f);
        deltaTime = 0.001f * strength;
        float a = 0f;
        for (int i = 0; i < num; i++)
        {
            a = Mathf.Lerp(a, 1f, deltaTime);
        }
        return a;
    }

    public static float SpringLerp(float from, float to, float strength, float deltaTime)
    {
        if (deltaTime > 1f)
        {
            deltaTime = 1f;
        }
        int num = Mathf.RoundToInt(deltaTime * 1000f);
        deltaTime = 0.001f * strength;
        for (int i = 0; i < num; i++)
        {
            from = Mathf.Lerp(from, to, deltaTime);
        }
        return from;
    }

    public static Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime) => 
        Quaternion.Slerp(from, to, SpringLerp(strength, deltaTime));

    public static Vector2 SpringLerp(Vector2 from, Vector2 to, float strength, float deltaTime) => 
        Vector2.Lerp(from, to, SpringLerp(strength, deltaTime));

    public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime) => 
        Vector3.Lerp(from, to, SpringLerp(strength, deltaTime));

    public static Vector3 WorldToLocalPoint(Vector3 worldPos, Camera worldCam, Camera uiCam, Transform relativeTo)
    {
        worldPos = worldCam.WorldToViewportPoint(worldPos);
        worldPos = uiCam.ViewportToWorldPoint(worldPos);
        if (relativeTo == null)
        {
            return worldPos;
        }
        relativeTo = relativeTo.parent;
        return relativeTo?.InverseTransformPoint(worldPos);
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static float Wrap01(float val) => 
        (val - Mathf.FloorToInt(val));

    [DebuggerHidden, DebuggerStepThrough]
    public static float WrapAngle(float angle)
    {
        while (angle > 180f)
        {
            angle -= 360f;
        }
        while (angle < -180f)
        {
            angle += 360f;
        }
        return angle;
    }
}

