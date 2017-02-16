using System;
using System.Collections.Generic;

public class Target
{
    public static bool Check(TYPE cktype, int type) => 
        (cktype == type);

    public static int[] getTargetIds(BattleData bdata, int actorId, int targetId, int pttargetId, int type)
    {
        int[] numArray = new int[0];
        if (Check(TYPE.SELF, type))
        {
            numArray = new int[] { actorId };
            if (actorId == -1)
            {
                Debug.Log(" not actorId");
                int[] numArray2 = bdata.getFieldPlayerServantIDList();
                numArray[0] = numArray2[0];
            }
            return numArray;
        }
        if (Check(TYPE.PT_ONE, type))
        {
            numArray = new int[1];
            if (bdata.getServantData(pttargetId) != null)
            {
                numArray[0] = pttargetId;
                return numArray;
            }
            Debug.Log(" No Target");
            return new int[0];
        }
        if (Check(TYPE.PT_ALL, type))
        {
            if (bdata.isEnemyID(actorId))
            {
                return bdata.getFieldEnemyServantIDList();
            }
            return bdata.getFieldPlayerServantIDList();
        }
        if (Check(TYPE.ENEMY_ALL, type))
        {
            if (bdata.isEnemyID(actorId))
            {
                return bdata.getFieldPlayerServantIDList();
            }
            return bdata.getFieldEnemyServantIDList();
        }
        if (Check(TYPE.PT_FULL, type))
        {
            if (bdata.isEnemyID(actorId))
            {
                return bdata.getEnemyServantIDList(true);
            }
            return bdata.getPlayerServantIDList(true);
        }
        if (Check(TYPE.ENEMY_FULL, type))
        {
            if (bdata.isEnemyID(actorId))
            {
                return bdata.getPlayerServantIDList(true);
            }
            return bdata.getEnemyServantIDList(true);
        }
        if (Check(TYPE.ENEMY, type))
        {
            return new int[] { targetId };
        }
        if (Check(TYPE.PT_OTHER, type))
        {
            int[] numArray3;
            if (bdata.isEnemyID(actorId))
            {
                numArray3 = bdata.getFieldEnemyServantIDList();
            }
            else
            {
                numArray3 = bdata.getFieldPlayerServantIDList();
            }
            if (numArray3.Length <= 1)
            {
                return numArray;
            }
            List<int> list = new List<int>();
            for (int i = 0; i < numArray3.Length; i++)
            {
                if (numArray3[i] != actorId)
                {
                    list.Add(numArray3[i]);
                }
            }
            return list.ToArray();
        }
        if (Check(TYPE.ENEMY_OTHER, type))
        {
            int[] numArray4;
            if (bdata.isEnemyID(actorId))
            {
                numArray4 = bdata.getFieldPlayerServantIDList();
            }
            else
            {
                numArray4 = bdata.getFieldEnemyServantIDList();
            }
            if (numArray4.Length > 1)
            {
                numArray = new int[numArray4.Length - 1];
                int index = 0;
                int num4 = 0;
                while (index < numArray4.Length)
                {
                    if (numArray4[index] != targetId)
                    {
                        numArray[num4] = numArray4[index];
                        num4++;
                    }
                    index++;
                }
            }
            return numArray;
        }
        if (Check(TYPE.PT_ONE_OTHER, type))
        {
            int[] numArray5;
            if (bdata.isEnemyID(actorId))
            {
                numArray5 = bdata.getFieldEnemyServantIDList();
            }
            else
            {
                numArray5 = bdata.getFieldPlayerServantIDList();
            }
            if (numArray5.Length > 1)
            {
                numArray = new int[numArray5.Length - 1];
                int num5 = 0;
                int num6 = 0;
                while (num5 < numArray5.Length)
                {
                    if (numArray5[num5] != pttargetId)
                    {
                        numArray[num6] = numArray5[num5];
                        num6++;
                    }
                    num5++;
                }
            }
            return numArray;
        }
        if (Check(TYPE.PT_OTHER_FULL, type))
        {
            int[] numArray6;
            if (bdata.isEnemyID(actorId))
            {
                numArray6 = bdata.getEnemyServantIDList(true);
            }
            else
            {
                numArray6 = bdata.getPlayerServantIDList(true);
            }
            if (numArray6.Length > 1)
            {
                numArray = new int[numArray6.Length - 1];
                int num7 = 0;
                int num8 = 0;
                while (num7 < numArray6.Length)
                {
                    if (numArray6[num7] != actorId)
                    {
                        numArray[num8] = numArray6[num7];
                        num8++;
                    }
                    num7++;
                }
            }
            return numArray;
        }
        if (Check(TYPE.ENEMY_OTHER_FULL, type))
        {
            int[] numArray7;
            if (bdata.isEnemyID(actorId))
            {
                numArray7 = bdata.getPlayerServantIDList(true);
            }
            else
            {
                numArray7 = bdata.getEnemyServantIDList(true);
            }
            if (numArray7.Length > 1)
            {
                numArray = new int[numArray7.Length - 1];
                int num9 = 0;
                int num10 = 0;
                while (num9 < numArray7.Length)
                {
                    if (numArray7[num9] != targetId)
                    {
                        numArray[num10] = numArray7[num9];
                        num10++;
                    }
                    num9++;
                }
            }
            return numArray;
        }
        if (Check(TYPE.PT_RANDOM, type))
        {
            int[] numArray8;
            if (bdata.isEnemyID(actorId))
            {
                numArray8 = bdata.getFieldEnemyServantIDList();
            }
            else
            {
                numArray8 = bdata.getFieldPlayerServantIDList();
            }
            int num11 = BattleRandom.getNext(numArray8.Length);
            return new int[] { numArray8[num11] };
        }
        if (Check(TYPE.ENEMY_RANDOM, type))
        {
            int[] numArray9;
            if (bdata.isEnemyID(actorId))
            {
                numArray9 = bdata.getFieldPlayerServantIDList();
            }
            else
            {
                numArray9 = bdata.getFieldEnemyServantIDList();
            }
            int num12 = BattleRandom.getNext(numArray9.Length);
            return new int[] { numArray9[num12] };
        }
        if (Check(TYPE.PTSELECT_ONE_SUB, type))
        {
            numArray = new int[1];
            if (bdata.getServantData(pttargetId) != null)
            {
                numArray[0] = pttargetId;
                return numArray;
            }
            Debug.Log(" No Target");
            return new int[0];
        }
        if (Check(TYPE.PTSELECT_SUB, type))
        {
            numArray = new int[] { actorId };
            if (actorId == -1)
            {
                Debug.Log(" not actorId");
                int[] numArray10 = bdata.getFieldPlayerServantIDList();
                numArray[0] = numArray10[0];
            }
        }
        return numArray;
    }

