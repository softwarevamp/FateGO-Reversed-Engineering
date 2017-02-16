using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ClassCompatibilityMenu : BaseMonoBehaviour
{
    [SerializeField]
    protected ClassCompatibilityInfoDialog classCompatibilityConfirmMenu;
    protected State state;

    protected event System.Action callbackFunc;

    protected event System.Action closeCallbackFunc;

    protected void Callback()
    {
        System.Action callbackFunc = this.callbackFunc;
        this.callbackFunc = null;
        if (callbackFunc != null)
        {
            callbackFunc();
        }
    }

    public void Close(System.Action callback = null)
    {
        if (this.state != State.INIT)
        {
            this.closeCallbackFunc = callback;
            this.state = State.CLOSE;
            this.classCompatibilityConfirmMenu.Close(new System.Action(this.EndClose));
        }
    }

    protected void EndClose()
    {
        if (this.state == State.CLOSE)
        {
            this.classCompatibilityConfirmMenu.Init();
            base.gameObject.SetActive(false);
            this.state = State.INIT;
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            if (closeCallbackFunc != null)
            {
                this.closeCallbackFunc = null;
                closeCallbackFunc();
            }
        }
    }

    public void Open(int questId, int questPhase, System.Action callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            base.gameObject.SetActive(true);
            this.state = State.INPUT;
            this.classCompatibilityConfirmMenu.Open(questId, questPhase, new System.Action(this.Callback));
        }
    }

    protected enum State
    {
        INIT,
        INPUT,
        SELECTED,
        CLOSE
    }
}

