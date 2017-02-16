using System;
using UnityEngine;

public class PartyOrganizationConfirmItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UISprite baseSprite;
    [SerializeField]
    protected UILabel[] commandLabelList;
    [SerializeField]
    protected UISprite memberTypeBaseSprite;
    [SerializeField]
    protected UISprite memberTypeSprite;
    [SerializeField]
    protected ServantFaceIconComponent servantFaceIcon;
    [SerializeField]
    protected UISprite supportSprite;
    [SerializeField]
    protected UISprite typeSprite;

    public void SetInput(PartyOrganizationListViewItem item, bool isInput)
    {
    }

    public void SetItem(PartyOrganizationListViewItem item, DispMode mode)
    {
        if ((item != null) && (mode != DispMode.INVISIBLE))
        {
            int[] commandCardList = null;
            string str = null;
            if (!item.IsFollower)
            {
                this.typeSprite.gameObject.SetActive(false);
                if (item.UserServant != null)
                {
                    this.servantFaceIcon.Set(item.UserServant, item.GetEquipList(), null);
                    commandCardList = item.GetCommandCardList();
                    if (item.UserServant.IsEventJoin())
                    {
                        this.supportSprite.spriteName = "icon_eventjoin_02";
                    }
                    else
                    {
                        this.supportSprite.spriteName = null;
                    }
                }
                else
                {
                    this.servantFaceIcon.Clear();
                    str = "formation_blank_small";
                    this.supportSprite.spriteName = null;
                }
            }
            else if (item.FollowerData == null)
            {
                this.servantFaceIcon.Clear();
                this.typeSprite.gameObject.SetActive(false);
                str = "formation_support_small";
                this.supportSprite.spriteName = null;
            }
            else
            {
                this.servantFaceIcon.Set(item.ServantLeader, null, false);
                switch (item.FollowerData.type)
                {
                    case 1:
                        this.typeSprite.gameObject.SetActive(true);
                        this.typeSprite.spriteName = "icon_friend";
                        this.typeSprite.MakePixelPerfect();
                        break;

                    case 3:
                        this.typeSprite.gameObject.SetActive(true);
                        this.typeSprite.spriteName = "icon_support_01";
                        this.typeSprite.MakePixelPerfect();
                        break;

                    default:
                        this.typeSprite.gameObject.SetActive(false);
                        break;
                }
                commandCardList = item.GetCommandCardList();
                if (item.IsFollower)
                {
                    this.supportSprite.spriteName = "icon_support_02";
                }
                else
                {
                    this.supportSprite.spriteName = null;
                }
            }
            for (int i = 0; i < this.commandLabelList.Length; i++)
            {
                this.commandLabelList[i].text = (commandCardList == null) ? string.Empty : (string.Empty + commandCardList[i]);
            }
            this.baseSprite.spriteName = str;
            if (item.Index == 0)
            {
                this.memberTypeBaseSprite.spriteName = "formation_txtbg_01";
            }
            else if (item.Index < BalanceConfig.DeckMainMemberMax)
            {
                this.memberTypeBaseSprite.spriteName = "formation_txtbg_02";
            }
            else
            {
                this.memberTypeBaseSprite.spriteName = "formation_txtbg_03";
            }
            this.memberTypeSprite.spriteName = "member_txt_" + (item.Index + 1);
            this.memberTypeSprite.MakePixelPerfect();
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

