using System;
using UnityEngine;

public class PopupMessageDialog : BaseDialog
{
    [SerializeField]
    protected Camera dialogCamera;
    protected bool isButtonEnable;
    protected bool isInit;
    [SerializeField]
    protected UILabel messageLabel;
    protected Vector2 windowOffsetSize;

    protected void EndClose()
    {
        this.messageLabel.text = string.Empty;
    }

    protected void EndOpen()
    {
        this.isButtonEnable = true;
    }

    public void OnClickClose()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.isButtonEnable = false;
            base.Close(new System.Action(this.EndClose));
        }
    }

    public void Open(string message)
    {
        UISprite component = base.baseWindow.GetComponent<UISprite>();
        if (!this.isInit)
        {
            this.isInit = true;
            this.windowOffsetSize = new Vector2((float) (component.width - this.messageLabel.width), (float) (component.height - this.messageLabel.height));
        }
        this.messageLabel.width = 0x3d8;
        this.messageLabel.height = 0x400;
        this.messageLabel.text = (message == null) ? string.Empty : message;
        Vector2 printedSize = this.messageLabel.printedSize;
        component.width = (int) (this.windowOffsetSize.x + printedSize.x);
        component.height = (int) (this.windowOffsetSize.y + printedSize.y);
        Vector2 lastTouchPosition = UICamera.lastTouchPosition;
        Vector3 position = this.dialogCamera.ScreenToWorldPoint((Vector3) lastTouchPosition);
        Vector3 vector4 = base.baseWindow.transform.parent.InverseTransformPoint(position);
        Vector2 vector5 = new Vector2(((this.windowOffsetSize.x + printedSize.x) / 2f) + 20f, ((this.windowOffsetSize.y + printedSize.y) / 2f) + 20f);
        if (vector4.x < (vector5.x - (ManagerConfig.WIDTH / 2)))
        {
            vector4.x = vector5.x - (ManagerConfig.WIDTH / 2);
        }
        else if (vector4.x > ((ManagerConfig.WIDTH / 2) - vector5.x))
        {
            vector4.x = (ManagerConfig.WIDTH / 2) - vector5.x;
        }
        if (vector4.y < (vector5.y - (ManagerConfig.HEIGHT / 2)))
        {
            vector4.y = vector5.y - (ManagerConfig.HEIGHT / 2);
        }
        else if (vector4.y > ((ManagerConfig.HEIGHT / 2) - vector5.y))
        {
            vector4.y = (ManagerConfig.HEIGHT / 2) - vector5.y;
        }
        base.baseWindow.transform.localPosition = vector4;
        Vector3 localPosition = this.messageLabel.transform.localPosition;
        this.messageLabel.width = (int) printedSize.x;
        this.messageLabel.height = (int) printedSize.y;
        localPosition.x = -printedSize.x / 2f;
        localPosition.y = printedSize.y / 2f;
        this.messageLabel.transform.localPosition = localPosition;
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }
}

