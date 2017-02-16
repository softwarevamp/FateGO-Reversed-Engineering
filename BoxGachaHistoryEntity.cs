using System;

public class BoxGachaHistoryEntity : DataEntityBase
{
    public int boxGachaId;
    public int[] numbers;

    public int getDrawNum(int no)
    {
        int num = 0;
        if (this.numbers.Length > 0)
        {
            for (int i = 0; i < this.numbers.Length; i++)
            {
                if (this.numbers[i].Equals(no))
                {
                    num++;
                }
            }
        }
        return num;
    }

    public override string getPrimarykey() => 
        (string.Empty + this.boxGachaId);
}

