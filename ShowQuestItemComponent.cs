using System;
using System.Collections.Generic;
using UnityEngine;

public class ShowQuestItemComponent : BaseMonoBehaviour
{
    public ItemDetailInfoComponent itemDialog;
    public BattleViewItemlistComponent itemWindow;
    public UILabel titleConfLabel;
    private int tmp_itemId;
    private long tmp_userSvtId;
    public BattleWindowComponent window;

    private bool CheckDataIsRepeat(GiftEntity giftEntity, List<BattleDropItem> list)
    {
        foreach (BattleDropItem item in list)
        {
            if ((giftEntity.objectId == item.objectId) && (giftEntity.type == item.type))
            {
                return true;
            }
        }
        return false;
    }

    public void Close()
    {
        this.itemWindow.setHide();
        this.window.Close(new BattleWindowComponent.EndCall(this.endClose));
    }

    public void DialogCallBack(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(new System.Action(this.EndCloseDialogCallBack));
    }

    public void endClose()
    {
        this.setWindowShowPonstion(false);
    }

    public void EndCloseDialogCallBack()
    {
    }

    public void endItemDialogCallBack()
    {
    }

    public void endOpen()
    {
        this.itemWindow.setShow();
    }

    public void Init()
    {
        this.window.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.window.setClose();
        this.itemDialog.Init();
        string str = LocalizationManager.Get("BATTLE_RESULTITEM_TITLECONF");
        if (!str.Equals("BATTLE_RESULTITEM_TITLECONF"))
        {
            this.titleConfLabel.text = str;
        }
    }

    public void itemDialogCallBack(bool flg)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.itemDialog.Close(new System.Action(this.endItemDialogCallBack));
    }

    public void Open()
    {
        this.setWindowShowPonstion(true);
        this.window.Open(new BattleWindowComponent.EndCall(this.endOpen));
    }

    public void setResultData(List<GiftEntity> itemList)
    {
        List<BattleDropItem> list = new List<BattleDropItem>();
        foreach (GiftEntity entity in itemList)
        {
            if (!this.CheckDataIsRepeat(entity, list))
            {
                BattleDropItem item = new BattleDropItem {
                    type = entity.type,
                    objectId = entity.objectId,
                    num = 1
                };
                list.Add(item);
            }
        }
        this.itemWindow.setListData(list.ToArray(), new BattleDropItemComponent.ClickDelegate(this.setShowConf), 0);
        this.itemWindow.setHide();
    }

    public void setShowConf(BattleDropItem item)
    {
        switch (((Gift.Type) item.type))
        {
            case Gift.Type.SERVANT:
                this.setShowServantConf(item.userSvtId);
                break;

            case Gift.Type.ITEM:
                this.setShowItemConf(item.objectId);
                break;
        }
    }

    public void setShowItemConf(int itemId)
    {
        this.tmp_itemId = itemId;
        this.showItemDialog();
    }

    public void setShowServantConf(long userSvtId)
    {
        this.tmp_userSvtId = userSvtId;
        this.showServantDialog();
    }

    private void setWindowShowPonstion(bool isShow)
    {
        base.gameObject.transform.localPosition = new Vector3(!isShow ? ((float) 0x2710) : ((float) (-250)), 185f, 0f);
    }

    public void showItemDialog()
    {
        ItemEntity itemData = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM).getEntityFromId<ItemEntity>(this.tmp_itemId);
        this.itemDialog.Open(itemData, new ItemDetailInfoComponent.CallbackFunc(this.itemDialogCallBack));
    }

    public void showServantDialog()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(ServantStatusDialog.Kind.NORMAL, this.tmp_userSvtId, new ServantStatusDialog.ClickDelegate(this.DialogCallBack));
    }
}

