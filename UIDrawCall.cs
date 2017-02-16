using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/Internal/Draw Call")]
public class UIDrawCall : MonoBehaviour
{
    [NonSerialized, HideInInspector]
    public bool alwaysOnScreen;
    private static int[] ClipArgs = null;
    private static int[] ClipRange = null;
    [NonSerialized, HideInInspector]
    public Texture2D clipTexture;
    [NonSerialized, HideInInspector]
    public BetterList<Color32> cols = new BetterList<Color32>();
    [NonSerialized, HideInInspector]
    public int depthEnd = -2147483648;
    [NonSerialized, HideInInspector]
    public int depthStart = 0x7fffffff;
    [NonSerialized]
    public bool isDirty;
    private static BetterList<UIDrawCall> mActiveList = new BetterList<UIDrawCall>();
    [NonSerialized, HideInInspector]
    public UIPanel manager;
    private const int maxIndexBufferCache = 10;
    private static List<int[]> mCache = new List<int[]>(10);
    private int mClipCount;
    private Material mDynamicMat;
    private MeshFilter mFilter;
    private static BetterList<UIDrawCall> mInactiveList = new BetterList<UIDrawCall>();
    private int[] mIndices;
    private bool mLegacyShader;
    private Material mMaterial;
    private Mesh mMesh;
    private bool mRebuildMat = true;
    private MeshRenderer mRenderer;
    private int mRenderQueue = 0xbb8;
    private Shader mShader;
    private Texture mTexture;
    [NonSerialized]
    private bool mTextureClip;
    private Transform mTrans;
    private int mTriangles;
    [NonSerialized, HideInInspector]
    public BetterList<Vector3> norms = new BetterList<Vector3>();
    public OnRenderCallback onRender;
    [NonSerialized, HideInInspector]
    public UIPanel panel;
    [NonSerialized, HideInInspector]
    public BetterList<Vector4> tans = new BetterList<Vector4>();
    [NonSerialized, HideInInspector]
    public BetterList<Vector2> uvs = new BetterList<Vector2>();
    [NonSerialized, HideInInspector]
    public BetterList<Vector3> verts = new BetterList<Vector3>();
    [NonSerialized, HideInInspector]
    public int widgetCount;

    private void Awake()
    {
        if (ClipRange == null)
        {
            ClipRange = new int[] { Shader.PropertyToID("_ClipRange0"), Shader.PropertyToID("_ClipRange1"), Shader.PropertyToID("_ClipRange2"), Shader.PropertyToID("_ClipRange4") };
        }
        if (ClipArgs == null)
        {
            ClipArgs = new int[] { Shader.PropertyToID("_ClipArgs0"), Shader.PropertyToID("_ClipArgs1"), Shader.PropertyToID("_ClipArgs2"), Shader.PropertyToID("_ClipArgs3") };
        }
    }

    public static void ClearAll()
    {
        bool isPlaying = Application.isPlaying;
        int size = mActiveList.size;
        while (size > 0)
        {
            UIDrawCall call = mActiveList[--size];
            if (call != null)
            {
                if (isPlaying)
                {
                    NGUITools.SetActive(call.gameObject, false);
                }
                else
                {
                    NGUITools.DestroyImmediate(call.gameObject);
                }
            }
        }
        mActiveList.Clear();
    }

    public static int Count(UIPanel panel)
    {
        int num = 0;
        for (int i = 0; i < mActiveList.size; i++)
        {
            if (mActiveList[i].manager == panel)
            {
                num++;
            }
        }
        return num;
    }

    private static UIDrawCall Create(string name)
    {
        if (mInactiveList.size > 0)
        {
            UIDrawCall call = mInactiveList.Pop();
            mActiveList.Add(call);
            if (name != null)
            {
                call.name = name;
            }
            NGUITools.SetActive(call.gameObject, true);
            return call;
        }
        GameObject target = new GameObject(name);
        UnityEngine.Object.DontDestroyOnLoad(target);
        UIDrawCall item = target.AddComponent<UIDrawCall>();
        mActiveList.Add(item);
        return item;
    }

    public static UIDrawCall Create(UIPanel panel, Material mat, Texture tex, Shader shader) => 
        Create(null, panel, mat, tex, shader);

