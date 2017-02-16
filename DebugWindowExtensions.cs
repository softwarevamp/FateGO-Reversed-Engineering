using System;
using System.Runtime.CompilerServices;

public static class DebugWindowExtensions
{
    public static string GetStrSafe(this DebugWindow dwin, int idx) => 
        ((dwin == null) ? string.Empty : dwin.GetStr(idx));

    public static int GetValSafe(this DebugWindow dwin, int idx) => 
        ((dwin == null) ? 0 : dwin.GetVal(idx));

    public static bool IsActiveSafe(this DebugWindow dwin) => 
        ((dwin != null) && dwin.IsActive());

    public static bool IsEnableSafe(this DebugWindow dwin, int idx) => 
        ((dwin != null) && dwin.IsEnable(idx));

    public static void SetStrSafe(this DebugWindow dwin, int idx, string str)
    {
        if (dwin != null)
        {
            dwin.SetStr(idx, str);
        }
    }

    public static void SetValSafe(this DebugWindow dwin, int idx, int val)
    {
        if (dwin != null)
        {
            dwin.SetVal(idx, val);
        }
    }
}

