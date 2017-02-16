using System;
using UnityEngine;

[AddComponentMenu("Sample/DebugTest/DebugSignupMenu")]
public class DebugSignupMenu : MonoBehaviour
{
    protected PlayMakerFSM myFSM;
    protected string selectConnectPath;
    protected int selectGenderIndex;
    protected string selectObjectPath;
    protected string settingConnectPath;
    protected int settingGenderIndex;
    protected string settingObjectPath;
    public UIButton signupDecideButton;
    public UIPopupList signupGenderInput;
    public UILineInput signupIntegerInput1;
    public UILineInput signupIntegerInput2;
    public UILineInput signupLineInput1;
    public UIButton signupModeButton;
    public GameObject signupRootObject;
    protected State state;
    public UIButton takeoverDecideButton;
    public UILineInput takeoverLineInput1;
    public UILineInput takeoverLineInput2;
    public UILineInput takeoverLineInput3;
    public UIButton takeoverModeButton;
    public GameObject takeoverRootObject;

    private void callbackTopSignup(string result)
    {
        this.myFSM.SendEvent("REQUEST_OK");
    }

    protected bool CloseSignupInput()
    {
        this.signupLineInput1.SetInputEnable(false);
        this.signupIntegerInput1.SetInputEnable(false);
        this.signupIntegerInput2.SetInputEnable(false);
        this.signupGenderInput.enabled = false;
        this.signupDecideButton.enabled = false;
        this.signupDecideButton.isEnabled = false;
        this.signupModeButton.enabled = false;
        this.signupModeButton.isEnabled = false;
        this.signupRootObject.SetActive(false);
        return true;
    }

    protected bool CloseTakeoverInput()
    {
        this.takeoverLineInput1.SetInputEnable(false);
        this.takeoverLineInput2.SetInputEnable(false);
        this.takeoverLineInput3.SetInputEnable(false);
        this.takeoverDecideButton.enabled = false;
        this.takeoverDecideButton.isEnabled = false;
        this.takeoverModeButton.enabled = false;
        this.takeoverModeButton.isEnabled = false;
        this.takeoverRootObject.SetActive(false);
        return true;
    }

    public void OnChangeInputSignup()
    {
        try
        {
            string text = this.signupLineInput1.GetText();
            int month = int.Parse(this.signupIntegerInput1.GetText());
            int day = int.Parse(this.signupIntegerInput2.GetText());
            Debug.Log(string.Empty + new DateTime(0x7d0, month, day).ToString());
            bool flag = text != string.Empty;
            this.signupDecideButton.enabled = flag;
            this.signupDecideButton.isEnabled = flag;
        }
        catch
        {
            this.signupDecideButton.enabled = false;
            this.signupDecideButton.isEnabled = false;
        }
    }

    public void OnChangeInputTakeover()
    {
        try
        {
            string text = this.takeoverLineInput1.GetText();
            string str2 = this.takeoverLineInput2.GetText();
            string str3 = this.takeoverLineInput3.GetText();
            bool flag = ((text != string.Empty) && (str2 != string.Empty)) && (str3 != base.name);
            this.takeoverDecideButton.enabled = flag;
            this.takeoverDecideButton.isEnabled = flag;
        }
        catch
        {
            this.takeoverDecideButton.enabled = false;
            this.takeoverDecideButton.isEnabled = false;
        }
    }

    public void OnClickInputSignup()
    {
        Input.imeCompositionMode = IMECompositionMode.Auto;
        this.myFSM.SendEvent("SIGNUP_INPUT_OK");
    }

    public void OnClickInputTakeover()
    {
        Input.imeCompositionMode = IMECompositionMode.Auto;
        this.myFSM.SendEvent("TAKEOVER_INPUT_OK");
    }

    public void OnClickModeSignup()
    {
        this.CloseSignupInput();
        this.OpenTakeoverInput();
    }

    public void OnClickModeTakeover()
    {
        this.CloseTakeoverInput();
        this.OpenSignupInput();
    }

    public bool Open(PlayMakerFSM fsm)
    {
        this.myFSM = fsm;
        Input.imeCompositionMode = IMECompositionMode.On;
        this.signupModeButton.gameObject.SetActive(false);
        this.takeoverModeButton.gameObject.SetActive(false);
        this.OpenTakeoverInput();
        return true;
    }

    protected bool OpenSignupInput()
    {
        this.signupRootObject.SetActive(true);
        this.signupLineInput1.SetInputEnable(true);
        this.signupIntegerInput1.SetInputEnable(true);
        this.signupIntegerInput2.SetInputEnable(true);
        this.signupGenderInput.enabled = true;
        this.OnChangeInputSignup();
        bool flag = !ManagerConfig.UseMock;
        this.signupModeButton.enabled = flag;
        this.signupModeButton.isEnabled = flag;
        return true;
    }

    protected bool OpenTakeoverInput()
    {
        this.takeoverRootObject.SetActive(true);
        this.takeoverLineInput1.SetInputEnable(true);
        this.takeoverLineInput2.SetInputEnable(true);
        this.takeoverLineInput3.SetInputEnable(true);
        this.OnChangeInputTakeover();
        bool flag = !ManagerConfig.UseMock;
        this.takeoverModeButton.enabled = flag;
        this.takeoverModeButton.isEnabled = flag;
        return true;
    }

    public void RequestSignup(PlayMakerFSM fsm)
    {
        this.myFSM = fsm;
        TopSignupRequest request = NetworkManager.getRequest<TopSignupRequest>(new NetworkManager.ResultCallbackFunc(this.callbackTopSignup));
        if (ManagerConfig.UseMock)
        {
            request.beginRequest();
        }
        else
        {
            request.beginRequest();
        }
    }

    public void SetupTakeover(PlayMakerFSM fsm)
    {
        this.myFSM = fsm;
        string text = this.takeoverLineInput1.GetText();
        string authKey = this.takeoverLineInput2.GetText();
        string secretKey = this.takeoverLineInput3.GetText();
        int dataVer = SingletonMonoBehaviour<DataManager>.Instance.getMasterDataVersion();
        long dateVer = SingletonMonoBehaviour<DataManager>.Instance.getMasterDateVersion();
        UserSaveData.DeleteSaveData();
        SingletonMonoBehaviour<DataManager>.Instance.setMasterDataVersion(dataVer, dateVer);
        SingletonMonoBehaviour<NetworkManager>.Instance.SetAuth(text, authKey, secretKey);
        SingletonMonoBehaviour<NetworkManager>.Instance.WriteAuth();
        UserServantNewManager.CreateContinueDeviceSaveData();
        UserServantCollectionManager.CreateContinueDeviceSaveData();
        ServantCommentManager.CreateContinueDeviceSaveData();
        UserEquipNewManager.CreateContinueDeviceSaveData();
        OtherUserNewManager.CreateContinueDeviceSaveData();
        this.myFSM.SendEvent("SETUP_OK");
    }

    protected enum State
    {
        INIT,
        INPUT,
        INPUT_OBJECT_MENU,
        SELECTED,
        CLOSE
    }
}

