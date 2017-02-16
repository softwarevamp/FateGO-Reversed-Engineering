using System;
using UnityEngine;

public class UIUnityRenderer : UIWidget
{
    public bool allowSharedMaterial;
    private bool createMat;
    [SerializeField, HideInInspector]
    private Material[] mMats;
    [HideInInspector, SerializeField]
    private Renderer mRenderer;
    [SerializeField, HideInInspector]
    private int renderQueue = -1;

    private bool CheckMaterial(Material[] mats)
    {
        if ((mats == null) || (mats.Length <= 0))
        {
            return false;
        }
        for (int i = 0; i < mats.Length; i++)
        {
            if (mats[i] == null)
            {
                return false;
            }
        }
        return true;
    }

    private bool ExistSharedMaterial0() => 
        ((this.cachedRenderer != null) && this.CheckMaterial(this.cachedRenderer.sharedMaterials));

    public bool isEqualMaterials(Material[] a, Material[] b)
    {
        if (a.Length != b.Length)
        {
            return false;
        }
        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] != b[i])
            {
                return false;
            }
        }
        return true;
    }

    private void OnDestroy()
    {
        if (this.mMats != null)
        {
            for (int i = 0; i < this.mMats.Length; i++)
            {
                if (this.createMat)
                {
                    UnityEngine.Object.DestroyImmediate(this.mMats[i]);
                }
                this.mMats[i] = null;
            }
            this.mMats = null;
        }
    }

    public override void OnFill(BetterList<Vector3> verts, BetterList<Vector2> uvs, BetterList<Color32> cols)
    {
        verts.Add(new Vector3(10000f, 10000f));
        verts.Add(new Vector3(10000f, 10000f));
        verts.Add(new Vector3(10000f, 10000f));
        verts.Add(new Vector3(10000f, 10000f));
        uvs.Add(new Vector2(0f, 0f));
        uvs.Add(new Vector2(0f, 1f));
        uvs.Add(new Vector2(1f, 1f));
        uvs.Add(new Vector2(1f, 0f));
        cols.Add(base.color);
        cols.Add(base.color);
        cols.Add(base.color);
        cols.Add(base.color);
    }

    private void OnWillRenderObject()
    {
        if (!this.allowSharedMaterial)
        {
            if (this.CheckMaterial(this.mMats) && (base.drawCall != null))
            {
                this.renderQueue = base.drawCall.finalRenderQueue;
                for (int i = 0; i < this.mMats.Length; i++)
                {
                    if (this.mMats[i].renderQueue != this.renderQueue)
                    {
                        this.mMats[i].renderQueue = this.renderQueue;
                    }
                }
            }
        }
        else if (this.ExistSharedMaterial0() && (base.drawCall != null))
        {
            this.renderQueue = base.drawCall.finalRenderQueue;
            for (int j = 0; j < this.cachedRenderer.sharedMaterials.Length; j++)
            {
                this.cachedRenderer.sharedMaterials[j].renderQueue = this.renderQueue;
            }
        }
    }

    public Renderer cachedRenderer
    {
        get
        {
            if (this.mRenderer == null)
            {
                this.mRenderer = base.GetComponent<Renderer>();
            }
            return this.mRenderer;
        }
    }

    public override Material material
    {
        get
        {
            if (!this.ExistSharedMaterial0())
            {
                Debug.LogError("Renderer or Material is not found.");
                return null;
            }
            if (this.allowSharedMaterial)
            {
                return this.cachedRenderer.sharedMaterials[0];
            }
            if (!this.CheckMaterial(this.mMats))
            {
                this.createMat = true;
                this.mMats = new Material[this.cachedRenderer.sharedMaterials.Length];
                for (int i = 0; i < this.cachedRenderer.sharedMaterials.Length; i++)
                {
                    this.mMats[i] = new Material(this.cachedRenderer.sharedMaterials[i]);
                    this.mMats[i].name = this.mMats[i].name + " (Copy)";
                }
            }
            if ((this.CheckMaterial(this.mMats) && Application.isPlaying) && !this.isEqualMaterials(this.cachedRenderer.sharedMaterials, this.mMats))
            {
                this.cachedRenderer.sharedMaterials = this.mMats;
            }
            return this.mMats[0];
        }
        set
        {
            throw new NotImplementedException(base.GetType() + " has no material setter");
        }
    }

    public override Shader shader
    {
        get
        {
            if (!this.allowSharedMaterial)
            {
                if (this.CheckMaterial(this.mMats))
                {
                    return this.mMats[0].shader;
                }
            }
            else if (this.ExistSharedMaterial0())
            {
                return this.cachedRenderer.sharedMaterials[0].shader;
            }
            return null;
        }
        set
        {
            throw new NotImplementedException(base.GetType() + " has no shader setter");
        }
    }
}

