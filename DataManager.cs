using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

public class DataManager : SingletonMonoBehaviour<DataManager>
{
    [SerializeField]
    protected bool _DispLog = true;
    public static int actionIndex = 0;
    protected BattleActionData[] battleActionData;
    public static string channelId = string.Empty;
    protected DataMasterBase[] datalist;
    protected static int dataVersion = 0;
    protected static long dateVersion = 0L;
    public static int isBattleLive = 0;
    public static bool isLogin = false;
    private float lastFrameTime;
    protected Dictionary<string, DataMasterBase> lookup;
    public static List<int> mstSkillIds = new List<int>();
    public static List<int> questIds = new List<int>();
    protected static ReadMasterDataResult readMasterDataResult;
    protected List<uint> saveCrcList = new List<uint>();
    protected List<string> saveDataList = new List<string>();
    protected List<string> saveNameList = new List<string>();
    public int serverId = 0x3e9;
    public static List<int> svtIds = new List<int>();
    protected static object updateData = null;
    protected static UpdateMasterDataResult updateMasterDataResult;
    private const float waitFrameTime = 0.1f;

    public void checkMasterData()
    {
        Debug.Log("DataManager:checkMasterData");
        int length = this.datalist.Length;
        for (int i = 0; i < length; i++)
        {
            DataMasterBase base2 = this.datalist[i];
            string str = base2.getCacheName();
            DataMasterBase base3 = this.lookup[str];
            DataEntityBase[] baseArray = base2.getEntitys();
            DataEntityBase[] baseArray2 = base3.getEntitys();
            if ((baseArray != null) && (baseArray2 != null))
            {
                Debug.Log(string.Concat(new object[] { "    [", str, "][", base3.getCacheName(), "] ", baseArray.Length, ",", baseArray2.Length }));
            }
        }
    }

    private bool CheckWaitforFrame()
    {
        if ((Time.realtimeSinceStartup - this.lastFrameTime) > 0.1f)
        {
            this.lastFrameTime = Time.realtimeSinceStartup;
            return true;
        }
        return false;
    }

    public static void ClearCacheAll()
    {
        string path = getCachePath();
        Debug.Log("Delete Directory [" + path + "]");
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        dataVersion = 0;
        dateVersion = 0L;
    }

    protected void ClearSaveDataList()
    {
        dataVersion = 0;
        dateVersion = 0L;
        this.saveNameList.Clear();
        this.saveCrcList.Clear();
        this.saveDataList.Clear();
    }

    protected void DeleteCacheFile()
    {
        if (this._DispLog)
        {
            Debug.Log("DataManager : deleteMasterDataCacheFile");
        }
        if (!ManagerConfig.UseMock)
        {
            string path = getCacheListFileName();
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            string str2 = getCacheFileName();
            if (File.Exists(str2))
            {
                File.Delete(str2);
            }
        }
    }

    public BattleActionData[] GetbattleActionData() => 
        this.battleActionData;

    protected static string getCacheFileName() => 
        (getCachePath() + "/masterData.dat");

    protected static string getCacheListFileName() => 
        (getCachePath() + "/masterDataList.dat");

    public static string getCachePath() => 
        (Application.persistentDataPath + "/MasterDataCaches");

    public static int getDataVersion() => 
        dataVersion;

    public static long getDateVersion() => 
        dateVersion;

    public DataEntityBase[] getEntitys(DataNameKind.Kind kind)
    {
        DataMasterBase base2 = this.lookup[DataNameKind.GetName(kind)];
        return base2.getEntitys();
    }

    public T[] getEntitys<T>(DataNameKind.Kind kind) where T: DataEntityBase
    {
        DataMasterBase base2 = this.lookup[DataNameKind.GetName(kind)];
        return base2.getEntitys<T>();
    }

    public DataMasterBase getMasterData(DataNameKind.Kind kind) => 
        this.lookup[DataNameKind.GetName(kind)];

    public T getMasterData<T>(DataNameKind.Kind kind) where T: DataMasterBase => 
        (this.lookup[DataNameKind.GetName(kind)] as T);

    public int getMasterDataVersion() => 
        dataVersion;

    public long getMasterDateVersion() => 
        dateVersion;

