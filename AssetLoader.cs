using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class AssetLoader : MonoBehaviour
{
    protected AssetData data;
    protected static readonly string encKeySource = "Px:GuA/{*T5zy]?JAFx./&Dlihyay@h";
    protected bool isDownload;
    protected bool isEndLoad;
    protected bool isErrorDialog;
    protected bool isRequestDownload;
    protected WWW loader;
    protected byte[] readData;
    public static string tryNewLineErrorCode = string.Empty;

    protected event LoadEndHandler endCallback;

    protected event LoadEndDataHandler endCallback2;

    public void AddCallback(LoadEndDataHandler callbackFunc)
    {
        if (callbackFunc != null)
        {
            this.endCallback2 = (LoadEndDataHandler) Delegate.Combine(this.endCallback2, callbackFunc);
        }
    }

    public void AddEntry()
    {
        this.data.AddEntry();
    }

    protected void EndLoad()
    {
        this.isEndLoad = true;
        try
        {
            if (this.endCallback != null)
            {
                this.endCallback(this);
            }
        }
        catch (Exception exception)
        {
            Debug.LogWarning("loader callback error " + exception.Message);
        }
        try
        {
            if (this.endCallback2 != null)
            {
                this.endCallback2(this.data);
            }
        }
        catch (Exception exception2)
        {
            Debug.LogWarning("loader callback2 error " + exception2.Message);
        }
        UnityEngine.Object.Destroy(this);
    }

    protected void EndRebootDialog(bool isDecide)
    {
        if (this.loader != null)
        {
            this.loader.Dispose();
            this.loader = null;
        }
        this.readData = null;
        if (ManagementManager.IsDuringStartup)
        {
            Application.Quit();
        }
        else
        {
            SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
        }
    }

    protected void EndRetryDialog(bool isDecide)
    {
        if (isDecide)
        {
            this.isErrorDialog = false;
        }
        else
        {
            if (this.loader != null)
            {
                this.loader.Dispose();
                this.loader = null;
            }
            this.readData = null;
            if (ManagementManager.IsDuringStartup)
            {
                Application.Quit();
            }
            else
            {
                SingletonMonoBehaviour<ManagementManager>.Instance.reboot(false);
            }
        }
    }

    protected void EndWarningDialog(bool isDecide)
    {
        this.isErrorDialog = false;
    }

    protected byte[] GetEncryptAssetData(string dataPath, byte[] dat)
    {
        byte[] buffer = new byte[dat.Length];
        int length = dat.Length;
        byte[] encryptKey = this.GetEncryptKey(dataPath);
        int num2 = encryptKey.Length;
        for (int i = 0; i < length; i++)
        {
            buffer[i] = (byte) (dat[i] ^ encryptKey[i % num2]);
        }
        return buffer;
    }

    protected byte[] GetEncryptKey(string fkey)
    {
        uint num = Crc32.Compute(Encoding.ASCII.GetBytes(fkey));
        byte[] bytes = Encoding.ASCII.GetBytes(encKeySource);
        byte[] buffer2 = new byte[] { (byte) (num & 0xff), (byte) ((num >> 8) & 0xff), (byte) ((num >> 0x10) & 0xff), (byte) ((num >> 0x18) & 0xff) };
        int length = bytes.Length;
        int num3 = buffer2.Length;
        for (int i = 0; i < length; i++)
        {
            bytes[i] = (byte) (bytes[i] ^ buffer2[i % num3]);
        }
        return bytes;
    }

    public void Init(AssetData data)
    {
        this.data = data;
    }

    public bool IsSame(string name) => 
        ((this.data != null) && this.data.IsSame(name));

    public bool IsSame(AssetData.Type type, string name) => 
        ((this.data != null) && this.data.IsSame(type, name));

    [DebuggerHidden]
    protected IEnumerator LoadDataCR() => 
        new <LoadDataCR>c__Iterator9 { <>f__this = this };

    public void StartLoad(LoadEndHandler callbackFunc)
    {
        this.endCallback = callbackFunc;
        this.isRequestDownload = this.isDownload = this.data.IsNeedUpdateVersion();
        base.StartCoroutine(this.LoadDataCR());
    }

    public bool IsRequestDownload =>
        this.isRequestDownload;

    public int LoadSize
    {
        get
        {
            if (!this.isDownload)
            {
                return this.data.Size;
            }
            if (this.loader != null)
            {
                return (int) (this.loader.progress * this.data.Size);
            }
            return 0;
        }
    }

    public string Name =>
        this.data.Name;

    public int Size =>
        this.data.Size;

    [CompilerGenerated]
    private sealed class <LoadDataCR>c__Iterator9 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal AssetLoader <>f__this;
        internal byte[] <assetData>__31;
        internal AssetBundle <bundle>__33;
        internal string <cfname>__10;
        internal string <configFileUrl>__13;
        internal uint <crc>__18;
        internal uint <crc>__26;
        internal uint <crc>__29;
        internal uint <crc>__8;
        internal string <crcString>__16;
        internal IOException <e>__11;
        internal Exception <e>__12;
        internal string <errorCode>__6;
        internal string <errorLocalizeCode>__7;
        internal long <freeSize>__9;
        internal bool <fullDeck>__0;
        internal int <i>__22;
        internal bool <isBoot>__19;
        internal bool <isCrcError>__2;
        internal bool <isEncrypted>__1;
        internal string[] <lineData>__23;
        internal string[] <listData>__20;
        internal int <listDataSum>__21;
        internal string <loadData>__14;
        internal string <loadDateVersion>__25;
        internal string <loadMasterVersion>__24;
        internal float <loadProgress>__5;
        internal string <name>__27;
        internal string <name>__30;
        internal string <newname>__28;
        internal byte[] <readData>__17;
        internal AssetBundleCreateRequest <req>__32;
        internal float <requestTime>__4;
        internal int <ri>__15;
        internal string <url>__3;

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
                    this.<fullDeck>__0 = false;
                    Debug.Log("LoadData: " + this.<>f__this.data.Name + " start:" + this.<>f__this.data.Name.Replace('/', '@'));
                    this.<isEncrypted>__1 = this.<>f__this.data.IsAssetBundle;
                    break;

                case 1:
                    goto Label_01AF;

                case 2:
                    goto Label_024C;

                case 3:
                    goto Label_0561;

                case 4:
                    goto Label_05C5;

                case 5:
                    goto Label_0698;

                case 6:
                    goto Label_06FA;

                case 7:
                    goto Label_074D;

                case 8:
                    goto Label_08DB;

                case 9:
                    goto Label_0B26;

                case 10:
                    goto Label_0B78;

                case 11:
                    goto Label_0BDB;

                case 12:
                    goto Label_0818;

                case 13:
                    goto Label_0F15;

                case 14:
                    goto Label_0F78;

                case 15:
                    if (string.IsNullOrEmpty(this.<>f__this.loader.error))
                    {
                        this.<>f__this.readData = this.<>f__this.loader.bytes;
                        this.<crc>__29 = Crc32.Compute(this.<>f__this.readData);
                        Debug.Log(string.Concat(new object[] { "crc [", this.<crc>__29, "] [", this.<>f__this.data.Crc, "]" }));
                        if (this.<crc>__29 != this.<>f__this.data.Crc)
                        {
                            Debug.LogError("AssetData crc Error : " + this.<>f__this.data.Name);
                            this.<>f__this.isRequestDownload = this.<>f__this.isDownload = true;
                            this.<>f__this.data.ResetVersion();
                            this.<>f__this.loader.Dispose();
                            this.<>f__this.loader = null;
                            this.<>f__this.readData = null;
                            break;
                        }
                        goto Label_11C0;
                    }
                    Debug.LogError(this.<>f__this.loader.error);
                    Debug.LogError("url : file://" + this.<>f__this.data.Path);
                    this.<>f__this.loader.Dispose();
                    this.<>f__this.loader = null;
                    this.<>f__this.isRequestDownload = this.<>f__this.isDownload = true;
                    this.<>f__this.data.ResetVersion();
                    break;

                case 0x10:
                    this.<assetData>__31 = null;
                    this.<bundle>__33 = this.<req>__32.assetBundle;
                    this.<>f__this.data.SetAssetBundleData(this.<bundle>__33);
                    goto Label_12B9;

                default:
                    goto Label_1308;
            }
        Label_00B7:
            this.<isCrcError>__2 = false;
            this.<>f__this.readData = null;
            if (!this.<>f__this.isDownload)
            {
                goto Label_0FAF;
            }
            this.<url>__3 = this.<>f__this.data.Url;
            Debug.Log("LoadData: WWW " + this.<url>__3);
            this.<>f__this.loader = new WWW(this.<url>__3);
            this.<requestTime>__4 = Time.time + ManagerConfig.TIMEOUT;
            this.<loadProgress>__5 = 0f;
        Label_01AF:
            while (!this.<>f__this.loader.isDone)
            {
                if (this.<>f__this.loader.progress != this.<loadProgress>__5)
                {
                    this.<requestTime>__4 = Time.time + ManagerConfig.TIMEOUT;
                    this.<loadProgress>__5 = this.<>f__this.loader.progress;
                }
                else if (Time.time >= this.<requestTime>__4)
                {
                    break;
                }
                this.$current = new WaitForEndOfFrame();
                this.$PC = 1;
                goto Label_130A;
            }
            this.<errorCode>__6 = null;
            this.<errorLocalizeCode>__7 = null;
            if (!this.<>f__this.loader.isDone)
            {
                this.<errorCode>__6 = "(" + this.<url>__3 + ")\n\nAssetBundle file download time over";
                goto Label_0561;
            }
            if (string.IsNullOrEmpty(this.<>f__this.loader.error))
            {
                this.<>f__this.readData = this.<>f__this.loader.bytes;
                this.<crc>__8 = Crc32.Compute(this.<>f__this.readData);
                Debug.Log(string.Concat(new object[] { "crc [", this.<crc>__8, "] [", this.<>f__this.data.Crc, "]" }));
                if (((this.<crc>__8 != this.<>f__this.data.Crc) && !this.<url>__3.StartsWith("file://")) && !this.<url>__3.StartsWith("jar:file://"))
                {
                    object[] objArray2 = new object[] { "(", this.<url>__3, ")\n\nAssetBundle file check sum error\nlist crc (", this.<>f__this.data.Crc, ")\nfile crc (", this.<crc>__8, ")" };
                    this.<errorCode>__6 = string.Concat(objArray2);
                    this.<isCrcError>__2 = true;
                }
                else
                {
                    Debug.Log("AssetBundle write storage " + this.<>f__this.data.Path);
                    try
                    {
                        this.<freeSize>__9 = CommonServicePluginScript.GetFreeSize(AssetStorageCache.GetPath());
                        if ((this.<freeSize>__9 > 0L) && (this.<freeSize>__9 < ManagerConfig.LIMIT_FREE_SIZE))
                        {
                            throw new IOException("Disk full");
                        }
                        this.<cfname>__10 = this.<>f__this.data.Path;
                        if (File.Exists(this.<cfname>__10))
                        {
                            File.Delete(this.<cfname>__10);
                        }
                        File.WriteAllBytes(this.<cfname>__10, this.<>f__this.readData);
                        this.<>f__this.readData = null;
                        this.<>f__this.data.UpdateVersion();
                        this.<>f__this.isDownload = false;
                        ManagementManager.mEndCdnIndex = ManagementManager.mCdnIndex;
                    }
                    catch (IOException exception)
                    {
                        this.<e>__11 = exception;
                        Debug.LogError("error io " + this.<e>__11.Message);
                        this.<>f__this.isErrorDialog = true;
                        if (this.<e>__11.Message.StartsWith("Disk full"))
                        {
                            this.<errorCode>__6 = "Disk full";
                            this.<errorLocalizeCode>__7 = "NETWORK_ERROR_DISK_FULL";
                            this.<fullDeck>__0 = true;
                        }
                        else
                        {
                            this.<errorCode>__6 = this.<e>__11.Message;
                        }
                    }
                    catch (Exception exception2)
                    {
                        this.<e>__12 = exception2;
                        this.<errorCode>__6 = "error " + this.<e>__12.ToString() + " " + this.<e>__12.Message;
                    }
                    this.$current = new WaitForSeconds(0.1f);
                    this.$PC = 3;
                    goto Label_130A;
                }
                goto Label_0561;
            }
            SingletonMonoBehaviour<ManagementManager>.Instance.TryNewTextCDNUrl("01");
        Label_024C:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 2;
                goto Label_130A;
            }
            this.<>f__this.loader.Dispose();
            this.<>f__this.StartCoroutine(this.<>f__this.LoadDataCR());
            goto Label_1308;
        Label_0561:
            if (this.<errorCode>__6 == null)
            {
                goto Label_075D;
            }
            if (!ManagerConfig.UseDebugCommand)
            {
                goto Label_05D5;
            }
            this.<>f__this.isErrorDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Download error for debug", this.<errorCode>__6, new ErrorDialog.ClickDelegate(this.<>f__this.EndWarningDialog), true);
        Label_05C5:
            while (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 4;
                goto Label_130A;
            }
        Label_05D5:
            if (this.<isCrcError>__2)
            {
                goto Label_075D;
            }
            this.<>f__this.isErrorDialog = true;
            if (this.<errorLocalizeCode>__7 != null)
            {
                if (ManagementManager.IsDuringStartup)
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryBootDialog(string.Empty, LocalizationManager.Get(this.<errorLocalizeCode>__7), new ErrorDialog.ClickDelegate(this.<>f__this.EndRetryDialog), true);
                }
                else
                {
                    SingletonMonoBehaviour<CommonUI>.Instance.OpenRetryDialog(string.Empty, LocalizationManager.Get(this.<errorLocalizeCode>__7), new ErrorDialog.ClickDelegate(this.<>f__this.EndRetryDialog), true);
                }
                goto Label_074D;
            }
            if (!ManagementManager.IsDuringStartup)
            {
                SingletonMonoBehaviour<ManagementManager>.Instance.TryNewTextCDNUrl("02");
                goto Label_06FA;
            }
            SingletonMonoBehaviour<ManagementManager>.Instance.TryNewTextCDNUrl("02");
        Label_0698:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 5;
                goto Label_130A;
            }
            this.<>f__this.isErrorDialog = false;
            this.<>f__this.StartCoroutine(this.<>f__this.LoadDataCR());
            goto Label_1308;
        Label_06FA:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 6;
                goto Label_130A;
            }
            this.<>f__this.isErrorDialog = false;
            this.<>f__this.StartCoroutine(this.<>f__this.LoadDataCR());
            goto Label_1308;
        Label_074D:
            if (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 7;
                goto Label_130A;
            }
        Label_075D:
            if (NetworkManager.IsRebootBlock)
            {
                this.<>f__this.loader.Dispose();
                this.<>f__this.loader = null;
                this.<>f__this.readData = null;
                goto Label_1308;
            }
            if (!this.<>f__this.isDownload)
            {
                goto Label_11C0;
            }
            if (this.<fullDeck>__0)
            {
                goto Label_00B7;
            }
            this.<>f__this.loader.Dispose();
            this.<>f__this.loader = null;
            this.<>f__this.readData = null;
            if (!this.<isCrcError>__2)
            {
                goto Label_0FAF;
            }
            this.<configFileUrl>__13 = AssetManager.AssetStorageFileAddress;
            this.<loadData>__14 = null;
            Debug.Log("AssetManager check config start [" + this.<configFileUrl>__13 + "]");
        Label_0818:
            this.<>f__this.loader = new WWW("http://" + AssetManager.getUrlStringWithUnix(this.<configFileUrl>__13));
            this.<requestTime>__4 = Time.time + ManagerConfig.TIMEOUT;
            this.<loadProgress>__5 = 0f;
        Label_08DB:
            while (!this.<>f__this.loader.isDone)
            {
                if (this.<>f__this.loader.progress != this.<loadProgress>__5)
                {
                    this.<requestTime>__4 = Time.time + ManagerConfig.TIMEOUT;
                    this.<loadProgress>__5 = this.<>f__this.loader.progress;
                }
                else if (Time.time >= this.<requestTime>__4)
                {
                    Debug.LogWarning("TimeOut");
                    break;
                }
                this.$current = new WaitForEndOfFrame();
                this.$PC = 8;
                goto Label_130A;
            }
            this.<errorCode>__6 = null;
            if (!this.<>f__this.loader.isDone)
            {
                this.<errorCode>__6 = "AssetStorageList download time over";
                Debug.LogError(this.<errorCode>__6);
            }
            else if (!string.IsNullOrEmpty(this.<>f__this.loader.error))
            {
                this.<errorCode>__6 = this.<>f__this.loader.error;
                Debug.LogError(this.<errorCode>__6);
            }
            else
            {
                this.<loadData>__14 = CryptData.TextDecrypt(this.<>f__this.loader.text);
                if (string.IsNullOrEmpty(this.<loadData>__14))
                {
                    this.<errorCode>__6 = "AssetStorageList download decrypt error";
                    Debug.LogError(this.<errorCode>__6);
                }
                else
                {
                    char[] trimChars = new char[] { 0xfeff };
                    this.<loadData>__14 = this.<loadData>__14.Trim(trimChars);
                    if (this.<loadData>__14.StartsWith("~"))
                    {
                        char[] anyOf = new char[] { '\r', '\n' };
                        this.<ri>__15 = this.<loadData>__14.IndexOfAny(anyOf);
                        if (this.<ri>__15 > 1)
                        {
                            this.<crcString>__16 = this.<loadData>__14.Substring(1, this.<ri>__15 - 1);
                            this.<loadData>__14 = this.<loadData>__14.Substring(this.<ri>__15 + 1);
                            this.<readData>__17 = Encoding.UTF8.GetBytes(this.<loadData>__14);
                            this.<crc>__18 = Crc32.Compute(this.<readData>__17);
                            if (uint.Parse(this.<crcString>__16) == this.<crc>__18)
                            {
                                this.<isBoot>__19 = true;
                                if (this.<loadData>__14 != null)
                                {
                                    char[] separator = new char[] { '\r', '\n' };
                                    this.<listData>__20 = this.<loadData>__14.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                    this.<listDataSum>__21 = this.<listData>__20.Length;
                                    this.<i>__22 = 0;
                                    while (this.<i>__22 < this.<listDataSum>__21)
                                    {
                                        char[] chArray4 = new char[] { ',' };
                                        this.<lineData>__23 = this.<listData>__20[this.<i>__22].Split(chArray4);
                                        if (this.<lineData>__23.Length >= 1)
                                        {
                                            if (this.<lineData>__23[0].StartsWith("@"))
                                            {
                                                this.<loadMasterVersion>__24 = this.<lineData>__23[0].Substring(1);
                                                this.<loadDateVersion>__25 = (this.<lineData>__23.Length <= 1) ? string.Empty : this.<lineData>__23[1];
                                                if (!AssetManager.CheckVersion(this.<loadMasterVersion>__24, this.<loadDateVersion>__25))
                                                {
                                                    break;
                                                }
                                                goto Label_0E3F;
                                            }
                                            if (this.<lineData>__23[0].StartsWith("~") || (this.<lineData>__23[0].IndexOf('~') == 1))
                                            {
                                                goto Label_0E3F;
                                            }
                                        }
                                        if (this.<lineData>__23.Length != 5)
                                        {
                                            break;
                                        }
                                        this.<crc>__26 = uint.Parse(this.<lineData>__23[3].Trim());
                                        this.<name>__27 = this.<lineData>__23[4];
                                        this.<newname>__28 = string.Empty;
                                        if (this.<lineData>__23[4].Contains("%"))
                                        {
                                            char[] chArray5 = new char[] { '%' };
                                            char[] chArray6 = new char[] { '%' };
                                            this.<newname>__28 = this.<lineData>__23[4].Split(chArray5)[0] + this.<lineData>__23[4].Split(chArray6)[2];
                                        }
                                        else
                                        {
                                            this.<newname>__28 = this.<lineData>__23[4];
                                        }
                                        if (this.<>f__this.IsSame(this.<newname>__28))
                                        {
                                            if (this.<crc>__26 == this.<>f__this.data.Crc)
                                            {
                                                this.<isBoot>__19 = false;
                                            }
                                            break;
                                        }
                                    Label_0E3F:
                                        this.<i>__22++;
                                    }
                                }
                                if (this.<isBoot>__19)
                                {
                                    if (ManagementManager.IsDuringStartup)
                                    {
                                        SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_ASSET_UPDATE_BOOT"), new ErrorDialog.ClickDelegate(this.<>f__this.EndRebootDialog), false);
                                    }
                                    else
                                    {
                                        SingletonMonoBehaviour<CommonUI>.Instance.OpenErrorDialog(string.Empty, LocalizationManager.Get("NETWORK_ERROR_ASSET_UPDATE"), new ErrorDialog.ClickDelegate(this.<>f__this.EndRebootDialog), false);
                                    }
                                    goto Label_1308;
                                }
                                this.<>f__this.isErrorDialog = true;
                                if (!ManagementManager.IsDuringStartup)
                                {
                                    SingletonMonoBehaviour<ManagementManager>.Instance.TryNewTextCDNUrl("03");
                                    goto Label_0F78;
                                }
                                SingletonMonoBehaviour<ManagementManager>.Instance.TryNewTextCDNUrl("03");
                                goto Label_0F15;
                            }
                        }
                    }
                    this.<errorCode>__6 = "AssetStorageList download data error";
                }
            }
            if (this.<>f__this.loader != null)
            {
                this.<>f__this.loader.Dispose();
                this.<>f__this.loader = null;
            }
            this.<loadData>__14 = null;
            if (this.<errorCode>__6 == null)
            {
                this.$current = new WaitForSeconds(1f);
                this.$PC = 12;
                goto Label_130A;
            }
            if (!ManagerConfig.UseDebugCommand)
            {
                goto Label_0B36;
            }
            this.<>f__this.isErrorDialog = true;
            SingletonMonoBehaviour<CommonUI>.Instance.OpenWarningDialog("[FFFF80]Download error for debug", this.<errorCode>__6, new ErrorDialog.ClickDelegate(this.<>f__this.EndWarningDialog), false);
        Label_0B26:
            while (this.<>f__this.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 9;
                goto Label_130A;
            }
        Label_0B36:
            this.<>f__this.isErrorDialog = true;
            if (!ManagementManager.IsDuringStartup)
            {
                SingletonMonoBehaviour<ManagementManager>.Instance.TryNewTextCDNUrl("04");
                goto Label_0BDB;
            }
            SingletonMonoBehaviour<ManagementManager>.Instance.TryNewTextCDNUrl("04");
        Label_0B78:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 10;
                goto Label_130A;
            }
            this.<>f__this.isErrorDialog = false;
            this.<>f__this.StartCoroutine(this.<>f__this.LoadDataCR());
            goto Label_1308;
        Label_0BDB:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 11;
                goto Label_130A;
            }
            this.<>f__this.isErrorDialog = false;
            this.<>f__this.StartCoroutine(this.<>f__this.LoadDataCR());
            goto Label_1308;
        Label_0F15:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 13;
                goto Label_130A;
            }
            this.<>f__this.isErrorDialog = false;
            this.<>f__this.StartCoroutine(this.<>f__this.LoadDataCR());
            goto Label_1308;
        Label_0F78:
            while (SingletonMonoBehaviour<ManagementManager>.Instance.isErrorDialog)
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 14;
                goto Label_130A;
            }
            this.<>f__this.isErrorDialog = false;
            this.<>f__this.StartCoroutine(this.<>f__this.LoadDataCR());
            goto Label_1308;
        Label_0FAF:
            if (this.<>f__this.data.EntryCount > 0)
            {
                this.<>f__this.loader = new WWW("file://" + this.<>f__this.data.Path);
                this.$current = this.<>f__this.loader;
                this.$PC = 15;
                goto Label_130A;
            }
        Label_11C0:
            if (this.<>f__this.data.EntryCount > 0)
            {
                if (this.<isEncrypted>__1)
                {
                    this.<name>__30 = this.<>f__this.data.Name + ".unity3d";
                    this.<assetData>__31 = this.<>f__this.loader.bytes;
                    this.<>f__this.loader.Dispose();
                    this.<>f__this.loader = null;
                    this.<>f__this.readData = null;
                    this.<req>__32 = AssetBundle.LoadFromMemoryAsync(this.<assetData>__31);
                    this.$current = this.<req>__32;
                    this.$PC = 0x10;
                    goto Label_130A;
                }
                this.<>f__this.data.SetData(this.<>f__this.loader);
            }
        Label_12B9:
            if (this.<>f__this.loader != null)
            {
                this.<>f__this.loader.Dispose();
                this.<>f__this.loader = null;
            }
            this.<>f__this.readData = null;
            this.<>f__this.EndLoad();
            GC.Collect();
            this.$PC = -1;
        Label_1308:
            return false;
        Label_130A:
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

    public delegate void LoadEndDataHandler(AssetData data);

    public delegate void LoadEndHandler(AssetLoader loader);
}

