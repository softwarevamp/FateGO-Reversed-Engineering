using System;
using UnityEngine;

public class UserSaveData
{
    public static void DeleteContinueData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        NetworkManager.DeleteSaveData();
        AccountingManager.ClearAll();
        UserServantLockManager.DeleteSaveData();
        UserServantNewManager.DeleteSaveData();
        UserServantCollectionManager.DeleteSaveData();
        ServantCommentManager.DeleteSaveData();
        UserEquipNewManager.DeleteSaveData();
        OtherUserNewManager.DeleteSaveData();
    }

    public static void DeleteSaveData()
    {
        DeleteContinueData();
        DataManager.ClearCacheAll();
        AssetStorageCache.ClearCacheAll(true);
    }
}

