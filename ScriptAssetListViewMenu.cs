using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptAssetListViewMenu")]
public class ScriptAssetListViewMenu : MonoBehaviour
{
    public ScriptAssetListViewManager listViewManager;
    public ScriptObjectListViewMenu scriptObjectListViewMenu;
    public UIButton scriptTestAssetCancelButton;
    public GameObject scriptTestAssetRootObject;
    protected string selectAssetPath;
    protected string selectObjectPath;
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
            this.state = State.INIT;
        }
        this.scriptTestAssetRootObject.SetActive(false);
    }

    public void EndInput()
    {
        if (this.state != State.INIT)
        {
            this.listViewManager.IsInput = false;
            this.scriptTestAssetCancelButton.enabled = false;
        }
    }

    protected void EndPlayScriptDebug(bool isExit)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(0.1f, null);
        this.state = State.INPUT_OBJECT_MENU;
        this.scriptObjectListViewMenu.Open(this.selectAssetPath, new ScriptObjectListViewMenu.CallbackFunc(this.OnEndSelectObject));
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

    protected void OnClickItem()
    {
        if (this.state == State.INPUT)
        {
            int clickResult = this.listViewManager.GetClickResult();
            if (clickResult >= 0)
            {
                ScriptAssetListViewItem item = this.listViewManager.GetItem(clickResult);
                this.selectAssetPath = item.Path;
                this.state = State.INPUT_OBJECT_MENU;
                this.scriptObjectListViewMenu.Open(this.selectAssetPath, new ScriptObjectListViewMenu.CallbackFunc(this.OnEndSelectObject));
            }
        }
    }

    protected void OnEndSelectObject(ScriptObjectListViewMenu.ResultKind result, string path)
    {
        if (this.state == State.INPUT_OBJECT_MENU)
        {
            switch (result)
            {
                case ScriptObjectListViewMenu.ResultKind.PLAY:
                case ScriptObjectListViewMenu.ResultKind.VIEW_PLAY:
                {
                    int jumpLine = this.scriptObjectListViewMenu.GetJumpLine();
                    this.selectObjectPath = path;
                    string selectAssetPath = this.selectAssetPath;
                    string selectObjectPath = this.selectObjectPath;
                    if (selectAssetPath.StartsWith(ScriptManager.textPath) && (selectObjectPath != string.Empty))
                    {
                        string startModeForAssetStorage = ScriptManager.GetStartModeForAssetStorage(selectAssetPath, selectObjectPath);
                        int num2 = selectAssetPath.LastIndexOf("/");
                        this.state = State.PLAY_SCRIPT;
                        ScriptManager.DebugPlay(startModeForAssetStorage, (num2 < 0) ? selectAssetPath : selectAssetPath.Substring(num2 + 1), selectObjectPath, jumpLine, new ScriptManager.CallbackFunc(this.EndPlayScriptDebug));
                    }
                    return;
                }
            }
            this.scriptObjectListViewMenu.Close();
            this.state = State.INPUT;
            this.listViewManager.SetMode(ScriptAssetListViewManager.InitMode.INPUT, new System.Action(this.OnClickItem));
            this.scriptTestAssetCancelButton.enabled = true;
        }
    }

    public void Open(CallbackFunc callback)
    {
        if ((this.state == State.INIT) || (this.state == State.CLOSE))
        {
            this.callbackFunc = callback;
            this.scriptTestAssetRootObject.SetActive(true);
            this.listViewManager.IsInput = false;
            this.scriptTestAssetCancelButton.enabled = false;
            this.listViewManager.CreateList();
        }
        this.state = State.INIT_MOVE;
        this.StartInput();
    }

    public void StartInput()
    {
        this.state = State.INPUT;
        this.listViewManager.SetMode(ScriptAssetListViewManager.InitMode.INPUT, new System.Action(this.OnClickItem));
        this.scriptTestAssetCancelButton.enabled = true;
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        INIT_MOVE,
        INPUT,
        INPUT_OBJECT_MENU,
        PLAY_SCRIPT,
        SELECTED,
        CLOSE
    }
}

