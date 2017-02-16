using System;
using System.Collections.Generic;

public class AssetsFile
{
    protected static Dictionary<Path, string> filelist;

    static AssetsFile()
    {
        Dictionary<Path, string> dictionary = new Dictionary<Path, string> {
            { 
                Path.NONE,
                null
            },
            { 
                Path.SERVANTS,
                "Servants"
            },
            { 
                Path.BG,
                "Bg"
            }
        };
        filelist = dictionary;
    }

    public enum Path
    {
        NONE,
        SERVANTS,
        BG
    }
}

