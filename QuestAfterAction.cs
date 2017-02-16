using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class QuestAfterAction : SingletonMonoBehaviour<QuestAfterAction>
{
    [CompilerGenerated]
    private static Converter<string, int> <>f__am$cacheA;
    public const float ANIM_TIME = 0.4f;
    public const UITweener.Method ANIM_TWEEN_METHOD = UITweener.Method.EaseOut;
    private const int COMMAND_SIZE = 2;
    private BoxCollider mColl;
    private Command[] mCommandBuf;
    private int mCommandIdx;
    private System.Action mEndAct;
    private const string MESSAGE_KEY_PREFIX = "QUEST_AFTER_ACTION_MESSAGE_";
    private CStateManager<QuestAfterAction> mFSM;
    [SerializeField]
    private GameObject mGimmickRoot;
    private SrcSpotBasePrefab mLastDispSpot;
    [SerializeField]
    private MapCamera mMapCamera;
    [SerializeField]
    private GameObject mRoadRoot;
    [SerializeField]
    private GameObject mSpotRoot;

    public void CreateCommandBuf()
    {
        int[] afterAction = null;
        string[] afterActionVals = null;
        if (TerminalSceneComponent.Instance.TransitionInfo != null)
        {
            int missionId = TerminalSceneComponent.Instance.TransitionInfo.missionId;
            if (missionId > 0)
            {
                afterActionVals = TerminalSceneComponent.Instance.TransitionInfo.afterActionVals;
                int eventId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionMaster>(DataNameKind.Kind.EVENT_MISSION).getEntityFromId<EventMissionEntity>(missionId).eventId;
                TerminalPramsManager.WarId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<WarMaster>(DataNameKind.Kind.WAR).getByEventId(eventId).getWarId();
            }
        }
        if (afterActionVals == null)
        {
            if (!TerminalPramsManager.IsQuestClear)
            {
                return;
            }
            if (TerminalPramsManager.Debug_IsQuestReleaseAll)
            {
                return;
            }
            int questId = TerminalPramsManager.QuestId;
            QuestEntity entity3 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(questId);
            if (entity3 == null)
            {
                return;
            }
            afterAction = entity3.afterAction;
        }
        else
        {
            int[][] numArray2 = new int[afterActionVals.Length][];
            for (int i = 0; i < afterActionVals.Length; i++)
            {
                char[] separator = new char[] { ',' };
                string[] array = afterActionVals[i].Replace("[", string.Empty).Replace("]", string.Empty).Split(separator);
                if (<>f__am$cacheA == null)
                {
                    <>f__am$cacheA = delegate (string input) {
                        int num;
                        if (int.TryParse(input, out num))
                        {
                            return num;
                        }
                        return 0;
                    };
                }
                numArray2[i] = Array.ConvertAll<string, int>(array, <>f__am$cacheA);
            }
            List<int> list = new List<int>();
            foreach (int[] numArray3 in numArray2)
            {
                foreach (int num6 in numArray3)
                {
                    list.Add(num6);
                }
            }
            afterAction = list.ToArray();
        }
        if ((afterAction != null) && ((afterAction.Length > 1) && ((afterAction.Length % 2) == 0)))
        {
            this.mCommandBuf = new Command[afterAction.Length / 2];
            int index = 0;
            for (int j = 0; j < this.mCommandBuf.Length; j++)
            {
                this.mCommandBuf[j] = new Command { 
                    id = afterAction[index],
                    prm = afterAction[index + 1]
                };
                index += 2;
            }
            TerminalPramsManager.IsAutoResume = true;
            TerminalPramsManager.DispState = TerminalPramsManager.eDispState.Map;
            TerminalPramsManager.SpotId = -1;
        }
    }

    private MapGimmickComponent GetGimmick(int id)
    {
        Transform transform = this.mGimmickRoot.transform.FindChild(MapGimmickComponent.GetGobjName(id));
        return transform?.gameObject.GetComponent<MapGimmickComponent>();
    }

    public SrcSpotBasePrefab GetLastDispSpot() => 
        this.mLastDispSpot;

    private srcLineSprite GetRoad(int id)
    {
        Transform transform = this.mRoadRoot.transform.FindChild(srcLineSprite.GetGobjName(id));
        return transform?.gameObject.GetComponent<srcLineSprite>();
    }

    private SrcSpotBasePrefab GetSpot(int id)
    {
        Transform transform = this.mSpotRoot.transform.FindChild(SrcSpotBasePrefab.GetGobjName(id));
        return transform?.gameObject.GetComponent<SrcSpotBasePrefab>();
    }

    public STATE GetState() => 
        ((STATE) this.mFSM.getState());

    public void Init()
    {
        if (this.mFSM == null)
        {
            this.mFSM = new CStateManager<QuestAfterAction>(this, 2);
            this.mFSM.add(0, new StateNone());
            this.mFSM.add(1, new StateMain());
        }
        this.SetState(STATE.NONE);
        this.mCommandBuf = null;
        this.mCommandIdx = 0;
        this.mColl = base.gameObject.GetComponent<BoxCollider>();
        this.mColl.enabled = false;
        this.mLastDispSpot = null;
    }

    public bool IsActiveCommand() => 
        (this.mCommandBuf != null);

    public bool IsPlaying() => 
        (this.GetState() == STATE.MAIN);

    public void Play(System.Action end_act = null)
    {
        this.mEndAct = end_act;
        if (this.mCommandBuf != null)
        {
            this.SetState(STATE.MAIN);
        }
        else
        {
            this.mEndAct.Call();
        }
    }

    public void SetState(STATE state)
    {
        this.mFSM.setState((int) state);
    }

    private void Update()
    {
        if (this.mFSM != null)
        {
            this.mFSM.update();
        }
    }

    private class Command
    {
        public int id;
        public int prm;
    }

    public enum COMMAND
    {
        CAM_MV_GIMMICK = 0x12e,
        CAM_MV_ROAD = 0x12d,
        CAM_MV_SPOT = 300,
        EVENT_REWARD = 700,
        GIMMICK_DISP = 0x191,
        GIMMICK_DISP_QUICK = 0x193,
        GIMMICK_HIDE = 400,
        GIMMICK_HIDE_QUICK = 0x192,
        MESSAGE_WINDOW = 600,
        QUEST_FOCUS = 500,
        ROAD_DISP = 0xca,
        ROAD_GRAY = 0xc9,
        ROAD_HIDE = 200,
        SPOT_DISP = 0x66,
        SPOT_GRAY = 0x65,
        SPOT_HIDE = 100
    }

    public enum STATE
    {
        NONE,
        MAIN,
        SIZEOF
    }

    private class StateMain : IState<QuestAfterAction>
    {
        private bool mIsAnimDoing;
        private QuestAfterAction mThat;

        public void begin(QuestAfterAction that)
        {
            this.mThat = that;
            this.mThat.mColl.enabled = true;
        }

        public void end(QuestAfterAction that)
        {
        }

        private void EndAnim()
        {
            this.mIsAnimDoing = false;
            this.mThat.mCommandIdx++;
            if (this.mThat.mCommandIdx >= this.mThat.mCommandBuf.Length)
            {
                this.mThat.mEndAct.Call();
                this.mThat.mColl.enabled = false;
                this.mThat.Init();
            }
        }

        public void update(QuestAfterAction that)
        {
            this.UpdateAnim(that);
        }

        private void UpdateAnim(QuestAfterAction that)
        {
            if (!this.mIsAnimDoing)
            {
                this.mIsAnimDoing = true;
                QuestAfterAction.Command command = that.mCommandBuf[that.mCommandIdx];
                switch (((QuestAfterAction.COMMAND) command.id))
                {
                    case QuestAfterAction.COMMAND.GIMMICK_HIDE:
                    {
                        <UpdateAnim>c__AnonStoreyBA yba = new <UpdateAnim>c__AnonStoreyBA {
                            <>f__this = this,
                            mg = that.GetGimmick(command.prm)
                        };
                        yba.mg.SetState(MapGimmickComponent.STATE.HIDE_ANIM, new System.Action(yba.<>m__1B2));
                        break;
                    }
                    case QuestAfterAction.COMMAND.GIMMICK_DISP:
                    {
                        <UpdateAnim>c__AnonStoreyBB ybb = new <UpdateAnim>c__AnonStoreyBB {
                            <>f__this = this,
                            mg = that.GetGimmick(command.prm)
                        };
                        ybb.mg.SetState(MapGimmickComponent.STATE.DISP_ANIM, new System.Action(ybb.<>m__1B3));
                        break;
                    }
                    case QuestAfterAction.COMMAND.GIMMICK_HIDE_QUICK:
                    {
                        MapGimmickComponent gimmick = that.GetGimmick(command.prm);
                        gimmick.SetAlphaAnimQuick(false);
                        gimmick.GetMapCtrl_MapGimmickInfo().mfSetDispType(clsMapCtrl_MapGimmickInfo.enDispType.None);
                        this.EndAnim();
                        this.UpdateAnim(that);
                        break;
                    }
                    case QuestAfterAction.COMMAND.GIMMICK_DISP_QUICK:
                    {
                        MapGimmickComponent component3 = that.GetGimmick(command.prm);
                        component3.SetAlphaAnimQuick(true);
                        component3.GetMapCtrl_MapGimmickInfo().mfSetDispType(clsMapCtrl_MapGimmickInfo.enDispType.Normal);
                        this.EndAnim();
                        this.UpdateAnim(that);
                        break;
                    }
                    case QuestAfterAction.COMMAND.SPOT_HIDE:
                    {
                        <UpdateAnim>c__AnonStoreyB4 yb = new <UpdateAnim>c__AnonStoreyB4 {
                            <>f__this = this,
                            spt = that.GetSpot(command.prm)
                        };
                        yb.spt.SetState(SrcSpotBasePrefab.STATE.QAA_HIDE, new System.Action(yb.<>m__1A9));
                        break;
                    }
                    case QuestAfterAction.COMMAND.SPOT_GRAY:
                    {
                        <UpdateAnim>c__AnonStoreyB5 yb2 = new <UpdateAnim>c__AnonStoreyB5 {
                            <>f__this = this,
                            spt = that.GetSpot(command.prm)
                        };
                        yb2.spt.SetState(SrcSpotBasePrefab.STATE.QAA_GRAY, new System.Action(yb2.<>m__1AA));
                        break;
                    }
                    case QuestAfterAction.COMMAND.SPOT_DISP:
                    {
                        <UpdateAnim>c__AnonStoreyB6 yb3 = new <UpdateAnim>c__AnonStoreyB6 {
                            <>f__this = this,
                            spt = that.GetSpot(command.prm)
                        };
                        yb3.spt.SetState(SrcSpotBasePrefab.STATE.QAA_DISP, new System.Action(yb3.<>m__1AB));
                        that.mLastDispSpot = yb3.spt;
                        break;
                    }
                    case QuestAfterAction.COMMAND.ROAD_HIDE:
                    {
                        <UpdateAnim>c__AnonStoreyB7 yb4 = new <UpdateAnim>c__AnonStoreyB7 {
                            <>f__this = this,
                            rd = that.GetRoad(command.prm)
                        };
                        yb4.rd.SetState(srcLineSprite.STATE.QAA_HIDE, new System.Action(yb4.<>m__1AC));
                        break;
                    }
                    case QuestAfterAction.COMMAND.ROAD_GRAY:
                    {
                        <UpdateAnim>c__AnonStoreyB8 yb5 = new <UpdateAnim>c__AnonStoreyB8 {
                            <>f__this = this,
                            rd = that.GetRoad(command.prm)
                        };
                        yb5.rd.SetState(srcLineSprite.STATE.QAA_GRAY, new System.Action(yb5.<>m__1AD));
                        break;
                    }
                    case QuestAfterAction.COMMAND.ROAD_DISP:
                    {
                        <UpdateAnim>c__AnonStoreyB9 yb6 = new <UpdateAnim>c__AnonStoreyB9 {
                            <>f__this = this,
                            rd = that.GetRoad(command.prm)
                        };
                        yb6.rd.SetState(srcLineSprite.STATE.QAA_DISP, new System.Action(yb6.<>m__1AE));
                        break;
                    }
                    case QuestAfterAction.COMMAND.CAM_MV_SPOT:
                    {
                        SrcSpotBasePrefab spot = that.GetSpot(command.prm);
                        Vector3 localPosition = spot.gameObject.GetLocalPosition();
                        localPosition.y += spot.mcSpotSprite.height / 2;
                        that.mMapCamera.Scrl.StartAutoMove(localPosition, 0.4f, () => this.EndAnim());
                        break;
                    }
                    case QuestAfterAction.COMMAND.CAM_MV_ROAD:
                    {
                        Vector3 screenPos = that.GetRoad(command.prm).gameObject.GetLocalPosition();
                        that.mMapCamera.Scrl.StartAutoMove(screenPos, 0.4f, () => this.EndAnim());
                        break;
                    }
                    case QuestAfterAction.COMMAND.CAM_MV_GIMMICK:
                    {
                        Vector3 vector3 = that.GetGimmick(command.prm).gameObject.GetLocalPosition();
                        that.mMapCamera.Scrl.StartAutoMove(vector3, 0.4f, () => this.EndAnim());
                        break;
                    }
                    case QuestAfterAction.COMMAND.QUEST_FOCUS:
                    {
                        int prm = command.prm;
                        QuestEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<QuestMaster>(DataNameKind.Kind.QUEST).getEntityFromId<QuestEntity>(prm);
                        TerminalPramsManager.QuestId = prm;
                        that.GetSpot(entity.getSpotId()).cbfBtn_Click();
                        this.EndAnim();
                        break;
                    }
                    case QuestAfterAction.COMMAND.MESSAGE_WINDOW:
                    {
                        string title = string.Empty;
                        string message = LocalizationManager.Get("QUEST_AFTER_ACTION_MESSAGE_" + command.prm.ToString());
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(title, message, () => this.EndAnim(), -1);
                        break;
                    }
                    case QuestAfterAction.COMMAND.EVENT_REWARD:
                    {
                        int id = command.prm;
                        SceneJumpInfo data = new SceneJumpInfo(string.Empty, id);
                        SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.EventGacha, SceneManager.FadeType.BLACK, data);
                        this.EndAnim();
                        break;
                    }
                }
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAnim>c__AnonStoreyB4
        {
            internal QuestAfterAction.StateMain <>f__this;
            internal SrcSpotBasePrefab spt;

            internal void <>m__1A9()
            {
                this.spt.GetMapCtrl_SpotInfo().mfSetDispType(clsMapCtrl_SpotInfo.enDispType.None);
                this.spt.SetTouchType(clsMapCtrl_SpotInfo.enTouchType.Disable);
                this.<>f__this.EndAnim();
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAnim>c__AnonStoreyB5
        {
            internal QuestAfterAction.StateMain <>f__this;
            internal SrcSpotBasePrefab spt;

            internal void <>m__1AA()
            {
                this.spt.GetMapCtrl_SpotInfo().mfSetDispType(clsMapCtrl_SpotInfo.enDispType.Glay);
                this.spt.SetTouchType(clsMapCtrl_SpotInfo.enTouchType.Disable);
                this.<>f__this.EndAnim();
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAnim>c__AnonStoreyB6
        {
            internal QuestAfterAction.StateMain <>f__this;
            internal SrcSpotBasePrefab spt;

            internal void <>m__1AB()
            {
                this.spt.GetMapCtrl_SpotInfo().mfSetDispType(clsMapCtrl_SpotInfo.enDispType.Normal);
                this.spt.SetTouchType(clsMapCtrl_SpotInfo.enTouchType.Enable);
                this.<>f__this.EndAnim();
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAnim>c__AnonStoreyB7
        {
            internal QuestAfterAction.StateMain <>f__this;
            internal srcLineSprite rd;

            internal void <>m__1AC()
            {
                this.rd.GetMapCtrl_SpotRoadInfo().mfSetDispType(clsMapCtrl_SpotRoadInfo.enDispType.None);
                this.<>f__this.EndAnim();
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAnim>c__AnonStoreyB8
        {
            internal QuestAfterAction.StateMain <>f__this;
            internal srcLineSprite rd;

            internal void <>m__1AD()
            {
                this.rd.GetMapCtrl_SpotRoadInfo().mfSetDispType(clsMapCtrl_SpotRoadInfo.enDispType.Glay);
                this.<>f__this.EndAnim();
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAnim>c__AnonStoreyB9
        {
            internal QuestAfterAction.StateMain <>f__this;
            internal srcLineSprite rd;

            internal void <>m__1AE()
            {
                this.rd.GetMapCtrl_SpotRoadInfo().mfSetDispType(clsMapCtrl_SpotRoadInfo.enDispType.Normal);
                this.<>f__this.EndAnim();
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAnim>c__AnonStoreyBA
        {
            internal QuestAfterAction.StateMain <>f__this;
            internal MapGimmickComponent mg;

            internal void <>m__1B2()
            {
                this.mg.GetMapCtrl_MapGimmickInfo().mfSetDispType(clsMapCtrl_MapGimmickInfo.enDispType.None);
                this.<>f__this.EndAnim();
            }
        }

        [CompilerGenerated]
        private sealed class <UpdateAnim>c__AnonStoreyBB
        {
            internal QuestAfterAction.StateMain <>f__this;
            internal MapGimmickComponent mg;

            internal void <>m__1B3()
            {
                this.mg.GetMapCtrl_MapGimmickInfo().mfSetDispType(clsMapCtrl_MapGimmickInfo.enDispType.Normal);
                this.<>f__this.EndAnim();
            }
        }
    }

    private class StateNone : IState<QuestAfterAction>
    {
        public void begin(QuestAfterAction that)
        {
        }

        public void end(QuestAfterAction that)
        {
        }

        public void update(QuestAfterAction that)
        {
        }
    }
}

