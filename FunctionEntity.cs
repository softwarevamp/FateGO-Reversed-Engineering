using System;

public class FunctionEntity : DataEntityBase
{
    public int applyTarget;
    public int cond;
    public string detail;
    public int[] effectList;
    public int funcType;
    public int id;
    public int popupIconId;
    public string popupText;
    public int popupTextColor;
    public int[] questTvals;
    public int targetType;
    public int[] tvals;
    public int[] vals;

    public int[] getEffectList()
    {
        if (this.effectList == null)
        {
            return new int[0];
        }
        return this.effectList;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public int[] getQuestTargetValues()
    {
        if (this.questTvals == null)
        {
            return new int[0];
        }
        return this.questTvals;
    }

    public int[] getTargetValues() => 
        this.tvals;

    public int[] getValues() => 
        this.vals;

    public bool isTargetEnemy() => 
        ((this.applyTarget == 2) || (this.applyTarget == 3));

    public bool isTargetPlayer() => 
        ((this.applyTarget == 1) || (this.applyTarget == 3));
}

