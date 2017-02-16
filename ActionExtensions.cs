using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

public static class ActionExtensions
{
    [DebuggerStepThrough, DebuggerHidden]
    public static void Call(this System.Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static void Call<T>(this Action<T> action, T arg)
    {
        if (action != null)
        {
            action(arg);
        }
    }

    [DebuggerStepThrough, DebuggerHidden]
    public static void Call<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
    {
        if (action != null)
        {
            action(arg1, arg2);
        }
    }

    [DebuggerHidden, DebuggerStepThrough]
    public static void Call<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
    {
        if (action != null)
        {
            action(arg1, arg2, arg3);
        }
    }
}

