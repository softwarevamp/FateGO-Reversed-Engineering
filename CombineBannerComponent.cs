using System;
using UnityEngine;

public class CombineBannerComponent : MonoBehaviour
{
    public UIButton bannerBtn;
    public UISprite bannerSprite;
    private EventEntity eventEntity;

    public void onOpenWebView()
    {
        WebViewManager.OpenView(string.Empty, this.eventEntity.getEventLinkBody(), null);
    }

    public void setBannerInfo(EventEntity eventData)
    {
        AtlasManager.SetBanner(this.bannerSprite, eventData);
        this.eventEntity = eventData;
    }
}

