using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ConnectMark : BaseMonoBehaviour
{
    protected System.Action callbackFunc;
    protected int connectCount;
    protected bool isBusy;
    protected bool isLoadCancel;
    protected bool isLoadPause;
    [SerializeField]
    protected UITexture loadBackTexture;
    protected Texture2D loadBackTextureData;
    [SerializeField]
    protected UICommonButton loadCancelButton;
    [SerializeField]
    protected ErrorDialog loadCancelConfirmDialog;
    [SerializeField]
    protected UILabel loadCancelLabel;
    protected string markAnimationName;
    [SerializeField]
    protected GameObject markBase;
    protected IEnumerator markCRW;
    protected static string[] markIconAnimationNameList = new string[] { "connectMarkA", "connectMarkB", "connectMarkC" };
    [SerializeField]
    protected UILabel markLabel;
    [SerializeField]
    protected UISprite markSprite;
    [SerializeField]
    protected GameObject maskBase;
    protected Mode mode;
    [SerializeField]
    protected NotificationDialog notificationDialog;
    [SerializeField]
    protected GameObject progressBarBase;
    protected IEnumerator progressBarCRW;
    [SerializeField]
    protected UILabel progressBarLabel;
    [SerializeField]
    protected UISlider progressBarSlider;
    [SerializeField]
    protected GameObject tipsBase;
    [SerializeField]
    protected UILabel tipsMessageLabel;
    [SerializeField]
    protected UILabel tipsMessageLabel1;

    protected void CallbackConfirmDialog(bool isDecie)
    {
        if (isDecie)
        {
            this.isLoadCancel = true;
        }
        else
        {
            this.isLoadPause = false;
        }
        AssetManager.resumeDownloadAssetStorage();
    }

    protected void EndCloseNotificationDownload()
    {
        this.isBusy = false;
    }

    protected void EndNotificationDownload(bool isDecide)
    {
        this.notificationDialog.Close(new System.Action(this.EndCloseNotificationDownload));
    }

    private string GetProgressStatus()
    {
        if (!AssetManager.IsOnline)
        {
            return "资源正在加载中，请稍等片刻。";
        }
        return "正在下载更新资源，请稍待片刻。";
    }

    public void Init()
    {
        this.ReleaseBackImage();
        this.mode = Mode.DEFAULT;
        this.connectCount = 0;
        this.SetDispMode();
    }

    public bool IsBusy() => 
        this.isBusy;

    protected void LoadBackImage(string fileName)
    {
        if (this.loadBackTextureData == null)
        {
            this.loadBackTextureData = Resources.Load(fileName, typeof(Texture2D)) as Texture2D;
            this.loadBackTexture.mainTexture = this.loadBackTextureData;
        }
    }

    [DebuggerHidden]
    private IEnumerator MarkCR(string message) => 
        new <MarkCR>c__Iterator7 { 
            message = message,
            <$>message = message,
            <>f__this = this
        };

    public void OnClickCancel()
    {
        if (!this.isLoadCancel)
        {
            SoundManager.playSystemSe(SeManager.SystemSeKind.DECIDE);
            AssetManager.pauseDownloadAssetStorage();
            this.isLoadPause = true;
            this.CallbackConfirmDialog(true);
        }
    }

    protected void OnEndAlphaTween()
    {
        this.markBase.SetActive(false);
    }

    [DebuggerHidden]
    private IEnumerator ProgressBarCR() => 
        new <ProgressBarCR>c__Iterator8 { <>f__this = this };

    protected void ReleaseBackImage()
    {
        if (this.loadBackTextureData != null)
        {
            this.loadBackTexture.mainTexture = null;
            Resources.UnloadAsset(this.loadBackTextureData);
            this.loadBackTextureData = null;
        }
    }

    public void SetConnect(bool isConnect)
    {
        if (isConnect)
        {
            this.connectCount++;
            if (this.connectCount > 1)
            {
                return;
            }
        }
        else
        {
            if (this.connectCount <= 0)
            {
                Debug.LogError("ConnectMark: SetConnect false is over request");
                return;
            }
            this.connectCount--;
            if (this.connectCount > 0)
            {
                return;
            }
        }
        if (this.mode == Mode.DEFAULT)
        {
            this.SetDispMode();
        }
    }

    protected void SetDispMode()
    {
        string message = null;
        bool flag = false;
        switch (this.mode)
        {
            case Mode.DEFAULT:
                if (this.connectCount > 0)
                {
                    message = "CONNECTING";
                }
                break;

            case Mode.LOAD:
            case Mode.LOAD_TIP:
                message = "LOADING";
                break;

            case Mode.LOAD_BAR:
            case Mode.LOAD_BAR_CANCEL:
            case Mode.LOAD_BAR_BOOT:
                message = "CONNECTING";
                flag = true;
                break;
        }
        if (this.markCRW != null)
        {
            base.StopCoroutine(this.markCRW);
            this.markCRW = null;
        }
        if (message != null)
        {
            this.isBusy = true;
            this.maskBase.SetActive(true);
            this.markCRW = this.MarkCR(message);
            base.StartCoroutine(this.markCRW);
        }
        else
        {
            this.maskBase.SetActive(false);
            this.tipsBase.SetActive(false);
            this.ReleaseBackImage();
            TweenAlpha component = this.markBase.GetComponent<TweenAlpha>();
            component.from = 1f;
            component.to = 0f;
            component.ResetToBeginning();
            component.Play();
            EventDelegate.Set(component.onFinished, new EventDelegate.Callback(this.OnEndAlphaTween));
        }
        if (this.progressBarCRW != null)
        {
            base.StopCoroutine(this.progressBarCRW);
            this.progressBarCRW = null;
        }
        if (flag)
        {
            this.isBusy = true;
            this.isLoadPause = false;
            this.isLoadCancel = false;
            this.progressBarCRW = this.ProgressBarCR();
            base.StartCoroutine(this.progressBarCRW);
        }
        else
        {
            this.progressBarBase.SetActive(false);
        }
    }

    public void SetMessageShow(bool isShow)
    {
        this.tipsMessageLabel1.gameObject.SetActive(isShow);
    }

    public void SetMode(Mode mode)
    {
        switch (mode)
        {
            case Mode.DEFAULT:
            case Mode.LOAD:
            case Mode.LOAD_TIP:
                if (this.mode != mode)
                {
                    break;
                }
                return;
        }
        this.mode = mode;
        this.SetDispMode();
    }

    [CompilerGenerated]
    private sealed class <MarkCR>c__Iterator7 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>message;
        internal ConnectMark <>f__this;
        internal TweenAlpha <alphaTween>__3;
        internal Animation <an>__4;
        internal AnimationState <anst>__6;
        internal Color <col>__2;
        internal bool <isNew>__0;
        internal int <n>__5;
        internal UISprite <sprite>__1;
        internal int <sum>__8;
        internal string <tenText>__11;
        internal int <tipId>__9;
        internal TipsEntity <tipsEntity>__10;
        internal TipsMaster <tipsMaster>__7;
        internal string message;

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
                    Debug.Log(string.Concat(new object[] { "MarkCR(", this.message, ") ", this.<>f__this.mode }));
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    goto Label_04C4;

                case 1:
                    this.<isNew>__0 = !this.<>f__this.markBase.activeSelf || (this.<>f__this.markAnimationName == null);
                    this.<>f__this.markBase.SetActive(true);
                    this.<sprite>__1 = this.<>f__this.markBase.GetComponent<UISprite>();
                    this.<col>__2 = this.<sprite>__1.color;
                    this.<col>__2.a = 0f;
                    this.<sprite>__1.color = this.<col>__2;
                    this.<alphaTween>__3 = this.<>f__this.markBase.GetComponent<TweenAlpha>();
                    EventDelegate.Remove(this.<alphaTween>__3.onFinished, new EventDelegate.Callback(this.<>f__this.OnEndAlphaTween));
                    this.<alphaTween>__3.from = 0f;
                    this.<alphaTween>__3.to = 1f;
                    this.<alphaTween>__3.ResetToBeginning();
                    this.<alphaTween>__3.Play();
                    this.<>f__this.markLabel.text = this.message;
                    this.<an>__4 = this.<>f__this.markSprite.GetComponent<Animation>();
                    if (this.<an>__4 != null)
                    {
                        if (!this.<isNew>__0)
                        {
                            if (!this.<an>__4.isPlaying)
                            {
                                this.<an>__4.Play(this.<>f__this.markAnimationName);
                            }
                            break;
                        }
                        this.<n>__5 = 0;
                        this.<anst>__6 = this.<an>__4[ConnectMark.markIconAnimationNameList[this.<n>__5]];
                        this.<an>__4.wrapMode = WrapMode.Loop;
                        this.<>f__this.markAnimationName = this.<anst>__6.name;
                        this.<an>__4.Play(this.<>f__this.markAnimationName);
                    }
                    break;

                case 2:
                    this.<tenText>__11 = string.Empty;
                    goto Label_0384;

                case 3:
                    if (!this.<>f__this.isLoadPause)
                    {
                        goto Label_0430;
                    }
                    if (this.<an>__4 != null)
                    {
                        this.<an>__4.Stop();
                    }
                    goto Label_03E8;

                case 4:
                    goto Label_03E8;

                case 5:
                    if (this.<tenText>__11.Length >= 3)
                    {
                        this.<tenText>__11 = string.Empty;
                    }
                    else
                    {
                        this.<tenText>__11 = this.<tenText>__11 + ".";
                    }
                    this.<>f__this.markLabel.text = this.message + this.<tenText>__11;
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 6;
                    goto Label_04C4;

                case 6:
                    goto Label_0384;
                    this.$PC = -1;
                    goto Label_04C2;

                default:
                    goto Label_04C2;
            }
            if (this.<>f__this.mode == ConnectMark.Mode.LOAD_TIP)
            {
                this.<tipsMaster>__7 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TipsMaster>(DataNameKind.Kind.TIPS);
                this.<sum>__8 = this.<tipsMaster>__7.getSum();
                if (this.<sum>__8 > 0)
                {
                    this.<tipId>__9 = UnityEngine.Random.Range(1, this.<sum>__8 + 1);
                    if (this.<tipId>__9 > this.<sum>__8)
                    {
                        this.<tipId>__9 = this.<sum>__8;
                    }
                    this.<tipsEntity>__10 = this.<tipsMaster>__7.getEntityFromId<TipsEntity>(this.<tipId>__9);
                    this.<>f__this.tipsBase.SetActive(true);
                    this.<>f__this.tipsMessageLabel.text = string.Format(LocalizationManager.Get("CONNECT_TIP_MESSAGE"), this.<tipsEntity>__10.comment);
                }
                else
                {
                    this.<>f__this.tipsBase.SetActive(false);
                }
                this.<>f__this.LoadBackImage("System/tips_back01");
            }
            else
            {
                this.<>f__this.tipsBase.SetActive(false);
                this.<>f__this.ReleaseBackImage();
            }
            this.$current = new WaitForEndOfFrame();
            this.$PC = 2;
            goto Label_04C4;
        Label_0384:
            this.$current = new WaitForSeconds(1f);
            this.$PC = 3;
            goto Label_04C4;
        Label_03E8:
            if (this.<>f__this.isLoadPause)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 4;
                goto Label_04C4;
            }
            if ((this.<an>__4 != null) && (this.<>f__this.markAnimationName != null))
            {
                this.<an>__4.Play(this.<>f__this.markAnimationName);
            }
        Label_0430:
            this.$current = new WaitForEndOfFrame();
            this.$PC = 5;
            goto Label_04C4;
        Label_04C2:
            return false;
        Label_04C4:
            return true;
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

    [CompilerGenerated]
    private sealed class <ProgressBarCR>c__Iterator8 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ConnectMark <>f__this;
        internal long <allSize>__3;
        internal bool <isUseCancel>__0;
        internal bool <isUseDialog>__1;
        internal long <size>__4;
        internal float <sliderValue>__2;
        internal float <vs>__5;

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
                    Debug.Log("ProgressBarCR() " + this.<>f__this.mode);
                    this.<isUseCancel>__0 = false;
                    this.<isUseDialog>__1 = false;
                    this.<sliderValue>__2 = 0f;
                    if (this.<>f__this.mode == ConnectMark.Mode.LOAD_BAR_BOOT)
                    {
                        if (!Application.isEditor && !ManagerConfig.UseDebugCommand)
                        {
                            this.<>f__this.LoadBackImage("System/loadImage");
                        }
                        break;
                    }
                    break;

                case 1:
                    this.<>f__this.progressBarBase.SetActive(true);
                    this.<>f__this.progressBarSlider.value = this.<sliderValue>__2;
                    this.<>f__this.progressBarLabel.text = !this.<isUseCancel>__0 ? this.<>f__this.GetProgressStatus() : LocalizationManager.Get("CONNECT_LOAD_MESSAGE1");
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 2;
                    goto Label_0520;

                case 2:
                    this.<allSize>__3 = AssetManager.getDownloadSize();
                    if (this.<allSize>__3 <= 0L)
                    {
                        if (this.<isUseDialog>__1)
                        {
                            this.<>f__this.isLoadPause = true;
                            this.<>f__this.markBase.SetActive(false);
                            this.<>f__this.notificationDialog.Open(string.Empty, LocalizationManager.Get("CONNECT_LATEST_MESSAGE"), new NotificationDialog.ClickDelegate(this.<>f__this.EndNotificationDownload), -1);
                        }
                        else
                        {
                            this.<>f__this.isBusy = false;
                        }
                        goto Label_051E;
                    }
                    if (this.<isUseCancel>__0)
                    {
                        this.<>f__this.loadCancelButton.isEnabled = true;
                        this.<>f__this.loadCancelButton.enabled = true;
                        this.<>f__this.loadCancelButton.SetState(UICommonButtonColor.State.Normal, false);
                    }
                    goto Label_0276;

                case 3:
                    goto Label_02A8;

                case 4:
                    this.<vs>__5 = 1f - (((float) this.<size>__4) / ((float) this.<allSize>__3));
                    if (this.<sliderValue>__2 < this.<vs>__5)
                    {
                        this.<>f__this.progressBarSlider.value = this.<sliderValue>__2 = this.<vs>__5;
                    }
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 5;
                    goto Label_0520;

                case 5:
                    this.$current = new WaitForSeconds(0.1f);
                    this.$PC = 6;
                    goto Label_0520;

                case 6:
                    goto Label_0276;

                case 7:
                    this.<>f__this.progressBarSlider.value = 1f;
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 8;
                    goto Label_0520;

                case 8:
                    this.$current = new WaitForSeconds(0.1f);
                    this.$PC = 9;
                    goto Label_0520;

                case 9:
                    if (!this.<isUseDialog>__1)
                    {
                        this.<>f__this.isBusy = false;
                    }
                    else
                    {
                        this.<>f__this.isLoadPause = true;
                        this.<>f__this.markBase.SetActive(false);
                        this.<>f__this.notificationDialog.Open(string.Empty, LocalizationManager.Get("CONNECT_COMPLET_MESSAGE"), new NotificationDialog.ClickDelegate(this.<>f__this.EndNotificationDownload), -1);
                    }
                    this.$PC = -1;
                    goto Label_051E;

                default:
                    goto Label_051E;
            }
            this.<>f__this.loadCancelLabel.text = LocalizationManager.Get("CONNECT_LOAD_CANCEL");
            ConnectMark.Mode mode = this.<>f__this.mode;
            if (mode == ConnectMark.Mode.LOAD_BAR_CANCEL)
            {
                this.<isUseCancel>__0 = true;
                this.<isUseDialog>__1 = true;
            }
            else if (mode == ConnectMark.Mode.LOAD_BAR_BOOT)
            {
                if (ManagerConfig.ServerDefaultType == "SCRIPT")
                {
                    this.<isUseCancel>__0 = true;
                }
            }
            else
            {
                this.<>f__this.loadCancelButton.gameObject.SetActive(false);
            }
            this.<>f__this.loadCancelButton.gameObject.SetActive(this.<isUseCancel>__0);
            this.<>f__this.loadCancelButton.isEnabled = true;
            this.<>f__this.loadCancelButton.enabled = false;
            this.<>f__this.loadCancelButton.SetState(UICommonButtonColor.State.Normal, false);
            this.$current = new WaitForEndOfFrame();
            this.$PC = 1;
            goto Label_0520;
        Label_0276:
            this.<size>__4 = AssetManager.getDownloadSize();
            if (this.<size>__4 <= 0L)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 7;
                goto Label_0520;
            }
            if (!this.<>f__this.isLoadCancel)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 4;
                goto Label_0520;
            }
            AssetManager.cancelDownloadAssetStorage();
        Label_02A8:
            this.<size>__4 = AssetManager.getDownloadSize();
            if (this.<size>__4 > 0L)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 3;
                goto Label_0520;
            }
            if (this.<isUseDialog>__1)
            {
                this.<>f__this.isLoadPause = true;
                this.<>f__this.notificationDialog.Open(string.Empty, LocalizationManager.Get("CONNECT_CANCEL_MESSAGE"), new NotificationDialog.ClickDelegate(this.<>f__this.EndNotificationDownload), -1);
            }
            else
            {
                this.<>f__this.isBusy = false;
            }
        Label_051E:
            return false;
        Label_0520:
            return true;
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

    public enum Mode
    {
        DEFAULT,
        LOAD,
        LOAD_TIP,
        LOAD_BAR,
        LOAD_BAR_CANCEL,
        LOAD_BAR_BOOT
    }
}

