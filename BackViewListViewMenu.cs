using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/BackViewListViewMenu")]
public class BackViewListViewMenu : MonoBehaviour
{
    protected string[] backAssetList;
    public UIButton cancelButton;
    public BackViewListViewManager listViewManager;
    public GameObject rootObject;
    protected State state;

    protected event CallbackFunc callbackFunc;

    protected void Callback(bool result)
    {
        CallbackFunc callbackFunc = this.callbackFunc;
        if (callbackFunc != null)
        {
            this.callbackFunc = null;
            callbackFunc(result);
        }
    }

    public void Close()
    {
        this.EndInput();
        if (this.state != State.INIT)
        {
            this.listViewManager.DestroyList();
            this.backAssetList = null;
            this.state = State.INIT;
        }
        this.rootObject.SetActive(false);
    }

    public void EndInput()
    {
        if (this.state != State.INIT)
        {
            this.listViewManager.IsInput = false;
            this.cancelButton.enabled = false;
        }
    }

    public void OnClickCancel()
    {
        if (this.state == State.INPUT)
        {
            this.EndInput();
            this.state = State.SELECTED;
            this.Callback(false);
        }
    }

    public void OnClickClear()
    {
        if (this.state == State.INPUT)
        {
            ScriptManager.BackViewClear();
        }
    }

    protected void OnClickItem()
    {
        if (this.state == State.INPUT)
        {
            int clickResult = this.listViewManager.GetClickResult();
            if (clickResult >= 0)
            {
                BackViewListViewItem item = this.listViewManager.GetItem(clickResult);
                this.state = State.VIEW_BACK;
                ScriptManager.BackViewPlay(item.Path, this.backAssetList, new ScriptManager.CallbackFunc(this.OnEndBackView));
            }
        }
    }

    protected void OnEndBackView(bool isExit)
    {
        if (this.state == State.VIEW_BACK)
        {
            this.state = State.INPUT;
            this.listViewManager.SetMode(BackViewListViewManager.InitMode.INPUT, new System.Action(this.OnClickItem));
            this.cancelButton.enabled = true;
        }
    }

    public void Open(CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.backAssetList = AssetManager.getAssetStorageList("Back");
            this.rootObject.SetActive(true);
            this.listViewManager.IsInput = false;
            this.cancelButton.enabled = false;
            this.listViewManager.CreateList(this.backAssetList);
        }
        this.StartInput();
    }

    public void StartInput()
    {
        this.state = State.INPUT;
        this.listViewManager.SetMode(BackViewListViewManager.InitMode.INPUT, new System.Action(this.OnClickItem));
        this.cancelButton.enabled = true;
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        INIT_MOVE,
        INPUT,
        VIEW_BACK,
        SELECTED,
        CLOSE
    }
}