    protected uint getMdk(string name, int version)
    {
        uint num = Crc32.Compute(Encoding.UTF8.GetBytes(name));
        uint num2 = (uint) (((version * 3) + 1) % 0x100);
        uint num3 = num & 0xff;
        uint num4 = (num >> 8) & 0xff;
        uint num5 = (num >> 0x10) & 0xff;
        uint num6 = (num >> 0x18) & 0xff;
        switch ((version % 4))
        {
            case 1:
                num3 -= num2;
                num4 *= num2 % 0x10;
                num5 += num2;
                num6 ^= num2;
                break;

            case 2:
                num3 ^= num2;
                num4 += num2;
                num5 *= num2 % 0x10;
                num6 -= num2;
                break;

            case 3:
                num3 *= num2 % 0x10;
                num4 -= num2;
                num5 ^= num2;
                num6 += num2;
                break;

            default:
                num3 += num2;
                num4 ^= num2;
                num5 -= num2;
                num6 *= num2 % 0x10;
                break;
        }
        return (uint) (((((num6 & 0xff) << 0x18) | ((num5 & 0xff) << 0x10)) | ((num4 & 0xff) << 8)) | ((num3 & 0xff) << 8));
    }

    public ReadMasterDataResult getReadMasterDataResult() => 
        readMasterDataResult;

    public T getSingleEntity<T>(DataNameKind.Kind kind) where T: DataEntityBase
    {
        DataMasterBase base2 = this.lookup[DataNameKind.GetName(kind)];
        return base2.getSingleEntity<T>();
    }

    public UpdateMasterDataResult getUpdateMasterDataResult() => 
        updateMasterDataResult;

    public T getUserIdEntity<T>(DataNameKind.Kind kind) where T: DataEntityBase
    {
        DataMasterBase base2 = this.lookup[DataNameKind.GetName(kind)];
        return base2.getUserIdEntity<T>();
    }

    public void Initialize()
    {
        if (this.datalist != null)
        {
            int length = this.datalist.Length;
            for (int i = 0; i < length; i++)
            {
                this.datalist[i].Clear();
            }
        }
        else
        {
            this.datalist = new List<DataMasterBase> { 
                new UserMaster(),
                new UserGameMaster(),
                new TblUserMaster(),
                new UserItemMaster(),
                new UserServantMaster(),
                new UserAccessaryMaster(),
                new UserQuestMaster(),
                new BattleMaster(),
                new OtherUserGameMaster(),
                new TblFriendMaser(),
                new AreaMaster(),
                new ServantClassMaster(),
                new ServantCardMaster(),
                new EventMaster(),
                new ItemMaster(),
                new QuestMaster(),
                new QuestReleaseMaster(),
                new QuestPhaseMaster(),
                new QuestGroupMaster(),
                new SpotMaster(),
                new SpotRoadMaster(),
                new MapGimmickMaster(),
                new GiftMaster(),
                new DropMaster(),
                new ShopMaster(),
                new StoneShopMaster(),
                new BankShopMaster(),
                new ShopScriptMaster(),
                new StageMaster(),
                new ServantMaster(),
                new ServantGroupMaster(),
                new ServantLimitMaster(),
                new ServantLimitAddMaster(),
                new ServantSkillMaster(),
                new BgmMaster(),
                new WarMaster(),
                new ServantScriptMaster(),
                new NewsMaster(),
                new TelopMaster(),
                new UserExpMaster(),
                new TreasureDvcMaster(),
                new ServantTreasureDvcMaster(),
                new SkillMaster(),
                new SkillLvMaster(),
                new SkillDetailMaster(),
                new CommandSpellMaster(),
                new EquipMaster(),
                new EquipExpMaster(),
                new EquipSkillMaster(),
                new SubEquipMaster(),
                new AccessaryMaster(),
                new UserPresentBoxMaster(),
                new UserDeckMaster(),
                new UserSubEquipMaster(),
                new GachaMaster(),
                new GachaImageMaster(),
                new UserGachaMaster(),
                new UserEquipMaster(),
                new UserServantCollectionMaster(),
                new FriendshipMaster(),
                new GachaTicketMaster(),
                new UserFormationMaster(),
                new FunctionMaster(),
                new BuffMaster(),
                new GachaReleaseMaster(),
                new CombineQpMaster(),
                new CombineMaterialMaster(),
                new EventCombineMaster(),
                new ServantExpMaster(),
                new ServantCommentMaster(),
                new CombineSkillMaster(),
                new CombineTdMaster(),
                new EventQuestMaster(),
                new EventCampaignMaster(),
                new IllustratorMaster(),
                new CvMaster(),
                new TreasureDvcLvMaster(),
                new TreasureDvcDetailMaster(),
                new UserFollowerMaster(),
                new NpcDeckMaster(),
                new NpcFollowerMaster(),
                new NpcServantMaster(),
                new NpcServantFollowerMaster(),
                new UserEventMaster(),
                new UserShopMaster(),
                new UserContinueMaster(),
                new ConstantMaster(),
                new ConstantStrMaster(),
                new AiMaster(),
                new AiActMaster(),
                new AttriRelationMaster(),
                new ClassRelationMaster(),
                new EffectMaster(),
                new EquipImageMaster(),
                new ServantVoiceMaster(),
                new CombineLimitMaster(),
                new CardMaster(),
                new CombineQpSvtEquipMaster(),
                new ServantRarityMaster(),
                new SetItemMaster(),
                new ApRecoverMaster(),
                new BannerMaster(),
                new ShopReleaseMaster(),
                new EventRewardMaster(),
                new EventDetailMaster(),
                new EventServantMaster(),
                new BoxGachaMaster(),
                new BoxGachaBaseMaster(),
                new BoxGachaExtraMaster(),
                new BoxGachaTalkMaster(),
                new UserBoxGachaMaster(),
                new BoxGachaHistoryMaster(),
                new BattleBgMaster(),
                new TipsMaster(),
                new UserLoginMaster(),
                new VoiceMaster(),
                new EventRewardExtraMaster(),
                new EventMissionMaster(),
                new EventMissionActionMaster(),
                new EventMissionConditionMaster(),
                new EventMissionCondDetailMaster(),
                new EventRewardSetMaster(),
                new UserEventMissionMaster(),
                new UserEventMissionCondDetailMaster(),
                new BoxGachaBaseDetailMaster(),
                new UserServantLearderMaster(),
                new ClosedMessageMaster(),
                new FunctionGroupMaster(),
                new UserTermMaster(),
                new GameIllustrationMaster()
            }.ToArray();
            this.lookup = new Dictionary<string, DataMasterBase>();
            int num3 = this.datalist.Length;
            for (int j = 0; j < num3; j++)
            {
                this.lookup.Add(this.datalist[j].getCacheName(), this.datalist[j]);
            }
        }
    }

