using System;
using UnityEngine;

[AddComponentMenu("NGUI/UI/NGUI Mesh Sprite"), ExecuteInEditMode]
public class UIMeshSprite : UISprite
{
    protected static int[][] meshPolygons;
    public SpriteMeshType meshType = SpriteMeshType.TopCutFace;
    protected static double[][] meshVertices;
    [NonSerialized]
    private Rect mOuterUV = new Rect();
    public const int SpriteMeshTypeCount = 4;
    protected static Vector2[][] sprMeshUv;
    protected static Vector3[][] sprMeshVert;
    protected static int[][] uvIndex;
    protected static double[][] uvPos;

    private void Initialize()
    {
        if (sprMeshVert == null)
        {
            meshVertices = new double[][] { new double[] { -1.0, -1.0, 0.0, 1.0, -1.0, 0.0, -1.0, 1.0, 0.0, 1.0, 1.0, 0.0 }, new double[] { 
                -1.0, -1.0, 0.0, 1.0, -1.0, 0.0, -1.0, 0.850214, 0.0, -0.850471, 1.0, 0.0, 0.997653, 0.849241, -0.000549, 0.848194,
                1.0, 0.0
            }, new double[] { -1.0, -1.0, 0.0, 1.0, -1.0, 0.0, -1.0, 1.0, 0.0, 1.0, 1.0, 0.0 }, new double[] { -1.0, -1.0, 0.0, 1.0, -1.0, 0.0, -1.0, 1.0, 0.0, 1.0, 1.0, 0.0 } };
            int[][] numArrayArray2 = new int[4][];
            int[] numArray1 = new int[4];
            numArray1[1] = 1;
            numArray1[2] = 3;
            numArray1[3] = -3;
            numArrayArray2[0] = numArray1;
            numArrayArray2[1] = new int[] { 2, 4, 5, -4, 0, 1, 4, -3 };
            int[] numArray3 = new int[4];
            numArray3[1] = 1;
            numArray3[2] = 3;
            numArray3[3] = -3;
            numArrayArray2[2] = numArray3;
            int[] numArray4 = new int[4];
            numArray4[1] = 1;
            numArray4[2] = 3;
            numArray4[3] = -3;
            numArrayArray2[3] = numArray4;
            meshPolygons = numArrayArray2;
            uvPos = new double[][] { new double[] { 0.0001, 0.0001, 0.9999, 0.0001, 0.9999, 0.9999, 0.0001, 0.9999 }, new double[] { 0.9999, 0.0001, 0.0001, 0.925021, 0.998727, 0.924535, 0.924013, 0.999899, 0.07485, 0.9999, 0.0001, 0.0001 }, new double[] { 0.0, 0.185, 1.0, 0.185, 1.0, 0.815, 0.0, 0.815 }, new double[] { 0.0, 0.0, 1.0, 0.0, 1.0, 1.0, 0.0, 1.0 } };
            int[][] numArrayArray4 = new int[4][];
            int[] numArray5 = new int[4];
            numArray5[1] = 1;
            numArray5[2] = 2;
            numArray5[3] = 3;
            numArrayArray4[0] = numArray5;
            numArrayArray4[1] = new int[] { 1, 2, 3, 4, 5, 0, 2, 1 };
            int[] numArray6 = new int[4];
            numArray6[1] = 1;
            numArray6[2] = 2;
            numArray6[3] = 3;
            numArrayArray4[2] = numArray6;
            int[] numArray7 = new int[4];
            numArray7[1] = 1;
            numArray7[2] = 2;
            numArray7[3] = 3;
            numArrayArray4[3] = numArray7;
            uvIndex = numArrayArray4;
            for (int i = 0; i < 4; i++)
            {
                int length = meshPolygons[i].Length;
                for (int k = 0; k < length; k++)
                {
                    if (meshPolygons[i][k] < 0)
                    {
                        meshPolygons[i][k] = (-1 * meshPolygons[i][k]) - 1;
                    }
                }
                for (int m = 0; m < (length / 2); m++)
                {
                    int num5 = meshPolygons[i][m * 2];
                    meshPolygons[i][m * 2] = meshPolygons[i][(m * 2) + 1];
                    meshPolygons[i][(m * 2) + 1] = num5;
                }
                int num6 = uvIndex[i].Length;
                for (int n = 0; n < (num6 / 2); n++)
                {
                    int num8 = uvIndex[i][n * 2];
                    uvIndex[i][n * 2] = uvIndex[i][(n * 2) + 1];
                    uvIndex[i][(n * 2) + 1] = num8;
                }
            }
            sprMeshVert = new Vector3[4][];
            sprMeshUv = new Vector2[4][];
            for (int j = 0; j < 4; j++)
            {
                double[] numArray = meshVertices[j];
                double[] numArray2 = uvPos[j];
                Vector3[] vectorArray = new Vector3[numArray.Length / 3];
                int num10 = vectorArray.Length;
                for (int num11 = 0; num11 < num10; num11++)
                {
                    vectorArray[num11] = new Vector3((float) numArray[num11 * 3], (float) numArray[(num11 * 3) + 1], (float) numArray[(num11 * 3) + 2]);
                }
                Vector2[] vectorArray2 = new Vector2[numArray2.Length / 2];
                int num12 = vectorArray2.Length;
                for (int num13 = 0; num13 < num12; num13++)
                {
                    vectorArray2[num13] = new Vector2((float) numArray2[num13 * 2], (float) numArray2[(num13 * 2) + 1]);
                }
                float x = 1000f;
                float y = 1000f;
                float num16 = 0f;
                float num17 = 0f;
                foreach (Vector3 vector in vectorArray)
                {
                    if (vector.x < x)
                    {
                        x = vector.x;
                    }
                    else if (vector.x > num16)
                    {
                        num16 = vector.x;
                    }
                    if (vector.y < y)
                    {
                        y = vector.y;
                    }
                    else if (vector.y > num17)
                    {
                        num17 = vector.y;
                    }
                }
                num16 -= x;
                num17 -= y;
                float num19 = num16;
                if (num16 < num17)
                {
                    num19 = num17;
                }
                Vector3 vector2 = new Vector3(-x, -y, 0f);
                int num20 = vectorArray.Length;
                for (int num21 = 0; num21 < num20; num21++)
                {
                    vectorArray[num21] = (Vector3) ((vectorArray[num21] + vector2) / num19);
                }
                sprMeshVert[j] = vectorArray;
                sprMeshUv[j] = vectorArray2;
            }
        }
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        Texture mainTexture = this.mainTexture;
        if (mainTexture != null)
        {
            if (base.mSprite == null)
            {
                base.mSprite = base.atlas.GetSprite(base.spriteName);
            }
            if (base.mSprite != null)
            {
                Rect rect = new Rect((float) base.mSprite.x, (float) base.mSprite.y, (float) base.mSprite.width, (float) base.mSprite.height);
                Rect rect2 = new Rect((float) (base.mSprite.x + base.mSprite.borderLeft), (float) (base.mSprite.y + base.mSprite.borderTop), (float) ((base.mSprite.width - base.mSprite.borderLeft) - base.mSprite.borderRight), (float) ((base.mSprite.height - base.mSprite.borderBottom) - base.mSprite.borderTop));
                rect = NGUIMath.ConvertToTexCoords(rect, mainTexture.width, mainTexture.height);
                rect2 = NGUIMath.ConvertToTexCoords(rect2, mainTexture.width, mainTexture.height);
                int size = verts.size;
                this.RotFill(verts, uvs, cols, rect, rect2);
                if (base.onPostFill != null)
                {
                    base.onPostFill(this, size, verts, uvs, cols);
                }
            }
        }
    }

