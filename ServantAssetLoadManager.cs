using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantAssetLoadManager : SingletonMonoBehaviour<ServantAssetLoadManager>
{
    private Dictionary<string, UnityEngine.Object> _releaseAssets = new Dictionary<string, UnityEngine.Object>();
    protected Dictionary<int, SePlayer> ActorVoice = new Dictionary<int, SePlayer>();
    private List<string> downloadlist = new List<string>();
    private readonly Vector3 InitPoint = new Vector3(0f, 0.05f, 0f);
    protected Dictionary<int, Voice.BATTLE> LastActorVoice = new Dictionary<int, Voice.BATTLE>();
    private List<string> loadlist = new List<string>();
    private AssetData nobleAssetData;
    private AssetData nobleSequenceData;
    private int soundCount;
    private ServantLimitAddMaster svtlimitaddmaster;
    private ServantLimitMaster svtlimitmaster;
    private ServantMaster svtmaster;

    public static void changeChocoDeadShader(GameObject obj)
    {
        Color white = Color.white;
        Color color = new Color(0f, 0f, 0f, 0f);
        foreach (SkinnedMeshRenderer renderer in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (renderer != null)
            {
                white = renderer.material.GetColor("_Color");
                color = renderer.material.GetColor("_AddColor");
                break;
            }
        }
        Shader shader = Shader.Find("Custom/SoftEdgeUnlitCutZ_Choco (SoftClip)");
        Texture2D texture = Resources.Load<Texture2D>("Shaders/ChocoMap");
        Transform transform = obj.transform.getNodeFromName("joint_all_Base", false);
        foreach (SkinnedMeshRenderer renderer2 in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (renderer2 != null)
            {
                foreach (Material material in renderer2.materials)
                {
                    material.shader = shader;
                    material.SetColor("_Color", white);
                    material.SetColor("_AddColor", color);
                    material.SetTexture("_ChocoTex", texture);
                    if (renderer2.gameObject.name.StartsWith("body"))
                    {
                        Vector3 vector2 = transform.InverseTransformPoint(renderer2.rootBone.position) - transform.localPosition;
                        Vector3 center = renderer2.localBounds.center + vector2;
                        Bounds bounds = new Bounds(center, renderer2.localBounds.size);
                        renderer2.localBounds = bounds;
                        renderer2.rootBone = transform;
                    }
                    else
                    {
                        material.SetFloat("_ClipSharpnessY", renderer2.localBounds.center.y - renderer2.localBounds.extents.y);
                    }
                }
            }
        }
    }

    public static void changeChocoSahder(GameObject obj)
    {
        Color white = Color.white;
        Color color = new Color(0f, 0f, 0f, 0f);
        foreach (SkinnedMeshRenderer renderer in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (renderer != null)
            {
                white = renderer.material.GetColor("_Color");
                color = renderer.material.GetColor("_AddColor");
                break;
            }
        }
        Shader shader = Shader.Find("Custom/SoftEdgeUnlitCutZ_Choco");
        Texture2D texture = Resources.Load<Texture2D>("Shaders/ChocoMap");
        foreach (SkinnedMeshRenderer renderer2 in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (renderer2 != null)
            {
                foreach (Material material in renderer2.materials)
                {
                    material.shader = shader;
                    material.SetColor("_Color", white);
                    material.SetColor("_AddColor", color);
                    material.SetTexture("_ChocoTex", texture);
                }
            }
        }
    }

    public static void changeDeadShader(GameObject obj)
    {
        Color white = Color.white;
        Color color = new Color(0f, 0f, 0f, 0f);
        foreach (SkinnedMeshRenderer renderer in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (renderer != null)
            {
                white = renderer.material.GetColor("_Color");
                color = renderer.material.GetColor("_AddColor");
                break;
            }
        }
        Shader shader = Shader.Find("Custom/SoftEdgeUnlitCutZ (SoftClip)");
        Transform transform = obj.transform.getNodeFromName("joint_all_Base", false);
        foreach (SkinnedMeshRenderer renderer2 in obj.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (renderer2 != null)
            {
                foreach (Material material in renderer2.materials)
                {
                    material.shader = shader;
                    material.SetColor("_Color", white);
                    material.SetColor("_AddColor", color);
                    if (renderer2.gameObject.name.StartsWith("body"))
                    {
                        Vector3 vector2 = transform.InverseTransformPoint(renderer2.rootBone.position) - transform.localPosition;
                        Vector3 center = renderer2.localBounds.center + vector2;
                        Bounds bounds = new Bounds(center, renderer2.localBounds.size);
                        renderer2.localBounds = bounds;
                        renderer2.rootBone = transform;
                    }
                    else
                    {
                        material.SetFloat("_ClipSharpnessY", renderer2.localBounds.center.y - renderer2.localBounds.extents.y);
                    }
                }
            }
        }
    }

    public static bool checkBattleVoice(int svtId, int limit, Voice.BATTLE type) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.checkBattleVoicelocal(svtId, limit, type);

    public bool checkBattleVoicelocal(int svtId, int limit, Voice.BATTLE type)
    {
        ServantEntity entity = this.getSvtEntity(svtId);
        int num = this.getVoicePrefix(svtId, limit);
        int num2 = this.getVoiceId(svtId, limit);
        string name = $"{num:D0}_{Voice.getFileName(type)}";
        if (entity == null)
        {
            Debug.LogError("svtent == null : " + svtId);
            return false;
        }
        return SoundManager.checkServantVoice("Servants_" + num2, name);
    }

    public static bool checkLoad() => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.checkLoadlocal();

    public bool checkLoadlocal() => 
        (((0 < this.loadlist.Count) || (0 < this.downloadlist.Count)) || (0 < this.soundCount));

    public void checkMaster()
    {
        if (this.svtmaster == null)
        {
            this.svtmaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT);
        }
        if (this.svtlimitmaster == null)
        {
            this.svtlimitmaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitMaster>(DataNameKind.Kind.SERVANT_LIMIT);
        }
        if (this.svtlimitaddmaster == null)
        {
            this.svtlimitaddmaster = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantLimitAddMaster>(DataNameKind.Kind.SERVANT_LIMIT_ADD);
        }
    }

    public static void clearServantlist()
    {
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.clearServantlistlocal();
    }

    public void clearServantlistlocal()
    {
        this.loadlist.Clear();
        this.downloadlist.Clear();
    }

    public GameObject createShadowEffect(int tp)
    {
        string key = "effect/ef_shadow0" + (1 + tp);
        GameObject original = null;
        if (this._releaseAssets.ContainsKey(key))
        {
            original = this._releaseAssets[key] as GameObject;
        }
        else
        {
            original = Resources.Load<GameObject>(key);
            this._releaseAssets[key] = original;
        }
        return UnityEngine.Object.Instantiate<GameObject>(original);
    }

    public void DebugPrint(AssetData data)
    {
        Debug.Log("DebugPrint>>");
        foreach (string str in data.GetObjectNameList())
        {
            Debug.Log("name:" + str);
        }
    }

    protected void endloadList(AssetData data)
    {
        if (data != null)
        {
            string name = data.Name;
            if (this.loadlist.Contains(name))
            {
                this.loadlist.Remove(name);
            }
            if (this.downloadlist.Contains(name))
            {
                this.downloadlist.Remove(name);
            }
        }
    }

    public void EndPlaySe()
    {
    }

    public static bool ExistsBattleVoice(int svtId, int limit, Voice.BATTLE type) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.ExistsBattleVoiceLocal(svtId, limit, type);

    public bool ExistsBattleVoiceLocal(int svtId, int limit, Voice.BATTLE type)
    {
        int num = this.getVoicePrefix(svtId, limit);
        int num2 = this.getVoiceId(svtId, limit);
        string name = $"{num:D0}_{Voice.getFileName(type)}";
        return SingletonMonoBehaviour<SoundManager>.Instance.IsExistsSound("Servants_" + num2, name);
    }

    public T getAssetObject<T>(string path, string file) where T: UnityEngine.Object
    {
        AssetData data = AssetManager.getAssetStorage(path);
        if (data != null)
        {
            return data.GetObject<T>(file);
        }
        Debug.Log("No asdata");
        return null;
    }

    public string getBattleChrId(int svtId, int limitCount)
    {
        this.checkMaster();
        return this.svtlimitaddmaster.getBattleChrId(svtId, limitCount);
    }

    public int getBattleChrLimitCount(int svtId, int limitCount)
    {
        this.checkMaster();
        return this.svtlimitaddmaster.getBattleChrLimitCount(svtId, limitCount);
    }

    public static Voice.BATTLE GetLastVoiceType(int uniqueId) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.GetLastVoiceTypeLocal(uniqueId);

    public Voice.BATTLE GetLastVoiceTypeLocal(int uniqueId)
    {
        if (this.LastActorVoice.ContainsKey(uniqueId))
        {
            return this.LastActorVoice[uniqueId];
        }
        return Voice.BATTLE.NONE;
    }

    public static int GetLimitImageIndex(int svtId, int limitCount) => 
        ImageLimitCount.GetImageLimitCount(svtId, limitCount);

    public ServantEntity getSvtEntity(int svtId)
    {
        this.checkMaster();
        return this.svtmaster.getEntityFromId<ServantEntity>(svtId);
    }

    public ServantLimitAddEntity getSvtLimitAddEntity(int svtId, int limitCnt)
    {
        this.checkMaster();
        long[] args = new long[] { (long) svtId, (long) limitCnt };
        if (this.svtlimitaddmaster.isEntityExistsFromId(args))
        {
            return this.svtlimitaddmaster.getEntityFromId<ServantLimitAddEntity>(svtId, limitCnt);
        }
        return null;
    }

    public ServantLimitEntity getSvtLimitEntity(int svtId, int limitCnt)
    {
        this.checkMaster();
        return this.svtlimitmaster.getEntityFromId<ServantLimitEntity>(svtId, limitCnt);
    }

    public int getVoiceId(int svtId, int limitCount)
    {
        this.checkMaster();
        return this.svtlimitaddmaster.getVoiceId(svtId, limitCount);
    }

    public int getVoicePrefix(int svtId, int limitCount)
    {
        this.checkMaster();
        return this.svtlimitaddmaster.getVoicePrefix(svtId, limitCount);
    }

    public static GameObject loadActorEffectFromActor(int svtId, int limitCount, int weapongroup, string name) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadActorEffectlocalFromActor(svtId, limitCount, weapongroup, name);

    private GameObject loadActorEffectlocalFromActor(int svtId, int limitCount, int weapongroup, string name)
    {
        string str = this.getBattleChrId(svtId, limitCount);
        return this.getAssetObject<GameObject>("Servants/" + str, name);
    }

    public static GameObject loadActorMotion(GameObject parent, int svtId, int weapongroup) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadActorMotionlocal(parent, svtId, weapongroup);

    private GameObject loadActorMotionlocal(GameObject parent, int svtId, int weapongroup)
    {
        Debug.Log("Servants/Motion/" + weapongroup);
        GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.getAssetObject<GameObject>("Servants/Motion/" + weapongroup, "motion"));
        obj3.transform.parent = parent.transform;
        obj3.transform.localPosition = Vector3.zero;
        obj3.transform.eulerAngles = Vector3.up;
        obj3.transform.localScale = Vector3.one;
        return obj3;
    }

    public static TextAsset loadAnimEvents(int svtId, int limitCount) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadAnimEventslocal(svtId, limitCount);

    public TextAsset loadAnimEventslocal(int svtId, int limitCount)
    {
        string str = this.getBattleChrId(svtId, limitCount);
        return this.getAssetObject<TextAsset>("Servants/" + str, "fbxevent");
    }

    public static GameObject loadBattleActor(GameObject parent, int svtId, int limitCount) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadBattleActorlocal(parent, svtId, limitCount);

    private GameObject loadBattleActorlocal(GameObject parent, int svtId, int limitCount)
    {
        Debug.Log("svtId:" + svtId);
        string str = this.getBattleChrId(svtId, limitCount);
        GameObject obj3 = UnityEngine.Object.Instantiate<GameObject>(this.getAssetObject<GameObject>("Servants/" + str, "chr"));
        obj3.transform.parent = parent.transform;
        obj3.transform.localPosition = this.InitPoint;
        obj3.transform.eulerAngles = Vector3.up;
        obj3.transform.localScale = Vector3.one;
        return obj3;
    }

    public static UITexture loadCommandCard(UITexture tex, int svtId, int limit, int commandLimit) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadCommandCardlocal(tex, svtId, limit, commandLimit);

    private UITexture loadCommandCardlocal(UITexture tex, int svtId, int limit, int commandLimit)
    {
        ServantEntity entity = this.getSvtEntity(svtId);
        Texture2D textured = this.getAssetObject<Texture2D>("Servants/" + this.getBattleChrId(svtId, limit), "card_servant_" + (GetLimitImageIndex(svtId, commandLimit) + 1));
        if (textured == null)
        {
            Debug.Log("no resoureces " + entity.name);
            return null;
        }
        if (tex != null)
        {
            tex.mainTexture = textured;
        }
        return tex;
    }

    public static GameObject loadCommonEffect(string name) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadCommonEffectlocal(name);

    public GameObject loadCommonEffectlocal(string name)
    {
        GameObject original = Resources.Load("effect/" + name) as GameObject;
        if (original != null)
        {
            return UnityEngine.Object.Instantiate<GameObject>(original);
        }
        return null;
    }

    public static GameObject loadEffect(string name, int weapongroup, int effectFolder) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadEffectlocal(name, weapongroup, effectFolder);

    public GameObject loadEffectlocal(string name, int weapongroup, int effectFolder)
    {
        GameObject obj2 = this.loadCommonEffectlocal(name);
        if (obj2 == null)
        {
            obj2 = this.loadWeaponGroupEffectlocal(name, weapongroup, effectFolder);
        }
        return obj2;
    }

    public static UIStandFigureR loadFigureObject(GameObject parent, int svtId, int limitCount, Face.Type faceType, System.Action callbackFunc) => 
        StandFigureManager.CreateRenderPrefab(parent, svtId, limitCount, faceType, 50, callbackFunc);

    public static UITexture loadNobleName(UITexture tex, int svtId, int limit, int treasureDvcId) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadNobleNamelocal(tex, svtId, limit, treasureDvcId);

    public UITexture loadNobleNamelocal(UITexture tex, int svtId, int limit, int treasureDvcId)
    {
        ServantEntity entity = this.getSvtEntity(svtId);
        string str = this.getBattleChrId(svtId, limit);
        ServantTreasureDvcEntity entity2 = ServantTreasureDvcMaster.getEntityFromIDID(svtId, treasureDvcId);
        Texture2D textured = null;
        if (entity2 != null)
        {
            string file = "card_servant_np";
            if (entity2.imageIndex < 2)
            {
                textured = this.getAssetObject<Texture2D>("Servants/" + str, file);
            }
            else
            {
                int num = entity2.imageIndex / 2;
                textured = this.getAssetObject<Texture2D>("Servants/" + str, file + $"{num}");
            }
        }
        if (textured == null)
        {
            Debug.Log("no resoureces " + entity.name);
            return tex;
        }
        if (tex != null)
        {
            tex.mainTexture = textured;
            if ((entity2.imageIndex % 2) == 0)
            {
                tex.uvRect = new Rect(0f, 0.5f, 1f, 0.5f);
                return tex;
            }
            tex.uvRect = new Rect(0f, 0f, 1f, 0.5f);
        }
        return tex;
    }

    public GameObject loadNoblePhantasm(GameObject parent, int svtId, int limitCount, int treasureDvcId, onGameObjectLoadComplete callback)
    {
        int seqId;
        <loadNoblePhantasm>c__AnonStorey61 storey = new <loadNoblePhantasm>c__AnonStorey61 {
            parent = parent,
            callback = callback,
            <>f__this = this
        };
        this.releaseNoblePhantasm();
        TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcMaster>(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(treasureDvcId);
        if (entity != null)
        {
            seqId = entity.seqId;
        }
        else
        {
            seqId = treasureDvcId;
        }
        storey.sequencePath = "NoblePhantasm/Sequence/" + seqId;
        storey.nobleDataPath = "NoblePhantasm/" + seqId;
        if (!AssetManager.isExistAssetStorage(storey.sequencePath))
        {
            Debug.LogWarning(string.Concat(new object[] { "loadNoblePhantasm : Old Style TreasureDeviceId : ", svtId, ":", seqId }));
            storey.sequencePath = "NoblePhantasm/Sequence/" + svtId;
        }
        if (!AssetManager.isExistAssetStorage(storey.nobleDataPath))
        {
            storey.nobleDataPath = "NoblePhantasm/" + svtId;
        }
        AssetManager.loadAssetStorage(storey.sequencePath, new AssetLoader.LoadEndDataHandler(storey.<>m__37));
        return null;
    }

    public static GameObject LoadNoblePhantasm(GameObject parent, int svtId, int limitCount, int treasureDvcId, onGameObjectLoadComplete callback) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadNoblePhantasm(parent, svtId, limitCount, treasureDvcId, callback);

    public static GameObject loadNoblePhantasmEffect(int treasureDeviceId, string name) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadNoblePhantasmEffectlocal(treasureDeviceId, name);

    private GameObject loadNoblePhantasmEffectlocal(int treasureDeviceId, string name)
    {
        int seqId;
        TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcMaster>(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(treasureDeviceId);
        if (entity != null)
        {
            seqId = entity.seqId;
        }
        else
        {
            seqId = treasureDeviceId;
        }
        string str = "NoblePhantasm/" + seqId;
        if (!AssetManager.isExistAssetStorage(str))
        {
            Debug.LogWarning("loadNoblePhantasmEffect : Old Style TresureDeviceId  : " + seqId);
            str = "NoblePhantasm/" + ((seqId / 10) * 10);
        }
        return this.getAssetObject<GameObject>(str, name);
    }

    public static UITexture loadStatusFace(UITexture tex, int svtID, int limit) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadStatusFacelocal(tex, svtID, limit);

    public UITexture loadStatusFacelocal(UITexture tex, int svtID, int limit)
    {
        ServantEntity entity = this.getSvtEntity(svtID);
        if (entity == null)
        {
            Debug.LogError("loadStatusFacelocal:Servant not founud:" + svtID);
        }
        Texture2D textured = this.getAssetObject<Texture2D>("Servants/" + this.getBattleChrId(svtID, limit), "status_servant_" + (GetLimitImageIndex(svtID, limit) + 1));
        if (textured == null)
        {
            Debug.Log("no resoureces " + entity.name);
            return null;
        }
        if (tex != null)
        {
            tex.mainTexture = textured;
        }
        return tex;
    }

    public static GameObject loadWeaponGroupEffect(string name, int weapongroup, int effectFolder) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.loadWeaponGroupEffectlocal(name, weapongroup, effectFolder);

    public GameObject loadWeaponGroupEffectlocal(string name, int weapongroup, int effectFolder)
    {
        string str = "Effect/weapon/" + weapongroup;
        if (effectFolder != 0)
        {
            str = str + "/" + effectFolder;
        }
        AssetData data = AssetManager.getAssetStorage(str);
        GameObject obj2 = null;
        if (data != null)
        {
            obj2 = data.GetObject<GameObject>(name);
        }
        if (obj2 != null)
        {
            return obj2;
        }
        return null;
    }

    public void localStopVoice(int uniqueId)
    {
        if (this.ActorVoice.ContainsKey(uniqueId))
        {
            this.ActorVoice[uniqueId].StopSe(0f);
        }
    }

    public static SePlayer playBattleVoice(int svtId, int limit, Voice.BATTLE type, float volume, System.Action callback = null, int uniqueId = -1) => 
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.playBattleVoicelocal(svtId, limit, type, volume, callback, uniqueId);

    public SePlayer playBattleVoicelocal(int svtId, int limit, Voice.BATTLE type, float volume, System.Action callback = null, int uniqueId = -1)
    {
        <playBattleVoicelocal>c__AnonStorey60 storey = new <playBattleVoicelocal>c__AnonStorey60 {
            callback = callback,
            <>f__this = this
        };
        ServantEntity entity = this.getSvtEntity(svtId);
        int num = this.getVoiceId(svtId, limit);
        int num2 = this.getVoicePrefix(svtId, limit);
        string objectName = $"{num2:D0}_{Voice.getFileName(type)}";
        if (entity == null)
        {
            if (storey.callback != null)
            {
                storey.callback();
            }
            return null;
        }
        if (((uniqueId != -1) && this.ActorVoice.ContainsKey(uniqueId)) && (this.ActorVoice[uniqueId] != null))
        {
            this.ActorVoice[uniqueId].StopSe(0f);
        }
        this.LastActorVoice[uniqueId] = type;
        this.ActorVoice[uniqueId] = SoundManager.playVoice("Servants/" + num, objectName, volume, new System.Action(storey.<>m__36));
        return this.ActorVoice[uniqueId];
    }

    public static void preloadActorMotion(int weapongroup)
    {
        Debug.Log("preloadActorMotion:" + weapongroup);
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.setLoadList("Servants/Motion/" + weapongroup);
    }

    public static void preloadServant(int svtId, int limitCount)
    {
        Debug.Log(string.Concat(new object[] { "preloadServant:", svtId, ":", limitCount }));
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.preloadServantlocal(svtId, limitCount);
    }

    public void preloadServantlocal(int svtId, int limitCount)
    {
        int num = this.getVoiceId(svtId, limitCount);
        this.setLoadList("Servants/" + this.getBattleChrId(svtId, limitCount));
        if (num != 0)
        {
            this.soundCount++;
            SingletonMonoBehaviour<SoundManager>.Instance.LoadAudioAssetStorage("Servants_" + num, (System.Action) (() => this.soundCount--), SoundManager.CueType.ALL);
        }
    }

    public static void preloadWeaponEffect(int weapongroup, int effectFolder)
    {
        Debug.Log("preloadWeaponEffect:" + weapongroup);
        if (effectFolder == 0)
        {
            SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.setLoadList("Effect/weapon/" + weapongroup);
        }
        else
        {
            object[] objArray1 = new object[] { "Effect/weapon/", weapongroup, "/", effectFolder };
            SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.setLoadList(string.Concat(objArray1));
        }
    }

    public void releaseNoblePhantasm()
    {
        if (this.nobleAssetData != null)
        {
            this.nobleAssetData.RemoveEntry();
            this.nobleAssetData = null;
        }
        if (this.nobleSequenceData != null)
        {
            this.nobleSequenceData.RemoveEntry();
            this.nobleSequenceData = null;
        }
    }

    public void setDownloadList(string path)
    {
        this.downloadlist.Add(path);
        if (!AssetManager.downloadAssetStorage(path, new AssetLoader.LoadEndDataHandler(this.endloadList)))
        {
            throw new FileNotFoundException("not found file " + path);
        }
    }

    public void setLoadList(string path)
    {
        this.loadlist.Add(path);
        if (!AssetManager.loadAssetStorage(path, new AssetLoader.LoadEndDataHandler(this.endloadList)))
        {
            throw new FileNotFoundException("not found file " + path);
        }
    }

    public static void StopVoice(int uniqueId)
    {
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.localStopVoice(uniqueId);
    }

    public void UnloadAllAsset()
    {
        foreach (KeyValuePair<string, UnityEngine.Object> pair in this._releaseAssets)
        {
            Resources.UnloadAsset(pair.Value);
        }
    }

    public static void unloadServant(int svtId, int limitCount)
    {
        Debug.Log(string.Concat(new object[] { "unloadServant:", svtId, ":", limitCount }));
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.unloadServantlocal(svtId, limitCount);
    }

    public void unloadServantlocal(int svtId, int limitCount)
    {
        AssetManager.releaseAsset(AssetManager.getAssetStorage("Servants/" + this.getBattleChrId(svtId, limitCount)));
        int num = this.getVoiceId(svtId, limitCount);
        if (num != 0)
        {
            SingletonMonoBehaviour<SoundManager>.Instance.ReleaseAudioAssetStorage("Servants_" + num);
        }
    }

    public static void unloadWeaponGroupEffect(int weapongroup, int effectFolder)
    {
        Debug.Log("unloadWeaponGroupEffect:" + weapongroup);
        SingletonMonoBehaviour<ServantAssetLoadManager>.Instance.unloadWeaponGroupEffectlocal(weapongroup, effectFolder);
    }

    public void unloadWeaponGroupEffectlocal(int weapongroup, int effectFolder)
    {
        string name = "Effect/weapon/" + weapongroup;
        if (effectFolder != 0)
        {
            name = name + "/" + effectFolder;
        }
        AssetManager.releaseAsset(AssetManager.getAssetStorage(name));
    }

    [CompilerGenerated]
    private sealed class <loadNoblePhantasm>c__AnonStorey61
    {
        internal ServantAssetLoadManager <>f__this;
        internal ServantAssetLoadManager.onGameObjectLoadComplete callback;
        internal string nobleDataPath;
        internal GameObject parent;
        internal string sequencePath;

        internal void <>m__37(AssetData data)
        {
            this.<>f__this.nobleSequenceData = data;
            AssetManager.loadAssetStorage(this.nobleDataPath, delegate (AssetData assetData) {
                this.<>f__this.nobleAssetData = assetData;
                GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.<>f__this.getAssetObject<GameObject>(this.sequencePath, "ChrSequence1"));
                obj2.SetActive(false);
                obj2.transform.parent = this.parent.transform;
                obj2.transform.localPosition = Vector3.zero;
                obj2.transform.eulerAngles = Vector3.up;
                obj2.transform.localScale = Vector3.one;
                this.callback(obj2);
            });
        }

        internal void <>m__38(AssetData assetData)
        {
            this.<>f__this.nobleAssetData = assetData;
            GameObject obj2 = UnityEngine.Object.Instantiate<GameObject>(this.<>f__this.getAssetObject<GameObject>(this.sequencePath, "ChrSequence1"));
            obj2.SetActive(false);
            obj2.transform.parent = this.parent.transform;
            obj2.transform.localPosition = Vector3.zero;
            obj2.transform.eulerAngles = Vector3.up;
            obj2.transform.localScale = Vector3.one;
            this.callback(obj2);
        }
    }

    [CompilerGenerated]
    private sealed class <playBattleVoicelocal>c__AnonStorey60
    {
        internal ServantAssetLoadManager <>f__this;
        internal System.Action callback;

        internal void <>m__36()
        {
            this.<>f__this.EndPlaySe();
            if (this.callback != null)
            {
                this.callback();
            }
        }
    }

    public delegate void onGameObjectLoadComplete(GameObject obj);
}

