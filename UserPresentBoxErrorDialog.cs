using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserPresentBoxErrorDialog : BaseDialog
{
    [SerializeField]
    private UILabel closeLabel;
    [SerializeField]
    private UILabel combineLabel;
    [SerializeField]
    private UILabel messageLabel;
    [SerializeField]
    private UILabel shopLabel;

    public event Action<SceneList.Type> OnErrorDialogClosed;

    private void ChangeScene(SceneList.Type scene)
    {
        <ChangeScene>c__AnonStorey5C storeyc = new <ChangeScene>c__AnonStorey5C {
            scene = scene,
            <>f__this = this
        };
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        SingletonMonoBehaviour<SceneManager>.Instance.changeScene(storeyc.scene, SceneManager.FadeType.BLACK, null);
        base.Close(new System.Action(storeyc.<>m__2D));
    }

    private void Init()
    {
        base.Init();
        this.messageLabel.text = string.Empty;
        this.closeLabel.text = LocalizationManager.Get("COMMON_CONFIRM_CLOSE");
        this.shopLabel.text = LocalizationManager.Get("PRESENT_BOX_ERROR_DIALOG_GO_SHOP");
        this.combineLabel.text = LocalizationManager.Get("PRESENT_BOX_ERROR_DIALOG_GO_COMBINE");
    }

    private void OnClickCloseButton()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.CANCEL);
        base.Close(delegate {
            this.Init();
            if (this.OnErrorDialogClosed != null)
            {
                this.OnErrorDialogClosed(SceneList.Type.None);
            }
        });
    }

    private void OnClickCombineButton()
    {
        this.ChangeScene(SceneList.Type.Combine);
    }

    private void OnClickShopButton()
    {
        this.ChangeScene(SceneList.Type.Shop);
    }

    public void Open(string message)
    {
        this.Init();
        this.messageLabel.text = message;
        base.Open(null, true);
    }

    [CompilerGenerated]
    private sealed class <ChangeScene>c__AnonStorey5C
    {
        internal UserPresentBoxErrorDialog <>f__this;
        internal SceneList.Type scene;

        internal void <>m__2D()
        {
            this.<>f__this.Init();
            if (this.<>f__this.OnErrorDialogClosed != null)
            {
                this.<>f__this.OnErrorDialogClosed(this.scene);
            }
        }
    }
}

