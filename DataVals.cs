using System;
using System.Collections.Generic;

public class DataVals
{
    public FuncList.TYPE funcType;
    public string svals;
    public Dictionary<string, int> vals = new Dictionary<string, int>();

    public DataVals(string str)
    {
        this.svals = str;
    }

    public int GetParam(TYPE type)
    {
        string key = type.ToString();
        if (this.vals.ContainsKey(key))
        {
            return this.vals[key];
        }
        return 0;
    }

    public int GetValue() => 
        this.GetParam(TYPE.Value);

    public int GetValue(int index)
    {
        string key = index.ToString();
        if (this.vals.ContainsKey(key))
        {
            return this.vals[key];
        }
        return 0;
    }

    public bool isHideMiss() => 
        (1 == this.GetParam(TYPE.HideMiss));

    public int Length() => 
        this.vals.Count;

    public void SetType(FuncList.TYPE ft)
    {
        this.funcType = ft;
        char[] separator = new char[] { ',' };
        string[] strArray = this.svals.Replace("[", string.Empty).Replace("]", string.Empty).Split(separator);
        for (int i = 0; i < strArray.Length; i++)
        {
            int num2;
            string key = string.Empty;
            if (int.TryParse(strArray[i], out num2))
            {
                if ((this.funcType == FuncList.TYPE.ADD_STATE) || (this.funcType == FuncList.TYPE.ADD_STATE_SHORT))
                {
                    switch (i)
                    {
                        case 0:
                            key = TYPE.Rate.ToString();
                            goto Label_01F0;

                        case 1:
                            key = TYPE.Turn.ToString();
                            goto Label_01F0;

                        case 2:
                            key = TYPE.Count.ToString();
                            goto Label_01F0;

                        case 3:
                            key = TYPE.Value.ToString();
                            goto Label_01F0;

                        case 4:
                            key = TYPE.UseRate.ToString();
                            goto Label_01F0;
                    }
                    if (i == 5)
                    {
                        key = TYPE.Value2.ToString();
                    }
                }
                else if ((this.funcType == FuncList.TYPE.DAMAGE_NP_INDIVIDUAL) || (this.funcType == FuncList.TYPE.DAMAGE_NP_STATE_INDIVIDUAL))
                {
                    switch (i)
                    {
                        case 0:
                            key = TYPE.Rate.ToString();
                            goto Label_01F0;

                        case 1:
                            key = TYPE.Value.ToString();
                            goto Label_01F0;

                        case 2:
                            key = TYPE.Target.ToString();
                            goto Label_01F0;
                    }
                    if (i == 3)
                    {
                        key = TYPE.Correction.ToString();
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            key = TYPE.Rate.ToString();
                            goto Label_01F0;

                        case 1:
                            key = TYPE.Value.ToString();
                            goto Label_01F0;
                    }
                    if (i == 2)
                    {
                        key = TYPE.Target.ToString();
                    }
                }
            }
            else
            {
                char[] chArray2 = new char[] { ':' };
                string[] strArray2 = strArray[i].Split(chArray2);
                if (1 < strArray2.Length)
                {
                    key = strArray2[0];
                    int.TryParse(strArray2[1], out num2);
                }
            }
        Label_01F0:
            if (key != string.Empty)
            {
                this.vals.Add(key, num2);
            }
        }
    }

    public enum TYPE
    {
        Rate,
        Turn,
        Count,
        Value,
        Value2,
        UseRate,
        Target,
        Correction,
        ParamAdd,
        ParamMax,
        HideMiss
    }
}

