using System;
using UnityEngine;

public class TitleInfoEventRaidStatusComponent : TitleInfoEventInfoComponent
{
    private int currentDay;
    private EventRaidEntity eventRaidEntity;
    private static readonly string RAIDBOSS_ICON_SPNAME_PREFIX_BATTLE = "raid_boss_status_battle_";
    private static readonly string RAIDBOSS_ICON_SPNAME_PREFIX_LOSE = "raid_boss_status_lose_";
    private static readonly string RAIDBOSS_ICON_SPNAME_PREFIX_WIN = "raid_boss_status_win_";
    [SerializeField]
    private UISprite raidBossIconSp;

    private void Destroy()
    {
    }

    public override bool IsDispPossible() => 
        true;

    public void OnDestroy()
    {
        this.Destroy();
    }

    public void Setup(EventRaidEntity eventRaidEntity, int currentDay)
    {
        this.Destroy();
        this.eventRaidEntity = eventRaidEntity;
        this.currentDay = currentDay;
        GameObject gameObject = base.gameObject;
        gameObject.name = gameObject.name + eventRaidEntity.day.ToString();
        this.UpdateDisp();
    }

    public override void UpdateDisp()
    {
        if (this.eventRaidEntity != null)
        {
            int eventId = this.eventRaidEntity.eventId;
            int day = this.eventRaidEntity.day;
            base.gameObject.SetActive(day <= this.currentDay);
            if (base.gameObject.activeSelf)
            {
                TotalEventRaidEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TotalEventRaidMaster>(DataNameKind.Kind.TOTAL_EVENT_RAID).getEntityFromId<TotalEventRaidEntity>(eventId, day);
                string str = string.Empty;
                if (day == this.currentDay)
                {
                    int raidDeadQuestId = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<EventRaidMaster>(DataNameKind.Kind.EVENT_RAID).GetRaidDeadQuestId(eventId, day);
                    if (!SingletonTemplate<clsQuestCheck>.Instance.IsEncountRaidBoss(eventId, day))
                    {
                        base.gameObject.SetActive(false);
                        return;
                    }
                    if (!SingletonTemplate<clsQuestCheck>.Instance.IsQuestClear(raidDeadQuestId, false))
                    {
                        str = RAIDBOSS_ICON_SPNAME_PREFIX_BATTLE;
                    }
                }
                if (string.IsNullOrEmpty(str))
                {
                    str = (this.eventRaidEntity.maxHp > entity.totalDamage) ? RAIDBOSS_ICON_SPNAME_PREFIX_LOSE : RAIDBOSS_ICON_SPNAME_PREFIX_WIN;
                }
                string bannerName = str + this.eventRaidEntity.iconId.ToString();
                this.raidBossIconSp.enabled = AtlasManager.SetBanner(this.raidBossIconSp, bannerName);
                this.raidBossIconSp.MakePixelPerfect();
            }
        }
    }
}

