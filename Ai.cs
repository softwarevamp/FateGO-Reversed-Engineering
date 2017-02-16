using System;
using System.Runtime.CompilerServices;

public static class Ai
{
    public static bool Check(this ACT_NUM actNum, int num) => 
        (actNum == num);

    public static int getChangeThinking(int[] avals)
    {
        if ((avals != null) && (1 <= avals.Length))
        {
            return avals[0];
        }
        return 0;
    }

    public static COND getCond(int intype) => 
        ((COND) intype);

    public enum ACT_NUM
    {
        ANYTIME = -1,
        NOMAL = 0,
        REACTION_DAMAGE = -2,
        REACTION_DEAD = -6,
        REACTION_ENEMYTURN_END = -5,
        REACTION_ENEMYTURN_START = -4,
        REACTION_PLAYERACTIONEND = -7,
        REACTION_SKILL = -3
    }

    public enum COND
    {
        ACTCOUNT = 20,
        ACTCOUNT_MULTIPLE = 0x15,
        ACTCOUNT_THISTURN = 0x3f,
        BEFORE_ACT_ID = 40,
        BEFORE_ACT_TYPE = 0x29,
        BEFORE_NOT_ACT_ID = 0x2a,
        BEFORE_NOT_ACT_TYPE = 0x2b,
        CHECK_OPPONENT_BUFF = 0x37,
        CHECK_OPPONENT_BUFF_INDIVIDUALITY = 0x3b,
        CHECK_OPPONENT_HEIGHT_NPGAUGE = 0x3e,
        CHECK_OPPONENT_INDIVIDUALITY = 0x38,
        CHECK_PT_BUFF = 0x34,
        CHECK_PT_BUFF_INDIVIDUALITY = 0x3a,
        CHECK_PT_INDIVIDUALITY = 0x35,
        CHECK_PT_LOWER_NPTURN = 0x3d,
        CHECK_SELF_BUFF = 50,
        CHECK_SELF_BUFF_INDIVIDUALITY = 0x39,
        CHECK_SELF_INDIVIDUALITY = 0x33,
        CHECK_SELF_NPTURN = 60,
        HP_HIGHER = 10,
        HP_LOWER = 11,
        NONE = 0,
        TURN = 30,
        TURN_MULTIPLE = 0x1f
    }
}

