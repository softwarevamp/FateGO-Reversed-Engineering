using System;

public class AiActEntity : DataEntityBase
{
    public int id;
    public int[] skillVals;
    public int target;
    public int[] targetIndividuality;
    public int type;

    public AiAct.TARGET getActTarget() => 
        ((AiAct.TARGET) this.target);

    public AiAct.TYPE getActType() => 
        ((AiAct.TYPE) this.type);

    public override string getPrimarykey() => 
        (string.Empty + this.id);

    public bool isThinkEnd() => 
        AiAct.isThinkEnd(this.type);
}

