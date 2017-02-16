using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BattleResultEventItemComponent : MonoBehaviour
{
    [CompilerGenerated]
    private static Predicate<BattleDropItem> <>f__am$cache10;
    public UILabel atLabel;
    public UISprite eventIconSprite;
    private int itemCount;
    public ItemDetailInfoComponent itemDialog;
    public BattleViewItemlistComponent itemWindow;
    public PlayMakerFSM myFsm;
    private List<BattleDropItem> newDroplist;
    public UILabel nextItemLabel;
    public BattleResultComponent parentComp;
    public UILabel titleAtLabel;
    public UILabel titleConfLabel;
    public UILabel titleNextItemLabel;
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

    public BattleDropItem getNewDrop()
    {
        if ((this.newDroplist != null) && (0 < this.newDroplist.Count))
        {
            BattleDropItem item = this.newDroplist[0];
            this.newDroplist.RemoveAt(0);
            return item;
        }
        return null;
    }

    public void Init()
    {
        this.window.setInitData(BattleWindowComponent.ACTIONTYPE.ALPHA, 0.3f, false);
        this.window.setClose();
        this.itemDialog.Init();
        string str = LocalizationManager.Get("BATTLE_RESULEVENTTITEM_TITLECONF");
        if (!str.Equals("BATTLE_RESULEVENTTITEM_TITLECONF"))
        {
            this.titleConfLabel.text = str;
        }
        this.titleAtLabel.text = LocalizationManager.Get("BATTLE_RESULEVENTTITEM_AT_POINT");
        this.titleNextItemLabel.text = LocalizationManager.Get("BATTLE_RESULEVENTTITEM_AT_ITEM");
        this.itemCount = 0;
        this.touchNextRoot.SetActive(false);
    }

    public bool isGetItems() => 
        (0 < this.itemCount);

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

    public void setResultData(BattleDropItem[] drop, int eventId)
    {
        if (drop == null)
        {
            this.itemCount = 0;
        }
        List<BattleDropItem> list = new List<BattleDropItem>();
        list.AddRange(drop);
        this.itemWindow.setListData(list.ToArray(), new BattleDropItemComponent.ClickDelegate(this.setShowConf), 0);
        this.itemWindow.setHide();
        this.itemCount = list.Count;
        if (<>f__am$cache10 == null)
        {
            <>f__am$cache10 = s => s.isNew && Gift.IsServant(s.type);
        }
        this.newDroplist = list.FindAll(<>f__am$cache10);
        UserGameEntity entity = UserGameMaster.getSelfUserGame();
        long[] args = new long[] { entity.userId, (long) eventId };
        UserEventEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEventMaster>(DataNameKind.Kind.USER_EVENT).getEntityFromId<UserEventEntity>(args);
        if (entity2 != null)
        {
            EventRewardEntity nextEventRewardEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventRewardMaster>(DataNameKind.Kind.EVENT_REWARD).GetNextEventRewardEntity(eventId, entity2.value);
            EventDetailEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventDetailMaster>(DataNameKind.Kind.EVENT_DETAIL).getEntityFromId<EventDetailEntity>(eventId);
            AtlasManager.SetItem(this.eventIconSprite, entity4.pointImageId);
            if (nextEventRewardEntity != null)
            {
                string str;
                string str2;
                nextEventRewardEntity.GetInfo(out str, out str2);
                this.atLabel.text = string.Format(LocalizationManager.Get("BATTLE_RESULEVENTTITEM_NEXT_POINT"), nextEventRewardEntity.point - entity2.value);
                if (nextEventRewardEntity.isQp())
                {
                    this.nextItemLabel.text = string.Format(LocalizationManager.Get("BATTLE_RESULEVENTTITEM_NEXT_ITEM"), string.Empty, str2);
                }
                else
                {
                    this.nextItemLabel.text = string.Format(LocalizationManager.Get("BATTLE_RESULEVENTTITEM_NEXT_ITEM"), str, str2);
                }
            }
            else
            {
                this.atLabel.text = LocalizationManager.Get("BATTLE_RESULEVENTTITEM_AT_POINT_NONE");
                this.nextItemLabel.text = LocalizationManager.Get("BATTLE_RESULEVENTTITEM_AT_ITEM_NONE");
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