    private static UIDrawCall Create(string name, UIPanel pan, Material mat, Texture tex, Shader shader)
    {
        UIDrawCall call = Create(name);
        call.gameObject.layer = pan.cachedGameObject.layer;
        call.baseMaterial = mat;
        call.mainTexture = tex;
        call.shader = shader;
        call.renderQueue = pan.startingRenderQueue;
        call.sortingOrder = pan.sortingOrder;
        call.manager = pan;
        return call;
    }

    private void CreateMaterial()
    {
        this.mTextureClip = false;
        this.mLegacyShader = false;
        this.mClipCount = this.panel.clipCount;
        string name = (this.mShader == null) ? ((this.mMaterial == null) ? "Unlit/Transparent Colored" : this.mMaterial.shader.name) : this.mShader.name;
        name = name.Replace("GUI/Text Shader", "Unlit/Text");
        if ((name.Length > 2) && (name[name.Length - 2] == ' '))
        {
            int num = name[name.Length - 1];
            if ((num > 0x30) && (num <= 0x39))
            {
                name = name.Substring(0, name.Length - 2);
            }
        }
        if (name.StartsWith("Hidden/"))
        {
            name = name.Substring(7);
        }
        name = name.Replace(" (SoftClip)", string.Empty).Replace(" (TextureClip)", string.Empty);
        if (this.panel.clipping == Clipping.TextureMask)
        {
            this.mTextureClip = true;
            this.shader = Shader.Find("Hidden/" + name + " (TextureClip)");
        }
        else if (this.mClipCount != 0)
        {
            this.shader = Shader.Find(string.Concat(new object[] { "Hidden/", name, " ", this.mClipCount }));
            if (this.shader == null)
            {
                this.shader = Shader.Find(name + " " + this.mClipCount);
            }
            if ((this.shader == null) && (this.mClipCount == 1))
            {
                this.mLegacyShader = true;
                this.shader = Shader.Find(name + " (SoftClip)");
            }
        }
        else
        {
            this.shader = Shader.Find(name);
        }
        if (this.shader == null)
        {
            this.shader = Shader.Find("Unlit/Transparent Colored");
        }
        if (this.mMaterial != null)
        {
            this.mDynamicMat = new Material(this.mMaterial);
            this.mDynamicMat.name = "[NGUI] " + this.mMaterial.name;
            this.mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
            this.mDynamicMat.CopyPropertiesFromMaterial(this.mMaterial);
            string[] shaderKeywords = this.mMaterial.shaderKeywords;
            for (int i = 0; i < shaderKeywords.Length; i++)
            {
                this.mDynamicMat.EnableKeyword(shaderKeywords[i]);
            }
            if (this.shader != null)
            {
                this.mDynamicMat.shader = this.shader;
            }
            else if (this.mClipCount != 0)
            {
                Debug.LogError(string.Concat(new object[] { name, " shader doesn't have a clipped shader version for ", this.mClipCount, " clip regions" }));
            }
        }
        else
        {
            this.mDynamicMat = new Material(this.shader);
            this.mDynamicMat.name = "[NGUI] " + this.shader.name;
            this.mDynamicMat.hideFlags = HideFlags.DontSave | HideFlags.NotEditable;
        }
    }

    public static void Destroy(UIDrawCall dc)
    {
        if (dc != null)
        {
            dc.onRender = null;
            if (Application.isPlaying)
            {
                if (mActiveList.Remove(dc))
                {
                    NGUITools.SetActive(dc.gameObject, false);
                    mInactiveList.Add(dc);
                }
            }
            else
            {
                mActiveList.Remove(dc);
                NGUITools.DestroyImmediate(dc.gameObject);
            }
        }
    }

    private int[] GenerateCachedIndexBuffer(int vertexCount, int indexCount)
    {
        int num = 0;
        int count = mCache.Count;
        while (num < count)
        {
            int[] numArray = mCache[num];
            if ((numArray != null) && (numArray.Length == indexCount))
            {
                return numArray;
            }
            num++;
        }
        int[] item = new int[indexCount];
        int num3 = 0;
        for (int i = 0; i < vertexCount; i += 4)
        {
            item[num3++] = i;
            item[num3++] = i + 1;
            item[num3++] = i + 2;
            item[num3++] = i + 2;
            item[num3++] = i + 3;
            item[num3++] = i;
        }
        if (mCache.Count > 10)
        {
            mCache.RemoveAt(0);
        }
        mCache.Add(item);
        return item;
    }

