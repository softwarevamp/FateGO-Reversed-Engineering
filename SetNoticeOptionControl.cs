using System;
using UnityEngine;

public class SetNoticeOptionControl : MonoBehaviour
{
    public UIButton apNoticeBtn;
    public UILabel apNoticeTitle;
    public UILabel apNoticeTxt;
    private bool isApNotice = true;
    private bool isGameNotice = true;
    public UIButton sysNoticeBtn;
    public UILabel sysNoticeTitle;
    public UILabel sysNoticeTxt;

    public void initSetNotice()
    {
        this.apNoticeTitle.text = LocalizationManager.Get("OPTION_NOTICE_AP");
        this.sysNoticeTitle.text = LocalizationManager.Get("OPTION_NOTICE_ELSE");
        this.isApNotice = OptionManager.GetLocalNotiffication();
        this.isGameNotice = OptionManager.GetNotiffication();
        this.setApNoticeValue();
        this.setGameNoticeValue();
    }

    public void OnApNoticeChangeBtn()
    {
        if (this.isApNotice)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.apNoticeBtn.normalSprite = "btn_off";
            this.isApNotice = false;
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.apNoticeBtn.normalSprite = "btn_on";
            this.isApNotice = true;
        }
    }

    public void OnGameNoticeChangeBtn()
    {
        if (this.isGameNotice)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.sysNoticeBtn.normalSprite = "btn_off";
            this.isGameNotice = false;
        }
        else
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            this.sysNoticeBtn.normalSprite = "btn_on";
            this.isGameNotice = true;
        }
    }

    public void reflectionNotice()
    {
        OptionManager.SetLocalNotiffication(this.isApNotice);
        OptionManager.SetNotiffication(this.isGameNotice, false);
    }

    private void setApNoticeValue()
    {
        if (this.isApNotice)
        {
            this.apNoticeBtn.normalSprite = "btn_on";
        }
        else
        {
            this.apNoticeBtn.normalSprite = "btn_off";
        }
    }

    private void setGameNoticeValue()
    {
        if (this.isGameNotice)
        {
            this.sysNoticeBtn.normalSprite = "btn_on";
        }
        else
        {
            this.sysNoticeBtn.normalSprite = "btn_off";
        }
    }
}

