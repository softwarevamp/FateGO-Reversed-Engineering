using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class MyRoomStateMaterial : IState<MyRoomControl>
{
    [CompilerGenerated]
    private static Converter<DataEntityBase, ShopScriptEntity> <>f__am$cache10;
    [CompilerGenerated]
    private static Comparison<ShopScriptEntity> <>f__am$cache11;
    [CompilerGenerated]
    private static System.Action <>f__am$cache12;
    public static readonly int DEFAULT_FONT_SIZE = 0x1c;
    public static readonly int DEFAULT_MIN_FONT_SIZE = 20;
    private GameObject GachaBg;
    protected static readonly string GachaBgPath = "Bg/10500";
    private List<List<MaterialEventLogListViewItem.Info>> mEventItemInfs;
    private List<List<MaterialEventLogListViewItem.Info>> mFreeItemInfs;
    private CStateManager<MyRoomStateMaterial> mFSM;
    private MaterialEventLogListViewItem.Info mItemInfo_PlayQuestTalk;
    private MaterialEventLogListViewManager mListViewManager;
    private List<List<MaterialEventLogListViewItem.Info>> mMapItemInfs;
    private MyRoomControl mMyRoomControl;
    private STATE mOldState;
    private PlayMakerFSM mPlayMakerFSM;
    private string[] mScriptFileListStrs;
    private List<MaterialEventLogListViewItem.Info> mSelectInfs;
    private List<List<MaterialEventLogListViewItem.Info>> mStoryItemInfs;

    public bool Back()
    {
        if (this.GetState() == STATE.TOP)
        {
            return false;
        }
        if (this.mListViewManager.IsInput)
        {
            switch (this.GetState())
            {
                case STATE.TOP:
                    return false;

                case STATE.MAP:
                case STATE.STORY:
                case STATE.EVENT:
                case STATE.FREE:
                    this.FrameOut(delegate {
                        this.SetState(STATE.TOP);
                    });
                    break;

                case STATE.QUEST:
                    this.FrameOut(delegate {
                        this.SetState(this.mOldState);
                    });
                    break;
            }
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        }
        return true;
    }

    public void begin(MyRoomControl that)
    {
        this.SetState(STATE.TOP);
    }

    public void end(MyRoomControl that)
    {
        if (<>f__am$cache12 == null)
        {
            <>f__am$cache12 = delegate {
            };
        }
        this.FrameOut(<>f__am$cache12);
    }

    private void FrameOut(System.Action end_act)
    {
        this.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.EXIT, end_act);
    }

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public void Init(MyRoomControl that)
    {
        <Init>c__AnonStorey8D storeyd = new <Init>c__AnonStorey8D {
            that = that,
            <>f__this = this
        };
        this.mMyRoomControl = storeyd.that;
        this.mPlayMakerFSM = this.mMyRoomControl.myRoomFsm;
        this.mListViewManager = this.mMyRoomControl.GetMaterialEventLogListViewManager();
        this.mScriptFileListStrs = this.mMyRoomControl.GetScriptFileListStrs();
        List<clsMapCtrl_WarInfo> list = SingletonTemplate<QuestTree>.Instance.GetWarInfoAll_OrderReverse();
        List<clsMapCtrl_QuestInfo> list2 = SingletonTemplate<QuestTree>.Instance.mfGetQuestInfoListP();
        this.mMapItemInfs = new List<List<MaterialEventLogListViewItem.Info>>();
        foreach (clsMapCtrl_WarInfo info in list)
        {
            int num = info.mfGetWarID();
            if (((info.GetEventId() <= 0) && (num != WarEntity.CALDEAGATE_ID)) && (info.mfGetStatus() != clsMapCtrl_WarInfo.enStatus.None))
            {
                List<MaterialEventLogListViewItem.Info> item = new List<MaterialEventLogListViewItem.Info>();
                if ((num != ConstantMaster.getValue("FIRST_WAR_ID")) && (info.mfGetStatus() != clsMapCtrl_WarInfo.enStatus.New))
                {
                    MaterialEventLogListViewItem.Info info2 = new MaterialEventLogListViewItem.Info {
                        str = LocalizationManager.Get("MATERIAL_MAP_AVANT_TITLE"),
                        war_id = num,
                        on_click_act = new Action<MaterialEventLogListViewItem>(storeyd.<>m__12D)
                    };
                    item.Add(info2);
                }
                for (int i = 0; i < list2.Count; i++)
                {
                    clsMapCtrl_QuestInfo info3 = list2[i];
                    if ((info3.IsClear() && (info3.mfGetWarID() == num)) && (info3.mfGetQuestType() == QuestEntity.enType.MAIN))
                    {
                        int num3 = info3.mfGetQuestID();
                        if (num3 != ConstantMaster.getValue("TUTORIAL_QUEST_ID1"))
                        {
                            MaterialEventLogListViewItem.Info inf = new MaterialEventLogListViewItem.Info {
                                str = string.Format(LocalizationManager.Get("MATERIAL_MAP_QUEST_TITLE"), info3.mfGetMine().getChapterSubId(), info3.mfGetMine().getQuestName()),
                                war_id = num,
                                quest_id = num3,
                                phase_max = info3.mfGetPhaseMax()
                            };
                            if (this.IsExistScriptFile(inf))
                            {
                                inf.on_click_act = new Action<MaterialEventLogListViewItem>(storeyd.<>m__12E);
                                item.Add(inf);
                            }
                        }
                    }
                }
                if (item.Count > 0)
                {
                    this.mMapItemInfs.Add(item);
                }
            }
        }
        UserServantCollectionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).getList(CollectionStatus.Kind.GET);
        this.mStoryItemInfs = new List<List<MaterialEventLogListViewItem.Info>>();
        foreach (UserServantCollectionEntity entity in entityArray)
        {
            int id = entity.getSvtId();
            ServantEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(id);
            List<MaterialEventLogListViewItem.Info> list4 = new List<MaterialEventLogListViewItem.Info>();
            MaterialEventLogListViewItem.Info info5 = new MaterialEventLogListViewItem.Info();
            if (!entity2.checkIsHeroineSvt())
            {
                info5.str = LocalizationManager.Get("MATERIAL_STORY_SUMMON");
                info5.svt_id = id;
                info5.on_click_act = new Action<MaterialEventLogListViewItem>(storeyd.<>m__12F);
                list4.Add(info5);
            }
            for (int j = 0; j < list2.Count; j++)
            {
                clsMapCtrl_QuestInfo info6 = list2[j];
                if ((info6.IsClear() && (info6.mfGetQuestType() == QuestEntity.enType.FRIENDSHIP)) && (info6.mfGetMine().getServantId() == id))
                {
                    int num7 = info6.mfGetQuestID();
                    info5 = new MaterialEventLogListViewItem.Info {
                        svt_id = id,
                        limit_count = info6.mfGetMine().getLimitCount(),
                        str = info6.mfGetMine().getQuestName(),
                        quest_id = num7,
                        phase_max = info6.mfGetPhaseMax()
                    };
                    if (this.IsExistScriptFile(info5))
                    {
                        info5.on_click_act = new Action<MaterialEventLogListViewItem>(storeyd.<>m__130);
                        list4.Add(info5);
                    }
                }
            }
            for (int k = 1; k <= entity.maxLimitCount; k++)
            {
                <Init>c__AnonStorey8F storeyf = new <Init>c__AnonStorey8F {
                    <>f__this = this,
                    userServant = new UserServantEntity()
                };
                storeyf.userServant.limitCount = k - 1;
                storeyf.userServant.svtId = entity.svtId;
                storeyf.userServant.hp = entity.maxHp;
                storeyf.userServant.atk = entity.maxAtk;
                info5 = new MaterialEventLogListViewItem.Info {
                    svt_id = id,
                    str = LocalizationManager.Get($"MATERIAL_STORY_LIMIT_UP_{k.ToString()}"),
                    on_click_act = new Action<MaterialEventLogListViewItem>(storeyf.<>m__131)
                };
                list4.Add(info5);
            }
            if (list4.Count > 0)
            {
                this.mStoryItemInfs.Add(list4);
            }
        }
        List<EventEntity> sortedEntityList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).GetSortedEntityList();
        if (<>f__am$cache10 == null)
        {
            <>f__am$cache10 = entity => entity as ShopScriptEntity;
        }
        List<ShopScriptEntity> list6 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ShopScriptMaster>(DataNameKind.Kind.SHOP_SCRIPT).getEntityList().ConvertAll<ShopScriptEntity>(<>f__am$cache10);
        UserShopMaster master3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserShopMaster>(DataNameKind.Kind.USER_SHOP);
        this.mEventItemInfs = new List<List<MaterialEventLogListViewItem.Info>>();
        QuestGroupMaster master4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestGroupMaster>(DataNameKind.Kind.QUEST_GROUP);
        <Init>c__AnonStorey90 storey = new <Init>c__AnonStorey90 {
            <>f__this = this
        };
        using (List<EventEntity>.Enumerator enumerator2 = sortedEntityList.GetEnumerator())
        {
            while (enumerator2.MoveNext())
            {
                storey.event_ent = enumerator2.Current;
                int num9 = storey.event_ent.getEventId();
                List<MaterialEventLogListViewItem.Info> list7 = new List<MaterialEventLogListViewItem.Info>();
                bool flag = NetworkManager.getTime() >= storey.event_ent.materialOpenedAt;
                foreach (clsMapCtrl_QuestInfo info7 in list2)
                {
                    if ((flag || info7.IsClear()) && (master4.getEntityFromId<QuestGroupEntity>(info7.mfGetQuestID(), 1, num9) != null))
                    {
                        MaterialEventLogListViewItem.Info info8 = new MaterialEventLogListViewItem.Info {
                            str = info7.mfGetMine().getQuestName(),
                            event_id = num9,
                            war_id = info7.mfGetWarID(),
                            quest_id = info7.mfGetQuestID(),
                            phase_max = info7.mfGetPhaseMax()
                        };
                        if (this.IsExistScriptFile(info8))
                        {
                            info8.on_click_act = new Action<MaterialEventLogListViewItem>(storey.<>m__133);
                            list7.Add(info8);
                        }
                    }
                }
                List<ShopScriptEntity> list8 = list6.FindAll(new Predicate<ShopScriptEntity>(storey.<>m__134));
                int count = list8.Count;
                if (<>f__am$cache11 == null)
                {
                    <>f__am$cache11 = (a, b) => b.priority - a.priority;
                }
                list8.Sort(<>f__am$cache11);
                for (int m = 0; m < count; m++)
                {
                    <Init>c__AnonStorey91 storey2 = new <Init>c__AnonStorey91 {
                        <>f__this = this,
                        shopScriptEntity = list8[m]
                    };
                    UserShopEntity entity4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserShopMaster>(DataNameKind.Kind.USER_SHOP).getEntityFromId(NetworkManager.UserId, storey2.shopScriptEntity.shopId);
                    if (((entity4 != null) && (entity4.num > 0)) || flag)
                    {
                        MaterialEventLogListViewItem.Info info9 = new MaterialEventLogListViewItem.Info {
                            str = storey2.shopScriptEntity.name
                        };
                        if (storey2.shopScriptEntity.svtId != 0)
                        {
                            info9.svt_id = storey2.shopScriptEntity.svtId;
                            info9.flag |= MaterialEventLogListViewItem.Flag.SVT_FACE;
                        }
                        info9.event_id = num9;
                        info9.on_click_act = new Action<MaterialEventLogListViewItem>(storey2.<>m__136);
                        list7.Add(info9);
                    }
                }
                clsMapCtrl_WarInfo warInfoByEventID = SingletonTemplate<QuestTree>.Instance.GetWarInfoByEventID(num9);
                if ((warInfoByEventID != null) && ((list7.Count > 0) || ((warInfoByEventID.mfGetStatus() != clsMapCtrl_WarInfo.enStatus.None) && (warInfoByEventID.mfGetStatus() != clsMapCtrl_WarInfo.enStatus.New))))
                {
                    MaterialEventLogListViewItem.Info info11 = new MaterialEventLogListViewItem.Info {
                        str = LocalizationManager.Get("MATERIAL_MAP_AVANT_TITLE"),
                        event_id = num9,
                        war_id = warInfoByEventID.mfGetWarID(),
                        on_click_act = new Action<MaterialEventLogListViewItem>(storey.<>m__137)
                    };
                    list7.Insert(0, info11);
                }
                if (list7.Count > 0)
                {
                    this.mEventItemInfs.Add(list7);
                }
            }
        }
        this.mFreeItemInfs = new List<List<MaterialEventLogListViewItem.Info>>();
        foreach (clsMapCtrl_WarInfo info12 in list)
        {
            int num13 = info12.mfGetWarID();
            List<MaterialEventLogListViewItem.Info> list9 = new List<MaterialEventLogListViewItem.Info>();
            foreach (clsMapCtrl_QuestInfo info13 in list2)
            {
                if ((info13.IsClear() && (info13.mfGetWarID() == num13)) && (info13.mfGetQuestType() == QuestEntity.enType.FREE))
                {
                    MaterialEventLogListViewItem.Info info14 = new MaterialEventLogListViewItem.Info {
                        str = info13.mfGetMine().getQuestName(),
                        war_id = num13,
                        quest_id = info13.mfGetQuestID(),
                        phase_max = info13.mfGetPhaseMax()
                    };
                    if (this.IsExistScriptFile(info14))
                    {
                        info14.on_click_act = new Action<MaterialEventLogListViewItem>(storeyd.<>m__138);
                        list9.Add(info14);
                    }
                }
            }
            if (list9.Count > 0)
            {
                this.mFreeItemInfs.Add(list9);
            }
        }
        this.mFSM = new CStateManager<MyRoomStateMaterial>(this, 7);
        this.mFSM.add(0, new StateTop());
        this.mFSM.add(2, new StateMap());
        this.mFSM.add(3, new StateStory());
        this.mFSM.add(4, new StateEvent());
        this.mFSM.add(5, new StateFree());
        this.mFSM.add(6, new StateQuest());
    }

    private bool IsExistScriptFile(MaterialEventLogListViewItem.Info inf)
    {
        for (int i = 0; i < inf.phase_max; i++)
        {
            if (this.IsExistScriptFile(inf.quest_id, i + 1))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsExistScriptFile(string file_name)
    {
        file_name = file_name + ".txt";
        foreach (string str in this.mScriptFileListStrs)
        {
            if (str == file_name)
            {
                return true;
            }
        }
        return false;
    }

    private bool IsExistScriptFile(int quest_id, int phase)
    {
        string str = ScriptManager.GetScriptName_BattleStart(quest_id, phase);
        if (this.IsExistScriptFile(str))
        {
            return true;
        }
        str = ScriptManager.GetScriptName_BattleEnd(quest_id, phase);
        return this.IsExistScriptFile(str);
    }

    private void PlayBattleDemo(string demoInfo, MaterialEventLogListViewItem.Info inf, int phase, bool is_battle_before, bool is_talk_before)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD);
        BattleSetupInfo data = new BattleSetupInfo {
            questId = inf.quest_id,
            questPhase = phase,
            battleBefore = is_battle_before,
            isBefore = is_talk_before,
            demoInfo = demoInfo
        };
        SingletonMonoBehaviour<SceneManager>.Instance.pushScene(SceneList.Type.BattleDemoScene, SceneManager.FadeType.BLACK, data);
        this.mItemInfo_PlayQuestTalk = inf;
    }

    private void PlayChapterClear(MaterialEventLogListViewItem.Info inf, System.Action end_act)
    {
        <PlayChapterClear>c__AnonStorey99 storey = new <PlayChapterClear>c__AnonStorey99 {
            end_act = end_act
        };
        WarEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getByLastQuestId(inf.quest_id);
        if (entity != null)
        {
            ScriptManager.PlayChapterClear(entity.id, new ScriptManager.CallbackFunc(storey.<>m__144), true);
        }
        else
        {
            storey.end_act.Call();
        }
    }

    private void PlayQuestTalk(MaterialEventLogListViewItem.Info inf)
    {
        <PlayQuestTalk>c__AnonStorey95 storey = new <PlayQuestTalk>c__AnonStorey95 {
            inf = inf,
            <>f__this = this
        };
        this.mItemInfo_PlayQuestTalk = storey.inf;
        this.ReadyTalk(new System.Action(storey.<>m__13F), true);
    }

    private void PlayQuestTalk_BattleAfter_TalkAfter(MaterialEventLogListViewItem.Info inf, int phase)
    {
        <PlayQuestTalk_BattleAfter_TalkAfter>c__AnonStorey98 storey = new <PlayQuestTalk_BattleAfter_TalkAfter>c__AnonStorey98 {
            inf = inf,
            phase = phase,
            <>f__this = this
        };
        ScriptManager.PlayBattleEnd(storey.inf.quest_id, storey.phase, new ScriptManager.CallbackFunc(storey.<>m__143), true);
    }

    private void PlayQuestTalk_BattleAfter_TalkBefore(MaterialEventLogListViewItem.Info inf, int phase)
    {
        <PlayQuestTalk_BattleAfter_TalkBefore>c__AnonStorey97 storey = new <PlayQuestTalk_BattleAfter_TalkBefore>c__AnonStorey97 {
            inf = inf,
            phase = phase,
            <>f__this = this
        };
        ScriptManager.LoadBattleEndGameDemo(storey.inf.quest_id, storey.phase, true, new Action<string>(storey.<>m__142));
    }

    private void PlayQuestTalk_BattleBefore_TalkAfter(MaterialEventLogListViewItem.Info inf, int phase)
    {
        <PlayQuestTalk_BattleBefore_TalkAfter>c__AnonStorey96 storey = new <PlayQuestTalk_BattleBefore_TalkAfter>c__AnonStorey96 {
            inf = inf,
            phase = phase,
            <>f__this = this
        };
        if (storey.phase > storey.inf.phase_max)
        {
            this.PlayChapterClear(storey.inf, new System.Action(storey.<>m__140));
        }
        else
        {
            ScriptManager.PlayBattleStart(storey.inf.quest_id, storey.phase, new ScriptManager.CallbackFunc(storey.<>m__141), true);
        }
    }

    private void ReadyTalk(System.Action end_act, bool is_fade = true)
    {
        <ReadyTalk>c__AnonStorey93 storey = new <ReadyTalk>c__AnonStorey93 {
            is_fade = is_fade,
            end_act = end_act
        };
        SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
        this.mMyRoomControl.stopSvtVoice();
        this.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.VALID, new System.Action(storey.<>m__13C));
    }

    private void ReturnFromBattleDemo()
    {
        BattleSetupInfo mBattleSetupInfo = this.mMyRoomControl.mBattleSetupInfo;
        if (mBattleSetupInfo != null)
        {
            MaterialEventLogListViewItem.Info inf = this.mItemInfo_PlayQuestTalk;
            if (mBattleSetupInfo.battleBefore)
            {
                this.PlayQuestTalk_BattleAfter_TalkBefore(inf, mBattleSetupInfo.questPhase);
            }
            else if (mBattleSetupInfo.isBefore)
            {
                this.PlayQuestTalk_BattleAfter_TalkAfter(inf, mBattleSetupInfo.questPhase);
            }
            else
            {
                this.PlayQuestTalk_BattleBefore_TalkAfter(inf, mBattleSetupInfo.questPhase + 1);
            }
            this.mMyRoomControl.mBattleSetupInfo = null;
        }
    }

    private void ReturnFromTalk(bool is_fade = true)
    {
        this.ReturnFromTalk(null, is_fade);
    }

    private void ReturnFromTalk(System.Action end_act, bool is_fade = true)
    {
        <ReturnFromTalk>c__AnonStorey94 storey = new <ReturnFromTalk>c__AnonStorey94 {
            end_act = end_act,
            <>f__this = this
        };
        if (!SoundManager.isPlayBgm(this.mMyRoomControl.getMyRoomBgm()))
        {
            SoundManager.playBgm(this.mMyRoomControl.getMyRoomBgm());
        }
        storey.act = new System.Action(storey.<>m__13D);
        if (is_fade)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(storey.<>m__13E));
        }
        else
        {
            storey.act.Call();
        }
    }

    public void SetState(STATE state)
    {
        this.mOldState = this.GetState();
        this.mFSM.setState((int) state);
    }

    protected void SetupGachaBg(Transform bgRoot, System.Action callback)
    {
        <SetupGachaBg>c__AnonStorey8B storeyb = new <SetupGachaBg>c__AnonStorey8B {
            bgRoot = bgRoot,
            callback = callback,
            <>f__this = this
        };
        AssetManager.loadAssetStorage(GachaBgPath, new AssetLoader.LoadEndDataHandler(storeyb.<>m__12C));
    }

    public void update(MyRoomControl that)
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey8D
    {
        internal MyRoomStateMaterial <>f__this;
        internal MyRoomControl that;

        internal void <>m__12D(MaterialEventLogListViewItem itm)
        {
            <Init>c__AnonStorey8C storeyc = new <Init>c__AnonStorey8C {
                <>f__ref$141 = this,
                itm = itm
            };
            this.<>f__this.ReadyTalk(new System.Action(storeyc.<>m__145), true);
        }

        internal void <>m__12E(MaterialEventLogListViewItem itm)
        {
            this.<>f__this.PlayQuestTalk(itm.info);
        }

        internal void <>m__12F(MaterialEventLogListViewItem itm)
        {
            <Init>c__AnonStorey8E storeye = new <Init>c__AnonStorey8E {
                <>f__ref$141 = this,
                itm = itm
            };
            this.<>f__this.ReadyTalk(new System.Action(storeye.<>m__146), true);
        }

        internal void <>m__130(MaterialEventLogListViewItem itm)
        {
            this.<>f__this.PlayQuestTalk(itm.info);
        }

        internal void <>m__138(MaterialEventLogListViewItem itm)
        {
            this.<>f__this.PlayQuestTalk(itm.info);
        }

        private sealed class <Init>c__AnonStorey8C
        {
            internal MyRoomStateMaterial.<Init>c__AnonStorey8D <>f__ref$141;
            internal MaterialEventLogListViewItem itm;

            internal void <>m__145()
            {
                ScriptManager.PlayChapterStart(this.itm.info.war_id, isExit => this.<>f__ref$141.<>f__this.ReturnFromTalk(true), true);
            }

            internal void <>m__147(bool isExit)
            {
                this.<>f__ref$141.<>f__this.ReturnFromTalk(true);
            }
        }

        private sealed class <Init>c__AnonStorey8E
        {
            internal MyRoomStateMaterial.<Init>c__AnonStorey8D <>f__ref$141;
            internal MaterialEventLogListViewItem itm;

            internal void <>m__146()
            {
                this.<>f__ref$141.<>f__this.SetupGachaBg(this.<>f__ref$141.that.MaterialGachaBgRoot, delegate {
                    this.<>f__ref$141.<>f__this.mMyRoomControl.GetCamera2DUI().enabled = false;
                    SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, () => ScriptManager.PlayGacha(this.itm.info.svt_id, 0, true, string.Empty, (System.Action) (() => SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                        this.<>f__ref$141.<>f__this.mMyRoomControl.GetCamera2DUI().enabled = true;
                        this.<>f__ref$141.<>f__this.ReturnFromTalk(null, true);
                    }))));
                });
            }

            internal void <>m__148()
            {
                this.<>f__ref$141.<>f__this.mMyRoomControl.GetCamera2DUI().enabled = false;
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, () => ScriptManager.PlayGacha(this.itm.info.svt_id, 0, true, string.Empty, (System.Action) (() => SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                    this.<>f__ref$141.<>f__this.mMyRoomControl.GetCamera2DUI().enabled = true;
                    this.<>f__ref$141.<>f__this.ReturnFromTalk(null, true);
                }))));
            }

            internal void <>m__149()
            {
                ScriptManager.PlayGacha(this.itm.info.svt_id, 0, true, string.Empty, (System.Action) (() => SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                    this.<>f__ref$141.<>f__this.mMyRoomControl.GetCamera2DUI().enabled = true;
                    this.<>f__ref$141.<>f__this.ReturnFromTalk(null, true);
                })));
            }

            internal void <>m__14A()
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, delegate {
                    this.<>f__ref$141.<>f__this.mMyRoomControl.GetCamera2DUI().enabled = true;
                    this.<>f__ref$141.<>f__this.ReturnFromTalk(null, true);
                });
            }

            internal void <>m__14B()
            {
                this.<>f__ref$141.<>f__this.mMyRoomControl.GetCamera2DUI().enabled = true;
                this.<>f__ref$141.<>f__this.ReturnFromTalk(null, true);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey8F
    {
        internal MyRoomStateMaterial <>f__this;
        internal UserServantEntity userServant;

        internal void <>m__131(MaterialEventLogListViewItem itm)
        {
            this.<>f__this.ReadyTalk(delegate {
                this.<>f__this.mMyRoomControl.GetCamera2DUI().gameObject.SetActive(false);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenLimitUpCombineResult(CombineResultEffectComponent.Kind.MATERIAL_LIMIT_UP, this.userServant, delegate (bool isDecide) {
                    SingletonMonoBehaviour<CommonUI>.Instance.CloseCombineResult();
                    this.<>f__this.mMyRoomControl.GetCamera2DUI().gameObject.SetActive(true);
                    this.<>f__this.ReturnFromTalk(true);
                });
                SoundManager.playBgm("BGM_CHALDEA_1");
            }, false);
        }

        internal void <>m__14C()
        {
            this.<>f__this.mMyRoomControl.GetCamera2DUI().gameObject.SetActive(false);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenLimitUpCombineResult(CombineResultEffectComponent.Kind.MATERIAL_LIMIT_UP, this.userServant, delegate (bool isDecide) {
                SingletonMonoBehaviour<CommonUI>.Instance.CloseCombineResult();
                this.<>f__this.mMyRoomControl.GetCamera2DUI().gameObject.SetActive(true);
                this.<>f__this.ReturnFromTalk(true);
            });
            SoundManager.playBgm("BGM_CHALDEA_1");
        }

        internal void <>m__14D(bool isDecide)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseCombineResult();
            this.<>f__this.mMyRoomControl.GetCamera2DUI().gameObject.SetActive(true);
            this.<>f__this.ReturnFromTalk(true);
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey90
    {
        internal MyRoomStateMaterial <>f__this;
        internal EventEntity event_ent;

        internal void <>m__133(MaterialEventLogListViewItem itm)
        {
            this.<>f__this.PlayQuestTalk(itm.info);
        }

        internal bool <>m__134(ShopScriptEntity script) => 
            (script.eventId == this.event_ent.id);

        internal void <>m__137(MaterialEventLogListViewItem itm)
        {
            <Init>c__AnonStorey92 storey = new <Init>c__AnonStorey92 {
                <>f__ref$144 = this,
                itm = itm
            };
            this.<>f__this.ReadyTalk(new System.Action(storey.<>m__14E), true);
        }

        private sealed class <Init>c__AnonStorey92
        {
            internal MyRoomStateMaterial.<Init>c__AnonStorey90 <>f__ref$144;
            internal MaterialEventLogListViewItem itm;

            internal void <>m__14E()
            {
                ScriptManager.PlayChapterStart(this.itm.info.war_id, isExit => this.<>f__ref$144.<>f__this.ReturnFromTalk(true), true);
            }

            internal void <>m__14F(bool isExit)
            {
                this.<>f__ref$144.<>f__this.ReturnFromTalk(true);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <Init>c__AnonStorey91
    {
        internal MyRoomStateMaterial <>f__this;
        internal ShopScriptEntity shopScriptEntity;

        internal void <>m__136(MaterialEventLogListViewItem itm)
        {
            this.<>f__this.ReadyTalk(() => ScriptManager.PlayShop(this.shopScriptEntity.scriptId, isExit => this.<>f__this.ReturnFromTalk(true), true), true);
        }

        internal void <>m__150()
        {
            ScriptManager.PlayShop(this.shopScriptEntity.scriptId, isExit => this.<>f__this.ReturnFromTalk(true), true);
        }

        internal void <>m__151(bool isExit)
        {
            this.<>f__this.ReturnFromTalk(true);
        }
    }

    [CompilerGenerated]
    private sealed class <PlayChapterClear>c__AnonStorey99
    {
        internal System.Action end_act;

        internal void <>m__144(bool isExit)
        {
            this.end_act.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <PlayQuestTalk_BattleAfter_TalkAfter>c__AnonStorey98
    {
        internal MyRoomStateMaterial <>f__this;
        internal MaterialEventLogListViewItem.Info inf;
        internal int phase;

        internal void <>m__143(bool isExit)
        {
            if (isExit)
            {
                this.<>f__this.ReturnFromTalk(true);
            }
            else
            {
                ScriptManager.LoadBattleEndGameDemo(this.inf.quest_id, this.phase, false, delegate (string demoInfo) {
                    if (demoInfo != null)
                    {
                        this.<>f__this.PlayBattleDemo(demoInfo, this.inf, this.phase, false, false);
                    }
                    else
                    {
                        this.<>f__this.PlayQuestTalk_BattleBefore_TalkAfter(this.inf, this.phase + 1);
                    }
                });
            }
        }

        internal void <>m__156(string demoInfo)
        {
            if (demoInfo != null)
            {
                this.<>f__this.PlayBattleDemo(demoInfo, this.inf, this.phase, false, false);
            }
            else
            {
                this.<>f__this.PlayQuestTalk_BattleBefore_TalkAfter(this.inf, this.phase + 1);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayQuestTalk_BattleAfter_TalkBefore>c__AnonStorey97
    {
        internal MyRoomStateMaterial <>f__this;
        internal MaterialEventLogListViewItem.Info inf;
        internal int phase;

        internal void <>m__142(string demoInfo)
        {
            if (demoInfo != null)
            {
                this.<>f__this.PlayBattleDemo(demoInfo, this.inf, this.phase, false, true);
            }
            else
            {
                this.<>f__this.PlayQuestTalk_BattleAfter_TalkAfter(this.inf, this.phase);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayQuestTalk_BattleBefore_TalkAfter>c__AnonStorey96
    {
        internal MyRoomStateMaterial <>f__this;
        internal MaterialEventLogListViewItem.Info inf;
        internal int phase;

        internal void <>m__140()
        {
            this.<>f__this.ReturnFromTalk(true);
        }

        internal void <>m__141(bool isExit)
        {
            if (isExit)
            {
                this.<>f__this.ReturnFromTalk(true);
            }
            else
            {
                ScriptManager.LoadBattleStartGameDemo(this.inf.quest_id, this.phase, false, delegate (string demoInfo) {
                    if (demoInfo != null)
                    {
                        this.<>f__this.PlayBattleDemo(demoInfo, this.inf, this.phase, true, false);
                    }
                    else
                    {
                        this.<>f__this.PlayQuestTalk_BattleAfter_TalkBefore(this.inf, this.phase);
                    }
                });
            }
        }

        internal void <>m__155(string demoInfo)
        {
            if (demoInfo != null)
            {
                this.<>f__this.PlayBattleDemo(demoInfo, this.inf, this.phase, true, false);
            }
            else
            {
                this.<>f__this.PlayQuestTalk_BattleAfter_TalkBefore(this.inf, this.phase);
            }
        }
    }

    [CompilerGenerated]
    private sealed class <PlayQuestTalk>c__AnonStorey95
    {
        internal MyRoomStateMaterial <>f__this;
        internal MaterialEventLogListViewItem.Info inf;

        internal void <>m__13F()
        {
            ScriptManager.PlayQuestStart(this.inf.war_id, this.inf.quest_id, () => this.<>f__this.PlayQuestTalk_BattleBefore_TalkAfter(this.inf, 1));
        }

        internal void <>m__154()
        {
            this.<>f__this.PlayQuestTalk_BattleBefore_TalkAfter(this.inf, 1);
        }
    }

    [CompilerGenerated]
    private sealed class <ReadyTalk>c__AnonStorey93
    {
        internal System.Action end_act;
        internal bool is_fade;

        internal void <>m__13C()
        {
            if (this.is_fade)
            {
                SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, () => this.end_act.Call());
            }
            else
            {
                this.end_act.Call();
            }
        }

        internal void <>m__152()
        {
            this.end_act.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <ReturnFromTalk>c__AnonStorey94
    {
        internal MyRoomStateMaterial <>f__this;
        internal System.Action act;
        internal System.Action end_act;

        internal void <>m__13D()
        {
            this.<>f__this.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.INPUT, delegate {
                if (this.<>f__this.GachaBg != null)
                {
                    UnityEngine.Object.Destroy(this.<>f__this.GachaBg);
                }
                AssetManager.releaseAssetStorage(MyRoomStateMaterial.GachaBgPath);
                this.end_act.Call();
            });
            SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
        }

        internal void <>m__13E()
        {
            this.act.Call();
        }

        internal void <>m__153()
        {
            if (this.<>f__this.GachaBg != null)
            {
                UnityEngine.Object.Destroy(this.<>f__this.GachaBg);
            }
            AssetManager.releaseAssetStorage(MyRoomStateMaterial.GachaBgPath);
            this.end_act.Call();
        }
    }

    [CompilerGenerated]
    private sealed class <SetupGachaBg>c__AnonStorey8B
    {
        internal MyRoomStateMaterial <>f__this;
        internal Transform bgRoot;
        internal System.Action callback;

        internal void <>m__12C(AssetData data)
        {
            this.<>f__this.GachaBg = UnityEngine.Object.Instantiate<GameObject>(data.GetObject<GameObject>("bg"));
            this.<>f__this.GachaBg.transform.parent = this.bgRoot;
            this.<>f__this.GachaBg.transform.localPosition = Vector3.zero;
            this.<>f__this.GachaBg.transform.localScale = Vector3.one;
            if (this.callback != null)
            {
                this.callback();
            }
        }
    }

    public enum STATE
    {
        TOP,
        SERVANT,
        MAP,
        STORY,
        EVENT,
        FREE,
        QUEST,
        SIZEOF
    }

    private class StateEvent : IState<MyRoomStateMaterial>
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache1;
        public static readonly int FONT_SIZE = MyRoomStateMaterial.DEFAULT_MIN_FONT_SIZE;

        public void begin(MyRoomStateMaterial that)
        {
            <begin>c__AnonStoreyA0 ya = new <begin>c__AnonStoreyA0 {
                that = that
            };
            List<MaterialEventLogListViewItem.Info> infs = new List<MaterialEventLogListViewItem.Info>();
            foreach (List<MaterialEventLogListViewItem.Info> list2 in ya.that.mEventItemInfs)
            {
                int id = list2[0].event_id;
                EventEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMaster>(DataNameKind.Kind.EVENT).getEntityFromId<EventEntity>(id);
                MaterialEventLogListViewItem.Info item = new MaterialEventLogListViewItem.Info {
                    str = entity.getEventName(),
                    on_click_act = new Action<MaterialEventLogListViewItem>(ya.<>m__16E)
                };
                infs.Add(item);
            }
            foreach (MaterialEventLogListViewItem.Info info2 in infs)
            {
                info2.font_size = FONT_SIZE;
            }
            ya.that.mListViewManager.CreateList(MaterialEventLogListViewItem.Kind.EVENT, infs);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate {
                };
            }
            ya.that.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.INTO, <>f__am$cache1);
        }

        public void end(MyRoomStateMaterial that)
        {
        }

        public void update(MyRoomStateMaterial that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyA0
        {
            internal MyRoomStateMaterial that;

            internal void <>m__16E(MaterialEventLogListViewItem itm)
            {
                <begin>c__AnonStoreyA1 ya = new <begin>c__AnonStoreyA1 {
                    <>f__ref$160 = this,
                    itm = itm
                };
                this.that.FrameOut(new System.Action(ya.<>m__170));
            }

            private sealed class <begin>c__AnonStoreyA1
            {
                internal MyRoomStateMaterial.StateEvent.<begin>c__AnonStoreyA0 <>f__ref$160;
                internal MaterialEventLogListViewItem itm;

                internal void <>m__170()
                {
                    this.<>f__ref$160.that.mSelectInfs = this.<>f__ref$160.that.mEventItemInfs[this.itm.Index];
                    this.<>f__ref$160.that.SetState(MyRoomStateMaterial.STATE.QUEST);
                }
            }
        }
    }

    private class StateFree : IState<MyRoomStateMaterial>
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache1;
        public static readonly int FONT_SIZE = MyRoomStateMaterial.DEFAULT_FONT_SIZE;

        public void begin(MyRoomStateMaterial that)
        {
            <begin>c__AnonStoreyA2 ya = new <begin>c__AnonStoreyA2 {
                that = that
            };
            List<MaterialEventLogListViewItem.Info> infs = new List<MaterialEventLogListViewItem.Info>();
            foreach (List<MaterialEventLogListViewItem.Info> list2 in ya.that.mFreeItemInfs)
            {
                int id = list2[0].war_id;
                WarEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(id);
                MaterialEventLogListViewItem.Info item = new MaterialEventLogListViewItem.Info {
                    str = entity.name,
                    on_click_act = new Action<MaterialEventLogListViewItem>(ya.<>m__171)
                };
                infs.Add(item);
            }
            foreach (MaterialEventLogListViewItem.Info info2 in infs)
            {
                info2.font_size = FONT_SIZE;
            }
            ya.that.mListViewManager.CreateList(MaterialEventLogListViewItem.Kind.FREE, infs);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate {
                };
            }
            ya.that.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.INTO, <>f__am$cache1);
        }

        public void end(MyRoomStateMaterial that)
        {
        }

        public void update(MyRoomStateMaterial that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStoreyA2
        {
            internal MyRoomStateMaterial that;

            internal void <>m__171(MaterialEventLogListViewItem itm)
            {
                <begin>c__AnonStoreyA3 ya = new <begin>c__AnonStoreyA3 {
                    <>f__ref$162 = this,
                    itm = itm
                };
                this.that.FrameOut(new System.Action(ya.<>m__173));
            }

            private sealed class <begin>c__AnonStoreyA3
            {
                internal MyRoomStateMaterial.StateFree.<begin>c__AnonStoreyA2 <>f__ref$162;
                internal MaterialEventLogListViewItem itm;

                internal void <>m__173()
                {
                    this.<>f__ref$162.that.mSelectInfs = this.<>f__ref$162.that.mFreeItemInfs[this.itm.Index];
                    this.<>f__ref$162.that.SetState(MyRoomStateMaterial.STATE.QUEST);
                }
            }
        }
    }

    private class StateMap : IState<MyRoomStateMaterial>
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache1;
        public static readonly int FONT_SIZE = MyRoomStateMaterial.DEFAULT_MIN_FONT_SIZE;

        public void begin(MyRoomStateMaterial that)
        {
            <begin>c__AnonStorey9B storeyb = new <begin>c__AnonStorey9B {
                that = that
            };
            List<MaterialEventLogListViewItem.Info> infs = new List<MaterialEventLogListViewItem.Info>();
            MaterialEventLogListViewItem.Info item = new MaterialEventLogListViewItem.Info {
                str = LocalizationManager.Get("MATERIAL_MAP_PROLOGUE"),
                on_click_act = new Action<MaterialEventLogListViewItem>(storeyb.<>m__163)
            };
            infs.Add(item);
            for (int i = 0; i < storeyb.that.mMapItemInfs.Count; i++)
            {
                List<MaterialEventLogListViewItem.Info> list2 = storeyb.that.mMapItemInfs[i];
                int id = list2[0].war_id;
                WarEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getEntityFromId<WarEntity>(id);
                MaterialEventLogListViewItem.Info info2 = new MaterialEventLogListViewItem.Info {
                    str = entity.longName,
                    on_click_act = new Action<MaterialEventLogListViewItem>(storeyb.<>m__164)
                };
                infs.Add(info2);
            }
            foreach (MaterialEventLogListViewItem.Info info3 in infs)
            {
                info3.font_size = FONT_SIZE;
            }
            storeyb.that.mListViewManager.CreateList(MaterialEventLogListViewItem.Kind.MAP, infs);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate {
                };
            }
            storeyb.that.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.INTO, <>f__am$cache1);
        }

        public void end(MyRoomStateMaterial that)
        {
        }

        public void update(MyRoomStateMaterial that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStorey9B
        {
            internal MyRoomStateMaterial that;

            internal void <>m__163(MaterialEventLogListViewItem itm)
            {
                this.that.ReadyTalk(delegate {
                    <begin>c__AnonStorey9C storeyc = new <begin>c__AnonStorey9C {
                        <>f__ref$155 = this,
                        qid = ConstantMaster.getValue("TUTORIAL_QUEST_ID1"),
                        phase = 1
                    };
                    ScriptManager.PlayBattleStart(storeyc.qid, storeyc.phase, new ScriptManager.CallbackFunc(storeyc.<>m__168), true);
                }, true);
            }

            internal void <>m__164(MaterialEventLogListViewItem itm)
            {
                <begin>c__AnonStorey9D storeyd = new <begin>c__AnonStorey9D {
                    <>f__ref$155 = this,
                    itm = itm
                };
                this.that.FrameOut(new System.Action(storeyd.<>m__167));
            }

            internal void <>m__166()
            {
                <begin>c__AnonStorey9C storeyc = new <begin>c__AnonStorey9C {
                    <>f__ref$155 = this,
                    qid = ConstantMaster.getValue("TUTORIAL_QUEST_ID1"),
                    phase = 1
                };
                ScriptManager.PlayBattleStart(storeyc.qid, storeyc.phase, new ScriptManager.CallbackFunc(storeyc.<>m__168), true);
            }

            private sealed class <begin>c__AnonStorey9C
            {
                internal MyRoomStateMaterial.StateMap.<begin>c__AnonStorey9B <>f__ref$155;
                internal int phase;
                internal int qid;

                internal void <>m__168(bool isExit)
                {
                    if (isExit)
                    {
                        this.<>f__ref$155.that.ReturnFromTalk(true);
                    }
                    else
                    {
                        ScriptManager.PlayBattleEnd(this.qid, this.phase, isExit2 => this.<>f__ref$155.that.ReturnFromTalk(true), true);
                    }
                }

                internal void <>m__169(bool isExit2)
                {
                    this.<>f__ref$155.that.ReturnFromTalk(true);
                }
            }

            private sealed class <begin>c__AnonStorey9D
            {
                internal MyRoomStateMaterial.StateMap.<begin>c__AnonStorey9B <>f__ref$155;
                internal MaterialEventLogListViewItem itm;

                internal void <>m__167()
                {
                    this.<>f__ref$155.that.mSelectInfs = this.<>f__ref$155.that.mMapItemInfs[this.itm.Index - 1];
                    this.<>f__ref$155.that.SetState(MyRoomStateMaterial.STATE.QUEST);
                }
            }
        }
    }

    private class StateQuest : IState<MyRoomStateMaterial>
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache1;
        public static readonly int FONT_SIZE = MyRoomStateMaterial.DEFAULT_MIN_FONT_SIZE;

        public void begin(MyRoomStateMaterial that)
        {
            List<MaterialEventLogListViewItem.Info> mSelectInfs = that.mSelectInfs;
            foreach (MaterialEventLogListViewItem.Info info in mSelectInfs)
            {
                info.font_size = FONT_SIZE;
            }
            that.mListViewManager.CreateList(MaterialEventLogListViewItem.Kind.QUEST, mSelectInfs);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate {
                };
            }
            that.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.INTO, <>f__am$cache1);
        }

        public void end(MyRoomStateMaterial that)
        {
        }

        public void update(MyRoomStateMaterial that)
        {
            that.ReturnFromBattleDemo();
        }
    }

    private class StateStory : IState<MyRoomStateMaterial>
    {
        [CompilerGenerated]
        private static Comparison<MaterialEventLogListViewItem.Info> <>f__am$cache1;
        [CompilerGenerated]
        private static System.Action <>f__am$cache2;
        public static readonly int FONT_SIZE = MyRoomStateMaterial.DEFAULT_MIN_FONT_SIZE;

        public void begin(MyRoomStateMaterial that)
        {
            <begin>c__AnonStorey9E storeye = new <begin>c__AnonStorey9E {
                that = that
            };
            List<MaterialEventLogListViewItem.Info> infs = new List<MaterialEventLogListViewItem.Info>();
            foreach (List<MaterialEventLogListViewItem.Info> list2 in storeye.that.mStoryItemInfs)
            {
                <begin>c__AnonStorey9F storeyf = new <begin>c__AnonStorey9F {
                    <>f__ref$158 = storeye
                };
                int id = list2[0].svt_id;
                int num2 = list2[0].limit_count;
                ServantEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).getEntityFromId<ServantEntity>(id);
                MaterialEventLogListViewItem.Info item = new MaterialEventLogListViewItem.Info();
                storeyf.capturedItems = list2;
                item.str = entity.name;
                item.ruby = entity.ruby;
                item.svt_id = id;
                item.limit_count = num2;
                item.flag |= MaterialEventLogListViewItem.Flag.SVT_FACE;
                item.on_click_act = new Action<MaterialEventLogListViewItem>(storeyf.<>m__16A);
                infs.Add(item);
            }
            foreach (MaterialEventLogListViewItem.Info info2 in infs)
            {
                info2.font_size = FONT_SIZE;
            }
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = (x, y) => x.ruby.CompareTo(y.ruby);
            }
            infs.Sort(<>f__am$cache1);
            storeye.that.mListViewManager.CreateList(MaterialEventLogListViewItem.Kind.STORY, infs);
            if (<>f__am$cache2 == null)
            {
                <>f__am$cache2 = delegate {
                };
            }
            storeye.that.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.INTO, <>f__am$cache2);
        }

        public void end(MyRoomStateMaterial that)
        {
        }

        public void update(MyRoomStateMaterial that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStorey9E
        {
            internal MyRoomStateMaterial that;
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStorey9F
        {
            internal MyRoomStateMaterial.StateStory.<begin>c__AnonStorey9E <>f__ref$158;
            internal List<MaterialEventLogListViewItem.Info> capturedItems;

            internal void <>m__16A(MaterialEventLogListViewItem itm)
            {
                this.<>f__ref$158.that.FrameOut(delegate {
                    this.<>f__ref$158.that.mSelectInfs = this.capturedItems;
                    this.<>f__ref$158.that.SetState(MyRoomStateMaterial.STATE.QUEST);
                });
            }

            internal void <>m__16D()
            {
                this.<>f__ref$158.that.mSelectInfs = this.capturedItems;
                this.<>f__ref$158.that.SetState(MyRoomStateMaterial.STATE.QUEST);
            }
        }
    }

    private class StateTop : IState<MyRoomStateMaterial>
    {
        [CompilerGenerated]
        private static System.Action <>f__am$cache1;
        public static readonly int FONT_SIZE = MyRoomStateMaterial.DEFAULT_FONT_SIZE;

        public void begin(MyRoomStateMaterial that)
        {
            <begin>c__AnonStorey9A storeya = new <begin>c__AnonStorey9A {
                that = that
            };
            List<MaterialEventLogListViewItem.Info> infs = new List<MaterialEventLogListViewItem.Info>();
            MaterialEventLogListViewItem.Info item = new MaterialEventLogListViewItem.Info {
                str = LocalizationManager.Get("MATERIAL_TOP_SERVANT"),
                on_click_act = new Action<MaterialEventLogListViewItem>(storeya.<>m__157)
            };
            infs.Add(item);
            MaterialEventLogListViewItem.Info info2 = new MaterialEventLogListViewItem.Info {
                str = LocalizationManager.Get("MATERIAL_TOP_MAP"),
                on_click_act = new Action<MaterialEventLogListViewItem>(storeya.<>m__158)
            };
            infs.Add(info2);
            if (storeya.that.mStoryItemInfs.Count > 0)
            {
                MaterialEventLogListViewItem.Info info3 = new MaterialEventLogListViewItem.Info {
                    str = LocalizationManager.Get("MATERIAL_TOP_STORY"),
                    on_click_act = new Action<MaterialEventLogListViewItem>(storeya.<>m__159)
                };
                infs.Add(info3);
            }
            if (storeya.that.mEventItemInfs.Count > 0)
            {
                MaterialEventLogListViewItem.Info info4 = new MaterialEventLogListViewItem.Info {
                    str = LocalizationManager.Get("MATERIAL_TOP_EVENT"),
                    on_click_act = new Action<MaterialEventLogListViewItem>(storeya.<>m__15A)
                };
                infs.Add(info4);
            }
            if (storeya.that.mFreeItemInfs.Count > 0)
            {
                MaterialEventLogListViewItem.Info info5 = new MaterialEventLogListViewItem.Info {
                    str = LocalizationManager.Get("MATERIAL_TOP_FREE"),
                    on_click_act = new Action<MaterialEventLogListViewItem>(storeya.<>m__15B)
                };
                infs.Add(info5);
            }
            if (SingletonTemplate<clsQuestCheck>.Instance.IsWarClear(ConstantMaster.getValue("FIRST_WAR_ID")))
            {
                MaterialEventLogListViewItem.Info info6 = new MaterialEventLogListViewItem.Info {
                    str = LocalizationManager.Get("MATERIAL_TOP_OPENING")
                };
                info6.flag |= MaterialEventLogListViewItem.Flag.NOPLAY_DECIDE_SE;
                info6.on_click_act = new Action<MaterialEventLogListViewItem>(storeya.<>m__15C);
                infs.Add(info6);
            }
            foreach (MaterialEventLogListViewItem.Info info7 in infs)
            {
                info7.font_size = FONT_SIZE;
            }
            storeya.that.mListViewManager.CreateList(MaterialEventLogListViewItem.Kind.TOP, infs);
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = delegate {
                };
            }
            storeya.that.mListViewManager.SetMode(MaterialEventLogListViewManager.InitMode.INTO, <>f__am$cache1);
        }

        public void end(MyRoomStateMaterial that)
        {
        }

        public void update(MyRoomStateMaterial that)
        {
        }

        [CompilerGenerated]
        private sealed class <begin>c__AnonStorey9A
        {
            internal MyRoomStateMaterial that;

            internal void <>m__157(MaterialEventLogListViewItem itm)
            {
                this.that.FrameOut(() => this.that.mPlayMakerFSM.SendEvent("CLICK_MATERIAL_COLLECTION"));
            }

            internal void <>m__158(MaterialEventLogListViewItem itm)
            {
                this.that.FrameOut(() => this.that.SetState(MyRoomStateMaterial.STATE.MAP));
            }

            internal void <>m__159(MaterialEventLogListViewItem itm)
            {
                this.that.FrameOut(() => this.that.SetState(MyRoomStateMaterial.STATE.STORY));
            }

            internal void <>m__15A(MaterialEventLogListViewItem itm)
            {
                this.that.FrameOut(() => this.that.SetState(MyRoomStateMaterial.STATE.EVENT));
            }

            internal void <>m__15B(MaterialEventLogListViewItem itm)
            {
                this.that.FrameOut(() => this.that.SetState(MyRoomStateMaterial.STATE.FREE));
            }

            internal void <>m__15C(MaterialEventLogListViewItem itm)
            {
                this.that.mPlayMakerFSM.SendEvent("CLICK_OPENING");
            }

            internal void <>m__15E()
            {
                this.that.mPlayMakerFSM.SendEvent("CLICK_MATERIAL_COLLECTION");
            }

            internal void <>m__15F()
            {
                this.that.SetState(MyRoomStateMaterial.STATE.MAP);
            }

            internal void <>m__160()
            {
                this.that.SetState(MyRoomStateMaterial.STATE.STORY);
            }

            internal void <>m__161()
            {
                this.that.SetState(MyRoomStateMaterial.STATE.EVENT);
            }

            internal void <>m__162()
            {
                this.that.SetState(MyRoomStateMaterial.STATE.FREE);
            }
        }
    }
}