    public bool IsBattleLiveOpen(BattleData data)
    {
        <IsBattleLiveOpen>c__AnonStorey68 storey = new <IsBattleLiveOpen>c__AnonStorey68 {
            data = data
        };
        if (isBattleLive == 0)
        {
            return false;
        }
        bool flag = !svtIds.Exists(new Predicate<int>(storey.<>m__4F));
        bool flag2 = !svtIds.Exists(new Predicate<int>(storey.<>m__50));
        bool flag3 = !svtIds.Exists(new Predicate<int>(storey.<>m__51));
        bool flag4 = !questIds.Contains(storey.data.Questid);
        bool flag5 = !mstSkillIds.Exists(new Predicate<int>(storey.<>m__52));
        return (((flag2 && flag3) && (flag4 && flag5)) && flag);
    }

    [DebuggerHidden]
    public IEnumerator readMasterData() => 
        new <readMasterData>c__Iterator11 { <>f__this = this };

    public bool readMasterDataListFile()
    {
        this.saveNameList.Clear();
        this.saveCrcList.Clear();
        this.saveDataList.Clear();
        if (!ManagerConfig.UseMock)
        {
            string path = getCacheListFileName();
            if (File.Exists(path))
            {
                if (this._DispLog)
                {
                    Debug.Log("DataManager load list [" + path + "]");
                }
                string s = null;
                string[] strArray = null;
                string message = null;
                int num = 3;
                while (num-- > 0)
                {
                    try
                    {
                        message = null;
                        uint? crc = null;
                        s = MdcStr.Dc(File.ReadAllText(path), 0, crc);
                        break;
                    }
                    catch (Exception exception)
                    {
                        Debug.LogError("error " + exception.Message);
                        s = null;
                        message = exception.Message;
                        continue;
                    }
                }
                if (s != null)
                {
                    try
                    {
                        char[] trimChars = new char[] { 0xfeff };
                        s = s.Trim(trimChars);
                        char[] anyOf = new char[] { '\r', '\n' };
                        int length = s.IndexOfAny(anyOf);
                        if (length > 1)
                        {
                            string str4 = s.Substring(0, length);
                            if (str4.StartsWith("~"))
                            {
                                str4 = str4.Substring(1);
                                s = s.Substring(length + 1);
                                uint num3 = Crc32.Compute(Encoding.UTF8.GetBytes(s));
                                if (this._DispLog)
                                {
                                    Debug.Log(string.Concat(new object[] { "CRC (cache) :[", s.Length, "] ", str4, " ", num3 }));
                                }
                                if (uint.Parse(str4) == num3)
                                {
                                    char[] separator = new char[] { '\r', '\n' };
                                    strArray = s.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                                }
                                else
                                {
                                    message = "DataManager boot load crc error : チェックサム値が不一致";
                                }
                            }
                            else
                            {
                                message = "DataManager boot load error : 読み込んだファイルの先頭がチェックサムデータではなかった";
                            }
                        }
                        else
                        {
                            message = "DataManager boot load error : ファイル先頭の１行目の内容が空";
                        }
                    }
                    catch (Exception exception2)
                    {
                        message = "DataManager boot load error : " + exception2.Message;
                    }
                }
                if (strArray != null)
                {
                    if ((strArray.Length > 0) && strArray[0].StartsWith("@"))
                    {
                        char[] chArray4 = new char[] { ',' };
                        string[] strArray2 = strArray[0].Split(chArray4);
                        string str5 = strArray2[0].Substring(1);
                        if (ManagerConfig.MasterDataCacheVer != str5)
                        {
                            string[] textArray1 = new string[] { "master data versiton different (", str5, ") -> (", ManagerConfig.MasterDataCacheVer, ")" };
                            message = string.Concat(textArray1);
                        }
                        else if (strArray2.Length != 3)
                        {
                            message = "DataManager boot load error : list file parameter error";
                        }
                        else
                        {
                            dataVersion = int.Parse(strArray2[1]);
                            dateVersion = long.Parse(strArray2[2]);
                            int num4 = strArray.Length;
                            for (int i = 1; i < num4; i++)
                            {
                                char[] chArray5 = new char[] { ',' };
                                strArray2 = strArray[i].Split(chArray5);
                                if (strArray2.Length != 2)
                                {
                                    break;
                                }
                                string item = strArray2[0];
                                uint num6 = uint.Parse(strArray2[1].Trim());
                                this.saveNameList.Add(item);
                                this.saveCrcList.Add(num6);
                            }
                        }
                    }
                    else
                    {
                        message = "DataManager boot load error : list file break";
                    }
                }
                if (message != null)
                {
                    Debug.LogError(message);
                }
                else
                {
                    return true;
                }
            }
            this.ClearSaveDataList();
        }
        return false;
    }

