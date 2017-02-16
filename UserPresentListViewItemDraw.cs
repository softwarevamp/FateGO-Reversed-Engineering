using System;
using UnityEngine;

public class UserPresentListViewItemDraw : MonoBehaviour
{
    [SerializeField]
    protected UIButton baseButton;
    [SerializeField]
    protected GameObject blockObj;
    [SerializeField]
    protected UILabel checkCntLabel;
    [SerializeField]
    protected GameObject checkObj;
    [SerializeField]
    protected UISprite frameImageSprite;
    [SerializeField]
    protected UILabel holdNumCntLb;
    [SerializeField]
    protected UILabel holdNumTitleLb;
    [SerializeField]
    protected UISprite iconImageSprite;
    [SerializeField]
    protected ItemIconComponent itemIcon;
    [SerializeField]
    protected UILabel msgTextLabel;
    [SerializeField]
    protected UILabel nameTextLabel;
    [SerializeField]
    protected UILabel resTimeLb;

    public bool checkHoldDisp(UserPresentListViewItem item)
    {
        if (item.Type != Gift.Type.ITEM)
        {
            return false;
        }
        ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
        if (master.isQP(item.ItemEntity.id))
        {
            return false;
        }
        if (master.isFriendPoint(item.ItemEntity.id))
        {
            return false;
        }
        return true;
    }

    public int getHoldCount(UserPresentListViewItem item)
    {
        if (SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).isMana(item.ItemEntity.id))
        {
            return SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).mana;
        }
        if (item.ItemEntity.type == 2)
        {
            return SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME).stone;
        }
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserItemMaster>(DataNameKind.Kind.USER_ITEM).getEntityFromId(NetworkManager.UserId, item.ItemEntity.id).num;
    }

    public void SetBlocked(bool val)
    {
        this.blockObj.SetActive(val);
    }

    public void SetCheck(bool val)
    {
        this.checkObj.SetActive(val);
    }

    public void SetCheckCnt(int count)
    {
        this.checkCntLabel.text = string.Empty + count;
    }

    public void SetInput(UserPresentListViewItem item, bool isInput)
    {
        if (this.baseButton != null)
        {
            Debug.Log("UserPresentListViewItemDraw SetInput: " + isInput);
            this.baseButton.GetComponent<Collider>().enabled = isInput;
        }
    }

    public void SetItem(UserPresentListViewItem item, DispMode mode)
    {
        if (item == null)
        {
            mode = DispMode.INVISIBLE;
        }
        if (mode != DispMode.INVISIBLE)
        {
            this.itemIcon.SetGift(item.Type, item.PresentObjId, -1);
            string str = string.Format(LocalizationManager.Get("PRESENT_INFO"), item.NameText, item.NumText);
            this.nameTextLabel.text = str;
            this.msgTextLabel.text = item.MsgText;
            this.resTimeLb.gameObject.SetActive(false);
            this.resTimeLb.text = LocalizationManager.Get("TIME_REST_PRESENT") + LocalizationManager.GetRestTime(item.UserPresentEntity.expireAt());
            if (this.checkHoldDisp(item))
            {
                if (this.holdNumTitleLb != null)
                {
                    this.holdNumTitleLb.gameObject.SetActive(true);
                    this.holdNumTitleLb.text = LocalizationManager.Get("SHOP_BUY_ITEM_HOLD");
                }
                if (this.holdNumCntLb != null)
                {
                    this.holdNumCntLb.gameObject.SetActive(true);
                    this.holdNumCntLb.text = LocalizationManager.GetNumberFormat(this.getHoldCount(item));
                }
            }
            else
            {
                if (this.holdNumTitleLb != null)
                {
                    this.holdNumTitleLb.gameObject.SetActive(false);
                }
                if (this.holdNumCntLb != null)
                {
                    this.holdNumCntLb.gameObject.SetActive(false);
                }
            }
            this.SetCheck(item.checkBoxed);
            this.SetCheckCnt(item.checkCount);
            this.SetBlocked(item.blocked);
            Debug.Log("UserPresentListViewItemDraw SetItem: " + item.checkBoxed);
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

