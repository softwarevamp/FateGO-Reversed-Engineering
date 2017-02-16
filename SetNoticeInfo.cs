using System;

public class SetNoticeInfo
{
    private static bool isApRecover = true;
    private static bool isGameNotice = true;

    public static bool GetisApRecover() => 
        isApRecover;

    public static bool GetisGameNotice() => 
        isGameNotice;

    public static void SetisApRecover(bool isNotice)
    {
        isApRecover = isNotice;
        Debug.Log("**!! SetNoticeInfo isApRecover : " + isApRecover);
    }

    public static void SetisGameNotice(bool isNotice)
    {
        isGameNotice = isNotice;
        Debug.Log("**!! SetNoticeInfo isApRecover : " + isApRecover);
    }
}

