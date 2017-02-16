using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResponseData
{
    public static bool _never;
    [CompilerGenerated]
    private static Dictionary<string, int> <>f__switch$map19;
    public Dictionary<string, object> fail;
    public int isBattleLive;
    public int[] mstSkillIds;
    public string nid;
    public int[] questIds;
    public string resCode;
    public Dictionary<string, object> success;
    public int[] svtIds;
    public string usk;

    public ResponseData()
    {
        if (_never)
        {
            Debug.Log(string.Empty + new ResponseData[1]);
        }
    }

    public bool checkError() => 
        this.checkError(this.resCode);

    public bool checkError(string resCode)
    {
        if (resCode != null)
        {
            string key = resCode;
            if (key != null)
            {
                int num;
                if (<>f__switch$map19 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(15) {
                        { 
                            "00",
                            0
                        },
                        { 
                            "100",
                            1
                        },
                        { 
                            "01",
                            2
                        },
                        { 
                            "02",
                            2
                        },
                        { 
                            "03",
                            2
                        },
                        { 
                            "04",
                            2
                        },
                        { 
                            "11",
                            2
                        },
                        { 
                            "71",
                            2
                        },
                        { 
                            "88",
                            2
                        },
                        { 
                            "89",
                            2
                        },
                        { 
                            "96",
                            3
                        },
                        { 
                            "98",
                            4
                        },
                        { 
                            "99",
                            4
                        },
                        { 
                            "1001",
                            5
                        },
                        { 
                            "1002",
                            6
                        }
                    };
                    <>f__switch$map19 = dictionary;
                }
                if (<>f__switch$map19.TryGetValue(key, out num))
                {
                    switch (num)
                    {
                        case 0:
                            return true;

                        case 1:
                            BattleData.deleteSaveData();
                            return true;

                        case 3:
                        {
                            string str = PlayerPrefs.GetString("Asset", "offline");
                            PlayerPrefs.DeleteAll();
                            PlayerPrefs.SetString("Asset", str);
                            PlayerPrefs.Save();
                            break;
                        }
                        case 5:
                            return true;
                    }
                }
            }
        }
        return false;
    }

    public void debugPrint()
    {
        Debug.Log("resCode:" + this.resCode);
        Debug.Log("nid:" + this.nid);
    }

    public string getErrorMessage()
    {
        Dictionary<string, object> fail = this.fail;
        if (fail.ContainsKey("detail"))
        {
            return fail["detail"].ToString();
        }
        return null;
    }
}

