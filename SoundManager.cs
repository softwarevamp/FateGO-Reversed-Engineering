using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    protected Dictionary<string, int> assetNameCounter = new Dictionary<string, int>();
    [SerializeField]
    protected CriAtom criware;
    public static readonly float DEFAULT_VOLUME = 1f;
    [SerializeField]
    protected CriWareInitializer initializer;
    protected bool isBusy = true;
    protected List<System.Action> loadingAssetCallbackList = new List<System.Action>();
    protected List<string> loadingAssetNameList = new List<string>();
    protected Dictionary<string, CriFsBinder> SoundBinders;

    protected void AddAssetNameCounter(string name)
    {
        if (this.assetNameCounter.ContainsKey(name))
        {
            Dictionary<string, int> dictionary;
            string str;
            int num = dictionary[str];
            (dictionary = this.assetNameCounter)[str = name] = num + 1;
        }
        else
        {
            this.assetNameCounter[name] = 1;
        }
    }

    public static bool checkServantVoice(string path, string name) => 
        SingletonMonoBehaviour<SoundManager>.Instance.IsExistsSound(path, name);

    protected bool ExistsAssetNameCounter(string name) => 
        (this.assetNameCounter.ContainsKey(name) && (this.assetNameCounter[name] > 0));

    public static void fadeoutBgm(float fadeoutTime)
    {
        Debug.Log("fadeoutBgm:" + fadeoutTime);
        BgmManager.FadeoutBgm(fadeoutTime);
    }

    public static string getAssetName(string name)
    {
        if (name.StartsWith("ba"))
        {
            return "Battle";
        }
        return null;
    }

    public static string getBgmName() => 
        BgmManager.GetBgmName();

    public static string getCharaVoiceAssetName(string name)
    {
        char[] separator = new char[] { '_' };
        string[] strArray = name.Split(separator, StringSplitOptions.RemoveEmptyEntries);
        if (name.StartsWith("NP_"))
        {
            if (strArray.Length >= 3)
            {
                return ("NoblePhantasm_" + strArray[1]);
            }
        }
        else if (strArray.Length >= 3)
        {
            string str = strArray[0];
            string str2 = strArray[2];
            if (!str2.StartsWith("B"))
            {
                return ("ChrVoice_" + str);
            }
            if (str2.StartsWith("B5"))
            {
                return ("NoblePhantasm_" + str);
            }
            return ("Servants_" + str);
        }
        return null;
    }

    public static string getCharaVoiceFileName(string name)
    {
        if (name.StartsWith("NP_"))
        {
            return name;
        }
        int index = name.IndexOf("_");
        if (index >= 0)
        {
            return name.Substring(index + 1);
        }
        return null;
    }

    public CriAtomEx.CueInfo GetCueInfo(string cueSheetName, string cueName)
    {
        CriAtomEx.CueInfo[] cueInfoList = this.GetCueInfoList(cueSheetName);
        if (cueInfoList != null)
        {
            foreach (CriAtomEx.CueInfo info in cueInfoList)
            {
                if (info.name == cueName)
                {
                    return info;
                }
            }
        }
        return new CriAtomEx.CueInfo();
    }

    public CriAtomEx.CueInfo[] GetCueInfoList(string cueSheetName)
    {
        foreach (CriAtomCueSheet sheet in this.criware.cueSheets)
        {
            if (sheet.name == cueSheetName)
            {
                CriAtomExAcb acb = CriAtom.GetAcb(sheet.name);
                if (acb != null)
                {
                    return acb.GetCueInfoList();
                }
            }
        }
        Debug.LogError("Can not found cueSheet : " + cueSheetName);
        return null;
    }

    public string[] GetCueList(string cueSheetName)
    {
        string[] strArray = null;
        foreach (CriAtomCueSheet sheet in this.criware.cueSheets)
        {
            if (sheet.name == cueSheetName)
            {
                CriAtomExAcb acb = CriAtom.GetAcb(sheet.name);
                if (acb != null)
                {
                    CriAtomEx.CueInfo[] cueInfoList = acb.GetCueInfoList();
                    strArray = new string[cueInfoList.Length];
                    int num2 = 0;
                    foreach (CriAtomEx.CueInfo info in cueInfoList)
                    {
                        strArray[num2++] = info.name;
                    }
                    return strArray;
                }
                Debug.LogError("Could not load cueSheet : " + sheet.name + ":" + sheet.acbFile);
            }
        }
        return strArray;
    }

    public string GetCueSheet(string cueName)
    {
        foreach (CriAtomCueSheet sheet in this.criware.cueSheets)
        {
            CriAtomExAcb acb = CriAtom.GetAcb(sheet.name);
            if ((acb != null) && acb.Exists(cueName))
            {
                return sheet.name;
            }
        }
        Debug.LogWarning("SoundManager:GetCueSheet:Not Found!:" + cueName);
        return null;
    }

    public static string getDownloadAssetName(string cueName) => 
        ("Audio/" + cueName + ".cpk.bytes");

    public static void initialize()
    {
        SingletonMonoBehaviour<SoundManager>.Instance.Setup();
        CriAtom.AttachDspBusSetting("DspBusSetting_0");
        if ((ManagerConfig.UseDebugCommand && (SingletonMonoBehaviour<SoundManager>.Instance != null)) && (SingletonMonoBehaviour<SoundManager>.Instance.initializer != null))
        {
            SingletonMonoBehaviour<SoundManager>.Instance.initializer.atomConfig.usesInGamePreview = true;
        }
        BgmManager.Initialize();
        SeManager.Initialize();
    }

    public static void initializeAssetStorage()
    {
        BgmManager.InitializeAssetStorage();
        SeManager.InitializeAssetStorage();
    }

    public static bool isBusySe(string name = null) => 
        SeManager.IsBusySe(name);

    public static bool isBusyVoice(string name = null) => 
        SeManager.IsBusyVoice(name);

    public bool IsExistsSound(string cueSheetName, string name)
    {
        foreach (CriAtomCueSheet sheet in this.criware.cueSheets)
        {
            if (sheet.name == cueSheetName)
            {
                CriAtomExAcb acb = CriAtom.GetAcb(sheet.name);
                if (acb != null)
                {
                    foreach (CriAtomEx.CueInfo info in acb.GetCueInfoList())
                    {
                        if (name == info.name)
                        {
                            return true;
                        }
                    }
                    return false;
                }
                Debug.LogError("Could not load cueSheet : " + sheet.name + ":" + sheet.acbFile);
            }
        }
        return false;
    }

    public static bool isPlayBgm(string name = null) => 
        BgmManager.IsPlayBgm(name);

    public static bool isPlayJingle(string name) => 
        BgmManager.IsPlayJingle(name);

    public static void loadAudioAssetStorage(string name, System.Action callback, CueType tp = 1)
    {
        SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(name, callback, tp);
    }

    public void LoadAudioAssetStorage(string name, System.Action callback, CueType tp = 1)
    {
        if (this.ExistsAssetNameCounter(name))
        {
            this.AddAssetNameCounter(name);
            if (callback != null)
            {
                callback();
            }
        }
        else
        {
            this.AddAssetNameCounter(name);
            Debug.Log("LoadAudioAssetStorage:add:" + name);
            foreach (CriAtomCueSheet sheet in this.criware.cueSheets)
            {
                if (sheet.name == name)
                {
                    if (callback != null)
                    {
                        callback();
                    }
                    return;
                }
            }
            new CpkLoader(this, name, callback, tp).Start();
        }
    }

    public static void loadBgm(string name, System.Action callbackFunc = null)
    {
        Debug.Log("loadBgm:" + name);
        BgmManager.LoadBgm(name, callbackFunc);
    }

    public static void loadSe(string name, System.Action callbackFunc = null)
    {
        SeManager.LoadSe(name, callbackFunc);
    }

    private void OnDestroy()
    {
        if (this.SoundBinders != null)
        {
            foreach (string str in this.SoundBinders.Keys)
            {
                this.SoundBinders[str].Dispose();
            }
            this.SoundBinders.Clear();
        }
    }

    public static void playBgm(string name)
    {
        Debug.Log("playBgm:" + name);
        BgmManager.PlayBgm(name, BgmManager.DEFAULT_VOLUME, 0f, 0L);
    }

    public static void playBgm(string name, float volume)
    {
        Debug.Log("playBgm:" + name);
        BgmManager.PlayBgm(name, volume, 0f, 0L);
    }

    public static void playBgm(string name, float volume, float fadeinTime)
    {
        Debug.Log("playBgm:" + name);
        BgmManager.PlayBgm(name, volume, fadeinTime, 0L);
    }

    public static SePlayer playCharaVoice(string name)
    {
        Debug.Log("playCharaVoice:" + name);
        string assetName = getCharaVoiceAssetName(name);
        string objectName = getCharaVoiceFileName(name);
        if (assetName != null)
        {
            return SeManager.PlayVoice(assetName, objectName, SeManager.DEFAULT_VOLUME, null);
        }
        return null;
    }

    public static void playJingle(string name)
    {
        Debug.Log("playJingle:" + name);
        BgmManager.PlayJingle(name, BgmManager.DEFAULT_VOLUME);
    }

    public static void playJingle(string name, System.Action callbackFunc)
    {
        Debug.Log("playJingle:" + name);
        BgmManager.PlayJingle(name, BgmManager.DEFAULT_VOLUME, callbackFunc);
    }

    public static void playJingle(string name, float volume)
    {
        Debug.Log("playJingle:" + name);
        BgmManager.PlayJingle(name, volume);
    }

    public static void playJingle(string name, float volume, System.Action callbackFunc)
    {
        Debug.Log("playJingle:" + name);
        BgmManager.PlayJingle(name, volume, callbackFunc);
    }

    public static SePlayer playSe(string name) => 
        SeManager.PlaySe(name, SeManager.DEFAULT_VOLUME, null);

    public static SePlayer playSe(string name, System.Action callbackFunc) => 
        SeManager.PlaySe(name, SeManager.DEFAULT_VOLUME, callbackFunc);

    public static SePlayer playSe(string name, float volume) => 
        SeManager.PlaySe(name, volume, null);

    public static SePlayer playSe(string assetName, string objectName) => 
        SeManager.PlaySe(assetName, objectName, SeManager.DEFAULT_VOLUME, null);

    public static SePlayer playSe(string name, float volume, System.Action callbackFunc) => 
        SeManager.PlaySe(name, volume, callbackFunc);

    public static SePlayer playSe(string assetName, string objectName, float volume, System.Action callbackFunc) => 
        SeManager.PlaySe(assetName, objectName, volume, callbackFunc);

    public static SePlayer playSeLoop(string name) => 
        SeManager.PlaySeLoop(name, SeManager.DEFAULT_VOLUME);

    public static SePlayer playSeLoop(string name, float volume) => 
        SeManager.PlaySeLoop(name, volume);

    public static SePlayer playSeLoop(string assetName, string name) => 
        SeManager.PlaySeLoop(assetName, name, SeManager.DEFAULT_VOLUME);

    public static SePlayer playSeLoop(string assetName, string name, float volume) => 
        SeManager.PlaySeLoop(assetName, name, volume);

    public static void playSystemSe(SeManager.SystemSeKind kind)
    {
        SeManager.PlaySystemSe(kind);
    }

    public static SePlayer playVoice(string name) => 
        SeManager.PlayVoice(name, SeManager.DEFAULT_VOLUME, null);

    public static SePlayer playVoice(string name, System.Action callbackFunc) => 
        SeManager.PlayVoice(name, SeManager.DEFAULT_VOLUME, callbackFunc);

    public static SePlayer playVoice(string name, float volume) => 
        SeManager.PlayVoice(name, volume, null);

    public static SePlayer playVoice(string name, float volume, System.Action callbackFunc) => 
        SeManager.PlayVoice(name, volume, callbackFunc);

    public static SePlayer playVoice(AssetData assetData, string objectName, float volume, System.Action callbackFunc) => 
        SeManager.PlayVoice(assetData, objectName, volume, callbackFunc);

    public static SePlayer playVoice(string assetName, string objectName, float volume, System.Action callbackFunc) => 
        SeManager.PlayVoice(assetName, objectName, volume, callbackFunc);

    public static void preloadBgm(string name, System.Action callbackFunc = null)
    {
        Debug.Log("preloadBgm:" + name);
        BgmManager.PreloadBgm(name, callbackFunc);
    }

    public static void reboot()
    {
        stopAll();
    }

    protected void RebootAssetNameCounter()
    {
        this.loadingAssetNameList.Clear();
        this.loadingAssetCallbackList.Clear();
        this.assetNameCounter.Clear();
    }

    protected static void RebootCueSheet()
    {
    }

    public static void releaseAudioAssetStorage(string name)
    {
        SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage(name);
    }

    public void ReleaseAudioAssetStorage(string name)
    {
        Debug.Log("ReleaseAudioAssetStorage:" + name);
        if (this.SubAssetNameCounter(name))
        {
            Debug.Log("ReleaseAudioAssetStorage:release");
            CriAtom.RemoveCueSheet(name);
            if (this.SoundBinders.ContainsKey(name))
            {
                this.SoundBinders[name].Dispose();
                this.SoundBinders.Remove(name);
            }
        }
    }

    public static void releaseBgm(string name)
    {
        Debug.Log("releaseBgm:" + name);
        BgmManager.ReleaseBgm(name);
    }

    public static void releaseSe(string name)
    {
        SeManager.ReleaseSe(name);
    }

    public static void reset()
    {
        Debug.Log("resetAudio:");
        BgmManager.Reset();
        SeManager.Reset();
    }

    public static void SetBgmMasterVolume(float volume)
    {
        Debug.Log("setBgmMasterVolume:" + volume);
        BgmManager.SetMasterVolume(volume);
    }

    public static void SetSeMasterVolume(float volume)
    {
        Debug.Log("setSeMasterVolume:" + volume);
        SeManager.SetMasterVolume(volume);
    }

    protected void Setup()
    {
        bool flag = false;
        this.criware = base.gameObject.GetComponent<CriAtom>();
        if (this.SoundBinders == null)
        {
            this.SoundBinders = new Dictionary<string, CriFsBinder>();
        }
        if (this.criware == null)
        {
            this.criware = base.gameObject.AddComponent<CriAtom>();
            this.criware.acfFile = "FGO.acf";
            flag = true;
        }
        if (flag)
        {
            CriAtom.AddCueSheet("ResourceSound", "ResourceSound.acb.bytes", "ResourceSound.awb.bytes", null);
            this.criware.dontDestroyOnLoad = true;
        }
    }

    public static void SetVoiceMasterVolume(float volume)
    {
        Debug.Log("setVoiceMasterVolume:" + volume);
        SeManager.SetVoiceMasterVolume(volume);
    }

    public static void stopAll()
    {
        Debug.Log("stopAudio:");
        BgmManager.StopBgm();
        BgmManager.StopJingle();
        SeManager.StopSeAll(0f);
    }

    public static void stopBgm()
    {
        Debug.Log("stopBgm:");
        BgmManager.StopBgm();
    }

    public static void stopJingle()
    {
        Debug.Log("stopJingle:");
        BgmManager.StopJingle();
    }

    public static void stopSe(float fadeoutTime = 0f)
    {
        SeManager.StopSeAll(fadeoutTime);
    }

    public static void stopSe(string name, float fadeoutTime = 0f)
    {
        SeManager.StopSe(name, fadeoutTime);
    }

    public static void stopVoice(string name, float fadeoutTime = 0f)
    {
        SeManager.StopSe(name, fadeoutTime);
    }

    public static void stopVoice(string assetName, string objectName, float fadeoutTime = 0f)
    {
        SeManager.StopSe(assetName, objectName, fadeoutTime);
    }

    protected bool SubAssetNameCounter(string name)
    {
        if (!this.assetNameCounter.ContainsKey(name))
        {
            Debug.LogWarning("SoundManager:SubAssetNameCounter:has not key:" + name);
        }
        else
        {
            Dictionary<string, int> dictionary;
            string str;
            int num = dictionary[str];
            (dictionary = this.assetNameCounter)[str = name] = num - 1;
            if (this.assetNameCounter[name] == 0)
            {
                return true;
            }
        }
        return false;
    }

    public CriAtom CriwareComp =>
        this.criware;

    public CriWareInitializer Initializer =>
        this.initializer;

    public bool IsBusy =>
        (BgmManager.IsBusy || SeManager.IsBusy);

    protected class CpkLoader
    {
        public System.Action callback;
        protected SoundManager.CueType cueType;
        public SoundManager manager;
        public string name;

        public CpkLoader(SoundManager manager, string name, System.Action callback, SoundManager.CueType cueType)
        {
            this.manager = manager;
            this.name = name;
            this.callback = callback;
            this.cueType = cueType;
        }

        protected void EndLoadCallback(AssetData data)
        {
            this.manager.StartCoroutine(this.SetupCpk(this.name));
        }

        [DebuggerHidden]
        protected IEnumerator SetupCpk(string name) => 
            new <SetupCpk>c__Iterator10 { 
                name = name,
                <$>name = name,
                <>f__this = this
            };

        public void Start()
        {
            string name = SoundManager.getDownloadAssetName(this.name);
            if (!AssetManager.downloadAssetStorage(name, new AssetLoader.LoadEndDataHandler(this.EndLoadCallback)))
            {
                Debug.LogError("Can not Found Audio Asset :" + name);
                if (this.callback != null)
                {
                    this.callback();
                }
            }
        }

        [CompilerGenerated]
        private sealed class <SetupCpk>c__Iterator10 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal object $current;
            internal int $PC;
            internal string <$>name;
            internal SoundManager.CpkLoader <>f__this;
            internal CriFsBindRequest <bind_request>__2;
            internal CriFsBinder <binder>__0;
            internal string <cpkAssetPath>__1;
            internal string name;

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
                        this.<binder>__0 = new CriFsBinder();
                        this.<>f__this.manager.SoundBinders[this.name] = this.<binder>__0;
                        this.<cpkAssetPath>__1 = AssetManager.CachePathName + "Audio@" + this.name + ".cpk.bytes";
                        if (File.Exists(this.<cpkAssetPath>__1))
                        {
                            this.<bind_request>__2 = CriFsUtility.BindCpk(this.<binder>__0, this.<cpkAssetPath>__1);
                            this.$current = this.<bind_request>__2.WaitForDone(this.<>f__this.manager);
                            this.$PC = 1;
                            return true;
                        }
                        Debug.LogError("Can not Found Audio :" + this.<cpkAssetPath>__1);
                        if (this.<>f__this.callback != null)
                        {
                            this.<>f__this.callback();
                        }
                        break;

                    case 1:
                        if (this.<bind_request>__2.error == null)
                        {
                            if ((this.<>f__this.cueType == SoundManager.CueType.ALL) && (this.<binder>__0.GetFileSize(this.name + ".awb") >= 0L))
                            {
                                CriAtom.AddCueSheet(this.name, this.name + ".acb", this.name + ".awb", this.<binder>__0);
                            }
                            else
                            {
                                CriAtom.AddCueSheet(this.name, this.name + ".acb", string.Empty, this.<binder>__0);
                            }
                            if (this.<>f__this.callback != null)
                            {
                                this.<>f__this.callback();
                            }
                            this.$PC = -1;
                            break;
                        }
                        Debug.LogError("Error");
                        if (this.<>f__this.callback != null)
                        {
                            this.<>f__this.callback();
                        }
                        break;
                }
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

    public enum CueType
    {
        ACB_ONLY,
        ALL
    }
}

