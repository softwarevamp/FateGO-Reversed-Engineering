using System;
using System.Runtime.CompilerServices;

public class CommandSpellEntity : DataEntityBase
{
    [CompilerGenerated]
    private static Converter<string, int> <>f__am$cache9;
    public int consume;
    public string detail;
    public int[] funcId;
    public int id;
    public int motion;
    public string name;
    public int priority;
    public int type;
    public string[] vals;

    public DataVals[] getDataValsList()
    {
        if (this.vals != null)
        {
            DataVals[] valsArray = new DataVals[this.vals.Length];
            for (int i = 0; i < this.vals.Length; i++)
            {
                valsArray[i] = new DataVals(this.vals[i]);
            }
            return valsArray;
        }
        return new DataVals[] { new DataVals(string.Empty) };
    }

    public string getName() => 
        this.name;

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int[][] getValues()
    {
        if (this.vals != null)
        {
            int[][] numArray = new int[this.vals.Length][];
            for (int i = 0; i < this.vals.Length; i++)
            {
                char[] separator = new char[] { ',' };
                string[] array = this.vals[i].Replace("[", string.Empty).Replace("]", string.Empty).Split(separator);
                if (<>f__am$cache9 == null)
                {
                    <>f__am$cache9 = input => int.Parse(input);
                }
                numArray[i] = Array.ConvertAll<string, int>(array, <>f__am$cache9);
            }
            return numArray;
        }
        return new int[][] { new int[5] };
    }

    public bool isUseBattle() => 
        ((this.type == 1) || (this.type == 2));
}

