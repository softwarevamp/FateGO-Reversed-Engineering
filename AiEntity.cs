using System;

public class AiEntity : DataEntityBase
{
    public int actNum;
    public int aiActId;
    public int[] avals;
    public int cond;
    public int id;
    public int idx;
    public string infoText;
    public int priority;
    public int probability;
    public int[] vals;

    public int getActionValue() => 
        this.getIndexAval(1);

    public int[] getAvals()
    {
        if (this.avals == null)
        {
            return new int[0];
        }
        return this.avals;
    }

    public int getChangeAiId() => 
        this.getIndexAval(0);

    private int getIndexAval(int index)
    {
        if ((this.avals != null) && (this.avals.Length > index))
        {
            return this.avals[index];
        }
        return 0;
    }

    private int getIndexVal(int index)
    {
        if ((this.vals != null) && (this.vals.Length > index))
        {
            return this.vals[index];
        }
        return 0;
    }

    public override string getPrimarykey()
    {
        object[] objArray1 = new object[] { string.Empty, this.id, ":", this.idx };
        return string.Concat(objArray1);
    }

    public int getVal() => 
        this.getIndexVal(0);

    public int[] getVals()
    {
        if (this.vals == null)
        {
            return new int[0];
        }
        return this.vals;
    }
}

