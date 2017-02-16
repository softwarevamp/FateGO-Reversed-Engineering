using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserEquipNewManager
{
    protected static bool isContinueDevice = false;
    protected static bool isModfiy = false;
    protected static List<UserEquipLvInfo> openList = new List<UserEquipLvInfo>();
    protected static readonly string SAVE_DATA_VERSION = "Fgo_20150511_1";

    protected static void ClearSaveDataList()
    {
        if (openList.Count > 0)
        {
            openList.Clear();
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

    protected static string getSaveFileName() => 
        (Application.persistentDataPath + "/userequiplvsave.dat");

    public static void Initialize()
    {
        Debug.Log("UserEquipNewManager::Initialize ");
        ReadData();
    }

    public static bool IsNew(int equipId, int lv)
    {
        int count = openList.Count;
        for (int i = 0; i < count; i++)
        {
            UserEquipLvInfo info = openList[i];
            if ((info.equipId == equipId) && (info.equipLv < lv))
            {
                return true;
            }
        }
        return false;
    }

    public static void LoginProcess()
    {
        if (isContinueDevice)
        {
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserEquipMaster>(DataNameKind.Kind.USER_EQUIP).continueDeviceEquipLvInfo();
            isContinueDevice = false;
            isModfiy = true;
            WriteData();
        }
    }

    public static bool ReadData()
    {
        isModfiy = false;
        if (ManagerConfig.UseMock)
        {
            return true;
        }
        string path = getSaveFileName();
        Debug.Log("UserServantCommentManager::read " + path);
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
                        UserEquipLvInfo item = new UserEquipLvInfo {
                            equipId = num3,
                            equipLv = num4
                        };
                        openList.Add(item);
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

    public static void SetOld(int equipId, int lv)
    {
        int count = openList.Count;
        for (int i = 0; i < count; i++)
        {
            UserEquipLvInfo info = openList[i];
            if (info.equipId == equipId)
            {
                if (info.equipLv != lv)
                {
                    info.equipLv = lv;
                    isModfiy = true;
                }
                return;
            }
        }
        UserEquipLvInfo item = new UserEquipLvInfo {
            equipId = equipId,
            equipLv = lv
        };
        openList.Add(item);
        isModfiy = true;
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
                int count = openList.Count;
                writer.Write(SAVE_DATA_VERSION);
                writer.Write(isContinueDevice);
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    UserEquipLvInfo info = openList[i];
                    writer.Write(info.equipId);
                    writer.Write(info.equipLv);
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

    protected class UserEquipLvInfo
    {
        public int equipId;
        public int equipLv;
    }
}

