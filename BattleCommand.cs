using System;

public static class BattleCommand
{
    public static float getCriticalMagnification(BattleCommandData data) => 
        getCriticalMagnification(data.getCommandType(), data.ActionIndex);

    public static float getCriticalMagnification(int type, int index) => 
        CardMaster.getCritical(type, index);

    public static int[] getIndividuality(int type, int num) => 
        CardMaster.getIndividualities(type, num);

    public static float getMagnification(BattleCommandData data) => 
        getMagnification(data.getCommandType(), data.ActionIndex);

    public static float getMagnification(int type, int index) => 
        CardMaster.getAtk(type, index);

    public static float getNpMagnification(BattleCommandData command) => 
        getNpMagnification(command.getCommandType(), command.ActionIndex);

    public static float getNpMagnification(int type, int index) => 
        CardMaster.getTdGauge(type, index);

    public static TYPE getType(int type) => 
        ((TYPE) type);

    public static bool isADDATTACK(int type) => 
        (4 == type);

    public static bool isARTS(int type) => 
        (1 == type);

    public static bool isBLANK(int type) => 
        (5 == type);

    public static bool isBUSTER(int type) => 
        (2 == type);

    public static bool isNomalCommand(int type) => 
        (((type == 1) || (type == 3)) || (2 == type));

    public static bool isQUICK(int type) => 
        (3 == type);

    public static bool isShowCommandAction(int type) => 
        (((((type == 1) || (type == 3)) || ((type == 2) || (type == 4))) || (type == 10)) || (11 == type));

    public enum TYPE
    {
        ADDATTACK = 4,
        ARTS = 1,
        BLANK = 5,
        BUSTER = 2,
        NONE = 0,
        QUICK = 3,
        STRENGTH = 11,
        WEAK = 10
    }
}

