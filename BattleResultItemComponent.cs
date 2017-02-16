using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleResultItemComponent : BaseMonoBehaviour
{
    public UISprite eventIconSprite;
    public UILabel getEventPointLabel;
    public UILabel getQpLabel;
    public ItemDetailInfoComponent itemDialog;
    public BattleViewItemlistComponent itemWindow;
    public PlayMakerFSM myFsm;
    public UILabel nowEventPointLabel;
    public UILabel nowQpLabel;
    public BattleResultComponent parentComp;
    public GameObject rootEventPoint;
    public UILabel titleConfLabel;
    private int tmp_itemId;
    private long tmp_userSvtId;
    public GameObject touchNextRoot;
    public BattleWindowComponent window;

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
        base.gameObject.SetActive(false);
        this.myFsm.SendEvent("END_PROC");
    }

    public void EndCloseDialogCallBack()
    {
        this.myFsm.SendEvent("CLOSE");
    }

    public void endItemDialogCallBack()
    {
        this.myFsm.SendEvent("CLOSE");
    }

    public void endOpen()
    {
        this.myFsm.SendEvent("NEXT");
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
        this.touchNextRoot.SetActive(false);
    }

    public void itemDialogCallBack(bool flg)
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        this.itemDialog.Close(new System.Action(this.endItemDialogCallBack));
    }

    public void Open()
    {
        this.touchNextRoot.SetActive(true);
        this.myFsm.SendEvent("END_OPEN");
        this.window.Open(new BattleWindowComponent.EndCall(this.endOpen));
    }

    public void setResultData(BattleDropItem[] drop, int getqp, UserGameEntity oldGame, int eventId, UserEventEntity[] oldUsrEvent)
    {
        List<BattleDropItem> list = new List<BattleDropItem>();
        list.AddRange(drop);
        if (0 < getqp)
        {
            BattleDropItem item = new BattleDropItem {
                type = 2,
                objectId = 5,
                num = getqp
            };
            list.Insert(0, item);
        }
        this.itemWindow.setListData(list.ToArray(), new BattleDropItemComponent.ClickDelegate(this.setShowConf), 0);
        this.itemWindow.setHide();
        ItemMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ItemMaster>(DataNameKind.Kind.ITEM);
        int num = 0;
        foreach (BattleDropItem item2 in list)
        {
            if (item2.type == 2)
            {
                ItemEntity entity = master.getEntityFromId<ItemEntity>(item2.objectId);
                if ((entity.type == 1) || (entity.type == 0x10))
                {
                    num += item2.num;
                }
            }
        }
        int num2 = oldGame.qp + num;
        if (num2 <= BalanceConfig.QpMax)
        {
            this.getQpLabel.text = $"+ {num:#,0}";
            this.nowQpLabel.text = $"{num2:#,0}";
        }
        else
        {
            this.getQpLabel.text = string.Format("已达上限", new object[0]);
            this.nowQpLabel.text = $"{BalanceConfig.QpMax:#,0}";
        }
        this.rootEventPoint.SetActive(false);
        if (0 < eventId)
        {
            EventEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(eventId);
            if ((entity2 != null) && entity2.isCheckEventDetail())
            {
                EventDetailMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL);
                long[] args = new long[] { (long) eventId };
                if (master2.isEntityExistsFromId(args))
                {
                    EventDetailEntity entity3 = master2.getEntityFromId<EventDetailEntity>(eventId);
                    if (entity3.isEventPoint)
                    {
                        UserGameEntity entity4 = UserGameMaster.getSelfUserGame();
                        long[] numArray2 = new long[] { entity4.userId, (long) eventId };
                        UserEventEntity entity5 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMaster>(DataNameKind.Kind.USER_EVENT).getEntityFromId<UserEventEntity>(numArray2);
                        if (entity5 != null)
                        {
                            this.getEventPointLabel.text = $"+ {entity5.value - oldUsrEvent[0].value:#,0}";
                            this.nowEventPointLabel.text = $"{entity5.value:#,0}";
                            AtlasManager.SetItem(this.eventIconSprite, entity3.pointImageId);
                            this.rootEventPoint.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void setShowConf(BattleDropItem item)
    {
        Gift.Type type = (Gift.Type) item.type;
        if (type.IsServant())
        {
            this.setShowServantConf(item.userSvtId);
        }
        else if (type == Gift.Type.ITEM)
        {
            this.setShowItemConf(item.objectId);
        }
    }

    public void setShowItemConf(int itemId)
    {
        this.tmp_itemId = itemId;
        this.myFsm.SendEvent("OPEN_ITEM");
    }

    public void setShowServantConf(long userSvtId)
    {
        this.tmp_userSvtId = userSvtId;
        this.myFsm.SendEvent("OPEN_SERVANT");
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

