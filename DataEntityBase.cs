using System;

public class DataEntityBase
{
    public virtual string getPrimarykey() => 
        null;

    public virtual void printDebug()
    {
        Debug.Log("DataEntityBase:");
    }
}

