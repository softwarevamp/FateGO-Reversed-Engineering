using System;
using System.Collections.Generic;

public class ScriptReplaceString
{
    protected static int playerGenderIndex = 1;
    protected static List<string> replaceList = new List<string>();

    public static int GetPlayerGenderIndex() => 
        playerGenderIndex;

    public static string GetString(int num) => 
        replaceList[num];

    public static void Init()
    {
        replaceList.Clear();
        SetString(1, "[#[FF0000]主[-]人公:しゅじんこう]じゅげむじゅげむ");
        SetString(5, "ルビを含むテキストにも[#置換:ちかん]できますよ");
        playerGenderIndex = 1;
    }

    public static void SetPlayerGenderIndex(int index)
    {
        playerGenderIndex = index;
    }

    public static void SetString(int num, string str)
    {
        if (replaceList.Count <= num)
        {
            while (replaceList.Count < num)
            {
                replaceList.Add(string.Empty);
            }
            replaceList.Add(str);
        }
        else
        {
            replaceList[num] = str;
        }
    }
}

