using System;
using UnityEngine;

public class EventSuperBossEntity : DataEntityBase
{
    public int bossColor;
    public long endedAt;
    public int eventId;
    public int iconId;
    public int id;
    public long maxHp;
    public long[] splitHp;
    public int[] splitQuestIds;
    public long startedAt;

    public int DamageProgressNum()
    {
        UserSuperBossEntity userSuperBossEntity = this.GetUserSuperBossEntity();
        if (userSuperBossEntity == null)
        {
            return 0;
        }
        int index = 0;
        while (index < this.splitHp.Length)
        {
            if (userSuperBossEntity.damage < this.splitHp[index])
            {
                return index;
            }
            index++;
        }
        return index;
    }

    public Color GetBossColor()
    {
        int num = (this.bossColor & 0xff0000) >> 0x10;
        int num2 = (this.bossColor & 0xff00) >> 8;
        int num3 = this.bossColor & 0xff;
        return new Color(((float) num) / 255f, ((float) num2) / 255f, ((float) num3) / 255f);
    }

    public override string getPrimarykey() => 
        (this.eventId.ToString() + ":" + this.id.ToString());

    public UserSuperBossEntity GetUserSuperBossEntity()
    {
        long[] args = new long[] { NetworkManager.UserId, (long) this.eventId, (long) this.id };
        UserSuperBossEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<UserSuperBossMaster>(DataNameKind.Kind.USER_SUPER_BOSS).getEntityFromId<UserSuperBossEntity>(args);
        if (entity == null)
        {
            entity = new UserSuperBossEntity {
                userId = NetworkManager.UserId,
                eventId = this.eventId,
                superBossId = this.id,
                damage = 0L
            };
        }
        return entity;
    }

    public bool IsCleard() => 
        (this.GetUserSuperBossEntity().damage >= this.maxHp);

    public bool IsEncounted() => 
        SingletonTemplate<clsQuestCheck>.Instance.IsQuestRelease(this.splitQuestIds[0], -1, CondType.Kind.NONE);
}

