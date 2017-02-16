using System;
using System.Runtime.InteropServices;

public class TreasureDvcEntity : DataEntityBase
{
    public int attackAttri;
    public int id;
    public int maxLv;
    public string name;
    public string rank;
    public string ruby;
    public int seqId;
    public string typeText;

    public bool getEffectExplanation(out string tdName, out string tdExplanation, out int maxLv, out int tdGuageCount, int lv)
    {
        TreasureDvcLvEntity entity = SingletonMonoBehaviour<DataManager>.getInstance().getMasterData(DataNameKind.Kind.TREASUREDEVICE_LEVEL).getEntityFromId<TreasureDvcLvEntity>(this.id, lv);
        maxLv = this.maxLv;
        if (lv > 0)
        {
            tdName = string.Format(LocalizationManager.Get("NP_NAME_LEVEL"), this.name, lv);
        }
        else
        {
            tdName = string.Format(LocalizationManager.Get("NP_NAME"), this.name);
        }
        if (entity != null)
        {
            tdExplanation = entity.getDetail(lv);
            tdGuageCount = entity.gaugeCount;
            return true;
        }
        tdExplanation = LocalizationManager.GetUnknownName();
        tdGuageCount = 0;
        return false;
    }

    public string getName() => 
        this.name;

    public override string getPrimarykey() => 
        (string.Empty + this.id);
}

