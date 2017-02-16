using System;
using UnityEngine;

public class BattleCommandParamComponent : BaseMonoBehaviour
{
    private GameObject numberobj;
    public Transform NumberTr;
    private GameObject[] yakuobj;
    public Transform YakuTr;

    public void resetComboGuid()
    {
        if (this.yakuobj != null)
        {
            for (int i = 0; i < this.yakuobj.Length; i++)
            {
                UnityEngine.Object.Destroy(this.yakuobj[i]);
            }
        }
    }

    public void resetNo()
    {
        if (this.numberobj != null)
        {
            UnityEngine.Object.Destroy(this.numberobj);
        }
    }

    public void setComboGuid(int index, BattleComboData combo)
    {
        string resouceurl = null;
        this.resetComboGuid();
        if (combo.flash)
        {
            if (combo.sameflg[2])
            {
                if (combo.samecount == 2)
                {
                    resouceurl = "effect/ef_cardcombo_u2";
                }
                else if (combo.samecount == 3)
                {
                    resouceurl = "effect/ef_cardcombo_u3";
                }
            }
            else
            {
                resouceurl = "effect/ef_cardcombo_ua";
            }
        }
        else if (combo.sameflg[index])
        {
            if (combo.samecount == 2)
            {
                resouceurl = "effect/ef_cardcombo_2a";
            }
            else if (combo.samecount == 3)
            {
                resouceurl = "effect/ef_cardcombo_3a";
            }
        }
        if (resouceurl != null)
        {
            this.yakuobj[0] = base.createObject(resouceurl, this.YakuTr, null);
        }
    }

    public void setInit()
    {
        if (this.numberobj != null)
        {
            UnityEngine.Object.Destroy(this.numberobj);
        }
        if (this.yakuobj != null)
        {
            for (int i = 0; i < this.yakuobj.Length; i++)
            {
                UnityEngine.Object.Destroy(this.yakuobj[i]);
            }
        }
        this.yakuobj = new GameObject[2];
    }

    public void setNo(int count)
    {
        string resouceurl = $"effect/ef_command_{count:00}";
        this.numberobj = base.createObject(resouceurl, this.NumberTr, null);
    }
}

