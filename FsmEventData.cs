using System;
using UnityEngine;

[Serializable, AddComponentMenu("NGUI/ListView/FsmEventData")]
public class FsmEventData
{
    [SerializeField]
    protected string eventData = "none";
    [SerializeField]
    protected string title = string.Empty;

    ~FsmEventData()
    {
    }

    public string EventData
    {
        get => 
            this.eventData;
        set
        {
            this.eventData = value;
        }
    }

    public string Title
    {
        get => 
            this.title;
        set
        {
            this.title = value;
        }
    }
}

