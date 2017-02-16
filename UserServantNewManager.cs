using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserServantNewManager
{
    protected static bool isContinueDevice = false;
    protected static bool isModfiy = false;
    protected static List<long> oldList = new List<long>();
    protected static readonly string SAVE_DATA_VERSION = "Fgo_20150511_1";

    protected static void ClearSaveDataList()
    {
        if (oldList.Count > 0)
        {
            oldList.Clear();
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
        (Application.persistentDataPath + "/userservantnewsave.dat");

    public static void Initialize()
    {
        Debug.Log("UserServantNewManager::Initialize");
        ReadData();
    }

    public static bool IsNew(long userSvtId)
    {
        int count = oldList.Count;
        for (int i = 0; i < count; i++)
        {
            if (oldList[i] == userSvtId)
            {
                return false;
            }
        }
        return true;
    }

    public static void LoginProcess()
    {
        if (isContinueDevice)
        {
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).continueDeviceUserServant();
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
        Debug.Log("UserServantNewManager::read " + path);
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
                        long item = reader.ReadInt64();
                        oldList.Add(item);
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

    public static void SetOld(long userSvtId)
    {
        int count = oldList.Count;
        for (int i = 0; i < count; i++)
        {
            if (oldList[i] == userSvtId)
            {
                return;
            }
        }
        oldList.Add(userSvtId);
        isModfiy = true;
    }

    public static void SetOld(long[] userSvtList)
    {
        if (userSvtList != null)
        {
            int length = userSvtList.Length;
            if (length > 0)
            {
                int count = oldList.Count;
                for (int i = 0; i < count; i++)
                {
                    long num4 = oldList[i];
                    for (int k = 0; k < length; k++)
                    {
                        if (num4 == userSvtList[k])
                        {
                            userSvtList[k] = 0L;
                            break;
                        }
                    }
                }
                for (int j = 0; j < length; j++)
                {
                    if (userSvtList[j] > 0L)
                    {
                        oldList.Add(userSvtList[j]);
                        isModfiy = true;
                    }
                }
            }
        }
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
            return true;
        }
        string path = getSaveFileName();
        try
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(path)))
            {
                int count = oldList.Count;
                writer.Write(SAVE_DATA_VERSION);
                writer.Write(isContinueDevice);
                writer.Write(count);
                for (int i = 0; i < count; i++)
                {
                    writer.Write(oldList[i]);
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

