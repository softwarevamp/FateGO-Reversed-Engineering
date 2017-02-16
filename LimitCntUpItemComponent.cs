using System;
using UnityEngine;

public class LimitCntUpItemComponent : MonoBehaviour
{
    private int currentItemId;
    private int haveItemNum;
    public UILabel haveNumLb;
    public UILabel haveTitleLb;
    private bool isItemNum;
    public ItemDetailInfoComponent itemDetailComp;
    public GameObject itemDetailInfo;
    private ItemEntity itemEnt;
    public ItemIconComponent itemIconComp;
    private int itemImgId;
    public GameObject itemInfo;
    private int needItemNum;
    public UILabel needNumLb;
    public UILabel needTitleLb;

    public bool checkItemNum() => 
        this.isItemNum;

    private void closeItemDetail(bool isDecide)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.itemDetailComp.Close();
    }

    public void enableDispItemInfo()
    {
        this.itemInfo.SetActive(false);
    }

    public int getItemId() => 
        this.currentItemId;

    public int getItemImgId() => 
        this.itemImgId;

    public void OnClickItem()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.itemDetailComp.OpenCombine(this.itemEnt, new ItemDetailInfoComponent.CallbackFunc(this.closeItemDetail));
    }

    public void setLimitUpItemInfo(long usrId, int itemId, int needNum, int idx)
    {
        this.haveItemNum = 0;
        this.needItemNum = needNum;
        long[] args = new long[] { usrId, (long) itemId };
        UserItemEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_ITEM).getEntityFromId<UserItemEntity>(args);
        if (entity != null)
        {
            this.haveItemNum = entity.num;
        }
        this.itemIconComp.SetCombineItem(itemId, -1);
        this.needTitleLb.text = "必要";
        this.haveTitleLb.text = "所持";
        this.haveNumLb.text = this.haveItemNum.ToString();
        this.needNumLb.text = this.needItemNum.ToString();
        if (this.haveItemNum >= this.needItemNum)
        {
            this.isItemNum = true;
            this.needTitleLb.color = Color.white;
            this.needNumLb.color = Color.white;
        }
        else
        {
            this.isItemNum = false;
            this.needTitleLb.color = new Color(0.855f, 0f, 0.32f);
            this.needNumLb.color = new Color(0.855f, 0f, 0.32f);
        }
        this.itemInfo.SetActive(true);
        this.currentItemId = itemId;
        this.itemEnt = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.currentItemId);
        this.itemImgId = this.itemEnt.imageId;
        Vector3 localPosition = this.itemDetailInfo.transform.localPosition;
        this.itemDetailInfo.transform.localPosition = new Vector3(-((135f * idx) - 135f), localPosition.y, localPosition.z);
    }
}