    private void RotFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols, Rect outer, Rect inner)
    {
        this.mOuterUV = outer;
        Vector4 drawingDimensions = this.drawingDimensions;
        Vector4 drawingUVs = this.drawingUVs;
        Color32 drawingColor = this.drawingColor;
        int[] numArray = meshPolygons[(int) this.meshType];
        Vector3[] vectorArray = sprMeshVert[(int) this.meshType];
        int[] numArray2 = uvIndex[(int) this.meshType];
        Vector2[] vectorArray2 = sprMeshUv[(int) this.meshType];
        int length = numArray.Length;
        for (int i = 0; i < length; i++)
        {
            Vector3 vector3 = vectorArray[numArray[i]];
            Vector2 vector4 = vectorArray2[numArray2[i]];
            verts.Add(new Vector3(drawingDimensions.x + ((drawingDimensions.z - drawingDimensions.x) * vector3.x), drawingDimensions.y + ((drawingDimensions.w - drawingDimensions.y) * vector3.y)));
            uvs.Add(new Vector2(drawingUVs.x + ((drawingUVs.z - drawingUVs.x) * vector4.x), drawingUVs.y + ((drawingUVs.w - drawingUVs.y) * vector4.y)));
            cols.Add(drawingColor);
        }
    }

    private void Start()
    {
        base.Start();
        this.Initialize();
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
            switch (base.mFlip)
            {
                case UIBasicSprite.Flip.Horizontally:
                    return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMin, this.mOuterUV.xMin, this.mOuterUV.yMax);

                case UIBasicSprite.Flip.Vertically:
                    return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMax, this.mOuterUV.xMax, this.mOuterUV.yMin);

                case UIBasicSprite.Flip.Both:
                    return new Vector4(this.mOuterUV.xMax, this.mOuterUV.yMax, this.mOuterUV.xMin, this.mOuterUV.yMin);
            }
            return new Vector4(this.mOuterUV.xMin, this.mOuterUV.yMin, this.mOuterUV.xMax, this.mOuterUV.yMax);
        }
    }

    public enum SpriteMeshType
    {
        NormalRect,
        TopCutFace,
        ServantEquipRect,
        ServantEquipBigRect
    }
}

