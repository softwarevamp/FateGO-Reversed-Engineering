using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class ImageLimitCount
{
    private static readonly int enemyLimitCountStart;
    public const int FIRST_CHANGE_LIMIT_COUNT = 1;
    protected static readonly int[] imageLimitCountList = new int[] { 0, 1, 1, 2, 2 };
    protected static bool isAprilFool;
    protected static Dictionary<int, int> limitMaxList;
    protected static readonly int[] maxLimitCountList;
    protected static readonly int[] minLimitCountList;
    public const int REWARD_IMAGE_LIMIT_COUNT = 3;
    public const int SECOND_CHANGE_LIMIT_COUNT = 3;

    static ImageLimitCount()
    {
        int[] numArray1 = new int[4];
        numArray1[1] = 1;
        numArray1[2] = 3;
        numArray1[3] = 4;
        minLimitCountList = numArray1;
        int[] numArray2 = new int[4];
        numArray2[1] = 2;
        numArray2[2] = 3;
        numArray2[3] = 4;
        maxLimitCountList = numArray2;
        enemyLimitCountStart = 0x65;
    }

    public static int GetCardImageLimitCount(int svtId, int limitCount, bool isOwn = true, bool isReal = true)
    {
        if (limitCount < 0)
        {
            return 0;
        }
        if (!limitMaxList.ContainsKey(svtId))
        {
            return (limitCount - 1);
        }
        int index = limitMaxList[svtId];
        if (isAprilFool && !isReal)
        {
            return BalanceConfig.OtherImageLimitCount;
        }
        if (limitCount < index)
        {
            return imageLimitCountList[limitCount];
        }
        if (isOwn && (index >= 4))
        {
            return 3;
        }
        return imageLimitCountList[index];
    }

    public static int GetImageLimitCount(int svtId, int limitCount)
    {
        if (limitCount < 0)
        {
            return 0;
        }
        if (limitCount >= enemyLimitCountStart)
        {
            return (limitCount - enemyLimitCountStart);
        }
        if (!limitMaxList.ContainsKey(svtId))
        {
            return (limitCount - 1);
        }
        int index = limitMaxList[svtId];
        if (limitCount >= index)
        {
            return imageLimitCountList[index];
        }
        return imageLimitCountList[limitCount];
    }

    public static int GetLimitCountByImageLimit(int imageLimitCount)
    {
        if (imageLimitCount < minLimitCountList.Length)
        {
            return minLimitCountList[imageLimitCount];
        }
        return 0;
    }

    public static int GetLimitCountByImageLimit(int imageLimitCount, int maxLimitCount)
    {
        if (imageLimitCount < maxLimitCountList.Length)
        {
            int num = maxLimitCountList[imageLimitCount];
            return ((num <= maxLimitCount) ? num : maxLimitCount);
        }
        return 0;
    }

    public static void Initialize()
    {
        limitMaxList = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<ServantMaster>(DataNameKind.Kind.SERVANT).GetLimitCountMaxList();
        isAprilFool = false;
    }

    public static void initializeAssetStorage()
    {
        if (AssetManager.CheckDateVersion(BalanceConfig.AprilFoolAssetStorageDateVersion))
        {
            isAprilFool = true;
        }
    }

    public static bool IsAprilFool =>
        isAprilFool;
}

