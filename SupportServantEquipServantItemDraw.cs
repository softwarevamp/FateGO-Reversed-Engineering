using System;
using UnityEngine;

public class SupportServantEquipServantItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UIIconLabel attackIconLabel;
    [SerializeField]
    protected UICommonButton decideButton;
    [SerializeField]
    protected UIIconLabel differAttackIconLabel;
    [SerializeField]
    protected UIIconLabel differHpIconLabel;
    [SerializeField]
    protected UICommonButton equipButton;
    [SerializeField]
    protected GameObject equipIconBase;
    [SerializeField]
    protected UISprite equipLimitCountSprite;
    [SerializeField]
    protected UIMeshSprite equipSprite;
    [SerializeField]
    protected UIIconLabel hpIconLabel;
    [SerializeField]
    protected UISprite noneEquipSprite;
    [SerializeField]
    protected ServantFaceIconComponent servantEquipFaceIcon;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UILabel skillExplanationLabel;
    [SerializeField]
    protected UILabel skillNameLabel;

    public void SetInput(bool isInput)
    {
        this.decideButton.GetComponent<Collider>().enabled = isInput;
        this.decideButton.SetState(UICommonButtonColor.State.Normal, true);
    }

    public void SetItem(SupportServantEquipServantItem item)
    {
        if (item.UserServant != null)
        {
            this.servantFaceIcon.FaceSpriteDisp(true);
            this.servantFaceIcon.Set(item.UserServant, item.IconInfo);
            if (this.hpIconLabel != null)
            {
                this.hpIconLabel.gameObject.SetActive(true);
                this.hpIconLabel.Set(IconLabelInfo.IconKind.HP, item.Hp, item.AdjustHp, item.EquipHp, 0L, false, false);
            }
            if (this.attackIconLabel != null)
            {
                this.attackIconLabel.gameObject.SetActive(true);
                this.attackIconLabel.Set(IconLabelInfo.IconKind.ATK, item.Atk, item.AdjustAtk, item.EquipAtk, 0L, false, false);
            }
        }
        else
        {
            this.servantFaceIcon.FaceSpriteDisp(false);
            this.hpIconLabel.gameObject.SetActive(false);
            this.attackIconLabel.gameObject.SetActive(false);
        }
        if (this.differHpIconLabel != null)
        {
            this.differHpIconLabel.gameObject.SetActive(true);
            this.differHpIconLabel.Set(IconLabelInfo.IconKind.DIFFER_HP, item.EquipHp, 0, 0, 0L, false, false);
        }
        if (this.differAttackIconLabel != null)
        {
            this.differAttackIconLabel.gameObject.SetActive(true);
            this.differAttackIconLabel.Set(IconLabelInfo.IconKind.DIFFER_ATK, item.EquipAtk, 0, 0, 0L, false, false);
        }
        if (item.EquipUserServant != null)
        {
            int[] numArray;
            int[] numArray2;
            int[] numArray3;
            string[] strArray;
            string[] strArray2;
            this.servantEquipFaceIcon.Set(item.EquipUserServant, item.IconInfo);
            item.EquipUserServant.getSkillInfo(out numArray, out numArray2, out numArray3, out strArray, out strArray2);
            this.skillNameLabel.text = strArray[0];
            WrapControlText.textAdjust(this.skillExplanationLabel, strArray2[0]);
            this.noneEquipSprite.gameObject.SetActive(false);
            this.equipSprite.gameObject.SetActive(true);
            AtlasManager.SetEquipFace(this.equipSprite, item.EquipUserServant.svtId);
            if (this.equipLimitCountSprite != null)
            {
                int equipLimitCountMax = item.EquipLimitCountMax;
                this.equipLimitCountSprite.gameObject.SetActive((equipLimitCountMax > 0) && (item.EquipLimitCount >= equipLimitCountMax));
            }
        }
        else
        {
            this.servantEquipFaceIcon.NoMount();
            this.skillNameLabel.text = string.Empty;
            this.skillExplanationLabel.text = string.Empty;
            this.noneEquipSprite.gameObject.SetActive(true);
            this.equipSprite.gameObject.SetActive(false);
            if (this.equipLimitCountSprite != null)
            {
                this.equipLimitCountSprite.gameObject.SetActive(false);
            }
        }
        if (this.equipButton != null)
        {
            bool isEnable = item.EquipUserServant != null;
            this.equipButton.SetColliderEnable(isEnable, true);
        }
    }

    public enum DispMode
    {
        INVISIBLE,
        INVALID,
        VALID,
        INPUT
    }
}

