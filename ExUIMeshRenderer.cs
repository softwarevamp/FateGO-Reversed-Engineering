using System;
using System.Runtime.InteropServices;
using UnityEngine;

[ExecuteInEditMode, AddComponentMenu("NGUI/UI/NGUI ExMeshRenderer")]
public class ExUIMeshRenderer : UITweenRenderer
{
    protected AssetData assetData;
    protected string assetLabel;
    protected int baseHeight;
    protected int baseWidth;
    protected System.Action callbackFunc;
    [SerializeField]
    public Texture defaultTexture;
    protected bool isFirst = true;
    [SerializeField]
    public bool isRescale;
    protected string loadAssetName;
    [SerializeField]
    public MeshFilter meshFilter;
    [SerializeField]
    public MeshRenderer meshRenderer;
    [SerializeField]
    public Vector2 uvOffset = new Vector2(0f, 0f);
    [SerializeField]
    public Vector2 uvSize = new Vector2(1f, 1f);

    public void ClearImage()
    {
        this.meshRenderer.material.mainTexture = null;
        if (this.assetData != null)
        {
            AssetManager.releaseAsset(this.assetData);
            this.assetData = null;
        }
        if (this.loadAssetName != null)
        {
            AssetManager.releaseAssetStorage(this.loadAssetName);
            this.loadAssetName = null;
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc();
            }
        }
    }

    protected void EndLoad(AssetData data)
    {
        if (((data != null) && (this.loadAssetName != null)) && AssetManager.compAssetStorage(data, this.loadAssetName))
        {
            AssetData assetData = this.assetData;
            this.assetData = data;
            this.SetTexture(data.GetObject<Texture2D>(this.assetLabel));
            if (assetData != null)
            {
                AssetManager.releaseAsset(assetData);
            }
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc();
            }
        }
    }

    protected override void OnStart()
    {
        base.OnStart();
        if (this.meshRenderer == null)
        {
            this.meshRenderer = base.GetComponent<MeshRenderer>();
        }
        if (this.defaultTexture != null)
        {
            this.SetImage(this.defaultTexture);
        }
        this.ReScale();
    }

    protected void ReScale()
    {
        if (this.isFirst)
        {
            this.isFirst = false;
            this.baseWidth = base.width;
            this.baseHeight = base.height;
            base.width = 0;
        }
        float num = 1f;
        if (this.isRescale)
        {
            float num2 = ((float) (Screen.height * ManagerConfig.WIDTH)) / ((float) (Screen.width * ManagerConfig.HEIGHT));
            if (num2 > 1f)
            {
                num = num2;
            }
        }
        int num3 = (int) (num * this.baseWidth);
        int num4 = (int) (num * this.baseHeight);
        if ((num3 != base.width) || (num4 != base.height))
        {
            base.width = num3;
            base.height = num4;
            Mesh mesh = this.meshFilter.mesh;
            Vector3[] vectorArray = new Vector3[4];
            float x = -base.pivotOffset.x * num3;
            float y = -base.pivotOffset.y * num4;
            vectorArray[0] = new Vector3(x, y, 0f);
            vectorArray[1] = new Vector3(x + num3, y + num4, 0f);
            vectorArray[2] = new Vector3(x + num3, y, 0f);
            vectorArray[3] = new Vector3(x, y + num4, 0f);
            mesh.vertices = vectorArray;
            mesh.uv = new Vector2[] { new Vector2(this.uvOffset.x, this.uvOffset.y), new Vector2(this.uvOffset.x + this.uvSize.x, this.uvOffset.y + this.uvSize.y), new Vector2(this.uvOffset.x + this.uvSize.x, this.uvOffset.y), new Vector2(this.uvOffset.x, this.uvOffset.y + this.uvSize.y) };
            this.meshFilter.mesh = mesh;
        }
    }

    public void SetAssetImage(string assetName, System.Action callback = null)
    {
        this.SetAssetImage(assetName, null, callback);
    }

    public void SetAssetImage(string assetName, string assetLabel, System.Action callback = null)
    {
        this.assetLabel = assetLabel;
        if (this.loadAssetName != null)
        {
            if (AssetManager.compAssetStorage(this.loadAssetName, assetName))
            {
                if (this.callbackFunc != null)
                {
                    this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, this.callbackFunc);
                }
                return;
            }
            AssetManager.releaseAssetStorage(this.loadAssetName);
        }
        else if ((this.assetData != null) && AssetManager.compAssetStorage(this.assetData, assetName))
        {
            this.SetTexture(this.assetData.GetObject<Texture2D>(assetLabel));
            if (this.callbackFunc != null)
            {
                this.callbackFunc();
            }
            return;
        }
        this.loadAssetName = assetName;
        if (callback != null)
        {
            this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callback);
        }
        AssetManager.loadAssetStorage(assetName, new AssetLoader.LoadEndDataHandler(this.EndLoad));
    }

    public void SetImage(Texture tex)
    {
        this.SetTexture(tex);
        if (this.assetData != null)
        {
            AssetManager.releaseAsset(this.assetData);
            this.assetData = null;
        }
        if (this.loadAssetName != null)
        {
            AssetManager.releaseAssetStorage(this.loadAssetName);
            this.loadAssetName = null;
            System.Action callbackFunc = this.callbackFunc;
            this.callbackFunc = null;
            if (callbackFunc != null)
            {
                callbackFunc();
            }
        }
    }

    protected void SetTexture(Texture tex)
    {
        this.meshRenderer.material.mainTexture = tex;
        this.ReScale();
    }

    public override void SetTweenColor(Color c)
    {
        base.color = c;
        Material material = this.material;
        if ((material != null) && material.HasProperty("_Color"))
        {
            material.SetColor("_Color", c);
        }
        this.meshRenderer.enabled = c.a > 0f;
    }

    public override void SetTweenVolume(float v)
    {
        base.volume = v;
        Material material = this.material;
        if ((material != null) && material.HasProperty("_Volume"))
        {
            material.SetFloat("_Volume", v);
        }
    }

    public Material material
    {
        get => 
            this.meshRenderer?.material;
        set
        {
            this.meshRenderer.material = value;
        }
    }
}

