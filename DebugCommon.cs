using System;
using System.Diagnostics;

public static class DebugCommon
{
    [Conditional("DEBUG")]
    public static void Assert(bool condition)
    {
        if (!condition)
        {
            throw new Exception();
        }
    }

    [Conditional("DEBUG")]
    public static void Assert(bool condition, Func<string> getMessage)
    {
        if (!condition)
        {
            throw new Exception(getMessage());
        }
    }

    [Conditional("DEBUG")]
    public static void Assert(bool condition, string message)
    {
        if (!condition)
        {
            throw new Exception(message);
        }
    }
}

