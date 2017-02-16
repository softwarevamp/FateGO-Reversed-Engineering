using System;

public class BattleTDConfWIndowComponent : BattleWindowComponent
{
    public UILabel confLabel;
    public UILabel lvLabel;
    public UILabel maxParLbal;
    public UILabel nameLabel;
    public UILabel rubyLabel;

    public void setData(int tdId, int lv)
    {
        TreasureDvcEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcMaster>(DataNameKind.Kind.TREASUREDEVICE).getEntityFromId<TreasureDvcEntity>(tdId);
        TreasureDvcLvEntity entity2 = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<TreasureDvcLvMaster>(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(tdId, lv);
        this.nameLabel.text = entity.name;
        this.rubyLabel.text = entity.ruby;
        this.lvLabel.text = string.Empty + lv;
        this.maxParLbal.text = $"MAX {entity2.gaugeCount * 100}%";
        this.confLabel.text = entity2.getDetalShort(lv);
    }
}

