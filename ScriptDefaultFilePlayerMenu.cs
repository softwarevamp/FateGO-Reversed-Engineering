using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ScriptDefaultFilePlayerMenu")]
public class ScriptDefaultFilePlayerMenu : MonoBehaviour
{
    public DebugTestRootComponent debugTestRootComponent;
    protected const string DEFAULT_SCRIPT_FILE_NAME = "DefaultScript.txt";
    protected const string DEFAULT_SCRIPT_PATH = "C:/FGO/Temporary/ScriptData";
    public UIPopupList genderInput;
    public UILineInput jumpLineObjectInput;
    protected int selectGenderIndex;
    protected string selectObjectPath;
    protected string selectPlayerFilePath;
    protected string selectStartModeName;
    public UIButton selectViewButton;
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
            this.selectGenderIndex = Gender.ToData(this.genderInput.value);
            this.selectStartModeName = this.startModeInput.value;
            if (ScriptManager.SetScriptPlayerSetting(this.selectPlayerFilePath, this.selectObjectPath, this.selectGenderIndex, this.selectStartModeName))
            {
                SingletonMonoBehaviour<ScriptManager>.Instance.WriteSetting();
            }
            this.state = State.PLAY_SCRIPT;
            this.debugTestRootComponent.StartFileScript("C:/FGO/Temporary/ScriptData", "DefaultScript.txt", new System.Action(this.EndPlayScript), new System.Action(this.EndPlayScript), this.GetJumpLine(), false);
        }
    }

    public void OnClickView()
    {
        if (this.state == State.INPUT)
        {
            this.selectGenderIndex = Gender.ToData(this.genderInput.value);
            this.selectStartModeName = this.startModeInput.value;
            if (ScriptManager.SetScriptPlayerSetting(this.selectPlayerFilePath, this.selectObjectPath, this.selectGenderIndex, this.selectStartModeName))
            {
                SingletonMonoBehaviour<ScriptManager>.Instance.WriteSetting();
            }
            this.state = State.PLAY_SCRIPT;
            this.debugTestRootComponent.StartFileScript("C:/FGO/Temporary/ScriptData", "DefaultScript.txt", new System.Action(this.EndPlayScript), new System.Action(this.EndPlayScript), this.GetJumpLine(), true);
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
            this.jumpLineObjectInput.SetInputEnable(true);
            this.genderInput.enabled = true;
            this.startModeInput.enabled = true;
            this.serverDecideButton.enabled = true;
            this.serverCancelButton.enabled = true;
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

