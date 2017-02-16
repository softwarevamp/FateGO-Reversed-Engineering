using System;
using UnityEngine;

[AddComponentMenu("Sample/Test2ListView/SampleEventListViewScene")]
public class SampleEventListViewScene : MonoBehaviour
{
    public int listSum;
    public SampleEventListViewManager listViewManager;
    protected State state;

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.listViewManager.CreateList(this.listSum);
        }
        this.state = State.INIT_MOVE;
        this.listViewManager.SetMode(SampleEventListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
    }

    private void OnMoveEnd()
    {
        if (this.state == State.INIT_MOVE)
        {
            this.state = State.INPUT;
            this.listViewManager.SetMode(SampleEventListViewManager.InitMode.INPUT);
        }
    }

    private void Start()
    {
        this.listViewManager.IsInput = false;
    }

    protected enum State
    {
        INIT,
        INIT_MOVE,
        INPUT
    }
}

