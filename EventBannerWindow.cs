using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class EventBannerWindow : BaseDialog
{
    [CompilerGenerated]
    private static System.Action <>f__am$cacheB;
    [SerializeField]
    protected UIButton closeBtn;
    [SerializeField]
    protected UIGrid grid;
    private int mEventCount;
    private List<EventBannerWindowScrollItem> mScrollItems = new List<EventBannerWindowScrollItem>();
    public static readonly int OPEN_POSSIBLE_COUNT = 2;
    public static readonly int SCROLL_ITEM_MIN = 2;
    public static readonly float SCROLL_ITEM_Y_INTERVAL = 150f;
    [SerializeField]
    protected GameObject scrollItemPrefab;
    [SerializeField]
    protected UIScrollView scrollView;
    [SerializeField]
    protected UILabel titleDetailLabel;
    [SerializeField]
    protected UILabel titleLabel;

    public bool IsOpenPossible() => 
        (this.mEventCount >= OPEN_POSSIBLE_COUNT);

    public void OnClickClose()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        if (<>f__am$cacheB == null)
        {
            <>f__am$cacheB = delegate {
            };
        }
        base.Close(<>f__am$cacheB);
    }

    public void Open(System.Action end_act = null)
    {
        base.Open(end_act, true);
        this.scrollView.ResetPosition();
    }

    public bool Setup(List<TitleInfoControl.EventEndTimeInfo> ev_end_time_infs)
    {
        bool flag = this.mEventCount != ev_end_time_infs.Count;
        if (flag)
        {
            foreach (EventBannerWindowScrollItem item in this.mScrollItems)
            {
                UnityEngine.Object.Destroy(item.gameObject);
            }
            this.mScrollItems.Clear();
            int count = ev_end_time_infs.Count;
            if (count < SCROLL_ITEM_MIN)
            {
                count = SCROLL_ITEM_MIN;
            }
            for (int j = 0; j < count; j++)
            {
                GameObject self = UnityEngine.Object.Instantiate<GameObject>(this.scrollItemPrefab);
                self.SafeSetParent(this.grid);
                EventBannerWindowScrollItem component = self.GetComponent<EventBannerWindowScrollItem>();
                this.mScrollItems.Add(component);
            }
            this.mEventCount = ev_end_time_infs.Count;
        }
        for (int i = 0; i < this.mScrollItems.Count; i++)
        {
            EventBannerWindowScrollItem item3 = this.mScrollItems[i];
            TitleInfoControl.EventEndTimeInfo info = null;
            if (i < ev_end_time_infs.Count)
            {
                info = ev_end_time_infs[i];
            }
            item3.Setup(info);
        }
        this.grid.Reposition();
        base.Init();
        this.titleLabel.text = LocalizationManager.Get("EVENT_BANNER_WINDOW_TITLE");
        this.titleDetailLabel.text = LocalizationManager.Get("EVENT_BANNER_WINDOW_MESSAGE");
        return flag;
    }
}

