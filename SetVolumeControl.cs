using System;
using UnityEngine;

public class SetVolumeControl : MonoBehaviour
{
    public UISlider bgmSilder;
    public UILabel bgmTxt;
    private float bgmValue;
    public UILabel bgmValueTxt;
    public UISlider seSilder;
    public UILabel seTxt;
    private float seValue;
    public UILabel seValueTxt;
    public UISlider voiceSilder;
    public UILabel voiceTxt;
    private float voiceValue;
    public UILabel voiceValueTxt;

    public void getChangeBgmValue()
    {
        this.bgmValue = this.bgmSilder.value;
        this.bgmValueTxt.text = Mathf.CeilToInt(this.bgmValue * 10f).ToString();
        OptionManager.TestBgmVolume(this.bgmValue);
    }

    public void getChangeSeValue()
    {
        float num = this.seSilder.value;
        bool flag = this.seValue != num;
        this.seValue = this.seSilder.value;
        this.seValueTxt.text = Mathf.CeilToInt(this.seValue * 10f).ToString();
        OptionManager.TestSeVolume(this.seValue);
        if (flag)
        {
            SoundManager.playSe("testSe");
        }
    }

    public void getChangeVoiceValue()
    {
        float num = this.voiceSilder.value;
        bool flag = this.voiceValue != num;
        this.voiceValue = this.voiceSilder.value;
        this.voiceValueTxt.text = Mathf.CeilToInt(this.voiceValue * 10f).ToString();
        OptionManager.TestVoiceVolume(this.voiceValue);
        if (flag)
        {
            SoundManager.playVoice("testVoice");
        }
    }

    public void initSetVolume()
    {
        this.bgmTxt.text = LocalizationManager.Get("OPTION_BGM");
        this.seTxt.text = LocalizationManager.Get("OPTION_SE");
        this.voiceTxt.text = LocalizationManager.Get("OPTION_VOICE");
        this.bgmValue = OptionManager.GetBgmVolume();
        this.seValue = OptionManager.GetSeVolume();
        this.voiceValue = OptionManager.GetVoiceVolume();
        this.bgmSilder.value = OptionManager.GetBgmVolume();
        this.seSilder.value = OptionManager.GetSeVolume();
        this.voiceSilder.value = OptionManager.GetVoiceVolume();
    }

    public void reflectionVolume()
    {
        OptionManager.SetBgmVolume(this.bgmValue);
        OptionManager.SetSeVolume(this.seValue);
        OptionManager.SetVoiceVolume(this.voiceValue);
    }

    private void setChangeBgmValue()
    {
        this.bgmValue = this.bgmSilder.value;
        OptionManager.TestBgmVolume(this.bgmValue);
    }

    private void setChangeSeValue()
    {
        this.seValue = this.seSilder.value;
        OptionManager.TestSeVolume(this.seValue);
    }

    private void setChangeVoiceValue()
    {
        this.voiceValue = this.voiceSilder.value;
        OptionManager.TestVoiceVolume(this.voiceValue);
    }
}

