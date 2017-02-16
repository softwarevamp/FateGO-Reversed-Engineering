using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TopGameDataRequest : RequestBase
{
    private bool isErrorDialog;
    private bool isReDownload;
    private int mDownloadUrlIndex;
    private int mEndDownloadUrlIndex = -1;
    protected static long resetTime;
    protected string tryDataNewLineErrorCode = string.Empty;

    public static bool checkResetTime() => 
        ((resetTime > 0L) && (NetworkManager.getTime() > resetTime));

    [DebuggerHidden]
    private IEnumerator DownloadDataTxT(ResponseData[] responseList) => 
        new <DownloadDataTxT>c__Iterator13 { 
            responseList = responseList,
            <$>responseList = responseList,
            <>f__this = this
        };

    protected void EndRetryDialog(bool isDecide)
    {
        if (isDecide)
        {
            this.mDownloadUrlIndex = 0;
            this.mEndDownloadUrlIndex = -1;
            this.isErrorDialog = false;
            this.isReDownload = true;
        }
        else
        {
            Application.Quit();
        }
    }

    public override string getMockData() => 
        NetworkManager.getMockFile("MockTopGameDataRequest");

    public override string getMockURL() => 
        (NetworkManager.getBaseMockUrl() + "MockTopGameDataRequest.txt");

    public override string getURL() => 
        (NetworkManager.getBaseUrl(false) + "rgfate/60_member/member.php");

    public override void requestCompleted(ResponseData[] responseList)
    {
        SingletonMonoBehaviour<ManagementManager>.Instance.StartCoroutine(this.DownloadDataTxT(responseList));
    }

    [DebuggerHidden]
    private IEnumerator TryNewDownloadDataTxT(string[] urls, string dataVer, string dateVer, string errorCode, int index) => 
        new <TryNewDownloadDataTxT>c__Iterator14 { 
            urls = urls,
            errorCode = errorCode,
            index = index,
            dataVer = dataVer,
            dateVer = dateVer,
            <$>urls = urls,
            <$>errorCode = errorCode,
            <$>index = index,
            <$>dataVer = dataVer,
            <$>dateVer = dateVer,
            <>f__this = this
        };

    [CompilerGenerated]
    private sealed class <DownloadDataTxT>c__Iterator13 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal ResponseData[] <$>responseList;
        internal TopGameDataRequest <>f__this;
        internal string <assetbundleFolder>__7;
        internal string <dataVer>__2;
        internal string <dateVer>__3;
        internal string <masterUrl>__5;
        internal string <oldMasterVersion>__4;
        internal ResponseData <responseData>__0;
        internal Dictionary<string, object> <successList>__1;
        internal string[] <urls>__6;
        internal ResponseData[] responseList;

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
                    TopHomeRequest.clearExpirationDate();
                    this.<responseData>__0 = ResponseCommandKind.SearchData(ResponseCommandKind.Kind.GAME_DATA, this.responseList);
                    if ((this.<responseData>__0 == null) || !this.<responseData>__0.checkError())
                    {
                        goto Label_02CF;
                    }
                    this.<successList>__1 = this.<responseData>__0.success;
                    if (this.<successList>__1 == null)
                    {
                        goto Label_02CF;
                    }
                    if (!this.<successList>__1.ContainsKey("version"))
                    {
                        break;
                    }
                    this.<dataVer>__2 = this.<successList>__1["version"].ToString();
                    this.<dateVer>__3 = "0";
                    if (this.<successList>__1.ContainsKey("version"))
                    {
                        this.<dateVer>__3 = this.<successList>__1["version"].ToString();
                    }
                    Debug.Log("gamedata ver " + this.<dataVer>__2 + "[" + this.<dateVer>__3 + "]");
                    TopGameDataRequest.resetTime = NetworkManager.getNextDayTime(BalanceConfig.GameDataResetTime);
                    this.<oldMasterVersion>__4 = SingletonMonoBehaviour<DataManager>.Instance.getMasterDataVersion().ToString();
                    Debug.Log("master ver ：：：" + this.<oldMasterVersion>__4 + "->" + this.<dateVer>__3);
                    if ((this.<dateVer>__3 == this.<oldMasterVersion>__4) || !this.<successList>__1.ContainsKey("master"))
                    {
                        break;
                    }
                    Debug.Log("gamedata master");
                    this.<masterUrl>__5 = this.<successList>__1["master"].ToString();
                    this.<urls>__6 = SingletonMonoBehaviour<ManagementManager>.Instance.GetmCdn().list[0].cdn;
                    SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(true);
                    this.$current = this.<>f__this.TryNewDownloadDataTxT(this.<urls>__6, this.<dataVer>__2, this.<dateVer>__3, "00", 0);
                    this.$PC = 1;
                    goto Label_02E8;

                case 1:
                case 2:
                    if (this.<>f__this.isErrorDialog)
                    {
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 2;
                        goto Label_02E8;
                    }
                    SingletonMonoBehaviour<CommonUI>.Instance.SetConnect(false);
                    if (this.<>f__this.isReDownload)
                    {
                        this.<>f__this.isReDownload = false;
                        SingletonMonoBehaviour<ManagementManager>.Instance.StartCoroutine(this.<>f__this.DownloadDataTxT(this.responseList));
                        goto Label_02E6;
                    }
                    break;

                default:
                    goto Label_02E6;
            }
            if (this.<successList>__1.ContainsKey("assetbundleFolder"))
            {
                this.<assetbundleFolder>__7 = this.<successList>__1["assetbundleFolder"].ToString();
                NetworkManager.SetDataServerFolderName(this.<assetbundleFolder>__7);
            }
            this.<>f__this.completed("ok");
            goto Label_02E6;
        Label_02CF:
            this.<>f__this.completed("ng");
            this.$PC = -1;
        Label_02E6:
            return false;
        Label_02E8:
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
    private sealed class <TryNewDownloadDataTxT>c__Iterator14 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal string <$>dataVer;
        internal string <$>dateVer;
        internal string <$>errorCode;
        internal int <$>index;
        internal string[] <$>urls;
        internal TopGameDataRequest <>f__this;
        internal string <downloadDataTxT>__2;
        internal string <error>__0;
        internal WWW <www>__1;
        internal string dataVer;
        internal string dateVer;
        internal string errorCode;
        internal int index;
        internal string[] urls;

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
                    if (this.<>f__this.mDownloadUrlIndex == this.urls.Length)
                    {
                        this.<>f__this.mDownloadUrlIndex = 0;
                    }
                    if (this.errorCode != "00")
                    {
                        this.<error>__0 = this.errorCode;
                        this.<>f__this.tryDataNewLineErrorCode = SingletonMonoBehaviour<ManagementManager>.Instance.CheckErrorCode(this.index, this.<error>__0, this.<>f__this.tryDataNewLineErrorCode);
                    }
                    if (this.<>f__this.mEndDownloadUrlIndex == this.<>f__this.mDownloadUrlIndex)
                    {
                        this.<>f__this.isErrorDialog = true;
                        SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, "下载资源文件失败(" + this.<>f__this.tryDataNewLineErrorCode + ")", "重试", "退出", new ErrorDialog.ClickDelegate(this.<>f__this.EndRetryDialog), false);
                        goto Label_0251;
                    }
                    if (this.<>f__this.mEndDownloadUrlIndex == -1)
                    {
                        this.<>f__this.mEndDownloadUrlIndex = 0;
                    }
                    this.<www>__1 = new WWW(this.urls[this.<>f__this.mDownloadUrlIndex] + "/MasterDataCachesOutput/" + this.dataVer + "/data.txt");
                    this.$current = this.<www>__1;
                    this.$PC = 1;
                    goto Label_0253;

                case 1:
                    if (!string.IsNullOrEmpty(this.<www>__1.error) || string.IsNullOrEmpty(this.<www>__1.text))
                    {
                        this.<>f__this.mDownloadUrlIndex++;
                        this.$current = this.<>f__this.TryNewDownloadDataTxT(this.urls, this.dataVer, this.dateVer, "01", this.<>f__this.mDownloadUrlIndex - 1);
                        this.$PC = 2;
                        goto Label_0253;
                    }
                    this.<>f__this.mEndDownloadUrlIndex = this.<>f__this.mDownloadUrlIndex;
                    this.<downloadDataTxT>__2 = this.<www>__1.text;
                    SingletonMonoBehaviour<DataManager>.Instance.setMasterData(int.Parse(this.dataVer), long.Parse(this.dateVer), this.<downloadDataTxT>__2);
                    this.<>f__this.mEndDownloadUrlIndex = -1;
                    break;

                case 2:
                    break;

                default:
                    goto Label_0251;
            }
            this.$PC = -1;
        Label_0251:
            return false;
        Label_0253:
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
}

