using System;

public static class Individuality
{
    public static bool CheckIndividualities(int[] self, int[] target)
    {
        if ((target == null) || (target.Length <= 0))
        {
            return true;
        }
        if (self == null)
        {
            return true;
        }
        if (self.Length > 0)
        {
            foreach (int num in self)
            {
                foreach (int num3 in target)
                {
                    if (num == num3)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public enum TYPE
    {
        NONE
    }
}

