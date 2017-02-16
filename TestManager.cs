using System;
using UnityEngine;

public class TestManager : SingletonMonoBehaviour<TestManager>
{
    private static AndroidJavaClass classPlugin;
    private static AndroidJavaObject objPlugin;

    public void Awake()
    {
        base.Awake();
        classPlugin = new AndroidJavaClass("jp.delightworks.unityplugin.SampleCallPlugin");
        objPlugin = new AndroidJavaObject("jp.delightworks.unityplugin.SampleCallPlugin", new object[0]);
        Debug.Log(string.Concat(new object[] { "TestWebView ", classPlugin, ",", objPlugin }));
    }

    public static void CallFuncA(string str)
    {
        if (objPlugin != null)
        {
            object[] args = new object[] { str };
            objPlugin.Call("FuncA", args);
            Debug.Log("CallFuncA " + str);
        }
    }

    public static string CallFuncB(string str)
    {
        string str2 = null;
        if (objPlugin != null)
        {
            object[] args = new object[] { str };
            str2 = objPlugin.Call<string>("FuncB", args);
            Debug.Log("CallFuncB " + str + "," + str2);
        }
        return str2;
    }

    public static void CallFuncC(string str)
    {
        if (objPlugin != null)
        {
            object[] args = new object[] { "TestWebView", str };
            objPlugin.Call("FuncC", args);
            Debug.Log("CallFuncC " + str);
        }
    }
}

