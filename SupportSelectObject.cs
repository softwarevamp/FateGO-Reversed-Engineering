using System;
using System.Runtime.CompilerServices;

public class SupportSelectObject : BaseMonoBehaviour
{
    protected int classPos;
    protected SupportSelectItemDraw itemDraw;
    protected SupportServantData supportServantData;

    protected event CallbackFunc callbackFunc;

    protected void Awake()
    {
        this.itemDraw = base.GetComponent<SupportSelectItemDraw>();
    }

    public void ClearItem()
    {
        this.supportServantData = null;
        this.classPos = 0;
        this.callbackFunc = null;
        if (this.itemDraw != null)
        {
            this.itemDraw.ClearItem();
        }
    }

    protected void EndShowEquip(bool isDecide)
    {
        if (isDecide)
        {
            this.ModifyItem();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantEquipStatusDialog(null);
    }

    protected void EndShowServant(bool isDecide)
    {
        if (isDecide)
        {
            this.ModifyItem();
        }
        SingletonMonoBehaviour<CommonUI>.Instance.CloseServantStatusDialog(null);
    }

    public void ModifyItem()
    {
        if ((this.itemDraw != null) && (this.supportServantData != null))
        {
            this.itemDraw.SetItem(this.supportServantData, this.classPos, SupportSelectItemDraw.DispMode.VALID);
        }
    }

    public void OnClickItem()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ResultKind.SELECT_SERVANT, this.classPos);
        }
    }

    public void OnClickItemEquip()
    {
        if (this.callbackFunc != null)
        {
            this.callbackFunc(ResultKind.SELECT_EQUIP, this.classPos);
        }
    }

    public void OnLongPressItem()
    {
        if (this.callbackFunc != null)
        {
            if (this.supportServantData.IsFriendInfo)
            {
                ServantLeaderInfo servantLeaderInfo = this.supportServantData.getUserServantLearderEntity(this.classPos).getServantLeaderInfo();
                if ((servantLeaderInfo != null) && (servantLeaderInfo.svtId != 0))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(this.supportServantData.Kind, servantLeaderInfo, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                }
            }
            else
            {
                UserServantEntity userSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.supportServantData.getServant(this.classPos));
                if ((userSvtEntity != null) && (userSvtEntity.svtId != 0))
                {
                    SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantStatusDialog(this.supportServantData.Kind, userSvtEntity, new ServantStatusDialog.ClickDelegate(this.EndShowServant));
                }
            }
        }
    }

    public void OnLongPressItemEquip()
    {
        if (this.callbackFunc != null)
        {
            long userSvtId = this.supportServantData.getEquip(this.classPos);
            if (userSvtId != 0)
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
                if (this.supportServantData.IsFriendInfo)
                {
                    ServantStatusDialog.Kind kind;
                    UserServantLearderEntity entity = this.supportServantData.getUserServantLearderEntity(this.classPos);
                    if (this.supportServantData.Kind == ServantStatusDialog.Kind.FOLLOWER)
                    {
                        kind = ServantStatusDialog.Kind.FOLLOWER_SERVANT_EQUIP;
                    }
                    else
                    {
                        kind = ServantStatusDialog.Kind.FRIEND_SERVANT_EQUIP;
                    }
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(kind, entity.equipTarget1, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
                }
                else
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenServantEquipStatusDialog(ServantStatusDialog.Kind.SERVANT_EQUIP, userSvtId, true, new ServantStatusDialog.ClickDelegate(this.EndShowEquip));
                }
            }
            else
            {
                SoundManager.playSystemSe(SeManager.SystemSeKind.WARNING);
            }
        }
    }

    public void SetItem(SupportServantData supportServantData, int classPos, CallbackFunc callback)
    {
        this.supportServantData = supportServantData;
        this.classPos = classPos;
        this.callbackFunc = callback;
        if (this.itemDraw != null)
        {
            this.itemDraw.SetItem(supportServantData, this.classPos, SupportSelectItemDraw.DispMode.VALID);
        }
    }

    public delegate void CallbackFunc(SupportSelectObject.ResultKind result, int n);

    public enum ResultKind
    {
        NONE,
        SELECT_SERVANT,
        SELECT_EQUIP
    }
}

