using System;
using System.Runtime.CompilerServices;

public static class Gift
{
    public static bool IsEventSvtGet(this Type type) => 
        (type == Type.EVENT_SVT_GET);

    public static bool IsEventSvtGet(int type) => 
        ((Type) type).IsEventSvtGet();

    public static bool IsItem(this Type type) => 
        ((((type == Type.ITEM) || (type == Type.FRIENDSHIP)) || (type == Type.USER_EXP)) || (type == Type.EQUIP));

    public static bool IsItem(int type) => 
        ((Type) type).IsItem();

    public static bool IsServant(this Type type) => 
        (((type == Type.SERVANT) || (type == Type.EVENT_SVT_GET)) || (type == Type.EVENT_SVT_JOIN));

    public static bool IsServant(int type) => 
        ((Type) type).IsServant();

    public enum Type
    {
        EQUIP = 5,
        EVENT_SVT_GET = 7,
        EVENT_SVT_JOIN = 6,
        FRIENDSHIP = 3,
        ITEM = 2,
        SERVANT = 1,
        USER_EXP = 4
    }
}

