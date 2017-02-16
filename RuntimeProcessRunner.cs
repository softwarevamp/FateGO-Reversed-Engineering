using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RuntimeProcessRunner
{
    [CompilerGenerated]
    private static System.Action <>f__am$cache3;
    [CompilerGenerated]
    private static System.Action <>f__am$cache4;
    [CompilerGenerated]
    private static System.Action <>f__am$cache5;
    [CompilerGenerated]
    private static System.Action <>f__am$cache6;

    public RuntimeProcessRunner(string executable, string args)
    {
        if (<>f__am$cache5 == null)
        {
            <>f__am$cache5 = new System.Action(RuntimeProcessRunner.<RuntimeProcessRunner>m__260);
        }
        this.OnProcessSuccesful = <>f__am$cache5;
        if (<>f__am$cache6 == null)
        {
            <>f__am$cache6 = new System.Action(RuntimeProcessRunner.<RuntimeProcessRunner>m__261);
        }
        this.OnProcessFailed = <>f__am$cache6;
    }

    public RuntimeProcessRunner(string executable, string args, string workingDirectory, int timeoutMs)
    {
        if (<>f__am$cache3 == null)
        {
            <>f__am$cache3 = new System.Action(RuntimeProcessRunner.<RuntimeProcessRunner>m__25E);
        }
        this.OnProcessSuccesful = <>f__am$cache3;
        if (<>f__am$cache4 == null)
        {
            <>f__am$cache4 = new System.Action(RuntimeProcessRunner.<RuntimeProcessRunner>m__25F);
        }
        this.OnProcessFailed = <>f__am$cache4;
    }

    [CompilerGenerated]
    private static void <RuntimeProcessRunner>m__25E()
    {
    }

    [CompilerGenerated]
    private static void <RuntimeProcessRunner>m__25F()
    {
    }

    [CompilerGenerated]
    private static void <RuntimeProcessRunner>m__260()
    {
    }

    [CompilerGenerated]
    private static void <RuntimeProcessRunner>m__261()
    {
    }

    public void Abort()
    {
    }

    public void Execute()
    {
        try
        {
            this.ProcessSuccesful();
        }
        catch (Exception exception)
        {
            UnityEngine.Debug.LogError("Exception on process queue Thread: " + exception.Message + "\n" + exception.StackTrace);
        }
        finally
        {
            this.IsComplete = true;
        }
    }

    protected virtual void ProcessFailed(bool timedOut, string errorMessage, int errorCode)
    {
        this.OnProcessFailed();
        UnityEngine.Debug.Log($"Process Failed : {errorMessage} with code : {errorCode}");
    }

    protected virtual void ProcessSuccesful()
    {
        this.OnProcessSuccesful();
        UnityEngine.Debug.Log("Process Complete");
    }

    public bool IsComplete { get; private set; }

    public System.Action OnProcessFailed { get; set; }

    public System.Action OnProcessSuccesful { get; set; }
}

