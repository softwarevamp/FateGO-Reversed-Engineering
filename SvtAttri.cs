using System;

public class SvtAttri
{
    public static float getMagnification(int attack, int defense) => 
        AttriRelationMaster.getRate(attack, defense);

    public enum TYPE
    {
        BEAST = 5,
        GROUND = 3,
        HUMAN = 1,
        SKY = 2,
        STAR = 4,
        VOID = 10
    }
}

