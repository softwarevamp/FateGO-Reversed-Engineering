using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MissionActionManager : MonoBehaviour
{
    [CompilerGenerated]
    private static Predicate<EventMissionActionEntity> <>f__am$cache1;
    private List<EventMissionActionEntity> evMissionActionList;

    public static bool checkScroll(int missionID, MissionProgressType.Type progressType)
    {
        if (<>f__am$cache1 == null)
        {
            <>f__am$cache1 = x => x.missionActionType == 3;
        }
        return SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionActionMaster>(DataNameKind.Kind.EVENT_MISSION_ACTION).getEntityListFromIDnType(missionID, progressType).Exists(<>f__am$cache1);
    }

    public void doMissionAction()
    {
        if (this.evMissionActionList.Count <= 0)
        {
            this.SetBackGroundUntouchableWhileMissionAction(false);
            UnityEngine.Object.Destroy(base.gameObject);
        }
        else
        {
            EventMissionActionEntity entity = this.evMissionActionList[0];
            switch (entity.GetMissionActionType())
            {
                case EventMissionActionEntity.MissionActionType.TALK:
                    ScriptManager.PlayScenario(entity.getValMessage(), delegate (bool skip) {
                        this.SetBackGroundUntouchableWhileMissionAction(true);
                        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, new System.Action(this.doMissionAction));
                    }, false);
                    break;

                case EventMissionActionEntity.MissionActionType.SYSTEM_WINDOW:
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenNotificationDialog(string.Empty, entity.getValMessage(), () => this.doMissionAction(), 150);
                    break;

                case EventMissionActionEntity.MissionActionType.SCROLL_MISSION:
                    if (SingletonMonoBehaviour<SceneManager>.Instance.targetRoot.checkSceneName("EventGachaScene"))
                    {
                        ((EventGachaRootComponent) SingletonMonoBehaviour<SceneManager>.Instance.targetRoot).missionItemListViewManager.setNextMissionInfo(entity.getValID(), new System.Action(this.doMissionAction));
                    }
                    break;

                case EventMissionActionEntity.MissionActionType.TRANSITION_TERMINAL:
                {
                    TerminalTransitionInfo data = new TerminalTransitionInfo {
                        missionId = entity.missionId,
                        afterActionVals = entity.vals
                    };
                    this.SetBackGroundUntouchableWhileMissionAction(false);
                    SingletonMonoBehaviour<SceneManager>.Instance.transitionScene(SceneList.Type.Terminal, SceneManager.FadeType.BLACK, data);
                    UnityEngine.Object.Destroy(base.gameObject);
                    break;
                }
                case EventMissionActionEntity.MissionActionType.VOICE:
                    if (SingletonMonoBehaviour<SceneManager>.Instance.targetRoot.checkSceneName("EventGachaScene"))
                    {
                        ((EventGachaRootComponent) SingletonMonoBehaviour<SceneManager>.Instance.targetRoot).playEventMissionSvtVoice(entity.getValMessage());
                    }
                    UnityEngine.Object.Destroy(base.gameObject);
                    break;

                case EventMissionActionEntity.MissionActionType.IMAGE_WINDOW:
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenImageDialogWithAssets(entity.vals, new System.Action(this.doMissionAction));
                    break;
            }
            this.evMissionActionList.RemoveAt(0);
        }
    }

    private void SetBackGroundUntouchableWhileMissionAction(bool onoff)
    {
        SingletonMonoBehaviour<AutomatedAction>.Instance.SetBackGroundUntouchableWhileMissionAction(onoff);
    }

    public void setMissionAction(int missionID, MissionProgressType.Type progressType)
    {
        this.evMissionActionList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionActionMaster>(DataNameKind.Kind.EVENT_MISSION_ACTION).getEntityListFromIDnType(missionID, progressType);
        this.SetBackGroundUntouchableWhileMissionAction(true);
        this.doMissionAction();
    }
}

