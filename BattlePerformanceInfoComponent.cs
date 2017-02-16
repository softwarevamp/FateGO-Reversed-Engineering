using System;
using UnityEngine;

public class BattlePerformanceInfoComponent : MonoBehaviour
{
    public float AdjustY = 24f;
    private int eventId;
    private bool isUpdateTimer;
    private long lastCheckTime;
    public UILabel nokoriTimeLabel;
    public GameObject nokoriTimeObject;
    public UILabel nokoriTurnLabel;
    public GameObject nokoriTurnObject;
    private long remaingTimeSeconds;
    private Vector3 timeDefPos;

    private void drawTime(long showTimeSeconds)
    {
        if (0L < showTimeSeconds)
        {
            TimeSpan span = new TimeSpan(showTimeSeconds * 0x989680L);
            int totalHours = (int) span.TotalHours;
            this.nokoriTimeLabel.text = string.Format(LocalizationManager.Get("BATTLE_RAID_REMAINING_TIME"), totalHours, span.Minutes, span.Seconds);
            if (totalHours <= 0)
            {
                this.nokoriTimeLabel.color = Color.red;
            }
        }
        else
        {
            this.nokoriTimeLabel.text = string.Format(LocalizationManager.Get("BATTLE_RAID_REMAINING_TIME"), 0, 0, 0);
            this.nokoriTimeLabel.color = Color.red;
        }
    }

    public void Initialize()
    {
        if (this.nokoriTimeObject != null)
        {
            this.nokoriTimeObject.SetActive(false);
            this.timeDefPos = new Vector3(this.nokoriTimeObject.transform.localPosition.x, this.nokoriTimeObject.transform.localPosition.y, this.nokoriTimeObject.transform.localPosition.z);
        }
        if (this.nokoriTurnObject != null)
        {
            this.nokoriTurnObject.SetActive(false);
        }
    }

    public void setQuest(BattleData data)
    {
    }

    public void setShowElapsedTurn(int state, int now)
    {
        if (this.nokoriTurnObject != null)
        {
            this.nokoriTurnObject.SetActive(true);
            if (now <= 0)
            {
                now = 1;
            }
            if (state == 1)
            {
                this.nokoriTurnLabel.text = string.Format(LocalizationManager.Get("BATTLE_ELAPSED_TURN"), now);
            }
            else if (state == 2)
            {
                this.nokoriTurnLabel.text = string.Format(LocalizationManager.Get("BATTLE_TOTALELAPSED_TURN"), now);
            }
        }
    }

    public void setShowLimitTurn(int now, int limit)
    {
        if (this.nokoriTurnObject != null)
        {
            this.nokoriTurnObject.SetActive(true);
            if (now <= 0)
            {
                now = 1;
            }
            int num = (limit - now) + 1;
            if (num <= 0)
            {
                num = 0;
            }
        }
    }

    public void setShowTurn(BattleData bdata, int addturn)
    {
        if (bdata.isLimitTurn())
        {
            this.setShowLimitTurn(bdata.turnCount + addturn, bdata.limitTurnCount);
        }
        else if (bdata.isShowTurn())
        {
            this.setShowElapsedTurn(bdata.stateshowturn, bdata.totalTurnCount + bdata.turnCount);
        }
        else
        {
            this.nokoriTurnObject.SetActive(false);
        }
    }

    private void Update()
    {
    }

    private void updateTime()
    {
        TimeSpan span = new TimeSpan(DateTime.Now.Ticks - this.lastCheckTime);
        int totalSeconds = (int) span.TotalSeconds;
        if ((totalSeconds > 0) && (this.remaingTimeSeconds > 0L))
        {
            this.lastCheckTime = DateTime.Now.Ticks;
            this.remaingTimeSeconds -= totalSeconds;
            this.drawTime(this.remaingTimeSeconds);
        }
    }
}

