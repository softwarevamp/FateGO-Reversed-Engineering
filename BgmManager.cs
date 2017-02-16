using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class BgmManager : SingletonMonoBehaviour<BgmManager>
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache1B;
    protected static string[] bgmAssetBundleList = null;
    protected List<string> BgmDataList;
    protected float bgmFadeinTime = -1f;
    protected CriAtomSource bgmFadePlayer;
    protected string bgmName;
    protected CriAtomSource bgmPlayer;
    protected float bgmVolume;
    protected static readonly string DEFAULT_BGM_CUESHEET = "Bgm";
    public static readonly float DEFAULT_VOLUME = 1f;
    protected float fadeBgmVolume;
    protected float fadeinTime;
    protected float fadeoutTime;
    protected bool isLoadingPlayer;
    protected static bool isMute = false;
    protected System.Action jingleCallbackFunc;
    protected string jingleName;
    protected float jingleVolume;
    private static int loadBgmCnt;
    private static string[] LoadBgmList = new string[] { "BGM_SYSTEM", "BGM_BATTLE", "BGM_EVENT", "BGM_MAP", "BGM_EVENT2" };
    protected int LoadCounter;
    public static readonly float LOW_VOLUME = 0.1f;
    protected static float masterVolume = 1f;
    protected int playRequestNumber;
    protected float playTime;
    protected float playVolume;
    protected string preloadName;
    protected long startTime;

    protected bool CheckPlaying(CriAtomSource player) => 
        ((player.status == CriAtomSource.Status.Prep) || (player.status == CriAtomSource.Status.Playing));

    public static void FadeoutBgm(float fadeoutTime)
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        if (instance != null)
        {
            instance.FadeoutBgmLocal(fadeoutTime);
        }
    }

    protected void FadeoutBgmLocal(float fadeoutTime)
    {
        if (this.bgmName != null)
        {
            if ((this.jingleName == null) && !isMute)
            {
                this.FadeoutLocal(fadeoutTime);
            }
            this.bgmName = null;
            this.bgmVolume = 0f;
        }
    }

    protected void FadeoutLocal(float fadeoutTime)
    {
        if (this.isLoadingPlayer)
        {
            this.StopLocal();
        }
        else if (this.IsBgmPlaying() && (this.fadeoutTime <= 0f))
        {
            if ((this.fadeinTime > 0f) && this.CheckPlaying(this.bgmFadePlayer))
            {
                CriAtomSource bgmFadePlayer = this.bgmFadePlayer;
                this.bgmFadePlayer = this.bgmPlayer;
                this.bgmPlayer = bgmFadePlayer;
                this.bgmFadePlayer.Stop();
            }
            this.fadeoutTime = fadeoutTime;
            this.fadeinTime = 0f;
            this.fadeBgmVolume = this.bgmPlayer.volume;
            this.playTime = -1f;
        }
    }

    public static AssetData.Type GetAssetType(string name)
    {
        if (SingletonMonoBehaviour<SoundManager>.Instance.GetCueSheet(name) == "ResourceSound")
        {
            return AssetData.Type.ASSET_RESOURCE;
        }
        return AssetData.Type.ASSET_STORAGE;
    }

    public static string GetBgmName()
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        return instance?.BgmName;
    }

    protected static float GetPlayerVolume(float volume) => 
        (volume * masterVolume);

    public static void Initialize()
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        if (instance != null)
        {
            instance.InitializeLocal();
        }
    }

    public static void InitializeAssetStorage()
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        if (instance != null)
        {
            instance.LoadBgmData();
        }
    }

    protected void InitializeLocal()
    {
        if (this.bgmPlayer == null)
        {
            this.bgmPlayer = base.gameObject.AddComponent<CriAtomSource>();
            this.bgmPlayer.androidUseLowLatencyVoicePool = false;
            this.bgmPlayer.cueSheet = "BGM";
        }
        else
        {
            this.bgmPlayer.Stop();
            this.bgmName = null;
            this.jingleName = null;
            this.jingleCallbackFunc = null;
        }
        if (this.bgmFadePlayer == null)
        {
            this.bgmFadePlayer = base.gameObject.AddComponent<CriAtomSource>();
            this.bgmFadePlayer.androidUseLowLatencyVoicePool = false;
            this.bgmFadePlayer.cueSheet = "BGM";
        }
        else
        {
            this.bgmFadePlayer.Stop();
        }
    }

    protected bool IsBgmPlaying() => 
        ((this.bgmPlayer.status == CriAtomSource.Status.Prep) || (this.bgmPlayer.status == CriAtomSource.Status.Playing));

    public static bool IsPlayBgm(string name)
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        return instance?.IsPlayBgmLocal(name);
    }

    protected bool IsPlayBgmLocal(string name)
    {
        if (name != null)
        {
            return (this.bgmName == name);
        }
        return (this.bgmName != null);
    }

    public static bool IsPlayJingle(string name)
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        return instance?.IsPlayJingleLocal(name);
    }

    protected bool IsPlayJingleLocal(string name)
    {
        if (name != null)
        {
            return (this.jingleName == name);
        }
        return (this.jingleName != null);
    }

    protected void LateUpdate()
    {
        if (this.bgmPlayer != null)
        {
            float deltaTime = RealTime.deltaTime;
            if (deltaTime > 0.5f)
            {
                deltaTime = Time.deltaTime;
            }
            bool flag = false;
            if (this.fadeinTime > 0f)
            {
                flag = this.CheckPlaying(this.bgmFadePlayer);
            }
            if (flag || this.IsBgmPlaying())
            {
                if (this.fadeoutTime > 0f)
                {
                    if (this.playTime >= 0f)
                    {
                        this.playTime += deltaTime;
                    }
                    else
                    {
                        this.playTime = 0f;
                    }
                    if (this.playTime < this.fadeoutTime)
                    {
                        if (isMute)
                        {
                            this.bgmPlayer.volume = 0f;
                        }
                        else
                        {
                            float num2 = 1f - (this.playTime / this.fadeoutTime);
                            this.bgmPlayer.volume = this.fadeBgmVolume * num2;
                        }
                    }
                    else
                    {
                        this.StopLocal();
                    }
                }
                else if (this.fadeinTime > 0f)
                {
                    CriAtomSource bgmFadePlayer;
                    CriAtomSource bgmPlayer;
                    if (this.CheckPlaying(this.bgmFadePlayer))
                    {
                        bgmFadePlayer = this.bgmFadePlayer;
                        bgmPlayer = this.bgmPlayer;
                    }
                    else
                    {
                        bgmFadePlayer = this.bgmPlayer;
                        bgmPlayer = null;
                    }
                    if (this.playTime >= 0f)
                    {
                        this.playTime += deltaTime;
                    }
                    else
                    {
                        this.playTime = 0f;
                    }
                    if (isMute)
                    {
                        bgmFadePlayer.volume = 0f;
                        if (this.playTime >= this.fadeinTime)
                        {
                            this.fadeinTime = 0f;
                        }
                        CriAtomSource source3 = this.bgmFadePlayer;
                        this.bgmFadePlayer = this.bgmPlayer;
                        this.bgmPlayer = source3;
                    }
                    else if (this.playTime < this.fadeinTime)
                    {
                        bgmFadePlayer.volume = ((masterVolume * this.playVolume) * this.playTime) / this.fadeinTime;
                        if (bgmPlayer != null)
                        {
                            bgmPlayer.volume = this.fadeBgmVolume - ((this.fadeBgmVolume * this.playTime) / this.fadeinTime);
                        }
                    }
                    else
                    {
                        this.fadeinTime = 0f;
                        bgmFadePlayer.volume = masterVolume * this.playVolume;
                        if (bgmPlayer != null)
                        {
                            bgmPlayer.volume = 0f;
                            CriAtomSource source4 = this.bgmFadePlayer;
                            this.bgmFadePlayer = this.bgmPlayer;
                            this.bgmPlayer = source4;
                        }
                    }
                }
            }
            else if (!this.isLoadingPlayer)
            {
                this.fadeoutTime = 0f;
                if (this.jingleName != null)
                {
                    System.Action jingleCallbackFunc = this.jingleCallbackFunc;
                    this.jingleName = null;
                    this.jingleCallbackFunc = null;
                    if (this.bgmName != null)
                    {
                        string bgmName = this.bgmName;
                        this.bgmName = null;
                        this.PlayBgmLocal(bgmName, this.bgmVolume, this.bgmFadeinTime, 0L);
                    }
                    else
                    {
                        this.StopLocal();
                    }
                    if (jingleCallbackFunc != null)
                    {
                        jingleCallbackFunc();
                    }
                }
                else if (this.bgmName != null)
                {
                    if (this.bgmFadeinTime >= 0f)
                    {
                        string name = this.bgmName;
                        this.bgmName = null;
                        this.PlayBgmLocal(name, this.bgmVolume, this.bgmFadeinTime, 0L);
                    }
                    else
                    {
                        Debug.Log("App bgm play stop " + this.bgmName);
                        this.bgmName = null;
                        this.bgmVolume = 0f;
                        this.StopLocal();
                    }
                }
            }
        }
    }

    [Obsolete("ADX2においては何もしないメソッドになっているので削除可能")]
    public static bool LoadBgm(string name, System.Action callbackFunc)
    {
        if (SingletonMonoBehaviour<BgmManager>.Instance == null)
        {
            return false;
        }
        if (callbackFunc != null)
        {
            callbackFunc();
        }
        return true;
    }

    protected void LoadBgmData()
    {
        loadBgmCnt = LoadBgmList.Length;
        for (int i = 0; i < LoadBgmList.Length; i++)
        {
            if (<>f__am$cache1B == null)
            {
                <>f__am$cache1B = (System.Action) (() => loadBgmCnt--);
            }
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage(LoadBgmList[i], <>f__am$cache1B, SoundManager.CueType.ALL);
        }
    }

    public static void PlayBgm(string name, float volume, float fadeinTime, long startTime = 0)
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        if (instance != null)
        {
            instance.PlayBgmLocal(name, volume, fadeinTime, startTime);
        }
    }

    protected void PlayBgmLocal(string name, float volume, float fadeinTime, long startTime = 0)
    {
        if (name.Equals(this.bgmName))
        {
            if ((this.fadeoutTime <= 0f) && (volume == this.bgmVolume))
            {
                return;
            }
            this.bgmVolume = volume;
            this.bgmFadeinTime = fadeinTime;
            if (this.jingleName != null)
            {
                return;
            }
        }
        else
        {
            this.bgmName = name;
            this.startTime = startTime;
        }
        this.bgmVolume = volume;
        this.bgmFadeinTime = fadeinTime;
        if (((this.jingleName == null) && !isMute) && this.PlayLocal(name, volume, fadeinTime))
        {
            this.bgmFadeinTime = 0f;
        }
        else
        {
            this.bgmFadeinTime = fadeinTime;
        }
    }

    public static void PlayJingle(string name, float volume)
    {
        PlayJingle(name, volume, null);
    }

    public static void PlayJingle(string name, float volume, System.Action callbackFunc)
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        if (instance != null)
        {
            instance.PlayJingleLocal(name, volume, callbackFunc);
        }
    }

    protected void PlayJingleLocal(string name, float volume, System.Action callbackFunc)
    {
        if (this.jingleName != null)
        {
            System.Action jingleCallbackFunc = this.jingleCallbackFunc;
            if (jingleCallbackFunc != null)
            {
                this.jingleCallbackFunc = null;
                jingleCallbackFunc();
            }
        }
        this.jingleName = name;
        this.jingleVolume = volume;
        this.jingleCallbackFunc = callbackFunc;
        this.PlayLocal(name, !isMute ? volume : 0f, -1f);
    }

    protected bool PlayLocal(string name, float volume, float fadeinTime)
    {
        CriAtomSource bgmFadePlayer;
        if ((fadeinTime > 0f) && this.IsBgmPlaying())
        {
            bgmFadePlayer = this.bgmFadePlayer;
            this.fadeBgmVolume = this.bgmPlayer.volume;
        }
        else
        {
            bgmFadePlayer = this.bgmPlayer;
            this.bgmFadePlayer.Stop();
        }
        this.fadeinTime = fadeinTime;
        this.fadeoutTime = 0f;
        this.playVolume = volume;
        this.playTime = -1f;
        this.fadeoutTime = 0f;
        char[] separator = new char[] { '/' };
        string[] strArray = name.Split(separator);
        string cueName = strArray[strArray.Length - 1];
        bgmFadePlayer.Stop();
        bgmFadePlayer.cueSheet = SingletonMonoBehaviour<SoundManager>.Instance.GetCueSheet(cueName);
        bgmFadePlayer.cueName = cueName;
        bgmFadePlayer.volume = (fadeinTime <= 0f) ? (volume * masterVolume) : 0f;
        bgmFadePlayer.startTime = (int) this.startTime;
        bgmFadePlayer.Play();
        this.isLoadingPlayer = false;
        this.playRequestNumber++;
        return true;
    }

    [Obsolete("ADX2においては何もしないメソッドになっているので削除可能")]
    public static bool PreloadBgm(string name, System.Action callbackFunc)
    {
        if (SingletonMonoBehaviour<BgmManager>.Instance == null)
        {
            return false;
        }
        if (callbackFunc != null)
        {
            callbackFunc();
        }
        return true;
    }

    [Obsolete("ADX2においては何もしないメソッドになっているので削除可能")]
    public static void ReleaseBgm(string name)
    {
        if (SingletonMonoBehaviour<BgmManager>.Instance == null)
        {
        }
    }

    public static void Reset()
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        if (instance != null)
        {
            instance.ResetLocal();
        }
    }

    protected void ResetLocal()
    {
        this.StopLocal();
        if (this.bgmName != null)
        {
            this.bgmName = null;
        }
        if (this.jingleName != null)
        {
            this.jingleName = null;
            System.Action jingleCallbackFunc = this.jingleCallbackFunc;
            if (jingleCallbackFunc != null)
            {
                this.jingleCallbackFunc = null;
                jingleCallbackFunc();
            }
        }
    }

    public static void SetMasterVolume(float volume)
    {
        if (masterVolume != volume)
        {
            masterVolume = volume;
            BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
            if (instance != null)
            {
                instance.SetMasterVolumeLocal(volume);
            }
        }
    }

    protected void SetMasterVolumeLocal(float volume)
    {
        if (this.jingleName != null)
        {
            if ((this.fadeinTime == 0f) || (this.fadeoutTime == 0f))
            {
                this.bgmPlayer.volume = this.jingleVolume * masterVolume;
            }
        }
        else if ((this.bgmName != null) && ((this.fadeinTime == 0f) || (this.fadeoutTime == 0f)))
        {
            this.bgmPlayer.volume = this.bgmVolume * masterVolume;
        }
    }

    public static void SetMute(bool isMute)
    {
        if (BgmManager.isMute != isMute)
        {
            BgmManager.isMute = isMute;
            BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
            if (instance != null)
            {
                instance.SetMuteLocal(isMute);
            }
        }
    }

    protected void SetMuteLocal(bool isMute)
    {
        if (isMute)
        {
            if (this.jingleName != null)
            {
                this.bgmPlayer.volume = 0f;
            }
            else if (this.bgmName != null)
            {
                this.StopLocal();
            }
        }
        else if (this.jingleName != null)
        {
            if ((this.fadeinTime == 0f) || (this.fadeoutTime == 0f))
            {
                this.bgmPlayer.volume = this.jingleVolume * masterVolume;
            }
        }
        else if (this.bgmName != null)
        {
            string preloadName = this.preloadName;
            float bgmVolume = this.bgmVolume;
            this.preloadName = this.bgmName;
            this.bgmName = null;
            this.bgmVolume = -1f;
            this.PlayBgmLocal(this.preloadName, bgmVolume, 0f, 0L);
            this.preloadName = preloadName;
        }
    }

    protected void SetPlayerVolume(float mixVolume)
    {
        this.bgmVolume = mixVolume / masterVolume;
        if (this.bgmPlayer != null)
        {
            this.bgmPlayer.volume = masterVolume * this.bgmVolume;
        }
    }

    public static void StopBgm()
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        if (instance != null)
        {
            instance.StopBgmLocal();
        }
    }

    protected void StopBgmLocal()
    {
        if ((this.bgmName != null) || (this.fadeoutTime > 0f))
        {
            this.bgmName = null;
            this.bgmVolume = 0f;
            this.bgmFadeinTime = -1f;
            this.startTime = 0L;
            if (this.jingleName == null)
            {
                this.StopLocal();
            }
        }
    }

    public static void StopJingle()
    {
        BgmManager instance = SingletonMonoBehaviour<BgmManager>.Instance;
        if (instance != null)
        {
            instance.StopJingleLocal();
        }
    }

    protected void StopJingleLocal()
    {
        if (this.jingleName != null)
        {
            System.Action jingleCallbackFunc = this.jingleCallbackFunc;
            this.jingleName = null;
            this.jingleVolume = 0f;
            this.jingleCallbackFunc = null;
            this.StopLocal();
            if (jingleCallbackFunc != null)
            {
                jingleCallbackFunc();
            }
        }
    }

    protected void StopLocal()
    {
        this.playRequestNumber++;
        this.isLoadingPlayer = false;
        if (this.IsBgmPlaying())
        {
            this.bgmPlayer.Stop();
            this.bgmFadePlayer.Stop();
        }
        this.fadeinTime = 0f;
        this.fadeoutTime = 0f;
    }

    protected void UpdateVolume()
    {
        if (this.bgmPlayer != null)
        {
            this.bgmPlayer.volume = masterVolume * this.bgmVolume;
        }
    }

    public string BgmName =>
        this.bgmName;

    public float BgmVolume
    {
        get => 
            this.bgmVolume;
        set
        {
            this.bgmVolume = value;
            this.UpdateVolume();
        }
    }

    public static bool IsBusy =>
        (loadBgmCnt > 0);

    public static bool IsMute =>
        isMute;

    public float JingleVolume =>
        this.jingleVolume;

    public float PlayerVolume
    {
        get => 
            GetPlayerVolume(this.bgmVolume);
        set
        {
            this.SetPlayerVolume(value);
        }
    }

    public int PlayTime
    {
        get
        {
            int time = 0;
            if (this.bgmPlayer != null)
            {
                time = (int) this.bgmPlayer.time;
            }
            return time;
        }
        set
        {
            if (this.bgmPlayer != null)
            {
                this.bgmPlayer.startTime = value;
            }
        }
    }
}

