using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TerminalDebugWindow : SingletonTemplate<TerminalDebugWindow>
{
    [CompilerGenerated]
    private static Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> <>f__am$cache20;
    [CompilerGenerated]
    private static Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> <>f__am$cache21;
    [CompilerGenerated]
    private static Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> <>f__am$cache22;
    [CompilerGenerated]
    private static Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> <>f__am$cache23;
    [CompilerGenerated]
    private static Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> <>f__am$cache24;
    [CompilerGenerated]
    private static Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> <>f__am$cache25;
    [CompilerGenerated]
    private static Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> <>f__am$cache26;
    [CompilerGenerated]
    private static Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage> <>f__am$cache27;
    private List<DebugWindow> mDebugWindowList = new List<DebugWindow>();
    private GameObject mDebugWindowPrefab;
    private DebugWindow mHeroineFriendshipWin;
    private DebugWindow mHeroineLimitCountWin;
    private DebugWindow mHeroineTreasureWin;
    private static bool mIsHeroineFriendship_StartReq;
    private static bool mIsHeroineLimitCount_StartReq;
    private static bool mIsHeroineTreasure_IsRunkUp;
    private static int mIsHeroineTreasure_TreasureDvcId;
    private static int mIsHeroineTreasure_TreasureDvcLv;
    private static bool mIsQuestClearSkill_IsRunkUp;
    private static int mIsQuestClearSkill_Num;
    private static int mIsQuestClearSkill_Priority;
    private static int mIsQuestClearSkill_SvtId;
    private static bool mIsQuestClearTreasure_IsRunkUp;
    private static int mIsQuestClearTreasure_TreasureDeviceId;
    private DebugWindow mMissionNotifyWin;
    private GameObject mParentObj;
    private DebugWindow mQuestAfterWin;
    private DebugWindow mQuestClearActionWin;
    private DebugWindow mQuestClearSkillWin;
    private DebugWindow mQuestClearTreasureWin;
    private static int mQuestReward_IsNew;
    private static int mQuestReward_ItemCount;
    private static int mQuestReward_LimitCount;
    private static int mQuestReward_ObjectId;
    private static int mQuestReward_TreasureBoxId;
    private static int mQuestReward_UserSvtId;
    private DebugWindow mQuestRewardWin;
    private static int mTop_IsEarthLineEnable;
    private static int mTop_QuestAfterQuestId;
    private DebugWindow mTopWin;

    public void ClearHeroineFriendshipVal()
    {
        mIsHeroineFriendship_StartReq = false;
    }

    public void ClearHeroineLimitCountVal()
    {
        mIsHeroineLimitCount_StartReq = false;
    }

    public void ClearHeroineTreasureVal()
    {
        mIsHeroineTreasure_IsRunkUp = false;
        mIsHeroineTreasure_TreasureDvcId = 0;
        mIsHeroineTreasure_TreasureDvcLv = 0;
    }

    public void ClearQuestAfterVal()
    {
        mTop_QuestAfterQuestId = 0;
    }

    public void ClearQuestClearSkillVal()
    {
        mIsQuestClearSkill_IsRunkUp = false;
        mIsQuestClearSkill_SvtId = 0;
        mIsQuestClearSkill_Num = 0;
        mIsQuestClearSkill_Priority = 0;
    }

    public void ClearQuestClearTreasureVal()
    {
        mIsQuestClearTreasure_IsRunkUp = false;
        mIsQuestClearTreasure_TreasureDeviceId = 0;
    }

    public void ClearQuestRewardVal()
    {
        mQuestReward_TreasureBoxId = 0;
        mQuestReward_ObjectId = 0;
        mQuestReward_ItemCount = 0;
        mQuestReward_IsNew = 0;
        mQuestReward_UserSvtId = 0;
        mQuestReward_LimitCount = 0;
    }

    private DebugWindow Create(string gobj_name)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.mDebugWindowPrefab);
        obj2.transform.parent = this.mParentObj.transform;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localRotation = Quaternion.identity;
        obj2.transform.localScale = Vector3.one;
        obj2.name = obj2.name + "_" + gobj_name;
        DebugWindow component = obj2.GetComponent<DebugWindow>();
        this.mDebugWindowList.Add(component);
        return component;
    }

    public bool IsActive()
    {
        foreach (DebugWindow window in this.mDebugWindowList)
        {
            if (window.IsActiveSafe())
            {
                return true;
            }
        }
        return false;
    }

    public void Open()
    {
        if (this.mTopWin != null)
        {
            this.mTopWin.Open();
        }
    }

    public void Setup(GameObject debug_window_prefab, GameObject parent_obj)
    {
        this.mDebugWindowPrefab = debug_window_prefab;
        this.mParentObj = parent_obj;
        this.SetupMissionNotifyWin();
        this.SetupQuestAfterWin();
        this.SetupQuestClearTreasureWin();
        this.SetupQuestClearSkillWin();
        this.SetupQuestRewardWin();
        this.SetupHeroineFriendshipWin();
        this.SetupHeroineTreasureWin();
        this.SetupHeroineLimitCountWin();
        this.SetupQuestClearActionWin();
        this.SetupTopWin();
    }

    private void SetupHeroineFriendshipWin()
    {
        if (this.mHeroineFriendshipWin == null)
        {
            string str = "HeroineFriendship";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("Start", DebugWindowScrollItem.Type.PUSH_ONLY, mIsHeroineFriendship_StartReq) };
            this.mHeroineFriendshipWin = this.Create(str);
            if (<>f__am$cache23 == null)
            {
                <>f__am$cache23 = delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                    if ((item.Idx == 0) && (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK))
                    {
                        mIsHeroineFriendship_StartReq = true;
                        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                    }
                };
            }
            this.mHeroineFriendshipWin.Setup(str, iinfs, <>f__am$cache23);
        }
    }

    private void SetupHeroineLimitCountWin()
    {
        if (this.mHeroineLimitCountWin == null)
        {
            string str = "HeroineLimitCount";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("Start", DebugWindowScrollItem.Type.PUSH_ONLY, mIsHeroineLimitCount_StartReq) };
            this.mHeroineLimitCountWin = this.Create(str);
            if (<>f__am$cache21 == null)
            {
                <>f__am$cache21 = delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                    if ((item.Idx == 0) && (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK))
                    {
                        mIsHeroineLimitCount_StartReq = true;
                        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                    }
                };
            }
            this.mHeroineLimitCountWin.Setup(str, iinfs, <>f__am$cache21);
        }
    }

    private void SetupHeroineTreasureWin()
    {
        if (this.mHeroineTreasureWin == null)
        {
            string str = "HeroineTreasure";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("Start", DebugWindowScrollItem.Type.PUSH_ONLY, 0), new DebugWindowScrollItem.ItemInfo("IsRankUp", DebugWindowScrollItem.Type.TOGGLE, mIsHeroineTreasure_IsRunkUp), new DebugWindowScrollItem.ItemInfo("TreasureDvcId", DebugWindowScrollItem.Type.VAL_EDIT, mIsHeroineTreasure_TreasureDvcId), new DebugWindowScrollItem.ItemInfo("TreasureDvcLv", DebugWindowScrollItem.Type.VAL_EDIT, mIsHeroineTreasure_TreasureDvcLv) };
            this.mHeroineTreasureWin = this.Create(str);
            if (<>f__am$cache22 == null)
            {
                <>f__am$cache22 = delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                    switch (item.Idx)
                    {
                        case 0:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                                break;
                            }
                            break;

                        case 1:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                mIsHeroineTreasure_IsRunkUp = item.Val != 0;
                                break;
                            }
                            break;

                        case 2:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mIsHeroineTreasure_TreasureDvcId = item.Val;
                                break;
                            }
                            break;

                        case 3:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mIsHeroineTreasure_TreasureDvcLv = item.Val;
                                break;
                            }
                            break;
                    }
                };
            }
            this.mHeroineTreasureWin.Setup(str, iinfs, <>f__am$cache22);
        }
    }

    private void SetupMissionNotifyWin()
    {
        if (this.mMissionNotifyWin == null)
        {
            string str = "MissionNotify";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("Start", DebugWindowScrollItem.Type.PUSH_ONLY, string.Empty), new DebugWindowScrollItem.ItemInfo("RequestDispCount", DebugWindowScrollItem.Type.VAL_EDIT, 10), new DebugWindowScrollItem.ItemInfo("MissionNotifyCount", DebugWindowScrollItem.Type.DISP_ONLY, string.Empty), new DebugWindowScrollItem.ItemInfo("Pause", DebugWindowScrollItem.Type.TOGGLE, SingletonTemplate<MissionNotifyManager>.Instance.IsPause()), new DebugWindowScrollItem.ItemInfo("ClearRequest", DebugWindowScrollItem.Type.PUSH_ONLY, string.Empty), new DebugWindowScrollItem.ItemInfo("Drag", DebugWindowScrollItem.Type.TOGGLE, false) };
            this.mMissionNotifyWin = this.Create(str);
            this.mMissionNotifyWin.Setup(str, iinfs, delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                switch (item.Idx)
                {
                    case 0:
                        if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                        {
                            int val = this.mMissionNotifyWin.GetVal(1);
                            for (int j = 0; j < val; j++)
                            {
                                MissionNotifyDispInfo info = new MissionNotifyDispInfo {
                                    eventMissionId = j + 1
                                };
                                if ((j & 1) == 0)
                                {
                                    info.message = "ミッション文字列";
                                    info.progressFrom = 50;
                                    info.progressTo = 0x4b;
                                    info.condition = 0x7d;
                                }
                                else
                                {
                                    info.message = "ミッション文字列ミッション文字列ミッション文字列ミッション文字列ミッション文字列ミッション文字列ミッション文字列";
                                    info.progressFrom = 40;
                                    info.progressTo = 50;
                                    info.condition = 50;
                                }
                                SingletonTemplate<MissionNotifyManager>.Instance.RequestDisp(info);
                            }
                        }
                        break;

                    case 2:
                        if (cbm == DebugWindowScrollItem.CallBackMessage.UPDATE)
                        {
                            item.Val = SingletonTemplate<MissionNotifyManager>.Instance.GetDispInfoCount();
                        }
                        break;

                    case 3:
                        if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                        {
                            if (item.IsEnable)
                            {
                                SingletonTemplate<MissionNotifyManager>.Instance.StartPause();
                            }
                            else
                            {
                                SingletonTemplate<MissionNotifyManager>.Instance.EndPause();
                            }
                            break;
                        }
                        break;

                    case 4:
                        if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                        {
                            SingletonTemplate<MissionNotifyManager>.Instance.ClearRequest();
                            break;
                        }
                        break;
                }
            });
        }
    }

    private void SetupQuestAfterWin()
    {
        if (this.mQuestAfterWin == null)
        {
            string str = "QuestAfter";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("Start", DebugWindowScrollItem.Type.PUSH_ONLY, 0), new DebugWindowScrollItem.ItemInfo("QuestId", DebugWindowScrollItem.Type.VAL_EDIT, mTop_QuestAfterQuestId) };
            this.mQuestAfterWin = this.Create(str);
            if (<>f__am$cache27 == null)
            {
                <>f__am$cache27 = delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                    switch (item.Idx)
                    {
                        case 0:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                TerminalPramsManager.IsAutoResume = true;
                                TerminalPramsManager.IsQuestClear = true;
                                TerminalPramsManager.QuestId = mTop_QuestAfterQuestId;
                                TerminalPramsManager.WarId = 100 + ((mTop_QuestAfterQuestId - 0xf4240) / 100);
                                SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                            }
                            break;

                        case 1:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mTop_QuestAfterQuestId = item.Val;
                            }
                            break;
                    }
                };
            }
            this.mQuestAfterWin.Setup(str, iinfs, <>f__am$cache27);
        }
    }

    private void SetupQuestClearActionWin()
    {
        if (this.mQuestClearActionWin == null)
        {
            string str = "QuestClearAction";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo(this.mHeroineLimitCountWin), new DebugWindowScrollItem.ItemInfo(this.mHeroineTreasureWin), new DebugWindowScrollItem.ItemInfo(this.mHeroineFriendshipWin), new DebugWindowScrollItem.ItemInfo(this.mQuestRewardWin), new DebugWindowScrollItem.ItemInfo(this.mQuestClearSkillWin), new DebugWindowScrollItem.ItemInfo(this.mQuestClearTreasureWin), new DebugWindowScrollItem.ItemInfo(this.mQuestAfterWin) };
            this.mQuestClearActionWin = this.Create(str);
            if (<>f__am$cache20 == null)
            {
                <>f__am$cache20 = delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                };
            }
            this.mQuestClearActionWin.Setup(str, iinfs, <>f__am$cache20);
        }
    }

    private void SetupQuestClearSkillWin()
    {
        if (this.mQuestClearSkillWin == null)
        {
            string str = "QuestClearSkill";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("Start", DebugWindowScrollItem.Type.PUSH_ONLY, 0), new DebugWindowScrollItem.ItemInfo("IsRankUp", DebugWindowScrollItem.Type.TOGGLE, mIsQuestClearSkill_IsRunkUp), new DebugWindowScrollItem.ItemInfo("SvtId", DebugWindowScrollItem.Type.VAL_EDIT, mIsQuestClearSkill_SvtId), new DebugWindowScrollItem.ItemInfo("Num", DebugWindowScrollItem.Type.VAL_EDIT, mIsQuestClearSkill_Num), new DebugWindowScrollItem.ItemInfo("Priority", DebugWindowScrollItem.Type.VAL_EDIT, mIsQuestClearSkill_Priority) };
            this.mQuestClearSkillWin = this.Create(str);
            if (<>f__am$cache25 == null)
            {
                <>f__am$cache25 = delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                    switch (item.Idx)
                    {
                        case 0:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                                break;
                            }
                            break;

                        case 1:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                mIsQuestClearSkill_IsRunkUp = item.Val != 0;
                                break;
                            }
                            break;

                        case 2:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mIsQuestClearSkill_SvtId = item.Val;
                                break;
                            }
                            break;

                        case 3:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mIsQuestClearSkill_Num = item.Val;
                                break;
                            }
                            break;

                        case 4:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mIsQuestClearSkill_Priority = item.Val;
                                break;
                            }
                            break;
                    }
                };
            }
            this.mQuestClearSkillWin.Setup(str, iinfs, <>f__am$cache25);
        }
    }

    private void SetupQuestClearTreasureWin()
    {
        if (this.mQuestClearTreasureWin == null)
        {
            string str = "QuestClearTreasure";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("Start", DebugWindowScrollItem.Type.PUSH_ONLY, 0), new DebugWindowScrollItem.ItemInfo("IsRankUp", DebugWindowScrollItem.Type.TOGGLE, mIsQuestClearTreasure_IsRunkUp), new DebugWindowScrollItem.ItemInfo("TreasureDeviceId", DebugWindowScrollItem.Type.VAL_EDIT, mIsQuestClearTreasure_TreasureDeviceId) };
            this.mQuestClearTreasureWin = this.Create(str);
            if (<>f__am$cache26 == null)
            {
                <>f__am$cache26 = delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                    switch (item.Idx)
                    {
                        case 0:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                                break;
                            }
                            break;

                        case 1:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                mIsQuestClearTreasure_IsRunkUp = item.Val != 0;
                                break;
                            }
                            break;

                        case 2:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mIsQuestClearTreasure_TreasureDeviceId = item.Val;
                                break;
                            }
                            break;
                    }
                };
            }
            this.mQuestClearTreasureWin.Setup(str, iinfs, <>f__am$cache26);
        }
    }

    private void SetupQuestRewardWin()
    {
        if (this.mQuestRewardWin == null)
        {
            string str = "QuestReward";
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("Start", DebugWindowScrollItem.Type.PUSH_ONLY, 0), new DebugWindowScrollItem.ItemInfo("TreasureBox", DebugWindowScrollItem.Type.VAL_EDIT, mQuestReward_TreasureBoxId), new DebugWindowScrollItem.ItemInfo("ObjectId", DebugWindowScrollItem.Type.VAL_EDIT, mQuestReward_ObjectId), new DebugWindowScrollItem.ItemInfo("ItemCount", DebugWindowScrollItem.Type.VAL_EDIT, mQuestReward_ItemCount), new DebugWindowScrollItem.ItemInfo("IsNew", DebugWindowScrollItem.Type.TOGGLE, mQuestReward_IsNew), new DebugWindowScrollItem.ItemInfo("UserSvtId", DebugWindowScrollItem.Type.VAL_EDIT, mQuestReward_UserSvtId), new DebugWindowScrollItem.ItemInfo("LimitCount", DebugWindowScrollItem.Type.VAL_EDIT, mQuestReward_LimitCount) };
            this.mQuestRewardWin = this.Create(str);
            if (<>f__am$cache24 == null)
            {
                <>f__am$cache24 = delegate (DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm) {
                    switch (item.Idx)
                    {
                        case 0:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                                break;
                            }
                            break;

                        case 1:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mQuestReward_TreasureBoxId = item.Val;
                                break;
                            }
                            break;

                        case 2:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mQuestReward_ObjectId = item.Val;
                                break;
                            }
                            break;

                        case 3:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mQuestReward_ItemCount = item.Val;
                                break;
                            }
                            break;

                        case 4:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                            {
                                mQuestReward_IsNew = item.Val;
                                break;
                            }
                            break;

                        case 5:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mQuestReward_UserSvtId = item.Val;
                                break;
                            }
                            break;

                        case 6:
                            if (cbm == DebugWindowScrollItem.CallBackMessage.EDIT)
                            {
                                mQuestReward_LimitCount = item.Val;
                                break;
                            }
                            break;
                    }
                };
            }
            this.mQuestRewardWin.Setup(str, iinfs, <>f__am$cache24);
        }
    }

    private void SetupTopWin()
    {
        <SetupTopWin>c__AnonStoreyD7 yd = new <SetupTopWin>c__AnonStoreyD7();
        if (this.mTopWin == null)
        {
            yd.win_name = "Top";
            string str = (("Server: " + NetworkManager.UserCreateServer) + "\nUserId: " + NetworkManager.UserId) + "\n" + yd.win_name;
            DebugWindowScrollItem.ItemInfo[] iinfs = new DebugWindowScrollItem.ItemInfo[] { new DebugWindowScrollItem.ItemInfo("QuestReleaseAll", DebugWindowScrollItem.Type.TOGGLE, TerminalPramsManager.Debug_IsQuestReleaseAll), new DebugWindowScrollItem.ItemInfo("RestartTerminal", DebugWindowScrollItem.Type.PUSH_ONLY, string.Empty), new DebugWindowScrollItem.ItemInfo("GoToTitle", DebugWindowScrollItem.Type.PUSH_ONLY, string.Empty), new DebugWindowScrollItem.ItemInfo("WarStartActionSkip", DebugWindowScrollItem.Type.TOGGLE, TerminalPramsManager.Debug_IsWarStartActionSkip), new DebugWindowScrollItem.ItemInfo("UserNameEntry", DebugWindowScrollItem.Type.PUSH_ONLY, string.Empty), new DebugWindowScrollItem.ItemInfo("EarthLine", DebugWindowScrollItem.Type.TOGGLE, mTop_IsEarthLineEnable), new DebugWindowScrollItem.ItemInfo(this.mMissionNotifyWin), new DebugWindowScrollItem.ItemInfo(this.mQuestClearActionWin), new DebugWindowScrollItem.ItemInfo("Push Exception!!", DebugWindowScrollItem.Type.PUSH_ONLY, string.Empty) };
            this.mTopWin = this.Create(yd.win_name);
            this.mTopWin.Setup(str, iinfs, new Action<DebugWindowScrollItem, DebugWindowScrollItem.CallBackMessage>(yd.<>m__20C));
        }
    }

    public DebugWindow HeroineFriendshipWin =>
        this.mHeroineFriendshipWin;

    public DebugWindow HeroineLimitCountWin =>
        this.mHeroineLimitCountWin;

    public DebugWindow HeroineTreasureWin =>
        this.mHeroineTreasureWin;

    public DebugWindow MissionNotifyWin =>
        this.mMissionNotifyWin;

    public DebugWindow QuestAfterWin =>
        this.mQuestAfterWin;

    public DebugWindow QuestClearActionWin =>
        this.mQuestClearActionWin;

    public DebugWindow QuestClearSkillWin =>
        this.mQuestClearSkillWin;

    public DebugWindow QuestClearTreasureWin =>
        this.mQuestClearTreasureWin;

    public DebugWindow QuestRewardWin =>
        this.mQuestRewardWin;

    public DebugWindow TopWin =>
        this.mTopWin;

    [CompilerGenerated]
    private sealed class <SetupTopWin>c__AnonStoreyD7
    {
        private static System.Action <>f__am$cache1;
        private static System.Action <>f__am$cache2;
        internal string win_name;

        internal void <>m__20C(DebugWindowScrollItem item, DebugWindowScrollItem.CallBackMessage cbm)
        {
            <SetupTopWin>c__AnonStoreyD6 yd = new <SetupTopWin>c__AnonStoreyD6 {
                <>f__ref$215 = this,
                item = item
            };
            switch (yd.item.Idx)
            {
                case 0:
                    if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                    {
                        NetworkManager.getRequest<DebugQuestRequest>(new NetworkManager.ResultCallbackFunc(yd.<>m__216)).beginRequest(yd.item.IsEnable);
                        break;
                    }
                    break;

                case 1:
                    if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                    {
                        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                        break;
                    }
                    break;

                case 2:
                    if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                    {
                        if (<>f__am$cache1 == null)
                        {
                            <>f__am$cache1 = () => SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
                        }
                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, <>f__am$cache1);
                        break;
                    }
                    break;

                case 3:
                    if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                    {
                        TerminalPramsManager.Debug_IsWarStartActionSkip = yd.item.IsEnable;
                        break;
                    }
                    break;

                case 4:
                    if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                    {
                        if (<>f__am$cache2 == null)
                        {
                            <>f__am$cache2 = (System.Action) (() => SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null));
                        }
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenUserNameEntry(<>f__am$cache2);
                        break;
                    }
                    break;

                case 5:
                    if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                    {
                        TerminalDebugWindow.mTop_IsEarthLineEnable = yd.item.Val;
                        SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                        break;
                    }
                    break;

                case 8:
                    if (cbm == DebugWindowScrollItem.CallBackMessage.ON_CLICK)
                    {
                        this.win_name = null;
                        this.win_name.ToString();
                    }
                    break;
            }
        }

        private static void <>m__217()
        {
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }

        private static void <>m__218()
        {
            SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
        }

        private sealed class <SetupTopWin>c__AnonStoreyD6
        {
            internal TerminalDebugWindow.<SetupTopWin>c__AnonStoreyD7 <>f__ref$215;
            internal DebugWindowScrollItem item;

            internal void <>m__216(string result)
            {
                if (result == "ok")
                {
                    TerminalPramsManager.Debug_IsQuestReleaseAll = this.item.IsEnable;
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionSceneRefresh(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, null);
                }
                else
                {
                    this.item.Toggle();
                }
            }
        }
    }
}

