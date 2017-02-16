using System;

public class ImageItem
{
    public static bool IsTreasure(Id id)
    {
        switch (id)
        {
            case Id.TREASURE_1:
            case Id.TREASURE_2:
            case Id.TREASURE_3:
                return true;
        }
        return false;
    }

    public static bool IsTreasure(int id) => 
        IsTreasure((Id) id);

    public enum Id
    {
        NONE,
        TREASURE_1,
        TREASURE_2,
        TREASURE_3,
        SERVANT,
        QP,
        STONE,
        MANA,
        NP,
        SKILL,
        SVT_EQUIP,
        GACHA_TICKET,
        FRIEND_POINT
    }
}