    public void SetbattleActionData(BattleActionData[] data)
    {
        this.battleActionData = null;
        this.battleActionData = data;
    }

    public void setMasterData(int dataVer, long dateVer)
    {
        dataVersion = dataVer;
        dateVersion = dateVer;
    }

    public void setMasterData(int dataVer, long dateVer, object obj)
    {
        Debug.Log(string.Concat(new object[] { "DataManager : setMasterData ver ", dataVersion, "[", dateVersion, "] -> ", dataVer, "[", dateVer, "]" }));
        if ((dataVersion != dataVer) || (dateVersion != dateVer))
        {
            dataVersion = dataVer;
            dateVersion = dateVer;
            updateData = obj;
        }
        else
        {
            updateData = null;
        }
    }

    public void setMasterDataVersion(int dataVer, long dateVer)
    {
        dataVersion = dataVer;
        dateVersion = dateVer;
    }

    public void updateJsonData(Dictionary<string, object> data)
    {
        Dictionary<string, object> dictionary = null;
        if (data.ContainsKey("deleted"))
        {
            dictionary = (Dictionary<string, object>) data["deleted"];
            int length = this.datalist.Length;
            for (int i = 0; i < length; i++)
            {
                string key = this.datalist[i].getCacheName();
                if (dictionary.ContainsKey(key))
                {
                    Debug.Log("CacheData updated [" + key + "]");
                    this.datalist[i].Deleted(dictionary[key]);
                }
            }
        }
        if (data.ContainsKey("updated"))
        {
            dictionary = (Dictionary<string, object>) data["updated"];
            int num3 = this.datalist.Length;
            for (int j = 0; j < num3; j++)
            {
                string str2 = this.datalist[j].getCacheName();
                if (dictionary.ContainsKey(str2))
                {
                    Debug.Log("CacheData updated [" + str2 + "]");
                    this.datalist[j].Updated(dictionary[str2]);
                }
            }
        }
        if (data.ContainsKey("replaced"))
        {
            dictionary = (Dictionary<string, object>) data["replaced"];
            int num5 = this.datalist.Length;
            for (int k = 0; k < num5; k++)
            {
                string str3 = this.datalist[k].getCacheName();
                if (dictionary.ContainsKey(str3))
                {
                    Debug.Log("CacheData replaced [" + str3 + "]");
                    this.datalist[k].Replaced(dictionary[str3]);
                }
            }
        }
    }

    public void updateJsonData(object obj)
    {
        Dictionary<string, object> data = (Dictionary<string, object>) obj;
        this.updateJsonData(data);
    }

    [DebuggerHidden]
    public IEnumerator updateMasterData() => 
        new <updateMasterData>c__Iterator12 { <>f__this = this };

