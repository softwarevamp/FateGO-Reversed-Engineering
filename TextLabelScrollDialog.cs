using System;
using UnityEngine;

public class TextLabelScrollDialog : BaseDialog
{
    protected System.Action closeAction;
    [SerializeField]
    protected UILabel closeLabel;
    [SerializeField]
    protected UILabel messageLabel;
    [SerializeField]
    protected UIScrollView scrollView;
    [SerializeField]
    protected UILabel titleLabel;

    protected virtual void Init()
    {
        base.Init();
        this.titleLabel.text = string.Empty;
        this.messageLabel.text = string.Empty;
        this.closeLabel.text = LocalizationManager.Get("COMMON_CONFIRM_CLOSE");
        this.closeAction = null;
    }

    public void OnClickCloseButton()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        base.Close(delegate {
            this.closeAction.Call();
            this.Init();
        });
    }

    public void Open(string title, string message, System.Action closeAction)
    {
        this.Init();
        base.Open(null, true);
        this.titleLabel.text = title;
        this.messageLabel.text = message;
        this.closeAction = closeAction;
        this.ResetScrollPosition();
    }

    private void ResetScrollPosition()
    {
        NGUITools.UpdateWidgetCollider(this.messageLabel.gameObject, true);
        this.scrollView.contentPivot = (this.scrollView.panel.height <= this.messageLabel.height) ? UIWidget.Pivot.Top : UIWidget.Pivot.Center;
        this.scrollView.ResetPosition();
    }
}

