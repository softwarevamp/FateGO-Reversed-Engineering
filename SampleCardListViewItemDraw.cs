using System;
using UnityEngine;

[AddComponentMenu("Sample/TestListView/SampleCardListViewItemDraw")]
public class SampleCardListViewItemDraw : MonoBehaviour
{
    protected AssetData cardData;
    protected string cardFileName;
    protected int cardId = -1;
    [SerializeField]
    protected UITexture cardImageTexture;
    protected bool isFront;

    public void AddDepth(int v)
    {
        foreach (UIWidget widget in base.GetComponentsInChildren<UIWidget>())
        {
            widget.depth = v;
        }
    }

    protected void EndLoadCard(AssetData data)
    {
        if ((data != null) && (this.cardFileName != data.Name))
        {
            AssetManager.releaseAsset(data);
        }
        else
        {
            if (this.cardData != null)
            {
                AssetManager.releaseAsset(this.cardData);
                this.cardData = null;
            }
            if (data != null)
            {
                this.cardData = data;
                this.cardImageTexture.mainTexture = data.GetObject<Texture2D>();
            }
        }
    }

    protected void onDestroy()
    {
        if (this.cardData != null)
        {
            AssetManager.releaseAsset(this.cardData);
            this.cardData = null;
        }
        this.cardId = -1;
        this.cardFileName = null;
    }

    public void SetItem(SampleCardListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            int num = !this.isFront ? 0 : item.CardId;
            if (this.cardId != num)
            {
                this.cardId = num;
                this.cardFileName = $"Test/card{num:d3}";
                AssetManager.loadAssetStorage(this.cardFileName, new AssetLoader.LoadEndDataHandler(this.EndLoadCard));
            }
        }
        else
        {
            this.cardId = -1;
        }
        TweenColor component = this.cardImageTexture.gameObject.GetComponent<TweenColor>();
        if (component != null)
        {
            component.enabled = false;
        }
        this.cardImageTexture.color = (mode != DispMode.INVALID) ? Color.white : Color.gray;
    }

    public bool IsFront
    {
        get => 
            this.isFront;
        set
        {
            this.isFront = value;
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

