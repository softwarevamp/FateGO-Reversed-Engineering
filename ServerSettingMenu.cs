using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/ServerSettingMenu")]
public class ServerSettingMenu : MonoBehaviour
{
    public UILineInput dataServerAddressInput;
    public UILabel dataServerAddressLabel;
    public UILineInput gameServerAddressInput;
    public UILabel gameServerAddressLabel;
    public UIButton serverCancelButton;
    public UIButton serverCopyButton;
    public UIButton serverDecideButton;
    public UIButton serverSecurityButton;
    public UISprite serverSecurityDispSprite;
    public UISprite serverSecuritySprite;
    public UIPopupList serverSelectInput;
    public GameObject serverSettingRootObject;
    protected State state;
    public UILineInput webServerAddressInput;
    public UILabel webServerAddressLabel;

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
            this.gameServerAddressLabel.text = string.Empty;
            this.dataServerAddressLabel.text = string.Empty;
            this.webServerAddressLabel.text = string.Empty;
            UIInput component = this.gameServerAddressInput.GetComponent<UIInput>();
            UIInput input2 = this.dataServerAddressInput.GetComponent<UIInput>();
            UIInput input3 = this.webServerAddressInput.GetComponent<UIInput>();
            component.value = string.Empty;
            input2.value = string.Empty;
            input3.value = string.Empty;
            this.serverSettingRootObject.SetActive(false);
        }
    }

    public void OnChangeServerInput()
    {
    }

    public void OnChangeServerInputType()
    {
        string type = this.serverSelectInput.value;
        this.serverSecurityDispSprite.enabled = NetworkManager.GetSecurityServerSetting(type);
        this.gameServerAddressLabel.text = NetworkManager.GetGameServerSetting(type);
        this.dataServerAddressLabel.text = NetworkManager.GetDataServerSetting(type);
        this.webServerAddressLabel.text = NetworkManager.GetWebServerSetting(type);
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
            string type = this.serverSelectInput.value;
            bool enabled = this.serverSecuritySprite.enabled;
            string text = this.gameServerAddressInput.GetText();
            string dataAddress = this.dataServerAddressInput.GetText();
            string webAddress = this.webServerAddressInput.GetText();
            SingletonMonoBehaviour<NetworkManager>.Instance.SetServerSetting(type, enabled, text, dataAddress, webAddress);
            SingletonMonoBehaviour<NetworkManager>.Instance.WriteServerSetting();
            this.serverSelectInput.enabled = false;
            this.serverSecurityButton.isEnabled = false;
            this.gameServerAddressInput.SetInputEnable(false);
            this.dataServerAddressInput.SetInputEnable(false);
            this.webServerAddressInput.SetInputEnable(false);
            this.serverDecideButton.enabled = false;
            this.serverCancelButton.enabled = false;
            this.serverCopyButton.enabled = false;
            Input.imeCompositionMode = IMECompositionMode.Auto;
            this.EndInput();
            this.state = State.SELECTED;
            this.Callback(true);
        }
    }

    public void OnClickServerCopyString()
    {
        if (this.state == State.INPUT)
        {
            string type = this.serverSelectInput.value;
            UIInput component = this.gameServerAddressInput.GetComponent<UIInput>();
            UIInput input2 = this.dataServerAddressInput.GetComponent<UIInput>();
            UIInput input3 = this.webServerAddressInput.GetComponent<UIInput>();
            this.serverSecuritySprite.enabled = NetworkManager.GetSecurityServerSetting(type);
            component.value = NetworkManager.GetGameServerSetting(type);
            input2.value = NetworkManager.GetDataServerSetting(type);
            input3.value = NetworkManager.GetWebServerSetting(type);
        }
    }

    public void OnClickServerInputSecurity()
    {
        bool enabled = this.serverSecuritySprite.enabled;
        Debug.Log("OnChangeServerInputSecurity : " + enabled);
        this.serverSecuritySprite.enabled = !enabled;
    }

    public void Open(CallbackFunc callback)
    {
        if (this.state == State.INIT)
        {
            this.callbackFunc = callback;
            this.serverSettingRootObject.SetActive(true);
            this.serverSelectInput.enabled = true;
            this.serverSecurityButton.isEnabled = true;
            this.gameServerAddressInput.SetInputEnable(true);
            this.dataServerAddressInput.SetInputEnable(true);
            this.webServerAddressInput.SetInputEnable(true);
            this.serverDecideButton.enabled = true;
            this.serverCancelButton.enabled = true;
            this.serverCopyButton.enabled = true;
            this.serverSelectInput.value = NetworkManager.GetServerSettingType();
            this.serverSecuritySprite.enabled = NetworkManager.GetSecurityServerSetting();
            UIInput component = this.gameServerAddressInput.GetComponent<UIInput>();
            UIInput input2 = this.dataServerAddressInput.GetComponent<UIInput>();
            UIInput input3 = this.webServerAddressInput.GetComponent<UIInput>();
            component.value = NetworkManager.GetGameServerSetting();
            input2.value = NetworkManager.GetDataServerSetting();
            input3.value = NetworkManager.GetWebServerSetting();
            this.state = State.INPUT;
        }
    }

    public delegate void CallbackFunc(bool result);

    protected enum State
    {
        INIT,
        INPUT,
        SELECTED,
        CLOSE
    }
}

