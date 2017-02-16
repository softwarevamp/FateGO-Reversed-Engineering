using System;
using UnityEngine;

public abstract class UIBasicSprite : UIWidget
{
    public AdvancedType bottomType = AdvancedType.Sliced;
    public AdvancedType centerType = AdvancedType.Sliced;
    public AdvancedType leftType = AdvancedType.Sliced;
    [HideInInspector, SerializeField, Range(0f, 1f)]
    protected float mFillAmount = 1f;
    [SerializeField, HideInInspector]
    protected FillDirection mFillDirection = FillDirection.Radial360;
    [HideInInspector, SerializeField]
    protected Flip mFlip;
    [NonSerialized]
    private Rect mInnerUV = new Rect();
    [SerializeField, HideInInspector]
    protected bool mInvert;
    [NonSerialized]
    private Rect mOuterUV = new Rect();
    protected static Vector2[] mTempPos = new Vector2[4];
    protected static Vector2[] mTempUVs = new Vector2[4];
    [SerializeField, HideInInspector]
    protected Type mType;
    public AdvancedType rightType = AdvancedType.Sliced;
    public AdvancedType topType = AdvancedType.Sliced;

    protected UIBasicSprite()
    {
    }

    private void AdvancedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            Vector4 vector = (Vector4) (this.border * this.pixelSize);
            if (((vector.x == 0f) && (vector.y == 0f)) && ((vector.z == 0f) && (vector.w == 0f)))
            {
                this.SimpleFill(verts, uvs, cols);
            }
            else
            {
                Color32 drawingColor = this.drawingColor;
                Vector4 drawingDimensions = this.drawingDimensions;
                Vector2 vector3 = new Vector2(this.mInnerUV.width * mainTexture.width, this.mInnerUV.height * mainTexture.height);
                vector3 = (Vector2) (vector3 * this.pixelSize);
                if (vector3.x < 1f)
                {
                    vector3.x = 1f;
                }
                if (vector3.y < 1f)
                {
                    vector3.y = 1f;
                }
                mTempPos[0].x = drawingDimensions.x;
                mTempPos[0].y = drawingDimensions.y;
                mTempPos[3].x = drawingDimensions.z;
                mTempPos[3].y = drawingDimensions.w;
                if ((this.mFlip == Flip.Horizontally) || (this.mFlip == Flip.Both))
                {
                    mTempPos[1].x = mTempPos[0].x + vector.z;
                    mTempPos[2].x = mTempPos[3].x - vector.x;
                    mTempUVs[3].x = this.mOuterUV.xMin;
                    mTempUVs[2].x = this.mInnerUV.xMin;
                    mTempUVs[1].x = this.mInnerUV.xMax;
                    mTempUVs[0].x = this.mOuterUV.xMax;
                }
                else
                {
                    mTempPos[1].x = mTempPos[0].x + vector.x;
                    mTempPos[2].x = mTempPos[3].x - vector.z;
                    mTempUVs[0].x = this.mOuterUV.xMin;
                    mTempUVs[1].x = this.mInnerUV.xMin;
                    mTempUVs[2].x = this.mInnerUV.xMax;
                    mTempUVs[3].x = this.mOuterUV.xMax;
                }
                if ((this.mFlip == Flip.Vertically) || (this.mFlip == Flip.Both))
                {
                    mTempPos[1].y = mTempPos[0].y + vector.w;
                    mTempPos[2].y = mTempPos[3].y - vector.y;
                    mTempUVs[3].y = this.mOuterUV.yMin;
                    mTempUVs[2].y = this.mInnerUV.yMin;
                    mTempUVs[1].y = this.mInnerUV.yMax;
                    mTempUVs[0].y = this.mOuterUV.yMax;
                }
                else
                {
                    mTempPos[1].y = mTempPos[0].y + vector.y;
                    mTempPos[2].y = mTempPos[3].y - vector.w;
                    mTempUVs[0].y = this.mOuterUV.yMin;
                    mTempUVs[1].y = this.mInnerUV.yMin;
                    mTempUVs[2].y = this.mInnerUV.yMax;
                    mTempUVs[3].y = this.mOuterUV.yMax;
                }
                for (int i = 0; i < 3; i++)
                {
                    int index = i + 1;
                    for (int j = 0; j < 3; j++)
                    {
                        if (((this.centerType != AdvancedType.Invisible) || (i != 1)) || (j != 1))
                        {
                            int num4 = j + 1;
                            if ((i == 1) && (j == 1))
                            {
                                if (this.centerType == AdvancedType.Tiled)
                                {
                                    float x = mTempPos[i].x;
                                    float num6 = mTempPos[index].x;
                                    float y = mTempPos[j].y;
                                    float num8 = mTempPos[num4].y;
                                    float a = mTempUVs[i].x;
                                    float num10 = mTempUVs[j].y;
                                    for (float k = y; k < num8; k += vector3.y)
                                    {
                                        float num12 = x;
                                        float b = mTempUVs[num4].y;
                                        float num14 = k + vector3.y;
                                        if (num14 > num8)
                                        {
                                            b = Mathf.Lerp(num10, b, (num8 - k) / vector3.y);
                                            num14 = num8;
                                        }
                                        while (num12 < num6)
                                        {
                                            float num15 = num12 + vector3.x;
                                            float num16 = mTempUVs[index].x;
                                            if (num15 > num6)
                                            {
                                                num16 = Mathf.Lerp(a, num16, (num6 - num12) / vector3.x);
                                                num15 = num6;
                                            }
                                            Fill(verts, uvs, cols, num12, num15, k, num14, a, num16, num10, b, (Color) drawingColor);
                                            num12 += vector3.x;
                                        }
                                    }
                                }
                                else if (this.centerType == AdvancedType.Sliced)
                                {
                                    Fill(verts, uvs, cols, mTempPos[i].x, mTempPos[index].x, mTempPos[j].y, mTempPos[num4].y, mTempUVs[i].x, mTempUVs[index].x, mTempUVs[j].y, mTempUVs[num4].y, (Color) drawingColor);
                                }
                            }
                            else if (i == 1)
                            {
                                if (((j == 0) && (this.bottomType == AdvancedType.Tiled)) || ((j == 2) && (this.topType == AdvancedType.Tiled)))
                                {
                                    float num17 = mTempPos[i].x;
                                    float num18 = mTempPos[index].x;
                                    float num19 = mTempPos[j].y;
                                    float num20 = mTempPos[num4].y;
                                    float num21 = mTempUVs[i].x;
                                    float num22 = mTempUVs[j].y;
                                    float num23 = mTempUVs[num4].y;
                                    for (float m = num17; m < num18; m += vector3.x)
                                    {
                                        float num25 = m + vector3.x;
                                        float num26 = mTempUVs[index].x;
                                        if (num25 > num18)
                                        {
                                            num26 = Mathf.Lerp(num21, num26, (num18 - m) / vector3.x);
                                            num25 = num18;
                                        }
                                        Fill(verts, uvs, cols, m, num25, num19, num20, num21, num26, num22, num23, (Color) drawingColor);
                                    }
                                }
                                else if (((j == 0) && (this.bottomType != AdvancedType.Invisible)) || ((j == 2) && (this.topType != AdvancedType.Invisible)))
                                {
                                    Fill(verts, uvs, cols, mTempPos[i].x, mTempPos[index].x, mTempPos[j].y, mTempPos[num4].y, mTempUVs[i].x, mTempUVs[index].x, mTempUVs[j].y, mTempUVs[num4].y, (Color) drawingColor);
                                }
                            }
                            else if (j == 1)
                            {
                                if (((i == 0) && (this.leftType == AdvancedType.Tiled)) || ((i == 2) && (this.rightType == AdvancedType.Tiled)))
                                {
                                    float num27 = mTempPos[i].x;
                                    float num28 = mTempPos[index].x;
                                    float num29 = mTempPos[j].y;
                                    float num30 = mTempPos[num4].y;
                                    float num31 = mTempUVs[i].x;
                                    float num32 = mTempUVs[index].x;
                                    float num33 = mTempUVs[j].y;
                                    for (float n = num29; n < num30; n += vector3.y)
                                    {
                                        float num35 = mTempUVs[num4].y;
                                        float num36 = n + vector3.y;
                                        if (num36 > num30)
                                        {
                                            num35 = Mathf.Lerp(num33, num35, (num30 - n) / vector3.y);
                                            num36 = num30;
                                        }
                                        Fill(verts, uvs, cols, num27, num28, n, num36, num31, num32, num33, num35, (Color) drawingColor);
                                    }
                                }
                                else if (((i == 0) && (this.leftType != AdvancedType.Invisible)) || ((i == 2) && (this.rightType != AdvancedType.Invisible)))
                                {
                                    Fill(verts, uvs, cols, mTempPos[i].x, mTempPos[index].x, mTempPos[j].y, mTempPos[num4].y, mTempUVs[i].x, mTempUVs[index].x, mTempUVs[j].y, mTempUVs[num4].y, (Color) drawingColor);
                                }
                            }
                            else if ((((j == 0) && (this.bottomType != AdvancedType.Invisible)) || ((j == 2) && (this.topType != AdvancedType.Invisible))) || (((i == 0) && (this.leftType != AdvancedType.Invisible)) || ((i == 2) && (this.rightType != AdvancedType.Invisible))))
                            {
                                Fill(verts, uvs, cols, mTempPos[i].x, mTempPos[index].x, mTempPos[j].y, mTempPos[num4].y, mTempUVs[i].x, mTempUVs[index].x, mTempUVs[j].y, mTempUVs[num4].y, (Color) drawingColor);
                            }
                        }
                    }
                }
            }
        }
    }

    protected void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, Rect outer, Rect inner)
    {
        this.mOuterUV = outer;
        this.mInnerUV = inner;
        switch (this.type)
        {
            case Type.Simple:
                this.SimpleFill(verts, uvs, cols);
                break;

            case Type.Sliced:
                this.SlicedFill(verts, uvs, cols);
                break;

            case Type.Tiled:
                this.TiledFill(verts, uvs, cols);
                break;

            case Type.Filled:
                this.FilledFill(verts, uvs, cols);
                break;

            case Type.Advanced:
                this.AdvancedFill(verts, uvs, cols);
                break;
        }
    }

    private static void Fill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, float v0x, float v1x, float v0y, float v1y, float u0x, float u1x, float u0y, float u1y, Color col)
    {
        verts.Add(new Vector3(v0x, v0y));
        verts.Add(new Vector3(v0x, v1y));
        verts.Add(new Vector3(v1x, v1y));
        verts.Add(new Vector3(v1x, v0y));
        uvs.Add(new Vector2(u0x, u0y));
        uvs.Add(new Vector2(u0x, u1y));
        uvs.Add(new Vector2(u1x, u1y));
        uvs.Add(new Vector2(u1x, u0y));
        cols.Add(col);
        cols.Add(col);
        cols.Add(col);
        cols.Add(col);
    }

    private unsafe void FilledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        if (this.mFillAmount >= 0.001f)
        {
            Vector4 drawingDimensions = this.drawingDimensions;
            Vector4 drawingUVs = this.drawingUVs;
            Color32 drawingColor = this.drawingColor;
            if ((this.mFillDirection == FillDirection.Horizontal) || (this.mFillDirection == FillDirection.Vertical))
            {
                if (this.mFillDirection == FillDirection.Horizontal)
                {
                    float num = (drawingUVs.z - drawingUVs.x) * this.mFillAmount;
                    if (this.mInvert)
                    {
                        drawingDimensions.x = drawingDimensions.z - ((drawingDimensions.z - drawingDimensions.x) * this.mFillAmount);
                        drawingUVs.x = drawingUVs.z - num;
                    }
                    else
                    {
                        drawingDimensions.z = drawingDimensions.x + ((drawingDimensions.z - drawingDimensions.x) * this.mFillAmount);
                        drawingUVs.z = drawingUVs.x + num;
                    }
                }
                else if (this.mFillDirection == FillDirection.Vertical)
                {
                    float num2 = (drawingUVs.w - drawingUVs.y) * this.mFillAmount;
                    if (this.mInvert)
                    {
                        drawingDimensions.y = drawingDimensions.w - ((drawingDimensions.w - drawingDimensions.y) * this.mFillAmount);
                        drawingUVs.y = drawingUVs.w - num2;
                    }
                    else
                    {
                        drawingDimensions.w = drawingDimensions.y + ((drawingDimensions.w - drawingDimensions.y) * this.mFillAmount);
                        drawingUVs.w = drawingUVs.y + num2;
                    }
                }
            }
            mTempPos[0] = new Vector2(drawingDimensions.x, drawingDimensions.y);
            mTempPos[1] = new Vector2(drawingDimensions.x, drawingDimensions.w);
            mTempPos[2] = new Vector2(drawingDimensions.z, drawingDimensions.w);
            mTempPos[3] = new Vector2(drawingDimensions.z, drawingDimensions.y);
            mTempUVs[0] = new Vector2(drawingUVs.x, drawingUVs.y);
            mTempUVs[1] = new Vector2(drawingUVs.x, drawingUVs.w);
            mTempUVs[2] = new Vector2(drawingUVs.z, drawingUVs.w);
            mTempUVs[3] = new Vector2(drawingUVs.z, drawingUVs.y);
            if (this.mFillAmount < 1f)
            {
                if (this.mFillDirection == FillDirection.Radial90)
                {
                    if (RadialCut(mTempPos, mTempUVs, this.mFillAmount, this.mInvert, 0))
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            verts.Add(*((Vector3*) &(mTempPos[j])));
                            uvs.Add(mTempUVs[j]);
                            cols.Add(drawingColor);
                        }
                    }
                    return;
                }
                if (this.mFillDirection == FillDirection.Radial180)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        float num5;
                        float num6;
                        float t = 0f;
                        float num8 = 1f;
                        if (k == 0)
                        {
                            num5 = 0f;
                            num6 = 0.5f;
                        }
                        else
                        {
                            num5 = 0.5f;
                            num6 = 1f;
                        }
                        mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num5);
                        mTempPos[1].x = mTempPos[0].x;
                        mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num6);
                        mTempPos[3].x = mTempPos[2].x;
                        mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, t);
                        mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num8);
                        mTempPos[2].y = mTempPos[1].y;
                        mTempPos[3].y = mTempPos[0].y;
                        mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, num5);
                        mTempUVs[1].x = mTempUVs[0].x;
                        mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, num6);
                        mTempUVs[3].x = mTempUVs[2].x;
                        mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, t);
                        mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, num8);
                        mTempUVs[2].y = mTempUVs[1].y;
                        mTempUVs[3].y = mTempUVs[0].y;
                        float num9 = this.mInvert ? ((this.mFillAmount * 2f) - (1 - k)) : ((this.fillAmount * 2f) - k);
                        if (RadialCut(mTempPos, mTempUVs, Mathf.Clamp01(num9), !this.mInvert, NGUIMath.RepeatIndex(k + 3, 4)))
                        {
                            for (int m = 0; m < 4; m++)
                            {
                                verts.Add(*((Vector3*) &(mTempPos[m])));
                                uvs.Add(mTempUVs[m]);
                                cols.Add(drawingColor);
                            }
                        }
                    }
                    return;
                }
                if (this.mFillDirection == FillDirection.Radial360)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        float num12;
                        float num13;
                        float num14;
                        float num15;
                        if (n < 2)
                        {
                            num12 = 0f;
                            num13 = 0.5f;
                        }
                        else
                        {
                            num12 = 0.5f;
                            num13 = 1f;
                        }
                        switch (n)
                        {
                            case 0:
                            case 3:
                                num14 = 0f;
                                num15 = 0.5f;
                                break;

                            default:
                                num14 = 0.5f;
                                num15 = 1f;
                                break;
                        }
                        mTempPos[0].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num12);
                        mTempPos[1].x = mTempPos[0].x;
                        mTempPos[2].x = Mathf.Lerp(drawingDimensions.x, drawingDimensions.z, num13);
                        mTempPos[3].x = mTempPos[2].x;
                        mTempPos[0].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num14);
                        mTempPos[1].y = Mathf.Lerp(drawingDimensions.y, drawingDimensions.w, num15);
                        mTempPos[2].y = mTempPos[1].y;
                        mTempPos[3].y = mTempPos[0].y;
                        mTempUVs[0].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, num12);
                        mTempUVs[1].x = mTempUVs[0].x;
                        mTempUVs[2].x = Mathf.Lerp(drawingUVs.x, drawingUVs.z, num13);
                        mTempUVs[3].x = mTempUVs[2].x;
                        mTempUVs[0].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, num14);
                        mTempUVs[1].y = Mathf.Lerp(drawingUVs.y, drawingUVs.w, num15);
                        mTempUVs[2].y = mTempUVs[1].y;
                        mTempUVs[3].y = mTempUVs[0].y;
                        float num16 = !this.mInvert ? ((this.mFillAmount * 4f) - (3 - NGUIMath.RepeatIndex(n + 2, 4))) : ((this.mFillAmount * 4f) - NGUIMath.RepeatIndex(n + 2, 4));
                        if (RadialCut(mTempPos, mTempUVs, Mathf.Clamp01(num16), this.mInvert, NGUIMath.RepeatIndex(n + 2, 4)))
                        {
                            for (int num17 = 0; num17 < 4; num17++)
                            {
                                verts.Add(*((Vector3*) &(mTempPos[num17])));
                                uvs.Add(mTempUVs[num17]);
                                cols.Add(drawingColor);
                            }
                        }
                    }
                    return;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                verts.Add(*((Vector3*) &(mTempPos[i])));
                uvs.Add(mTempUVs[i]);
                cols.Add(drawingColor);
            }
        }
    }

    private static bool RadialCut(Vector2[] xy, Vector2[] uv, float fill, bool invert, int corner)
    {
        if (fill < 0.001f)
        {
            return false;
        }
        if ((corner & 1) == 1)
        {
            invert = !invert;
        }
        if (invert || (fill <= 0.999f))
        {
            float f = Mathf.Clamp01(fill);
            if (invert)
            {
                f = 1f - f;
            }
            f *= 1.570796f;
            float cos = Mathf.Cos(f);
            float sin = Mathf.Sin(f);
            RadialCut(xy, cos, sin, invert, corner);
            RadialCut(uv, cos, sin, invert, corner);
        }
        return true;
    }

    private static void RadialCut(Vector2[] xy, float cos, float sin, bool invert, int corner)
    {
        int index = corner;
        int num2 = NGUIMath.RepeatIndex(corner + 1, 4);
        int num3 = NGUIMath.RepeatIndex(corner + 2, 4);
        int num4 = NGUIMath.RepeatIndex(corner + 3, 4);
        if ((corner & 1) == 1)
        {
            if (sin > cos)
            {
                cos /= sin;
                sin = 1f;
                if (invert)
                {
                    xy[num2].x = Mathf.Lerp(xy[index].x, xy[num3].x, cos);
                    xy[num3].x = xy[num2].x;
                }
            }
            else if (cos > sin)
            {
                sin /= cos;
                cos = 1f;
                if (!invert)
                {
                    xy[num3].y = Mathf.Lerp(xy[index].y, xy[num3].y, sin);
                    xy[num4].y = xy[num3].y;
                }
            }
            else
            {
                cos = 1f;
                sin = 1f;
            }
            if (!invert)
            {
                xy[num4].x = Mathf.Lerp(xy[index].x, xy[num3].x, cos);
            }
            else
            {
                xy[num2].y = Mathf.Lerp(xy[index].y, xy[num3].y, sin);
            }
        }
        else
        {
            if (cos > sin)
            {
                sin /= cos;
                cos = 1f;
                if (!invert)
                {
                    xy[num2].y = Mathf.Lerp(xy[index].y, xy[num3].y, sin);
                    xy[num3].y = xy[num2].y;
                }
            }
            else if (sin > cos)
            {
                cos /= sin;
                sin = 1f;
                if (invert)
                {
                    xy[num3].x = Mathf.Lerp(xy[index].x, xy[num3].x, cos);
                    xy[num4].x = xy[num3].x;
                }
            }
            else
            {
                cos = 1f;
                sin = 1f;
            }
            if (invert)
            {
                xy[num4].y = Mathf.Lerp(xy[index].y, xy[num3].y, sin);
            }
            else
            {
                xy[num2].x = Mathf.Lerp(xy[index].x, xy[num3].x, cos);
            }
        }
    }

    private void SimpleFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Vector4 drawingDimensions = this.drawingDimensions;
        Vector4 drawingUVs = this.drawingUVs;
        Color32 drawingColor = this.drawingColor;
        verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.y));
        verts.Add(new Vector3(drawingDimensions.x, drawingDimensions.w));
        verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.w));
        verts.Add(new Vector3(drawingDimensions.z, drawingDimensions.y));
        uvs.Add(new Vector2(drawingUVs.x, drawingUVs.y));
        uvs.Add(new Vector2(drawingUVs.x, drawingUVs.w));
        uvs.Add(new Vector2(drawingUVs.z, drawingUVs.w));
        uvs.Add(new Vector2(drawingUVs.z, drawingUVs.y));
        cols.Add(drawingColor);
        cols.Add(drawingColor);
        cols.Add(drawingColor);
        cols.Add(drawingColor);
    }

    private void SlicedFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Vector4 vector = (Vector4) (this.border * this.pixelSize);
        if (((vector.x == 0f) && (vector.y == 0f)) && ((vector.z == 0f) && (vector.w == 0f)))
        {
            this.SimpleFill(verts, uvs, cols);
        }
        else
        {
            Color32 drawingColor = this.drawingColor;
            Vector4 drawingDimensions = this.drawingDimensions;
            mTempPos[0].x = drawingDimensions.x;
            mTempPos[0].y = drawingDimensions.y;
            mTempPos[3].x = drawingDimensions.z;
            mTempPos[3].y = drawingDimensions.w;
            if ((this.mFlip == Flip.Horizontally) || (this.mFlip == Flip.Both))
            {
                mTempPos[1].x = mTempPos[0].x + vector.z;
                mTempPos[2].x = mTempPos[3].x - vector.x;
                mTempUVs[3].x = this.mOuterUV.xMin;
                mTempUVs[2].x = this.mInnerUV.xMin;
                mTempUVs[1].x = this.mInnerUV.xMax;
                mTempUVs[0].x = this.mOuterUV.xMax;
            }
            else
            {
                mTempPos[1].x = mTempPos[0].x + vector.x;
                mTempPos[2].x = mTempPos[3].x - vector.z;
                mTempUVs[0].x = this.mOuterUV.xMin;
                mTempUVs[1].x = this.mInnerUV.xMin;
                mTempUVs[2].x = this.mInnerUV.xMax;
                mTempUVs[3].x = this.mOuterUV.xMax;
            }
            if ((this.mFlip == Flip.Vertically) || (this.mFlip == Flip.Both))
            {
                mTempPos[1].y = mTempPos[0].y + vector.w;
                mTempPos[2].y = mTempPos[3].y - vector.y;
                mTempUVs[3].y = this.mOuterUV.yMin;
                mTempUVs[2].y = this.mInnerUV.yMin;
                mTempUVs[1].y = this.mInnerUV.yMax;
                mTempUVs[0].y = this.mOuterUV.yMax;
            }
            else
            {
                mTempPos[1].y = mTempPos[0].y + vector.y;
                mTempPos[2].y = mTempPos[3].y - vector.w;
                mTempUVs[0].y = this.mOuterUV.yMin;
                mTempUVs[1].y = this.mInnerUV.yMin;
                mTempUVs[2].y = this.mInnerUV.yMax;
                mTempUVs[3].y = this.mOuterUV.yMax;
            }
            for (int i = 0; i < 3; i++)
            {
                int index = i + 1;
                for (int j = 0; j < 3; j++)
                {
                    if (((this.centerType != AdvancedType.Invisible) || (i != 1)) || (j != 1))
                    {
                        int num4 = j + 1;
                        verts.Add(new Vector3(mTempPos[i].x, mTempPos[j].y));
                        verts.Add(new Vector3(mTempPos[i].x, mTempPos[num4].y));
                        verts.Add(new Vector3(mTempPos[index].x, mTempPos[num4].y));
                        verts.Add(new Vector3(mTempPos[index].x, mTempPos[j].y));
                        uvs.Add(new Vector2(mTempUVs[i].x, mTempUVs[j].y));
                        uvs.Add(new Vector2(mTempUVs[i].x, mTempUVs[num4].y));
                        uvs.Add(new Vector2(mTempUVs[index].x, mTempUVs[num4].y));
                        uvs.Add(new Vector2(mTempUVs[index].x, mTempUVs[j].y));
                        cols.Add(drawingColor);
                        cols.Add(drawingColor);
                        cols.Add(drawingColor);
                        cols.Add(drawingColor);
                    }
                }
            }
        }
    }

    private void TiledFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            Vector2 vector = new Vector2(this.mInnerUV.width * mainTexture.width, this.mInnerUV.height * mainTexture.height);
            vector = (Vector2) (vector * this.pixelSize);
            if (((mainTexture != null) && (vector.x >= 2f)) && (vector.y >= 2f))
            {
                Vector4 vector3;
                Color32 drawingColor = this.drawingColor;
                Vector4 drawingDimensions = this.drawingDimensions;
                if ((this.mFlip == Flip.Horizontally) || (this.mFlip == Flip.Both))
                {
                    vector3.x = this.mInnerUV.xMax;
                    vector3.z = this.mInnerUV.xMin;
                }
                else
                {
                    vector3.x = this.mInnerUV.xMin;
                    vector3.z = this.mInnerUV.xMax;
                }
                if ((this.mFlip == Flip.Vertically) || (this.mFlip == Flip.Both))
                {
                    vector3.y = this.mInnerUV.yMax;
                    vector3.w = this.mInnerUV.yMin;
                }
                else
                {
                    vector3.y = this.mInnerUV.yMin;
                    vector3.w = this.mInnerUV.yMax;
                }
                float x = drawingDimensions.x;
                float y = drawingDimensions.y;
                float num3 = vector3.x;
                float num4 = vector3.y;
                while (y < drawingDimensions.w)
                {
                    x = drawingDimensions.x;
                    float num5 = y + vector.y;
                    float w = vector3.w;
                    if (num5 > drawingDimensions.w)
                    {
                        w = Mathf.Lerp(vector3.y, vector3.w, (drawingDimensions.w - y) / vector.y);
                        num5 = drawingDimensions.w;
                    }
                    while (x < drawingDimensions.z)
                    {
                        float num7 = x + vector.x;
                        float z = vector3.z;
                        if (num7 > drawingDimensions.z)
                        {
                            z = Mathf.Lerp(vector3.x, vector3.z, (drawingDimensions.z - x) / vector.x);
                            num7 = drawingDimensions.z;
                        }
                        verts.Add(new Vector3(x, y));
                        verts.Add(new Vector3(x, num5));
                        verts.Add(new Vector3(num7, num5));
                        verts.Add(new Vector3(num7, y));
                        uvs.Add(new Vector2(num3, num4));
                        uvs.Add(new Vector2(num3, w));
                        uvs.Add(new Vector2(z, w));
                        uvs.Add(new Vector2(z, num4));
                        cols.Add(drawingColor);
                        cols.Add(drawingColor);
                        cols.Add(drawingColor);
                        cols.Add(drawingColor);
                        x += vector.x;
                    }
                    y += vector.y;
                }
            }
        }
    }

    private Color32 drawingColor
    {
        get
        {
            Color c = base.color;
            c.a = base.finalAlpha;
            if (this.premultipliedAlpha)
            {
                c = NGUITools.ApplyPMA(c);
            }
            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
            {
                c.r = Mathf.Pow(c.r, 2.2f);
                c.g = Mathf.Pow(c.g, 2.2f);
                c.b = Mathf.Pow(c.b, 2.2f);
            }
            return c;
        }
    }

    private Vector4 drawingUVs
    {
        get
        {
            switch (this.mFlip)
            {
                case Flip.Horizontally:
                    return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMin, this.mOuterUV.xMin, this.mOuterUV.yMax);

                case Flip.Vertically:
                    return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMax, this.mOuterUV.xMax, this.mOuterUV.yMin);

                case Flip.Both:
                    return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMax, this.mOuterUV.xMin, this.mOuterUV.yMin);
            }
            return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMin, this.mOuterUV.xMax, this.mOuterUV.yMax);
        }
    }

    public float fillAmount
    {
        get => 
            this.mFillAmount;
        set
        {
            float num = Mathf.Clamp01(value);
            if (this.mFillAmount != num)
            {
                this.mFillAmount = num;
                base.mChanged = true;
            }
        }
    }

    public FillDirection fillDirection
    {
        get => 
            this.mFillDirection;
        set
        {
            if (this.mFillDirection != value)
            {
                this.mFillDirection = value;
                base.mChanged = true;
            }
        }
    }

    public Flip flip
    {
        get => 
            this.mFlip;
        set
        {
            if (this.mFlip != value)
            {
                this.mFlip = value;
                this.MarkAsChanged();
            }
        }
    }

    public bool hasBorder
    {
        get
        {
            Vector4 border = this.border;
            return ((((border.x != 0f) || (border.y != 0f)) || (border.z != 0f)) || (border.w != 0f));
        }
    }

    public bool invert
    {
        get => 
            this.mInvert;
        set
        {
            if (this.mInvert != value)
            {
                this.mInvert = value;
                base.mChanged = true;
            }
        }
    }

    public override int minHeight
    {
        get
        {
            if ((this.type == Type.Sliced) || (this.type == Type.Advanced))
            {
                Vector4 vector = (Vector4) (this.border * this.pixelSize);
                int num = Mathf.RoundToInt(vector.y + vector.w);
                return Mathf.Max(base.minHeight, ((num & 1) != 1) ? num : (num + 1));
            }
            return base.minHeight;
        }
    }

    public override int minWidth
    {
        get
        {
            if ((this.type == Type.Sliced) || (this.type == Type.Advanced))
            {
                Vector4 vector = (Vector4) (this.border * this.pixelSize);
                int num = Mathf.RoundToInt(vector.x + vector.z);
                return Mathf.Max(base.minWidth, ((num & 1) != 1) ? num : (num + 1));
            }
            return base.minWidth;
        }
    }

    public virtual float pixelSize =>
        1f;

    public virtual bool premultipliedAlpha =>
        false;

    public virtual Type type
    {
        get => 
            this.mType;
        set
        {
            if (this.mType != value)
            {
                this.mType = value;
                this.MarkAsChanged();
            }
        }
    }

    public enum AdvancedType
    {
        Invisible,
        Sliced,
        Tiled
    }

    public enum FillDirection
    {
        Horizontal,
        Vertical,
        Radial90,
        Radial180,
        Radial360
    }

    public enum Flip
    {
        Nothing,
        Horizontally,
        Vertically,
        Both
    }

    public enum Type
    {
        Simple,
        Sliced,
        Tiled,
        Filled,
        Advanced
    }
}

