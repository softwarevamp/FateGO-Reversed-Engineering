using System;
using UnityEngine;

public class DetailInfoDialog : BaseDialog
{
    [SerializeField]
    protected UILabel detailMsgLabel;
    [SerializeField]
    protected UILabel infoLabel;
    [SerializeField]
    protected UILabel infoLongLabel;
    protected bool isButtonEnable;
    [SerializeField]
    protected UILabel nameLabel;

    protected void EndClose()
    {
        this.Init();
    }

    protected void EndOpen()
    {
        this.isButtonEnable = true;
    }

    public void Init()
    {
        this.nameLabel.text = string.Empty;
        this.infoLabel.text = string.Empty;
        this.detailMsgLabel.text = string.Empty;
        if (this.nameLabel != null)
        {
            this.nameLabel.text = string.Empty;
        }
        if (this.infoLabel != null)
        {
            this.infoLabel.text = string.Empty;
        }
        if (this.detailMsgLabel != null)
        {
            this.detailMsgLabel.text = string.Empty;
        }
        base.gameObject.SetActive(false);
        base.Init();
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

    public void Open(string name, string info, string detail)
    {
        this.nameLabel.text = (name == null) ? string.Empty : name;
        this.infoLabel.text = (info == null) ? string.Empty : info;
        this.infoLongLabel.text = string.Empty;
        this.detailMsgLabel.text = (detail == null) ? string.Empty : detail;
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }

    public void OpenWithLongInfo(string name, string info, string detail)
    {
        this.nameLabel.text = (name == null) ? string.Empty : name;
        this.infoLabel.text = string.Empty;
        this.infoLongLabel.text = (info == null) ? string.Empty : info;
        this.detailMsgLabel.text = (detail == null) ? string.Empty : detail;
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }
}

