using System;

public class BalanceConfig
{
    public static string AprilFoolAssetStorageDateVersion = "20160401_";
    public static int CommandSpellMax;
    public static int CommandSpellRecoverCost;
    public static int DailyFreeGachaResetTime;
    public static int DeckMainMemberMax = 3;
    public static int DeckMax;
    public static int DeckMemberMax = 6;
    public static int FollowerPointFriend;
    public static int FollowerPointNotFriend;
    public static int FollowerPointNpc;
    public static int FriendPointMax;
    public static int GameDataResetTime;
    public static bool IsIOS_Examination;
    public static int LvExceedItemId = 0x1f3f;
    public static int LvExceedNeedItemNum = 1;
    public static int ManaMax;
    public static int OtherImageLimitCount = 0x3e8;
    public static int PresentBoxCheckMax;
    public static int PresentBoxMax;
    public static int PresentBoxValidTime = 0x1e13380;
    public static int QpMax;
    public static int RequestTopHomeExpirationDateSec = 600;
    public static int RequestTopLoginDay;
    public static int RequestTopLoginResetTime1;
    public static int RequestTopLoginResetTime2;
    public static int SameClassExp;
    public static float SameClassMultiExp;
    public static int SerialCodeMenuDispFlg;
    public static int ServantCombineMax = 5;
    public static int ServantEquipFrameMax;
    public static int ServantEquipFrameUseStone = 5;
    public static int ServantFrameMax;
    public static int ServantFrameUseStone = 5;
    public static int ServantIdHyde;
    public static int ServantIdJekyll;
    public static int ServantIdMashu1;
    public static int ServantIdMashu2;
    public static int ServantLimitMax = 4;
    public static int ServantSellSelectMax = 0x63;
    public static long ServerTimeOverLimit = 0xe10L;
    public static int SpendApRecvItemNum = 1;
    public static int StatusUpAdjustAtk;
    public static int StatusUpAdjustHp;
    public static int StoneMax;
    public static int SupportDeckMax = 8;
    public static int SvtEquipMax = 1;
    public static int SvtEquipSkillListMax = 1;
    public static int SvtSkillListMax = 3;
    public static int UerGameActRecoverCost;
    public static string UsePolicyVersion;
    public static int UserEquipSkillListMax = 3;
    public static int UserEventItemMax = 0x3b9ac9ff;
    public static int UserGameActRecoverMenuDispFlg;
    public static int UserItemMax;
    public static int UserLevelMax;
    public static int UserPointEventMax = 0x3b9ac9ff;
    public static long UserSuperBossDamagePointMax = 0xe8d4a50fffL;
    public static long VtReleaseAt;

    public static void Initialize()
    {
        ConstantMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ConstantMaster>(DataNameKind.Kind.CONSTANT);
        ConstantStrMaster master2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ConstantStrMaster>(DataNameKind.Kind.CONSTANT_STR);
        QpMax = master.GetValue("MAX_QP");
        ManaMax = master.GetValue("MAX_MANA");
        StoneMax = master.GetValue("MAX_STONE");
        FriendPointMax = master.GetValue("MAX_FRIENDPOINT");
        UserLevelMax = master.GetValue("MAX_USER_LV");
        UserItemMax = master.GetValue("MAX_USER_ITEM");
        UserEventItemMax = master.GetValue("MAX_USER_ITEM");
        UserPointEventMax = master.GetValue("MAX_EVENT_POINT");
        PresentBoxMax = master.GetValue("MAX_PRESENT_BOX_NUM");
        PresentBoxCheckMax = master.GetValue("MAX_PRESENT_RECEIVE_NUM");
        ServantFrameMax = master.GetValue("MAX_USER_SVT");
        ServantEquipFrameMax = master.GetValue("MAX_USER_SVT_EQUIP");
        DeckMax = master.GetValue("DECK_MAX");
        UerGameActRecoverCost = master.GetValue("ONE_ACT");
        FollowerPointFriend = master.GetValue("FRIEND_POINT");
        FollowerPointNotFriend = master.GetValue("NOT_FRIEND_POINT");
        FollowerPointNpc = master.GetValue("NPC_FRIEND_POINT");
        RequestTopLoginResetTime1 = master.GetValue("LOGIN_RESET_AT");
        RequestTopLoginResetTime2 = master.GetValue("CAMPAIGN_RESET_AT");
        DailyFreeGachaResetTime = master.GetValue("FREE_GACHA_RESET_AT");
        GameDataResetTime = master.GetValue("GAMEDATA_RESET_AT");
        RequestTopLoginDay = master.GetValue("LOGIN_DAY");
        ServerTimeOverLimit = master.GetValue("NEED_REBOOT_TIME");
        if (ServerTimeOverLimit < ManagerConfig.SERVER_TIME_OVER_LIMIT)
        {
            ServerTimeOverLimit = ManagerConfig.SERVER_TIME_OVER_LIMIT;
        }
        CommandSpellMax = master.GetValue("MAX_COMMAND_SPELL");
        CommandSpellRecoverCost = master.GetValue("ONE_COMMAND_SPELL");
        UsePolicyVersion = master2.GetValue("USE_POLICY_VERSION");
        IsIOS_Examination = master.GetValue("IS_IOS_EXAMINATION") != 0;
        SerialCodeMenuDispFlg = master.GetValue("ENABLE_SERIAL_CODE");
        UserGameActRecoverMenuDispFlg = master.GetValue("ENABLE_AP_RECOVER");
        StatusUpAdjustAtk = master.GetValue("STATUS_UP_ADJUST_ATK");
        StatusUpAdjustHp = master.GetValue("STATUS_UP_ADJUST_HP");
        SameClassMultiExp = ConstantMaster.getRateValue("SAME_CLASS_MULI_EXP");
        SameClassExp = master.GetValue("SAME_CLASS_MULI_EXP");
        ServantIdJekyll = master.GetValue("JEKYLL_SVT_ID");
        ServantIdHyde = master.GetValue("HYDE_SVT_ID");
        ServantIdMashu1 = master.GetValue("MASHU_SVT_ID1");
        ServantIdMashu2 = master.GetValue("MASHU_SVT_ID2");
        VtReleaseAt = master.GetValue("VALENTINE_RELEASE_AT");
        OtherImageLimitCount = master.GetValue("OTHER_IMAGE_LIMIT_COUNT");
    }
}

