using System;
using UnityEngine;

[ExecuteInEditMode]
public class UIRootReScale : MonoBehaviour
{
    public CommonUI commonUi;

    public void ReScale()
    {
        int wIDTH = ManagerConfig.WIDTH;
        int hEIGHT = ManagerConfig.HEIGHT;
        if ((wIDTH > 0) && (hEIGHT > 0))
        {
            UIRoot component = base.GetComponent<UIRoot>();
            if (component != null)
            {
                float num3 = ((float) (Screen.height * wIDTH)) / ((float) (Screen.width * hEIGHT));
                int num4 = (num3 <= 1f) ? hEIGHT : ((int) (hEIGHT * num3));
                int num5 = (num3 <= 1f) ? ((int) (((float) wIDTH) / num3)) : wIDTH;
                if (component.manualHeight != num4)
                {
                    component.manualHeight = num4;
                }
                if (this.commonUi != null)
                {
                    float num6 = Mathf.Ceil(Mathf.Abs((int) (num4 - hEIGHT)) * 0.5f);
                    float num7 = Mathf.Ceil(Mathf.Abs((int) (wIDTH - num5)) * 0.5f);
                    SingletonMonoBehaviour<CommonUI>.Instance.setObiImgSize((int) num6, (int) num7);
                }
            }
        }
    }

    protected void Start()
    {
        this.ReScale();
    }
}

