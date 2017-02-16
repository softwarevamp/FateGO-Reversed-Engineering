using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptObjectListViewMenu")]
public class ScriptObjectListViewMenu : MonoBehaviour
{
    public UILineInput jumpLineObjectInput;
    public ScriptObjectListViewManager listViewManager;
    protected string path;
    protected AssetData scriptAsset;
    public UILabel scriptTestDefaultNameLabel;
    public UIButton scriptTestObjectCancelButton;
    public GameObject scriptTestObjectRootObject;
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
        if (this.scriptAsset != null)
        {
            AssetManager.releaseAsset(this.scriptAsset);
            this.scriptAsset = null;
        }
        this.scriptTestObjectRootObject.SetActive(false);
    }

    public void EndInput()
    {
        if (this.state != State.INIT)
        {
            this.listViewManager.IsInput = false;
            this.scriptTestObjectCancelButton.enabled = false;
            this.jumpLineObjectInput.SetInputEnable(false);
        }
    }

    protected void EndLoadAsset(AssetData data)
    {
        this.scriptAsset = data;
        this.listViewManager.CreateList(data);
        this.StartInput();
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

    protected void OnClickItem(ScriptObjectListViewManager.ResultKind result, ScriptObjectListViewItem item)
    {
        if (this.state == State.INPUT)
        {
            switch (result)
            {
                case ScriptObjectListViewManager.ResultKind.PLAY:
                    this.state = State.SELECTED;
                    this.EndInput();
                    this.Callback(ResultKind.PLAY, item.Path);
                    break;

                case ScriptObjectListViewManager.ResultKind.VIEW_PLAY:
                    this.state = State.SELECTED;
                    this.EndInput();
                    this.Callback(ResultKind.VIEW_PLAY, item.Path);
                    break;
            }
        }
    }

    public void Open(string assetName, CallbackFunc callback)
    {
        if ((this.state == State.INIT) || (this.state == State.CLOSE))
        {
            this.path = assetName;
            this.callbackFunc = callback;
            this.scriptTestObjectRootObject.SetActive(true);
            this.listViewManager.IsInput = false;
            this.scriptTestObjectCancelButton.enabled = false;
            this.jumpLineObjectInput.SetInputEnable(false);
            int num = this.path.LastIndexOf('/');
            this.scriptTestDefaultNameLabel.text = (num >= 0) ? this.path.Substring(num + 1) : this.path;
            if (this.scriptAsset != null)
            {
                AssetManager.releaseAsset(this.scriptAsset);
                this.scriptAsset = null;
            }
            this.state = State.LOAD;
            AssetManager.loadAssetStorage(this.path, new AssetLoader.LoadEndDataHandler(this.EndLoadAsset));
        }
        else if (this.state == State.SELECTED)
        {
            this.callbackFunc = callback;
            this.StartInput();
        }
    }

    protected void StartInput()
    {
        this.state = State.INPUT;
        this.listViewManager.SetMode(ScriptObjectListViewManager.InitMode.INPUT, new ScriptObjectListViewManager.CallbackFunc(this.OnClickItem));
        this.scriptTestObjectCancelButton.enabled = true;
        this.jumpLineObjectInput.SetInputEnable(true);
    }

    public delegate void CallbackFunc(ScriptObjectListViewMenu.ResultKind result, string objectName);

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
        INPUT,
        INPUT_OBJECT_MENU,
        SELECTED,
        CLOSE
    }
}

