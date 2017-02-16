using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class SeManager : SingletonMonoBehaviour<SeManager>
{
    [CompilerGenerated]
    private static Func<string, string> <>f__am$cache11;
    [CompilerGenerated]
    private static Func<string, string> <>f__am$cache12;
    public static float DEFAULT_VOLUME = 1f;
    public static bool IsBusy;
    protected static bool isMute;
    protected static int loadCounter;
    protected static float masterVoiceVolume = 1f;
    protected static float masterVolume = 1f;
    protected GameObject playerGameObject;
    protected static string[] residentSeAssetBundleList;
    protected static string[] seAssetBundleList;
    [SerializeField]
    public int seMax = 8;
    protected List<CriAtomExPlayback> sePlaybackStatusList;
    protected SePlayer[] sePlayerStatusList;
    protected int sePlayNum;
    protected CriAtomSource[] seSourceList;
    protected string[] systemSeClipNames = new string[] { "sy1", "sy2", "sy3", "sy4", "sy5", "sy6", "sy7", "sy8", "sy1a", "um1", "um2" };
    protected CriAtomSource systemSeSource;
    protected List<SePlayer> workSePlayerStatusList;

    public CriAtomSource GetAudioSource(SePlayer player)
    {
        for (int i = 0; i < this.seMax; i++)
        {
            if (this.sePlayerStatusList[i] == null)
            {
                this.sePlayerStatusList[i] = player;
                return this.seSourceList[i];
            }
        }
        return null;
    }

    public CriAtomSource GetAudioSource(SePlayer player, string cueSheet, string cueName)
    {
        for (int i = 0; i < this.seMax; i++)
        {
            SePlayer player2 = this.sePlayerStatusList[i];
            CriAtomSource source = this.seSourceList[i];
            if (((player2 != null) && (source.cueSheet == cueSheet)) && (source.cueName == cueName))
            {
                player2.StopSe(0f);
                this.sePlayerStatusList[i] = player;
                return source;
            }
        }
        return this.GetAudioSource(player);
    }

    public static string GetPathName(string name) => 
        ("Se/" + name);

    protected SePlayer GetSePlayer(int num)
    {
        for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
        {
            SePlayer player = this.workSePlayerStatusList[i];
            if (player.PlayNum == num)
            {
                return player;
            }
        }
        return null;
    }

    protected SePlayer GetSePlayer(string name)
    {
        for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
        {
            SePlayer player = this.workSePlayerStatusList[i];
            if (player.DataName == name)
            {
                return player;
            }
        }
        return null;
    }

    protected SePlayer GetSePlayer(SePlayer.SeType type, string name)
    {
        for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
        {
            SePlayer player = this.workSePlayerStatusList[i];
            if ((player.Type == type) && (player.DataName == name))
            {
                return player;
            }
        }
        return null;
    }

    protected SePlayer GetSePlayer(string assetName, string objectName)
    {
        for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
        {
            SePlayer player = this.workSePlayerStatusList[i];
            if ((player.AssetName == assetName) && (player.DataName == objectName))
            {
                return player;
            }
        }
        return null;
    }

    protected SePlayer GetSePlayer(SePlayer.SeType type, string assetName, string objectName)
    {
        for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
        {
            SePlayer player = this.workSePlayerStatusList[i];
            if (((player.Type == type) && (player.AssetName == assetName)) && (player.DataName == objectName))
            {
                return player;
            }
        }
        return null;
    }

    public static void Initialize()
    {
        if (seAssetBundleList != null)
        {
            seAssetBundleList = null;
        }
        if (residentSeAssetBundleList != null)
        {
            residentSeAssetBundleList = null;
        }
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        if (instance != null)
        {
            instance.InitializeLocal();
        }
    }

    public static void InitializeAssetStorage()
    {
        IsBusy = true;
        loadCounter = 2;
        SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage("ResidentSE", new System.Action(SeManager.LoadEndResidentSeAsset), SoundManager.CueType.ALL);
        SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage("SE", new System.Action(SeManager.LoadEndResidentSeAsset), SoundManager.CueType.ALL);
    }

    protected void InitializeLocal()
    {
        if (this.playerGameObject == null)
        {
            this.playerGameObject = new GameObject();
            this.playerGameObject.name = "SePlayerObject";
            this.playerGameObject.transform.parent = base.gameObject.transform;
            this.seSourceList = new CriAtomSource[this.seMax];
            this.sePlayerStatusList = new SePlayer[this.seMax];
            this.workSePlayerStatusList = new List<SePlayer>();
            this.systemSeSource = this.playerGameObject.AddComponent<CriAtomSource>();
            this.systemSeSource.androidUseLowLatencyVoicePool = true;
            for (int i = 0; i < this.seMax; i++)
            {
                this.seSourceList[i] = this.playerGameObject.AddComponent<CriAtomSource>();
                this.seSourceList[i].use3dPositioning = false;
                this.seSourceList[i].androidUseLowLatencyVoicePool = true;
            }
        }
        else
        {
            this.StopSeAllLocal(0f);
        }
    }

    public static bool IsBusySe(string name = null)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.IsBusySeLocal(name);
    }

    protected bool IsBusySeLocal(string name)
    {
        if (name != null)
        {
            SePlayer sePlayer = this.GetSePlayer(SePlayer.SeType.NORMAL, name);
            return sePlayer?.IsBusy;
        }
        for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
        {
            SePlayer player2 = this.workSePlayerStatusList[i];
            if ((player2.Type == SePlayer.SeType.NORMAL) && player2.IsBusy)
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsBusyVoice(string name = null)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.IsBusyVoiceLocal(name);
    }

    protected bool IsBusyVoiceLocal(string name)
    {
        if (name != null)
        {
            SePlayer sePlayer = this.GetSePlayer(SePlayer.SeType.VOICE, name);
            return sePlayer?.IsBusy;
        }
        for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
        {
            SePlayer player2 = this.workSePlayerStatusList[i];
            if ((player2.Type == SePlayer.SeType.VOICE) && player2.IsBusy)
            {
                return true;
            }
        }
        return false;
    }

    protected static bool IsDownloadFile(string pathName) => 
        (Array.IndexOf<string>(seAssetBundleList, pathName) >= 0);

    protected void LateUpdate()
    {
        if (this.playerGameObject != null)
        {
            float deltaTime = RealTime.deltaTime;
            if (deltaTime > 0.5f)
            {
                deltaTime = Time.deltaTime;
            }
            if (this.workSePlayerStatusList != null)
            {
                for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
                {
                    SePlayer player = this.workSePlayerStatusList[i];
                    if (!player.Update(deltaTime))
                    {
                        for (int j = 0; j < this.seMax; j++)
                        {
                            SePlayer player2 = this.sePlayerStatusList[j];
                            if (player == player2)
                            {
                                this.sePlayerStatusList[j] = null;
                                break;
                            }
                        }
                        this.workSePlayerStatusList.RemoveAt(i);
                        player.Callback();
                    }
                }
            }
        }
    }

    protected static void LoadEndResidentSeAsset()
    {
        loadCounter--;
        if (loadCounter == 0)
        {
            string[] cueList = SingletonMonoBehaviour<SoundManager>.Instance.GetCueList("SE");
            if (cueList != null)
            {
                if (<>f__am$cache11 == null)
                {
                    <>f__am$cache11 = str => "Se/" + str;
                }
                seAssetBundleList = cueList.Select<string, string>(<>f__am$cache11).ToArray<string>();
            }
            cueList = SingletonMonoBehaviour<SoundManager>.Instance.GetCueList("ResidentSE");
            if (cueList != null)
            {
                if (<>f__am$cache12 == null)
                {
                    <>f__am$cache12 = str => "ResidentSE/" + str;
                }
                residentSeAssetBundleList = cueList.Select<string, string>(<>f__am$cache12).ToArray<string>();
            }
            IsBusy = false;
        }
    }

    public static void LoadSe(string name, System.Action callbackFunc)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        if (instance != null)
        {
            instance.LoadSeLocal(name, callbackFunc);
        }
    }

    public void LoadSeLocal(string name, System.Action callbackFunc)
    {
        if (callbackFunc != null)
        {
            callbackFunc();
        }
    }

    protected SePlayer PlayLocal(SePlayer.SeType seType, AssetData.Type assetType, string pathName, string name, float volume, bool isLoop, System.Action callbackFunc)
    {
        int num = ++this.sePlayNum;
        SePlayer item = new SePlayer(num, seType, assetType, pathName, name, volume, isLoop, callbackFunc);
        if (this.workSePlayerStatusList != null)
        {
            this.workSePlayerStatusList.Add(item);
        }
        return item;
    }

    public static SePlayer PlaySe(string name, float volume, System.Action callbackFunc)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.NORMAL, name, volume, false, callbackFunc);
    }

    public static SePlayer PlaySe(AssetData assetData, string objectName, float volume, System.Action callbackFunc)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.NORMAL, assetData.Name, objectName, volume, false, callbackFunc);
    }

    public static SePlayer PlaySe(string assetName, string objectName, float volume, System.Action callbackFunc)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.NORMAL, assetName, objectName, volume, false, callbackFunc);
    }

    protected SePlayer PlaySeLocal(SePlayer.SeType seType, string name, float volume, bool isLoop, System.Action callbackFunc)
    {
        SePlayer item = null;
        int num = ++this.sePlayNum;
        string cueSheet = SingletonMonoBehaviour<SoundManager>.Instance.GetCueSheet(name);
        if (cueSheet != null)
        {
            item = new SePlayer(num, seType, AssetData.Type.ASSET_STORAGE, cueSheet, name, volume, isLoop, callbackFunc);
            this.workSePlayerStatusList.Add(item);
            return item;
        }
        if (Array.IndexOf<string>(residentSeAssetBundleList, "ResidentSE/" + name) >= 0)
        {
            item = new SePlayer(num, seType, AssetData.Type.ASSET_STORAGE, "ResidentSE", name, volume, isLoop, callbackFunc);
            this.workSePlayerStatusList.Add(item);
            return item;
        }
        if (Array.IndexOf<string>(seAssetBundleList, "Se/" + name) >= 0)
        {
            item = new SePlayer(num, seType, AssetData.Type.ASSET_STORAGE, "Se", name, volume, isLoop, callbackFunc);
            this.workSePlayerStatusList.Add(item);
            return item;
        }
        Debug.LogError("SeManager: Not found Se : " + name);
        return item;
    }

    protected SePlayer PlaySeLocal(SePlayer.SeType seType, string pathName, string name, float volume, bool isLoop, System.Action callbackFunc)
    {
        SePlayer player = this.PlayLocal(seType, AssetData.Type.ASSET_STORAGE, pathName, name, volume, isLoop, callbackFunc);
        if (player == null)
        {
            Debug.LogWarning("SePlay: player all busy [" + pathName + " " + name + "]");
        }
        return player;
    }

    public static SePlayer PlaySeLoop(string name, float volume)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.NORMAL, name, volume, true, null);
    }

    public static SePlayer PlaySeLoop(AssetData assetData, string objectName, float volume)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.NORMAL, assetData.Name, objectName, volume, true, null);
    }

    public static SePlayer PlaySeLoop(string assetName, string objectName, float volume)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.NORMAL, assetName, objectName, volume, true, null);
    }

    public static CriAtomSource PlaySystemSe(SystemSeKind kind) => 
        SingletonMonoBehaviour<SeManager>.Instance.PlaySystemSeLocal(kind);

    protected CriAtomSource PlaySystemSeLocal(SystemSeKind kind)
    {
        string str = this.systemSeClipNames[(int) kind];
        if (this.systemSeSource == null)
        {
            return null;
        }
        CriAtomSource systemSeSource = this.systemSeSource;
        if (!IsMute)
        {
            systemSeSource.cueSheet = "ResourceSound";
            systemSeSource.cueName = str;
            systemSeSource.volume = masterVolume;
            Debug.Log("PlaySE:" + str + "ok?");
            systemSeSource.Play();
        }
        return systemSeSource;
    }

    public static SePlayer PlayVoice(string name, float volume, System.Action callbackFunc)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.VOICE, name, volume, false, callbackFunc);
    }

    public static SePlayer PlayVoice(AssetData assetData, string objectName, float volume, System.Action callbackFunc)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.VOICE, assetData.Name, objectName, volume, false, callbackFunc);
    }

    public static SePlayer PlayVoice(string assetName, string objectName, float volume, System.Action callbackFunc)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        return instance?.PlaySeLocal(SePlayer.SeType.VOICE, assetName, objectName, volume, false, callbackFunc);
    }

    public bool ReleaseAudioSource(SePlayer player)
    {
        for (int i = 0; i < this.seMax; i++)
        {
            if (this.sePlayerStatusList[i] == player)
            {
                this.sePlayerStatusList[i] = null;
                this.seSourceList[i].Stop();
                return true;
            }
        }
        return false;
    }

    public static void ReleaseSe(string name)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        if (instance != null)
        {
            instance.ReleaseSeLocal(name);
        }
    }

    public void ReleaseSeLocal(string name)
    {
    }

    public static void Reset()
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        if (instance != null)
        {
            instance.ResetLocal();
        }
    }

    protected void ResetLocal()
    {
        this.StopSeAllLocal(0f);
    }

    public static void SetMasterVolume(float volume)
    {
        if (masterVolume != volume)
        {
            masterVolume = volume;
            SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
            if (instance != null)
            {
                instance.SetMuteLocal(isMute);
            }
        }
    }

    public static void SetMute(bool isMute)
    {
        if (SeManager.isMute != isMute)
        {
            SeManager.isMute = isMute;
            SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
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
            this.StopLocal(this.systemSeSource);
        }
        if (this.workSePlayerStatusList != null)
        {
            for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
            {
                this.workSePlayerStatusList[i].MuteSe(isMute);
            }
        }
    }

    public static void SetVoiceMasterVolume(float volume)
    {
        if (masterVoiceVolume != volume)
        {
            masterVoiceVolume = volume;
            SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
            if (instance != null)
            {
                instance.SetMuteLocal(isMute);
            }
        }
    }

    protected void StopLocal(CriAtomSource player)
    {
        if ((player != null) && (player.status == CriAtomSource.Status.Playing))
        {
            player.Stop();
        }
    }

    public static void StopSe(int num, float fadeoutTime = 0f)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        if (instance != null)
        {
            instance.StopSeLocal(num, fadeoutTime);
        }
    }

    public static void StopSe(string name, float fadeoutTime = 0f)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        if (instance != null)
        {
            instance.StopSeLocal(name, fadeoutTime);
        }
    }

    public static void StopSe(string assetName, string objectName, float fadeoutTime = 0f)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        if (instance != null)
        {
            instance.StopSeLocal(assetName, objectName, fadeoutTime);
        }
    }

    public static void StopSeAll(float fadeoutTime = 0f)
    {
        SeManager instance = SingletonMonoBehaviour<SeManager>.Instance;
        if (instance != null)
        {
            instance.StopSeAllLocal(fadeoutTime);
        }
    }

    protected void StopSeAllLocal(float fadeoutTime)
    {
        for (int i = this.workSePlayerStatusList.Count - 1; i >= 0; i--)
        {
            this.workSePlayerStatusList[i].StopSe(fadeoutTime);
        }
    }

    protected void StopSeLocal(int num, float fadeoutTime)
    {
        SePlayer sePlayer = this.GetSePlayer(num);
        if (sePlayer != null)
        {
            sePlayer.StopSe(fadeoutTime);
        }
    }

    protected void StopSeLocal(string name, float fadeoutTime)
    {
        SePlayer sePlayer = this.GetSePlayer(name);
        if (sePlayer != null)
        {
            sePlayer.StopSe(fadeoutTime);
        }
    }

    protected void StopSeLocal(string assetName, string objectName, float fadeoutTime)
    {
        SePlayer sePlayer = this.GetSePlayer(assetName, objectName);
        if (sePlayer != null)
        {
            sePlayer.StopSe(fadeoutTime);
        }
    }

    public static bool IsMute =>
        isMute;

    public static float MasterVoiceVolume =>
        masterVoiceVolume;

    public static float MasterVolume =>
        masterVolume;

    public enum SystemSeKind
    {
        DECIDE,
        CANCEL,
        WARNING,
        METER,
        GET_ITEM,
        LEVEL_UP,
        STATUS_OPEN,
        WINDOW_SLIDE,
        DECIDE2,
        UNIOPEN,
        UNICLOSE
    }
}

