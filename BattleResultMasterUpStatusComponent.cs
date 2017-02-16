using System;
using UnityEngine;

public class BattleResultMasterUpStatusComponent : MonoBehaviour
{
    public UILabel newParamLabel;
    public UILabel oldParamLabel;
    public UILabel titleParamLabel;

    public void setData(int oldVal, int newVal)
    {
        if (this.oldParamLabel != null)
        {
            this.oldParamLabel.text = string.Empty + oldVal;
        }
        if (this.newParamLabel != null)
        {
            this.newParamLabel.text = string.Empty + newVal;
        }
    }

    public void setTitle(string key)
    {
        if (this.titleParamLabel != null)
        {
            string str = LocalizationManager.Get(key);
            if (!str.Equals(key))
            {
                this.titleParamLabel.text = str;
            }
        }
    }
}

