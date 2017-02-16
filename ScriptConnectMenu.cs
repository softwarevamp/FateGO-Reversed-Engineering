using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptConnectMenu")]
public class ScriptConnectMenu : MonoBehaviour
{
    public UILineInput dataScriptObjectInput;
    public UILineInput dataScriptPathInput;
    public DebugTestRootComponent debugTestRootComponent;
    public UIPopupList genderInput;
    public UILineInput jumpLineObjectInput;
    public ScriptConnectListViewMenu scriptConnectListViewMenu;
    protected string selectConnectPath;
    protected int selectGenderIndex;
    public UIButton selectListButton;
    protected string selectObjectPath;
    protected string selectStartModeName;
    public UIButton serverCancelButton;
    public UIButton serverDecideButton;
    public GameObject serverSettingRootObject;
    public UIPopupList startModeInput;
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
        this.state = State.INIT;
        this.serverSettingRootObject.SetActive(false);
    }

    protected void EndConnectScriptPlay()
    {
        this.state = State.INPUT_OBJECT_MENU;
        this.scriptConnectListViewMenu.Open(this.selectConnectPath, this.selectObjectPath, this.GetJumpLineString(), new ScriptConnectListViewMenu.CallbackFunc(this.OnEndSelectObject));
    }

    protected void EndInput()
    {
        if (this.state != State.INIT)
        {
            UIInput component = this.dataScriptPathInput.GetComponent<UIInput>();
            UIInput input2 = this.dataScriptObjectInput.GetComponent<UIInput>();
            component.value = string.Empty;
            input2.value = string.Empty;
            this.dataScriptPathInput.SetInputEnable(false);
            this.dataScriptObjectInput.SetInputEnable(false);
            this.jumpLineObjectInput.SetInputEnable(false);
            this.genderInput.enabled = false;
            this.startModeInput.enabled = true;
            this.serverDecideButton.enabled = false;
            this.serverCancelButton.enabled = false;
            Input.imeCompositionMode = IMECompositionMode.Auto;
        }
    }

    protected void EndPlayScript()
    {
        this.state = State.INPUT;
    }

    public int GetJumpLine()
    {
        string text = this.jumpLineObjectInput.GetText();
        return (!string.IsNullOrEmpty(text) ? int.Parse(text) : -1);
    }

    public string GetJumpLineString() => 
        this.jumpLineObjectInput.GetText();

    public void OnChangeServerInput()
    {
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

    public void OnClickDecide()
    {
        if (this.state == State.INPUT)
        {
            this.selectConnectPath = this.dataScriptPathInput.GetText();
            this.selectObjectPath = this.dataScriptObjectInput.GetText();
            this.selectGenderIndex = Gender.ToData(this.genderInput.value);
            this.selectStartModeName = this.startModeInput.value;
            if (ScriptManager.SetScriptServerSetting(this.selectConnectPath, this.selectObjectPath, this.selectGenderIndex, this.selectStartModeName))
            {
                SingletonMonoBehaviour<ScriptManager>.Instance.WriteSetting();
            }
            this.state = State.PLAY_SCRIPT;
            this.debugTestRootComponent.StartConnectScript(this.selectConnectPath, this.selectObjectPath, new System.Action(this.EndPlayScript), new System.Action(this.EndPlayScript), this.GetJumpLine(), false);
        }
    }

    public void OnClickSelect()
    {
        if (this.state == State.INPUT)
        {
            this.selectConnectPath = this.dataScriptPathInput.GetText();
            this.selectGenderIndex = Gender.ToData(this.genderInput.value);
            this.selectStartModeName = this.startModeInput.value;
            this.state = State.INPUT_OBJECT_MENU;
            this.scriptConnectListViewMenu.Open(this.selectConnectPath, this.selectObjectPath, this.GetJumpLineString(), new ScriptConnectListViewMenu.CallbackFunc(this.OnEndSelectObject));
        }
    }

    protected void OnEndSelectObject(ScriptConnectListViewMenu.ResultKind result, string path)
    {
        if (this.state == State.INPUT_OBJECT_MENU)
        {
            switch (result)
            {
                case ScriptConnectListViewMenu.ResultKind.PLAY:
                case ScriptConnectListViewMenu.ResultKind.VIEW_PLAY:
                {
                    this.selectObjectPath = path;
                    UIInput component = this.dataScriptObjectInput.GetComponent<UIInput>();
                    UIInput input2 = this.jumpLineObjectInput.GetComponent<UIInput>();
                    component.value = this.selectObjectPath;
                    input2.value = this.scriptConnectListViewMenu.GetJumpLineString();
                    if (ScriptManager.SetScriptServerSetting(this.selectConnectPath, this.selectObjectPath, this.selectGenderIndex, this.selectStartModeName))
                    {
                        SingletonMonoBehaviour<ScriptManager>.Instance.WriteSetting();
                    }
                    this.state = State.PLAY_SCRIPT;
                    this.debugTestRootComponent.StartConnectScript(this.selectConnectPath, this.selectObjectPath, new System.Action(this.EndConnectScriptPlay), new System.Action(this.EndConnectScriptPlay), this.GetJumpLine(), result == ScriptConnectListViewMenu.ResultKind.VIEW_PLAY);
                    return;
                }
            }
            this.scriptConnectListViewMenu.Close();
            this.state = State.INPUT;
            this.dataScriptPathInput.SetInputEnable(true);
            this.serverDecideButton.enabled = true;
            this.serverCancelButton.enabled = true;
        }
    }

    public void Open(CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            SingletonMonoBehaviour<ScriptManager>.Instance.ReadSetting();
            this.selectConnectPath = ScriptManager.GetScriptServerSetting();
            this.selectObjectPath = ScriptManager.GetScriptObjectSetting();
            this.selectGenderIndex = ScriptManager.GetScriptGenderSetting();
            this.selectStartModeName = ScriptManager.GetScriptStartModeSetting();
            this.serverSettingRootObject.SetActive(true);
            this.dataScriptPathInput.SetInputEnable(true);
            this.dataScriptObjectInput.SetInputEnable(true);
            this.jumpLineObjectInput.SetInputEnable(true);
            this.genderInput.enabled = true;
            this.startModeInput.enabled = true;
            this.serverDecideButton.enabled = true;
            this.serverCancelButton.enabled = true;
            UIInput component = this.dataScriptPathInput.GetComponent<UIInput>();
            UIInput input2 = this.dataScriptObjectInput.GetComponent<UIInput>();
            component.value = this.selectConnectPath;
            input2.value = this.selectObjectPath;
            this.genderInput.value = ((Gender.Type) this.selectGenderIndex).ToString();
            this.startModeInput.value = this.selectStartModeName;
            this.state = State.INPUT;
        }
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        INPUT,
        INPUT_OBJECT_MENU,
        SELECTED,
        CLOSE,
        PLAY_SCRIPT
    }
}

