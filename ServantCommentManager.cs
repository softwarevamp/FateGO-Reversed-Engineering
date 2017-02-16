using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class ServantCommentManager
{
    protected static bool isContinueDevice = false;
    protected static bool isModfiy = false;
    protected static List<UserServantCommentOpenInfo> openList = new List<UserServantCommentOpenInfo>();
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

    public static List<ServantCommentEntity> GetOpenServantCommentEntityByServantFriendShip(int svt_id, int oldFriendShipRank = -1)
    {
        List<ServantCommentEntity> collection = new List<ServantCommentEntity>();
        ServantCommentEntity[] entitiyList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantCommentMaster>(DataNameKind.Kind.SERVANT_COMMENT).GetEntitiyList(svt_id, CondType.Kind.SVT_FRIENDSHIP);
        if (entitiyList != null)
        {
            collection.AddRange(entitiyList);
        }
        for (int i = collection.Count - 1; i >= 0; i--)
        {
            if (!collection[i].IsOpen(oldFriendShipRank))
            {
                collection.RemoveAt(i);
            }
        }
        if (oldFriendShipRank >= 0)
        {
            List<ServantCommentEntity> list2 = new List<ServantCommentEntity>(collection);
            collection = GetOpenServantCommentEntityByServantFriendShip(svt_id, -1);
            for (int j = collection.Count - 1; j >= 0; j--)
            {
                ServantCommentEntity entity = collection[j];
                foreach (ServantCommentEntity entity2 in list2)
                {
                    if (entity.id == entity2.id)
                    {
                        collection.RemoveAt(j);
                        break;
                    }
                }
            }
        }
        return collection;
    }

    protected static string getSaveFileName() => 
        (Application.persistentDataPath + "/userservantcommentsave.dat");

    public static void Initialize()
    {
        Debug.Log("UserServantCommentManager::Initialize ");
        ReadData();
    }

    public static bool IsOpen(int svtId, int svtCommentId)
    {
        int count = openList.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantCommentOpenInfo info = openList[i];
            if (info.svtId == svtId)
            {
                int num3 = info.svtCommentIdList.Count;
                for (int j = 0; j < num3; j++)
                {
                    if (info.svtCommentIdList[j] == svtCommentId)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static bool IsOpenByServantFriendShip(int svt_id, int oldFriendShipRank = -1) => 
        (GetOpenServantCommentEntityByServantFriendShip(svt_id, oldFriendShipRank).Count > 0);

    public static void LoginProcess()
    {
        if (isContinueDevice)
        {
            SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserServantCollectionMaster>(DataNameKind.Kind.USER_SERVANT_COLLECTION).continueDeviceServantComment();
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
            BinaryReader reader = new BinaryReader(File.OpenRead(path));
            try
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
                    UserServantCommentOpenInfo item = new UserServantCommentOpenInfo {
                        svtId = num3,
                        svtCommentIdList = new List<int>()
                    };
                    int num4 = reader.ReadInt32();
                    for (int j = 0; j < num4; j++)
                    {
                        int num6 = reader.ReadInt32();
                        item.svtCommentIdList.Add(num6);
                    }
                    openList.Add(item);
                }
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
            }
            finally
            {
                if (reader != null)
                {
                    ((IDisposable) reader).Dispose();
                }
            }
        }
        ClearSaveDataList();
        return false;
    }

    public static void SetOpen(int svtId, int svtCommentId)
    {
        int count = openList.Count;
        for (int i = 0; i < count; i++)
        {
            UserServantCommentOpenInfo info = openList[i];
            if (info.svtId == svtId)
            {
                int num3 = info.svtCommentIdList.Count;
                for (int j = 0; j < num3; j++)
                {
                    if (info.svtCommentIdList[j] == svtCommentId)
                    {
                        return;
                    }
                }
                info.svtCommentIdList.Add(svtCommentId);
                isModfiy = true;
                return;
            }
        }
        UserServantCommentOpenInfo item = new UserServantCommentOpenInfo {
            svtId = svtId,
            svtCommentIdList = new List<int>()
        };
        item.svtCommentIdList.Add(svtCommentId);
        openList.Add(item);
        isModfiy = true;
    }

    public static void SetOpen(int svtId, int[] svtCommentIdList)
    {
        if (svtCommentIdList != null)
        {
            int length = svtCommentIdList.Length;
            if (length > 0)
            {
                int count = openList.Count;
                for (int i = 0; i < count; i++)
                {
                    UserServantCommentOpenInfo info = openList[i];
                    if (info.svtId == svtId)
                    {
                        int num4 = info.svtCommentIdList.Count;
                        for (int k = 0; k < length; k++)
                        {
                            int num6 = svtCommentIdList[k];
                            for (int m = 0; m < num4; m++)
                            {
                                if (info.svtCommentIdList[m] == num6)
                                {
                                    num6 = 0;
                                    break;
                                }
                            }
                            if (num6 > 0)
                            {
                                info.svtCommentIdList.Add(num6);
                                isModfiy = true;
                            }
                        }
                        return;
                    }
                }
                UserServantCommentOpenInfo item = new UserServantCommentOpenInfo {
                    svtId = svtId,
                    svtCommentIdList = new List<int>()
                };
                for (int j = 0; j < length; j++)
                {
                    item.svtCommentIdList.Add(svtCommentIdList[j]);
                }
                openList.Add(item);
                isModfiy = true;
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
                    UserServantCommentOpenInfo info = openList[i];
                    int num3 = info.svtCommentIdList.Count;
                    writer.Write(info.svtId);
                    writer.Write(num3);
                    for (int j = 0; j < num3; j++)
                    {
                        writer.Write(info.svtCommentIdList[j]);
                    }
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

    protected class UserServantCommentOpenInfo
    {
        public List<int> svtCommentIdList;
        public int svtId;
    }
}

