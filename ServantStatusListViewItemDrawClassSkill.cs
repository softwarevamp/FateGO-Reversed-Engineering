using System;
using UnityEngine;

public class ServantStatusListViewItemDrawClassSkill : ServantStatusListViewItemDraw
{
    [SerializeField]
    protected UICommonButton baseButton;
    [SerializeField]
    protected BoxCollider baseCollider;
    protected Vector3 baseSize;
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected UILabel explanationLabel;
    protected string[] explanationMessageList;
    [SerializeField]
    protected GameObject[] skillBaseList = new GameObject[BalanceConfig.SvtSkillListMax];
    protected Vector3 skillBasePosition;
    [SerializeField]
    protected UILabel[] skillExplanationLabelList = new UILabel[BalanceConfig.SvtSkillListMax];
    [SerializeField]
    protected SkillIconComponent[] skillIconList = new SkillIconComponent[BalanceConfig.SvtSkillListMax];
    protected int skillPitch;
    [SerializeField]
    protected UILabel[] skillTitleLabelList = new UILabel[BalanceConfig.SvtSkillListMax];
    protected Vector3 titleBasePosition;
    protected string[] titleMessageList;
    [SerializeField]
    protected UISprite titleSprite;

    protected void Awake()
    {
        if (this.skillBaseList.Length >= 2)
        {
            this.skillPitch = (int) (this.skillBaseList[0].transform.localPosition.y - this.skillBaseList[1].transform.localPosition.y);
        }
        int num = this.skillBaseList.Length * this.skillPitch;
        this.baseSize = new Vector3((float) this.baseSprite.width, (float) (this.baseSprite.height - num));
        this.titleBasePosition = this.titleSprite.transform.localPosition;
        this.titleBasePosition.y -= num / 2;
        this.skillBasePosition = this.skillBaseList[0].transform.localPosition;
        this.skillBasePosition.y -= num / 2;
    }

    public override ServantStatusListViewItemDraw.Kind GetKind() => 
        ServantStatusListViewItemDraw.Kind.CLASS_SKILL;

    public override void SetItem(ServantStatusListViewItem item, ServantStatusListViewItemDraw.DispMode mode)
    {
        base.SetItem(item, mode);
        if ((item != null) && (mode != ServantStatusListViewItemDraw.DispMode.INVISIBLE))
        {
            int[] numArray;
            this.explanationLabel.text = LocalizationManager.Get("SERVANT_STATUS_EXPLANATION_SKILL_PASSIVE");
            item.Servant.getClassSkillInfo(out numArray, out this.titleMessageList, out this.explanationMessageList);
            int num = 1;
            for (int i = 0; i < this.skillIconList.Length; i++)
            {
                if ((i < numArray.Length) && (numArray[i] > 0))
                {
                    num = i + 1;
                }
            }
            int num3 = this.skillPitch * num;
            Vector3 baseSize = this.baseSize;
            baseSize.y += num3;
            if (this.baseCollider != null)
            {
                this.baseCollider.size = baseSize;
            }
            this.baseSprite.width = (int) baseSize.x;
            this.baseSprite.height = (int) baseSize.y;
            Vector3 titleBasePosition = this.titleBasePosition;
            titleBasePosition.y += num3 / 2;
            this.titleSprite.transform.localPosition = titleBasePosition;
            Vector3 skillBasePosition = this.skillBasePosition;
            skillBasePosition.y += num3 / 2;
            for (int j = 0; j < this.skillBaseList.Length; j++)
            {
                if (j < num)
                {
                    this.skillBaseList[j].SetActive(true);
                    this.skillBaseList[j].transform.localPosition = skillBasePosition;
                    this.skillIconList[j].Set(numArray[j]);
                    this.skillTitleLabelList[j].text = this.titleMessageList[j];
                    WrapControlText.textAdjust(this.skillExplanationLabelList[j], this.explanationMessageList[j]);
                }
                else
                {
                    this.skillBaseList[j].SetActive(false);
                }
                skillBasePosition.y -= this.skillPitch;
            }
        }
    }
}

