using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class UserServantMaster : DataMasterBase
{
    public UserServantMaster()
    {
        base.cachename = DataNameKind.GetName(DataNameKind.Kind.USER_SERVANT);
        if (DataMasterBase._never)
        {
            Debug.Log(string.Empty + new UserServantEntity[1]);
        }
    }

    public bool CheckEquipAdd(int count)
    {
        int num;
        int num2;
        this.getCount(out num, out num2);
        num2 += count;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        return (num2 > entity.svtEquipKeep);
    }

    public bool CheckServantAdd(int count)
    {
        int num;
        int num2;
        this.getCount(out num, out num2);
        num += count;
        UserGameEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
        return (num > entity.svtKeep);
    }

    public bool CheckServantAndEquipAdd(int servantCount, int equipCount) => 
        (this.CheckServantAdd(servantCount) || this.CheckEquipAdd(equipCount));

    public void continueDeviceUserServant()
    {
        List<long> list = new List<long>();
        int count = base.list.Count;
        long userId = NetworkManager.UserId;
        for (int i = 0; i < count; i++)
        {
            UserServantEntity entity = base.list[i] as UserServantEntity;
            if (((entity != null) && (entity.userId == userId)) && !entity.IsWithdrawal())
            {
                list.Add(entity.id);
            }
        }
        UserServantNewManager.SetOld(list.ToArray());
    }

    public UserServantEntity[] getCombineMaterialList()
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        long userId = NetworkManager.UserId;
        int count = base.list.Count;
        List<UserServantEntity> list = new List<UserServantEntity>();
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && (item.userId == userId)) && (!item.IsWithdrawal() && master.getEntityFromId<ServantEntity>(item.svtId).IsKeepServant))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public int getCount(out int servantSum, out int servantEquipSum)
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        long userId = NetworkManager.UserId;
        int count = base.list.Count;
        int num3 = 0;
        servantSum = 0;
        servantEquipSum = 0;
        for (int i = 0; i < count; i++)
        {
            UserServantEntity entity = base.list[i] as UserServantEntity;
            if (((entity != null) && (entity.userId == userId)) && !entity.IsWithdrawal())
            {
                num3++;
                ServantEntity entity2 = master.getEntityFromId<ServantEntity>(entity.svtId);
                if (entity2 != null)
                {
                    SvtType.Type type = (SvtType.Type) entity2.type;
                    if (SvtType.IsKeepServant(type))
                    {
                        servantSum++;
                    }
                    else if (SvtType.IsKeepServantEquip(type))
                    {
                        servantEquipSum++;
                    }
                }
            }
        }
        return num3;
    }

    public UserServantEntity getHeroineData(int heroineId)
    {
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantEntity entity = base.list[i] as UserServantEntity;
            if (((entity != null) && (entity.svtId == heroineId)) && entity.IsHeroine())
            {
                return entity;
            }
        }
        return null;
    }

    public UserServantEntity[] getKeepServantEquipList()
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        long userId = NetworkManager.UserId;
        int count = base.list.Count;
        List<UserServantEntity> list = new List<UserServantEntity>();
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && (item.userId == userId)) && (!item.IsWithdrawal() && master.getEntityFromId<ServantEntity>(item.svtId).IsKeepServantEquip))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public UserServantEntity[] getKeepServantList()
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        long userId = NetworkManager.UserId;
        int count = base.list.Count;
        List<UserServantEntity> list = new List<UserServantEntity>();
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && (item.userId == userId)) && (!item.IsWithdrawal() && master.getEntityFromId<ServantEntity>(item.svtId).IsKeepServant))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public UserServantEntity[] getList() => 
        this.getList(NetworkManager.UserId);

    public UserServantEntity[] getList(long userId)
    {
        int count = base.list.Count;
        List<UserServantEntity> list = new List<UserServantEntity>();
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && (item.userId == userId)) && !item.IsWithdrawal())
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public override DataEntityBase[] getList(object obj) => 
        JsonManager.DeserializeArray<UserServantEntity>(obj);

    public UserServantEntity[] getNoneCombineSvt(long userId)
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        int count = base.list.Count;
        List<UserServantEntity> list = new List<UserServantEntity>();
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && (item.userId == userId)) && (!item.IsWithdrawal() && (master.getEntityFromId<ServantEntity>(item.svtId).type != 3)))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public UserServantEntity[] getNpUpServantList(UserServantEntity usrSvtEnt)
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        List<UserServantEntity> list = new List<UserServantEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && !item.IsWithdrawal()) && (master.getEntityFromId<ServantEntity>(item.svtId).baseSvtId == usrSvtEnt.svtId))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public UserServantEntity[] getOrganizationList()
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        long userId = NetworkManager.UserId;
        int count = base.list.Count;
        List<UserServantEntity> list = new List<UserServantEntity>();
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && (item.userId == userId)) && (!item.IsWithdrawal() && master.getEntityFromId<ServantEntity>(item.svtId).IsOrganization))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public UserServantEntity[] getSameServantList(UserServantEntity usrSvtEnt)
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        List<UserServantEntity> list = new List<UserServantEntity>();
        int count = base.list.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && !item.IsWithdrawal()) && ((item.id != usrSvtEnt.id) && (master.getEntityFromId<ServantEntity>(item.svtId).baseSvtId == usrSvtEnt.svtId)))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }

    public UserServantEntity[] getServantEquipList()
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        long userId = NetworkManager.UserId;
        int count = base.list.Count;
        List<UserServantEntity> list = new List<UserServantEntity>();
        for (int i = 0; i < count; i++)
        {
            UserServantEntity item = base.list[i] as UserServantEntity;
            if (((item != null) && (item.userId == userId)) && (!item.IsWithdrawal() && master.getEntityFromId<ServantEntity>(item.svtId).IsServantEquip))
            {
                list.Add(item);
            }
        }
        return list.ToArray();
    }
}

