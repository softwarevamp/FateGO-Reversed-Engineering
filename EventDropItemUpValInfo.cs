using System;

public class EventDropItemUpValInfo
{
    public int addCount;
    public int baseFuncId;
    public FuncList.TYPE baseFuncType;
    public FunctionEntity funcEntity;
    public FunctionGroupEntity funcGroupEntity;
    public bool isEquipUp;
    public ItemEntity itemEntity;
    public int member;
    public int priority;
    public int rateCount;
    public Target.TYPE targetType;

    public EventDropItemUpValInfo(FunctionEntity funcEntity, ItemEntity itemEntity)
    {
        this.member = 0;
        this.funcEntity = funcEntity;
        this.baseFuncId = this.funcEntity.id;
        this.baseFuncType = (FuncList.TYPE) this.funcEntity.funcType;
        this.targetType = Target.TYPE.SELF;
        this.itemEntity = itemEntity;
        FunctionGroupMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionGroupMaster>(DataNameKind.Kind.FUNCTION_GROUP);
        this.funcGroupEntity = master.getEntityFromId(this.funcEntity.id);
        if (this.funcGroupEntity != null)
        {
            if (this.funcGroupEntity.baseFuncId > 0)
            {
                this.baseFuncId = this.funcGroupEntity.baseFuncId;
                this.funcGroupEntity = master.getEntityFromId<FunctionGroupEntity>(this.baseFuncId);
            }
            this.priority = this.funcGroupEntity.priority;
        }
        this.isEquipUp = false;
        this.addCount = 0;
        this.rateCount = 0;
    }

    public EventDropItemUpValInfo(int member, EventDropUpValInfo dropInfo)
    {
        this.member = member;
        this.funcEntity = dropInfo.funcEntity;
        this.baseFuncId = this.funcEntity.id;
        this.baseFuncType = (FuncList.TYPE) this.funcEntity.funcType;
        this.targetType = (Target.TYPE) this.funcEntity.targetType;
        FunctionGroupMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionGroupMaster>(DataNameKind.Kind.FUNCTION_GROUP);
        this.funcGroupEntity = master.getEntityFromId(this.funcEntity.id);
        if (this.funcGroupEntity != null)
        {
            if (this.funcGroupEntity.baseFuncId > 0)
            {
                this.baseFuncId = this.funcGroupEntity.baseFuncId;
                this.funcGroupEntity = master.getEntityFromId<FunctionGroupEntity>(this.baseFuncId);
            }
            this.priority = this.funcGroupEntity.priority;
        }
        this.isEquipUp = dropInfo.isEquipUp;
        this.addCount = dropInfo.addCount;
        this.rateCount = dropInfo.rateCount;
    }

    public EventDropItemUpValInfo(int member, EventDropUpValInfo dropInfo, ItemEntity itemEntity)
    {
        this.member = member;
        this.funcEntity = dropInfo.funcEntity;
        this.baseFuncId = this.funcEntity.id;
        this.baseFuncType = (FuncList.TYPE) this.funcEntity.funcType;
        this.targetType = Target.TYPE.SELF;
        this.itemEntity = itemEntity;
        FunctionGroupMaster master = SingletonMonoBehaviour<DataManager>.Instance.getMasterData<FunctionGroupMaster>(DataNameKind.Kind.FUNCTION_GROUP);
        this.funcGroupEntity = master.getEntityFromId(this.funcEntity.id);
        if (this.funcGroupEntity != null)
        {
            if (this.funcGroupEntity.baseFuncId > 0)
            {
                this.baseFuncId = this.funcGroupEntity.baseFuncId;
                this.funcGroupEntity = master.getEntityFromId<FunctionGroupEntity>(this.baseFuncId);
            }
            this.priority = this.funcGroupEntity.priority;
        }
        this.isEquipUp = dropInfo.isEquipUp;
        this.addCount = dropInfo.addCount;
        this.rateCount = dropInfo.rateCount;
    }
}

