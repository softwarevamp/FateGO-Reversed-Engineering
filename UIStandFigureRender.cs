using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIStandFigureRender : UITweenRenderer
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map16;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map17;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map18;
    public static readonly float BODY_H = (((float) (BODY_SIZE_Y - 1)) / ((float) MAIN_SIZE_Y));
    public static readonly int BODY_SIZE_X = 0x3fe;
    public static readonly int BODY_SIZE_Y = 0x2fe;
    public static readonly float BODY_U = (1.5f / ((float) MAIN_SIZE_X));
    public static readonly float BODY_V = (((float) (MAIN_SIZE_Y - 1.5)) / ((float) MAIN_SIZE_Y));
    public static readonly float BODY_W = (((float) (BODY_SIZE_X - 1)) / ((float) MAIN_SIZE_X));
    [SerializeField]
    protected MeshFilter bodyFilter;
    [SerializeField]
    protected MeshRenderer bodyRenderer;
    public static readonly int BOTTOM_Y = -254;
    protected Vector2 dispOffset;
    protected Vector2 dispTop;
    protected static readonly int FACE_SIZE_X = 0xfe;
    protected static readonly int FACE_SIZE_Y = 0xfe;
    protected static readonly float FACE1_H = (((float) (FACE_SIZE_Y - 1)) / ((float) MAIN_SIZE_Y));
    protected static readonly float FACE1_SX = (1.5f / ((float) MAIN_SIZE_X));
    protected static readonly float FACE1_SY = (((float) (254.5 - FACE_SIZE_Y)) / ((float) MAIN_SIZE_Y));
    protected static readonly float FACE1_W = (((float) (FACE_SIZE_X - 1)) / ((float) MAIN_SIZE_X));
    protected static readonly float FACE2A_H;
    protected static readonly float FACE2A_SIZE;
    protected static readonly float FACE2A_SX;
    protected static readonly float FACE2A_SY;
    protected static readonly float FACE2A_W;
    protected static readonly float FACE2B_H;
    protected static readonly float FACE2B_SIZE;
    protected static readonly float FACE2B_SX;
    protected static readonly float FACE2B_SY;
    protected static readonly float FACE2B_W;
    protected static readonly float FACE2C_H;
    protected static readonly float FACE2C_SIZE;
    protected static readonly float FACE2C_SX;
    protected static readonly float FACE2C_SY;
    protected static readonly float FACE2C_W;
    [SerializeField]
    protected MeshFilter faceFilter;
    protected Rect facePositionRect;
    protected static readonly ReadOnlyCollection<Rect> faceRectTable;
    protected static readonly ReadOnlyCollection<Rect> faceRectTable2a;
    protected static readonly ReadOnlyCollection<Rect> faceRectTable2b;
    protected static readonly ReadOnlyCollection<Rect> faceRectTable2c;
    [SerializeField]
    protected MeshRenderer faceRenderer;
    protected Rect faceTextureRect;
    protected Face.Type faceType;
    protected Vector2 figureOffset;
    protected Color filterColor = Color.white;
    protected string filterName = "normal";
    protected int imageLimitCount;
    protected bool isBusyMoveAlpha;
    protected bool isFaceUse;
    protected bool isShadow;
    protected bool isTalkMask;
    public static readonly int LEFT_X = -510;
    public static readonly int MAIN_SIZE_X = 0x400;
    public static readonly int MAIN_SIZE_Y = 0x400;
    protected string moveAlphaCallbackFunc;
    protected GameObject moveAlphaCallbackObject;
    public static readonly int RIGHT_X = 0x1ff;
    protected int svtId;
    protected Texture2D[] textureList;
    public static readonly int TOP_Y = 0x1ff;

    static UIStandFigureRender()
    {
        Rect[] array = new Rect[] { new Rect(FACE1_SX, FACE1_SY, FACE1_W, FACE1_H), new Rect(FACE1_SX + 0.25f, FACE1_SY, FACE1_W, FACE1_H), new Rect(FACE1_SX + 0.5f, FACE1_SY, FACE1_W, FACE1_H), new Rect(FACE1_SX + 0.75f, FACE1_SY, FACE1_W, FACE1_H) };
        faceRectTable = Array.AsReadOnly<Rect>(array);
        FACE2A_SIZE = 256f;
        FACE2A_W = ((float) (FACE_SIZE_X - 1)) / FACE2A_SIZE;
        FACE2A_H = ((float) (FACE_SIZE_Y - 1)) / FACE2A_SIZE;
        FACE2A_SX = 1.5f / FACE2A_SIZE;
        FACE2A_SY = ((float) ((FACE2A_SIZE - 1.5) - FACE_SIZE_Y)) / FACE2A_SIZE;
        Rect[] rectArray2 = new Rect[] { new Rect(FACE2A_SX, FACE2A_SY, FACE2A_W, FACE2A_H) };
        faceRectTable2a = Array.AsReadOnly<Rect>(rectArray2);
        FACE2B_SIZE = 512f;
        FACE2B_W = ((float) (FACE_SIZE_X - 1)) / FACE2B_SIZE;
        FACE2B_H = ((float) (FACE_SIZE_Y - 1)) / FACE2B_SIZE;
        FACE2B_SX = 1.5f / FACE2B_SIZE;
        FACE2B_SY = ((float) ((FACE2B_SIZE - 1.5) - FACE_SIZE_Y)) / FACE2B_SIZE;
        Rect[] rectArray3 = new Rect[] { new Rect(FACE2B_SX, FACE2B_SY, FACE2B_W, FACE2B_H), new Rect(FACE2B_SX + 0.25f, FACE2B_SY, FACE2B_W, FACE2B_H), new Rect(FACE2B_SX, FACE2B_SY - 0.25f, FACE2B_W, FACE2B_H), new Rect(FACE2B_SX + 0.25f, FACE2B_SY - 0.25f, FACE2B_W, FACE2B_H) };
        faceRectTable2b = Array.AsReadOnly<Rect>(rectArray3);
        FACE2C_SIZE = 1024f;
        FACE2C_W = ((float) (FACE_SIZE_X - 1)) / FACE2C_SIZE;
        FACE2C_H = ((float) (FACE_SIZE_Y - 1)) / FACE2C_SIZE;
        FACE2C_SX = 1.5f / FACE2C_SIZE;
        FACE2C_SY = ((float) ((FACE2C_SIZE - 1.5) - FACE_SIZE_Y)) / FACE2C_SIZE;
        Rect[] rectArray4 = new Rect[] { new Rect(FACE2C_SX, FACE2C_SY, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.25f, FACE2C_SY, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.5f, FACE2C_SY, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.75f, FACE2C_SY, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX, FACE2C_SY - 0.25f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.25f, FACE2C_SY - 0.25f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.5f, FACE2C_SY - 0.25f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.75f, FACE2C_SY - 0.25f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX, FACE2C_SY - 0.5f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.25f, FACE2C_SY - 0.5f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.5f, FACE2C_SY - 0.5f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.75f, FACE2C_SY - 0.5f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX, FACE2C_SY - 0.75f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.25f, FACE2C_SY - 0.75f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.5f, FACE2C_SY - 0.75f, FACE2C_W, FACE2C_H), new Rect(FACE2C_SX + 0.75f, FACE2C_SY - 0.75f, FACE2C_W, FACE2C_H) };
        faceRectTable2c = Array.AsReadOnly<Rect>(rectArray4);
    }

    protected void EndMoveAlpha()
    {
        this.SetBaseColor();
        GameObject moveAlphaCallbackObject = this.moveAlphaCallbackObject;
        string moveAlphaCallbackFunc = this.moveAlphaCallbackFunc;
        this.moveAlphaCallbackObject = null;
        this.moveAlphaCallbackFunc = null;
        this.isBusyMoveAlpha = false;
        if (moveAlphaCallbackObject != null)
        {
            moveAlphaCallbackObject.SendMessage(moveAlphaCallbackFunc);
        }
    }

    public static string[] GetAssetNameList(string imageName)
    {
        string name = !imageName.StartsWith("CharaFigure/") ? ("CharaFigure/" + imageName) : imageName;
        if (!AssetManager.isExistAssetStorage(name))
        {
            name = "CharaFigure/1000000";
        }
        return new string[] { name };
    }

    public static string[] GetAssetNameList(int svtId, int imageLimitCount)
    {
        string name = "CharaFigure/" + svtId + imageLimitCount;
        if (!AssetManager.isExistAssetStorage(name))
        {
            name = "CharaFigure/1000000";
        }
        return new string[] { name };
    }

    public Vector2 GetBodySize() => 
        new Vector2((float) BODY_SIZE_X, (float) BODY_SIZE_Y);

    public Vector2 GetCenterOffset() => 
        new Vector2(0f, (float) (-ManagerConfig.HEIGHT / 2));

    public static Vector2 GetCharacterOffset(int svtId, int imageLimitCount)
    {
        ServantScriptMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantScriptMaster>(DataNameKind.Kind.SERVANT_SCRIPT);
        ServantScriptEntity entity = master.getEntityFromId<ServantScriptEntity>(svtId, imageLimitCount);
        if (entity == null)
        {
            entity = master.getEntityFromId<ServantScriptEntity>(1, 0);
        }
        return new Vector2((float) entity.offsetX, (float) (entity.offsetY + 1));
    }

    public static Vector2 GetCharacterOffsetMyroom(int svtId, int imageLimitCount)
    {
        ServantScriptMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantScriptMaster>(DataNameKind.Kind.SERVANT_SCRIPT);
        ServantScriptEntity entity = master.getEntityFromId<ServantScriptEntity>(svtId, imageLimitCount);
        if (entity == null)
        {
            entity = master.getEntityFromId<ServantScriptEntity>(1, 0);
        }
        return new Vector2((float) entity.offsetXMyroom, (float) (entity.offsetYMyroom + 1));
    }

    public static int GetImageLimitCount(string imageName)
    {
        if (imageName.StartsWith("CharaFigure/"))
        {
            return int.Parse(imageName.Substring(imageName.Length - 1));
        }
        return int.Parse(imageName.Substring(imageName.Length - 1));
    }

    public static int GetServantId(string imageName)
    {
        if (imageName.StartsWith("CharaFigure/"))
        {
            return int.Parse(imageName.Substring(12, imageName.Length - 13));
        }
        return int.Parse(imageName.Substring(0, imageName.Length - 1));
    }

    public bool IsBusyMoveAlpha() => 
        this.isBusyMoveAlpha;

    public void MoveAlpha(float duration, float alpha, GameObject callbackObject = null, string callbackFunc = null)
    {
        this.isBusyMoveAlpha = true;
        this.moveAlphaCallbackObject = callbackObject;
        this.moveAlphaCallbackFunc = callbackFunc;
        if (duration > 0f)
        {
            Color color = base.color;
            color.a = alpha;
            TweenRendererColor color2 = TweenRendererColor.Begin(base.gameObject, duration, color);
            if (color2 != null)
            {
                color2.eventReceiver = base.gameObject;
                color2.callWhenFinished = "EndMoveAlpha";
                return;
            }
        }
        this.SetAlpha(alpha);
        this.EndMoveAlpha();
    }

    public void RecoverSharder()
    {
        string filterName = this.filterName;
        if (filterName != null)
        {
            int num;
            if (<>f__switch$map18 == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(2) {
                    { 
                        "silhouette",
                        0
                    },
                    { 
                        "normal",
                        1
                    }
                };
                <>f__switch$map18 = dictionary;
            }
            if (<>f__switch$map18.TryGetValue(filterName, out num))
            {
                if (num == 0)
                {
                    this.SetSharder("Custom/Sprite-ScriptActionFigureSilhouette");
                    return;
                }
                if (num == 1)
                {
                }
            }
        }
        this.SetSharder("Custom/Sprite-ScriptActionFigureNormal");
    }

    public void ReleaseCharacter()
    {
        this.SetActive(false);
        this.textureList = null;
        if (this.bodyRenderer.material != null)
        {
            UnityEngine.Object.Destroy(this.bodyRenderer.material);
            this.bodyRenderer.material = null;
        }
        if (this.faceRenderer.material != null)
        {
            UnityEngine.Object.Destroy(this.faceRenderer.material);
            this.faceRenderer.material = null;
        }
        if (this.bodyFilter.mesh != null)
        {
            UnityEngine.Object.Destroy(this.bodyFilter.mesh);
            this.bodyFilter.mesh = null;
        }
        if (this.faceFilter.mesh != null)
        {
            UnityEngine.Object.Destroy(this.faceFilter.mesh);
            this.faceFilter.mesh = null;
        }
    }

    public void SetActive(bool isActive)
    {
        this.bodyRenderer.gameObject.SetActive(isActive);
        this.faceRenderer.gameObject.SetActive(isActive);
    }

    public void SetAlpha(float alpha)
    {
        Color c = base.color;
        c.a = alpha;
        this.SetTweenColor(c);
    }

    public void SetBaseColor()
    {
        Color c = !this.isShadow ? Color.white : new Color(0.1f, 0.1f, 0.1f, 0f);
        if (this.isTalkMask)
        {
            c = (Color) (c * 0.5f);
        }
        c.a = base.color.a;
        this.SetTweenColor(c);
    }

    public void SetCharacter(int svtId, int imageLimitCount, Face.Type faceType, Texture2D[] textureList)
    {
        this.svtId = svtId;
        this.imageLimitCount = imageLimitCount;
        this.faceType = faceType;
        this.isTalkMask = false;
        this.isShadow = false;
        this.textureList = textureList;
        this.SetCharacterRender();
    }

    protected void SetCharacterRender()
    {
        string filterName;
        Dictionary<string, int> dictionary;
        int num;
        ServantScriptMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantScriptMaster>(DataNameKind.Kind.SERVANT_SCRIPT);
        ServantScriptEntity entity = master.getEntityFromId<ServantScriptEntity>(this.svtId, this.imageLimitCount);
        if (entity == null)
        {
            entity = master.getEntityFromId<ServantScriptEntity>(1, 0);
        }
        Vector2 vector = new Vector2((float) entity.faceX, (float) entity.faceY);
        if ((entity.faceX != 0x300) || (entity.faceY != 0x300))
        {
            this.isFaceUse = true;
        }
        this.facePositionRect = new Rect(LEFT_X + vector.x, (TOP_Y - FACE_SIZE_Y) - vector.y, (float) FACE_SIZE_X, (float) FACE_SIZE_Y);
        this.faceTextureRect = new Rect((vector.x + 1.5f) / ((float) MAIN_SIZE_X), (((MAIN_SIZE_Y - FACE_SIZE_Y) - vector.y) - 1.5f) / ((float) MAIN_SIZE_Y), FACE1_W, FACE1_H);
        this.figureOffset = new Vector2((float) entity.offsetX, (float) entity.offsetY);
        this.dispTop = new Vector2(0f, (float) -TOP_Y);
        this.dispOffset = new Vector2((float) entity.offsetX, (float) (entity.offsetY - TOP_Y));
        Material material = null;
        Material material2 = null;
        if (this.textureList[1] != null)
        {
            filterName = this.filterName;
            if (filterName != null)
            {
                if (<>f__switch$map16 == null)
                {
                    dictionary = new Dictionary<string, int>(2) {
                        { 
                            "silhouette",
                            0
                        },
                        { 
                            "normal",
                            1
                        }
                    };
                    <>f__switch$map16 = dictionary;
                }
                if (<>f__switch$map16.TryGetValue(filterName, out num))
                {
                    if (num == 0)
                    {
                        material = new Material(Shader.Find("Custom/Sprite-ScriptActionFigureSilhouette2"));
                        material2 = new Material(Shader.Find("Custom/Sprite-ScriptActionFigureSilhouette2"));
                        goto Label_02C5;
                    }
                    if (num == 1)
                    {
                    }
                }
            }
            material = new Material(Shader.Find("Custom/Sprite-ScriptActionFigureNormal2"));
            material2 = new Material(Shader.Find("Custom/Sprite-ScriptActionFigureNormal2"));
        }
        else
        {
            filterName = this.filterName;
            if (filterName != null)
            {
                if (<>f__switch$map17 == null)
                {
                    dictionary = new Dictionary<string, int>(2) {
                        { 
                            "silhouette",
                            0
                        },
                        { 
                            "normal",
                            1
                        }
                    };
                    <>f__switch$map17 = dictionary;
                }
                if (<>f__switch$map17.TryGetValue(filterName, out num))
                {
                    if (num == 0)
                    {
                        material = new Material(Shader.Find("Custom/Sprite-ScriptActionFigureSilhouette"));
                        material2 = new Material(Shader.Find("Custom/Sprite-ScriptActionFigureSilhouette"));
                        goto Label_02C5;
                    }
                    if (num == 1)
                    {
                    }
                }
            }
            material = new Material(Shader.Find("Custom/Sprite-ScriptActionFigureNormal"));
            material2 = new Material(Shader.Find("Custom/Sprite-ScriptActionFigureNormal"));
        }
    Label_02C5:
        material.SetTexture("_MainTex", this.textureList[0]);
        material.SetTexture("_SubTex", this.textureList[1]);
        if (this.textureList.Length > 2)
        {
            material2.SetTexture("_MainTex", this.textureList[2]);
            material2.SetTexture("_SubTex", this.textureList[3]);
        }
        this.bodyRenderer.material = material;
        this.faceRenderer.material = material2;
        Mesh mesh = new Mesh();
        Mesh mesh2 = new Mesh();
        Vector3[] vectorArray = new Vector3[] { new Vector3((float) LEFT_X, (float) TOP_Y, 0f), new Vector3((float) RIGHT_X, (float) TOP_Y, 0f), new Vector3((float) LEFT_X, (float) BOTTOM_Y, 0f), new Vector3((float) RIGHT_X, (float) BOTTOM_Y, 0f), new Vector3(this.facePositionRect.xMin, this.facePositionRect.yMax, 0f), new Vector3(this.facePositionRect.xMax, this.facePositionRect.yMax, 0f), new Vector3(this.facePositionRect.xMin, this.facePositionRect.yMin, 0f), new Vector3(this.facePositionRect.xMax, this.facePositionRect.yMin, 0f), new Vector3(this.facePositionRect.xMin, this.facePositionRect.yMax, 0f), new Vector3(this.facePositionRect.xMax, this.facePositionRect.yMax, 0f), new Vector3(this.facePositionRect.xMin, this.facePositionRect.yMin, 0f), new Vector3(this.facePositionRect.xMax, this.facePositionRect.yMin, 0f) };
        mesh.vertices = vectorArray;
        mesh2.vertices = vectorArray;
        mesh.normals = new Vector3[] { -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward };
        mesh.uv = new Vector2[] { new Vector2(BODY_U, BODY_V), new Vector2(BODY_U + BODY_W, BODY_V), new Vector2(BODY_U, BODY_V - BODY_H), new Vector2(BODY_U + BODY_W, BODY_V - BODY_H), new Vector2(this.faceTextureRect.xMin, this.faceTextureRect.yMax), new Vector2(this.faceTextureRect.xMax, this.faceTextureRect.yMax), new Vector2(this.faceTextureRect.xMin, this.faceTextureRect.yMin), new Vector2(this.faceTextureRect.xMax, this.faceTextureRect.yMin), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f) };
        mesh2.uv = new Vector2[] { new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0f, 0f) };
        Vector2[] vectorArray5 = new Vector2[] { new Vector2(BODY_U, BODY_V), new Vector2(BODY_U + BODY_W, BODY_V), new Vector2(BODY_U, BODY_V - BODY_H), new Vector2(BODY_U + BODY_W, BODY_V - BODY_H), new Vector2(this.faceTextureRect.xMin, this.faceTextureRect.yMax), new Vector2(this.faceTextureRect.xMax, this.faceTextureRect.yMax), new Vector2(this.faceTextureRect.xMin, this.faceTextureRect.yMin), new Vector2(this.faceTextureRect.xMax, this.faceTextureRect.yMin), new Vector2(this.faceTextureRect.xMin, this.faceTextureRect.yMax), new Vector2(this.faceTextureRect.xMax, this.faceTextureRect.yMax), new Vector2(this.faceTextureRect.xMin, this.faceTextureRect.yMin), new Vector2(this.faceTextureRect.xMax, this.faceTextureRect.yMin) };
        mesh.uv2 = vectorArray5;
        mesh2.uv2 = vectorArray5;
        this.bodyFilter.mesh = mesh;
        this.faceFilter.mesh = mesh2;
        this.isBusyMoveAlpha = false;
        this.SetBaseColor();
        this.SetFilterColor(this.filterColor);
        this.SetFace(this.faceType);
        this.SetActive(true);
    }

    public void SetDepth(float d)
    {
        Vector3 localPosition = base.transform.localPosition;
        localPosition.z = -d;
        base.transform.localPosition = localPosition;
    }

    public void SetFace(Face.Type faceType)
    {
        this.faceType = faceType;
        if (this.textureList != null)
        {
            int num = !this.isFaceUse ? 0 : ((int) faceType);
            Mesh mesh = this.bodyFilter.mesh;
            Mesh mesh2 = this.faceFilter.mesh;
            Vector2[] uv = this.bodyFilter.mesh.uv;
            Vector2[] vectorArray2 = this.faceFilter.mesh.uv;
            if ((num <= 0) || ((num > 4) && (this.textureList.Length <= 2)))
            {
                mesh.triangles = new int[] { 
                    0, 1, 4, 1, 5, 4, 1, 3, 5, 3, 7, 5, 3, 2, 7, 2,
                    6, 7, 2, 0, 6, 0, 4, 6, 4, 6, 5, 5, 6, 7
                };
                mesh2.triangles = null;
            }
            else if (num <= 4)
            {
                mesh.triangles = new int[] { 
                    0, 1, 4, 1, 5, 4, 1, 3, 5, 3, 7, 5, 3, 2, 7, 2,
                    6, 7, 2, 0, 6, 0, 4, 6, 8, 10, 9, 9, 10, 11
                };
                mesh2.triangles = null;
                Rect rect = faceRectTable[num - 1];
                uv[8] = new Vector2(rect.xMin, rect.yMax);
                uv[9] = new Vector2(rect.xMax, rect.yMax);
                uv[10] = new Vector2(rect.xMin, rect.yMin);
                uv[11] = new Vector2(rect.xMax, rect.yMin);
            }
            else
            {
                mesh.triangles = new int[] { 
                    0, 1, 4, 1, 5, 4, 1, 3, 5, 3, 7, 5, 3, 2, 7, 2,
                    6, 7, 2, 0, 6, 0, 4, 6
                };
                mesh2.triangles = new int[] { 8, 10, 9, 9, 10, 11 };
                if (this.textureList[2].height == 0x100)
                {
                    Rect rect2 = faceRectTable2a[0];
                    vectorArray2[8] = new Vector2(rect2.xMin, rect2.yMax);
                    vectorArray2[9] = new Vector2(rect2.xMax, rect2.yMax);
                    vectorArray2[10] = new Vector2(rect2.xMin, rect2.yMin);
                    vectorArray2[11] = new Vector2(rect2.xMax, rect2.yMin);
                }
                else if (this.textureList[2].height == 0x200)
                {
                    Rect rect3 = faceRectTable2b[(num > 8) ? 8 : (num - 5)];
                    vectorArray2[8] = new Vector2(rect3.xMin, rect3.yMax);
                    vectorArray2[9] = new Vector2(rect3.xMax, rect3.yMax);
                    vectorArray2[10] = new Vector2(rect3.xMin, rect3.yMin);
                    vectorArray2[11] = new Vector2(rect3.xMax, rect3.yMin);
                }
                else
                {
                    Rect rect4 = faceRectTable2c[(num > 20) ? 20 : (num - 5)];
                    vectorArray2[8] = new Vector2(rect4.xMin, rect4.yMax);
                    vectorArray2[9] = new Vector2(rect4.xMax, rect4.yMax);
                    vectorArray2[10] = new Vector2(rect4.xMin, rect4.yMin);
                    vectorArray2[11] = new Vector2(rect4.xMax, rect4.yMin);
                }
            }
            mesh.uv = uv;
            mesh2.uv = vectorArray2;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }

    public void SetFilter(string filterName, Color filterColor)
    {
        this.filterName = filterName;
        this.RecoverSharder();
        this.SetFilterColor(filterColor);
    }

    public void SetFilterColor(Color c)
    {
        this.filterColor = c;
        if (this.textureList != null)
        {
            this.bodyRenderer.material.SetColor("_FilterColor", c);
            this.faceRenderer.material.SetColor("_FilterColor", c);
        }
    }

    public void SetGradation(float g)
    {
        if (this.bodyRenderer.material.HasProperty("_Gradation"))
        {
            this.bodyRenderer.material.SetFloat("_Gradation", g);
            this.faceRenderer.material.SetFloat("_Gradation", g);
        }
    }

    public void SetLayer(int layer)
    {
        if (base.gameObject.layer != layer)
        {
            this.SetLayer(base.transform, layer);
        }
    }

    protected void SetLayer(Transform tf, int layer)
    {
        tf.gameObject.layer = layer;
        IEnumerator enumerator = tf.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                this.SetLayer(current, layer);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
    }

    public void SetShadow(bool isShadow)
    {
        this.isShadow = isShadow;
        this.SetBaseColor();
    }

    public void SetSharder(string shaderName)
    {
        if (this.textureList != null)
        {
            Material material = this.bodyRenderer.material;
            Material material2 = this.faceRenderer.material;
            if (material != null)
            {
                Shader shader = Shader.Find((this.textureList[1] == null) ? shaderName : (shaderName + "2"));
                material.shader = shader;
                material2.shader = shader;
            }
        }
    }

    public void SetTalkMask(bool isMask)
    {
        this.isTalkMask = isMask;
        this.SetBaseColor();
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        if (this.textureList != null)
        {
            this.bodyRenderer.material.SetColor("_Color", c);
            this.faceRenderer.material.SetColor("_Color", c);
            if (c.a > 0f)
            {
                this.bodyRenderer.enabled = true;
                this.faceRenderer.enabled = true;
                return;
            }
        }
        this.bodyRenderer.enabled = false;
        this.faceRenderer.enabled = false;
    }

    public void SetVolume(float v)
    {
        if (this.bodyRenderer.material.HasProperty("_Volume"))
        {
            this.bodyRenderer.material.SetFloat("_Volume", v);
            this.faceRenderer.material.SetFloat("_Volume", v);
        }
    }

    public void SetWipeTexture(Texture texture)
    {
        this.bodyRenderer.material.SetTexture("_WipeTex", texture);
        this.faceRenderer.material.SetTexture("_WipeTex", texture);
        if (this.bodyRenderer.material.HasProperty("_WipeX"))
        {
            float num = -this.figureOffset.x / ((float) MAIN_SIZE_X);
            this.bodyRenderer.material.SetFloat("_WipeX", num);
            this.faceRenderer.material.SetFloat("_WipeX", num);
        }
        if (this.bodyRenderer.material.HasProperty("_WipeY"))
        {
            float num2 = ((((float) (Screen.height - BODY_SIZE_Y)) / 2f) - this.figureOffset.y) / ((float) MAIN_SIZE_Y);
            this.bodyRenderer.material.SetFloat("_WipeY", num2);
            this.faceRenderer.material.SetFloat("_WipeY", num2);
        }
    }
}

