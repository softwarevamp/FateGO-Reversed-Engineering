using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class UIEquipGraphRender : UITweenRenderer
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$mapE;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$mapF;
    public static readonly float BODY_H = (((float) (BODY_SIZE_Y - 1)) / ((float) MAIN_SIZE_Y));
    public static readonly int BODY_SIZE_X = 510;
    public static readonly int BODY_SIZE_Y = 0x368;
    public static readonly float BODY_U = (1.5f / ((float) MAIN_SIZE_X));
    public static readonly float BODY_V = (((float) (MAIN_SIZE_Y - 1.5)) / ((float) MAIN_SIZE_Y));
    public static readonly float BODY_W = (((float) (BODY_SIZE_X - 1)) / ((float) MAIN_SIZE_X));
    [SerializeField]
    protected MeshFilter bodyFilter;
    [SerializeField]
    protected MeshRenderer bodyRenderer;
    public static readonly int BOTTOM_Y = -364;
    protected Vector2 dispOffset;
    protected Vector2 dispTop;
    protected Vector2 figureOffset;
    protected Color filterColor = Color.white;
    protected string filterName = "normal";
    protected bool isBusyMoveAlpha;
    protected bool isShadow;
    protected bool isTalkMask;
    public static readonly int LEFT_X = -254;
    public static readonly int MAIN_SIZE_X = 0x400;
    public static readonly int MAIN_SIZE_Y = 0x400;
    protected string moveAlphaCallbackFunc;
    protected GameObject moveAlphaCallbackObject;
    public static readonly int RIGHT_X = 0xff;
    protected Texture2D[] textureList;
    public static readonly int TOP_Y = 0x1ff;

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
        string name = !imageName.StartsWith("CharaGraph/") ? ("CharaGraph/" + imageName) : imageName;
        if (!AssetManager.isExistAssetStorage(name))
        {
            name = "CharaGraph/9400010";
        }
        return new string[] { name };
    }

    public Vector2 GetBodySize() => 
        new Vector2((float) BODY_SIZE_X, (float) BODY_SIZE_Y);

    public Vector2 GetCenterOffset() => 
        new Vector2(0f, (float) (-ManagerConfig.HEIGHT / 2));

    public static Vector2 GetCharacterOffset() => 
        new Vector2(0f, 0f);

    public static Vector2 GetCharacterOffsetMyroom() => 
        new Vector2(0f, 0f);

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
            if (<>f__switch$mapF == null)
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
                <>f__switch$mapF = dictionary;
            }
            if (<>f__switch$mapF.TryGetValue(filterName, out num))
            {
                if (num == 0)
                {
                    this.SetSharder("Custom/Sprite-ScriptActionEquipSilhouette");
                    return;
                }
                if (num == 1)
                {
                }
            }
        }
        this.SetSharder("Custom/Sprite-ScriptActionEquipNormal");
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
        if (this.bodyFilter.mesh != null)
        {
            UnityEngine.Object.Destroy(this.bodyFilter.mesh);
            this.bodyFilter.mesh = null;
        }
    }

    public void SetActive(bool isActive)
    {
        this.bodyRenderer.gameObject.SetActive(isActive);
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

    public void SetCharacter(int svtId, Face.Type faceType, Texture2D[] textureList)
    {
        this.isTalkMask = false;
        this.isShadow = false;
        this.textureList = textureList;
        this.SetCharacterRender();
    }

    protected void SetCharacterRender()
    {
        this.dispTop = new Vector2(0f, (float) -TOP_Y);
        this.dispOffset = new Vector2(0f, (float) -TOP_Y);
        Material material = null;
        string filterName = this.filterName;
        if (filterName != null)
        {
            int num;
            if (<>f__switch$mapE == null)
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
                <>f__switch$mapE = dictionary;
            }
            if (<>f__switch$mapE.TryGetValue(filterName, out num))
            {
                if (num == 0)
                {
                    material = new Material(Shader.Find("Custom/Sprite-ScriptActionEquipSilhouette"));
                    goto Label_00C3;
                }
                if (num == 1)
                {
                }
            }
        }
        material = new Material(Shader.Find("Custom/Sprite-ScriptActionEquipNormal"));
    Label_00C3:
        material.SetTexture("_MainTex", this.textureList[0]);
        material.SetTexture("_SubTex", this.textureList[0]);
        this.bodyRenderer.material = material;
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] { new Vector3((float) LEFT_X, (float) TOP_Y, 0f), new Vector3((float) RIGHT_X, (float) TOP_Y, 0f), new Vector3((float) LEFT_X, (float) BOTTOM_Y, 0f), new Vector3((float) RIGHT_X, (float) BOTTOM_Y, 0f) };
        mesh.normals = new Vector3[] { -Vector3.forward, -Vector3.forward, -Vector3.forward, -Vector3.forward };
        mesh.uv = new Vector2[] { new Vector2(BODY_U, BODY_V), new Vector2(BODY_U + BODY_W, BODY_V), new Vector2(BODY_U, BODY_V - BODY_H), new Vector2(BODY_U + BODY_W, BODY_V - BODY_H) };
        mesh.uv2 = new Vector2[] { new Vector2(BODY_U, BODY_V), new Vector2(BODY_U + BODY_W, BODY_V), new Vector2(BODY_U, BODY_V - BODY_H), new Vector2(BODY_U + BODY_W, BODY_V - BODY_H) };
        mesh.triangles = new int[] { 0, 1, 2, 1, 3, 2 };
        this.bodyFilter.mesh = mesh;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        this.isBusyMoveAlpha = false;
        this.SetBaseColor();
        this.SetFilterColor(this.filterColor);
        this.SetActive(true);
    }

    public void SetDepth(float d)
    {
        Vector3 localPosition = base.transform.localPosition;
        localPosition.z = -d;
        base.transform.localPosition = localPosition;
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
        }
    }

    public void SetGradation(float g)
    {
        if (this.bodyRenderer.material.HasProperty("_Gradation"))
        {
            this.bodyRenderer.material.SetFloat("_Gradation", g);
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
            if (material != null)
            {
                Shader shader = Shader.Find(shaderName);
                material.shader = shader;
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
            if (c.a > 0f)
            {
                this.bodyRenderer.enabled = true;
                return;
            }
        }
        this.bodyRenderer.enabled = false;
    }

    public void SetVolume(float v)
    {
        if (this.bodyRenderer.material.HasProperty("_Volume"))
        {
            this.bodyRenderer.material.SetFloat("_Volume", v);
        }
    }

    public void SetWipeTexture(Texture texture)
    {
        this.bodyRenderer.material.SetTexture("_WipeTex", texture);
        if (this.bodyRenderer.material.HasProperty("_WipeX"))
        {
            float num = -this.figureOffset.x / ((float) MAIN_SIZE_X);
            this.bodyRenderer.material.SetFloat("_WipeX", num);
        }
        if (this.bodyRenderer.material.HasProperty("_WipeY"))
        {
            float num2 = ((((float) (Screen.height - BODY_SIZE_Y)) / 2f) - this.figureOffset.y) / ((float) MAIN_SIZE_Y);
            this.bodyRenderer.material.SetFloat("_WipeY", num2);
        }
    }
}

