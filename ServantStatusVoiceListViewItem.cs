using System;

public class ServantStatusVoiceListViewItem : ListViewItem
{
    protected int firstPriority;
    protected bool isCanPlay;
    protected bool isPlay;
    protected int limitCount;
    protected string overwriteName;
    protected int svtId;
    protected VoiceEntity voiceEntitiy;

    public ServantStatusVoiceListViewItem(int index, bool isCanPlay, int svtId, int limitCount, VoiceEntity voiceEntitiy, string overwriteName, int firstPriority) : base(index)
    {
        this.isCanPlay = isCanPlay;
        this.svtId = svtId;
        this.limitCount = limitCount;
        this.voiceEntitiy = voiceEntitiy;
        this.overwriteName = overwriteName;
        this.firstPriority = firstPriority;
    }

    ~ServantStatusVoiceListViewItem()
    {
    }

    public void SetLimitCount(bool isCanPlay, int svtId, int limitCount, VoiceEntity voiceEntitiy, string overwriteName, int firstPriority)
    {
        this.isCanPlay = isCanPlay;
        this.svtId = svtId;
        this.limitCount = limitCount;
        this.voiceEntitiy = voiceEntitiy;
        this.overwriteName = overwriteName;
    }

    public void SetPLay(bool isPlay)
    {
        this.isPlay = isPlay;
    }

    public override bool SetSortValue(ListViewSort sort)
    {
        base.isTermination = false;
        base.isTerminationSpace = false;
        base.sortValue0 = -this.firstPriority;
        base.sortValue1 = -this.voiceEntitiy.priority;
        return true;
    }

    public CondType.Kind CondType =>
        ((CondType.Kind) this.voiceEntitiy.condType);

    public int CondValue =>
        this.voiceEntitiy.condValue;

    public bool IsCanPlay =>
        this.isCanPlay;

    public bool IsPlay =>
        this.isPlay;

    public string LabelName =>
        this.voiceEntitiy.id;

    public int LimitCount =>
        this.limitCount;

    public string Name
    {
        get
        {
            if (!this.isCanPlay && !string.IsNullOrEmpty(this.voiceEntitiy.nameDefault))
            {
                return this.voiceEntitiy.nameDefault;
            }
            if (string.IsNullOrEmpty(this.overwriteName))
            {
                return this.voiceEntitiy.name;
            }
            return this.overwriteName;
        }
    }

    public SvtVoiceType.Type PlayType =>
        ((SvtVoiceType.Type) this.voiceEntitiy.svtVoiceType);

    public int SvtId =>
        this.svtId;
}

