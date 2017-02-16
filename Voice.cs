using System;
using System.Collections.Generic;

public class Voice
{
    protected static Dictionary<BATTLE, string> filelist;

    static Voice()
    {
        Dictionary<BATTLE, string> dictionary = new Dictionary<BATTLE, string> {
            { 
                BATTLE.NONE,
                null
            },
            { 
                BATTLE.ATTACK1,
                "B010"
            },
            { 
                BATTLE.ATTACK2,
                "B020"
            },
            { 
                BATTLE.ATTACK3,
                "B030"
            },
            { 
                BATTLE.EX1,
                "B040"
            },
            { 
                BATTLE.EX2,
                "B040"
            },
            { 
                BATTLE.EX3,
                "B040"
            },
            { 
                BATTLE.HOUGU1_1,
                "B050"
            },
            { 
                BATTLE.HOUGU1_2,
                "B051"
            },
            { 
                BATTLE.HOUGU1_3,
                "B052"
            },
            { 
                BATTLE.HOUGU1_4,
                "B053"
            },
            { 
                BATTLE.HOUGU1_5,
                "B054"
            },
            { 
                BATTLE.HOUGU2_1,
                "B060"
            },
            { 
                BATTLE.HOUGU2_2,
                "B061"
            },
            { 
                BATTLE.HOUGU2_3,
                "B062"
            },
            { 
                BATTLE.HOUGU2_4,
                "B063"
            },
            { 
                BATTLE.HOUGU2_5,
                "B064"
            },
            { 
                BATTLE.HOUGU3_1,
                "B070"
            },
            { 
                BATTLE.HOUGU3_2,
                "B071"
            },
            { 
                BATTLE.HOUGU3_3,
                "B072"
            },
            { 
                BATTLE.HOUGU3_4,
                "B073"
            },
            { 
                BATTLE.HOUGU3_5,
                "B074"
            },
            { 
                BATTLE.SKILL1,
                "B080"
            },
            { 
                BATTLE.SKILL2,
                "B090"
            },
            { 
                BATTLE.START1,
                "B100"
            },
            { 
                BATTLE.START2,
                "B110"
            },
            { 
                BATTLE.WIN1,
                "B120"
            },
            { 
                BATTLE.WIN2,
                "B130"
            },
            { 
                BATTLE.DAMAGE1,
                "B140"
            },
            { 
                BATTLE.HDAMAGE1,
                "B150"
            },
            { 
                BATTLE.DEAD1,
                "B160"
            },
            { 
                BATTLE.DEAD2,
                "B170"
            },
            { 
                BATTLE.CARD1,
                "B180"
            },
            { 
                BATTLE.CARD2,
                "B190"
            },
            { 
                BATTLE.CARD3,
                "B200"
            },
            { 
                BATTLE.HCARD1,
                "B210"
            },
            { 
                BATTLE.HCARD2,
                "B220"
            },
            { 
                BATTLE.HCARD3,
                "B230"
            },
            { 
                BATTLE.EENTRY1,
                "B300"
            },
            { 
                BATTLE.EENTRY2,
                "B300"
            }
        };
        filelist = dictionary;
    }

    public static string getFileName(BATTLE type)
    {
        if (filelist.ContainsKey(type))
        {
            return filelist[type];
        }
        return null;
    }

    public enum BATTLE
    {
        ATTACK1 = 10,
        ATTACK2 = 11,
        ATTACK3 = 12,
        CARD1 = 0xc9,
        CARD2 = 0xca,
        CARD3 = 0xcb,
        DAMAGE1 = 0x3d,
        DEAD1 = 0x51,
        DEAD2 = 0x52,
        EENTRY1 = 0xd6,
        EENTRY2 = 0xd7,
        EX1 = 20,
        EX2 = 0x15,
        EX3 = 0x16,
        HCARD1 = 0xd3,
        HCARD2 = 0xd4,
        HCARD3 = 0xd5,
        HDAMAGE1 = 0x47,
        HOUGU1_1 = 0x6f,
        HOUGU1_2 = 0x70,
        HOUGU1_3 = 0x71,
        HOUGU1_4 = 0x72,
        HOUGU1_5 = 0x73,
        HOUGU2_1 = 0x79,
        HOUGU2_2 = 0x7a,
        HOUGU2_3 = 0x7b,
        HOUGU2_4 = 0x7c,
        HOUGU2_5 = 0x7d,
        HOUGU3_1 = 0x83,
        HOUGU3_2 = 0x84,
        HOUGU3_3 = 0x85,
        HOUGU3_4 = 0x86,
        HOUGU3_5 = 0x87,
        NONE = 0,
        SKILL1 = 0x1f,
        SKILL2 = 0x20,
        START1 = 0x29,
        START2 = 0x2a,
        WIN1 = 0x33,
        WIN2 = 0x34
    }
}