    private void OnDestroy()
    {
        NGUITools.DestroyImmediate(this.mMesh);
        this.mMesh = null;
    }

    private void OnDisable()
    {
        this.depthStart = 0x7fffffff;
        this.depthEnd = -2147483648;
        this.panel = null;
        this.manager = null;
        this.mMaterial = null;
        this.mTexture = null;
        this.clipTexture = null;
        if (this.mRenderer != null)
        {
            this.mRenderer.sharedMaterials = new Material[0];
        }
        NGUITools.DestroyImmediate(this.mDynamicMat);
        this.mDynamicMat = null;
    }

    private void OnEnable()
    {
        this.mRebuildMat = true;
    }

    private void OnWillRenderObject()
    {
        this.UpdateMaterials();
        if (this.onRender != null)
        {
            if (this.mDynamicMat == null)
            {
            }
            this.onRender(this.mMaterial);
        }
        if ((this.mDynamicMat != null) && (this.mClipCount != 0))
        {
            if (this.mTextureClip)
            {
                Vector4 drawCallClipRange = this.panel.drawCallClipRange;
                Vector2 clipSoftness = this.panel.clipSoftness;
                Vector2 vector3 = new Vector2(1000f, 1000f);
                if (clipSoftness.x > 0f)
                {
                    vector3.x = drawCallClipRange.z / clipSoftness.x;
                }
                if (clipSoftness.y > 0f)
                {
                    vector3.y = drawCallClipRange.w / clipSoftness.y;
                }
                this.mDynamicMat.SetVector(ClipRange[0], new Vector4(-drawCallClipRange.x / drawCallClipRange.z, -drawCallClipRange.y / drawCallClipRange.w, 1f / drawCallClipRange.z, 1f / drawCallClipRange.w));
                this.mDynamicMat.SetTexture("_ClipTex", this.clipTexture);
            }
            else if (!this.mLegacyShader)
            {
                UIPanel parentPanel = this.panel;
                int num = 0;
                while (parentPanel != null)
                {
                    if (parentPanel.hasClipping)
                    {
                        float angle = 0f;
                        Vector4 cr = parentPanel.drawCallClipRange;
                        if (parentPanel != this.panel)
                        {
                            Vector3 vector5 = parentPanel.cachedTransform.InverseTransformPoint(this.panel.cachedTransform.position);
                            cr.x -= vector5.x;
                            cr.y -= vector5.y;
                            Vector3 eulerAngles = this.panel.cachedTransform.rotation.eulerAngles;
                            Vector3 vector8 = parentPanel.cachedTransform.rotation.eulerAngles - eulerAngles;
                            vector8.x = NGUIMath.WrapAngle(vector8.x);
                            vector8.y = NGUIMath.WrapAngle(vector8.y);
                            vector8.z = NGUIMath.WrapAngle(vector8.z);
                            if ((Mathf.Abs(vector8.x) > 0.001f) || (Mathf.Abs(vector8.y) > 0.001f))
                            {
                                Debug.LogWarning("Panel can only be clipped properly if X and Y rotation is left at 0", this.panel);
                            }
                            angle = vector8.z;
                        }
                        this.SetClipping(num++, cr, parentPanel.clipSoftness, angle);
                    }
                    parentPanel = parentPanel.parentPanel;
                }
            }
            else
            {
                Vector2 vector9 = this.panel.clipSoftness;
                Vector4 vector10 = this.panel.drawCallClipRange;
                Vector2 vector11 = new Vector2(-vector10.x / vector10.z, -vector10.y / vector10.w);
                Vector2 vector12 = new Vector2(1f / vector10.z, 1f / vector10.w);
                Vector2 vector = new Vector2(1000f, 1000f);
                if (vector9.x > 0f)
                {
                    vector.x = vector10.z / vector9.x;
                }
                if (vector9.y > 0f)
                {
                    vector.y = vector10.w / vector9.y;
                }
                this.mDynamicMat.mainTextureOffset = vector11;
                this.mDynamicMat.mainTextureScale = vector12;
                this.mDynamicMat.SetVector("_ClipSharpness", vector);
            }
        }
    }

