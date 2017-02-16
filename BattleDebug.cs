using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public static class BattleDebug
{
    private static Queue<string> logQueue = new Queue<string>();
    private const int MAXQUEUE = 0x3e8;

    public static string[] getLoglist(string serchText, TAG tag = 0, bool reverseFlg = false)
    {
        <getLoglist>c__AnonStorey79 storey = new <getLoglist>c__AnonStorey79 {
            tag = tag,
            serchText = serchText
        };
        string[] array = logQueue.ToArray();
        if (storey.tag != TAG.NONE)
        {
            array = Array.FindAll<string>(array, new Predicate<string>(storey.<>m__92));
        }
        if ((storey.serchText != null) || !storey.serchText.Equals(string.Empty))
        {
            array = Array.FindAll<string>(array, new Predicate<string>(storey.<>m__93));
        }
        if (reverseFlg)
        {
            Array.Reverse(array);
        }
        return array;
    }

    public static TAG getTag(int param) => 
        ((TAG) param);

    [Conditional("UNITY_EDITOR")]
    public static void Log(LinkedList<BattleLogicTask> taskList, TAG tag = 0)
    {
    }

    [Conditional("UNITY_EDITOR")]
    public static void Log(string str, TAG tag = 0)
    {
    }

    public static void Reset()
    {
        logQueue.Clear();
    }

    [CompilerGenerated]
    private sealed class <getLoglist>c__AnonStorey79
    {
        internal string serchText;
        internal BattleDebug.TAG tag;

        internal bool <>m__92(string s) => 
            (0 <= s.IndexOf($"<{this.tag.ToString()}>"));

        internal bool <>m__93(string s) => 
            (0 <= s.IndexOf($"{this.serchText}"));
    }

    public enum TAG
    {
        NONE,
        SKILL,
        ACTION,
        FUNCTION,
        AI,
        ANIMATION,
        KEISAN,
        PRINT,
        BUFF,
        PERF,
        LOGIC
    }
}

