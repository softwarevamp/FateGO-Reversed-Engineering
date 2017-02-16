using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FullDownloadControl : MonoBehaviour
{
    [SerializeField]
    protected UIButton downLoadBtn;
    [SerializeField]
    protected UILabel infoDetailLb;
    [SerializeField]
    protected UILabel infoLb;
    [SerializeField]
    protected GameObject maskObj;

    private void checkDownLoadData()
    {
        SingletonMonoBehaviour<CommonUI>.Instance.maskFadeout(MaskFade.Kind.BLACK, SceneManager.DEFAULT_FADE_TIME, (System.Action) (() => base.StartCoroutine(this.downLoadAll())));
    }

    private void closeDlg(bool isRes)
    {
        if (isRes)
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog(new System.Action(this.checkDownLoadData));
        }
        else
        {
            SingletonMonoBehaviour<CommonUI>.Instance.CloseConfirmDialog();
            this.downLoadBtn.isEnabled = true;
        }
    }

    [DebuggerHidden]
    private IEnumerator downLoadAll() => 
        new <downLoadAll>c__Iterator34 { <>f__this = this };

    public void Init()
    {
        this.infoLb.text = LocalizationManager.Get("FULLDOWNLOAD_INFO_TXT");
        this.infoDetailLb.text = LocalizationManager.Get("FULLDOWNLOAD_INFO_DETAIL_TXT");
        this.downLoadBtn.isEnabled = true;
    }

    public void OnClickFullDl()
    {
        SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
        this.downLoadBtn.isEnabled = false;
        this.checkDownLoadData();
    }

    [CompilerGenerated]
    private sealed class <downLoadAll>c__Iterator34 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal FullDownloadControl <>f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.$PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.$PC;
            this.$PC = -1;
            switch (num)
            {
                case 0:
                    SingletonMonoBehaviour<AssetManager>.Instance.DownloadAssetStorageAll();
                    SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.LOAD_BAR_CANCEL);
                    break;

                case 1:
                    if (SingletonMonoBehaviour<CommonUI>.Instance.IsBusyLoad())
                    {
                        break;
                    }
                    SingletonMonoBehaviour<CommonUI>.Instance.SetLoadMode(ConnectMark.Mode.DEFAULT);
                    this.<>f__this.downLoadBtn.isEnabled = true;
                    SingletonMonoBehaviour<CommonUI>.Instance.maskFadein(SceneManager.DEFAULT_FADE_TIME, null);
                    this.$PC = -1;
                    goto Label_009A;

                default:
                    goto Label_009A;
            }
            this.$current = new WaitForEndOfFrame();
            this.$PC = 1;
            return true;
        Label_009A:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current =>
            this.$current;

        object IEnumerator.Current =>
            this.$current;
    }
}

