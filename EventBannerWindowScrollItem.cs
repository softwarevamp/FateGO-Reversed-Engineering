using System;
using UnityEngine;

public class EventBannerWindowScrollItem : MonoBehaviour
{
    public static readonly string DEFAULT_SP_NAME = "banner_loading";
    [SerializeField]
    private UISprite mBannerSp;
    [SerializeField]
    private UIAtlas mCommonAtlas;
    private TitleInfoControl.EventEndTimeInfo mEventEndTimeInfo;

    public void OnClickItem()
    {
        if (this.mEventEndTimeInfo != null)
        {
            TitleInfoControl.OnClickEventBtn(this.mEventEndTimeInfo.event_id);
        }
    }

    public void Setup(TitleInfoControl.EventEndTimeInfo ev_end_time_inf)
    {
        bool flag = false;
        this.mEventEndTimeInfo = ev_end_time_inf;
        if (this.mEventEndTimeInfo != null)
        {
            flag = AtlasManager.SetShopBanner(this.mBannerSp, this.mEventEndTimeInfo.event_id);
            this.mBannerSp.alpha = 1f;
        }
        if (!flag)
        {
            this.mBannerSp.atlas = this.mCommonAtlas;
            this.mBannerSp.spriteName = DEFAULT_SP_NAME;
            this.mBannerSp.alpha = 0f;
        }
        this.mBannerSp.MakePixelPerfect();
    }
}

