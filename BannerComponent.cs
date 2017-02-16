using System;
using UnityEngine;

public class BannerComponent : MonoBehaviour
{
    [SerializeField]
    protected UICommonButton bannerButton;
    [SerializeField]
    protected UISprite bannerSprite;
    private string linkBody;

    public void OnClick()
    {
        WebViewManager.OpenView(string.Empty, this.linkBody, null);
    }

    public void SetBanner(EventEntity eventData)
    {
        AtlasManager.SetBanner(this.bannerSprite, eventData);
        this.linkBody = eventData.getEventLinkBody();
    }
}

