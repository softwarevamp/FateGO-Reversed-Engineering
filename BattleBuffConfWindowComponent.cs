using System;

public class BattleBuffConfWindowComponent : BattleWindowComponent
{
    public BattleServantBuffIconComponent buffIcon;
    private int buffId = -1;
    public UILabel detailLabel;
    public UILabel nameLabel;

    public bool checkBuffId(int buffId) => 
        (this.buffId == buffId);

    public override void Close(BattleWindowComponent.EndCall call)
    {
        this.buffId = -1;
        base.Close(call);
    }

    public override void Open(BattleWindowComponent.EndCall call)
    {
        base.Open(call);
    }

    public void setData(int buffId)
    {
        BuffEntity entity = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<BuffMaster>(DataNameKind.Kind.BUFF).getEntityFromId<BuffEntity>(buffId);
        this.buffId = buffId;
        this.buffIcon.setIcon(entity.id);
        this.nameLabel.text = entity.name;
        this.detailLabel.text = entity.detail;
    }
}