    private Material RebuildMaterial()
    {
        NGUITools.DestroyImmediate(this.mDynamicMat);
        this.CreateMaterial();
        this.mDynamicMat.renderQueue = this.mRenderQueue;
        if (this.mTexture != null)
        {
            this.mDynamicMat.mainTexture = this.mTexture;
        }
        if (this.mRenderer != null)
        {
            this.mRenderer.sharedMaterials = new Material[] { this.mDynamicMat };
        }
        return this.mDynamicMat;
    }

    public static void ReleaseAll()
    {
        ClearAll();
        ReleaseInactive();
    }

    public static void ReleaseInactive()
    {
        int size = mInactiveList.size;
        while (size > 0)
        {
            UIDrawCall call = mInactiveList[--size];
            if (call != null)
            {
                NGUITools.DestroyImmediate(call.gameObject);
            }
        }
        mInactiveList.Clear();
    }

    private void SetClipping(int index, Vector4 cr, Vector2 soft, float angle)
    {
        angle *= -0.01745329f;
        Vector2 vector = new Vector2(1000f, 1000f);
        if (soft.x > 0f)
        {
            vector.x = cr.z / soft.x;
        }
        if (soft.y > 0f)
        {
            vector.y = cr.w / soft.y;
        }
        if (index < ClipRange.Length)
        {
            this.mDynamicMat.SetVector(ClipRange[index], new Vector4(-cr.x / cr.z, -cr.y / cr.w, 1f / cr.z, 1f / cr.w));
            this.mDynamicMat.SetVector(ClipArgs[index], new Vector4(vector.x, vector.y, Mathf.Sin(angle), Mathf.Cos(angle)));
        }
    }

    public void UpdateGeometry(int widgetCount)
    {
        this.widgetCount = widgetCount;
        int size = this.verts.size;
        if (((size > 0) && (size == this.uvs.size)) && ((size == this.cols.size) && ((size % 4) == 0)))
        {
            if (this.mFilter == null)
            {
                this.mFilter = base.gameObject.GetComponent<MeshFilter>();
            }
            if (this.mFilter == null)
            {
                this.mFilter = base.gameObject.AddComponent<MeshFilter>();
            }
            if (this.verts.size < 0xfde8)
            {
                int indexCount = (size >> 1) * 3;
                bool flag = (this.mIndices == null) || (this.mIndices.Length != indexCount);
                if (this.mMesh == null)
                {
                    this.mMesh = new Mesh();
                    this.mMesh.hideFlags = HideFlags.DontSave;
                    this.mMesh.name = (this.mMaterial == null) ? "[NGUI] Mesh" : ("[NGUI] " + this.mMaterial.name);
                    this.mMesh.MarkDynamic();
                    flag = true;
                }
                bool flag2 = (((this.uvs.buffer.Length != this.verts.buffer.Length) || (this.cols.buffer.Length != this.verts.buffer.Length)) || ((this.norms.buffer != null) && (this.norms.buffer.Length != this.verts.buffer.Length))) || ((this.tans.buffer != null) && (this.tans.buffer.Length != this.verts.buffer.Length));
                if (!flag2 && (this.panel.renderQueue != UIPanel.RenderQueue.Automatic))
                {
                    flag2 = (this.mMesh == null) || (this.mMesh.vertexCount != this.verts.buffer.Length);
                }
                this.mTriangles = this.verts.size >> 1;
                if (flag2 || (this.verts.buffer.Length > 0xfde8))
                {
                    if (flag2 || (this.mMesh.vertexCount != this.verts.size))
                    {
                        this.mMesh.Clear();
                        flag = true;
                    }
                    this.mMesh.vertices = this.verts.ToArray();
                    this.mMesh.uv = this.uvs.ToArray();
                    this.mMesh.colors32 = this.cols.ToArray();
                    if (this.norms != null)
                    {
                        this.mMesh.normals = this.norms.ToArray();
                    }
                    if (this.tans != null)
                    {
                        this.mMesh.tangents = this.tans.ToArray();
                    }
                }
                else
                {
                    if (this.mMesh.vertexCount != this.verts.buffer.Length)
                    {
                        this.mMesh.Clear();
                        flag = true;
                    }
                    this.mMesh.vertices = this.verts.buffer;
                    this.mMesh.uv = this.uvs.buffer;
                    this.mMesh.colors32 = this.cols.buffer;
                    if (this.norms != null)
                    {
                        this.mMesh.normals = this.norms.buffer;
                    }
                    if (this.tans != null)
                    {
                        this.mMesh.tangents = this.tans.buffer;
                    }
                }
                if (flag)
                {
                    this.mIndices = this.GenerateCachedIndexBuffer(size, indexCount);
                    this.mMesh.triangles = this.mIndices;
                }
                if (flag2 || !this.alwaysOnScreen)
                {
                    this.mMesh.RecalculateBounds();
                }
                this.mFilter.mesh = this.mMesh;
            }
            else
            {
                this.mTriangles = 0;
                if (this.mFilter.mesh != null)
                {
                    this.mFilter.mesh.Clear();
                }
                Debug.LogError("Too many vertices on one panel: " + this.verts.size);
            }
            if (this.mRenderer == null)
            {
                this.mRenderer = base.gameObject.GetComponent<MeshRenderer>();
            }
            if (this.mRenderer == null)
            {
                this.mRenderer = base.gameObject.AddComponent<MeshRenderer>();
            }
            this.UpdateMaterials();
        }
        else
        {
            if (this.mFilter.mesh != null)
            {
                this.mFilter.mesh.Clear();
            }
            Debug.LogError("UIWidgets must fill the buffer with 4 vertices per quad. Found " + size);
        }
        this.verts.Clear();
        this.uvs.Clear();
        this.cols.Clear();
        this.norms.Clear();
        this.tans.Clear();
    }

