using System;

public class StatusRank
{
    protected static readonly string[] aRankList = new string[] { string.Empty, "A", "A+", "A++", "A-" };
    protected static readonly string[] bRankList = new string[] { string.Empty, "B", "B+", "B++", "B-" };
    protected static readonly string[] cRankList = new string[] { string.Empty, "C", "C+", "C++", "C-" };
    protected static readonly string[] dRankList = new string[] { string.Empty, "D", "D+", "D++", "D-" };
    protected static readonly string[] eRankList = new string[] { string.Empty, "E", "E+", "E++", "E-" };
    protected static readonly string[] exRankList = new string[] { string.Empty, "EX" };

    public static string GetRankString(Kind kind)
    {
        int num = (int) kind;
        int num2 = num / 10;
        int index = num % 10;
        switch (num2)
        {
            case 1:
                return aRankList[index];

            case 2:
                return bRankList[index];

            case 3:
                return cRankList[index];

            case 4:
                return dRankList[index];

            case 5:
                return eRankList[index];

            case 6:
                return exRankList[index];
        }
        return string.Empty;
    }

    public enum Kind
    {
        A = 11,
        A_MINUS = 14,
        A_PLUS = 12,
        A_PLUS2 = 13,
        B = 0x15,
        B_MINUS = 0x18,
        B_PLUS = 0x16,
        B_PLUS2 = 0x17,
        C = 0x1f,
        C_MINUS = 0x22,
        C_PLUS = 0x20,
        C_PLUS2 = 0x21,
        D = 0x29,
        D_MINUS = 0x2c,
        D_PLUS = 0x2a,
        D_PLUS2 = 0x2b,
        E = 0x33,
        E_MINUS = 0x36,
        E_PLUS = 0x34,
        E_PLUS2 = 0x35,
        EX = 0x3d,
        NONE = 0x63
    }
}