    public static int[] getTargetIds(BattleData bdata, int actorId, int targetId, int pttargetId, TYPE tgType) => 
        getTargetIds(bdata, actorId, targetId, pttargetId, (int) tgType);

    public static bool isChoose(int targetType) => 
        (Check(TYPE.PT_ONE, targetType) || Check(TYPE.PT_ONE_OTHER, targetType));

    public static bool isEnemy(int targetType) => 
        ((((Check(TYPE.ENEMY, targetType) || Check(TYPE.ENEMY_ANOTHER, targetType)) || (Check(TYPE.ENEMY_ALL, targetType) || Check(TYPE.ENEMY_FULL, targetType))) || (Check(TYPE.ENEMY_OTHER, targetType) || Check(TYPE.ENEMY_RANDOM, targetType))) || Check(TYPE.ENEMY_OTHER_FULL, targetType));

    public static bool isPlayer(int targetType) => 
        (((((Check(TYPE.SELF, targetType) || Check(TYPE.PT_ONE, targetType)) || (Check(TYPE.PT_ANOTHER, targetType) || Check(TYPE.PT_ALL, targetType))) || ((Check(TYPE.PT_FULL, targetType) || Check(TYPE.PT_OTHER, targetType)) || (Check(TYPE.PT_ONE_OTHER, targetType) || Check(TYPE.PT_RANDOM, targetType)))) || (Check(TYPE.PT_OTHER_FULL, targetType) || Check(TYPE.PTSELECT_ONE_SUB, targetType))) || Check(TYPE.PTSELECT_SUB, targetType));

    public static bool isSubChoose(int targetType) => 
        (Check(TYPE.PTSELECT_ONE_SUB, targetType) || Check(TYPE.PTSELECT_SUB, targetType));

    public enum TYPE
    {
        SELF,
        PT_ONE,
        PT_ANOTHER,
        PT_ALL,
        ENEMY,
        ENEMY_ANOTHER,
        ENEMY_ALL,
        PT_FULL,
        ENEMY_FULL,
        PT_OTHER,
        PT_ONE_OTHER,
        PT_RANDOM,
        ENEMY_OTHER,
        ENEMY_RANDOM,
        PT_OTHER_FULL,
        ENEMY_OTHER_FULL,
        PTSELECT_ONE_SUB,
        PTSELECT_SUB
    }
}

