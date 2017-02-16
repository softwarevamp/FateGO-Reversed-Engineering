using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BattleResultComponent : BaseMonoBehaviour
{
    private resultData battleResult;
    public BattleResultBondsComponent bondsResult;
    public GameObject endtargetObject;
    private string eventEndMessage;
    private string eventEndTitle;
    public BattleResultEventItemComponent eventItemResult;
    public BattleResultExpComponent expResult;
    public GameObject FriendIconPrefab;
    public BattleResultFriendComponent friendResult;
    public PlayMakerFSM fsm;
    public BattleResultItemComponent itemResult;
    public GameObject obj_basebg;
    public GameObject obj_fronttouch;
    public UISprite resultSprite;
    public BattleResultType resultType;
    private GameObject ServantRewardActionObject;
    public GameObject ServantRewardActionPrefab;

    public void checkNew()
    {
        BattleDropItem item = this.eventItemResult.getNewDrop();
        if (item != null)
        {
            this.openNewServantView(item.userSvtId, Gift.IsEventSvtGet(item.type), new System.Action(this.endNewView));
            this.fsm.SendEvent("OPEN");
        }
        else
        {
            if (this.ServantRewardActionObject != null)
            {
                UnityEngine.Object.DestroyImmediate(this.ServantRewardActionObject);
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0f, null);
            }
            this.fsm.SendEvent("NEXT");
        }
    }

    public void checkStart()
    {
        if (this.bondsResult.isCollectsSvt())
        {
            this.fsm.SendEvent("START_Bonds");
        }
        else
        {
            this.fsm.SendEvent("START_Master");
        }
    }

    public void endCloseEndEventMessage(bool flg)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
    }

    public void endNewView()
    {
        this.fsm.SendEvent("END_PROC");
    }

    public void EndResult()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.endtargetObject.SendMessage("sendFsmEvent", "END_PROC");
    }

    public resultData getBattleResult() => 
        this.battleResult;

    public void Init()
    {
        base.gameObject.transform.localPosition = Vector3.zero;
        this.obj_basebg.SetActive(false);
        this.obj_fronttouch.SetActive(false);
        this.bondsResult.Init();
        this.expResult.Init();
        this.itemResult.Init();
        this.eventItemResult.Init();
        this.friendResult.Init();
        if (this.ServantRewardActionObject != null)
        {
            UnityEngine.Object.Destroy(this.ServantRewardActionObject);
        }
        this.resultSprite.gameObject.SetActive(true);
    }

    public void OpenBonds()
    {
        this.setTouch(false);
        this.obj_basebg.SetActive(true);
        this.bondsResult.Open();
    }

    public void OpenEventItems()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.resultSprite.gameObject.SetActive(false);
        this.setTouch(false);
        this.obj_basebg.SetActive(true);
        if (this.eventItemResult.isGetItems())
        {
            this.eventItemResult.Open();
        }
        else
        {
            this.fsm.SendEvent("SKIP");
        }
    }

    public void OpenExp()
    {
        this.setTouch(false);
        this.obj_basebg.SetActive(true);
        this.expResult.Open();
    }

    public void OpenFriend()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.CloseNotificationDialog();
        this.setTouch(false);
        this.friendResult.Open();
    }

    public void OpenItems()
    {
        if ((this.eventEndTitle != null) && (0 < this.eventEndTitle.Length))
        {
            SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(this.eventEndTitle, this.eventEndMessage, new NotificationDialog.ClickDelegate(this.endCloseEndEventMessage), -1);
        }
        this.resultSprite.gameObject.SetActive(false);
        this.setTouch(false);
        this.obj_basebg.SetActive(true);
        this.itemResult.Open();
    }

    public void openNewServantView(long userSvtId, bool isEventSvtGet, System.Action action)
    {
        UserServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(userSvtId);
        if (this.ServantRewardActionObject == null)
        {
            this.ServantRewardActionObject = base.createObject(this.ServantRewardActionPrefab, base.transform, null);
        }
        ServantRewardAction component = this.ServantRewardActionObject.GetComponent<ServantRewardAction>();
        if (isEventSvtGet)
        {
            component.Setup(entity.svtId, entity.id, entity.limitCount, 1, true, ServantRewardAction.PLAY_FLAG.EVENT_SVT_GET | ServantRewardAction.PLAY_FLAG.FADE_IN);
        }
        else
        {
            component.Setup(entity.svtId, entity.id, entity.limitCount, 1, true, ServantRewardAction.PLAY_FLAG.FADE_IN);
        }
        component.Play(action, 0f);
    }

    public void openTouchWait()
    {
        this.setTouch(true);
    }

    public void Set(string jsonstr)
    {
        Debug.Log("jsonstr:" + jsonstr);
        resultData[] dataArray = JsonManager.DeserializeArray<resultData>("[" + jsonstr + "]");
        this.battleResult = dataArray[0];
        if (this.battleResult.isWin())
        {
            this.battleResult.setDefaultDispFlag();
        }
        UserServantCollectionEntity[] entityArray = new UserServantCollectionEntity[dataArray[0].oldUserSvtCollection.Length];
        int index = 0;
        foreach (UserServantCollectionEntity entity in dataArray[0].oldUserSvtCollection)
        {
            UserServantCollectionEntity e = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, entity.svtId);
            entityArray[index] = new UserServantCollectionEntity(e);
            entityArray[index].friendship = entity.friendship;
            entityArray[index].friendshipRank = entity.friendshipRank;
            index++;
        }
        this.eventEndTitle = dataArray[0].eventEndTitle;
        this.eventEndMessage = dataArray[0].eventEndMessage;
        if ((this.battleResult.myDeck != null) && (this.battleResult.myDeck.svts != null))
        {
            this.bondsResult.setResultData(dataArray[0].myDeck, dataArray[0].oldUserSvtCollection);
        }
        else
        {
            this.battleResult.disableResultDispFlag(resultData.ResultDispFlagEnum.BONDS);
        }
        if (((this.battleResult.oldUserGame != null) && (this.battleResult.oldUserGame.Length > 0)) && ((this.battleResult.oldUserEquip != null) && (this.battleResult.oldUserEquip.Length > 0)))
        {
            this.expResult.setResultData(dataArray[0].oldUserGame[0], dataArray[0].oldUserEquip[0]);
        }
        else
        {
            this.battleResult.disableResultDispFlag(resultData.ResultDispFlagEnum.EXP);
        }
        if ((this.battleResult.oldUserGame != null) && (this.battleResult.oldUserGame.Length > 0))
        {
            this.itemResult.setResultData(dataArray[0].resultDropInfos, dataArray[0].phaseClearQp, dataArray[0].oldUserGame[0], dataArray[0].eventId, dataArray[0].oldUserEvent);
        }
        else
        {
            this.battleResult.disableResultDispFlag(resultData.ResultDispFlagEnum.ITEM);
        }
        this.friendResult.setResultData(dataArray[0].followerType, dataArray[0].followerStatus, dataArray[0].followerId, dataArray[0].followerClassId);
        this.eventItemResult.setResultData(dataArray[0].resultEventRewardInfos, dataArray[0].eventId);
        this.battleResult.enableResultDispFlag(resultData.ResultDispFlagEnum.TUTORIAL);
        TerminalPramsManager.mQuestClearHeroineInfo = null;
        UserServantEntity[] oldUserSvt = dataArray[0].oldUserSvt;
        if ((oldUserSvt != null) && (oldUserSvt.Length > 0))
        {
            UserServantEntity entity3 = oldUserSvt[0];
            UserServantEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData(DataNameKind.Kind.USER_SERVANT).getEntityFromId<UserServantEntity>(entity3.id);
            UserServantEntity entity5 = new UserServantEntity(entity4) {
                limitCount = entity3.limitCount,
                lv = entity3.lv
            };
            UserServantCollectionEntity entity6 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getEntityFromId(NetworkManager.UserId, entity5.svtId);
            UserServantCollectionEntity entity7 = null;
            foreach (UserServantCollectionEntity entity8 in entityArray)
            {
                if ((entity8.getUserId() == NetworkManager.UserId) && (entity8.getSvtId() == entity5.svtId))
                {
                    entity7 = entity8;
                    break;
                }
            }
            QuestClearHeroineInfo info = new QuestClearHeroineInfo {
                oldUsrSvtData = entity5,
                isChangeLimitcnt = entity5.getLimitCount() != entity4.getLimitCount(),
                isChangeTreasureDvc = false,
                treasureDvcId = 0,
                treasureDvcLv = 0,
                oldFriendShipRank = -1
            };
            if ((entity7 != null) && (entity7.getFriendShipRank() != entity6.getFriendShipRank()))
            {
                info.oldFriendShipRank = entity7.getFriendShipRank();
            }
            TerminalPramsManager.mQuestClearHeroineInfo = info;
        }
        UserQuestEntity entity9 = dataArray[0].oldUserQuest[0];
        TerminalPramsManager.IsPhaseClear = entity9 != null;
        TerminalPramsManager.IsQuestClear = false;
        if (TerminalPramsManager.IsPhaseClear)
        {
            int num4 = entity9.getQuestId();
            foreach (UserQuestEntity entity10 in SingletonMonoBehaviour<DataManager>.Instance.getEntitys<UserQuestEntity>(DataNameKind.Kind.USER_QUEST))
            {
                if (((entity10.getUserId() == entity9.getUserId()) && (entity10.getQuestId() == num4)) && ((entity9.getClearNum() == 0) && (entity10.getClearNum() == 1)))
                {
                    TerminalPramsManager.IsQuestClear = true;
                    break;
                }
            }
        }
        if (TerminalPramsManager.IsQuestClear)
        {
            int num6 = entity9.getQuestId();
            TerminalPramsManager.IsWarClear = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getByLastQuestId(num6) != null;
        }
        int num7 = entity9.getQuestId();
        TerminalPramsManager.mQuestRewardInfos = null;
        QuestRewardInfo[] rewardInfos = dataArray[0].rewardInfos;
        if ((rewardInfos != null) && (rewardInfos.Length > 0))
        {
            TerminalPramsManager.mQuestRewardInfos = rewardInfos;
        }
        TerminalPramsManager.mQuestClearReward_Skill = null;
        ServantSkillEntity[] entityArray7 = SingletonMonoBehaviour<DataManager>.Instance.getEntitys<ServantSkillEntity>(DataNameKind.Kind.SERVANT_SKILL);
        List<ServantSkillEntity> list = new List<ServantSkillEntity>();
        foreach (ServantSkillEntity entity12 in entityArray7)
        {
            if ((entity12.condQuestId > 0) && (entity12.condQuestId == num7))
            {
                list.Add(entity12);
            }
        }
        if (list.Count > 0)
        {
            TerminalPramsManager.mQuestClearReward_Skill = list;
        }
        TerminalPramsManager.mQuestClearReward_Treasure = null;
        ServantTreasureDvcEntity[] entityArray9 = SingletonMonoBehaviour<DataManager>.Instance.getEntitys<ServantTreasureDvcEntity>(DataNameKind.Kind.SERVANT_TREASUREDEVICE);
        List<ServantTreasureDvcEntity> list2 = new List<ServantTreasureDvcEntity>();
        foreach (ServantTreasureDvcEntity entity13 in entityArray9)
        {
            if ((entity13.condQuestId > 0) && (entity13.condQuestId == num7))
            {
                list2.Add(entity13);
            }
        }
        if (list2.Count > 0)
        {
            TerminalPramsManager.mQuestClearReward_Treasure = list2;
        }
    }

    public void setTouch(bool flg)
    {
        this.obj_fronttouch.SetActive(flg);
    }

    public void StartResult(GameObject target, string endEvent, BattleResultType resultType, BattlePerformance perf = null)
    {
        if (perf != null)
        {
            this.fsm.Fsm.GetFsmGameObject("Performance").Value = perf.gameObject;
        }
        this.resultType = resultType;
        this.fsm.SendEvent("OpenResult");
    }

    public enum BattleResultType
    {
        BATTLE_DROP = 2,
        BATTLE_NORMAL = 1
    }

    public class resultData
    {
        public int battleId;
        public int battleResult;
        public string eventEndMessage;
        public string eventEndTitle;
        public int eventId;
        public int followerClassId;
        public long followerId;
        public int followerStatus;
        public int followerType;
        public DeckData myDeck;
        public UserEquipEntity[] oldUserEquip;
        public UserEventEntity[] oldUserEvent;
        public UserGameEntity[] oldUserGame;
        public UserQuestEntity[] oldUserQuest;
        public UserServantEntity[] oldUserSvt;
        public UserServantCollectionEntity[] oldUserSvtCollection;
        public int phaseClearQp;
        public int resultDispFlag;
        public BattleDropItem[] resultDropInfos;
        public BattleDropItem[] resultEventRewardInfos;
        public QuestRewardInfo[] rewardInfos;

        public bool checkResultDispFlag(ResultDispFlagEnum flag) => 
            ((flag & this.resultDispFlag) != ((ResultDispFlagEnum) 0));

        public void disableResultDispFlag(ResultDispFlagEnum flag)
        {
            this.resultDispFlag &= ~flag;
        }

        public void enableResultDispFlag(ResultDispFlagEnum flag)
        {
            this.resultDispFlag |= flag;
        }

        public bool isWin() => 
            (this.battleResult == 1);

        public void setDefaultDispFlag()
        {
            this.resultDispFlag |= 0x3f;
        }

        public enum ResultDispFlagEnum
        {
            BONDS = 1,
            DAMAGE_ITEM = 0x10,
            EVENT_ITEM = 8,
            EXP = 2,
            FRIEND = 0x20,
            FRIEND_POINT = 0x80,
            ITEM = 4,
            RESERVE_3 = 0x200,
            RESERVE_4 = 0x400,
            RESERVE_5 = 0x800,
            SUPER_BOSS = 0x100,
            TUTORIAL = 0x40
        }
    }
}