    protected void writeMasterDataListFile()
    {
        if (this._DispLog)
        {
            Debug.Log("DataManager : writeMasterDataListFile");
        }
        if (!ManagerConfig.UseMock)
        {
            string path = getCacheListFileName();
            StringBuilder builder = new StringBuilder((this.saveNameList.Count + 1) * 0x80);
            builder.Append(string.Concat(new object[] { "@", ManagerConfig.MasterDataCacheVer, ",", dataVersion, ",", dateVersion, "\n" }));
            int count = this.saveNameList.Count;
            for (int i = 0; i < count; i++)
            {
                builder.Append(string.Concat(new object[] { this.saveNameList[i], ",", this.saveCrcList[i], "\n" }));
            }
            string s = builder.ToString();
            uint num3 = Crc32.Compute(Encoding.UTF8.GetBytes(s));
            Debug.Log(string.Concat(new object[] { "CRC MasterDataWriteRequest :[", s.Length, "] ", num3 }));
            object[] objArray4 = new object[] { "~", num3, "\n", s };
            string str = string.Concat(objArray4);
            uint? crc = null;
            string str4 = MdcStr.Ec(ref crc, str, 0);
            using (StreamWriter writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.Write(str4);
                writer.Close();
            }
        }
    }

    public bool DispLog =>
        this._DispLog;

    [CompilerGenerated]
    private sealed class <IsBattleLiveOpen>c__AnonStorey68
    {
        internal BattleData data;

