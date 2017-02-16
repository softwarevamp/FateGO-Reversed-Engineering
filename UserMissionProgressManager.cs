using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserMissionProgressManager
{
    protected static bool isContinueDevice = false;
    protected static bool isModfiy = false;
    protected static List<UserMissionProgressInfo> missionProgList = new List<UserMissionProgressInfo>();
    protected static readonly string SAVE_DATA_VERSION = "Fgo_20160211_1";
    protected static string saveName;

    protected static void ClearSaveDataList()
    {
        if (missionProgList.Count > 0)
        {
            missionProgList.Clear();
        }
        isContinueDevice = false;
        isModfiy = true;
    }

    public static void CreateContinueDeviceSaveData()
    {
        DeleteSaveData();
        ClearSaveDataList();
        isContinueDevice = true;
        isModfiy = true;
        WriteData();
    }

    public static void DeleteSaveData()
    {
        string path = getSaveFileName();
        Debug.Log("Delete File [" + path + "]");
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public static List<UserMissionProgressInfo> GetMissionProgInfoList() => 
        missionProgList;

    public static string getSaveFileName() => 
        (Application.persistentDataPath + saveName);

    public static string getSaveFileNameByEvent(int eventId) => 
        (Application.persistentDataPath + $"/usermissionprogsave_{eventId}.dat");

    public static void Initialize()
    {
        Debug.Log("UserMissionProgressManager::Initialize ");
        ReadData();
    }

    public static bool ReadData()
    {
        isModfiy = false;
        string path = getSaveFileName();
        Debug.Log("UserMissionProgressManager::read " + path);
        if (File.Exists(path))
        {
            ClearSaveDataList();
            try
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
                {
                    string str2 = reader.ReadString();
                    if (SAVE_DATA_VERSION != str2)
                    {
                        return false;
                    }
                    isContinueDevice = reader.ReadBoolean();
                    int num = reader.ReadInt32();
                    for (int i = 0; i < num; i++)
                    {
                        int num3 = reader.ReadInt32();
                        int num4 = reader.ReadInt32();
                        int num5 = reader.ReadInt32();
                        int num6 = reader.ReadInt32();
                        long num7 = reader.ReadInt64();
                        long num8 = reader.ReadInt64();
                        int num9 = reader.ReadInt32();
                        UserMissionProgressInfo item = new UserMissionProgressInfo {
                            eventId = num3,
                            missionId = num4,
                            currentProgressType = num5,
                            currentProgStatus = num6,
                            progNum = num7,
                            targetNum = num8,
                            condMsgType = num9
                        };
                        missionProgList.Add(item);
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
        }
        ClearSaveDataList();
        return false;
    }

    public static void SetAchiveMission(int missionId, int progStatus)
    {
        foreach (UserMissionProgressInfo info in missionProgList)
        {
            if ((info.missionId == missionId) && (info.currentProgressType == 4))
            {
                if (info.currentProgStatus == progStatus)
                {
                    break;
                }
                info.currentProgStatus = progStatus;
                isModfiy = true;
            }
        }
    }

    public static void SetMissionProgData(int eventId)
    {
        EventMissionEntity[] entityArray = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventMissionMaster>(DataNameKind.Kind.EVENT_MISSION).getEventMissionList(eventId);
        if (entityArray != null)
        {
            foreach (EventMissionEntity entity in entityArray)
            {
                UserMissionProgressInfo item = new UserMissionProgressInfo(eventId, entity.id);
                missionProgList.Add(item);
            }
        }
    }

    public static void setSaveFileName(int eventId)
    {
        saveName = $"/usermissionprogsave_{eventId}.dat";
    }

    public static bool WriteData()
    {
        if (!isModfiy)
        {
            return false;
        }
        isModfiy = false;
        if (ManagerConfig.UseMock)
        {
            return false;
        }
        string path = getSaveFileName();
        try
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(path)))
            {
                int count = missionProgList.Count;
                writer.Write(SAVE_DATA_VERSION);
                writer.Write(isContinueDevice);
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    UserMissionProgressInfo info = missionProgList[i];
                    writer.Write(info.eventId);
                    writer.Write(info.missionId);
                    writer.Write(info.currentProgressType);
                    writer.Write(info.currentProgStatus);
                    writer.Write(info.progNum);
                    writer.Write(info.targetNum);
                    writer.Write(info.condMsgType);
                }
            }
            return true;
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
            return false;
        }
    }
}

