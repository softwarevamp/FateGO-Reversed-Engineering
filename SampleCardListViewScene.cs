using System;
using UnityEngine;

[AddComponentMenu("Sample/TestListView/SampleCardListViewScene")]
public class SampleCardListViewScene : MonoBehaviour
{
    public SampleCardListViewManager cardListViewManager;
    public int cardSum;
    protected State state;

    public void Init()
    {
        if (this.state == State.INIT)
        {
            this.cardListViewManager.CreateList(this.cardSum);
        }
        this.state = State.INIT_MOVE;
        this.cardListViewManager.SetMode(SampleCardListViewManager.InitMode.INTO, new System.Action(this.OnMoveEnd));
    }

    private void OnMoveEnd()
    {
        switch (this.state)
        {
            case State.INIT_MOVE:
                this.state = State.INIT_TURN;
                this.cardListViewManager.SetMode(SampleCardListViewManager.InitMode.INTO_TURN, new System.Action(this.OnMoveEnd));
                break;

            case State.INIT_TURN:
                this.state = State.INPUT;
                this.cardListViewManager.SetMode(SampleCardListViewManager.InitMode.INPUT);
                break;
        }
    }

    private void Start()
    {
        this.cardListViewManager.IsInput = false;
    }

    protected enum State
    {
        INIT,
        INIT_MOVE,
        INIT_TURN,
        INPUT
    }
}

