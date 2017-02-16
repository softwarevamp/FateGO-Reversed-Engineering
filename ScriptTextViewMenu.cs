using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptTextViewMenu")]
public class ScriptTextViewMenu : MonoBehaviour
{
    public UIButton cancelButton;
    public GameObject menuRootObject;
    public ScriptTextListViewManager scriptTextListViewManager;
    protected State state;
    protected string[] textLineData;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result, int jumpLine = -1)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, jumpLine);
        }
    }

    public void Close()
    {
        this.state = State.INIT;
        this.scriptTextListViewManager.DestroyList();
        this.menuRootObject.SetActive(false);
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.Callback(ResultKind.CANCEL, -1);
        }
    }

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.Callback(ResultKind.PLAY, -1);
        }
    }

    protected void OnClickItem(ScriptTextListViewManager.ResultKind result, ScriptTextListViewItem item)
    {
        if (this.state == State.INPUT)
        {
            switch (result)
            {
                case ScriptTextListViewManager.ResultKind.PLAY:
                    this.state = State.SELECTED;
                    this.Callback(ResultKind.PLAY, -1);
                    break;

                case ScriptTextListViewManager.ResultKind.JUMP_PLAY:
                    this.state = State.SELECTED;
                    this.Callback(ResultKind.JUMP_PLAY, item.Index);
                    break;
            }
        }
    }

    public void Open(string textData, int jumpLine = -1, CallbackFunc callback = null)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.menuRootObject.SetActive(true);
            if (textData != null)
            {
                this.textLineData = textData.Replace("\r", string.Empty).Split(new char[] { '뮿', '﻿', '￾', '\n' }, StringSplitOptions.None);
            }
            else
            {
                this.textLineData = null;
            }
            this.scriptTextListViewManager.CreateList(this.textLineData);
            if (jumpLine >= 0)
            {
                this.scriptTextListViewManager.JumpItem(jumpLine);
            }
            this.state = State.INPUT;
            this.cancelButton.enabled = true;
            this.scriptTextListViewManager.SetMode(ScriptTextListViewManager.InitMode.INPUT, new ScriptTextListViewManager.CallbackFunc(this.OnClickItem));
        }
    }

    public delegate void CallbackFunc(ScriptTextViewMenu.ResultKind result, int jumpLine);

    public enum ResultKind
    {
        NONE,
        CANCEL,
        PLAY,
        JUMP_PLAY
    }

    protected enum State
    {
        INIT,
        INPUT,
        SELECTED,
        CLOSE
    }
}

