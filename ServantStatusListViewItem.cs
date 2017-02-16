using System;
using System.Runtime.InteropServices;

public class ServantStatusListViewItem
{
    protected int cardImageLimitCount;
    protected int collectionAtk;
    protected int collectionHp;
    protected int collectionLv;
    protected int commandCardLimitCount;
    protected int dispLimitCount;
    protected long[] equipIdList;
    protected ServantEntity equipServantEntity;
    protected ServantLimitEntity equipSvtLimitEntity;
    protected long equipTargetId1;
    protected EquipTargetInfo equipTargetInfo;
    protected UserServantEntity equipUserSvtEntity;
    protected int faceLimitCount;
    protected long favoriteUserSvtId;
    protected bool isCardReal;
    protected bool isCollection;
    protected bool isEquipChangeMode;
    protected bool isEquipShowMode;
    protected bool isLock;
    protected bool isTdResult;
    protected bool isUse;
    protected int maxCardImageLimitCount;
    protected int maxCommandCardLimitCount;
    protected int maxDispLimitCount;
    protected int maxFaceLimitCount;
    protected PartyOrganizationListViewItem memberItem;
    protected int memberNum;
    protected long oldEquipTargetId1;
    protected PartyListViewItem partyItem;
    protected ServantLeaderInfo servantLeaderInfo;
    protected ServantCommentEntity[] svtCommentEntitiyList;
    protected ServantEntity svtEntity;
    protected ServantExpEntity svtExpEntity;
    protected ServantLimitEntity svtLimitEntity;
    protected int tdCardId;
    protected string tdExplanation;
    protected int tdGuageCount;
    protected int tdId;
    protected int tdLv;
    protected int tdMaxLv;
    protected int tdMaxRank;
    protected string tdName;
    protected int tdRank;
    protected UserGameEntity userGameEntity;
    protected UserServantCollectionEntity userSvtCollectionEntity;
    protected UserServantEntity userSvtEntity;