    private void UpdateMaterials()
    {
        if ((this.mRebuildMat || (this.mDynamicMat == null)) || ((this.mClipCount != this.panel.clipCount) || (this.mTextureClip != (this.panel.clipping == Clipping.TextureMask))))
        {
            this.RebuildMaterial();
            this.mRebuildMat = false;
        }
        else if (this.mRenderer.sharedMaterial != this.mDynamicMat)
        {
            this.mRenderer.sharedMaterials = new Material[] { this.mDynamicMat };
        }
    }

    public static BetterList<UIDrawCall> activeList =>
        mActiveList;

    public Material baseMaterial
    {
        get => 
            this.mMaterial;
        set
        {
            if (this.mMaterial != value)
            {
                this.mMaterial = value;
                this.mRebuildMat = true;
            }
        }
    }

    public Transform cachedTransform
    {
        get
        {
            if (this.mTrans == null)
            {
                this.mTrans = base.transform;
            }
            return this.mTrans;
        }
    }

    public Material dynamicMaterial =>
        this.mDynamicMat;

    public int finalRenderQueue =>
        ((this.mDynamicMat == null) ? this.mRenderQueue : this.mDynamicMat.renderQueue);

    public static BetterList<UIDrawCall> inactiveList =>
        mInactiveList;

    public bool isClipped =>
        (this.mClipCount != 0);

    [Obsolete("Use UIDrawCall.activeList")]
    public static BetterList<UIDrawCall> list =>
        mActiveList;

    public Texture mainTexture
    {
        get => 
            this.mTexture;
        set
        {
            this.mTexture = value;
            if (this.mDynamicMat != null)
            {
                this.mDynamicMat.mainTexture = value;
            }
        }
    }

    public int renderQueue
    {
        get => 
            this.mRenderQueue;
        set
        {
            if (this.mRenderQueue != value)
            {
                this.mRenderQueue = value;
                if (this.mDynamicMat != null)
                {
                    this.mDynamicMat.renderQueue = value;
                }
            }
        }
    }

    public Shader shader
    {
        get => 
            this.mShader;
        set
        {
            if (this.mShader != value)
            {
                this.mShader = value;
                this.mRebuildMat = true;
            }
        }
    }

    public int sortingOrder
    {
        get => 
            ((this.mRenderer == null) ? 0 : this.mRenderer.sortingOrder);
        set
        {
            if ((this.mRenderer != null) && (this.mRenderer.sortingOrder != value))
            {
                this.mRenderer.sortingOrder = value;
            }
        }
    }

    public int triangles =>
        ((this.mMesh == null) ? 0 : this.mTriangles);

    public enum Clipping
    {
        ConstrainButDontClip = 4,
        None = 0,
        SoftClip = 3,
        TextureMask = 1
    }

    public delegate void OnRenderCallback(Material mat);
}

