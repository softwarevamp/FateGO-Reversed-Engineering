using System;
using System.Runtime.CompilerServices;

internal static class AdstoreActionExt
{
    public static string DisplayName(this AdstoreAction action)
    {
        string[] strArray = new string[] { "action001", "action002" };
        return strArray[(int) action];
    }
}

