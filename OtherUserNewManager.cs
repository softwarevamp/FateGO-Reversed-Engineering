using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class OtherUserNewManager
{
    protected static bool isContinueDevice = false;
    protected static bool isModfiy = false;
    protected static List<long> oldList = new List<long>();
    protected static readonly string SAVE_DATA_VERSION = "Fgo_20151127_1";

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
        (Application.persistentDataPath + "/otherusernewsave.dat");

    public static void Initialize()
    {
        Debug.Log("OtherUserNewManager::Initialize");
        ReadData();
    }

    public static bool IsNew(long userId)
    {
        int count = oldList.Count;
        for (int i = 0; i < count; i++)
        {
            if (oldList[i] == userId)
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
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData<OtherUserGameMaster>(DataNameKind.Kind.OTHER_USER_GAME).continueDeviceOtherUser();
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
        Debug.Log("OtherUserNewManager::read " + path);
        ClearSaveDataList();
        if (File.Exists(path))
        {
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
                ClearSaveDataList();
            }
        }
        return false;
    }

    public static void SetOld(long userId)
    {
        int count = oldList.Count;
        for (int i = 0; i < count; i++)
        {
            if (oldList[i] == userId)
            {
                return;
            }
        }
        oldList.Add(userId);
        isModfiy = true;
    }

    public static void SetOld(long[] userList)
    {
        if (userList != null)
        {
            int length = userList.Length;
            if (length > 0)
            {
                int count = oldList.Count;
                for (int i = 0; i < count; i++)
                {
                    long num4 = oldList[i];
                    for (int k = 0; k < length; k++)
                    {
                        if (num4 == userList[k])
                        {
                            userList[k] = 0L;
                            break;
                        }
                    }
                }
                for (int j = 0; j < length; j++)
                {
                    if (userList[j] > 0L)
                    {
                        oldList.Add(userList[j]);
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

