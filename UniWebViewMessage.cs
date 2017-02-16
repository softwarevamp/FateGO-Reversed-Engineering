using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayout(LayoutKind.Sequential)]
public struct UniWebViewMessage
{
    public UniWebViewMessage(string rawMessage)
    {
        this.rawMessage = rawMessage;
        string[] separator = new string[] { "://" };
        string[] strArray = rawMessage.Split(separator, StringSplitOptions.None);
        if (strArray.Length >= 2)
        {
            this.scheme = strArray[0];
            string str = string.Empty;
            for (int i = 1; i < strArray.Length; i++)
            {
                str = str + strArray[i];
            }
            char[] chArray1 = new char[] { "?"[0] };
            string[] strArray2 = str.Split(chArray1);
            char[] trimChars = new char[] { '/' };
            this.path = strArray2[0].TrimEnd(trimChars);
            this.args = new Dictionary<string, string>();
            if (strArray2.Length > 1)
            {
                char[] chArray3 = new char[] { "&"[0] };
                foreach (string str2 in strArray2[1].Split(chArray3))
                {
                    char[] chArray4 = new char[] { "="[0] };
                    string[] strArray4 = str2.Split(chArray4);
                    if (strArray4.Length > 1)
                    {
                        this.args[strArray4[0]] = WWW.UnEscapeURL(strArray4[1]);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Bad url scheme. Can not be parsed to UniWebViewMessage: " + rawMessage);
        }
    }

    public string rawMessage { get; private set; }
    public string scheme { get; private set; }
    public string path { get; private set; }
    public Dictionary<string, string> args { get; private set; }
}

