using System;
using System.Runtime.InteropServices;

public class SePlayer
{
    protected System.Action callbackFunc;
    protected string cueSheetName;
    protected AssetData data;
    protected float fadeoutTime;
    protected bool isLoading;
    protected bool isLoop;
    protected bool isStop;
    protected bool isWaitSource;
    protected string objName;
    protected int playNum;
    protected float playTime;
    protected SeType seType;
    protected CriAtomSource source;
    protected float volume = 1f;

    public SePlayer(int num, SeType type, AssetData.Type assetType, string pathName, string name, float volume, bool isLoop, System.Action callbackFunc)
    {
        this.playNum = num;
        this.seType = type;
        this.objName = name;
        this.callbackFunc = (System.Action) Delegate.Combine(this.callbackFunc, callbackFunc);
        this.volume = volume;
        this.isLoop = isLoop;
        this.isLoading = false;
        this.isStop = false;
        this.isWaitSource = true;
        string str = pathName.Replace('/', '_');
        this.cueSheetName = str;
    }

    public void Callback()
    {
        System.Action callbackFunc = this.callbackFunc;
        this.Destroy();
        if (callbackFunc != null)
        {
            callbackFunc();
        }
    }

    public void Destroy()
    {
        this.callbackFunc = null;
        if (this.source != null)
        {
            this.source.Stop();
            this.source = null;
        }
        this.data = null;
        this.isStop = true;
        this.isLoop = false;
        this.isLoading = false;
        this.isWaitSource = false;
    }

    public float GetBaseVolume()
    {
        if (SeManager.IsMute)
        {
            return 0f;
        }
        if (this.seType == SeType.VOICE)
        {
            return (this.volume * SeManager.MasterVoiceVolume);
        }
        return (this.volume * SeManager.MasterVolume);
    }

    public void MuteSe(bool isMute)
    {
        if (this.source != null)
        {
            this.source.volume = this.GetBaseVolume();
        }
    }

    public void RemoveCallback(System.Action callbackFunc)
    {
        if (callbackFunc != null)
        {
            this.callbackFunc = (System.Action) Delegate.Remove(this.callbackFunc, callbackFunc);
        }
    }

    public void StopSe(float fadeoutTime = 0f)
    {
        if (!this.isStop)
        {
            if (((fadeoutTime > 0f) && (this.fadeoutTime <= 0f)) && !SeManager.IsMute)
            {
                this.fadeoutTime = fadeoutTime;
                this.playTime = -1f;
            }
            else
            {
                this.isStop = true;
                if (this.source != null)
                {
                    this.isLoop = false;
                    this.fadeoutTime = 0f;
                    this.source.Stop();
                    this.source = null;
                    SingletonMonoBehaviour<SeManager>.Instance.ReleaseAudioSource(this);
                }
            }
        }
    }

    public bool Update(float delta)
    {
        if (this.isLoading)
        {
            return true;
        }
        if (this.source == null)
        {
            if (this.isStop)
            {
                return false;
            }
            if (!this.isWaitSource)
            {
                return false;
            }
            if ((this.cueSheetName == null) || (this.objName == null))
            {
                return false;
            }
            this.source = SingletonMonoBehaviour<SeManager>.Instance.GetAudioSource(this, this.cueSheetName, this.objName);
            if (this.source != null)
            {
                this.isWaitSource = false;
                this.source.cueSheet = this.cueSheetName;
                this.source.cueName = this.objName;
                this.source.volume = this.GetBaseVolume();
                if (SingletonMonoBehaviour<SoundManager>.Instance.IsExistsSound(this.cueSheetName, this.objName))
                {
                    this.source.Play();
                }
                else
                {
                    Debug.LogWarning(string.Concat(new object[] { "SE Play (not found) : [", this.cueSheetName, "] : [", this.objName, "] vol:", this.source.volume }));
                }
            }
            return true;
        }
        if (this.fadeoutTime > 0f)
        {
            if (this.playTime >= 0f)
            {
                this.playTime += delta;
                if ((this.playTime >= this.fadeoutTime) || SeManager.IsMute)
                {
                    this.isLoop = false;
                    this.fadeoutTime = 0f;
                    this.source.Stop();
                    this.source = null;
                    SingletonMonoBehaviour<SeManager>.Instance.ReleaseAudioSource(this);
                    return false;
                }
                this.source.volume = this.GetBaseVolume() * (1f - (this.playTime / this.fadeoutTime));
            }
            else
            {
                this.playTime = 0f;
            }
        }
        if (this.source.status == CriAtomSource.Status.Prep)
        {
            return true;
        }
        if (this.source.status == CriAtomSource.Status.Playing)
        {
            return true;
        }
        if (this.isLoop)
        {
            this.source.Play();
            return true;
        }
        this.source = null;
        SingletonMonoBehaviour<SeManager>.Instance.ReleaseAudioSource(this);
        return false;
    }

    public string AssetName =>
        ((this.cueSheetName == null) ? null : this.cueSheetName);

    public string DataName =>
        this.objName;

    public bool IsBusy =>
        (this.isLoading || (this.isWaitSource || (this.isLoop || ((this.source?.status == CriAtomSource.Status.Prep) || (this.source?.status == CriAtomSource.Status.Playing)))));

    public bool IsLoop =>
        ((this.source != null) && this.source.loop);

    public int PlayNum =>
        this.playNum;

    public SeType Type =>
        this.seType;

    public enum SeType
    {
        NORMAL,
        VOICE
    }
}

