using System;
using UnityEngine;

public class MyRoomData : MonoBehaviour
{
    private MstProfileData mstInfoData;
    private UserGameEntity usrData;
    private UserDeckEntity usrDeckData;
    private UserDeckMaster usrDeckMst;

    public bool checkUserBirthDay()
    {
        DateTime time = NetworkManager.getLocalDateTime();
        DateTime time2 = NetworkManager.getDateTime(this.usrData.birthDay);
        int month = time.Month;
        int day = time.Day;
        int num3 = time2.Month;
        int num4 = time2.Day;
        return ((month == num3) && (day == num4));
    }

    public MstProfileData getMstInfoData() => 
        this.mstInfoData;

    public UserPresentBoxEntity[] getPresentList() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserPresentBoxMaster>(DataNameKind.Kind.USER_PRESENT_BOX).getVaildList(this.usrData.userId);

    public ServantEntity getSvtData(int svtId)
    {
        ServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT) as ServantMaster;
        return master?.getEntityFromId<ServantEntity>(svtId);
    }

    public int getSvtFriendshipLv(int hSvtId) => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.usrData.userId, hSvtId).friendshipRank;

    public ServantLimitEntity getSvtLimitData(int svtId, int limitCnt)
    {
        ServantLimitMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT) as ServantLimitMaster;
        return master?.getEntityFromId<ServantLimitEntity>(svtId, limitCnt);
    }

    private int getUserFriendSum() => 
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TblFriendMaser>(DataNameKind.Kind.TBL_FRIEND).GetFriendSum();

    public UserGameEntity getUsrData() => 
        this.usrData;

    public UserDeckEntity getUsrDeckData(long mainDeckId)
    {
        this.usrDeckMst = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK);
        if (this.usrDeckMst == null)
        {
            return null;
        }
        this.usrDeckData = this.usrDeckMst.getEntityFromId<UserDeckEntity>(this.usrData.mainDeckId);
        return this.usrDeckData;
    }

    private UserExpEntity getUsrNextExpData(int currentLv)
    {
        UserExpMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_EXP) as UserExpMaster;
        return master.getEntityFromLevel(currentLv + 1);
    }

    public UserServantEntity getUsrSvtData(long usrSvtId)
    {
        UserServantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT) as UserServantMaster;
        return master?.getEntityFromId<UserServantEntity>(usrSvtId);
    }

    private int[] getUsrSvtNum()
    {
        int num;
        int num2;
        int[] numArray = new int[2];
        SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getCount(out num, out num2);
        numArray[0] = num;
        numArray[1] = num2;
        return numArray;
    }

    public void initMyRoomData()
    {
        this.setUserInfoData();
    }

    public void setUserInfoData()
    {
        int num;
        int num2;
        float num3;
        this.setUsrData();
        this.mstInfoData = new MstProfileData();
        this.mstInfoData.userName = this.usrData.name;
        this.mstInfoData.genderType = this.usrData.genderType;
        this.mstInfoData.userEquipId = this.usrData.userEquipId;
        this.mstInfoData.userLv = this.usrData.lv;
        this.mstInfoData.birthDayVal = this.usrData.birthDay;
        if (this.usrData.getExpInfo(out num, out num2, out num3))
        {
            this.mstInfoData.exp = num;
            this.mstInfoData.lateExp = num2;
            this.mstInfoData.barExp = num3;
        }
        this.mstInfoData.friendPoint = this.usrData.GetFriendPoint();
        this.mstInfoData.currentFriendNum = this.getUserFriendSum();
        this.mstInfoData.maxFriendNum = this.usrData.friendKeep;
        int[] numArray = this.getUsrSvtNum();
        this.mstInfoData.currentSvtNum = numArray[0];
        this.mstInfoData.maxSvtNum = this.usrData.svtKeep;
        this.mstInfoData.currentSvtEpNum = numArray[1];
        this.mstInfoData.maxSvtEqNum = this.usrData.svtEquipKeep;
        this.mstInfoData.friendCode = this.usrData.friendCode;
        this.mstInfoData.currentQp = this.usrData.qp;
        this.mstInfoData.currentMana = this.usrData.mana;
        this.mstInfoData.currentStone = this.usrData.stone;
    }

    private void setUsrData()
    {
        this.usrData = SingletonMonoBehaviour<DataManager>.Instance.getSingleEntity<UserGameEntity>(DataNameKind.Kind.USER_GAME);
    }
}

