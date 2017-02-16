using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ServantSkillInfoIconComponent : BaseMonoBehaviour
{
    [SerializeField]
    protected UISprite backSprite;
    protected ClickDelegate callbackFunc;
    private int currentSkillId;
    private int currentSkillLv;
    [SerializeField]
    protected GameObject defLvLabel;
    [SerializeField]
    protected UISprite frameSprite;
    private int index;
    private bool isSetSkillId;
    [SerializeField]
    protected GameObject levelInfo;
    [SerializeField]
    protected UILabel levelLabel;
    [SerializeField]
    protected UISprite noSelectMskImg;
    [SerializeField]
    protected SkillIconComponent skillIconComp;
    [SerializeField]
    protected UISprite skillIconSprite;
    [SerializeField]
    protected UILabel skillNameLabel;

    public int getSkillInfo() => 
        this.currentSkillId;

    public void OnClickSkill()
    {
        if (this.skillIconSprite.gameObject.activeSelf)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        }
        if (this.callbackFunc != null)
        {
            this.callbackFunc(true, this.index);
        }
    }

    public void OnClickSkillDetail()
    {
        if (this.skillIconSprite.gameObject.activeSelf)
        {
            string str;
            string str2;
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData<SkillMaster>(DataNameKind.Kind.SKILL).getEntityFromId<SkillEntity>(this.currentSkillId).getSkillMessageInfo(out str, out str2, this.currentSkillLv);
            string info = string.Format(LocalizationManager.Get("LEVEL_INFO"), this.currentSkillLv);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenDetailInfoDialog(str, info, str2);
        }
    }

    public void setDispSelectMskImg(bool isShow)
    {
        this.noSelectMskImg.gameObject.SetActive(isShow);
    }

    public void SetSkillInfo(int idx, int skillId, int skillLv, int skillMaxLv, string skillName, int skillIconId, ClickDelegate callback)
    {
        this.index = idx;
        this.currentSkillId = skillId;
        this.currentSkillLv = skillLv;
        this.skillIconComp.Set(skillId, skillLv);
        this.skillIconSprite.gameObject.SetActive(true);
        this.levelInfo.gameObject.SetActive(true);
        this.skillNameLabel.text = skillName;
        this.levelLabel.text = string.Format(LocalizationManager.Get("DISP_SKLL_LV"), skillLv, skillMaxLv);
        this.callbackFunc = callback;
    }

    public delegate void ClickDelegate(bool isDecide, int idx);
}