    public ServantStatusListViewItem(EquipTargetInfo equipTargetInfo)
    {
        this.userGameEntity = null;
        this.userSvtEntity = null;
        this.equipIdList = null;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = equipTargetInfo;
        this.isEquipShowMode = false;
        this.isEquipChangeMode = false;
        this.svtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.equipTargetInfo.svtId);
        this.svtLimitEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.equipTargetInfo.svtId, this.equipTargetInfo.limitCount);
        this.svtCommentEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetEntitiyList(this.svtEntity.id, CondType.Kind.NONE);
        this.isCardReal = false;
        this.cardImageLimitCount = this.maxCardImageLimitCount = 0;
        this.dispLimitCount = this.maxDispLimitCount = 0;
        this.commandCardLimitCount = this.maxCommandCardLimitCount = 0;
        this.faceLimitCount = this.maxFaceLimitCount = this.maxCardImageLimitCount;
        this.isLock = false;
        this.isUse = false;
        this.AnalyzeInfo();
        this.SetEquipTargetId1(0L);
        this.isCollection = false;
    }

    public ServantStatusListViewItem(ServantLeaderInfo servantLeaderInfo)
    {
        this.userGameEntity = null;
        this.userSvtEntity = null;
        this.equipIdList = null;
        this.userSvtCollectionEntity = null;
        this.servantLeaderInfo = servantLeaderInfo;
        this.equipTargetInfo = null;
        this.isEquipShowMode = true;
        this.isEquipChangeMode = false;
        this.svtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.servantLeaderInfo.svtId);
        this.svtLimitEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.servantLeaderInfo.svtId, this.servantLeaderInfo.limitCount);
        this.svtCommentEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetEntitiyList(this.svtEntity.id, CondType.Kind.NONE);
        this.isCardReal = false;
        this.cardImageLimitCount = ImageLimitCount.GetCardImageLimitCount(this.servantLeaderInfo.svtId, this.servantLeaderInfo.limitCount, false, this.isCardReal);
        this.maxCardImageLimitCount = ImageLimitCount.GetCardImageLimitCount(this.servantLeaderInfo.svtId, this.servantLeaderInfo.limitCount, false, true);
        this.dispLimitCount = this.maxDispLimitCount = ImageLimitCount.GetImageLimitCount(this.servantLeaderInfo.svtId, this.servantLeaderInfo.limitCount);
        this.commandCardLimitCount = this.maxCommandCardLimitCount = ImageLimitCount.GetImageLimitCount(this.servantLeaderInfo.svtId, this.servantLeaderInfo.limitCount);
        this.faceLimitCount = this.maxFaceLimitCount = this.maxCardImageLimitCount;
        this.isLock = false;
        this.isUse = false;
        this.AnalyzeInfo();
        this.SetEquipTargetId1(0L);
        this.isCollection = false;
        if ((this.servantLeaderInfo.equipTarget1 != null) && (this.servantLeaderInfo.equipTarget1.svtId > 0))
        {
            this.equipServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.servantLeaderInfo.equipTarget1.svtId);
            this.equipSvtLimitEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.servantLeaderInfo.equipTarget1.svtId, this.servantLeaderInfo.equipTarget1.limitCount);
        }
    }

    public ServantStatusListViewItem(UserServantCollectionEntity userSvtCollectionEntity)
    {
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
        this.isEquipShowMode = false;
        this.isEquipChangeMode = false;
        if (userSvtCollectionEntity.userId != this.userGameEntity.userId)
        {
            this.userGameEntity = null;
        }
        this.userSvtEntity = null;
        this.equipIdList = null;
        this.userSvtCollectionEntity = userSvtCollectionEntity;
        this.servantLeaderInfo = null;
        this.equipTargetInfo = null;
        this.svtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtCollectionEntity.svtId);
        this.svtLimitEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtCollectionEntity.svtId, this.userSvtCollectionEntity.maxLimitCount);
        this.svtCommentEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetEntitiyList(this.svtEntity.id, CondType.Kind.NONE);
        this.isCardReal = false;
        this.cardImageLimitCount = ImageLimitCount.GetCardImageLimitCount(this.userSvtCollectionEntity.svtId, this.userSvtCollectionEntity.maxLimitCount, true, this.isCardReal);
        this.maxCardImageLimitCount = ImageLimitCount.GetCardImageLimitCount(this.userSvtCollectionEntity.svtId, this.userSvtCollectionEntity.maxLimitCount, true, true);
        this.dispLimitCount = this.maxDispLimitCount = ImageLimitCount.GetImageLimitCount(this.userSvtCollectionEntity.svtId, this.userSvtCollectionEntity.maxLimitCount);
        this.commandCardLimitCount = this.maxCommandCardLimitCount = ImageLimitCount.GetImageLimitCount(this.userSvtCollectionEntity.svtId, this.userSvtCollectionEntity.maxLimitCount);
        this.faceLimitCount = this.maxFaceLimitCount = this.maxCardImageLimitCount;
        this.isLock = false;
        this.isUse = false;
        this.AnalyzeInfo();
        this.SetEquipTargetId1(0L);
        this.isCollection = true;
        this.userSvtCollectionEntity.getCollectionStatus(out this.collectionLv, out this.collectionHp, out this.collectionAtk);
    }

    public ServantStatusListViewItem(UserServantEntity userServant, bool isUse)
    {
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
        if (userServant.userId == this.userGameEntity.userId)
        {
            this.favoriteUserSvtId = this.userGameEntity.favoriteUserSvtId;
        }
        else
        {
            this.favoriteUserSvtId = -1L;
            this.userGameEntity = null;
        }
        this.userSvtEntity = userServant;
        this.equipIdList = null;
        this.isCardReal = false;
        this.cardImageLimitCount = this.userSvtEntity.getDispCardImageLimitCount(this.isCardReal);
        this.maxCardImageLimitCount = this.userSvtEntity.getMaxCardDispImageLimitCount();
        this.dispLimitCount = this.userSvtEntity.getDispLimitCount();
        this.maxDispLimitCount = this.userSvtEntity.getMaxDispLimitCount();
        this.commandCardLimitCount = this.userSvtEntity.getCommandCardLimitCount();
        this.maxCommandCardLimitCount = this.userSvtEntity.getMaxCommandCardLimitCount();
        this.faceLimitCount = this.userSvtEntity.getIconLimitCount();
        this.maxFaceLimitCount = this.userSvtEntity.getMaxFaceLimitCount();
        this.isEquipShowMode = false;
        this.isEquipChangeMode = false;
        if (NetworkManager.UserId == this.userSvtEntity.userId)
        {
            this.userSvtCollectionEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userSvtEntity.userId, this.userSvtEntity.svtId);
        }
        else
        {
            this.userSvtCollectionEntity = null;
        }
        this.servantLeaderInfo = null;
        this.equipTargetInfo = null;
        this.svtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtEntity.svtId);
        this.svtLimitEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtEntity.svtId, this.userSvtEntity.limitCount);
        this.svtCommentEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetEntitiyList(this.svtEntity.id, CondType.Kind.NONE);
        this.isLock = this.userSvtEntity.IsLock();
        this.isUse = isUse;
        this.AnalyzeInfo();
        this.SetEquipTargetId1(0L);
        this.isCollection = false;
    }

    public ServantStatusListViewItem(PartyListViewItem partyItem, int member, ServantStatusDialog.Kind kind)
    {
        this.partyItem = partyItem;
        this.memberItem = partyItem.GetMember(member);
        this.memberNum = member;
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
        this.favoriteUserSvtId = this.userGameEntity.favoriteUserSvtId;
        this.userSvtEntity = this.memberItem.UserServant;
        this.equipIdList = this.memberItem.GetEquipList();
        this.isCardReal = false;
        this.cardImageLimitCount = this.userSvtEntity.getDispCardImageLimitCount(this.isCardReal);
        this.maxCardImageLimitCount = this.userSvtEntity.getMaxCardDispImageLimitCount();
        this.dispLimitCount = this.userSvtEntity.getDispLimitCount();
        this.maxDispLimitCount = this.userSvtEntity.getMaxDispLimitCount();
        this.commandCardLimitCount = this.userSvtEntity.getCommandCardLimitCount();
        this.maxCommandCardLimitCount = this.userSvtEntity.getMaxCommandCardLimitCount();
        this.faceLimitCount = this.userSvtEntity.getIconLimitCount();
        this.maxFaceLimitCount = this.userSvtEntity.getMaxFaceLimitCount();
        this.isEquipShowMode = this.equipIdList != null;
        this.isEquipChangeMode = false;
        this.userSvtCollectionEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userSvtEntity.userId, this.userSvtEntity.svtId);
        this.svtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtEntity.svtId);
        this.svtLimitEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtEntity.svtId, this.userSvtEntity.limitCount);
        this.svtCommentEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetEntitiyList(this.svtEntity.id, CondType.Kind.NONE);
        this.isLock = this.userSvtEntity.IsLock();
        this.isUse = false;
        this.AnalyzeInfo();
        this.SetEquipTargetId1((this.equipIdList == null) ? 0L : this.equipIdList[0]);
        this.isCollection = false;
    }

    public ServantStatusListViewItem(UserServantEntity userServant, long[] equipIdList, ServantStatusDialog.Kind kind)
    {
        this.userGameEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_GAME).getSingleEntity<UserGameEntity>();
        if (userServant.userId == this.userGameEntity.userId)
        {
            this.favoriteUserSvtId = this.userGameEntity.favoriteUserSvtId;
        }
        else
        {
            this.favoriteUserSvtId = -1L;
            this.userGameEntity = null;
        }
        this.userSvtEntity = userServant;
        this.equipIdList = equipIdList;
        this.isCardReal = false;
        this.cardImageLimitCount = this.userSvtEntity.getDispCardImageLimitCount(this.isCardReal);
        this.maxCardImageLimitCount = this.userSvtEntity.getMaxCardDispImageLimitCount();
        this.dispLimitCount = this.userSvtEntity.getDispLimitCount();
        this.maxDispLimitCount = this.userSvtEntity.getMaxDispLimitCount();
        this.commandCardLimitCount = this.userSvtEntity.getCommandCardLimitCount();
        this.maxCommandCardLimitCount = this.userSvtEntity.getMaxCommandCardLimitCount();
        this.faceLimitCount = this.userSvtEntity.getIconLimitCount();
        this.maxFaceLimitCount = this.userSvtEntity.getMaxFaceLimitCount();
        this.isEquipShowMode = equipIdList != null;
        this.isEquipChangeMode = false;
        if (NetworkManager.UserId == this.userSvtEntity.userId)
        {
            this.userSvtCollectionEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(this.userSvtEntity.userId, this.userSvtEntity.svtId);
        }
        else
        {
            this.userSvtCollectionEntity = null;
        }
        this.servantLeaderInfo = null;
        this.equipTargetInfo = null;
        this.svtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.userSvtEntity.svtId);
        this.svtLimitEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.userSvtEntity.svtId, this.userSvtEntity.limitCount);
        this.svtCommentEntitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetEntitiyList(this.svtEntity.id, CondType.Kind.NONE);
        this.isLock = this.userSvtEntity.IsLock();
        this.isUse = this.svtEntity.IsServantEquip && SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserDeckMaster>(DataNameKind.Kind.USER_DECK).IsEquip(this.userSvtEntity.id);
        this.AnalyzeInfo();
        this.SetEquipTargetId1((this.equipIdList == null) ? 0L : this.equipIdList[0]);
        this.isCollection = false;
    }

    protected void AnalyzeInfo()
    {
        if (this.userSvtEntity != null)
        {
            this.isTdResult = this.userSvtEntity.getTreasureDeviceInfo(out this.tdId, out this.tdLv, out this.tdMaxLv, out this.tdRank, out this.tdMaxRank, out this.tdName, out this.tdExplanation, out this.tdGuageCount, out this.tdCardId);
        }
        else if (this.servantLeaderInfo != null)
        {
            this.isTdResult = this.servantLeaderInfo.getTreasureDeviceInfo(out this.tdId, out this.tdLv, out this.tdMaxLv, out this.tdRank, out this.tdMaxRank, out this.tdName, out this.tdExplanation, out this.tdGuageCount, out this.tdCardId);
        }
        else if (this.userSvtCollectionEntity != null)
        {
            this.isTdResult = this.userSvtCollectionEntity.getTreasureDeviceInfo(out this.tdId, out this.tdLv, out this.tdMaxLv, out this.tdRank, out this.tdMaxRank, out this.tdName, out this.tdExplanation, out this.tdGuageCount, out this.tdCardId, 1, -1);
        }
        else
        {
            this.isTdResult = false;
            this.tdId = 0;
            this.tdLv = 0;
            this.tdMaxLv = 0;
            this.tdRank = 0;
            this.tdMaxRank = 0;
            this.tdName = string.Empty;
            this.tdExplanation = string.Empty;
            this.tdGuageCount = 0;
            this.tdCardId = 0;
        }
    }

    public bool ChangeLock()
    {
        this.isLock = !this.isLock;
        return this.isLock;
    }

    ~ServantStatusListViewItem()
    {
    }

    public bool GetAdjustMax(out int maxAjustHp, out int maxAjustAtk)
    {
        if (this.userSvtEntity != null)
        {
            return this.userSvtEntity.GetAdjustMax(out maxAjustHp, out maxAjustAtk);
        }
        if (this.servantLeaderInfo != null)
        {
            return this.servantLeaderInfo.GetAdjustMax(out maxAjustHp, out maxAjustAtk);
        }
        maxAjustHp = 0;
        maxAjustAtk = 0;
        return false;
    }

    public bool GetEquipExpInfo(out int exp, out int lateExp, out float barExp)
    {
        if (this.equipUserSvtEntity != null)
        {
            return this.equipUserSvtEntity.getExpInfo(out exp, out lateExp, out barExp);
        }
        if (this.servantLeaderInfo != null)
        {
            return this.servantLeaderInfo.getEquipExpInfo(out exp, out lateExp, out barExp);
        }
        exp = lateExp = 0;
        barExp = 0f;
        return false;
    }

    public bool GetEquipSkillInfo(out int[] skillIdList, out int[] skillLvList, out int[] skillChargeList, out string[] skillTitleList, out string[] skillExplanationList)
    {
        if (this.userSvtEntity != null)
        {
            if (this.equipTargetId1 > 0L)
            {
                SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(this.equipTargetId1).getEquipSkillInfo(out skillIdList, out skillLvList, out skillChargeList, out skillTitleList, out skillExplanationList);
                return true;
            }
        }
        else if (this.servantLeaderInfo != null)
        {
            this.servantLeaderInfo.getEquipSkillInfo(out skillIdList, out skillLvList, out skillChargeList, out skillTitleList, out skillExplanationList);
            return true;
        }
        skillIdList = new int[BalanceConfig.SvtEquipSkillListMax];
        skillLvList = new int[BalanceConfig.SvtEquipSkillListMax];
        skillChargeList = new int[BalanceConfig.SvtEquipSkillListMax];
        skillTitleList = new string[BalanceConfig.SvtEquipSkillListMax];
        skillExplanationList = new string[BalanceConfig.SvtEquipSkillListMax];
        return false;
    }

    public bool GetExpInfo(out int exp, out int lateExp, out float barExp)
    {
        if (this.userSvtEntity != null)
        {
            return this.userSvtEntity.getExpInfo(out exp, out lateExp, out barExp);
        }
        exp = lateExp = 0;
        barExp = 0f;
        return false;
    }

    public bool GetFriendshipInfo(out int rank, out int max, out int late, out float fraction)
    {
        if (this.userSvtCollectionEntity != null)
        {
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FriendshipMaster>(DataNameKind.Kind.FRIENDSHIP).GetFriendshipRank(this.svtEntity.friendshipId, this.userSvtCollectionEntity.friendship, out rank, out max, out late, out fraction);
            return true;
        }
        rank = 0;
        max = 0;
        late = 0;
        fraction = 0f;
        return false;
    }

    public bool GetNpInfo(out int tdId, out int tdLv, out int tdMaxLv, out int tdRank, out int tdMaxRank, out string tdName, out string tdExplanation, out int tdGuageCount, out int tdCardId)
    {
        tdId = this.tdId;
        tdLv = this.tdLv;
        tdMaxLv = this.tdMaxLv;
        tdRank = this.tdRank;
        tdMaxRank = this.tdMaxRank;
        tdName = this.tdName;
        tdExplanation = this.tdExplanation;
        tdGuageCount = this.tdGuageCount;
        tdCardId = this.tdCardId;
        return this.isTdResult;
    }

    public bool GetSkillInfo(out int[] skillIdList, out int[] skillLvList, out int[] skillChargeList, out string[] skillTitleList, out string[] skillExplanationList)
    {
        if (this.userSvtEntity != null)
        {
            this.userSvtEntity.getSkillInfo(out skillIdList, out skillLvList, out skillChargeList, out skillTitleList, out skillExplanationList);
            return true;
        }
        if (this.servantLeaderInfo != null)
        {
            this.servantLeaderInfo.getSkillInfo(out skillIdList, out skillLvList, out skillChargeList, out skillTitleList, out skillExplanationList);
            return true;
        }
        if (this.equipTargetInfo != null)
        {
            this.equipTargetInfo.getSkillInfo(out skillIdList, out skillLvList, out skillChargeList, out skillTitleList, out skillExplanationList);
            return true;
        }
        if (this.userSvtCollectionEntity != null)
        {
            this.userSvtCollectionEntity.getSkillInfo(out skillIdList, out skillLvList, out skillChargeList, out skillTitleList, out skillExplanationList);
            return true;
        }
        skillIdList = new int[BalanceConfig.SvtSkillListMax];
        skillLvList = new int[BalanceConfig.SvtSkillListMax];
        skillChargeList = new int[BalanceConfig.SvtSkillListMax];
        skillTitleList = new string[BalanceConfig.SvtSkillListMax];
        skillExplanationList = new string[BalanceConfig.SvtSkillListMax];
        return false;
    }

    public bool GetVoiceInfo(out string illust, out string voice, out bool isPlayVoice)
    {
        illust = string.Empty;
        voice = string.Empty;
        isPlayVoice = false;
        if (this.svtEntity.illustratorId > 0)
        {
            IllustratorEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.ILLUSTRATOR).getEntityFromId<IllustratorEntity>(this.svtEntity.illustratorId);
            if (entity != null)
            {
                illust = entity.name;
            }
            else
            {
                illust = LocalizationManager.Get("UNKNOWN_NAME_ILLUST");
            }
        }
        else if (this.svtEntity.illustratorId == 0)
        {
            illust = LocalizationManager.Get("NO_ENTRY_NAME_ILLUST");
        }
        else
        {
            illust = LocalizationManager.Get("UNKNOWN_NAME_ILLUST");
        }
        if (this.svtEntity.cvId > 0)
        {
            CvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.CV).getEntityFromId<CvEntity>(this.svtEntity.cvId);
            if (entity2 != null)
            {
                voice = entity2.name;
                isPlayVoice = true;
            }
            else
            {
                voice = LocalizationManager.Get("UNKNOWN_NAME_CV");
            }
        }
        else if (this.svtEntity.cvId == 0)
        {
            voice = LocalizationManager.Get("NO_ENTRY_NAME_CV");
        }
        else
        {
            voice = LocalizationManager.Get("UNKNOWN_NAME_CV");
        }
        return true;
    }

    public bool IsModifyEquipId() => 
        (this.equipTargetId1 != this.oldEquipTargetId1);

    public bool IsModifyFavoriteUserSvtId() => 
        (((this.favoriteUserSvtId > 0L) && (this.userGameEntity != null)) && (this.userGameEntity.favoriteUserSvtId != this.favoriteUserSvtId));

    public void SetEquipTargetId1(long equipUserSvtId)
    {
        if ((this.userSvtEntity != null) && (equipUserSvtId > 0L))
        {
            this.equipUserSvtEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(equipUserSvtId);
            if (this.equipUserSvtEntity.svtId > 0)
            {
                if (this.equipIdList != null)
                {
                    this.equipIdList[0] = equipUserSvtId;
                }
                if (this.memberItem != null)
                {
                    this.memberItem.SetEquipUserServantId(equipUserSvtId);
                }
                this.equipTargetId1 = equipUserSvtId;
                this.equipServantEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(this.equipUserSvtEntity.svtId);
                this.equipSvtLimitEntity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.SERVANT_LIMIT).getEntityFromId<ServantLimitEntity>(this.equipUserSvtEntity.svtId, this.equipUserSvtEntity.limitCount);
                return;
            }
        }
        if (this.equipIdList != null)
        {
            this.equipIdList[0] = 0L;
        }
        if (this.memberItem != null)
        {
            this.memberItem.SetEquipUserServantId(0L);
        }
        this.equipTargetId1 = 0L;
        this.equipUserSvtEntity = null;
        this.equipServantEntity = null;
        this.equipSvtLimitEntity = null;
    }

    public int AdjustAtk
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.adjustAtk;
            }
            if (this.servantLeaderInfo != null)
            {
                return this.servantLeaderInfo.adjustAtk;
            }
            return 0;
        }
    }

    public int AdjustHp
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.adjustHp;
            }
            if (this.servantLeaderInfo != null)
            {
                return this.servantLeaderInfo.adjustHp;
            }
            return 0;
        }
    }

    public StatusRank.Kind Agility =>
        ((StatusRank.Kind) this.svtLimitEntity.agility);

    public int Atk
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.atk;
            }
            if (this.servantLeaderInfo != null)
            {
                return this.servantLeaderInfo.atk;
            }
            if (this.equipTargetInfo != null)
            {
                return this.equipTargetInfo.atk;
            }
            if (this.isCollection)
            {
                return this.collectionAtk;
            }
            return 0;
        }
    }

    public int CardImageLimitCount =>
        this.cardImageLimitCount;

    public int CommandCardLimitCount
    {
        get => 
            this.commandCardLimitCount;
        set
        {
            this.commandCardLimitCount = value;
        }
    }

    public int Cost =>
        this.svtEntity.cost;

    public StatusRank.Kind Defense =>
        ((StatusRank.Kind) this.svtLimitEntity.defense);

    public int DispLimitCount
    {
        get => 
            this.dispLimitCount;
        set
        {
            this.dispLimitCount = value;
        }
    }

    public int EquipAtk
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.atk;
            }
            if ((this.servantLeaderInfo != null) && (this.servantLeaderInfo.equipTarget1 != null))
            {
                return this.servantLeaderInfo.equipTarget1.atk;
            }
            return 0;
        }
    }

    public int EquipCost
    {
        get
        {
            if (this.equipServantEntity != null)
            {
                return this.equipServantEntity.cost;
            }
            return 0;
        }
    }

    public int EquipExp
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.exp;
            }
            if ((this.servantLeaderInfo != null) && (this.servantLeaderInfo.equipTarget1 != null))
            {
                return this.servantLeaderInfo.equipTarget1.exp;
            }
            return 0;
        }
    }

    public int EquipHp
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.hp;
            }
            if ((this.servantLeaderInfo != null) && (this.servantLeaderInfo.equipTarget1 != null))
            {
                return this.servantLeaderInfo.equipTarget1.hp;
            }
            return 0;
        }
    }

    public int EquipLevel
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.lv;
            }
            if ((this.servantLeaderInfo != null) && (this.servantLeaderInfo.equipTarget1 != null))
            {
                return this.servantLeaderInfo.equipTarget1.lv;
            }
            return 0;
        }
    }

    public int EquipLimitCount
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.limitCount;
            }
            if ((this.servantLeaderInfo != null) && (this.servantLeaderInfo.equipTarget1 != null))
            {
                return this.servantLeaderInfo.equipTarget1.limitCount;
            }
            return 0;
        }
    }

    public int EquipMaxLevel
    {
        get
        {
            if (this.equipUserSvtEntity != null)
            {
                return this.equipUserSvtEntity.getLevelMax();
            }
            if ((this.servantLeaderInfo != null) && (this.servantLeaderInfo.equipTarget1 != null))
            {
                return this.servantLeaderInfo.equipTarget1.getLevelMax();
            }
            return 0;
        }
    }

    public ServantEntity EquipServant =>
        this.equipServantEntity;

    public EquipTargetInfo EquipTargetData =>
        this.equipTargetInfo;

    public long EquipTargetId1 =>
        this.equipTargetId1;

    public UserServantEntity EquipUserServant =>
        this.equipUserSvtEntity;

    public int Exp
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.exp;
            }
            if (this.servantLeaderInfo != null)
            {
                return this.servantLeaderInfo.exp;
            }
            if (this.equipTargetInfo != null)
            {
                return this.equipTargetInfo.exp;
            }
            return 0;
        }
    }

    public int FaceLimitCount
    {
        get => 
            this.faceLimitCount;
        set
        {
            this.faceLimitCount = value;
        }
    }

    public long FavoriteUserSvtId
    {
        get => 
            this.favoriteUserSvtId;
        set
        {
            this.favoriteUserSvtId = value;
        }
    }

    public int Hp
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.hp;
            }
            if (this.servantLeaderInfo != null)
            {
                return this.servantLeaderInfo.hp;
            }
            if (this.equipTargetInfo != null)
            {
                return this.equipTargetInfo.hp;
            }
            if (this.isCollection)
            {
                return this.collectionHp;
            }
            return 0;
        }
    }

    public bool IsCardReal =>
        this.isCardReal;

    public bool IsChangeImageLimitCount =>
        ((this.userSvtEntity != null) && this.svtEntity.IsServant);

    public bool IsCollection =>
        this.isCollection;

    public bool IsEquip
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return (this.equipTargetId1 > 0L);
            }
            return ((this.servantLeaderInfo != null) && this.servantLeaderInfo.IsEquip);
        }
    }

    public bool IsEquipChangeMode =>
        this.isEquipChangeMode;

    public bool IsEquipShowMode =>
        this.isEquipShowMode;

    public bool IsEventJoin =>
        ((this.userSvtEntity != null) && this.userSvtEntity.IsEventJoin());

    public bool IsLock =>
        this.isLock;

    public bool IsModifyCommandCardLimitCount =>
        ((this.userSvtEntity != null) && this.userSvtEntity.IsModifyCommandCardLimitCount(this.commandCardLimitCount));

    public bool IsModifyDispLimitCount =>
        ((this.userSvtEntity != null) && this.userSvtEntity.IsModifyDispLimitCount(this.dispLimitCount));

    public bool IsModifyFaceLimitCount =>
        ((this.userSvtEntity != null) && this.userSvtEntity.IsModifyFaceLimitCount(this.faceLimitCount));

    public bool IsModifyLock =>
        ((this.userSvtEntity != null) && this.userSvtEntity.IsModifyLock(this.isLock));

    public bool IsUse =>
        this.isUse;

    public int Level
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.lv;
            }
            if (this.servantLeaderInfo != null)
            {
                return this.servantLeaderInfo.lv;
            }
            if (this.equipTargetInfo != null)
            {
                return this.equipTargetInfo.lv;
            }
            if (this.isCollection)
            {
                return this.collectionLv;
            }
            return 0;
        }
    }

    public int LimitCount
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.limitCount;
            }
            if (this.servantLeaderInfo != null)
            {
                return this.servantLeaderInfo.limitCount;
            }
            if (this.equipTargetInfo != null)
            {
                return this.equipTargetInfo.limitCount;
            }
            if (this.userSvtCollectionEntity != null)
            {
                return this.userSvtCollectionEntity.maxLimitCount;
            }
            return 0;
        }
    }

    public StatusRank.Kind Luck =>
        ((StatusRank.Kind) this.svtLimitEntity.luck);

    public StatusRank.Kind Magic =>
        ((StatusRank.Kind) this.svtLimitEntity.magic);

    public int MaxCardImageLimitCount =>
        this.maxCardImageLimitCount;

    public int MaxCommandCardLimitCount =>
        this.maxCommandCardLimitCount;

    public int MaxDispLimitCount =>
        this.maxDispLimitCount;

    public int MaxFaceLimitCount =>
        this.maxFaceLimitCount;

    public int MaxLevel
    {
        get
        {
            if (this.userSvtEntity != null)
            {
                return this.userSvtEntity.getLevelMax();
            }
            if (this.servantLeaderInfo != null)
            {
                return this.servantLeaderInfo.getLevelMax();
            }
            if (this.equipTargetInfo != null)
            {
                return this.equipTargetInfo.getLevelMax();
            }
            if (this.isCollection)
            {
                return this.collectionLv;
            }
            return 0;
        }
    }

    public int Member =>
        this.memberNum;

    public StatusRank.Kind Np =>
        ((StatusRank.Kind) this.svtLimitEntity.treasureDevice);

    public PartyListViewItem PartyItem =>
        this.partyItem;

    public StatusRank.Kind Power =>
        ((StatusRank.Kind) this.svtLimitEntity.power);

    public ServantEntity Servant =>
        this.svtEntity;

    public ServantCommentEntity[] ServantCommentDataList =>
        this.svtCommentEntitiyList;

    public ServantLeaderInfo ServantLeaderData =>
        this.servantLeaderInfo;

    public int SvtId
    {
        get
        {
            if (this.svtEntity != null)
            {
                return this.svtEntity.id;
            }
            return 0;
        }
    }

    public UserGameEntity UserGame =>
        this.userGameEntity;

    public UserServantEntity UserServant =>
        this.userSvtEntity;

    public UserServantCollectionEntity UserServantCollection =>
        this.userSvtCollectionEntity;
}

