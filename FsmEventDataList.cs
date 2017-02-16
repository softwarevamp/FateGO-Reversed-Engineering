using System;
using UnityEngine;

[AddComponentMenu("NGUI/ListView/FsmEventDataList")]
public class FsmEventDataList : MonoBehaviour
{
    [HideInInspector, SerializeField]
    protected FsmEventData[] eventDataList;
    [SerializeField]
    protected PlayMakerFSM targetFSM;

    ~FsmEventDataList()
    {
    }

    public FsmEventData Get(int index) => 
        this.eventDataList[index];

    public string GetEventData(int index) => 
        this.eventDataList[index].EventData;

    public string GetTitle(int index) => 
        this.eventDataList[index].Title;

    public void SendEvent(int index)
    {
        if (this.targetFSM != null)
        {
            this.targetFSM.SendEvent(this.eventDataList[index].EventData);
        }
    }

    public int Length
    {
        get => 
            ((this.eventDataList == null) ? 0 : this.eventDataList.Length);
        set
        {
            int length = this.Length;
            if (length != value)
            {
                FsmEventData[] dataArray = new FsmEventData[value];
                for (int i = 0; i < value; i++)
                {
                    if (i >= length)
                    {
                        dataArray[i] = new FsmEventData();
                    }
                    else
                    {
                        dataArray[i] = this.eventDataList[i];
                    }
                }
                this.eventDataList = dataArray;
            }
        }
    }

    public PlayMakerFSM TargetFSM
    {
        get => 
            this.targetFSM;
        set
        {
            this.targetFSM = value;
        }
    }
}

