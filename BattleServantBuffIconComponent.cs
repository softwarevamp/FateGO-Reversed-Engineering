using System;
using UnityEngine;

public class BattleServantBuffIconComponent : MonoBehaviour
{
    public int buffId = -1;
    public UISprite iconSprite;
    public BattlePerformanceStatus targetPerf;

    public void OnClick()
    {
        if ((this.targetPerf != null) && (0 < this.buffId))
        {
            this.targetPerf.OpenBuffConf(this.buffId);
        }
    }

    public void setIcon(int buffId)
    {
        if (buffId == -1)
        {
            base.gameObject.SetActive(false);
        }
        else
        {
            this.buffId = buffId;
            base.gameObject.SetActive(true);
            AtlasManager.SetSBuffIcon(this.iconSprite, this.buffId);
        }
    }

    public void setImageId(int iconId)
    {
        if (iconId == 0)
        {
            base.gameObject.SetActive(false);
        }
        else
        {
            base.gameObject.SetActive(true);
            AtlasManager.SetSBuffIconByIconId(this.iconSprite, iconId);
        }
    }
}

