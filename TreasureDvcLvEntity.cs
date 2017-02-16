using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class TreasureDvcLvEntity : DataEntityBase
{
    [CompilerGenerated]
    private static Converter<string, int> <>f__am$cache11;
    public int detailId;
    public int[] funcId;
    public int gaugeCount;
    public int lv;
    public int qp;
    public int tdPoint;
    public int tdPointA;
    public int tdPointB;
    public int tdPointDef;
    public int tdPointEx;
    public int tdPointQ;
    public int treaureDeviceId;
    public string[] vals;
    public string[] vals2;
    public string[] vals3;
    public string[] vals4;
    public string[] vals5;

    public DataVals[] getDataValsList(int param = 100)
    {
        string[] vals = null;
        if (param < 200)
        {
            vals = this.vals;
        }
        else if (param < 300)
        {
            vals = this.vals2;
        }
        else if (param < 400)
        {
            vals = this.vals3;
        }
        else if (param < 500)
        {
            vals = this.vals4;
        }
        else
        {
            vals = this.vals5;
        }
        if (vals != null)
        {
            DataVals[] valsArray = new DataVals[vals.Length];
            for (int i = 0; i < vals.Length; i++)
            {
                valsArray[i] = new DataVals(vals[i]);
            }
            return valsArray;
        }
        return new DataVals[] { new DataVals(string.Empty) };
    }

    public string getDetail() => 
        this.getDetail(this.lv);

    public string getDetail(int _lv)
    {
        string str = (_lv <= 0) ? string.Empty : string.Format(LocalizationManager.Get("LEVEL_INFO"), _lv);
        return string.Format(TreasureDvcDetailMaster.getDetail(this.treaureDeviceId), str);
    }

    public string getDetalShort() => 
        this.getDetalShort(this.lv);

    public string getDetalShort(int _lv)
    {
        string str = (_lv <= 0) ? string.Empty : string.Format(LocalizationManager.Get("LEVEL_INFO"), _lv);
        return string.Format(TreasureDvcDetailMaster.getDetailShort(this.treaureDeviceId), str);
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.treaureDeviceId, ":", this.lv };
        return string.Concat(objArray1);
    }

    public int[][] getValues(int param = 100)
    {
        string[] vals = null;
        if (param < 200)
        {
            vals = this.vals;
        }
        else if (param < 300)
        {
            vals = this.vals2;
        }
        else if (param < 400)
        {
            vals = this.vals3;
        }
        else if (param < 500)
        {
            vals = this.vals4;
        }
        else
        {
            vals = this.vals5;
        }
        if (vals != null)
        {
            int[][] numArray = new int[vals.Length][];
            for (int i = 0; i < vals.Length; i++)
            {
                char[] separator = new char[] { ',' };
                string[] array = vals[i].Replace("[", string.Empty).Replace("]", string.Empty).Split(separator);
                if (<>f__am$cache11 == null)
                {
                    <>f__am$cache11 = input => int.Parse(input);
                }
                numArray[i] = Array.ConvertAll<string, int>(array, <>f__am$cache11);
            }
            return numArray;
        }
        return new int[][] { new int[5] };
    }

    public override void printDebug()
    {
        Debug.Log("TreasureDvcLvEntity:" + this.treaureDeviceId);
        for (int i = 0; i < this.funcId.Length; i++)
        {
            Debug.Log(string.Concat(new object[] { "funcId[", i, "]", this.funcId[i] }));
        }
    }
}

