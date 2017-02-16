using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptFilePlayerMenu")]
public class ScriptFilePlayerMenu : MonoBehaviour
{
    public UILineInput dataScriptObjectInput;
    public UILineInput dataScriptPathInput;
    public DebugTestRootComponent debugTestRootComponent;
    public UIPopupList genderInput;
    public UILineInput jumpLineObjectInput;
    public ScriptPlayListViewMenu scriptPlayListViewMenu;
    protected int selectGenderIndex;
    public UIButton selectListButton;
    protected string selectObjectPath;
    protected string selectPlayerFilePath;
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

    protected void EndPlayScriptSelectObject()
    {
        this.state = State.INPUT_OBJECT_MENU;
        this.scriptPlayListViewMenu.Open(this.selectPlayerFilePath, this.selectObjectPath, this.GetJumpLineString(), new ScriptPlayListViewMenu.CallbackFunc(this.OnEndSelectObject));
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
            this.selectPlayerFilePath = this.dataScriptPathInput.GetText();
            this.selectObjectPath = this.dataScriptObjectInput.GetText();
            this.selectGenderIndex = Gender.ToData(this.genderInput.value);
            this.selectStartModeName = this.startModeInput.value;
            if (ScriptManager.SetScriptPlayerSetting(this.selectPlayerFilePath, this.selectObjectPath, this.selectGenderIndex, this.selectStartModeName))
            {
                SingletonMonoBehaviour<ScriptManager>.Instance.WriteSetting();
            }
            this.state = State.PLAY_SCRIPT;
            this.debugTestRootComponent.StartFileScript(this.selectPlayerFilePath, this.selectObjectPath, new System.Action(this.EndPlayScript), new System.Action(this.EndPlayScript), this.GetJumpLine(), false);
        }
    }

    public void OnClickSelect()
    {
        if (this.state == State.INPUT)
        {
            this.selectPlayerFilePath = this.dataScriptPathInput.GetText();
            this.selectGenderIndex = Gender.ToData(this.genderInput.value);
            this.selectStartModeName = this.startModeInput.value;
            this.state = State.INPUT_OBJECT_MENU;
            this.scriptPlayListViewMenu.Open(this.selectPlayerFilePath, this.selectObjectPath, this.GetJumpLineString(), new ScriptPlayListViewMenu.CallbackFunc(this.OnEndSelectObject));
        }
    }

    protected void OnEndSelectObject(ScriptPlayListViewMenu.ResultKind result, string path)
    {
        if (this.state == State.INPUT_OBJECT_MENU)
        {
            switch (result)
            {
                case ScriptPlayListViewMenu.ResultKind.PLAY:
                case ScriptPlayListViewMenu.ResultKind.VIEW_PLAY:
                {
                    this.selectObjectPath = path;
                    UIInput component = this.dataScriptObjectInput.GetComponent<UIInput>();
                    UIInput input2 = this.jumpLineObjectInput.GetComponent<UIInput>();
                    component.value = this.selectObjectPath;
                    input2.value = this.scriptPlayListViewMenu.GetJumpLineString();
                    if (ScriptManager.SetScriptPlayerSetting(this.selectPlayerFilePath, this.selectObjectPath, this.selectGenderIndex, this.selectStartModeName))
                    {
                        SingletonMonoBehaviour<ScriptManager>.Instance.WriteSetting();
                    }
                    this.state = State.PLAY_SCRIPT;
                    this.debugTestRootComponent.StartFileScript(this.selectPlayerFilePath, this.selectObjectPath, new System.Action(this.EndPlayScriptSelectObject), new System.Action(this.EndPlayScriptSelectObject), this.GetJumpLine(), result == ScriptPlayListViewMenu.ResultKind.VIEW_PLAY);
                    return;
                }
            }
            this.scriptPlayListViewMenu.Close();
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
            this.selectPlayerFilePath = ScriptManager.GetScriptPlayerPathSetting();
            this.selectObjectPath = ScriptManager.GetScriptPlayerObjectSetting();
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
            component.value = this.selectPlayerFilePath;
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

