using System;
using System.Runtime.CompilerServices;

public static class Follower
{
    public static Type getType(int type) => 
        ((Type) type);

    public static bool isUseTreasure(this Type type) => 
        (type != Type.NOT_FRIEND);

    public enum Type
    {
        NONE,
        FRIEND,
        NOT_FRIEND,
        NPC
    }
}

