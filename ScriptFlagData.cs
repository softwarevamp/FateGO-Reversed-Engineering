using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ScriptFlagData
{
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map1D;
    protected bool flag;
    protected string name;

    public ScriptFlagData(string name)
    {
        this.name = name;
        this.flag = false;
    }

    public bool Get() => 
        this.flag;

    public void Set(bool flag)
    {
        this.flag = flag;
    }

    public void Set(string v)
    {
        string key = v;
        if (key != null)
        {
            int num;
            if (<>f__switch$map1D == null)
            {
                Dictionary<string, int> dictionary = new Dictionary<string, int>(6) {
                    { 
                        "true",
                        0
                    },
                    { 
                        "on",
                        0
                    },
                    { 
                        "1",
                        0
                    },
                    { 
                        "false",
                        1
                    },
                    { 
                        "off",
                        1
                    },
                    { 
                        "0",
                        1
                    }
                };
                <>f__switch$map1D = dictionary;
            }
            if (<>f__switch$map1D.TryGetValue(key, out num))
            {
                if (num == 0)
                {
                    this.flag = true;
                }
                else if (num == 1)
                {
                    this.flag = false;
                }
            }
        }
    }

    public string Name =>
        this.name;
}

