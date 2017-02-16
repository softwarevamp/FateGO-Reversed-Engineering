using System;
using System.Runtime.CompilerServices;

internal static class CameraPositionExt
{
    public static string DisplayName(this CameraPosition camPos)
    {
        string[] strArray = new string[] { "Unit_Player2", "Unit_Enemy2", "NobleStartPos_player", "NobleStartPos_enemy", "BattleFazeCameraPos" };
        return strArray[(int) camPos];
    }
}