        internal bool <>m__4F(int t)
        {
            foreach (BattleServantData data in this.data.getPlayerServantList())
            {
                if (data.getEquipBattleUserServantList() != null)
                {
                    foreach (BattleUserServantData data2 in data.getEquipBattleUserServantList())
                    {
                        if ((data2 != null) && DataManager.svtIds.Contains(data2.svtId))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        internal bool <>m__50(int t)
        {
            foreach (BattleServantData data in this.data.getEnemyServantList())
            {
                if (DataManager.svtIds.Contains(data.svtId))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool <>m__51(int t)
        {
            foreach (BattleServantData data in this.data.getPlayerServantList())
            {
                if (DataManager.svtIds.Contains(data.svtId))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool <>m__52(int t)
        {
            foreach (BattleSkillInfoData data in this.data.masterSkillInfo)
            {
                if (DataManager.mstSkillIds.Contains(data.skillId))
                {
                    return true;
                }
            }
            return false;
        }
    }

    [CompilerGenerated]
    private sealed class <readMasterData>c__Iterator11 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal DataManager <>f__this;
        internal BinaryReader <br>__1;
        internal Exception <e>__7;
        internal int <forCnt>__2;
        internal int <i>__3;
        internal string <mainCryptString>__4;
        internal string <mainDataFileName>__0;
        internal string <mainDataString>__6;
        internal uint <mainKey>__5;

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
                    Debug.Log("DataManager : readMasterData ver " + DataManager.dataVersion);
                    DataManager.readMasterDataResult = DataManager.ReadMasterDataResult.BUSY;
                    if (!this.<>f__this.CheckWaitforFrame())
                    {
                        break;
                    }
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 1;
                    goto Label_0258;

                case 1:
                    break;

                case 2:
                    goto Label_020C;

                case 3:
                    goto Label_023E;

                default:
                    goto Label_0256;
            }
            if (!this.<>f__this.readMasterDataListFile())
            {
                goto Label_0217;
            }
            this.<mainDataFileName>__0 = DataManager.getCacheFileName();
            if (!File.Exists(this.<mainDataFileName>__0))
            {
                goto Label_0217;
            }
            try
            {
                this.<br>__1 = new BinaryReader(File.OpenRead(this.<mainDataFileName>__0));
                try
                {
                    this.<forCnt>__2 = this.<>f__this.saveNameList.Count;
                    this.<i>__3 = 0;
                    while (this.<i>__3 < this.<forCnt>__2)
                    {
                        this.<mainCryptString>__4 = this.<br>__1.ReadString();
                        this.<mainKey>__5 = this.<>f__this.getMdk(this.<>f__this.saveNameList[this.<i>__3], DataManager.dataVersion);
                        this.<mainDataString>__6 = MdcStr.Dc(this.<mainCryptString>__4, this.<mainKey>__5, new uint?(this.<>f__this.saveCrcList[this.<i>__3]));
                        this.<>f__this.saveCrcList.Add(0);
                        this.<>f__this.saveDataList.Add(this.<mainDataString>__6);
                        this.<i>__3++;
                    }
                }
                finally
                {
                    if (this.<br>__1 != null)
                    {
                        ((IDisposable) this.<br>__1).Dispose();
                    }
                }
            }
            catch (Exception exception)
            {
                this.<e>__7 = exception;
                Debug.LogError("exception : " + this.<e>__7.Message);
                this.<>f__this.ClearSaveDataList();
                DataManager.readMasterDataResult = DataManager.ReadMasterDataResult.READ_ERROR;
                goto Label_0256;
            }
            if (this.<>f__this.CheckWaitforFrame())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 2;
                goto Label_0258;
            }
        Label_020C:
            DataManager.readMasterDataResult = DataManager.ReadMasterDataResult.OK;
            goto Label_0256;
        Label_0217:
            if (this.<>f__this.CheckWaitforFrame())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 3;
                goto Label_0258;
            }
        Label_023E:
            this.<>f__this.ClearSaveDataList();
            DataManager.readMasterDataResult = DataManager.ReadMasterDataResult.EMPTY_MASTER_ERROR;
            this.$PC = -1;
        Label_0256:
            return false;
        Label_0258:
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
    private sealed class <updateMasterData>c__Iterator12 : IEnumerator, IDisposable, IEnumerator<object>
    {
        internal object $current;
        internal int $PC;
        internal DataManager <>f__this;
        internal BinaryWriter <bw>__19;
        internal string <cacheName>__31;
        internal string <cachePath>__0;
        internal uint? <crc>__22;
        internal string <cryptDataString>__23;
        internal string <cryptString>__4;
        internal Dictionary<string, object> <data>__3;
        internal string <dataFileName>__18;
        internal string <dataString>__5;
        internal Exception <e>__1;
        internal Exception <e>__13;
        internal Exception <e>__17;
        internal Exception <e>__24;
        internal Exception <e>__25;
        internal Exception <e>__6;
        internal Exception <e>__7;
        internal Exception <e>__8;
        internal int <forCnt>__9;
        internal int <forCntb>__15;
        internal int <forCntb>__29;
        internal int <forCntc>__26;
        internal int <i>__10;
        internal int <i>__20;
        internal int <i>__27;
        internal int <i>__32;
        internal bool <isAdd>__2;
        internal bool <isNew>__14;
        internal int <j>__16;
        internal int <j>__30;
        internal string <masterName>__11;
        internal string <masterName>__28;
        internal string <writeData>__12;
        internal uint <writeKey>__21;

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
                    Debug.Log("DataManager : updateMasterData ver " + DataManager.dataVersion);
                    DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.BUSY;
                    SingletonMonoBehaviour<CommonUI>.Instance.SetMessageShow(true);
                    try
                    {
                        this.<cachePath>__0 = DataManager.getCachePath();
                        if (!Directory.Exists(this.<cachePath>__0))
                        {
                            if (this.<>f__this._DispLog)
                            {
                                Debug.Log("DataManager : updateMasterData create directory start");
                            }
                            Directory.CreateDirectory(this.<cachePath>__0);
                            if (this.<>f__this._DispLog)
                            {
                                Debug.Log("DataManager : updateMasterData create directory end");
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        this.<e>__1 = exception;
                        Debug.Log("UpdateMasterData : error " + this.<e>__1.Message);
                        DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.WRITE_ERROR;
                        goto Label_0A66;
                    }
                    this.<isAdd>__2 = false;
                    if (DataManager.updateData == null)
                    {
                        goto Label_07F8;
                    }
                    this.<data>__3 = null;
                    this.<cryptString>__4 = null;
                    this.<dataString>__5 = null;
                    try
                    {
                        this.<cryptString>__4 = DataManager.updateData.ToString();
                        Debug.Log("cryptString ::: " + this.<cryptString>__4.Length);
                    }
                    catch (Exception exception2)
                    {
                        this.<e>__6 = exception2;
                        Debug.Log("UpdateMasterData : error " + this.<e>__6.Message);
                        DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.CRYPT_ERROR;
                        goto Label_0A66;
                    }
                    DataManager.updateData = null;
                    if (this.<>f__this.CheckWaitforFrame())
                    {
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 1;
                        goto Label_0A68;
                    }
                    break;

                case 1:
                    break;

                case 2:
                    goto Label_0250;

                case 3:
                    goto Label_02C8;

                case 4:
                    goto Label_0425;

                case 5:
                    goto Label_053F;

                case 6:
                    goto Label_05E9;

                case 7:
                    goto Label_0769;

                case 8:
                    goto Label_07F8;

                case 9:
                    goto Label_0952;

                case 10:
                    goto Label_0A02;

                default:
                    goto Label_0A66;
            }
            try
            {
                this.<dataString>__5 = CryptData.Decrypt(this.<cryptString>__4, true);
                this.<cryptString>__4 = null;
                Debug.Log("dataString ::: " + this.<dataString>__5.Length);
            }
            catch (Exception exception3)
            {
                this.<e>__7 = exception3;
                Debug.Log("UpdateMasterData : error " + this.<e>__7.Message);
                DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.CRYPT_ERROR;
                goto Label_0A66;
            }
            if (this.<>f__this.CheckWaitforFrame())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 2;
                goto Label_0A68;
            }
        Label_0250:
            try
            {
                this.<data>__3 = JsonManager.getDictionary(this.<dataString>__5);
                this.<dataString>__5 = null;
            }
            catch (Exception exception4)
            {
                this.<e>__8 = exception4;
                Debug.Log("UpdateMasterData : error " + this.<e>__8.Message);
                DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.JSON_ERROR;
                goto Label_0A66;
            }
            if (this.<>f__this.CheckWaitforFrame())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 3;
                goto Label_0A68;
            }
        Label_02C8:
            if (this.<>f__this._DispLog)
            {
                Debug.Log("MasterData master data sum " + this.<>f__this.datalist.Length);
            }
            if (this.<data>__3 == null)
            {
                Debug.Log("UpdateMasterData : error update master data empty");
                DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.EMPTY_MASTER_ERROR;
                goto Label_0A66;
            }
            this.<forCnt>__9 = this.<>f__this.datalist.Length;
            this.<i>__10 = 0;
            while (this.<i>__10 < this.<forCnt>__9)
            {
                this.<masterName>__11 = this.<>f__this.datalist[this.<i>__10].getCacheName();
                if (!this.<data>__3.ContainsKey(this.<masterName>__11))
                {
                    goto Label_053F;
                }
                if (this.<>f__this._DispLog)
                {
                    Debug.Log("MasterData write master " + this.<masterName>__11);
                }
                this.<writeData>__12 = null;
                try
                {
                    this.<writeData>__12 = JsonManager.toJson(this.<data>__3[this.<masterName>__11]);
                    this.<data>__3[this.<masterName>__11] = null;
                }
                catch (Exception exception5)
                {
                    this.<e>__13 = exception5;
                    Debug.Log("UpdateMasterData : error " + this.<e>__13.Message);
                    DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.JSON_ERROR;
                    goto Label_0A66;
                }
                if (this.<>f__this.CheckWaitforFrame())
                {
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 4;
                    goto Label_0A68;
                }
            Label_0425:
                this.<isNew>__14 = true;
                this.<forCntb>__15 = this.<>f__this.saveNameList.Count;
                this.<j>__16 = 0;
                while (this.<j>__16 < this.<forCntb>__15)
                {
                    if (this.<>f__this.saveNameList[this.<j>__16] == this.<masterName>__11)
                    {
                        this.<>f__this.saveDataList[this.<j>__16] = this.<writeData>__12;
                        this.<isNew>__14 = false;
                        break;
                    }
                    this.<j>__16++;
                }
                if (this.<isNew>__14)
                {
                    this.<>f__this.saveNameList.Add(this.<masterName>__11);
                    this.<>f__this.saveCrcList.Add(0);
                    this.<>f__this.saveDataList.Add(this.<writeData>__12);
                }
                this.<masterName>__11 = null;
                this.<writeData>__12 = null;
                this.<isAdd>__2 = true;
                if (this.<>f__this.CheckWaitforFrame())
                {
                    this.$current = new WaitForEndOfFrame();
                    this.$PC = 5;
                    goto Label_0A68;
                }
            Label_053F:
                this.<i>__10++;
            }
            this.<data>__3.Clear();
            if (!this.<isAdd>__2 || ManagerConfig.UseMock)
            {
                if (!ManagerConfig.UseMock)
                {
                    Debug.Log("UpdateMasterData : error update master data add none");
                    DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.EMPTY_MASTER_ERROR;
                    goto Label_0A66;
                }
                goto Label_07F8;
            }
            try
            {
                this.<>f__this.DeleteCacheFile();
            }
            catch (Exception exception6)
            {
                this.<e>__17 = exception6;
                Debug.Log("UpdateMasterData : error " + this.<e>__17.Message);
                DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.WRITE_ERROR;
                goto Label_0A66;
            }
            if (this.<>f__this.CheckWaitforFrame())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 6;
                goto Label_0A68;
            }
        Label_05E9:
            try
            {
                this.<dataFileName>__18 = DataManager.getCacheFileName();
                this.<bw>__19 = new BinaryWriter(File.OpenWrite(this.<dataFileName>__18));
                try
                {
                    this.<forCnt>__9 = this.<>f__this.saveDataList.Count;
                    this.<i>__20 = 0;
                    while (this.<i>__20 < this.<forCnt>__9)
                    {
                        this.<writeKey>__21 = this.<>f__this.getMdk(this.<>f__this.saveNameList[this.<i>__20], DataManager.dataVersion);
                        this.<crc>__22 = 0;
                        this.<cryptDataString>__23 = MdcStr.Ec(ref this.<crc>__22, this.<>f__this.saveDataList[this.<i>__20], this.<writeKey>__21);
                        this.<>f__this.saveCrcList[this.<i>__20] = this.<crc>__22.Value;
                        this.<bw>__19.Write(this.<cryptDataString>__23);
                        this.<i>__20++;
                    }
                    this.<bw>__19.Close();
                }
                finally
                {
                    if (this.<bw>__19 != null)
                    {
                        ((IDisposable) this.<bw>__19).Dispose();
                    }
                }
            }
            catch (Exception exception7)
            {
                this.<e>__24 = exception7;
                Debug.Log("UpdateMasterData : error " + this.<e>__24.Message);
                DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.WRITE_ERROR;
                goto Label_0A66;
            }
            if (this.<>f__this.CheckWaitforFrame())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 7;
                goto Label_0A68;
            }
        Label_0769:
            try
            {
                this.<>f__this.writeMasterDataListFile();
            }
            catch (Exception exception8)
            {
                this.<e>__25 = exception8;
                Debug.Log("UpdateMasterData : error " + this.<e>__25.Message);
                DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.WRITE_ERROR;
                goto Label_0A66;
            }
            if (this.<>f__this.CheckWaitforFrame())
            {
                this.$current = new WaitForEndOfFrame();
                this.$PC = 8;
                goto Label_0A68;
            }
        Label_07F8:
            if (this.<>f__this.saveNameList.Count <= 0)
            {
                Debug.Log("UpdateMasterData : error master data empty");
                DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.EMPTY_MASTER_ERROR;
            }
            else
            {
                this.<forCntc>__26 = this.<>f__this.saveNameList.Count;
                this.<i>__27 = 0;
                while (this.<i>__27 < this.<forCntc>__26)
                {
                    this.<masterName>__28 = this.<>f__this.saveNameList[this.<i>__27];
                    this.<forCntb>__29 = this.<>f__this.datalist.Length;
                    this.<j>__30 = 0;
                    while (this.<j>__30 < this.<forCntb>__29)
                    {
                        this.<cacheName>__31 = this.<>f__this.datalist[this.<j>__30].getCacheName();
                        if (this.<masterName>__28 == this.<cacheName>__31)
                        {
                            if (this.<>f__this._DispLog)
                            {
                                Debug.Log("MasterData replace master " + this.<masterName>__28);
                            }
                            this.<>f__this.datalist[this.<j>__30].Replaced(this.<>f__this.saveDataList[this.<i>__27]);
                            if (!this.<>f__this.CheckWaitforFrame())
                            {
                                break;
                            }
                            this.$current = new WaitForEndOfFrame();
                            this.$PC = 9;
                            goto Label_0A68;
                        }
                        this.<j>__30++;
                    }
                Label_0952:
                    this.<>f__this.saveDataList[this.<i>__27] = null;
                    this.<>f__this.saveNameList[this.<i>__27] = null;
                    this.<i>__27++;
                }
                this.<forCntc>__26 = this.<>f__this.datalist.Length;
                this.<i>__32 = 0;
                while (this.<i>__32 < this.<forCntc>__26)
                {
                    if (this.<>f__this.datalist[this.<i>__32].preProcess() && this.<>f__this.CheckWaitforFrame())
                    {
                        this.$current = new WaitForEndOfFrame();
                        this.$PC = 10;
                        goto Label_0A68;
                    }
                Label_0A02:
                    this.<i>__32++;
                }
                BalanceConfig.Initialize();
                UserServantLockManager.Initialize();
                UserServantNewManager.Initialize();
                UserServantCollectionManager.Initialize();
                ServantCommentManager.Initialize();
                OtherUserNewManager.Initialize();
                ImageLimitCount.Initialize();
                UserEquipNewManager.Initialize();
                UserMissionProgressManager.Initialize();
                SingletonMonoBehaviour<CommonUI>.Instance.SetMessageShow(false);
                DataManager.updateMasterDataResult = DataManager.UpdateMasterDataResult.OK;
                this.$PC = -1;
            }
        Label_0A66:
            return false;
        Label_0A68:
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

    public enum ReadMasterDataResult
    {
        BUSY,
        OK,
        CRYPT_ERROR,
        JSON_ERROR,
        READ_ERROR,
        EMPTY_MASTER_ERROR
    }

    public enum UpdateMasterDataResult
    {
        BUSY,
        OK,
        CRYPT_ERROR,
        JSON_ERROR,
        WRITE_ERROR,
        EMPTY_MASTER_ERROR
    }
}

