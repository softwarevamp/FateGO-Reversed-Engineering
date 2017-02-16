using System;
using System.Collections.Generic;

public class FileName
{
    protected static Dictionary<HIT_EFFECT, string> criticaleffectlist;
    public static string eventJoinIconName;
    public static string friendIconName;
    protected static Dictionary<HIT_EFFECT, string> hiteffectlist;

    static FileName()
    {
        Dictionary<HIT_EFFECT, string> dictionary = new Dictionary<HIT_EFFECT, string> {
            { 
                HIT_EFFECT.NONE,
                null
            },
            { 
                HIT_EFFECT.SLASH_VERTICAL,
                "ef_hit_slash01_M"
            },
            { 
                HIT_EFFECT.SLASH_HORIZON,
                "ef_hit_slash02_M"
            },
            { 
                HIT_EFFECT.BLOW,
                "ef_hit01_M"
            }
        };
        hiteffectlist = dictionary;
        dictionary = new Dictionary<HIT_EFFECT, string> {
            { 
                HIT_EFFECT.NONE,
                null
            },
            { 
                HIT_EFFECT.SLASH_VERTICAL,
                "ef_crit01"
            },
            { 
                HIT_EFFECT.SLASH_HORIZON,
                "ef_crit01"
            },
            { 
                HIT_EFFECT.BLOW,
                "ef_crit01"
            }
        };
        criticaleffectlist = dictionary;
        friendIconName = "icon_support_02";
        eventJoinIconName = "icon_eventjoin_02";
    }

    public static string getCriticalEffectName(HIT_EFFECT type)
    {
        if (criticaleffectlist.ContainsKey(type))
        {
            return criticaleffectlist[type];
        }
        return null;
    }

    public static string getEffectName(HIT_EFFECT type)
    {
        if (hiteffectlist.ContainsKey(type))
        {
            return hiteffectlist[type];
        }
        return null;
    }

    public enum HIT_EFFECT
    {
        NONE,
        SLASH_VERTICAL,
        SLASH_HORIZON,
        BLOW
    }
}

