using System;
using UnityEngine;

public class GachaBannerComponent : MonoBehaviour
{
    private string assetPath;
    private int bannerIdx;
    [SerializeField]
    protected UISprite bannerImg;
    private int bannerImgId;
    [SerializeField]
    protected UISprite closeImg;
    [SerializeField]
    protected GameObject closeInfo;
    [SerializeField]
    protected UILabel closeTxtLb;
    private VaildGachaInfo info;
    private int moveBannerIdx;

    public VaildGachaInfo getBannerGachaInfo() => 
        this.info;

    public int getBannerIdx() => 
        this.bannerIdx;

    public int getMoveBannerIdx() => 
        this.moveBannerIdx;

    public void OnClickDetail()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        string detailUrl = this.info.detailUrl;
        WebViewManager.OpenView(LocalizationManager.Get("WEB_VIEW_TITLE_SUMMON"), detailUrl, null);
    }

    public void setBannerGachaInfo(VaildGachaInfo data, int idx, int moveIdx, GameObject bannerAtlas, string imgName)
    {
        this.info = data;
        this.bannerIdx = idx;
        this.moveBannerIdx = moveIdx;
        if (bannerAtlas != null)
        {
            UIAtlas component = bannerAtlas.GetComponent<UIAtlas>();
            this.bannerImg.atlas = component;
            this.bannerImg.spriteName = imgName;
        }
    }

    public void setEnabledCollider(bool isEnable)
    {
        base.GetComponent<Collider>().enabled = isEnable;
    }
}

