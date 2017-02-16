using System;

public class BattleSpeedButtonComponent : BaseMonoBehaviour
{
    public BattlePerformance perf;

    private void OnPress(bool isPressed)
    {
        if (isPressed)
        {
            this.perf.setBattleSpeed(2f);
        }
        else
        {
            this.perf.setBattleSpeed(1f);
        }
    }
}

