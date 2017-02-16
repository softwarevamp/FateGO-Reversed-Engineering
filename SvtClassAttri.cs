using System;

public class SvtClassAttri
{
    public static float getMagnification(int attack, int defense) => 
        ClassRelationMaster.getRate(attack, defense);

    public enum TYPE
    {
        ALTEREGO = 10,
        ARCHER = 2,
        ASSASIN = 6,
        AVENGER = 11,
        BEAST = 12,
        BERSERKER = 7,
        CASTER = 5,
        LANCER = 3,
        RIDER = 4,
        RULER = 9,
        SABER = 1,
        SHIELDER = 8
    }
}

