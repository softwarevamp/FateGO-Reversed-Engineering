using System;
using System.Runtime.InteropServices;

public class MstExpInfoComponent : BaseDialog
{
    public UILabel expInfoLb;
    public UILabel expInfoTitleLb;
    protected bool isButtonEnable;

    public void Close()
    {
        if (this.isButtonEnable)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
            this.isButtonEnable = false;
            base.Close(new System.Action(this.EndClose));
        }
    }

    protected void EndClose()
    {
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

    public void openExpInfo(int exp, bool bNext = false)
    {
        if (!bNext)
        {
            this.expInfoTitleLb.text = LocalizationManager.Get("MASTER_TOTAL_EXP");
        }
        else
        {
            this.expInfoTitleLb.text = "NEXT";
        }
        this.expInfoLb.text = $"{exp:N0}";
        this.isButtonEnable = false;
        base.Open(new System.Action(this.EndOpen), true);
    }
}

