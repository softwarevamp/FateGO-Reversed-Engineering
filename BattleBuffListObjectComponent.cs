using System;
using UnityEngine;

public class BattleBuffListObjectComponent : MonoBehaviour
{
    public UILabel atCountLabel;
    public UILabel atTurnLabel;
    public BattleServantBuffIconComponent buffIcon;
    public UILabel confLabel;
    public UILabel nameLabel;

    public void setData(BattleBuffData.BuffData buffData)
    {
        BuffEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF).getEntityFromId<BuffEntity>(buffData.buffId);
        this.buffIcon.setIcon(buffData.buffId);
        this.nameLabel.text = entity.name;
        this.confLabel.text = entity.detail;
        this.atTurnLabel.text = string.Empty;
        this.atCountLabel.text = string.Empty;
        if (0 < buffData.turn)
        {
            this.atTurnLabel.text = $"{Mathf.FloorToInt((float) ((buffData.turn + 1) / 2))} 回合";
        }
        if (0 < buffData.count)
        {
            this.atCountLabel.text = $"次数 {buffData.count}次";
        }
    }
}

