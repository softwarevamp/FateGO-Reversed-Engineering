using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptConnectListViewMenu")]
public class ScriptConnectListViewMenu : MonoBehaviour
{
    public UILineInput jumpLineObjectInput;
    public ScriptConnectListViewManager listViewManager;
    protected string path;
    protected string[] scriptFileList;
    public UILabel scriptTestDefaultNameLabel;
    public UIButton scriptTestObjectCancelButton;
    public GameObject scriptTestObjectRootObject;
    public UIButton scriptTestObjectUpdateButton;
    protected State state;

    protected event CallbackFunc callbackFunc;

    protected void Callback(ResultKind result, string objectName)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result, objectName);
        }
    }

    public void Close()
    {
        this.EndInput();
        if (this.state != State.INIT)
        {
            this.listViewManager.DestroyList();
            this.state = State.INIT;
        }
        if (this.scriptFileList != null)
        {
            this.scriptFileList = null;
        }
        this.scriptTestObjectRootObject.SetActive(false);
    }

    public void EndInput()
    {
        if (this.state != State.INIT)
        {
            this.listViewManager.IsInput = false;
            this.scriptTestObjectCancelButton.enabled = false;
            this.scriptTestObjectUpdateButton.enabled = false;
            this.jumpLineObjectInput.SetInputEnable(false);
        }
    }

    public int GetJumpLine()
    {
        string text = this.jumpLineObjectInput.GetText();
        return (!string.IsNullOrEmpty(text) ? int.Parse(text) : -1);
    }

    public string GetJumpLineString() => 
        this.jumpLineObjectInput.GetText();

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.SELECTED;
            this.EndInput();
            this.Callback(ResultKind.CANCEL, null);
        }
    }

    protected void OnClickErrorDialog(bool isDecide)
    {
        this.state = State.SELECTED;
        this.EndInput();
        this.Callback(ResultKind.CANCEL, null);
    }

    protected void OnClickItem(ScriptConnectListViewManager.ResultKind result, ScriptConnectListViewItem item)
    {
        if (this.state == State.INPUT)
        {
            switch (result)
            {
                case ScriptConnectListViewManager.ResultKind.PLAY:
                    this.state = State.SELECTED;
                    this.EndInput();
                    this.Callback(ResultKind.PLAY, item.Path);
                    break;

                case ScriptConnectListViewManager.ResultKind.VIEW_PLAY:
                    this.state = State.SELECTED;
                    this.EndInput();
                    this.Callback(ResultKind.VIEW_PLAY, item.Path);
                    break;
            }
        }
    }

    public void OnClickUpdate()
    {
        if (this.state == State.INPUT)
        {
            this.state = State.LOAD;
            this.EndInput();
            this.listViewManager.DestroyList();
            this.scriptFileList = null;
            base.StartCoroutine(this.RequestListCR(this.path));
        }
    }

    public void Open(string path, string objectName, string jumpLine, CallbackFunc callback)
    {
        if ((this.state == State.INIT) || (this.state == State.CLOSE))
        {
            this.path = path;
            this.callbackFunc = callback;
            this.scriptTestObjectRootObject.SetActive(true);
            this.listViewManager.IsInput = false;
            this.scriptTestObjectCancelButton.enabled = false;
            this.scriptTestObjectUpdateButton.enabled = false;
            this.jumpLineObjectInput.SetInputEnable(false);
            this.scriptTestDefaultNameLabel.text = (this.path.Length <= 30) ? this.path : this.path.Substring(this.path.Length - 30);
            this.jumpLineObjectInput.GetComponent<UIInput>().value = jumpLine;
            this.state = State.LOAD;
            this.scriptFileList = null;
            base.StartCoroutine(this.RequestListCR(this.path));
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.StartInput();
        }
    }

    [DebuggerHidden]
    private IEnumerator RequestListCR(string path) => 
        new <RequestListCR>c__Iterator3A { 
            path = path,
            <$>path = path,
            <>f__this = this
        };

    protected void StartInput()
    {
        this.state = State.INPUT;
        this.listViewManager.SetMode(ScriptConnectListViewManager.InitMode.INPUT, new ScriptConnectListViewManager.CallbackFunc(this.OnClickItem));
        this.scriptTestObjectCancelButton.enabled = true;
        this.scriptTestObjectUpdateButton.enabled = true;
        this.jumpLineObjectInput.SetInputEnable(true);
    }

    [CompilerGenerated]
    private sealed class <RequestListCR>c__Iterator3A : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>path;
        internal ScriptConnectListViewMenu <>f__this;
        internal string <errorMessage>__4;
        internal string <fullPath>__0;
        internal WWW <loader>__1;
        internal float <loadProgress>__3;
        internal float <requestTime>__2;
        internal string path;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    Debug.Log("ConnectLoadData: " + this.path + " start");
                    this.<fullPath>__0 = this.path + "/scriptList.txt";
                    if (this.<fullPath>__0.StartsWith("file://"))
                    {
                        this.<fullPath>__0 = "file://" + WWW.EscapeURL(this.<fullPath>__0.Substring(7));
                    }
                    this.<loader>__1 = new WWW(this.<fullPath>__0);
                    this.<requestTime>__2 = Time.time + ManagerConfig.TIMEOUT;
                    this.<loadProgress>__3 = 0f;
                    this.<errorMessage>__4 = null;
                    break;

                case 1:
                    break;

                default:
                    goto Label_0259;
            }
            while (!this.<loader>__1.isDone)
            {
                if (this.<loader>__1.progress != this.<loadProgress>__3)
                {
                    this.<requestTime>__2 = Time.time + ManagerConfig.TIMEOUT;
                    this.<loadProgress>__3 = this.<loader>__1.progress;
                }
                else if (Time.time >= this.<requestTime>__2)
                {
                    Debug.LogWarning("TimeOut");
                    break;
                }
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                return true;
            }
            if (!this.<loader>__1.isDone)
            {
                this.<errorMessage>__4 = "file download time over";
            }
            else if (!string.IsNullOrEmpty(this.<loader>__1.error))
            {
                this.<errorMessage>__4 = this.<loader>__1.error;
            }
            else
            {
                this.<>f__this.scriptFileList = this.<loader>__1.text.Split(new char[] { '뮿', '﻿', '￾', '\r', '\n', '\0' }, StringSplitOptions.RemoveEmptyEntries);
            }
            this.<loader>__1.Dispose();
            if (this.<errorMessage>__4 != null)
            {
                Debug.LogError("ConnectLoadData: " + this.<errorMessage>__4);
                SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("Connect Script Error", this.<errorMessage>__4, new ErrorDialog.ClickDelegate(this.<>f__this.OnClickErrorDialog), false);
            }
            else
            {
                Debug.Log("ConnectLoadData: " + this.path + " end");
                this.<>f__this.listViewManager.CreateList(this.<>f__this.scriptFileList);
                this.<>f__this.StartInput();
            }
            this.$PC = -1;
        Label_0259:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }

    public delegate void CallbackFunc(ScriptConnectListViewMenu.ResultKind result, string objectName);

    public enum ResultKind
    {
        NONE,
        CANCEL,
        PLAY,
        VIEW_PLAY
    }

    protected enum State
    {
        INIT,
        LOAD,
        INIT_MOVE,
        INPUT,
        INPUT_OBJECT_MENU,
        SELECTED,
        CLOSE
    }
}

