using System;
using System.Runtime.CompilerServices;

public static class AiAct
{
    public static bool Check(this TYPE type, int intype) => 
        (type == intype);

    public static int getInt(this TYPE type) => 
        ((int) type);

    public static TYPE getType(int intype) => 
        ((TYPE) intype);

    public static bool isThinkEnd(this TYPE type) => 
        (type != TYPE.CHANGE_THINKING);

    public static bool isThinkEnd(int intype)
    {
        TYPE type = (TYPE) intype;
        return type.isThinkEnd();
    }

    public enum TARGET
    {
        HP_HIGHER = 2,
        HP_LOWER = 3,
        NONE = 0,
        NPGAUGE_HIGHER = 11,
        NPTURN_LOWER = 10,
        RANDOM = 1,
        REVENGE = 12
    }

    public enum TYPE
    {
        ATTACK = 2,
        ATTACK_CRITICAL = 30,
        CHANGE_THINKING = 0x63,
        NOBLE_PHANTASM = 80,
        NONE = 0,
        PLAY_MOTION = 0x47,
        RANDOM = 1,
        SKILL_ID = 40,
        SKILL_RANDOM = 10,
        SKILL1 = 11,
        SKILL2 = 12,
        SKILL3 = 13
    }
}

