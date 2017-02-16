using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/DebugListViewMenu")]
public class DebugListViewMenu : MonoBehaviour
{
    public DebugListViewManager listViewManager;
    protected State state;

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.listViewManager.CreateList();
        }
        this.state = State.INPUT;
        this.listViewManager.SetMode(DebugListViewManager.InitMode.INPUT);
    }

    public void StartInput()
    {
        if (this.state == State.INPUT)
        {
            this.listViewManager.SetMode(DebugListViewManager.InitMode.INPUT);
        }
    }

    protected enum State
    {
        INIT,
        INPUT
    }
}

