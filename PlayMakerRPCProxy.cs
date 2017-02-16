using System;
using UnityEngine;

public class PlayMakerRPCProxy : MonoBehaviour
{
    public PlayMakerFSM[] fsms;

    public void ForwardEvent(string eventName)
    {
        foreach (PlayMakerFSM rfsm in this.fsms)
        {
            rfsm.SendEvent(eventName);
        }
    }

    public void Reset()
    {
        this.fsms = base.GetComponents<PlayMakerFSM>();
    }
}

