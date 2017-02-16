using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

public class TutorialNotificationMessage : BaseDialog
{
    protected System.Action closeCallbackFunc;
    protected TutorialFlag.Id flagId;
    protected bool isButtonEnable;
    private int messageDefaultFontSize = -1;
    private Vector2 messageDefaultPosition;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected GameObject touchBase;
    protected GameObject touchObject;
    [SerializeField]
    protected GameObject touchPrefab;

    public void Close()
    {
        this.Close(null);
    }

    public void Close(System.Action callback)
    {
        this.closeCallbackFunc = callback;
        this.isButtonEnable = false;
        base.Close(new System.Action(this.EndClose));
    }

    protected void EndClose()
    {
        this.Init();
        base.gameObject.SetActive(false);
        this.setTutorialMaskActive(true);
        if (this.closeCallbackFunc != null)
        {
            System.Action closeCallbackFunc = this.closeCallbackFunc;
            this.closeCallbackFunc = null;
            closeCallbackFunc();
        }
    }

    protected void EndOpen()
    {
        if (this.touchObject == null)
        {
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.touchPrefab);
            Transform transform = obj2.transform;
            Vector3 localPosition = obj2.transform.localPosition;
            Vector3 localScale = obj2.transform.localScale;
            transform.parent = this.touchBase.transform;
            transform.localPosition = localPosition;
            transform.localRotation = Quaternion.identity;
            transform.localScale = localScale;
            this.touchObject = obj2;
        }
        this.isButtonEnable = true;
    }

    protected void EndTurorialRequest(string result)
    {
        this.Close(this.closeCallbackFunc);
    }

    public void Init()
    {
        if (this.messageLabel != null)
        {
            this.messageLabel.text = string.Empty;
        }
        if (this.touchObject != null)
        {
            UnityEngine.Object.Destroy(this.touchObject);
            this.touchObject = null;
        }
        this.isButtonEnable = false;
        base.gameObject.SetActive(false);
        base.Init();
    }

    public void OnClickClose()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.isButtonEnable = false;
            if (this.touchObject != null)
            {
                UnityEngine.Object.Destroy(this.touchObject);
                this.touchObject = null;
            }
            if (this.flagId == TutorialFlag.Id.NULL)
            {
                this.Close(this.closeCallbackFunc);
            }
            else
            {
                NetworkManager.getRequest<TutorialSetRequest>(new NetworkManager.ResultCallbackFunc(this.EndTurorialRequest)).beginRequest(this.flagId);
            }
        }
    }

    public void Open(string message, TutorialFlag.Id flagId = -1, System.Action func = null)
    {
        this.closeCallbackFunc = func;
        this.flagId = flagId;
        if (this.messageLabel != null)
        {
            this.messageLabel.text = (message == null) ? string.Empty : message;
            if (this.messageDefaultFontSize > 0)
            {
                this.messageLabel.transform.localPosition = (Vector3) this.messageDefaultPosition;
                this.messageLabel.fontSize = this.messageDefaultFontSize;
            }
        }
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void OpenWithArrow(string message, Vector2 messagePos, int fontSize)
    {
        this.closeCallbackFunc = null;
        this.flagId = this.flagId;
        if (this.messageLabel != null)
        {
            if (this.messageDefaultFontSize < 0)
            {
                this.messageDefaultPosition = this.messageLabel.transform.localPosition;
                this.messageDefaultFontSize = this.messageLabel.fontSize;
            }
            this.messageLabel.text = (message == null) ? string.Empty : message;
            this.messageLabel.transform.localPosition = (Vector3) (this.messageDefaultPosition + messagePos);
            if (fontSize < 0)
            {
                this.messageLabel.fontSize = this.messageDefaultFontSize;
            }
            else
            {
                this.messageLabel.fontSize = fontSize;
            }
        }
        this.setTutorialMaskActive(false);
        this.isButtonEnable = false;
        base.Open(null, true);
    }

    public void setTutorialMaskActive(bool active)
    {
        IEnumerator enumerator = base.gameObject.transform.GetEnumerator();
        try
        {
            while (enumerator.MoveNext())
            {
                Transform current = (Transform) enumerator.Current;
                current.gameObject.SetActive(active);
            }
        }
        finally
        {
            IDisposable disposable = enumerator as IDisposable;
            if (disposable == null)
            {
            }
            disposable.Dispose();
        }
        base.baseWindow.SetActive(true);
    }
}

