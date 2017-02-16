using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserServantLockManager
{
    protected static bool isModfiy = false;
    protected static List<long> lockList = new List<long>();
    protected static readonly string SAVE_DATA_VERSION = "Fgo_20150511";

    public static bool ChangeLock(long userSvtId)
    {
        int count = lockList.Count;
        for (int i = 0; i < count; i++)
        {
            if (lockList[i] == userSvtId)
            {
                lockList.RemoveAt(i);
                isModfiy = true;
                return false;
            }
        }
        lockList.Add(userSvtId);
        isModfiy = true;
        return true;
    }

    protected static void ClearSaveDataList()
    {
        if (lockList.Count > 0)
        {
            lockList.Clear();
            isModfiy = true;
        }
    }

    public static void DeleteSaveData()
    {
    }

    protected static string getSaveFileName() => 
        (Application.persistentDataPath + "/userservantlocksave.dat");

    public static void Initialize()
    {
    }

    public static void InitServantLockStatus()
    {
        foreach (UserServantEntity entity in SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantMaster>(DataNameKind.Kind.USER_SERVANT).getList())
        {
            if (entity.getLockState() == 1)
            {
                lockList.Add(entity.id);
            }
        }
    }

    public static bool IsLock(long userSvtId)
    {
        int count = lockList.Count;
        for (int i = 0; i < count; i++)
        {
            if (lockList[i] == userSvtId)
            {
                return true;
            }
        }
        return false;
    }

    public static bool ReadData()
    {
        isModfiy = false;
        if (ManagerConfig.UseMock)
        {
            return true;
        }
        string path = getSaveFileName();
        Debug.Log("UserServantLockManager::read " + path);
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
                    int num = reader.ReadInt32();
                    for (int i = 0; i < num; i++)
                    {
                        long item = reader.ReadInt64();
                        lockList.Add(item);
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

    public static void SetLock(long userSvtId, bool isLock)
    {
        int count = lockList.Count;
        for (int i = 0; i < count; i++)
        {
            if (lockList[i] == userSvtId)
            {
                if (!isLock)
                {
                    lockList.RemoveAt(i);
                    isModfiy = true;
                }
                return;
            }
        }
        if (isLock)
        {
            lockList.Add(userSvtId);
            isModfiy = true;
        }
    }

    public static bool WriteData()
    {
        if (!isModfiy)
        {
            return false;
        }
        isModfiy = false;
        return (ManagerConfig.UseMock || true);
    }

    public enum ServantLockState
    {
        UNLOCK,
        LOCK
    }
}

