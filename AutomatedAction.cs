using System;
using UnityEngine;

public class AutomatedAction : SingletonMonoBehaviour<AutomatedAction>
{
    public GameObject blocker;
    public GameObject missionActionManagerPrefab;

    public void SetBackGroundUntouchableWhileMissionAction(bool onoff)
    {
        this.blocker.SetActive(onoff);
    }

    public void SetMissionAction(int missionID, MissionProgressType.Type progressType)
    {
        GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.missionActionManagerPrefab);
        obj2.transform.parent = base.transform;
        obj2.transform.localPosition = Vector3.zero;
        obj2.transform.localScale = Vector3.one;
        obj2.GetComponent<MissionActionManager>().setMissionAction(missionID, progressType);
    }
}

