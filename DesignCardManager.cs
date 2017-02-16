using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DesignCardManager : SingletonMonoBehaviour<DesignCardManager>
{
    private readonly int[] cardTextureSize = new int[] { 0x200, 0x36b };

    public string GetDesignCardPath(int imageId, Rarity.TYPE rarity)
    {
        string str = "ClassCard/";
        string str2 = Rarity.getDesignCardPrefix((int) rarity);
        int num = imageId;
        if ((imageId & 1) == 0)
        {
            num--;
        }
        if (((num < 1) || (num > 15)) && ((num != 0x65) && (num != 0x67)))
        {
            num = 1;
        }
        return (str + str2 + num);
    }

    public void LoadDesignCardTexture(int imageId, Rarity.TYPE rarity, Action<Texture2D> callback)
    {
        <LoadDesignCardTexture>c__AnonStorey64 storey = new <LoadDesignCardTexture>c__AnonStorey64 {
            callback = callback
        };
        AssetManager.loadAssetStorage(this.GetDesignCardPath(imageId, rarity), new AssetLoader.LoadEndDataHandler(storey.<>m__40));
    }

    public void ReleaseDesignCard(int imageId, Rarity.TYPE rarity)
    {
        this.ReleaseDesignTexture(imageId, rarity);
    }

    public void ReleaseDesignTexture(int imageId, Rarity.TYPE rarity)
    {
        AssetManager.releaseAssetStorage(this.GetDesignCardPath(imageId, rarity));
    }

    public void SetupCardImage(AssetData d, Transform cardNode, int imageId)
    {
        Texture2D tex = d.GetObject<Texture2D>();
        UITexture component = cardNode.GetComponent<UITexture>();
        this.SetupDesignCardTexture(imageId, component, tex);
    }

    public void SetupDesignCard(int imageId, Rarity.TYPE rarity, UITexture target, System.Action callback)
    {
        <SetupDesignCard>c__AnonStorey65 storey = new <SetupDesignCard>c__AnonStorey65 {
            imageId = imageId,
            target = target,
            callback = callback,
            <>f__this = this
        };
        this.LoadDesignCardTexture(storey.imageId, rarity, new Action<Texture2D>(storey.<>m__41));
    }

    protected void SetupDesignCardTexture(int imageId, UITexture target, Texture2D tex)
    {
        target.mainTexture = tex;
        float height = ((float) this.cardTextureSize[1]) / 1024f;
        float x = 0.5f * ((imageId - 1) & 1);
        target.uvRect = new Rect(x, 1f - height, 0.5f, height);
    }

    [CompilerGenerated]
    private sealed class <LoadDesignCardTexture>c__AnonStorey64
    {
        internal Action<Texture2D> callback;

        internal void <>m__40(AssetData data)
        {
            Texture2D textured = data.GetObject<Texture2D>();
            if (this.callback != null)
            {
                this.callback(textured);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <SetupDesignCard>c__AnonStorey65
    {
        internal DesignCardManager <>f__this;
        internal System.Action callback;
        internal int imageId;
        internal UITexture target;

        internal void <>m__41(Texture2D tex)
        {
            this.<>f__this.SetupDesignCardTexture(this.imageId, this.target, tex);
            if (this.callback != null)
            {
                this.callback();
            }
        }
    }
}

