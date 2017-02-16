using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptPlayListViewMenu")]
public class ScriptPlayListViewMenu : MonoBehaviour
{
    public UILineInput jumpLineObjectInput;
    public ScriptPlayListViewManager listViewManager;
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

    protected void OnClickItem(ScriptPlayListViewManager.ResultKind result, ScriptPlayListViewItem item)
    {
        if (this.state == State.INPUT)
        {
            switch (result)
            {
                case ScriptPlayListViewManager.ResultKind.PLAY:
                    this.state = State.SELECTED;
                    this.EndInput();
                    this.Callback(ResultKind.PLAY, item.Path);
                    break;

                case ScriptPlayListViewManager.ResultKind.VIEW_PLAY:
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
            this.RequestList(this.path);
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
            this.RequestList(this.path);
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.StartInput();
        }
    }

    protected bool RequestList(string path)
    {
        Debug.Log("PlayScriptLoadData: " + path + " start");
        string message = null;
        if (!Directory.Exists(path))
        {
            message = "not find directory [" + path + "]";
        }
        else
        {
            try
            {
                FileInfo[] files = new DirectoryInfo(path).GetFiles("*.txt");
                List<string> list = new List<string>();
                foreach (FileInfo info2 in files)
                {
                    string name = info2.Name;
                    char ch = name[0];
                    if (!ch.Equals('.'))
                    {
                        char ch2 = name[0];
                        if (!ch2.Equals('_'))
                        {
                            list.Add(name);
                        }
                    }
                }
                this.scriptFileList = list.ToArray();
            }
            catch (Exception exception)
            {
                message = exception.Message;
            }
        }
        if (message != null)
        {
            Debug.LogError("ConnectLoadData: " + message);
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("Connect Script Error", message, new ErrorDialog.ClickDelegate(this.OnClickErrorDialog), false);
            return false;
        }
        Debug.Log("ConnectLoadData: " + path + " end");
        this.listViewManager.CreateList(this.scriptFileList);
        this.StartInput();
        return true;
    }

    protected void StartInput()
    {
        this.state = State.INPUT;
        this.listViewManager.SetMode(ScriptPlayListViewManager.InitMode.INPUT, new ScriptPlayListViewManager.CallbackFunc(this.OnClickItem));
        this.scriptTestObjectCancelButton.enabled = true;
        this.scriptTestObjectUpdateButton.enabled = true;
        this.jumpLineObjectInput.SetInputEnable(true);
    }

    public delegate void CallbackFunc(ScriptPlayListViewMenu.ResultKind result, string objectName);

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

