using System;

public class ItemType
{
    public static string GetCountText(Type type, int num)
    {
        switch (type)
        {
            case Type.QP:
                return string.Format(LocalizationManager.Get("QP_UNIT"), num);

            case Type.STONE:
                return string.Format(LocalizationManager.Get("STONE_UNIT"), num);

            case Type.MANA:
                return string.Format(LocalizationManager.Get("MANA_UNIT"), num);

            case Type.FRIEND_POINT:
                return string.Format(LocalizationManager.Get("FRIEND_POINT_UNIT"), num);
        }
        return LocalizationManager.GetUnitInfo(num);
    }

    public enum Type
    {
        AP_ADD = 4,
        AP_RECOVER = 3,
        EVENT_ITEM = 15,
        EVENT_POINT = 14,
        FRIEND_POINT = 13,
        GACHA_CLASS = 7,
        GACHA_RELIC = 8,
        GACHA_TICKET = 9,
        KEY = 6,
        LIMIT = 10,
        MANA = 5,
        QP = 1,
        QUEST_REWARD_QP = 0x10,
        SKILL_LV_UP = 11,
        STONE = 2,
        TD_LV_UP = 12
    }
}

