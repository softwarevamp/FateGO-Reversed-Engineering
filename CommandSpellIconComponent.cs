using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CommandSpellIconComponent : MonoBehaviour
{
    private AssetData assetData;
    protected System.Action callbackFunc;
    [SerializeField]
    protected int ImageType = 1;
    protected int ImageTypeOld = -1;
    [SerializeField]
    protected UITexture myTexture;
    [SerializeField]
    protected int Remain = 3;
    [SerializeField]
    protected Vector2 Size = new Vector2(40f, 40f);

    private void Awake()
    {
        this.myTexture = base.GetComponentInChildren<UITexture>();
    }

    private string GetSpellImageAssetStorageName(int tp) => 
        ("CommandSpell/" + this.GetSpellImageFileName(tp));

    private string GetSpellImageFileName(int tp) => 
        ("cs_" + $"{tp:0000}");

    private void Initialize()
    {
        this.SetImageType(this.ImageType);
        this.SetRemain(this.Remain);
        this.SetSize(this.Size);
    }

    private void OnDestroy()
    {
        this.ReleaseAll();
    }

    public void ReleaseAll()
    {
        UITexture myTexture = this.myTexture;
        if (myTexture != null)
        {
            Material material = myTexture.material;
            if (material == null)
            {
                return;
            }
            material.mainTexture = null;
            material.SetTexture("_MaskTex", null);
            UnityEngine.Object.Destroy(material);
            myTexture.material = null;
        }
        if (this.assetData != null)
        {
            AssetManager.releaseAsset(this.assetData);
            this.assetData = null;
        }
    }

    public void SetChangeCmdSpellData(int cmdSpellImgId)
    {
        int count = ConstantMaster.getValue("MAX_COMMAND_SPELL");
        this.SetImageType(cmdSpellImgId);
        this.SetRemain(count);
    }

    public void SetChangeCurrentCmdSepll(int cmdSpellImgId, int cmdRemain, System.Action callback)
    {
        int tp = cmdSpellImgId;
        int count = cmdRemain;
        this.SetImageType(tp);
        this.SetRemain(count);
        if (callback != null)
        {
            callback();
        }
    }

    public void SetData(UserGameEntity entity)
    {
        int spellImageId = entity.SpellImageId;
        int count = entity.getCommandSpell();
        this.SetImageType(spellImageId);
        this.SetRemain(count);
    }

    public void SetFullData(UserGameEntity entity)
    {
        int spellImageId = entity.SpellImageId;
        int count = ConstantMaster.getValue("MAX_COMMAND_SPELL");
        this.SetImageType(spellImageId);
        this.SetRemain(count);
    }

    public void SetImageType(int tp)
    {
        <SetImageType>c__AnonStorey62 storey = new <SetImageType>c__AnonStorey62 {
            tp = tp,
            <>f__this = this
        };
        if (this.ImageTypeOld != storey.tp)
        {
            this.ImageType = storey.tp;
            this.ImageTypeOld = storey.tp;
            Debug.Log("***** CommnadSpell SetImageType Type: " + storey.tp);
            string spellImageAssetStorageName = this.GetSpellImageAssetStorageName(storey.tp);
            Debug.Log("***** CommnadSpell SetImageType name: " + spellImageAssetStorageName);
            if (AssetManager.isExistAssetStorage(spellImageAssetStorageName))
            {
                this.ReleaseAll();
                AssetManager.loadAssetStorage(spellImageAssetStorageName, new AssetLoader.LoadEndDataHandler(storey.<>m__3D));
            }
        }
    }

    public void SetRemain(int count)
    {
        this.Remain = count;
        UITexture myTexture = this.myTexture;
        if (myTexture != null)
        {
            float x = ((count & 1) ^ 1) * 0.5f;
            float y = ((count >> 1) & 1) * 0.5f;
            myTexture.uvRect = new Rect(x, y, 0.5f, 0.5f);
        }
    }

    public void SetSize(Vector2 sz)
    {
        UITexture myTexture = this.myTexture;
        myTexture.width = (int) sz.x;
        myTexture.height = (int) sz.y;
    }

    private void SetTexture(AssetData data, int tp)
    {
        if (data != null)
        {
            string spellImageFileName = this.GetSpellImageFileName(tp);
            UITexture myTexture = this.myTexture;
            Debug.Log("***** CommnadSpell SetTexture tex: " + myTexture);
            if (myTexture != null)
            {
                Texture2D textured = data.GetObject<Texture2D>(spellImageFileName);
                Texture2D texture = data.GetObject<Texture2D>(spellImageFileName + "_alpha");
                Material material = myTexture.material;
                if (material == null)
                {
                    material = new Material(Shader.Find("Custom/SpriteWithMask"));
                    myTexture.material = material;
                }
                material.mainTexture = textured;
                material.SetTexture("_MaskTex", texture);
            }
        }
    }

    private void Update()
    {
    }

    [CompilerGenerated]
    private sealed class <SetImageType>c__AnonStorey62
    {
        internal CommandSpellIconComponent <>f__this;
        internal int tp;

        internal void <>m__3D(AssetData data)
        {
            this.<>f__this.assetData = data;
            this.<>f__this.SetTexture(data, this.tp);
        }
    }
}

