using System;

public class CombineEnableData
{
    public int limitUpEnableNum;
    public int npUpEnableNum;
    public int skillUpEnableNum;

    public int sumEnableNum() => 
        ((this.limitUpEnableNum + this.skillUpEnableNum) + this.npUpEnableNum);
}

