using System;

public class BattleRandom : SingletonMonoBehaviour<BattleRandom>
{
    public int gutsCount;
    public Random gutsrandom;
    public int logicCount;
    public Random logicrandom;

    public static void adjustment()
    {
        BattleRandom random = SingletonMonoBehaviour<BattleRandom>.getInstance();
        if (random != null)
        {
            for (int i = 0; i < random.logicCount; i++)
            {
                random.logicrandom.Next();
            }
            for (int j = 0; j < random.gutsCount; j++)
            {
                random.gutsrandom.Next();
            }
        }
    }

    public static int getGutsNext(int max)
    {
        BattleRandom random = SingletonMonoBehaviour<BattleRandom>.getInstance();
        if (random != null)
        {
            random.gutsCount++;
            return random.gutsrandom.Next(max);
        }
        return (max - 1);
    }

    public static int getNext(int max)
    {
        BattleRandom random = SingletonMonoBehaviour<BattleRandom>.getInstance();
        if (random != null)
        {
            random.logicCount++;
            return random.logicrandom.Next(max);
        }
        return (max - 1);
    }

    public static int getRandom(int min, int max)
    {
        BattleRandom random = SingletonMonoBehaviour<BattleRandom>.getInstance();
        if (random != null)
        {
            random.logicCount++;
            return random.logicrandom.Next(min, max);
        }
        return min;
    }

    public static T[] getShuffle<T>(T[] list)
    {
        T[] localArray = (T[]) list.Clone();
        int length = localArray.Length;
        while (length > 1)
        {
            length--;
            int index = getNext(length + 1);
            T local = localArray[index];
            localArray[index] = localArray[length];
            localArray[length] = local;
        }
        return localArray;
    }

    public static void setSeed(int seed)
    {
        BattleRandom random = SingletonMonoBehaviour<BattleRandom>.getInstance();
        if (random != null)
        {
            random.logicrandom = new Random(seed);
            random.gutsrandom = new Random(seed);
            random.logicCount = 0;
            random.gutsCount = 0;
        }
    }
}

