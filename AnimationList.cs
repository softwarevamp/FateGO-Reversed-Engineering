using System;
using System.Collections.Generic;
using System.Linq;

public class AnimationList
{
    private static int Count;
    private static List<string> Values;

    public static int getIndex(string inname)
    {
        if (Values == null)
        {
            Values = Enum.GetNames(typeof(NAME)).Cast<string>().ToList<string>();
            Count = Values.Count;
        }
        for (int i = 0; i < Count; i++)
        {
            if (Values[i].Equals(inname))
            {
                return i;
            }
        }
        return -1;
    }

    public enum NAME
    {
        attack_a,
        attack_b,
        attack_q,
        attack_gen,
        damage_01,
        spell,
        step_back,
        step_front,
        treasure_arms,
        wait,
        death_01,
        attack_ex,
        attack_a02,
        attack_b02,
        attack_q02,
        attack_gen02,
        attack_ex02,
        attack_a03,
        attack_b03,
        attack_q03,
        attack_gen03,
        attack_ex03,
        spell02,
        spell03,
        max
    }
}

