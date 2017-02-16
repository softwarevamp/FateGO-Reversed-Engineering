using System;
using System.Runtime.CompilerServices;

public class MessageItem : BaseScrollViewItem
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache3;
    private UILabel m_time;
    private UILabel m_title;
    private string url = string.Empty;

    public void OnClick()
    {
        Debug.Log("openurl ::: " + this.url);
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = delegate {
            };
        }
        WebViewManager.OpenUniWebView(LocalizationManager.Get("WEB_VIEW_TITLE_INFOMATION"), this.url, <>f__am$cache3);
    }

    public override void SetItem(params object[] o)
    {
        AnnouncementData data = new AnnouncementData();
        data = (AnnouncementData) o[0];
        this.m_time = base.gameObject.transform.FindChild("Time").GetComponent<UILabel>();
        this.m_title = base.gameObject.transform.FindChild("Title").GetComponent<UILabel>();
        this.m_time.text = data.addtime;
        this.m_title.text = data.title;
        this.url = data.url;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
}

