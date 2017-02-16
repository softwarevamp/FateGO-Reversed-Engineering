using System;
using UnityEngine;

public class TestNameInputRootComponent : SceneRootComponent
{
    public UIButton signupDecideButton;
    public UIPopupList signupGenderInput;
    public UILineInput signupIntegerInput1;
    public UILineInput signupIntegerInput2;
    public UILineInput signupLineInput1;
    public GameObject signupRootObject;

    public override void beginInitialize()
    {
        base.beginInitialize();
        SingletonMonoBehaviour<SceneManager>.Instance.endInitialize(this);
    }

    public override void beginStartUp()
    {
        base.beginStartUp();
    }

    protected bool closeSignupInput()
    {
        this.signupLineInput1.SetInputEnable(false);
        this.signupIntegerInput1.SetInputEnable(false);
        this.signupIntegerInput2.SetInputEnable(false);
        this.signupGenderInput.enabled = false;
        this.signupDecideButton.enabled = false;
        this.signupDecideButton.isEnabled = false;
        this.signupRootObject.SetActive(false);
        return true;
    }

    public void onChangeInput()
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

    public void onClickInput()
    {
        this.closeSignupInput();
        this.signupRootObject.SetActive(true);
        Input.imeCompositionMode = IMECompositionMode.Auto;
        base.myFSM.SendEvent("SIGNUP_INPUT_OK");
    }

    protected bool openSignupInput()
    {
        this.signupRootObject.SetActive(true);
        this.signupLineInput1.SetInputEnable(true);
        this.signupIntegerInput1.SetInputEnable(true);
        this.signupIntegerInput2.SetInputEnable(true);
        this.signupGenderInput.enabled = true;
        this.onChangeInput();
        return true;
    }

    public void requestSignup()
    {
        base.myFSM.SendEvent("REQUEST_OK");
    }

    public bool startSignupInput()
    {
        Input.imeCompositionMode = IMECompositionMode.On;
        this.openSignupInput();
        return true;
    }
}

