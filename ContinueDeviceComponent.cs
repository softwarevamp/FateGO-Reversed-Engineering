using System;
using UnityEngine;

public class ContinueDeviceComponent : MonoBehaviour
{
    protected int closeCount;
    protected string continueCode;
    [SerializeField]
    protected CommonConfirmDialog continueDeviceConfirmDialog;
    [SerializeField]
    protected ContinueDeviceDispMenu continueDeviceDispMenu;
    [SerializeField]
    protected ContinueDeviceInputMenu continueDeviceInputMenu;
    [SerializeField]
    protected PlayMakerFSM myRoomFsm;
    protected string passwardCode;
    protected State state;

    protected void callbackCodeInput(string result)
    {
        this.state = State.INPUT_PASSWARD;
        this.continueDeviceInputMenu.RepeatInputCode(new ContinueDeviceInputMenu.CallbackFunc(this.onInputCode));
    }

    public bool closeMenu()
    {
        switch (this.state)
        {
            case State.INPUT_PASSWARD:
                this.closeCount = 1;
                this.continueDeviceInputMenu.Close(new System.Action(this.onClose));
                break;

            case State.CONFIRM_PASSEWARD:
                this.closeCount = 2;
                this.continueDeviceInputMenu.Close(new System.Action(this.onClose));
                this.continueDeviceConfirmDialog.Close(new System.Action(this.onClose));
                break;

            case State.OUTPUT_CONTINUE_CODE:
                this.closeCount = 1;
                this.continueDeviceDispMenu.Close(new System.Action(this.onClose));
                break;

            case State.CONFIRM_RETRY:
                this.closeCount = 2;
                this.continueDeviceDispMenu.Close(new System.Action(this.onClose));
                this.continueDeviceConfirmDialog.Close(new System.Action(this.onClose));
                break;

            default:
                this.closeCount = 0;
                this.onClose();
                break;
        }
        return true;
    }

    public void hideMenu()
    {
        this.state = State.INIT;
        this.continueDeviceInputMenu.Init();
        this.continueDeviceDispMenu.Init();
        this.continueDeviceConfirmDialog.Init();
        base.gameObject.SetActive(false);
    }

    protected void onClose()
    {
        this.closeCount--;
        if (this.closeCount == 0)
        {
            this.myRoomFsm.SendEvent("CLOSE_MENU");
        }
    }

    protected void onCloseDialog(bool result)
    {
        this.myRoomFsm.SendEvent("CLOSE_SERIAL_CODE");
    }

    protected void onCloseMenu(PresentBoxNotificationMenu.Result result)
    {
        SingletonMonoBehaviour<CommonUI>.Instance.ClosePresentBoxNotificationMenu();
        this.myRoomFsm.SendEvent("CLOSE_SERIAL_CODE");
    }

    protected void onConfirmCode(bool isDecide)
    {
        if (isDecide)
        {
            this.state = State.REQUEST_PASSWARD;
            NetworkManager.getRequest<ContinuePrepareRequest>(new NetworkManager.ResultCallbackFunc(this.callbackCodeInput)).beginRequest(this.passwardCode);
        }
        else
        {
            this.state = State.INPUT_PASSWARD;
            this.continueDeviceConfirmDialog.Close();
            this.continueDeviceInputMenu.Open(new ContinueDeviceInputMenu.CallbackFunc(this.onInputCode));
        }
    }

    protected void onConfirmRetry(bool isDecide)
    {
        if (isDecide)
        {
            this.state = State.INIT;
            this.closeCount = 2;
            this.continueDeviceDispMenu.Close(new System.Action(this.onNextRetryOpen));
            this.continueDeviceConfirmDialog.Close(new System.Action(this.onNextRetryOpen));
        }
        else
        {
            this.state = State.OUTPUT_CONTINUE_CODE;
            this.continueDeviceConfirmDialog.Close();
            this.continueDeviceDispMenu.Open(this.continueCode, new ContinueDeviceDispMenu.CallbackFunc(this.onDispCode));
        }
    }

    protected void onDispCode(bool isDecide)
    {
        if (isDecide)
        {
            this.state = State.CONFIRM_PASSEWARD;
            this.continueDeviceConfirmDialog.Open(null, LocalizationManager.Get("CONTINUE_DEVICE_CONFIRM_MESSAGE2"), new CommonConfirmDialog.ClickDelegate(this.onConfirmRetry));
        }
        else
        {
            this.myRoomFsm.SendEvent("CLOSE_CONTINUE_DEVICE");
        }
    }

    protected void onInputCode(string code)
    {
        if (code != null)
        {
            this.passwardCode = code;
            this.state = State.REQUEST_PASSWARD;
            NetworkManager.getRequest<ContinuePrepareRequest>(new NetworkManager.ResultCallbackFunc(this.callbackCodeInput)).beginRequest(this.passwardCode);
        }
        else
        {
            this.myRoomFsm.SendEvent("CLOSE_CONTINUE_DEVICE");
        }
    }

    protected void onNextDispOpen()
    {
        this.closeCount--;
        if (this.closeCount == 0)
        {
            this.state = State.OUTPUT_CONTINUE_CODE;
            this.continueDeviceDispMenu.Open(this.continueCode, new ContinueDeviceDispMenu.CallbackFunc(this.onDispCode));
        }
    }

    protected void onNextRetryOpen()
    {
        this.closeCount--;
        if (this.closeCount == 0)
        {
            this.state = State.INPUT_PASSWARD;
            this.passwardCode = string.Empty;
            this.continueCode = string.Empty;
            this.continueDeviceInputMenu.Open(new ContinueDeviceInputMenu.CallbackFunc(this.onInputCode));
        }
    }

    public bool openMenu()
    {
        base.gameObject.SetActive(true);
        UserContinueMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserContinueMaster>(DataNameKind.Kind.USER_CONTINUE);
        long[] args = new long[] { NetworkManager.UserId };
        if (master.isEntityExistsFromId(args))
        {
            UserContinueEntity entity = master.getEntityFromId<UserContinueEntity>(NetworkManager.UserId);
            if (entity.isDel == 0)
            {
                this.state = State.OUTPUT_CONTINUE_CODE;
                this.passwardCode = string.Empty;
                this.continueCode = entity.continueKey;
                this.continueDeviceDispMenu.Open(this.continueCode, new ContinueDeviceDispMenu.CallbackFunc(this.onDispCode));
                return true;
            }
        }
        this.state = State.INPUT_PASSWARD;
        this.passwardCode = string.Empty;
        this.continueCode = string.Empty;
        this.continueDeviceInputMenu.Open(new ContinueDeviceInputMenu.CallbackFunc(this.onInputCode));
        return true;
    }

    public void showMenu()
    {
        base.gameObject.SetActive(true);
        this.state = State.INIT;
        this.continueDeviceInputMenu.Init();
        this.continueDeviceDispMenu.Init();
        this.continueDeviceConfirmDialog.Init();
    }

    protected enum State
    {
        INIT,
        INPUT_PASSWARD,
        CONFIRM_PASSEWARD,
        REQUEST_PASSWARD,
        OUTPUT_CONTINUE_CODE,
        CONFIRM_RETRY,
        CLOSE
    }
}

