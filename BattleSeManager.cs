using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class BattleSeManager : SingletonMonoBehaviour<BattleSeManager>
{
    private List<string> assetStorageList = new List<string>();
    protected EffectMaster effectMaster;
    private Dictionary<string, AssetLoader.LoadEndDataHandler> loadInfos = new Dictionary<string, AssetLoader.LoadEndDataHandler>();
    private List<BattleSePlayer> playingList = new List<BattleSePlayer>();
    private Dictionary<string, string> seToAssetTable = new Dictionary<string, string>();
    public float volume = 1f;

    public void DownloadSoundAsset(string cat, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        this.SetUp();
        this.loadInfos[cat] = callbackFunc;
        AssetManager.downloadAssetStorage("Audio/Battle" + cat + ".acb.bytes", new AssetLoader.LoadEndDataHandler(this.LoadEndAsset));
    }

    public void Initialize()
    {
        this.effectMaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EffectMaster>(DataNameKind.Kind.EFFECT);
    }

    protected void LoadEndAsset(AssetData data)
    {
        Debug.Log("L:LoadEndAsset:" + data.Name);
        this.assetStorageList.Add(data.Name);
        foreach (string str in data.GetObjectNameList())
        {
            this.seToAssetTable[str] = data.Name;
        }
        if (this.loadInfos.ContainsKey(data.Name))
        {
            AssetLoader.LoadEndDataHandler handler = this.loadInfos[data.Name];
            if (handler != null)
            {
                handler(data);
            }
        }
    }

    public void LoadSoundAsset(string cat, AssetLoader.LoadEndDataHandler callbackFunc)
    {
        this.SetUp();
        this.loadInfos[cat] = callbackFunc;
        AssetManager.downloadAssetStorage("Audio/Battle" + cat + ".acb.bytes", new AssetLoader.LoadEndDataHandler(this.LoadEndAsset));
    }

    public void OnFinished(BattleSePlayer player)
    {
    }

    protected BattleSePlayer playBattleSe(string assetName, string seName, float volume, System.Action callback, Action<BattleSePlayer> systemCallback)
    {
        BattleSePlayer player = this.SearchPlayingSe(seName);
        if (player != null)
        {
            Debug.Log("L:exists! Stop!" + player.SeName);
            player.Stop();
        }
        BattleSePlayer player2 = new BattleSePlayer(this, assetName, seName, volume, callback);
        player2.Play();
        return player2;
    }

    public void PlaySeByEffect(string effectName, System.Action callback)
    {
        Debug.Log("effectName: " + effectName);
    }

    private void playSeCallback(BattleSePlayer player)
    {
    }

    private BattleSePlayer SearchPlayingSe(string seName)
    {
        <SearchPlayingSe>c__AnonStorey7A storeya = new <SearchPlayingSe>c__AnonStorey7A {
            seName = seName
        };
        return this.playingList.Find(new Predicate<BattleSePlayer>(storeya.<>m__97));
    }

    public void SetUp()
    {
        if (this.effectMaster == null)
        {
            this.Initialize();
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    [CompilerGenerated]
    private sealed class <SearchPlayingSe>c__AnonStorey7A
    {
        internal string seName;

        internal bool <>m__97(BattleSeManager.BattleSePlayer x) => 
            (x.SeName == this.seName);
    }

    public class BattleSePlayer
    {
        private string assetName;
        private System.Action callback;
        private BattleSeManager manager;
        private SePlayer player;
        private string seName;
        private float volume;

        public BattleSePlayer(BattleSeManager manager, string assetName, string seName, float volume, System.Action callback)
        {
            this.manager = manager;
            this.assetName = assetName;
            this.seName = seName;
            this.volume = volume;
            this.callback = callback;
        }

        private void FinishCallback()
        {
            this.manager.OnFinished(this);
            if (this.callback != null)
            {
                this.callback();
            }
            this.player = null;
        }

        public void Play()
        {
            this.player = SoundManager.playVoice(this.assetName, this.seName, this.volume, new System.Action(this.FinishCallback));
        }

        public void Stop()
        {
            if (this.player != null)
            {
                this.player.StopSe(0f);
                this.manager.OnFinished(this);
                this.player = null;
            }
        }

        public string SeName =>
            this.seName;
    }

    protected class LocalAssetInfo
    {
        public AssetLoader.LoadEndDataHandler callback;
        public string name;
    }
}

