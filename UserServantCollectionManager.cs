using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserServantCollectionManager
{
    protected static bool isContinueDevice = false;
    protected static bool isModfiy = false;
    protected static List<int> oldList = new List<int>();
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
        (Application.persistentDataPath + "/userservantcollectionsave.dat");

    public static void Initialize()
    {
        Debug.Log("UserServantCollectionManager::Initialize");
        ReadData();
    }

    public static bool IsNew(int svtId)
    {
        int count = oldList.Count;
        for (int i = 0; i < count; i++)
        {
            if (oldList[i] == svtId)
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
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).continueDeviceUserServantCollection();
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
        Debug.Log("UserServantCollectionManager::read " + path);
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
                        int item = reader.ReadInt32();
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

    public static void SetOld(int svtId)
    {
        int count = oldList.Count;
        for (int i = 0; i < count; i++)
        {
            if (oldList[i] == svtId)
            {
                return;
            }
        }
        oldList.Add(svtId);
        isModfiy = true;
    }

    public static void SetOld(int[] svtIdList)
    {
        if (svtIdList != null)
        {
            int length = svtIdList.Length;
            if (length > 0)
            {
                int count = oldList.Count;
                for (int i = 0; i < count; i++)
                {
                    int num4 = oldList[i];
                    for (int k = 0; k < length; k++)
                    {
                        if (num4 == svtIdList[k])
                        {
                            svtIdList[k] = 0;
                            break;
                        }
                    }
                }
                for (int j = 0; j < length; j++)
                {
                    if (svtIdList[j] > 0)
                    {
                        oldList.Add(svtIdList[j]);
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

