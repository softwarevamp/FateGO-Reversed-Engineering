using System;
using UnityEngine;

public class ServantStatusFlavorTextListViewItemDrawVoice : ServantStatusFlavorTextListViewItemDraw
{
    [SerializeField]
    protected UILabel cvLabel;
    [SerializeField]
    protected UILabel cvTitleLabel;
    [SerializeField]
    protected UILabel illustLabel;
    [SerializeField]
    protected UILabel illustTitleLabel;
    [SerializeField]
    protected UICommonButton voiceButton;
    [SerializeField]
    protected UILabel voiceButtonLabel;

    public override ServantStatusFlavorTextListViewItemDraw.Kind GetKind() => 
        ServantStatusFlavorTextListViewItemDraw.Kind.VOICE;

    public override void SetItem(ServantStatusListViewItem item, bool isOpen, bool isNew, string text, string text2, ServantStatusFlavorTextListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, isOpen, isNew, text, text2, mode);
        if ((item != null) && (mode != ServantStatusFlavorTextListViewItemDraw.DispMode.INVISIBLE))
        {
            string str;
            string str2;
            bool flag;
            this.illustTitleLabel.text = LocalizationManager.Get("SERVANT_STATUS_PROFILE_ILLUST_TITLE");
            this.cvTitleLabel.text = LocalizationManager.Get("SERVANT_STATUS_PROFILE_CV_TITLE");
            this.voiceButtonLabel.text = LocalizationManager.Get("SERVANT_STATUS_PROFILE_VOICE_BUTTON");
            item.GetVoiceInfo(out str, out str2, out flag);
            this.illustLabel.text = str;
            this.cvLabel.text = str2;
            if (flag)
            {
                this.voiceButton.SetState(UICommonButtonColor.State.Normal, true);
            }
            else
            {
                this.voiceButton.SetState(UICommonButtonColor.State.Disabled, true);
            }
        }
    }
}

