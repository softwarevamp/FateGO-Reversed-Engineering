using System;
using System.Runtime.CompilerServices;

public static class FuncList
{
    public static bool Check(this TYPE type, int val) => 
        (type == val);

    public static int getCount(this TYPE type, int[] vals)
    {
        if ((vals != null) && (3 <= vals.Length))
        {
            return vals[2];
        }
        return 0;
    }

    public static int getRate(this TYPE type, int[] vals)
    {
        if ((vals != null) && (1 <= vals.Length))
        {
            return vals[0];
        }
        return 0;
    }

    public static int getRate(int intype, int[] vals)
    {
        TYPE type = (TYPE) intype;
        return type.getRate(vals);
    }

    public static int getTransformIndex(this TYPE type, int[] vals)
    {
        if (((type == TYPE.TRANSFORM_SERVANT) && (vals != null)) && (3 <= vals.Length))
        {
            return vals[2];
        }
        return 0;
    }

    public static int getTurn(this TYPE type, int[] vals)
    {
        if ((vals != null) && (2 <= vals.Length))
        {
            return vals[1];
        }
        return 0;
    }

    public static int getUseRate(this TYPE type, int[] vals)
    {
        if ((vals != null) && (5 <= vals.Length))
        {
            return vals[4];
        }
        return 0;
    }

    public static int getValue(this TYPE type, int[] vals)
    {
        if (((type == TYPE.ADD_STATE) || (type == TYPE.ADD_STATE_SHORT)) && ((vals != null) && (4 <= vals.Length)))
        {
            return vals[3];
        }
        if ((vals != null) && (2 <= vals.Length))
        {
            return vals[1];
        }
        return 0;
    }

    public static int getValueFronIndex(int[] vals, int index)
    {
        if ((vals != null) && (index <= vals.Length))
        {
            return vals[index];
        }
        return 0;
    }

    public enum COND
    {
        NONE,
        INVARIABLY,
        PROB,
        MOREHP,
        LESSHP
    }

    public enum TYPE
    {
        ADD_STATE = 1,
        ADD_STATE_SHORT = 0x10,
        CARD_RESET = 0x17,
        CLASS_DROP_UP = 0x71,
        DAMAGE = 3,
        DAMAGE_NP = 4,
        DAMAGE_NP_HPRATIO_HIGH = 0x15,
        DAMAGE_NP_HPRATIO_LOW = 0x16,
        DAMAGE_NP_INDIVIDUAL = 15,
        DAMAGE_NP_PIERCE = 14,
        DAMAGE_NP_STATE_INDIVIDUAL = 0x12,
        DELAY_NPTURN = 20,
        DROP_UP = 0x67,
        ENEMY_ENCOUNT_COPY_RATE_UP = 0x72,
        ENEMY_ENCOUNT_RATE_UP = 0x73,
        EVENT_DROP_RATE_UP = 0x6a,
        EVENT_DROP_UP = 0x69,
        EVENT_POINT_RATE_UP = 0x6c,
        EVENT_POINT_UP = 0x6b,
        EXP_UP = 0x65,
        EXTEND_SKILL = 10,
        FRIEND_POINT_UP = 0x68,
        GAIN_HP = 6,
        GAIN_HP_PER = 0x11,
        GAIN_NP = 7,
        GAIN_STAR = 5,
        HASTEN_NPTURN = 0x13,
        INSTANT_DEATH = 13,
        LOSS_HP = 12,
        LOSS_HP_SAFE = 0x19,
        LOSS_NP = 8,
        NONE = 0,
        QP_DROP_UP = 110,
        QP_UP = 0x66,
        RELEASE_STATE = 11,
        REPLACE_MEMBER = 0x18,
        SERVANT_FRIENDSHIP_UP = 0x6f,
        SHORTEN_SKILL = 9,
        SUB_STATE = 2,
        TRANSFORM_SERVANT = 0x6d,
        USER_EQUIP_EXP_UP = 0x70
    }
}

